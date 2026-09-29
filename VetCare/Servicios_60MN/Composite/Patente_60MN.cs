using Interfaces_60MN;

namespace Servicios_60MN.Composite
{
    /// <summary>Hoja del Composite: permiso atómico sobre una operación del sistema.</summary>
    public class Patente_60MN : Componente_60MN
    {
        /// <summary>Código fijo de la operación (ver CodigosPatente_60MN).</summary>
        public string Codigo { get; set; } = string.Empty;

        public override bool EsFamilia => false;

        public override void AgregarHijo(IPermiso_60MN permiso)
        {
            throw new InvalidOperationException("Una patente no puede contener otros permisos.");
        }

        public override void QuitarHijo(IPermiso_60MN permiso)
        {
            throw new InvalidOperationException("Una patente no puede contener otros permisos.");
        }

        public override IList<IPermiso_60MN> ObtenerHijos() => new List<IPermiso_60MN>();

        public override bool TienePatente(string codigo) =>
            string.Equals(Codigo, codigo, StringComparison.OrdinalIgnoreCase);
    }
}
