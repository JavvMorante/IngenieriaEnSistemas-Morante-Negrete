using Interfaces_60MN;

namespace Servicios_60MN.Composite
{
    /// <summary>
    /// Compuesto del Composite: conjunto de patentes (FamiliaPatente) y,
    /// opcionalmente, de otras familias (FamiliaFamilia). Representa un tipo
    /// de usuario, por ejemplo Vendedor o Veterinario.
    /// </summary>
    public class Familia_60MN : Componente_60MN
    {
        private readonly List<IPermiso_60MN> hijos = new List<IPermiso_60MN>();

        public override bool EsFamilia => true;

        public override void AgregarHijo(IPermiso_60MN permiso)
        {
            if (permiso is Componente_60MN componente && Id != 0 && componente.ContieneFamilia(Id))
                throw new InvalidOperationException("No se puede agregar el permiso porque generaría un ciclo.");
            if (!hijos.Any(h => Iguales(h, permiso)))
                hijos.Add(permiso);
        }

        public override void QuitarHijo(IPermiso_60MN permiso)
        {
            hijos.RemoveAll(h => Iguales(h, permiso));
        }

        public override IList<IPermiso_60MN> ObtenerHijos() => hijos;

        public override bool TienePatente(string codigo) => hijos.Any(h => h.TienePatente(codigo));

        public IEnumerable<Patente_60MN> PatentesDirectas => hijos.OfType<Patente_60MN>();

        public IEnumerable<Familia_60MN> FamiliasDirectas => hijos.OfType<Familia_60MN>();
    }
}
