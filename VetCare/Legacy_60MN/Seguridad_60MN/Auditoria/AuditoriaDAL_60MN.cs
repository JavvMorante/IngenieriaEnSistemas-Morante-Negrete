using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Seguridad_60MN.Auditoria
{
    /// <summary>
    /// Acceso a datos de la bitacora, 100% parametrizado. Reemplaza a
    /// DAL_60MN.BitacoraDAL_60MN, que armaba el INSERT y el SELECT
    /// concatenando texto (incluido el intento fallido de "blacklist" que
    /// chequeaba si el filtro contenia la palabra SELECT).
    /// </summary>
    public sealed class AuditoriaDAL_60MN
    {
        private readonly string cadenaConexion;

        public AuditoriaDAL_60MN(string cadenaConexion = null)
        {
            this.cadenaConexion = cadenaConexion ?? ConfigurationManager.AppSettings["conexionBD"];
        }

        public void Insertar(string nombreOperacion, string descripcion, int usuarioId, int criticidad, DateTime fechaYHora)
        {
            const string sql = @"INSERT INTO Bitacora (NombreOperacion, Descripcion, UsuarioID, Criticidad, FechayHora)
                                  VALUES (@nombreOperacion, @descripcion, @usuarioId, @criticidad, @fechaYHora)";
            using (SqlConnection con = new SqlConnection(cadenaConexion))
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                com.Parameters.AddWithValue("@nombreOperacion", nombreOperacion);
                com.Parameters.AddWithValue("@descripcion", descripcion);
                com.Parameters.AddWithValue("@usuarioId", usuarioId);
                com.Parameters.AddWithValue("@criticidad", criticidad);
                com.Parameters.AddWithValue("@fechaYHora", fechaYHora);
                con.Open();
                com.ExecuteNonQuery();
            }
        }

        public DataTable Consultar(DateTime desde, DateTime hastaExclusive, int? criticidad, int? usuarioId)
        {
            string sql = @"SELECT NombreOperacion, Descripcion, UsuarioID, Criticidad, FechayHora
                           FROM Bitacora
                           WHERE FechayHora >= @desde AND FechayHora < @hasta";
            if (criticidad.HasValue) sql += " AND Criticidad = @criticidad";
            if (usuarioId.HasValue) sql += " AND UsuarioID = @usuarioId";

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                com.Parameters.AddWithValue("@desde", desde);
                com.Parameters.AddWithValue("@hasta", hastaExclusive);
                if (criticidad.HasValue) com.Parameters.AddWithValue("@criticidad", criticidad.Value);
                if (usuarioId.HasValue) com.Parameters.AddWithValue("@usuarioId", usuarioId.Value);

                con.Open();
                using (SqlDataReader lector = com.ExecuteReader())
                {
                    DataTable tabla = new DataTable();
                    tabla.Load(lector);
                    return tabla;
                }
            }
        }
    }
}
