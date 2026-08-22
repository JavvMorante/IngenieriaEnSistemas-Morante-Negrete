using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces_60MN
{
    public interface IPermiso_60MN:IEntity_60MN
    {
        string Nombre { get; set; }
        void AgregarPermiso(IPermiso_60MN p);
        void QuitarPermiso(IPermiso_60MN p);
        IList<IPermiso_60MN> ObtenerHijos();
    }
}
