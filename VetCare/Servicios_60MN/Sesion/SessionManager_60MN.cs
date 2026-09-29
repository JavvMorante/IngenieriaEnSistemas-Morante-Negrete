using Interfaces_60MN;

namespace Servicios_60MN.Sesion
{
    /// <summary>
    /// Singleton que mantiene la sesión del usuario autenticado. Lo consultan
    /// la BLL (para registrar eventos con el usuario activo) y la GUI (para
    /// habilitar el menú según las patentes del rol).
    /// </summary>
    public sealed class SessionManager_60MN
    {
        private static SessionManager_60MN? instancia;
        private static readonly object candado = new object();

        private SessionManager_60MN() { }

        public static SessionManager_60MN Instancia
        {
            get
            {
                lock (candado)
                {
                    return instancia ??= new SessionManager_60MN();
                }
            }
        }

        public IUsuario_60MN? Usuario { get; private set; }

        public DateTime? Inicio { get; private set; }

        /// <summary>True si la sesión se abrió con el usuario de emergencia (archivo XML).</summary>
        public bool EsEmergencia { get; private set; }

        /// <summary>True si al iniciar sesión se detectaron dígitos verificadores inconsistentes.</summary>
        public bool IntegridadComprometida { get; set; }

        public bool EstaLogueado => Usuario != null;

        public void Login(IUsuario_60MN usuario, bool esEmergencia = false)
        {
            if (EstaLogueado)
                throw new InvalidOperationException("Ya existe una sesión iniciada.");
            Usuario = usuario;
            EsEmergencia = esEmergencia;
            Inicio = DateTime.Now;
        }

        public void Logout()
        {
            Usuario = null;
            EsEmergencia = false;
            IntegridadComprometida = false;
            Inicio = null;
        }

        public bool TienePermiso(string codigoPatente)
        {
            // Familias del usuario (y sus subfamilias) + patentes individuales asignadas al usuario.
            return Usuario != null && Usuario.Permisos.Any(p => p.TienePatente(codigoPatente));
        }
    }
}
