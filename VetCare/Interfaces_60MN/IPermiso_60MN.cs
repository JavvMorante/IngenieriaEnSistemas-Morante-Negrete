namespace Interfaces_60MN
{
    /// <summary>
    /// Componente del patrón Composite de permisos. Una patente es una hoja
    /// (un permiso atómico sobre una operación del sistema) y una familia es
    /// un compuesto que agrupa patentes y/o otras familias (un rol).
    /// </summary>
    public interface IPermiso_60MN : IEntity_60MN
    {
        string Nombre { get; set; }

        bool EsFamilia { get; }

        void AgregarHijo(IPermiso_60MN permiso);

        void QuitarHijo(IPermiso_60MN permiso);

        IList<IPermiso_60MN> ObtenerHijos();

        /// <summary>Indica si este componente o alguno de sus descendientes otorga la patente con ese código.</summary>
        bool TienePatente(string codigo);
    }
}
