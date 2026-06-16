using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL_60MN
{
    public class Conexion_60MN
    {

        DataTable dt;

        static string stringconexiontest = @"Data Source=.\SQLEXPRESS01,1433;Initial Catalog=VetCareBD;Integrated Security=True;TrustServerCertificate=True;";
        SqlConnection con = new SqlConnection(stringconexiontest);
        public void Conectar()
        {

            con.Close();
            if (con.State == ConnectionState.Open)
            {


            }
            else
            {
                con.Open();

            }

            Console.WriteLine("Conexion abierta Correctamente");

        }

        internal void Desconectar()
        {
            // string stringconexiontest = "Data Source=ID705881;Initial Catalog=ELARA;Integrated Security=True";
            SqlConnection con = new SqlConnection(stringconexiontest);
            if (con.State == ConnectionState.Closed)
            {

            }
            else
            {
                con.Close();
            }


            Console.WriteLine("Conexion cerrada Correctamente");
        }
        // 1. Centralizamos la obtención de la cadena de conexión de forma segura
        private string ObtenerCadenaConexion()
        {
            // Intentamos leer del App.config / Web.config, si no, usamos el fallback
            if (ConfigurationManager.AppSettings["conexionBD"] != null)
            {
                return ConfigurationManager.AppSettings["conexionBD"].ToString();
            }

            // Tu cadena local (Casa / Facu)
            return @"Data Source=.\SQLEXPRESS01,1433;Initial Catalog=VetCareBD;Integrated Security=True;TrustServerCertificate=True;";
            

        }

        // 2. Cambiados a PUBLIC para evitar problemas de incoherencia de accesibilidad (CS0052)
        public string VerificarStringConexion(string cadena)
        {
            using (SqlConnection testCon = new SqlConnection(cadena))
            {
                try
                {
                    testCon.Open();
                    return "OK";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            } // El bloque using cierra automáticamente la conexión acá
        }

        public string EjecutarRestore(int cant)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ObtenerCadenaConexion()))
                using (SqlCommand com = new SqlCommand("RealizarRestore", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@CANT", cant));

                    con.Open();
                    com.ExecuteNonQuery(); // Para restores, inserts o updates se usa ExecuteNonQuery, no Reader
                    return "Restore OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void EjecutarProcedureconListaParametros(string nombreSP, List<SqlParameter> parametrosSP)
        {
            using (SqlConnection con = new SqlConnection(ObtenerCadenaConexion()))
            using (SqlCommand com = new SqlCommand(nombreSP, con))
            {
                com.CommandType = CommandType.StoredProcedure;

                if (parametrosSP != null)
                {
                    foreach (SqlParameter param in parametrosSP)
                    {
                        // Clonamos el parámetro para evitar problemas si se reutiliza en bucles
                        com.Parameters.Add((SqlParameter)((ICloneable)param).Clone());
                    }
                }

                con.Open();
                com.ExecuteNonQuery(); // Modifica o ejecuta lógica, usamos NonQuery para evitar fugas del Reader
            }
        }

        public void EjecutarProcedure(string sp, int usuID)
        {
            using (SqlConnection con = new SqlConnection(ObtenerCadenaConexion()))
            using (SqlCommand com = new SqlCommand(sp, con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@usuid", usuID);

                con.Open();
                com.ExecuteNonQuery();
            }
        }

        public string Ejecutar(string sql)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ObtenerCadenaConexion()))
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    con.Open();
                    int filasAfectadas = com.ExecuteNonQuery();

                    return filasAfectadas > 0
                        ? "Se ejecutó satisfactoriamente"
                        : "No se pudo ejecutar la consulta o no afectó filas";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Ejecutarreader(string sql)
        {
            // Definimos el DataTable de forma LOCAL para que sea Thread-Safe
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ObtenerCadenaConexion()))
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    dt.Load(reader);
                } // El reader se destruye de forma segura acá
            } // La conexión se cierra de forma segura acá

            return dt;
        }

        public string VerificarDatoTabla(string sql)
        {
            using (SqlConnection con = new SqlConnection(ObtenerCadenaConexion()))
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                con.Open();
                // ExecuteScalar ejecuta la consulta y devuelve únicamente la primer columna de la primer fila.
                // Es muchísimo más rápido que instanciar un DataReader y cargar un DataTable completo.
                object resultado = com.ExecuteScalar();

                return resultado != null ? resultado.ToString() : "";
            }
        }
    }
}