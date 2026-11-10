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
        // Exportar (serializar) y deserializar la bitácora en XML
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
        /// Deserializar XML: lee un XML de bitácora exportado y lo devuelve para mostrarlo en
        /// pantalla, sin modificar la base. Si el archivo fue alterado o no lo generó este
        /// sistema (firmas, catálogo, fechas), se rechaza completo y no se muestra nada.
        /// Las descripciones se ven desencriptadas solo si el usuario puede consultar la bitácora.
        /// </summary>
        public ResumenArchivoBitacora_60MN Deserializar(string ruta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraDeserializar);
            if (string.IsNullOrWhiteSpace(ruta))
                throw new NegocioException_60MN("Seleccione el archivo XML.");
            bool puedeVerDescripciones = Servicios_60MN.Sesion.SessionManager_60MN.Instancia.TienePermiso(CodigosPatente_60MN.BitacoraEventos);

            ResumenArchivoBitacora_60MN resumen = serializador.Analizar(ruta, puedeVerDescripciones);
            if (!resumen.Valido)
            {
                Registrar(EventoSistema_60MN.BitacoraDeserializacionRechazada,
                    $"XML de bitácora rechazado al deserializar: {resumen.NombreArchivo} (SHA-256 {resumen.HashSha256}). " +
                    string.Join(" ", resumen.Errores.Take(5)));
                throw new NegocioException_60MN("El archivo fue rechazado y no se puede mostrar:\n- " +
                                                 string.Join("\n- ", resumen.Errores));
            }

            Registrar(EventoSistema_60MN.BitacoraDeserializada,
                $"XML de bitácora deserializado: {resumen.NombreArchivo} (SHA-256 {resumen.HashSha256}), exportado por " +
                $"{resumen.ExportadoPor} el {resumen.Exportado:dd/MM/yyyy HH:mm}, {resumen.Cantidad} evento(s).");
            return resumen;
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
