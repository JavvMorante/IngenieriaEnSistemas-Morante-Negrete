using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Data.SqlClient;
using Seguridad_60MN.Configuracion;
using Seguridad_60MN.Criptografia;
using Seguridad_60MN.Integridad;

namespace Seguridad_60MN.Auditoria
{
    // =========================================================================
    // Modelo del archivo XML (lo que serializa XmlSerializer)
    // =========================================================================

    /// <summary>
    /// Raíz del archivo de intercambio de la bitácora. La descripción de cada evento
    /// viaja CIFRADA con AES (igual que en la base) y el archivo va firmado con
    /// HMAC-SHA256: cada evento tiene su firma (como un DVH) y el archivo tiene una
    /// firma general que encadena las de los eventos (como un DVV).
    /// </summary>
    [XmlRoot("BitacoraVetCare")]
    public sealed class ArchivoBitacora_60MN
    {
        public const string VersionActual = "1.0";

        [XmlAttribute("version")]
        public string Version { get; set; } = VersionActual;

        [XmlAttribute("exportado")]
        public DateTime Exportado { get; set; }

        [XmlAttribute("exportadoPor")]
        public string ExportadoPor { get; set; } = string.Empty;

        /// <summary>Siempre true: el sistema nunca exporta la descripción en texto plano.</summary>
        [XmlAttribute("cifrado")]
        public bool Cifrado { get; set; } = true;

        [XmlAttribute("cantidad")]
        public int Cantidad { get; set; }

        [XmlArray("Eventos"), XmlArrayItem("Evento")]
        public List<EventoBitacoraXml_60MN> Eventos { get; set; } = new List<EventoBitacoraXml_60MN>();

        /// <summary>Firma HMAC-SHA256 de la cabecera y de todas las firmas de los eventos.</summary>
        [XmlElement("Firma")]
        public string Firma { get; set; } = string.Empty;
    }

    /// <summary>Un evento de la bitácora dentro del XML.</summary>
    public sealed class EventoBitacoraXml_60MN
    {
        /// <summary>BitacoraID en la base de origen. Solo informativo: al importar se asigna un Id nuevo.</summary>
        [XmlAttribute("idOriginal")]
        public int IdOriginal { get; set; }

        public DateTime FechaHora { get; set; }

        [XmlElement(IsNullable = true)]
        public int? UsuarioId { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Modulo { get; set; } = string.Empty;

        public string Evento { get; set; } = string.Empty;

        public int Criticidad { get; set; }

        /// <summary>Descripción cifrada con AES (formato "iv.datos" en Base64), tal cual sale de la base.</summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>Firma HMAC-SHA256 del evento: detecta cualquier cambio en sus campos.</summary>
        public string Firma { get; set; } = string.Empty;
    }

    // =========================================================================
    // Resultado del análisis de un archivo (para la vista previa en la GUI)
    // =========================================================================

    /// <summary>Lo que se sabe de un archivo XML de bitácora después de leerlo y validarlo.</summary>
    public sealed class ResumenArchivoBitacora_60MN
    {
        public string Ruta { get; set; } = string.Empty;

        public string NombreArchivo => Path.GetFileName(Ruta);

        /// <summary>SHA-256 del archivo completo: identifica exactamente qué archivo se exportó o importó.</summary>
        public string HashSha256 { get; set; } = string.Empty;

        /// <summary>Contenido del archivo, para mostrar el XML en pantalla.</summary>
        public string Xml { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public DateTime? Exportado { get; set; }

        public string ExportadoPor { get; set; } = string.Empty;

        public bool Cifrado { get; set; }

        public int Cantidad { get; set; }

        public DateTime? Desde { get; set; }

        public DateTime? Hasta { get; set; }

        /// <summary>Firma general del archivo correcta.</summary>
        public bool FirmaValida { get; set; }

        /// <summary>Eventos que todavía no están en la bitácora (los que se importarían).</summary>
        public int Nuevos { get; set; }

        /// <summary>Eventos que ya existen en la bitácora (se omiten al importar).</summary>
        public int Duplicados { get; set; }

        /// <summary>Eventos del archivo; la descripción se completa desencriptada solo si se pide.</summary>
        public List<RegistroBitacora_60MN> Eventos { get; set; } = new List<RegistroBitacora_60MN>();

        /// <summary>Motivos de rechazo. Vacío = el archivo es válido y se puede importar.</summary>
        public List<string> Errores { get; set; } = new List<string>();

        public bool Valido => Errores.Count == 0;

        internal ArchivoBitacora_60MN Archivo { get; set; }
    }

    /// <summary>Resultado de una importación.</summary>
    public sealed class ResultadoImportacionBitacora_60MN
    {
        public int Importados { get; set; }

        public int Duplicados { get; set; }
    }

    // =========================================================================
    // Serialización, validación e importación
    // =========================================================================

    /// <summary>
    /// Exportación e importación de la Bitácora de Eventos en XML.
    ///
    /// Medidas de seguridad:
    ///  1. Confidencialidad: la descripción se exporta cifrada (AES-256), nunca en claro.
    ///  2. Integridad y autenticidad: firma HMAC-SHA256 por evento y por archivo con la
    ///     clave de los dígitos verificadores (VETCARE_CLAVE_DV). Un archivo editado a
    ///     mano, o generado por otro sistema, se rechaza completo.
    ///  3. Lectura segura del XML: sin DTD ni entidades externas (evita XXE y "billion
    ///     laughs"), con tamaño máximo de archivo y de eventos.
    ///  4. Validación de contenido: módulo, evento y criticidad deben coincidir con el
    ///     catálogo; fechas no futuras; longitudes dentro de las columnas.
    ///  5. Importación todo-o-nada en una transacción, sin duplicar eventos que ya
    ///     existen, y con DVH/DVV recalculados por el mismo camino que Registrar Evento.
    ///  6. Solo se exporta o importa si la tabla Bitacora está íntegra (DVH/DVV), para no
    ///     "firmar" ni mezclar datos sobre una bitácora adulterada.
    /// </summary>
    public sealed class SerializadorBitacora_60MN
    {
        /// <summary>Tamaño máximo del archivo a importar.</summary>
        public const long TamanioMaximoBytes = 20L * 1024 * 1024;

        /// <summary>Cantidad máxima de eventos por archivo.</summary>
        public const int EventosMaximos = 20000;

        // Nombres de "tabla" de la firma: separan estas firmas de los DVH reales de la base.
        private const string TablaFirmaEvento = "BitacoraXML.Evento";
        private const string TablaFirmaEventos = "BitacoraXML.Eventos";
        private const string TablaFirmaArchivo = "BitacoraXML.Archivo";

        private const int MaximoErroresInformados = 30;

        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly GestorBitacora_60MN gestor = new GestorBitacora_60MN();

        /// <summary>True si la tabla Bitacora no tiene inconsistencias de DVH/DVV.</summary>
        public bool BitacoraIntegra() => dv.TablaConsistente(TablaDV_60MN.Bitacora);

        // ------------------------------- Exportar -------------------------------

        /// <summary>
        /// Arma el archivo firmado con los eventos del filtro y lo guarda en la ruta.
        /// Escribe primero un archivo temporal y lo renombra al final, para no dejar
        /// nunca un XML a medio escribir. Devuelve el resumen del archivo generado.
        /// </summary>
        public ResumenArchivoBitacora_60MN Exportar(FiltroBitacora_60MN filtro, string exportadoPor, string ruta)
        {
            List<RegistroBitacora_60MN> filas = gestor.ConsultarCifrado(filtro);
            if (filas.Count > EventosMaximos)
                throw new InvalidOperationException(
                    $"La exportación tiene {filas.Count} eventos y el máximo por archivo es {EventosMaximos}. Acote el rango de fechas.");

            ArchivoBitacora_60MN archivo = new ArchivoBitacora_60MN
            {
                Exportado = SinFraccion(DateTime.Now),
                ExportadoPor = exportadoPor ?? string.Empty,
                Cantidad = filas.Count,
                Eventos = filas.Select(f => new EventoBitacoraXml_60MN
                {
                    IdOriginal = f.Id,
                    FechaHora = SinFraccion(f.FechaHora),
                    UsuarioId = f.UsuarioId,
                    Usuario = f.Usuario,
                    Modulo = f.Modulo,
                    Evento = f.Evento,
                    Criticidad = f.Criticidad,
                    Descripcion = f.Descripcion
                }).ToList()
            };
            foreach (EventoBitacoraXml_60MN evento in archivo.Eventos)
                evento.Firma = FirmarEvento(evento);
            archivo.Firma = FirmarArchivo(archivo);

            string carpeta = Path.GetDirectoryName(Path.GetFullPath(ruta)) ?? ".";
            string temporal = Path.Combine(carpeta, "." + Path.GetFileName(ruta) + "." + Guid.NewGuid().ToString("N") + ".tmp");
            try
            {
                XmlWriterSettings opciones = new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) };
                using (FileStream flujo = new FileStream(temporal, FileMode.CreateNew, FileAccess.Write))
                using (XmlWriter escritor = XmlWriter.Create(flujo, opciones))
                    new XmlSerializer(typeof(ArchivoBitacora_60MN)).Serialize(escritor, archivo);
                File.Move(temporal, ruta, overwrite: true);
            }
            finally
            {
                if (File.Exists(temporal)) File.Delete(temporal);
            }

            // Se relee el archivo escrito: el resumen (y su hash) corresponden exactamente a lo que quedó en disco.
            return Analizar(ruta, desencriptarDescripciones: false);
        }

        // --------------------------- Leer y validar ---------------------------

        /// <summary>
        /// Lee el archivo de forma segura, verifica firmas y contenido, y cuenta cuántos
        /// eventos son nuevos y cuántos ya existen. No modifica la base. Si el archivo no
        /// se puede leer, el resumen vuelve con el motivo en Errores (no lanza excepción).
        /// </summary>
        public ResumenArchivoBitacora_60MN Analizar(string ruta, bool desencriptarDescripciones)
        {
            ResumenArchivoBitacora_60MN resumen = new ResumenArchivoBitacora_60MN { Ruta = ruta };

            byte[] contenido;
            try
            {
                FileInfo info = new FileInfo(ruta);
                if (!info.Exists)
                {
                    resumen.Errores.Add("El archivo no existe.");
                    return resumen;
                }
                if (info.Length == 0 || info.Length > TamanioMaximoBytes)
                {
                    resumen.Errores.Add($"El archivo está vacío o supera el tamaño máximo ({TamanioMaximoBytes / 1024 / 1024} MB).");
                    return resumen;
                }
                contenido = File.ReadAllBytes(ruta);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                resumen.Errores.Add("No se pudo leer el archivo: " + ex.Message);
                return resumen;
            }

            resumen.HashSha256 = Convert.ToHexString(SHA256.HashData(contenido));
            resumen.Xml = new UTF8Encoding(false).GetString(contenido).TrimStart('﻿');

            ArchivoBitacora_60MN archivo = Deserializar(contenido, resumen.Errores);
            if (archivo == null)
                return resumen;

            resumen.Archivo = archivo;
            resumen.Version = archivo.Version ?? string.Empty;
            resumen.Exportado = archivo.Exportado;
            resumen.ExportadoPor = archivo.ExportadoPor ?? string.Empty;
            resumen.Cifrado = archivo.Cifrado;
            resumen.Cantidad = archivo.Eventos.Count;
            if (archivo.Eventos.Count > 0)
            {
                resumen.Desde = archivo.Eventos.Min(e => e.FechaHora);
                resumen.Hasta = archivo.Eventos.Max(e => e.FechaHora);
            }

            Validar(archivo, resumen);

            foreach (EventoBitacoraXml_60MN e in archivo.Eventos)
                resumen.Eventos.Add(new RegistroBitacora_60MN
                {
                    Id = e.IdOriginal,
                    FechaHora = e.FechaHora,
                    UsuarioId = e.UsuarioId,
                    Usuario = e.Usuario,
                    Modulo = e.Modulo,
                    Evento = e.Evento,
                    Criticidad = e.Criticidad,
                    Descripcion = desencriptarDescripciones && resumen.FirmaValida
                        ? GestorBitacora_60MN.DesencriptarSeguro(e.Descripcion)
                        : "(cifrada)"
                });

            if (resumen.Valido)
            {
                HashSet<string> existentes = ClavesExistentes(archivo.Eventos);
                resumen.Duplicados = archivo.Eventos.Count(e => existentes.Contains(Clave(e)));
                resumen.Nuevos = archivo.Eventos.Count - resumen.Duplicados;
            }
            return resumen;
        }

        /// <summary>XmlSerializer sobre un XmlReader endurecido: sin DTD, sin resolver entidades externas.</summary>
        private static ArchivoBitacora_60MN Deserializar(byte[] contenido, List<string> errores)
        {
            XmlReaderSettings opciones = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
                MaxCharactersFromEntities = 0,
                MaxCharactersInDocument = TamanioMaximoBytes,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true
            };
            try
            {
                using MemoryStream flujo = new MemoryStream(contenido, writable: false);
                using XmlReader lector = XmlReader.Create(flujo, opciones);
                XmlSerializer serializador = new XmlSerializer(typeof(ArchivoBitacora_60MN));
                if (!serializador.CanDeserialize(lector))
                {
                    errores.Add("El archivo no es un XML de bitácora de VetCare (falta el elemento raíz BitacoraVetCare).");
                    return null;
                }
                ArchivoBitacora_60MN archivo = serializador.Deserialize(lector) as ArchivoBitacora_60MN;
                if (archivo == null)
                    errores.Add("El archivo no contiene una bitácora.");
                return archivo;
            }
            catch (Exception ex) when (ex is XmlException || ex is InvalidOperationException)
            {
                // InvalidOperationException envuelve los errores de formato de XmlSerializer (incluida una DTD prohibida).
                errores.Add("El XML no tiene un formato válido: " + (ex.InnerException?.Message ?? ex.Message));
                return null;
            }
        }

        private void Validar(ArchivoBitacora_60MN archivo, ResumenArchivoBitacora_60MN resumen)
        {
            List<string> errores = resumen.Errores;
            void Error(string mensaje)
            {
                if (errores.Count < MaximoErroresInformados) errores.Add(mensaje);
                else if (errores.Count == MaximoErroresInformados) errores.Add("(hay más errores que no se muestran)");
            }

            archivo.Eventos ??= new List<EventoBitacoraXml_60MN>();
            DateTime limiteFuturo = DateTime.Now.AddMinutes(5);

            // Cabecera
            if (archivo.Version != ArchivoBitacora_60MN.VersionActual)
                Error($"Versión de archivo no soportada: \"{archivo.Version}\" (se espera {ArchivoBitacora_60MN.VersionActual}).");
            if (!archivo.Cifrado)
                Error("El archivo declara descripciones sin cifrar: VetCare solo acepta bitácoras con la descripción cifrada.");
            if (archivo.Eventos.Count > EventosMaximos)
                Error($"El archivo tiene {archivo.Eventos.Count} eventos y el máximo es {EventosMaximos}.");
            if (archivo.Cantidad != archivo.Eventos.Count)
                Error($"La cabecera declara {archivo.Cantidad} eventos pero el archivo contiene {archivo.Eventos.Count}.");
            if (archivo.Exportado > limiteFuturo)
                Error("La fecha de exportación es futura.");

            // Firma general del archivo (detecta eventos agregados, quitados, reordenados o cabecera alterada)
            resumen.FirmaValida = !string.IsNullOrEmpty(archivo.Firma) &&
                                  FirmasIguales(archivo.Firma, FirmarArchivo(archivo));
            if (!resumen.FirmaValida)
                Error("La firma del archivo no es válida: fue modificado después de exportarse o no lo generó este sistema.");

            // Cada evento
            HashSet<int> ids = new HashSet<int>();
            foreach (EventoBitacoraXml_60MN e in archivo.Eventos)
            {
                string ref_ = $"Evento {e.IdOriginal}";
                if (!ids.Add(e.IdOriginal))
                    Error($"{ref_}: el id original está repetido en el archivo.");
                if (string.IsNullOrEmpty(e.Firma) || !FirmasIguales(e.Firma, FirmarEvento(e)))
                    Error($"{ref_}: la firma no coincide (el evento fue alterado).");

                if (!Enum.TryParse(e.Evento, ignoreCase: false, out EventoSistema_60MN tipo) || !Enum.IsDefined(typeof(EventoSistema_60MN), tipo)
                    || tipo.ToString() != e.Evento)
                {
                    Error($"{ref_}: el evento \"{e.Evento}\" no existe en el catálogo.");
                }
                else
                {
                    DefinicionEvento_60MN definicion = CatalogoEventos_60MN.Obtener(tipo);
                    if (definicion.Modulo.ToString() != e.Modulo)
                        Error($"{ref_}: el módulo \"{e.Modulo}\" no corresponde al evento {e.Evento}.");
                    if (definicion.Criticidad != e.Criticidad)
                        Error($"{ref_}: la criticidad {e.Criticidad} no corresponde al evento {e.Evento}.");
                }

                if (e.FechaHora == default || e.FechaHora > limiteFuturo)
                    Error($"{ref_}: fecha y hora inválida o futura.");
                if (string.IsNullOrWhiteSpace(e.Usuario) || e.Usuario.Length > 50)
                    Error($"{ref_}: el usuario está vacío o supera los 50 caracteres.");
                if (string.IsNullOrEmpty(e.Descripcion) || e.Descripcion.Length > 1500)
                    Error($"{ref_}: la descripción está vacía o supera el tamaño de la columna.");
                else if (resumen.FirmaValida && !SePuedeDesencriptar(e.Descripcion))
                    Error($"{ref_}: la descripción no se puede desencriptar con la clave de este sistema.");
            }
        }

        // ------------------------------- Importar -------------------------------

        /// <summary>
        /// Importa los eventos nuevos de un archivo ya analizado y válido. Todo-o-nada:
        /// si falla un INSERT no queda ningún evento a medias. Después se calcula el DVH
        /// de cada fila nueva y el DVV de la tabla.
        /// </summary>
        public ResultadoImportacionBitacora_60MN Importar(ResumenArchivoBitacora_60MN resumen)
        {
            if (!resumen.Valido || resumen.Archivo == null)
                throw new InvalidOperationException("El archivo no es válido y no se puede importar.");

            ArchivoBitacora_60MN archivo = resumen.Archivo;
            HashSet<string> existentes = ClavesExistentes(archivo.Eventos);
            List<EventoBitacoraXml_60MN> nuevos = archivo.Eventos.Where(e => existentes.Add(Clave(e))).ToList();
            ResultadoImportacionBitacora_60MN resultado = new ResultadoImportacionBitacora_60MN
            {
                Duplicados = archivo.Eventos.Count - nuevos.Count
            };
            if (nuevos.Count == 0)
                return resultado;

            const string sql = @"
INSERT INTO Bitacora (UsuarioID, Usuario, FechaHora, Modulo, Evento, Criticidad, Descripcion)
VALUES (CASE WHEN EXISTS (SELECT 1 FROM Usuario WHERE UsuarioID = @usuarioId) THEN @usuarioId END,
        @usuario, @fecha, @modulo, @evento, @criticidad, @descripcion);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            List<int> idsNuevos = new List<int>();
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlTransaction transaccion = con.BeginTransaction())
            {
                try
                {
                    foreach (EventoBitacoraXml_60MN e in nuevos)
                    {
                        using SqlCommand com = new SqlCommand(sql, con, transaccion);
                        // UsuarioID: se conserva solo si ese usuario existe en esta base (si no, queda el nombre).
                        com.Parameters.Add("@usuarioId", System.Data.SqlDbType.Int).Value = (object)e.UsuarioId ?? DBNull.Value;
                        com.Parameters.AddWithValue("@usuario", e.Usuario);
                        com.Parameters.Add("@fecha", System.Data.SqlDbType.DateTime2).Value = e.FechaHora;
                        com.Parameters.AddWithValue("@modulo", e.Modulo);
                        com.Parameters.AddWithValue("@evento", e.Evento);
                        com.Parameters.AddWithValue("@criticidad", e.Criticidad);
                        // La descripción ya viene cifrada: se guarda tal cual, sin pasar nunca por texto plano.
                        com.Parameters.AddWithValue("@descripcion", e.Descripcion);
                        idsNuevos.Add((int)com.ExecuteScalar());
                    }
                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }

            // Igual que Registrar Evento: DVH de cada fila nueva y DVV de la tabla.
            foreach (int id in idsNuevos)
                dv.ActualizarFila(TablaDV_60MN.Bitacora, id);

            resultado.Importados = idsNuevos.Count;
            return resultado;
        }

        // ------------------------------- Firmas -------------------------------

        private string FirmarEvento(EventoBitacoraXml_60MN e) =>
            dv.CalcularDVH(TablaFirmaEvento, e.IdOriginal, e.FechaHora, e.UsuarioId, e.Usuario, e.Modulo, e.Evento, e.Criticidad, e.Descripcion);

        private string FirmarArchivo(ArchivoBitacora_60MN a)
        {
            string firmaEventos = dv.CalcularDVV(TablaFirmaEventos, a.Eventos.Select(e => e.Firma ?? string.Empty));
            return dv.CalcularDVH(TablaFirmaArchivo, a.Version, a.Exportado, a.ExportadoPor, a.Cifrado, a.Cantidad, firmaEventos);
        }

        /// <summary>Comparación en tiempo constante (no revela cuántos caracteres coinciden).</summary>
        private static bool FirmasIguales(string a, string b) =>
            CryptographicOperations.FixedTimeEquals(Encoding.ASCII.GetBytes(a.Trim().ToUpperInvariant()), Encoding.ASCII.GetBytes(b.ToUpperInvariant()));

        private static bool SePuedeDesencriptar(string valor)
        {
            try
            {
                CriptografiaHandler_60MN.Desencriptar(valor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ---------------------------- Duplicados ----------------------------

        /// <summary>
        /// Un evento ya existe si hay una fila con la misma fecha, usuario, evento y
        /// descripción cifrada (el texto cifrado es único porque cada cifrado usa un IV al azar).
        /// </summary>
        private static string Clave(EventoBitacoraXml_60MN e) => Clave(e.FechaHora, e.Usuario, e.Evento, e.Descripcion);

        private static string Clave(DateTime fecha, string usuario, string evento, string descripcion) =>
            $"{fecha:yyyy-MM-ddTHH:mm:ss}|{usuario}|{evento}|{descripcion}";

        private static HashSet<string> ClavesExistentes(List<EventoBitacoraXml_60MN> eventos)
        {
            HashSet<string> claves = new HashSet<string>(StringComparer.Ordinal);
            if (eventos.Count == 0) return claves;

            const string sql = "SELECT FechaHora, Usuario, Evento, Descripcion FROM Bitacora WHERE FechaHora BETWEEN @desde AND @hasta";
            using SqlConnection con = ConexionSeguridad_60MN.Abrir();
            using SqlCommand com = new SqlCommand(sql, con);
            com.Parameters.Add("@desde", System.Data.SqlDbType.DateTime2).Value = eventos.Min(e => e.FechaHora);
            com.Parameters.Add("@hasta", System.Data.SqlDbType.DateTime2).Value = eventos.Max(e => e.FechaHora);
            using SqlDataReader lector = com.ExecuteReader();
            while (lector.Read())
                claves.Add(Clave(lector.GetDateTime(0), lector.GetString(1), lector.GetString(2), lector.GetString(3)));
            return claves;
        }

        private static DateTime SinFraccion(DateTime fecha) =>
            new DateTime(fecha.Year, fecha.Month, fecha.Day, fecha.Hour, fecha.Minute, fecha.Second, DateTimeKind.Unspecified);
    }
}
