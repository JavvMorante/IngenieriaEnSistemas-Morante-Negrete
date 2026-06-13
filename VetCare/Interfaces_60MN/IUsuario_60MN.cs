using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces_60MN
{
    public interface IUsuario_60MN
    {
        string Email { get; set; }
        string Password { get; set; }


        IList<IPermiso_60MN> Permisos { get; }
    }
}
