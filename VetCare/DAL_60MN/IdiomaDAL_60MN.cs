using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_60MN
{
    public class IdiomaDAL_60MN
    {
        Conexion_60MN con = new Conexion_60MN();

        public string CargarIdioma()
        {

            string sql = "select Descripcion from idioma where Seleccionado = 1";
            //traigo idioma 
            DataTable dt = con.Ejecutarreader(sql);

            string _idioma = dt.Rows[0][0].ToString();

            return _idioma;
        }

        public string SetearIdioma(int idiomaID)
        {
            string sql = "update Idioma set Seleccionado = 0;update idioma set seleccionado = 1 where IdiomaID = " + idiomaID + "";

            string result = con.Ejecutar(sql);

            return result;
        }
    }
}
