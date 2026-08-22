using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_60MN.Seguridad_MN60
{
    public class ConfigBLL_60MN
    {
        DAL_60MN.ConfigDAL_60MN conf = new DAL_60MN.ConfigDAL_60MN();

        public bool LeerStringConexion(string cadena)
        {
            conf.VerificarStringConexion(cadena);

            return conf.conectarok;

        }
        public string Cadena { get; set; }

        public void saveconection(string cadena)
        {
            conf.saveconection(cadena);


        }
    }
}
