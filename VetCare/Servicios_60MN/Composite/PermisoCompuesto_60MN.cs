using Interfaces_60MN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios_60MN.Composite
{
    public abstract class PermisoCompuesto_60MN : ServiceEntity_60MN, IPermiso_60MN
    {
        public string Nombre { get; set; }

        public abstract void AgregarPermiso(IPermiso_60MN p);

        public abstract void QuitarPermiso(IPermiso_60MN p);

        public abstract IList<IPermiso_60MN> ObtenerHijos();

        public override string ToString()
        {
            return this.Nombre;
        }
    }
}
