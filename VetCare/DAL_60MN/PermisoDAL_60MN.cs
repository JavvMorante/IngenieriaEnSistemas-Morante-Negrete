using Microsoft.Data.SqlClient;
using Servicios_60MN.Composite;

namespace DAL_60MN
{
    /// <summary>Asignaciones directas de un usuario: sus familias y sus patentes individuales.</summary>
    public class PermisosUsuario_60MN
    {
        public List<int> Familias { get; } = new List<int>();

        public List<int> Patentes { get; } = new List<int>();
    }

    /// <summary>
    /// Mapper del modelo de permisos Usuario - Familia - Patente: tablas Patente,
    /// Familia, FamiliaPatente, FamiliaFamilia, UsuarioFamilia y UsuarioPatente.
    /// Reconstruye el árbol Composite (familias con sus patentes y subfamilias).
    /// </summary>
    public class PermisoDAL_60MN
    {
        public Dictionary<int, Patente_60MN> CargarPatentes()
        {
            return Conexion_60MN.EjecutarLista("SELECT PatenteID, Nombre, Codigo FROM Patente ORDER BY PatenteID",
                    r => new Patente_60MN { Id = r.GetInt32(0), Nombre = r.GetString(1), Codigo = r.GetString(2) })
                .ToDictionary(p => p.Id);
        }

        /// <summary>
        /// Carga todas las familias con sus hijos ya enlazados: patentes (FamiliaPatente)
        /// y subfamilias (FamiliaFamilia). Las familias comparten las mismas instancias de patentes.
        /// </summary>
        public Dictionary<int, Familia_60MN> CargarFamilias(Dictionary<int, Patente_60MN> patentes)
        {
            Dictionary<int, Familia_60MN> familias = Conexion_60MN.EjecutarLista(
                    "SELECT FamiliaID, Nombre FROM Familia ORDER BY Nombre",
                    r => new Familia_60MN { Id = r.GetInt32(0), Nombre = r.GetString(1) })
                .ToDictionary(f => f.Id);

            foreach ((int familia, int patente) in LeerPares("SELECT FamiliaID, PatenteID FROM FamiliaPatente ORDER BY FamiliaID, PatenteID"))
                if (familias.TryGetValue(familia, out Familia_60MN? f) && patentes.TryGetValue(patente, out Patente_60MN? p))
                    f.ObtenerHijos().Add(p);

            foreach ((int padre, int hija) in LeerPares("SELECT FamiliaPadreID, FamiliaHijaID FROM FamiliaFamilia ORDER BY FamiliaPadreID, FamiliaHijaID"))
                if (familias.TryGetValue(padre, out Familia_60MN? p) && familias.TryGetValue(hija, out Familia_60MN? h))
                    p.ObtenerHijos().Add(h); // se agrega directo: el ciclo ya se valida al guardar

            return familias;
        }

        /// <summary>Familias y patentes individuales asignadas a cada usuario (UsuarioFamilia / UsuarioPatente).</summary>
        public Dictionary<int, PermisosUsuario_60MN> CargarPermisosDeUsuarios()
        {
            Dictionary<int, PermisosUsuario_60MN> resultado = new Dictionary<int, PermisosUsuario_60MN>();
            PermisosUsuario_60MN De(int usuarioId) =>
                resultado.TryGetValue(usuarioId, out PermisosUsuario_60MN? p) ? p : resultado[usuarioId] = new PermisosUsuario_60MN();

            foreach ((int usuario, int familia) in LeerPares("SELECT UsuarioID, FamiliaID FROM UsuarioFamilia"))
                De(usuario).Familias.Add(familia);
            foreach ((int usuario, int patente) in LeerPares("SELECT UsuarioID, PatenteID FROM UsuarioPatente"))
                De(usuario).Patentes.Add(patente);
            return resultado;
        }

        /// <summary>Reemplaza en una sola transacción las familias y patentes individuales de un usuario.</summary>
        public void AsignarPermisosUsuario(int usuarioId, IEnumerable<int> familias, IEnumerable<int> patentes)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            Ejecutar(con, tx, "DELETE FROM UsuarioFamilia WHERE UsuarioID = @u", ("@u", usuarioId));
            Ejecutar(con, tx, "DELETE FROM UsuarioPatente WHERE UsuarioID = @u", ("@u", usuarioId));
            foreach (int familia in familias.Distinct())
                Ejecutar(con, tx, "INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID) VALUES (@u, @f)", ("@u", usuarioId), ("@f", familia));
            foreach (int patente in patentes.Distinct())
                Ejecutar(con, tx, "INSERT INTO UsuarioPatente (UsuarioID, PatenteID) VALUES (@u, @p)", ("@u", usuarioId), ("@p", patente));
            tx.Commit();
        }

        public bool ExisteNombreFamilia(string nombre, int excluirId = 0)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(
                "SELECT COUNT(*) FROM Familia WHERE Nombre = @nombre AND FamiliaID <> @id",
                ("@nombre", nombre), ("@id", excluirId))) > 0;
        }

        public int InsertarFamilia(string nombre)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(
                "INSERT INTO Familia (Nombre) VALUES (@nombre); SELECT CAST(SCOPE_IDENTITY() AS int);",
                ("@nombre", nombre)));
        }

        public void RenombrarFamilia(int id, string nombre)
        {
            Conexion_60MN.EjecutarNonQuery("UPDATE Familia SET Nombre = @nombre WHERE FamiliaID = @id", ("@nombre", nombre), ("@id", id));
        }

        /// <summary>Reemplaza en una sola transacción las patentes y subfamilias de una familia.</summary>
        public void ReemplazarComposicion(int familiaId, IEnumerable<int> patentes, IEnumerable<int> subfamilias)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            Ejecutar(con, tx, "DELETE FROM FamiliaPatente WHERE FamiliaID = @f", ("@f", familiaId));
            Ejecutar(con, tx, "DELETE FROM FamiliaFamilia WHERE FamiliaPadreID = @f", ("@f", familiaId));
            foreach (int patente in patentes.Distinct())
                Ejecutar(con, tx, "INSERT INTO FamiliaPatente (FamiliaID, PatenteID) VALUES (@f, @p)", ("@f", familiaId), ("@p", patente));
            foreach (int hija in subfamilias.Distinct())
                Ejecutar(con, tx, "INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID) VALUES (@f, @h)", ("@f", familiaId), ("@h", hija));
            tx.Commit();
        }

        public void EliminarFamilia(int id)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            Ejecutar(con, tx, "DELETE FROM FamiliaPatente WHERE FamiliaID = @f", ("@f", id));
            Ejecutar(con, tx, "DELETE FROM FamiliaFamilia WHERE FamiliaPadreID = @f OR FamiliaHijaID = @f", ("@f", id));
            Ejecutar(con, tx, "DELETE FROM Familia WHERE FamiliaID = @f", ("@f", id));
            tx.Commit();
        }

        public int ContarUsuariosConFamilia(int familiaId, bool soloActivos)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(@"
SELECT COUNT(*) FROM UsuarioFamilia uf JOIN Usuario u ON u.UsuarioID = uf.UsuarioID
 WHERE uf.FamiliaID = @f AND (@soloActivos = 0 OR u.Activo = 1)",
                ("@f", familiaId), ("@soloActivos", soloActivos)));
        }

        private static List<(int A, int B)> LeerPares(string sql) =>
            Conexion_60MN.EjecutarLista(sql, r => (r.GetInt32(0), r.GetInt32(1)));

        private static void Ejecutar(SqlConnection con, SqlTransaction tx, string sql, params (string Nombre, object? Valor)[] parametros)
        {
            using SqlCommand com = Conexion_60MN.Crear(sql, con, tx, parametros);
            com.ExecuteNonQuery();
        }
    }
}
