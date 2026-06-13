using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces_60MN;

namespace Servicios_60MN.Composite
{
    public class Patente_60MN : PermisoCompuesto_60MN, IPatente_60MN
    {
        public Enum Tipo {  get; set; }

        public override void AgregarPermiso(IPermiso_60MN p)
        {

        }

        public override IList<IPermiso_60MN> ObtenerHijos()
        {
            return new List<IPermiso_60MN>();
        }

        public override void QuitarPermiso(IPermiso_60MN p)
        {

        }
    }
}
