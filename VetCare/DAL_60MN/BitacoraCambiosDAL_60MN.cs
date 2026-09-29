using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Consulta de la tabla Productos_C que alimenta el trigger (CUS10). Es de solo lectura.</summary>
    public class BitacoraCambiosDAL_60MN
    {
        private const string SelectBase = @"
SELECT h.ProductoCID, h.ProductoID, h.Codigo, h.Nombre, h.CategoriaID, c.Nombre AS Categoria, h.ProveedorID, pr.RazonSocial AS Proveedor,
       h.StockActual, h.StockMinimo, h.PrecioCosto, h.PrecioVenta, h.FechaVencimiento, h.Lote, h.ActivoProducto,
       h.Operacion, h.FechaHora, COALESCE(u.Usuario, '(desconocido)') AS Usuario, h.Act
  FROM Productos_C h
  LEFT JOIN Categoria c  ON c.CategoriaID = h.CategoriaID
  LEFT JOIN Proveedor pr ON pr.ProveedorID = h.ProveedorID
  LEFT JOIN Usuario u    ON u.UsuarioID = h.UsuarioID";

        public List<ProductoHistorial_60MN> Consultar(DateTime? desde, DateTime? hasta, string? codigo, string? nombre)
        {
            string sql = SelectBase + @"
 WHERE (@desde IS NULL OR h.FechaHora >= @desde)
   AND (@hasta IS NULL OR h.FechaHora < @hasta)
   AND (@codigo IS NULL OR h.Codigo LIKE @codigo)
   AND (@nombre IS NULL OR h.Nombre LIKE @nombre)
 ORDER BY h.ProductoID, h.FechaHora DESC, h.ProductoCID DESC";
            return Conexion_60MN.EjecutarLista(sql, Mapear,
                ("@desde", desde?.Date), ("@hasta", hasta?.Date.AddDays(1)),
                ("@codigo", string.IsNullOrWhiteSpace(codigo) ? null : "%" + codigo.Trim() + "%"),
                ("@nombre", string.IsNullOrWhiteSpace(nombre) ? null : "%" + nombre.Trim() + "%"));
        }

        public ProductoHistorial_60MN? ObtenerVersion(int productoCId)
        {
            return Conexion_60MN.EjecutarLista(SelectBase + " WHERE h.ProductoCID = @id", Mapear, ("@id", productoCId)).FirstOrDefault();
        }

        private static ProductoHistorial_60MN Mapear(SqlDataReader r) => new ProductoHistorial_60MN
        {
            Id = r.GetInt32(0),
            ProductoId = r.GetInt32(1),
            Codigo = r.GetString(2),
            Nombre = r.GetString(3),
            CategoriaId = r.GetInt32(4),
            Categoria = r.TextoONulo("Categoria") ?? string.Empty,
            ProveedorId = r.GetInt32(6),
            Proveedor = r.TextoONulo("Proveedor") ?? string.Empty,
            StockActual = r.GetInt32(8),
            StockMinimo = r.GetInt32(9),
            PrecioCosto = r.GetDecimal(10),
            PrecioVenta = r.GetDecimal(11),
            FechaVencimiento = r.GetDateTime(12),
            Lote = r.GetString(13),
            ActivoProducto = r.GetBoolean(14),
            Operacion = r.GetString(15),
            FechaHora = r.GetDateTime(16),
            Usuario = r.GetString(17),
            Act = r.GetBoolean(18)
        };
    }
}
