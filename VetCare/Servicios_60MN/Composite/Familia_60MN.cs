using Interfaces_60MN;

namespace Servicios_60MN.Composite
{
    /// <summary>
    /// Compuesto del Composite: conjunto de patentes (FamiliaPatente) y,
    /// opcionalmente, de otras familias (FamiliaFamilia). Representa un tipo
    /// de usuario, por ejemplo Vendedor o Veterinario.
    ///
    /// La lista de hijos solo se modifica con AgregarHijo, QuitarHijo o
    /// ReemplazarHijos, que garantizan que el árbol no tenga ciclos.
    /// </summary>
    public class Familia_60MN : Componente_60MN
    {
        private readonly List<IPermiso_60MN> hijos = new List<IPermiso_60MN>();

        public override bool EsFamilia => true;

        public override void AgregarHijo(IPermiso_60MN permiso)
        {
            // Si el nuevo hijo ya contiene a esta familia (o es ella misma), se formaría un ciclo.
            if (permiso is Componente_60MN componente && Id != 0 && componente.ContieneFamilia(Id))
                throw new CicloFamiliasException_60MN(Nombre, permiso.Nombre);
            if (!hijos.Any(h => Iguales(h, permiso)))
                hijos.Add(permiso);
        }

        public override void QuitarHijo(IPermiso_60MN permiso)
        {
            hijos.RemoveAll(h => Iguales(h, permiso));
        }

        /// <summary>
        /// Reemplaza todos los hijos validando cada uno con AgregarHijo. Si alguno
        /// generara un ciclo, la familia queda exactamente como estaba.
        /// </summary>
        public void ReemplazarHijos(IEnumerable<IPermiso_60MN> nuevos)
        {
            List<IPermiso_60MN> anteriores = new List<IPermiso_60MN>(hijos);
            hijos.Clear();
            try
            {
                foreach (IPermiso_60MN permiso in nuevos)
                    AgregarHijo(permiso);
            }
            catch
            {
                hijos.Clear();
                hijos.AddRange(anteriores);
                throw;
            }
        }

        /// <summary>Vista de solo lectura: agregar o quitar hijos por acá lanza NotSupportedException.</summary>
        public override IList<IPermiso_60MN> ObtenerHijos() => hijos.AsReadOnly();

        public override bool TienePatente(string codigo) => hijos.Any(h => h.TienePatente(codigo));

        public IEnumerable<Patente_60MN> PatentesDirectas => hijos.OfType<Patente_60MN>();

        public IEnumerable<Familia_60MN> FamiliasDirectas => hijos.OfType<Familia_60MN>();
    }
}
