using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interfaces_60MN;

namespace Entidades_60MN
{
    public class Usuario_60MN : Entity_60MN, IUsuario_60MN
    {

        private readonly IList<IPermiso_60MN> _permisos;

        public Usuario_60MN()
        {
            _permisos = new List<IPermiso_60MN>();
        }

        public long IdUsuario { get; set; }

        public long Dni { get; set; }

        public string Apellido { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Rol { get; set; }

        public int LoginCount { get; set; }

        public bool Locked { get; set; }

        public bool Deleted { get; set; }

        public IList<IPermiso_60MN> Permisos => _permisos;
    }
}
