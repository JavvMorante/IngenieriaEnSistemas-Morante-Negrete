using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>Atajos sobre el SessionManager que usan todos los servicios de la BLL.</summary>
    internal static class SesionActual_60MN
    {
        private static SessionManager_60MN Sesion => SessionManager_60MN.Instancia;

        /// <summary>Id del usuario en la base, o null si la sesión es del usuario de emergencia.</summary>
        internal static int? UsuarioId => Sesion.EstaLogueado && !Sesion.EsEmergencia ? Sesion.Usuario!.Id : null;

        internal static string NombreUsuario => Sesion.Usuario?.NombreUsuario ?? "(sin sesión)";

        /// <summary>
        /// Control de autorización en la BLL: además de que el menú oculte la
        /// opción, cada operación vuelve a verificar la patente.
        /// </summary>
        internal static void RequierePermiso(string codigoPatente)
        {
            if (!Sesion.EstaLogueado)
                throw new NegocioException_60MN("No hay una sesión iniciada.");
            if (!Sesion.TienePermiso(codigoPatente))
                throw new NegocioException_60MN("No tiene permiso para realizar esta operación.");
        }
    }
}
