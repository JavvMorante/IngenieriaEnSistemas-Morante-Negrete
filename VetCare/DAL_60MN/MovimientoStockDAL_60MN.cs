using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Mapper de MovimientoStock y AlertaReposicion (CUN14).</summary>
    public class MovimientoStockDAL_60MN
    {
        /// <summary>
        /// Registra el movimiento y actualiza el stock del producto en una sola
        /// transacción. La actualización se condiciona al stock leído
        /// (StockActual = @anterior) para no pisar un movimiento concurrente.
        /// Devuelve el Id del movimiento, o 0 si el stock cambió mientras tanto.
        /// </summary>
        public int Registrar(MovimientoStock_60MN m, bool darDeBajaProducto)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();

            string sqlProducto = darDeBajaProducto
                ? "UPDATE Productos SET StockActual = @nuevo, Activo = 0, UsuarioModificacion = @usuario WHERE ProductoID = @id AND StockActual = @anterior"
                : "UPDATE Productos SET StockActual = @nuevo, UsuarioModificacion = @usuario WHERE ProductoID = @id AND StockActual = @anterior";
            using (SqlCommand producto = Conexion_60MN.Crear(sqlProducto, con, tx,
                ("@nuevo", m.StockResultante), ("@usuario", m.UsuarioId), ("@id", m.ProductoId), ("@anterior", m.StockAnterior)))
            {
                if (producto.ExecuteNonQuery() == 0)
                {
                    tx.Rollback();
                    return 0;
                }
            }

            int id;
            using (SqlCommand movimiento = Conexion_60MN.Crear(@"
INSERT INTO MovimientoStock (ProductoID, Tipo, Cantidad, StockAnterior, StockResultante, Motivo, FechaHora, UsuarioID)
VALUES (@producto, @tipo, @cantidad, @anterior, @resultante, @motivo, @fecha, @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@producto", m.ProductoId), ("@tipo", m.Tipo.ToString().ToUpperInvariant()), ("@cantidad", m.Cantidad),
                ("@anterior", m.StockAnterior), ("@resultante", m.StockResultante), ("@motivo", m.Motivo),
                ("@fecha", m.FechaHora), ("@usuario", m.UsuarioId)))
            {
                id = Convert.ToInt32(movimiento.ExecuteScalar());
            }

            tx.Commit();
            return id;
        }

        public List<MovimientoStock_60MN> ListarPorProducto(int productoId)
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT MovimientoID, ProductoID, Tipo, Cantidad, StockAnterior, StockResultante, Motivo, FechaHora, UsuarioID
  FROM MovimientoStock WHERE ProductoID = @id ORDER BY FechaHora DESC, MovimientoID DESC",
                r => new MovimientoStock_60MN
                {
                    Id = r.GetInt32(0),
                    ProductoId = r.GetInt32(1),
                    Tipo = Enum.Parse<TipoMovimiento_60MN>(r.GetString(2), ignoreCase: true),
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
