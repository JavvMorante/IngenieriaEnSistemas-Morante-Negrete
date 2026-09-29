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

            if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde.Value.Date > filtro.Hasta.Value.Date)
                throw new NegocioException_60MN("La fecha de inicio no puede ser posterior a la fecha de fin.");

            bool sinFiltros = !filtro.Desde.HasValue && !filtro.Hasta.HasValue && !filtro.Modulo.HasValue &&
                              !filtro.Evento.HasValue && !filtro.Criticidad.HasValue && !filtro.UsuarioId.HasValue;
            if (sinFiltros)
                filtro.Desde = filtro.Hasta = DateTime.Today;

            List<RegistroBitacora_60MN> resultado = gestor.Consultar(filtro);
            Registrar(EventoSistema_60MN.BitacoraEventosConsultada, $"Consulta de bitácora de eventos ({resultado.Count} resultados).");
            return resultado;
        }
    }
}
