using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces_60MN
{
    public interface IUsuario_60MN
    {
        long IdUsuario { get; set; }

        long Dni { get; set; }

        string Apellido { get; set; }

        string Nombre { get; set; }

        string Email { get; set; }

        string Username { get; set; }

        string PasswordHash { get; set; }

        string Rol { get; set; }

        int LoginCount { get; set; }

        bool Locked { get; set; }

        bool Deleted { get; set; }

        IList<IPermiso_60MN> Permisos { get; }
    }
}
