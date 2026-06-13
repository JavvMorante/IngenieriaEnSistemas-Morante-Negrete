using Interfaces_60MN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Servicios_60MN.Composite
{
    public class Familia_60MN : PermisoCompuesto_60MN, IFamilia_60MN
    {
        private IList<IPermiso_60MN> _hijos;

        public Familia_60MN()
        {
            _hijos = new List<IPermiso_60MN>();
        }

        public override void AgregarPermiso(IPermiso_60MN p)
        {
            if (!_hijos.Contains(p))
                _hijos.Add(p);
        }

        public override IList<IPermiso_60MN> ObtenerHijos()
        {
            return _hijos.ToArray();
        }

        public override void QuitarPermiso(IPermiso_60MN p)
        {
            if (_hijos.Contains(p))
                _hijos.Remove(p);
        }
    }
}
