using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Usuario del sistema. DNI y Email viajan en claro en memoria y se guardan cifrados con AES.</summary>
    public class Usuario_60MN : IUsuario_60MN
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>Hash SHA-256 con sal ("sal.hash" en Base64). Nunca la contraseña en claro.</summary>
        public string ClaveHash { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Dni { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        /// <summary>Familias (UsuarioFamilia) y patentes individuales (UsuarioPatente) del usuario.</summary>
        public IList<IPermiso_60MN> Permisos { get; } = new List<IPermiso_60MN>();

        /// <summary>Nombres de las familias del usuario, para mostrar en grillas.</summary>
        public string Familias { get; set; } = string.Empty;

        /// <summary>Cantidad de patentes asignadas en forma individual (además de las familias).</summary>
        public int PatentesIndividuales { get; set; }

        public bool Activo { get; set; } = true;

        public bool Bloqueado { get; set; }

        public int IntentosFallidos { get; set; }

        /// <summary>True hasta que el usuario cambia la contraseña inicial (CUS05 «extend» CUS07).</summary>
        public bool PrimerIngreso { get; set; } = true;

        public bool EnSesion { get; set; }

        public string NombreCompleto => $"{Apellido}, {Nombre}";

        public string Estado => !Activo ? "Baja" : Bloqueado ? "Bloqueado" : "Activo";
    }
}
