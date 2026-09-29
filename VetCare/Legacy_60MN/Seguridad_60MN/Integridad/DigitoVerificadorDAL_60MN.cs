using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Seguridad_60MN.Integridad
{
    /// <summary>
    /// Acceso a datos para leer y actualizar los digitos verificadores.
    /// A diferencia del resto del proyecto, esta clase usa SqlParameter en
    /// TODAS las consultas -- ninguna concatena texto dentro del SQL.
    ///
    /// OJO: los nombres de columnas/tablas de aca (Usuario, UsuarioOperacion,
    /// Bitacora, PerfilUsuario, Operacion) se reconstruyeron leyendo las
    /// consultas de DAL_60MN/DigitosVerificadores_60MN.cs, no de un script
    /// de creacion real. Si tu base usa otros nombres, ajustalos aca.
    /// </summary>
    public sealed class DigitoVerificadorDAL_60MN
    {
        private readonly string cadenaConexion;

        public DigitoVerificadorDAL_60MN(string cadenaConexion = null)
        {
            this.cadenaConexion = cadenaConexion ?? ConfigurationManager.AppSettings["conexionBD"];
        }

        private SqlConnection AbrirConexion()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            conexion.Open();
            return conexion;
        }

        // ---------- Usuario ----------
        public IEnumerable<UsuarioParaDV> LeerUsuarios()
        {
            const string sql = "SELECT UsuarioId, Usuario, Clave FROM Usuario";
            using (SqlConnection con = AbrirConexion())
            using (SqlCommand com = new SqlCommand(sql, con))
            using (SqlDataReader lector = com.ExecuteReader())
                while (lector.Read())
                    yield return new UsuarioParaDV
                    {
                        UsuarioId = lector.GetInt32(0),
                        Usuario = lector.GetString(1),
                        Clave = lector.IsDBNull(2) ? null : lector.GetString(2)
                    };
        }

        public void ActualizarDVHUsuario(int usuarioId, string dvh)
        {
            EjecutarNonQuery("UPDATE Usuario SET DVH = @dvh WHERE UsuarioId = @id",
                ("@dvh", dvh), ("@id", usuarioId));
        }

        // ---------- UsuarioOperacion ----------
        public IEnumerable<UsuarioOperacionParaDV> LeerUsuarioOperacion()
        {
            const string sql = "SELECT UsuarioId, OperacionID, Habilitado FROM UsuarioOperacion";
            using (SqlConnection con = AbrirConexion())
            using (SqlCommand com = new SqlCommand(sql, con))
            using (SqlDataReader lector = com.ExecuteReader())
                while (lector.Read())
                    yield return new UsuarioOperacionParaDV
                    {
                        UsuarioId = lector.GetInt32(0),
                        OperacionId = lector.GetInt32(1),
                        Habilitado = lector.IsDBNull(2) ? null : lector.GetString(2)
                    };
        }

        public void ActualizarDVHUsuarioOperacion(int usuarioId, int operacionId, string dvh)
        {
            EjecutarNonQuery("UPDATE UsuarioOperacion SET DVH = @dvh WHERE UsuarioId = @uid AND OperacionID = @oid",
                ("@dvh", dvh), ("@uid", usuarioId), ("@oid", operacionId));
        }

        // ---------- Bitacora ----------
        public IEnumerable<BitacoraParaDV> LeerBitacora()
        {
            const string sql = "SELECT BitacoraID, UsuarioID, FechayHora FROM Bitacora";
            using (SqlConnection con = AbrirConexion())
            using (SqlCommand com = new SqlCommand(sql, con))
            using (SqlDataReader lector = com.ExecuteReader())
                while (lector.Read())
                    yield return new BitacoraParaDV
                    {
                        BitacoraId = lector.GetInt32(0),
                        UsuarioId = lector.GetInt32(1),
                        FechaYHora = lector.GetDateTime(2)
                    };
        }

        public void ActualizarDVHBitacora(int bitacoraId, string dvh)
        {
            EjecutarNonQuery("UPDATE Bitacora SET DVH = @dvh WHERE BitacoraID = @id",
                ("@dvh", dvh), ("@id", bitacoraId));
        }

        // ---------- PerfilUsuario ----------
        public IEnumerable<PerfilUsuarioParaDV> LeerPerfilUsuario()
        {
            const string sql = "SELECT PerfilUsuarioID, NombrePerfil, DescPerfil FROM PerfilUsuario";
            using (SqlConnection con = AbrirConexion())
            using (SqlCommand com = new SqlCommand(sql, con))
            using (SqlDataReader lector = com.ExecuteReader())
                while (lector.Read())
                    yield return new PerfilUsuarioParaDV
                    {
                        PerfilUsuarioId = lector.GetInt32(0),
                        NombrePerfil = lector.IsDBNull(1) ? null : lector.GetString(1),
                        DescPerfil = lector.IsDBNull(2) ? null : lector.GetString(2)
                    };
        }

        public void ActualizarDVHPerfilUsuario(int perfilUsuarioId, string dvh)
        {
            EjecutarNonQuery("UPDATE PerfilUsuario SET DVH = @dvh WHERE PerfilUsuarioID = @id",
                ("@dvh", dvh), ("@id", perfilUsuarioId));
        }

        // ---------- Operacion ----------
        public IEnumerable<OperacionParaDV> LeerOperacion()
        {
            const string sql = "SELECT OperacionID, Descripcion, PatenteEscencial FROM Operacion";
            using (SqlConnection con = AbrirConexion())
            using (SqlCommand com = new SqlCommand(sql, con))
            using (SqlDataReader lector = com.ExecuteReader())
                while (lector.Read())
                    yield return new OperacionParaDV
                    {
                        OperacionId = lector.GetInt32(0),
                        Descripcion = lector.IsDBNull(1) ? null : lector.GetString(1),
                        PatenteEscencial = lector.IsDBNull(2) ? null : lector.GetString(2)
                    };
        }

        public void ActualizarDVHOperacion(int operacionId, string dvh)
        {
            EjecutarNonQuery("UPDATE Operacion SET DVH = @dvh WHERE OperacionID = @id",
                ("@dvh", dvh), ("@id", operacionId));
        }

        // ---------- DVV (una fila por tabla) ----------
        public void GuardarDVV(string tabla, string dvv, string idClave)
        {
            const string sql = @"
MERGE DVV AS destino
USING (SELECT @tabla AS Tabla) AS origen ON destino.Tabla = origen.Tabla
WHEN MATCHED THEN UPDATE SET DVV = @dvv, ClaveId = @claveId, FechaActualizacion = GETDATE()
WHEN NOT MATCHED THEN INSERT (Tabla, DVV, ClaveId, FechaActualizacion) VALUES (@tabla, @dvv, @claveId, GETDATE());";
            EjecutarNonQuery(sql, ("@tabla", tabla), ("@dvv", dvv), ("@claveId", idClave));
        }

        public DVVGuardado LeerDVV(string tabla)
        {
            const string sql = "SELECT DVV, ClaveId FROM DVV WHERE Tabla = @tabla";
            using (SqlConnection con = AbrirConexion())
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                com.Parameters.AddWithValue("@tabla", tabla);
                using (SqlDataReader lector = com.ExecuteReader())
                {
                    if (!lector.Read()) return null;
                    return new DVVGuardado { DVV = lector.GetString(0), ClaveId = lector.GetString(1) };
                }
            }
        }

        private void EjecutarNonQuery(string sql, params (string Nombre, object Valor)[] parametros)
        {
            using (SqlConnection con = AbrirConexion())
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                foreach ((string nombre, object valor) in parametros)
                    com.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
                com.ExecuteNonQuery();
            }
        }
    }

    public sealed class UsuarioParaDV { public int UsuarioId; public string Usuario; public string Clave; }
    public sealed class UsuarioOperacionParaDV { public int UsuarioId; public int OperacionId; public string Habilitado; }
    public sealed class BitacoraParaDV { public int BitacoraId; public int UsuarioId; public DateTime FechaYHora; }
    public sealed class PerfilUsuarioParaDV { public int PerfilUsuarioId; public string NombrePerfil; public string DescPerfil; }
    public sealed class OperacionParaDV { public int OperacionId; public string Descripcion; public string PatenteEscencial; }
    public sealed class DVVGuardado { public string DVV; public string ClaveId; }
}
