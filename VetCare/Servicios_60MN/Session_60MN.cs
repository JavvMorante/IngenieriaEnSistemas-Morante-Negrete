using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interfaces_60MN;
using Servicios_60MN.Composite;

namespace Servicios_60MN
{
    public class Session_60MN
    {

        private IUsuario_60MN _user { get; set; }

        public IUsuario_60MN Usuario
        {
            get {
                return _user; 
            }
        }

    public void Login(IUsuario_60MN usuario)
        {
            _user = usuario;

        }

        public void  Logout()
        { 
            _user = null; 
        }

        private bool IsInRoleRecursivo(IPermiso_60MN p, Enum tipoPermiso, bool valid)
        {
            foreach (var item in p.ObtenerHijos())
            {
                if (item is Patente_60MN && ((Patente_60MN)item).Tipo.Equals(tipoPermiso))
                {
                    valid = true;
                }
                else
                {
                    valid = IsInRoleRecursivo(item, tipoPermiso, valid);

                }

            }
            return valid;

        }

        public bool IsInRole(Enum tipoPermiso)
        {
            if (_user == null) return false;

            bool valid = false;

            foreach (var p in _user.Permisos)
            {
                if (p is Patente_60MN && ((Patente_60MN)p).Tipo.Equals(tipoPermiso))
                {
                    valid = true;
                }
                else
                {
                    valid = IsInRoleRecursivo(p, tipoPermiso, valid);
                }
            }
            return valid;
        }

        public bool IsLogged()
        {
            return _user != null;
        }
    }
}
