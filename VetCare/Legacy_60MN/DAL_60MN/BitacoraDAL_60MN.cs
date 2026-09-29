using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL_60MN
{
    public class BitacoraDAL_60MN
    {
        DigitosVerificadores_60MN dv = new DigitosVerificadores_60MN();
        Conexion_60MN con = new Conexion_60MN();
        public string IngresarDatoBitacora(string nombreOperacion, string descripcion, int criticidad, int usuarioid)
        {
            string sql = "insert into Bitacora(NombreOperacion,Descripcion,UsuarioID,Criticidad,FechayHora) values ('"
                        + nombreOperacion + "','" + descripcion + "'," + usuarioid + ","
                        + criticidad + ",getdate())";


            string rta = con.Ejecutar(sql);
            dv.RecalcularDVH();


            return rta;


        }

        public DataTable traerUsuarios()
        {
            DataTable datausuario = new DataTable();
            string sql = "select distinct usuario from usuario";
            datausuario = con.Ejecutarreader(sql);


            return datausuario;

        }

        public DataTable traerCriticidad()
        {
            DataTable dataCriticidad = new DataTable();
            string sql = "select distinct Criticidad  from Bitacora";
            dataCriticidad = con.Ejecutarreader(sql);


            return dataCriticidad;

        }

        public DataTable ConsultarBitacora(string fechadesde, string fechahasta, string sqlcriticidad, string sqlusuario)
        {
            DataTable dt = new DataTable();

          
            string sql = "SELECT NombreOperacion, Descripcion, UsuarioID, Criticidad, FechayHora " +
                         "FROM Bitacora " +
                         $"WHERE FechayHora >= '{fechadesde}' AND FechayHora < '{fechahasta}' ";

            if (!string.IsNullOrWhiteSpace(sqlcriticidad) && !sqlcriticidad.ToUpper().Contains("SELECT"))
            {
                sql += $" AND Criticidad = {sqlcriticidad}";
            }

          
            if (!string.IsNullOrWhiteSpace(sqlusuario) && !sqlusuario.ToUpper().Contains("SELECT"))
            {
                sql += $" AND UsuarioID IN ({sqlusuario})";
            }


            dt = con.Ejecutarreader(sql);
            return dt;
        }


    }
}
