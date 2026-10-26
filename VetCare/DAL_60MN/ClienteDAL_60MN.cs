using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Criterio de búsqueda de CUN03 Consultar Cliente.</summary>
    public enum CriterioBusquedaCliente_60MN
    {
        Documento,
        Apellido,
        NombreMascota
    }

    /// <summary>Mapper de las tablas Cliente y Mascota (CUN01 a CUN04).</summary>
    public class ClienteDAL_60MN
    {
        private const string SelectCliente = @"
SELECT c.ClienteID, c.TipoDocumento, c.NroDocumento, c.Nombre, c.Apellido, c.Direccion, c.Telefono, c.Email, c.Activo
  FROM Cliente c";

        private const string SelectMascota = @"
SELECT m.MascotaID, m.ClienteID, m.Nombre, m.Especie, m.Raza, m.Sexo, m.FechaNacimiento, m.Activo, h.Numero
  FROM Mascota m
  LEFT JOIN HistoriaClinica h ON h.MascotaID = m.MascotaID";

        // ------------------------------- Clientes -------------------------------

        public List<Cliente_60MN> Buscar(CriterioBusquedaCliente_60MN criterio, string valor, bool incluirBajas)
        {
            string condicion = criterio switch
            {
                CriterioBusquedaCliente_60MN.Documento => "c.NroDocumento LIKE @patron",
                CriterioBusquedaCliente_60MN.Apellido => "c.Apellido LIKE @patron",
                _ => "EXISTS (SELECT 1 FROM Mascota m WHERE m.ClienteID = c.ClienteID AND m.Nombre LIKE @patron)"
            };
            string sql = SelectCliente + $" WHERE ({condicion}) AND (@bajas = 1 OR c.Activo = 1) ORDER BY c.Apellido, c.Nombre";
            return Conexion_60MN.EjecutarLista(sql, MapearCliente, ("@patron", "%" + valor.Trim() + "%"), ("@bajas", incluirBajas));
        }

        public Cliente_60MN? ObtenerPorId(int id)
        {
            Cliente_60MN? cliente = Conexion_60MN.EjecutarLista(SelectCliente + " WHERE c.ClienteID = @id", MapearCliente, ("@id", id)).FirstOrDefault();
            if (cliente != null)
                cliente.Mascotas.AddRange(ListarMascotas(id, incluirBajas: true));
            return cliente;
        }

        public Cliente_60MN? ObtenerPorDocumento(string tipo, string numero)
        {
            return Conexion_60MN.EjecutarLista(SelectCliente + " WHERE c.TipoDocumento = @tipo AND c.NroDocumento = @nro",
                MapearCliente, ("@tipo", tipo), ("@nro", numero)).FirstOrDefault();
        }

        /// <summary>CUN01: alta del cliente y de sus mascotas en una sola transacción. Devuelve los Id de las mascotas.</summary>
        public List<int> Insertar(Cliente_60MN c, IEnumerable<Mascota_60MN> mascotas)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            using (SqlCommand com = Conexion_60MN.Crear(@"
INSERT INTO Cliente (TipoDocumento, NroDocumento, Nombre, Apellido, Direccion, Telefono, Email, Activo)
VALUES (@tipo, @nro, @nombre, @apellido, @direccion, @telefono, @email, 1);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@tipo", c.TipoDocumento), ("@nro", c.NroDocumento), ("@nombre", c.Nombre), ("@apellido", c.Apellido),
                ("@direccion", c.Direccion), ("@telefono", c.Telefono), ("@email", c.Email)))
                c.Id = Convert.ToInt32(com.ExecuteScalar());

            List<int> ids = new List<int>();
            foreach (Mascota_60MN m in mascotas)
            {
                m.ClienteId = c.Id;
                m.Id = InsertarMascota(m, con, tx);
                ids.Add(m.Id);
            }
            tx.Commit();
            return ids;
        }

        /// <summary>CUN02: datos de contacto y filiatorios. El tipo y número de documento no se modifican.</summary>
        public void Modificar(Cliente_60MN c)
        {
            Conexion_60MN.EjecutarNonQuery(@"
UPDATE Cliente SET Nombre = @nombre, Apellido = @apellido, Direccion = @direccion, Telefono = @telefono, Email = @email
 WHERE ClienteID = @id",
                ("@nombre", c.Nombre), ("@apellido", c.Apellido), ("@direccion", c.Direccion),
                ("@telefono", c.Telefono), ("@email", c.Email), ("@id", c.Id));
        }

        public void CambiarActivoCliente(int id, bool activo)
        {
            Conexion_60MN.EjecutarNonQuery("UPDATE Cliente SET Activo = @activo WHERE ClienteID = @id", ("@activo", activo), ("@id", id));
        }

        // ------------------------------- Mascotas -------------------------------

        public List<Mascota_60MN> ListarMascotas(int clienteId, bool incluirBajas)
        {
            return Conexion_60MN.EjecutarLista(SelectMascota + " WHERE m.ClienteID = @id AND (@bajas = 1 OR m.Activo = 1) ORDER BY m.Nombre",
                MapearMascota, ("@id", clienteId), ("@bajas", incluirBajas));
        }

        public Mascota_60MN? ObtenerMascota(int id)
        {
            return Conexion_60MN.EjecutarLista(SelectMascota + " WHERE m.MascotaID = @id", MapearMascota, ("@id", id)).FirstOrDefault();
        }

        public bool ExisteMascotaConNombre(int clienteId, string nombre, int excluirId)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(
                "SELECT COUNT(*) FROM Mascota WHERE ClienteID = @c AND Nombre = @n AND MascotaID <> @id AND Activo = 1",
                ("@c", clienteId), ("@n", nombre), ("@id", excluirId))) > 0;
        }

        public int InsertarMascota(Mascota_60MN m)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            return InsertarMascota(m, con, null);
        }

        public void ModificarMascota(Mascota_60MN m)
        {
            Conexion_60MN.EjecutarNonQuery(@"
UPDATE Mascota SET Nombre = @nombre, Especie = @especie, Raza = @raza, Sexo = @sexo, FechaNacimiento = @nacimiento
 WHERE MascotaID = @id",
                ("@nombre", m.Nombre), ("@especie", m.Especie), ("@raza", m.Raza), ("@sexo", m.Sexo),
                ("@nacimiento", m.FechaNacimiento.Date), ("@id", m.Id));
        }

        public void CambiarActivoMascota(int id, bool activo)
        {
            Conexion_60MN.EjecutarNonQuery("UPDATE Mascota SET Activo = @activo WHERE MascotaID = @id", ("@activo", activo), ("@id", id));
        }

        private static int InsertarMascota(Mascota_60MN m, SqlConnection con, SqlTransaction? tx)
        {
            using SqlCommand com = Conexion_60MN.Crear(@"
INSERT INTO Mascota (ClienteID, Nombre, Especie, Raza, Sexo, FechaNacimiento, Activo)
VALUES (@cliente, @nombre, @especie, @raza, @sexo, @nacimiento, 1);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@cliente", m.ClienteId), ("@nombre", m.Nombre), ("@especie", m.Especie), ("@raza", m.Raza),
                ("@sexo", m.Sexo), ("@nacimiento", m.FechaNacimiento.Date));
            return Convert.ToInt32(com.ExecuteScalar());
        }

        private static Cliente_60MN MapearCliente(SqlDataReader r) => new Cliente_60MN
        {
            Id = r.GetInt32(0),
            TipoDocumento = r.GetString(1),
            NroDocumento = r.GetString(2),
            Nombre = r.GetString(3),
            Apellido = r.GetString(4),
            Direccion = r.GetString(5),
            Telefono = r.GetString(6),
            Email = r.IsDBNull(7) ? null : r.GetString(7),
            Activo = r.GetBoolean(8)
        };

        private static Mascota_60MN MapearMascota(SqlDataReader r) => new Mascota_60MN
        {
            Id = r.GetInt32(0),
            ClienteId = r.GetInt32(1),
            Nombre = r.GetString(2),
            Especie = r.GetString(3),
            Raza = r.GetString(4),
            Sexo = r.GetString(5),
            FechaNacimiento = r.GetDateTime(6),
            Activo = r.GetBoolean(7),
            NumeroHistoriaClinica = r.IsDBNull(8) ? string.Empty : r.GetString(8)
        };
    }
}
