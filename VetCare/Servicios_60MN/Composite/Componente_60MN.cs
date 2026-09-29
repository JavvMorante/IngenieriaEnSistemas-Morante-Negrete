using Interfaces_60MN;

namespace Servicios_60MN.Composite
{
    /// <summary>Componente abstracto del Composite de permisos (patentes y familias).</summary>
    public abstract class Componente_60MN : IPermiso_60MN
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public abstract bool EsFamilia { get; }

        public abstract void AgregarHijo(IPermiso_60MN permiso);

        public abstract void QuitarHijo(IPermiso_60MN permiso);

        public abstract IList<IPermiso_60MN> ObtenerHijos();

        public abstract bool TienePatente(string codigo);

        /// <summary>Devuelve todas las patentes (hojas) alcanzables desde este componente.</summary>
        public IEnumerable<Patente_60MN> ObtenerPatentes()
        {
            if (this is Patente_60MN patente)
            {
                yield return patente;
                yield break;
            }
            foreach (IPermiso_60MN hijo in ObtenerHijos())
                foreach (Patente_60MN p in ((Componente_60MN)hijo).ObtenerPatentes())
                    yield return p;
        }

        /// <summary>
        /// Indica si la familia con ese Id es este componente o forma parte de su
        /// árbol (sirve para evitar ciclos). Patentes y familias están en tablas
        /// distintas, por eso sus Id pueden coincidir y se compara también el tipo.
        /// </summary>
        public bool ContieneFamilia(int familiaId)
        {
            if (EsFamilia && Id == familiaId) return true;
            return ObtenerHijos().Any(h => ((Componente_60MN)h).ContieneFamilia(familiaId));
        }

        /// <summary>Mismo tipo (patente/familia) y mismo Id.</summary>
        public static bool Iguales(IPermiso_60MN a, IPermiso_60MN b) => a.EsFamilia == b.EsFamilia && a.Id == b.Id;

        public override string ToString() => Nombre;
    }
}
