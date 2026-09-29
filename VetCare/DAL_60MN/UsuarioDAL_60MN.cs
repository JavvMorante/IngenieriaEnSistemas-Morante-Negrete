using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>
    /// Mapper de la tabla Usuario. Trabaja con los valores tal como se guardan
    /// (DNI y Email cifrados, Clave hasheada): cifrar y descifrar es
    /// responsabilidad de la BLL a través de Seguridad_60MN.
    /// </summary>
    public class UsuarioDAL_60MN
    {
        private const string SelectBase = @"
SELECT u.UsuarioID, u.Usuario, u.Clave, u.Nombre, u.Apellido, u.DNI, u.Email,
       u.Activo, u.Bloqueado, u.IntentosFallidos, u.PrimerIngreso, u.EnSesion
  FROM Usuario u";

        public List<Usuario_60MN> ListarTodos()
        {
            return Conexion_60MN.EjecutarLista(SelectBase + " ORDER BY u.Apellido, u.Nombre", Mapear);
        }

        public Usuario_60MN? ObtenerPorId(int id)
        {
            return Conexion_60MN.EjecutarLista(SelectBase + " WHERE u.UsuarioID = @id", Mapear, ("@id", id)).FirstOrDefault();
        }

        public Usuario_60MN? ObtenerPorNombreUsuario(string nombreUsuario)
        {
            return Conexion_60MN.EjecutarLista(SelectBase + " WHERE u.Usuario = @usuario", Mapear, ("@usuario", nombreUsuario)).FirstOrDefault();
        }

        public bool ExisteNombreUsuario(string nombreUsuario, int excluirId = 0)
        {
            object? cantidad = Conexion_60MN.EjecutarEscalar(
                "SELECT COUNT(*) FROM Usuario WHERE Usuario = @usuario AND UsuarioID <> @id",
                ("@usuario", nombreUsuario), ("@id", excluirId));
            return Convert.ToInt32(cantidad) > 0;
        }

        public int Insertar(Usuario_60MN u)
        {
            const string sql = @"
INSERT INTO Usuario (Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion)
VALUES (@usuario, @clave, @nombre, @apellido, @dni, @email, 1, 0, 0, 1, 0);
SELECT CAST(SCOPE_IDENTITY() AS int);";
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(sql,
                ("@usuario", u.NombreUsuario), ("@clave", u.ClaveHash), ("@nombre", u.Nombre), ("@apellido", u.Apellido),
                ("@dni", u.Dni), ("@email", u.Email)));
        }

        /// <summary>Datos editables en CUS02: apellido, nombre y mail. Las familias y patentes se asignan con PermisoDAL_60MN.</summary>
        public void ModificarDatos(Usuario_60MN u)
        {
            Conexion_60MN.EjecutarNonQuery(
                "UPDATE Usuario SET Nombre = @nombre, Apellido = @apellido, Email = @email WHERE UsuarioID = @id",
                ("@nombre", u.Nombre), ("@apellido", u.Apellido), ("@email", u.Email), ("@id", u.Id));
        }

        /// <summary>Persiste los campos de control de acceso (login, bloqueo, baja, cambio de clave).</summary>
        public void ActualizarEstado(Usuario_60MN u)
        {
            Conexion_60MN.EjecutarNonQuery(@"
UPDATE Usuario
   SET Clave = @clave, Activo = @activo, Bloqueado = @bloqueado, IntentosFallidos = @intentos,
       PrimerIngreso = @primerIngreso, EnSesion = @enSesion
 WHERE UsuarioID = @id",
                ("@clave", u.ClaveHash), ("@activo", u.Activo), ("@bloqueado", u.Bloqueado), ("@intentos", u.IntentosFallidos),
                ("@primerIngreso", u.PrimerIngreso), ("@enSesion", u.EnSesion), ("@id", u.Id));
        }

        private static Usuario_60MN Mapear(SqlDataReader r) => new Usuario_60MN
        {
            Id = r.GetInt32(r.GetOrdinal("UsuarioID")),
            NombreUsuario = r.GetString(r.GetOrdinal("Usuario")),
            ClaveHash = r.GetString(r.GetOrdinal("Clave")),
            Nombre = r.GetString(r.GetOrdinal("Nombre")),
            Apellido = r.GetString(r.GetOrdinal("Apellido")),
            Dni = r.GetString(r.GetOrdinal("DNI")),
            Email = r.GetString(r.GetOrdinal("Email")),
            Activo = r.GetBoolean(r.GetOrdinal("Activo")),
            Bloqueado = r.GetBoolean(r.GetOrdinal("Bloqueado")),
            IntentosFallidos = r.GetInt32(r.GetOrdinal("IntentosFallidos")),
            PrimerIngreso = r.GetBoolean(r.GetOrdinal("PrimerIngreso")),
            EnSesion = r.GetBoolean(r.GetOrdinal("EnSesion"))
        };
    }
}
