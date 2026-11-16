using Entidades_60MN;

namespace DAL_60MN
{
    /// <summary>Consulta de CUN15 Generar Reporte de Compras.</summary>
    public class ReporteComprasDAL_60MN
    {
        /// <summary>Órdenes del período [desde, hasta) con lo pagado, lo recibido y la fecha de la última recepción.</summary>
        public List<FilaReporteCompra_60MN> Consultar(DateTime desde, DateTime hasta, int? proveedorId, EstadoOrden_60MN? estado)
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT o.OrdenCompraID, o.Numero, o.FechaHora, o.ProveedorID, pr.RazonSocial, o.Estado, o.Total,
       COALESCE(pg.Importe, 0) AS Pagado,
       (SELECT MAX(r.FechaHora) FROM Recepcion r WHERE r.OrdenCompraID = o.OrdenCompraID) AS UltimaRecepcion,
       (SELECT SUM(d.Cantidad) FROM OrdenCompraDetalle d WHERE d.OrdenCompraID = o.OrdenCompraID) AS Pedidas,
       (SELECT SUM(d.CantidadRecibida) FROM OrdenCompraDetalle d WHERE d.OrdenCompraID = o.OrdenCompraID) AS Recibidas
  FROM OrdenCompra o
  JOIN Proveedor pr ON pr.ProveedorID = o.ProveedorID
  LEFT JOIN PagoProveedor pg ON pg.OrdenCompraID = o.OrdenCompraID
 WHERE o.FechaHora >= @desde AND o.FechaHora < @hasta
   AND (@proveedor IS NULL OR o.ProveedorID = @proveedor)
   AND (@estado IS NULL OR o.Estado = @estado)
 ORDER BY pr.RazonSocial, o.FechaHora, o.OrdenCompraID",
                r =>
                {
                    EstadoOrden_60MN estadoFila = EstadosCompra_60MN.OrdenDesdeCodigo(r.GetString(5));
                    int pedidas = r.GetInt32(9), recibidas = r.GetInt32(10);
                    bool vigente = estadoFila != EstadoOrden_60MN.Rechazada && estadoFila != EstadoOrden_60MN.Anulada;
                    return new FilaReporteCompra_60MN
                    {
                        OrdenCompraId = r.GetInt32(0), Numero = r.GetString(1), FechaHora = r.GetDateTime(2), ProveedorId = r.GetInt32(3),
                        Proveedor = r.GetString(4), Estado = estadoFila, Total = r.GetDecimal(6), ImportePagado = r.GetDecimal(7),
                        UltimaRecepcion = r.IsDBNull(8) ? null : r.GetDateTime(8),
                        UnidadesPedidas = pedidas, UnidadesRecibidas = recibidas,
                        UnidadesPendientes = vigente ? pedidas - recibidas : 0
                    };
                },
                ("@desde", desde), ("@hasta", hasta), ("@proveedor", proveedorId), ("@estado", estado?.Codigo()));
        }
    }
}
