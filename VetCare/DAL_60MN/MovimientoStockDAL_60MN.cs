using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>
    /// Consulta de MovimientoStock y mapper de AlertaReposicion. Los movimientos se graban dentro de
    /// la transacción de la venta (CUN05) o de la recepción de mercadería (CUN14).
    /// </summary>
    public class MovimientoStockDAL_60MN
    {
        public List<MovimientoStock_60MN> ListarPorProducto(int productoId)
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT MovimientoID, ProductoID, Tipo, Cantidad, StockAnterior, StockResultante, Motivo, FechaHora, UsuarioID
  FROM MovimientoStock WHERE ProductoID = @id ORDER BY FechaHora DESC, MovimientoID DESC",
                r => new MovimientoStock_60MN
                {
                    Id = r.GetInt32(0),
                    ProductoId = r.GetInt32(1),
                    Tipo = (TipoMovimiento_60MN)Enum.Parse(typeof(TipoMovimiento_60MN), r.GetString(2), ignoreCase: true),
                    Cantidad = r.GetInt32(3),
                    StockAnterior = r.GetInt32(4),
                    StockResultante = r.GetInt32(5),
                    Motivo = r.GetString(6),
                    FechaHora = r.GetDateTime(7),
                    UsuarioId = r.EnteroONulo("UsuarioID")
                }, ("@id", productoId));
        }

        public bool ExisteAlertaActiva(int productoId)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(
                "SELECT COUNT(*) FROM AlertaReposicion WHERE ProductoID = @id AND Activa = 1", ("@id", productoId))) > 0;
        }

        public void CrearAlerta(int productoId)
        {
            Conexion_60MN.EjecutarNonQuery("INSERT INTO AlertaReposicion (ProductoID, Activa) VALUES (@id, 1)", ("@id", productoId));
        }

        public void DesactivarAlertas(int productoId)
        {
            Conexion_60MN.EjecutarNonQuery("UPDATE AlertaReposicion SET Activa = 0 WHERE ProductoID = @id AND Activa = 1", ("@id", productoId));
        }

        public int ContarAlertasActivas()
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(
                "SELECT COUNT(*) FROM AlertaReposicion a JOIN Productos p ON p.ProductoID = a.ProductoID WHERE a.Activa = 1 AND p.Activo = 1"));
        }
    }
}
