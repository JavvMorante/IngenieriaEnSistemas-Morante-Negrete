using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Mapper de SolicitudReposicion y SolicitudReposicionDetalle (CUN10).</summary>
    public class SolicitudReposicionDAL_60MN
    {
        /// <summary>
        /// Unidades de cada ítem ya incluidas en órdenes de compra de la misma solicitud. Las
        /// anuladas no cuentan; las rechazadas sí, porque se corrigen y vuelven a aprobación.
        /// </summary>
        private const string SelectDetalle = @"
SELECT d.SolicitudDetalleID, d.SolicitudID, d.ProductoID, p.Codigo, p.Nombre, pr.RazonSocial,
       d.StockActual, d.StockMinimo, d.CantidadPedida, p.PrecioCosto,
       COALESCE((SELECT SUM(od.Cantidad)
                   FROM OrdenCompraDetalle od JOIN OrdenCompra o ON o.OrdenCompraID = od.OrdenCompraID
                  WHERE o.SolicitudID = d.SolicitudID AND od.ProductoID = d.ProductoID AND o.Estado <> 'ANULADA'), 0) AS EnOrdenes
  FROM SolicitudReposicionDetalle d
  JOIN Productos p  ON p.ProductoID = d.ProductoID
  JOIN Proveedor pr ON pr.ProveedorID = p.ProveedorID";

        private const string SelectEncabezado = @"
SELECT s.SolicitudID, s.Numero, s.FechaHora, s.UsuarioID, u.Usuario, s.Estado, s.Observaciones
  FROM SolicitudReposicion s
  JOIN Usuario u ON u.UsuarioID = s.UsuarioID";

        /// <summary>Productos activos con alerta de reposición activa que todavía no se incluyeron en una solicitud.</summary>
        public List<SolicitudDetalle_60MN> ListarProductosEnAlerta()
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT p.ProductoID, p.Codigo, p.Nombre, pr.RazonSocial, p.StockActual, p.StockMinimo, p.PrecioCosto
  FROM Productos p
  JOIN Proveedor pr ON pr.ProveedorID = p.ProveedorID
 WHERE p.Activo = 1
   AND EXISTS (SELECT 1 FROM AlertaReposicion a WHERE a.ProductoID = p.ProductoID AND a.Activa = 1 AND a.Atendida = 0)
 ORDER BY p.Nombre, p.Lote",
                r => new SolicitudDetalle_60MN
                {
                    ProductoId = r.GetInt32(0), Codigo = r.GetString(1), Producto = r.GetString(2), ProveedorHabitual = r.GetString(3),
                    StockActual = r.GetInt32(4), StockMinimo = r.GetInt32(5), PrecioCostoActual = r.GetDecimal(6), EnAlerta = true
                });
        }

        /// <summary>
        /// Solicitudes vigentes (PENDIENTE o EN_COMPRA) u órdenes abiertas que ya incluyen alguno
        /// de los productos. Devuelve el producto y el documento que lo contiene (CUN10, advertencia).
        /// </summary>
        public List<(int ProductoId, string Documento)> ProductosYaEnCompra(IEnumerable<int> productoIds)
        {
            string ids = string.Join(",", productoIds.Distinct()); // enteros propios, no datos del usuario
            if (ids.Length == 0) return new List<(int, string)>();
            return Conexion_60MN.EjecutarLista($@"
SELECT d.ProductoID, 'la solicitud ' + s.Numero
  FROM SolicitudReposicionDetalle d JOIN SolicitudReposicion s ON s.SolicitudID = d.SolicitudID
 WHERE s.Estado IN ('PENDIENTE', 'EN_COMPRA') AND d.ProductoID IN ({ids})
UNION
SELECT od.ProductoID, 'la orden ' + o.Numero
  FROM OrdenCompraDetalle od JOIN OrdenCompra o ON o.OrdenCompraID = od.OrdenCompraID
 WHERE o.Estado NOT IN ('CERRADA', 'ANULADA') AND od.ProductoID IN ({ids})",
                r => (r.GetInt32(0), r.GetString(1)));
        }

        /// <summary>
        /// Graba la solicitud y su detalle y marca como atendidas las alertas de esos productos,
        /// en una sola transacción. Devuelve el Id de la solicitud y los Id del detalle.
        /// </summary>
        public (int SolicitudId, List<int> DetalleIds) Registrar(SolicitudReposicion_60MN solicitud)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();

            int id;
            using (SqlCommand enc = Conexion_60MN.Crear(@"
INSERT INTO SolicitudReposicion (FechaHora, UsuarioID, Estado, Observaciones)
VALUES (@fecha, @usuario, 'PENDIENTE', @observaciones);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@fecha", solicitud.FechaHora), ("@usuario", solicitud.UsuarioId), ("@observaciones", solicitud.Observaciones)))
                id = Convert.ToInt32(enc.ExecuteScalar());

            List<int> detalleIds = new List<int>();
            foreach (SolicitudDetalle_60MN d in solicitud.Detalle)
            {
                using (SqlCommand det = Conexion_60MN.Crear(@"
INSERT INTO SolicitudReposicionDetalle (SolicitudID, ProductoID, StockActual, StockMinimo, CantidadPedida)
VALUES (@solicitud, @producto, @stock, @minimo, @cantidad);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                    ("@solicitud", id), ("@producto", d.ProductoId), ("@stock", d.StockActual), ("@minimo", d.StockMinimo),
                    ("@cantidad", d.CantidadPedida)))
                    detalleIds.Add(Convert.ToInt32(det.ExecuteScalar()));

                using SqlCommand alerta = Conexion_60MN.Crear(
                    "UPDATE AlertaReposicion SET Atendida = 1 WHERE ProductoID = @producto AND Activa = 1 AND Atendida = 0",
                    con, tx, ("@producto", d.ProductoId));
                alerta.ExecuteNonQuery();
            }
            tx.Commit();
            return (id, detalleIds);
        }

        /// <summary>Solicitudes en PENDIENTE o EN_COMPRA con su detalle (CUN11 paso 1).</summary>
        public List<SolicitudReposicion_60MN> ListarVigentes()
        {
            List<SolicitudReposicion_60MN> solicitudes = Conexion_60MN.EjecutarLista(
                SelectEncabezado + " WHERE s.Estado IN ('PENDIENTE', 'EN_COMPRA') ORDER BY s.FechaHora, s.SolicitudID", Mapear);
            CargarDetalle(solicitudes);
            return solicitudes;
        }

        public SolicitudReposicion_60MN? ObtenerPorId(int id)
        {
            List<SolicitudReposicion_60MN> solicitudes = Conexion_60MN.EjecutarLista(SelectEncabezado + " WHERE s.SolicitudID = @id", Mapear, ("@id", id));
            CargarDetalle(solicitudes);
            return solicitudes.FirstOrDefault();
        }

        private static void CargarDetalle(List<SolicitudReposicion_60MN> solicitudes)
        {
            if (solicitudes.Count == 0) return;
            Dictionary<int, SolicitudReposicion_60MN> porId = solicitudes.ToDictionary(s => s.Id);
            string ids = string.Join(",", porId.Keys); // enteros propios, no datos del usuario
            List<SolicitudDetalle_60MN> detalle = Conexion_60MN.EjecutarLista(
                SelectDetalle + $" WHERE d.SolicitudID IN ({ids}) ORDER BY d.SolicitudDetalleID",
                r => new SolicitudDetalle_60MN
                {
                    Id = r.GetInt32(0), SolicitudId = r.GetInt32(1), ProductoId = r.GetInt32(2), Codigo = r.GetString(3),
                    Producto = r.GetString(4), ProveedorHabitual = r.GetString(5), StockActual = r.GetInt32(6),
                    StockMinimo = r.GetInt32(7), CantidadPedida = r.GetInt32(8), PrecioCostoActual = r.GetDecimal(9),
                    CantidadEnOrdenes = r.GetInt32(10)
                });
            foreach (SolicitudDetalle_60MN d in detalle)
                porId[d.SolicitudId].Detalle.Add(d);
        }

        private static SolicitudReposicion_60MN Mapear(SqlDataReader r) => new SolicitudReposicion_60MN
        {
            Id = r.GetInt32(0),
            Numero = r.GetString(1),
            FechaHora = r.GetDateTime(2),
            UsuarioId = r.GetInt32(3),
            Usuario = r.GetString(4),
            Estado = EstadosCompra_60MN.SolicitudDesdeCodigo(r.GetString(5)),
            Observaciones = r.TextoONulo("Observaciones")
        };
    }
}
