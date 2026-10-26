using System.Security.Cryptography;
using DAL_60MN;
using Entidades_60MN;
using Microsoft.Data.SqlClient;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>Resultado de una restauración: archivo usado e integridad de la base restaurada.</summary>
    public class ResultadoRestore_60MN
    {
        public RespaldoBD_60MN Respaldo { get; set; } = new RespaldoBD_60MN();

        /// <summary>Inconsistencias de dígitos verificadores encontradas en la base restaurada.</summary>
        public List<InconsistenciaDV_60MN> Inconsistencias { get; set; } = new List<InconsistenciaDV_60MN>();
    }

    /// <summary>
    /// CUS12 Gestión de Backup: respaldar y restaurar la base de datos.
    ///
    /// El archivo .bak lo escribe y lo lee el SERVICIO de SQL Server, que solo tiene permiso
    /// sobre su propia carpeta de backups. Para que el usuario pueda usar cualquier carpeta,
    /// todo se hace en dos pasos:
    /// - Respaldar: SQL Server genera el backup en su carpeta y VetCare (con los permisos del
    ///   usuario de Windows) lo copia a la carpeta elegida, verifica la copia con SHA-256 y
    ///   borra el archivo intermedio.
    /// - Restaurar: VetCare copia el archivo elegido (de cualquier carpeta) a la carpeta de
    ///   SQL Server, lo valida, restaura desde esa copia y la borra al terminar.
    /// Requisito: SQL Server en el mismo equipo (es el caso de VetCare, con SQL Express local).
    /// </summary>
    public class BackupBLL_60MN
    {
        /// <summary>Marca en la descripción del backup (msdb) que antecede a la carpeta final elegida.</summary>
        private const string PrefijoDestino = "VetCare|";

        private readonly BackupDAL_60MN mapper = new BackupDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Carpeta sugerida para guardar los backups: "Documentos\Backups VetCare" del usuario.</summary>
        public string CarpetaPredeterminada() =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Backups VetCare");

        /// <summary>Carpeta de backups de la instancia de SQL Server (paso intermedio de backup y restore).</summary>
        public string CarpetaServidor()
        {
            string? carpeta = mapper.CarpetaPredeterminada();
            if (string.IsNullOrWhiteSpace(carpeta))
                throw new NegocioException_60MN("No se pudo obtener la carpeta de backups de SQL Server.");
            return carpeta!;
        }

        /// <summary>Nombre del archivo: VetCareBD_60MN_aaaaMMdd_HHmmss.bak.</summary>
        public string NombreArchivo(DateTime fecha) => $"{mapper.NombreBaseDatos}_{fecha:yyyyMMdd_HHmmss}.bak";

        public string PatronNombreArchivo => $"{mapper.NombreBaseDatos}_aaaaMMdd_HHmmss.bak";

        // =====================================================================
        // Respaldar (pasos 2 a 4)
        // =====================================================================

        public RespaldoBD_60MN Respaldar(string carpeta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Backup);
            if (string.IsNullOrWhiteSpace(carpeta))
                throw new NegocioException_60MN("Indique la carpeta de destino del backup.");

            // 3.1: no se respalda una base con inconsistencias de dígitos verificadores.
            List<InconsistenciaDV_60MN> inconsistencias = dv.VerificarTodo();
            if (inconsistencias.Count > 0)
            {
                SessionManager_60MN.Instancia.IntegridadComprometida = true;
                throw new NegocioException_60MN(
                    $"La base tiene {inconsistencias.Count} inconsistencia(s) de dígitos verificadores en " +
                    string.Join(", ", inconsistencias.Select(i => i.Tabla).Distinct()) +
                    ". Revíselas en ADMIN > Dígitos verificadores antes de respaldar.");
            }

            string destinoCarpeta = Path.GetFullPath(carpeta.Trim());
            string servidor = CarpetaServidor();
            string archivo = NombreArchivo(DateTime.Now);
            string rutaServidor = Path.Combine(servidor, archivo);
            string rutaFinal = Path.Combine(destinoCarpeta, archivo);
            bool mismaCarpeta = MismaCarpeta(destinoCarpeta, servidor);

            // 4.1: la carpeta de destino tiene que existir (o poder crearse) con permiso de escritura del usuario.
            try
            {
                Directory.CreateDirectory(destinoCarpeta);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
            {
                throw new NegocioException_60MN($"No se puede usar la carpeta \"{destinoCarpeta}\": {ex.Message}");
            }

            // Paso 1: SQL Server genera el backup en su carpeta (donde siempre puede escribir).
            string descripcion = (PrefijoDestino + rutaFinal).Length <= 255 ? PrefijoDestino + rutaFinal : PrefijoDestino;
            try
            {
                mapper.Respaldar(rutaServidor, descripcion);
            }
            catch (SqlException ex)
            {
                throw new NegocioException_60MN($"SQL Server no pudo generar el backup.\n\nDetalle: {ex.Message}");
            }

            // Paso 2: VetCare lo copia a la carpeta elegida y verifica la copia.
            if (!mismaCarpeta)
            {
                try
                {
                    CopiarVerificando(rutaServidor, rutaFinal);
                    File.Delete(rutaServidor);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    // El backup ya existe en la carpeta de SQL Server: no se pierde, se informa dónde quedó.
                    bitacora.Registrar(EventoSistema_60MN.BackupRealizado,
                        $"Backup completo de {mapper.NombreBaseDatos} en {rutaServidor} (no se pudo copiar a {destinoCarpeta}: {ex.Message}).");
                    throw new NegocioException_60MN(
                        $"El backup se generó, pero no se pudo copiar a \"{destinoCarpeta}\": {ex.Message}\n\n" +
                        $"El archivo quedó en la carpeta de SQL Server:\n{rutaServidor}");
                }
            }

            RespaldoBD_60MN respaldo = new RespaldoBD_60MN
            {
                Ruta = rutaFinal,
                BaseDatos = mapper.NombreBaseDatos,
                Fecha = DateTime.Now,
                TamanioBytes = new FileInfo(rutaFinal).Length,
                Verificado = true // CHECKSUM al generarlo y SHA-256 de la copia
            };
            bitacora.Registrar(EventoSistema_60MN.BackupRealizado,
                $"Backup completo de {mapper.NombreBaseDatos} en {rutaFinal} ({respaldo.Tamanio}, verificado con checksum" +
                (mismaCarpeta ? ")." : " y copia verificada con SHA-256)."));
            return respaldo;
        }

        // =====================================================================
        // Restaurar (pasos 5 a 9)
        // =====================================================================

        /// <summary>
        /// Backups registrados por SQL Server (msdb) cuyo archivo todavía existe. Para los
        /// generados por VetCare se muestra la carpeta final elegida, no la intermedia.
        /// </summary>
        public List<RespaldoBD_60MN> ListarRespaldos()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Restore);
            List<RespaldoBD_60MN> respaldos = mapper.ListarRespaldos();
            foreach (RespaldoBD_60MN r in respaldos)
                if (r.Descripcion.StartsWith(PrefijoDestino) && r.Descripcion.Length > PrefijoDestino.Length)
                    r.Ruta = r.Descripcion.Substring(PrefijoDestino.Length);
            return respaldos.Where(r => ExisteSinError(r.Ruta))
                            .GroupBy(r => r.Ruta, StringComparer.OrdinalIgnoreCase).Select(g => g.First())
                            .ToList();
        }

        /// <summary>
        /// 8.1: el archivo (de cualquier carpeta) debe ser un backup completo, legible y verificado
        /// de esta base. Se valida sobre una copia en la carpeta de SQL Server, que se borra al terminar.
        /// </summary>
        public RespaldoBD_60MN ValidarArchivo(string ruta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Restore);
            string origen = RutaExistente(ruta);
            (string enServidor, bool esCopia) = LlevarAlServidor(origen);
            try
            {
                RespaldoBD_60MN respaldo = Validar(enServidor);
                respaldo.Ruta = origen;
                return respaldo;
            }
            finally
            {
                if (esCopia) BorrarSinError(enServidor);
            }
        }

        /// <summary>
        /// Paso 8: copia el archivo a la carpeta de SQL Server, lo valida, registra el inicio y
        /// reemplaza la base. Paso 9: verifica los dígitos verificadores de la base restaurada y
        /// registra el resultado. La GUI cierra la sesión a continuación.
        /// </summary>
        public ResultadoRestore_60MN Restaurar(string ruta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Restore);
            string origen = RutaExistente(ruta);
            (string enServidor, bool esCopia) = LlevarAlServidor(origen);
            try
            {
                RespaldoBD_60MN respaldo = Validar(enServidor);

                // Si el restore termina bien, este evento se pierde junto con el resto de la base
                // posterior al backup; si falla, queda como evidencia del intento.
                bitacora.Registrar(EventoSistema_60MN.RestoreIniciado,
                    $"Inicio de la restauración de {mapper.NombreBaseDatos} desde {origen} (backup del {respaldo.Fecha:dd/MM/yyyy HH:mm}).");
                try
                {
                    mapper.Restaurar(enServidor);
                }
                catch (SqlException ex)
                {
                    throw new NegocioException_60MN($"No se pudo restaurar la base de datos. La base no fue modificada.\n\nDetalle: {ex.Message}");
                }

                List<InconsistenciaDV_60MN> inconsistencias = dv.VerificarTodo();
                SessionManager_60MN.Instancia.IntegridadComprometida = inconsistencias.Count > 0;
                bitacora.Registrar(EventoSistema_60MN.RestoreRealizado,
                    $"Base {mapper.NombreBaseDatos} restaurada desde {origen} (backup del {respaldo.Fecha:dd/MM/yyyy HH:mm}). " +
                    (inconsistencias.Count == 0
                        ? "Dígitos verificadores: base íntegra."
                        : $"Dígitos verificadores: {inconsistencias.Count} inconsistencia(s)."));
                respaldo.Ruta = origen;
                return new ResultadoRestore_60MN { Respaldo = respaldo, Inconsistencias = inconsistencias };
            }
            finally
            {
                if (esCopia) BorrarSinError(enServidor);
            }
        }

        // =====================================================================
        // Auxiliares
        // =====================================================================

        /// <summary>Validación de 8.1 sobre un archivo que SQL Server puede leer.</summary>
        private RespaldoBD_60MN Validar(string rutaEnServidor)
        {
            RespaldoBD_60MN? respaldo = mapper.LeerArchivo(rutaEnServidor);
            if (respaldo == null)
                throw new NegocioException_60MN("El archivo no es un backup válido de SQL Server.");
            if (!string.Equals(respaldo.BaseDatos, mapper.NombreBaseDatos, StringComparison.OrdinalIgnoreCase))
                throw new NegocioException_60MN($"El archivo es un backup de la base \"{respaldo.BaseDatos}\", no de {mapper.NombreBaseDatos}.");
            if (!respaldo.TipoCompleto)
                throw new NegocioException_60MN("El archivo no contiene un backup completo de la base de datos.");
            if (!respaldo.Verificado)
                throw new NegocioException_60MN("El archivo de backup está dañado: falló la verificación con checksum.");
            return respaldo;
        }

        private static string RutaExistente(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                throw new NegocioException_60MN("Seleccione el archivo de backup a restaurar.");
            string completa = Path.GetFullPath(ruta.Trim());
            if (!File.Exists(completa))
                throw new NegocioException_60MN($"El archivo no existe o no se puede leer:\n{completa}");
            return completa;
        }

        /// <summary>
        /// Deja el archivo donde SQL Server lo pueda leer: si ya está en su carpeta se usa tal
        /// cual; si no, se copia ahí (con nombre temporal) y se verifica la copia.
        /// </summary>
        private (string Ruta, bool EsCopia) LlevarAlServidor(string origen)
        {
            string servidor = CarpetaServidor();
            if (MismaCarpeta(Path.GetDirectoryName(origen) ?? string.Empty, servidor))
                return (origen, false);

            string copia = Path.Combine(servidor, $"VetCare_restaurar_{Guid.NewGuid():N}.bak");
            try
            {
                CopiarVerificando(origen, copia);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                BorrarSinError(copia);
                throw new NegocioException_60MN(
                    $"No se pudo copiar el archivo a la carpeta de SQL Server ({servidor}) para restaurarlo: {ex.Message}\n\n" +
                    "Copie el archivo a esa carpeta manualmente o pida permiso de escritura sobre ella.");
            }
            return (copia, true);
        }

        /// <summary>Copia el archivo y comprueba que la copia sea idéntica al original (SHA-256).</summary>
        private static void CopiarVerificando(string origen, string destino)
        {
            File.Copy(origen, destino, overwrite: false);
            if (!HashArchivo(origen).SequenceEqual(HashArchivo(destino)))
            {
                BorrarSinError(destino);
                throw new IOException("la copia no coincide con el original (SHA-256 distinto).");
            }
        }

        private static byte[] HashArchivo(string ruta)
        {
            using (SHA256 sha = SHA256.Create())
            using (FileStream archivo = File.OpenRead(ruta))
                return sha.ComputeHash(archivo);
        }

        private static bool MismaCarpeta(string a, string b) =>
            string.Equals(Path.GetFullPath(a).TrimEnd('\\', '/'), Path.GetFullPath(b).TrimEnd('\\', '/'), StringComparison.OrdinalIgnoreCase);

        private static bool ExisteSinError(string ruta)
        {
            try { return File.Exists(ruta); }
            catch (Exception) { return false; }
        }

        private static void BorrarSinError(string ruta)
        {
            try { if (File.Exists(ruta)) File.Delete(ruta); }
            catch (Exception) { /* un archivo temporal que no se pudo borrar no impide la operación */ }
        }
    }
}
