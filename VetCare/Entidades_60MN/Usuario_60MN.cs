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

        private IList<IPermiso_60MN> _permisos;

        public Usuario_60MN()
        {
            _permisos = new List<IPermiso_60MN>();
        }

        public String Email { get; set; }

        public String Password { get; set; }

        public IList<IPermiso_60MN> Permisos
        {
            get
            {
                return _permisos;
            }
        }
    }
}
