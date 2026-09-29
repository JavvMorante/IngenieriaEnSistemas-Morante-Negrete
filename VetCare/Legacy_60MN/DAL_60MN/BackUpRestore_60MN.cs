using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL_60MN
{
    public class BackUpRestore_60MN
    {
        public string RealizarBackup(string path, int cantidad)
        {
            List<SqlParameter> ParametrosSP = new List<SqlParameter>();
            SqlParameter Parametro1 = new SqlParameter("@CANT", cantidad);
            ParametrosSP.Add(Parametro1);

            try
            {
                DAL_60MN.Conexion_60MN con = new Conexion_60MN();

                con.EjecutarProcedureconListaParametros("RealizarBackUp", ParametrosSP);

                return "Back Up exitoso";
            }
            catch (Exception ex)
            {

                return "Revise el acceso a la carpeta " + ex.Message;
            }
        }

        public string RealizarRestore(int cant)
        {
            string rta;
            try
            {
                DAL_60MN.Conexion_60MN con = new Conexion_60MN ();

                rta = con.EjecutarRestore(cant);

            }
            catch (Exception ex)
            {
                rta = ex.Message;

            }


            return rta;

        }
    }
}
