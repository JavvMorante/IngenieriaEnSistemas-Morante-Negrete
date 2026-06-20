using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_60MN
{
    public class ConfigDAL_60MN
    {
        Conexion_60MN con = new Conexion_60MN();

        public bool conectarok { get; set; }

        public void VerificarStringConexion(string cadena)
        {


            if (con.VerificarStringConexion(cadena) == "OK")
            {
                conectarok = true;

            }
            else
            {
                conectarok = false;
            }

        }

        public void saveconection(string cadena)
        {
            //XmlTextReader reader = new XmlTextReader();
        }
    }
}

