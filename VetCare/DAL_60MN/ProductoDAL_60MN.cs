using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>
    /// Mapper de la tabla Productos. Cada alta/modificación deja el Id del
    /// usuario en UsuarioModificacion, que el trigger TR_Productos_BitacoraCambios
    /// copia a Productos_C.
    /// </summary>
    public class ProductoDAL_60MN
    {
        private const string SelectBase = @"
SELECT p.ProductoID, p.Codigo, p.Nombre, p.CategoriaID, c.Nombre AS Categoria, p.ProveedorID, pr.RazonSocial AS Proveedor,
       p.StockActual, p.StockMinimo, p.PrecioCosto, p.PrecioVenta, p.FechaVencimiento, p.Lote, p.Activo
  FROM Productos p
  JOIN Categoria c  ON c.CategoriaID = p.CategoriaID
  JOIN Proveedor pr ON pr.ProveedorID = p.ProveedorID";

        /// <summary>Búsqueda de CUN12 por código, nombre o categoría.</summary>
        public List<Producto_60MN> Buscar(string? texto, int? categoriaId, bool incluirInactivos)
        {
            string sql = SelectBase + @"
 WHERE (@texto IS NULL OR p.Codigo LIKE @patron OR p.Nombre LIKE @patron OR p.Lote LIKE @patron)
   AND (@categoria IS NULL OR p.CategoriaID = @categoria)
   AND (@inactivos = 1 OR p.Activo = 1)
 ORDER BY p.Nombre, p.Lote";
            string? limpio = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
            return Conexion_60MN.EjecutarLista(sql, Mapear,
                ("@texto", limpio), ("@patron", limpio == null ? null : "%" + limpio + "%"),
                ("@categoria", categoriaId), ("@inactivos", incluirInactivos));
        }

        public Producto_60MN? ObtenerPorId(int id)
        {
            return Conexion_60MN.EjecutarLista(SelectBase + " WHERE p.ProductoID = @id", Mapear, ("@id", id)).FirstOrDefault();
        }

        public Producto_60MN? ObtenerPorNombreYLote(string nombre, string lote, int excluirId = 0)
        {
            return Conexion_60MN.EjecutarLista(SelectBase + " WHERE p.Nombre = @nombre AND p.Lote = @lote AND p.ProductoID <> @id",
                Mapear, ("@nombre", nombre), ("@lote", lote), ("@id", excluirId)).FirstOrDefault();
        }

        public int Insertar(Producto_60MN p, int usuarioId)
        {
            const string sql = @"
INSERT INTO Productos (Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo, PrecioCosto, PrecioVenta, FechaVencimiento, Lote, Activo, UsuarioModificacion)
VALUES (@nombre, @categoria, @proveedor, @stock, @minimo, @costo, @venta, @vencimiento, @lote, 1, @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);";
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(sql,
                ("@nombre", p.Nombre), ("@categoria", p.CategoriaId), ("@proveedor", p.ProveedorId), ("@stock", p.StockActual),
                ("@minimo", p.StockMinimo), ("@costo", p.PrecioCosto), ("@venta", p.PrecioVenta),
                ("@vencimiento", p.FechaVencimiento.Date), ("@lote", p.Lote), ("@usuario", usuarioId)));
        }

        /// <summary>Modifica los datos comerciales (CUN12). El stock NO se toca acá: solo por movimientos (CUN14).</summary>
        public void Modificar(Producto_60MN p, int usuarioId)
        {
            Conexion_60MN.EjecutarNonQuery(@"
UPDATE Productos
   SET Nombre = @nombre, CategoriaID = @categoria, ProveedorID = @proveedor, StockMinimo = @minimo,
       PrecioCosto = @costo, PrecioVenta = @venta, FechaVencimiento = @vencimiento, Lote = @lote,
       UsuarioModificacion = @usuario
 WHERE ProductoID = @id",
                ("@nombre", p.Nombre), ("@categoria", p.CategoriaId), ("@proveedor", p.ProveedorId), ("@minimo", p.StockMinimo),
                ("@costo", p.PrecioCosto), ("@venta", p.PrecioVenta), ("@vencimiento", p.FechaVencimiento.Date), ("@lote", p.Lote),
                ("@usuario", usuarioId), ("@id", p.Id));
        }

        public void CambiarActivo(int id, bool activo, int usuarioId)
        {
            Conexion_60MN.EjecutarNonQuery(
                "UPDATE Productos SET Activo = @activo, UsuarioModificacion = @usuario WHERE ProductoID = @id",
                ("@activo", activo), ("@usuario", usuarioId), ("@id", id));
        }

        private static Producto_60MN Mapear(SqlDataReader r) => new Producto_60MN
        {
            Id = r.GetInt32(0),
            Codigo = r.GetString(1),
            Nombre = r.GetString(2),
            CategoriaId = r.GetInt32(3),
            CategoriaNombre = r.GetString(4),
            ProveedorId = r.GetInt32(5),
            ProveedorNombre = r.GetString(6),
            StockActual = r.GetInt32(7),
            StockMinimo = r.GetInt32(8),
            PrecioCosto = r.GetDecimal(9),
            PrecioVenta = r.GetDecimal(10),
            FechaVencimiento = r.GetDateTime(11),
            Lote = r.GetString(12),
            Activo = r.GetBoolean(13)
        };
    }
}
