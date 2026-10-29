using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    public enum CriterioBusquedaProveedor_60MN
    {
        RazonSocial,
        Cuit
    }

    /// <summary>Mapper de la tabla Proveedor (CUN11 Gestionar Proveedores).</summary>
    public class ProveedorDAL_60MN
    {
        private const string Select = @"
SELECT pr.ProveedorID, pr.RazonSocial, pr.CUIT, pr.Telefono, pr.Email, pr.Direccion, pr.Contacto, pr.Activo,
       (SELECT COUNT(*) FROM Productos p WHERE p.ProveedorID = pr.ProveedorID AND p.Activo = 1) AS ProductosActivos
  FROM Proveedor pr";

        /// <summary>Con valor vacío lista todos (los proveedores son pocos).</summary>
        public List<Proveedor_60MN> Buscar(CriterioBusquedaProveedor_60MN criterio, string valor, bool incluirBajas)
        {
            string condicion = criterio == CriterioBusquedaProveedor_60MN.Cuit ? "pr.CUIT LIKE @patron" : "pr.RazonSocial LIKE @patron";
            string sql = Select + $" WHERE ({condicion}) AND (@bajas = 1 OR pr.Activo = 1) ORDER BY pr.RazonSocial";
            return Conexion_60MN.EjecutarLista(sql, Mapear, ("@patron", "%" + valor.Trim() + "%"), ("@bajas", incluirBajas));
        }

        public Proveedor_60MN? ObtenerPorId(int id) =>
            Conexion_60MN.EjecutarLista(Select + " WHERE pr.ProveedorID = @id", Mapear, ("@id", id)).FirstOrDefault();

        public Proveedor_60MN? ObtenerPorCuit(string cuit) =>
            Conexion_60MN.EjecutarLista(Select + " WHERE pr.CUIT = @cuit", Mapear, ("@cuit", cuit)).FirstOrDefault();

        public int Insertar(Proveedor_60MN p)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(@"
INSERT INTO Proveedor (RazonSocial, CUIT, Telefono, Email, Direccion, Contacto, Activo)
VALUES (@razon, @cuit, @telefono, @email, @direccion, @contacto, 1);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                ("@razon", p.RazonSocial), ("@cuit", p.Cuit), ("@telefono", p.Telefono), ("@email", p.Email),
                ("@direccion", p.Direccion), ("@contacto", p.Contacto)));
        }

        /// <summary>El CUIT no se modifica.</summary>
        public void Modificar(Proveedor_60MN p)
        {
            Conexion_60MN.EjecutarNonQuery(@"
UPDATE Proveedor
   SET RazonSocial = @razon, Telefono = @telefono, Email = @email, Direccion = @direccion, Contacto = @contacto
 WHERE ProveedorID = @id",
                ("@razon", p.RazonSocial), ("@telefono", p.Telefono), ("@email", p.Email),
                ("@direccion", p.Direccion), ("@contacto", p.Contacto), ("@id", p.Id));
        }

        public void CambiarActivo(int id, bool activo) =>
            Conexion_60MN.EjecutarNonQuery("UPDATE Proveedor SET Activo = @activo WHERE ProveedorID = @id", ("@activo", activo), ("@id", id));

        /// <summary>"Productos que provee": activos y dados de baja.</summary>
        public List<Producto_60MN> ListarProductos(int proveedorId)
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT p.ProductoID, p.Codigo, p.Nombre, c.Nombre AS Categoria, p.StockActual, p.StockMinimo, p.FechaVencimiento, p.Lote, p.Activo
  FROM Productos p
  JOIN Categoria c ON c.CategoriaID = p.CategoriaID
 WHERE p.ProveedorID = @id
 ORDER BY p.Activo DESC, p.Nombre", r => new Producto_60MN
            {
                Id = r.GetInt32(0),
                Codigo = r.GetString(1),
                Nombre = r.GetString(2),
                CategoriaNombre = r.GetString(3),
                ProveedorId = proveedorId,
                StockActual = r.GetInt32(4),
                StockMinimo = r.GetInt32(5),
                FechaVencimiento = r.GetDateTime(6),
                Lote = r.GetString(7),
                Activo = r.GetBoolean(8)
            }, ("@id", proveedorId));
        }

        private static Proveedor_60MN Mapear(SqlDataReader r) => new Proveedor_60MN
        {
            Id = r.GetInt32(0),
            RazonSocial = r.GetString(1),
            Cuit = r.GetString(2),
            Telefono = r.GetString(3),
            Email = r.IsDBNull(4) ? null : r.GetString(4),
            Direccion = r.GetString(5),
            Contacto = r.IsDBNull(6) ? null : r.GetString(6),
            Activo = r.GetBoolean(7),
            ProductosActivos = r.GetInt32(8)
        };
    }
}
