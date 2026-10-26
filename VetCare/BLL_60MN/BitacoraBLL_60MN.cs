using Seguridad_60MN.Auditoria;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// Caso de uso transversal Registrar Evento (T06A) y CUS09 Auditar Eventos.
    /// Obtiene el usuario de la sesión activa a través del SessionManager.
    /// </summary>
    public class BitacoraBLL_60MN
    {
        private readonly GestorBitacora_60MN gestor = new GestorBitacora_60MN();

        /// <summary>Registra un evento a nombre del usuario de la sesión activa.</summary>
        public bool Registrar(EventoSistema_60MN evento, string descripcion)
        {
            return gestor.Registrar(SesionActual_60MN.UsuarioId, SesionActual_60MN.NombreUsuario, evento, descripcion);
        }

        /// <summary>Registra un evento cuando todavía no hay sesión (intentos de login).</summary>
        public bool RegistrarSinSesion(int? usuarioId, string nombreUsuario, EventoSistema_60MN evento, string descripcion)
        {
            return gestor.Registrar(usuarioId, nombreUsuario, evento, descripcion);
        }

        /// <summary>Usuarios para el filtro de CUS09 y para mostrar nombre y apellido del evento seleccionado (paso 6).</summary>
        public List<Entidades_60MN.Usuario_60MN> ListarUsuariosParaFiltro()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraEventos);
            return new DAL_60MN.UsuarioDAL_60MN().ListarTodos();
        }

        /// <summary>
        /// CUS09. Si no se aplica ningún filtro devuelve los eventos del día
        /// (flujo 3.2). La consulta queda a su vez registrada como evento.
        /// </summary>
        public List<RegistroBitacora_60MN> Consultar(FiltroBitacora_60MN filtro)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraEventos);
            NormalizarFiltro(filtro);

            List<RegistroBitacora_60MN> resultado = gestor.Consultar(filtro);
            Registrar(EventoSistema_60MN.BitacoraEventosConsultada, $"Consulta de bitácora de eventos ({resultado.Count} resultados).");
            return resultado;
        }

        /// <summary>3.1 fechas invertidas; 3.2 sin filtros = eventos del día.</summary>
        private static void NormalizarFiltro(FiltroBitacora_60MN filtro)
        {
            if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde.Value.Date > filtro.Hasta.Value.Date)
                throw new NegocioException_60MN("La fecha de inicio no puede ser posterior a la fecha de fin.");

            bool sinFiltros = !filtro.Desde.HasValue && !filtro.Hasta.HasValue && !filtro.Modulo.HasValue &&
                              !filtro.Evento.HasValue && !filtro.Criticidad.HasValue && !filtro.UsuarioId.HasValue;
            if (sinFiltros)
                filtro.Desde = filtro.Hasta = DateTime.Today;
        }

        // =====================================================================
        // Exportar / importar la bitácora en XML (serialización)
        // =====================================================================

        private readonly SerializadorBitacora_60MN serializador = new SerializadorBitacora_60MN();

        /// <summary>
        /// Exporta a XML los eventos que cumplen el filtro (los mismos que se ven en la grilla).
        /// La descripción sale cifrada y el archivo firmado. No se exporta una bitácora
        /// adulterada: firmarla le daría apariencia de válida.
        /// </summary>
        public ResumenArchivoBitacora_60MN Exportar(FiltroBitacora_60MN filtro, string ruta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraExportar);
            if (string.IsNullOrWhiteSpace(ruta))
                throw new NegocioException_60MN("Indique el archivo de destino.");
            NormalizarFiltro(filtro);
            ExigirBitacoraIntegra("exportar");

            ResumenArchivoBitacora_60MN resumen;
            try
            {
                resumen = serializador.Exportar(filtro, SesionActual_60MN.NombreUsuario, ruta);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
            {
                throw new NegocioException_60MN("No se pudo exportar la bitácora: " + ex.Message);
            }

            Registrar(EventoSistema_60MN.BitacoraExportada,
                $"Bitácora exportada a XML: {resumen.Cantidad} evento(s) " +
                (resumen.Desde.HasValue ? $"del {resumen.Desde:dd/MM/yyyy HH:mm} al {resumen.Hasta:dd/MM/yyyy HH:mm} " : string.Empty) +
                $"en {resumen.NombreArchivo} (SHA-256 {resumen.HashSha256}).");
            return resumen;
        }

        /// <summary>
        /// Lee y valida un XML de bitácora sin modificar nada (vista previa). Las
        /// descripciones se muestran desencriptadas solo a quien puede consultar la bitácora.
        /// </summary>
        public ResumenArchivoBitacora_60MN AnalizarArchivo(string ruta)
        {
            SesionActual_60MN.RequiereAlgunPermiso(CodigosPatente_60MN.BitacoraExportar, CodigosPatente_60MN.BitacoraImportar);
            if (string.IsNullOrWhiteSpace(ruta))
                throw new NegocioException_60MN("Seleccione el archivo XML.");
            bool puedeVerDescripciones = Servicios_60MN.Sesion.SessionManager_60MN.Instancia.TienePermiso(CodigosPatente_60MN.BitacoraEventos);
            return serializador.Analizar(ruta, puedeVerDescripciones);
        }

        /// <summary>
        /// Importa los eventos nuevos del archivo. Se vuelve a leer y validar el archivo
        /// (no se confía en la vista previa) y debe ser exactamente el mismo que se
        /// previsualizó (mismo SHA-256): si cambió en el medio, se rechaza.
        /// </summary>
        public ResultadoImportacionBitacora_60MN Importar(string ruta, string hashPrevisualizado)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraImportar);
            ExigirBitacoraIntegra("importar");

            ResumenArchivoBitacora_60MN resumen = serializador.Analizar(ruta, desencriptarDescripciones: false);
            if (!string.Equals(resumen.HashSha256, hashPrevisualizado, StringComparison.OrdinalIgnoreCase))
                resumen.Errores.Add("El archivo cambió después de la vista previa.");
            if (!resumen.Valido)
            {
                Registrar(EventoSistema_60MN.BitacoraImportacionRechazada,
                    $"Importación de bitácora rechazada: {Path.GetFileName(ruta)} (SHA-256 {resumen.HashSha256}). " +
                    string.Join(" ", resumen.Errores.Take(5)));
                throw new NegocioException_60MN("El archivo fue rechazado y no se importó ningún evento:\n- " +
                                                 string.Join("\n- ", resumen.Errores));
            }

            ResultadoImportacionBitacora_60MN resultado = serializador.Importar(resumen);
            // El evento de la importación queda registrado en la misma bitácora que se acaba de completar.
            Registrar(EventoSistema_60MN.BitacoraImportada,
                $"Bitácora importada desde {resumen.NombreArchivo} (SHA-256 {resumen.HashSha256}), exportada por " +
                $"{resumen.ExportadoPor} el {resumen.Exportado:dd/MM/yyyy HH:mm}: {resultado.Importados} evento(s) nuevo(s), " +
                $"{resultado.Duplicados} ya existente(s) omitido(s).");
            return resultado;
        }

        /// <summary>Registra un intento rechazado en la vista previa (archivo alterado o inválido).</summary>
        public void RegistrarRechazo(ResumenArchivoBitacora_60MN resumen)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraImportar);
            if (resumen.Valido) return;
            Registrar(EventoSistema_60MN.BitacoraImportacionRechazada,
                $"Archivo de bitácora rechazado en la vista previa: {resumen.NombreArchivo} (SHA-256 {resumen.HashSha256}). " +
                string.Join(" ", resumen.Errores.Take(5)));
        }

        private void ExigirBitacoraIntegra(string operacion)
        {
            if (!serializador.BitacoraIntegra())
            {
                Servicios_60MN.Sesion.SessionManager_60MN.Instancia.IntegridadComprometida = true;
                throw new NegocioException_60MN(
                    $"La tabla Bitacora tiene inconsistencias de dígitos verificadores: no se puede {operacion} la bitácora. " +
                    "Revíselas en ADMIN > Dígitos verificadores.");
            }
        }
    }
}
