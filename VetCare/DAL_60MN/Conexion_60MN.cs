using System.Configuration;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>
    /// Acceso a VetCareBD_60MN. Todas las consultas de la DAL pasan por acá y
    /// reciben sus valores como SqlParameter: ningún dato del usuario se
    /// concatena dentro del texto SQL.
    /// </summary>
    internal static class Conexion_60MN
    {
        internal static SqlConnection Abrir()
        {
            string? cadena = ConfigurationManager.AppSettings["conexionBD"];
            if (string.IsNullOrWhiteSpace(cadena))
                throw new InvalidOperationException("Falta la cadena de conexión 'conexionBD' en App.config.");
            SqlConnection conexion = new SqlConnection(cadena);
            conexion.Open();
            return conexion;
        }

        internal static int EjecutarNonQuery(string sql, params (string Nombre, object? Valor)[] parametros)
        {
            using SqlConnection con = Abrir();
            using SqlCommand com = Crear(sql, con, null, parametros);
            return com.ExecuteNonQuery();
        }

        internal static object? EjecutarEscalar(string sql, params (string Nombre, object? Valor)[] parametros)
        {
            using SqlConnection con = Abrir();
            using SqlCommand com = Crear(sql, con, null, parametros);
            object? valor = com.ExecuteScalar();
            return valor is DBNull ? null : valor;
        }

        internal static List<T> EjecutarLista<T>(string sql, Func<SqlDataReader, T> mapear, params (string Nombre, object? Valor)[] parametros)
        {
            List<T> resultado = new List<T>();
            using SqlConnection con = Abrir();
            using SqlCommand com = Crear(sql, con, null, parametros);
            using SqlDataReader lector = com.ExecuteReader();
            while (lector.Read())
                resultado.Add(mapear(lector));
            return resultado;
        }

        internal static SqlCommand Crear(string sql, SqlConnection con, SqlTransaction? transaccion, params (string Nombre, object? Valor)[] parametros)
        {
            SqlCommand com = new SqlCommand(sql, con, transaccion);
            foreach ((string nombre, object? valor) in parametros)
                com.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
            return com;
        }

        internal static string? TextoONulo(this SqlDataReader lector, string columna)
        {
            int i = lector.GetOrdinal(columna);
            return lector.IsDBNull(i) ? null : lector.GetString(i);
        }

        internal static int? EnteroONulo(this SqlDataReader lector, string columna)
        {
            int i = lector.GetOrdinal(columna);
            return lector.IsDBNull(i) ? null : lector.GetInt32(i);
        }
    }
}
