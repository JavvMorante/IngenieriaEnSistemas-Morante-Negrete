namespace Interfaces_60MN
{
    /// <summary>Datos mínimos del usuario que necesita el SessionManager para operar.</summary>
    public interface IUsuario_60MN : IEntity_60MN
    {
        string NombreUsuario { get; set; }

        string Nombre { get; set; }

        string Apellido { get; set; }

        /// <summary>
        /// Permisos asignados directamente al usuario: sus familias (UsuarioFamilia)
        /// y sus patentes individuales (UsuarioPatente), además de las de la familia.
        /// </summary>
        IList<IPermiso_60MN> Permisos { get; }
    }
}
