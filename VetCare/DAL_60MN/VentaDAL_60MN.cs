using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Resultado de vender un ítem del carrito: el egreso de stock y el detalle generados.</summary>
    public class EntregaCarrito_60MN
    {
        public int ProductoId { get; set; }

        public int MovimientoId { get; set; }

        public int DetalleId { get; set; }

        public int StockResultante { get; set; }
    }

    /// <summary>Mapper de VentaEncabezado y VentaDetalle (CUN05 Registrar Medicación en Carrito).</summary>
    public class VentaDAL_60MN
    {
        private const string SelectEncabezado = @"
SELECT v.VentaID, v.ClienteID, v.AtencionID, v.FechaHora, v.UsuarioID, v.Total,
       c.Apellido + ', ' + c.Nombre AS Cliente, c.TipoDocumento + ' ' + c.NroDocumento AS Documento,
       m.Nombre AS Paciente, u.Usuario
  FROM VentaEncabezado v
  JOIN Cliente c ON c.ClienteID = v.ClienteID
  LEFT JOIN Atencion a ON a.AtencionID = v.AtencionID
  LEFT JOIN HistoriaClinica h ON h.HistoriaClinicaID = a.HistoriaClinicaID
  LEFT JOIN Mascota m ON m.MascotaID = h.MascotaID
  LEFT JOIN Usuario u ON u.UsuarioID = v.UsuarioID";

        /// <summary>
        /// CUN05 «include» CUN09: en una sola transacción graba el encabezado y, por cada ítem,
        /// descuenta el stock (bloqueando la fila del producto), registra el egreso en
        /// MovimientoStock y el detalle en VentaDetalle. Si algún producto no alcanza, no se
        /// registra nada. Devuelve el Id de la venta y lo generado por cada ítem.
        /// </summary>
        public (int VentaId, List<EntregaCarrito_60MN> Entregas) Registrar(Carrito_60MN carrito, int? usuarioId, string motivo)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            List<EntregaCarrito_60MN> entregas = new List<EntregaCarrito_60MN>();
            DateTime ahora = DateTime.Now;

            int ventaId;
            using (SqlCommand enc = Conexion_60MN.Crear(@"
INSERT INTO VentaEncabezado (ClienteID, AtencionID, FechaHora, UsuarioID, Total)
VALUES (@cliente, @atencion, @fecha, @usuario, @total);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@cliente", carrito.ClienteId), ("@atencion", carrito.AtencionId), ("@fecha", ahora),
                ("@usuario", usuarioId), ("@total", carrito.Total)))
                ventaId = Convert.ToInt32(enc.ExecuteScalar());

            foreach (ItemCarrito_60MN item in carrito.Items)
            {
                int stockAnterior;
                using (SqlCommand leer = Conexion_60MN.Crear(
                    "SELECT StockActual FROM Productos WITH (UPDLOCK, ROWLOCK) WHERE ProductoID = @id AND Activo = 1", con, tx, ("@id", item.ProductoId)))
                {
                    object? valor = leer.ExecuteScalar();
                    if (valor == null || valor is DBNull)
                        throw new InvalidOperationException($"El producto {item.Codigo} ya no está disponible.");
                    stockAnterior = Convert.ToInt32(valor);
                }
                if (item.Cantidad > stockAnterior)
                    throw new InvalidOperationException($"Stock insuficiente de {item.Producto}: disponible {stockAnterior}, solicitado {item.Cantidad}.");

                int stockResultante = stockAnterior - item.Cantidad;
                using (SqlCommand stock = Conexion_60MN.Crear(
                    "UPDATE Productos SET StockActual = @nuevo, UsuarioModificacion = @usuario WHERE ProductoID = @id", con, tx,
                    ("@nuevo", stockResultante), ("@usuario", usuarioId ?? 0), ("@id", item.ProductoId)))
                    stock.ExecuteNonQuery();

                int movimientoId;
                using (SqlCommand mov = Conexion_60MN.Crear(@"
INSERT INTO MovimientoStock (ProductoID, Tipo, Cantidad, StockAnterior, StockResultante, Motivo, FechaHora, UsuarioID)
VALUES (@producto, 'EGRESO', @cantidad, @anterior, @resultante, @motivo, @fecha, @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                    ("@producto", item.ProductoId), ("@cantidad", -item.Cantidad), ("@anterior", stockAnterior),
                    ("@resultante", stockResultante), ("@motivo", motivo), ("@fecha", ahora), ("@usuario", usuarioId)))
                    movimientoId = Convert.ToInt32(mov.ExecuteScalar());

                int detalleId;
                using (SqlCommand det = Conexion_60MN.Crear(@"
INSERT INTO VentaDetalle (VentaID, ProductoID, Cantidad, PrecioUnitario, Subtotal, MovimientoID)
VALUES (@venta, @producto, @cantidad, @precio, @subtotal, @movimiento);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                    ("@venta", ventaId), ("@producto", item.ProductoId), ("@cantidad", item.Cantidad),
                    ("@precio", item.PrecioUnitario), ("@subtotal", item.Subtotal), ("@movimiento", movimientoId)))
                    detalleId = Convert.ToInt32(det.ExecuteScalar());

                entregas.Add(new EntregaCarrito_60MN
                {
                    ProductoId = item.ProductoId, MovimientoId = movimientoId, DetalleId = detalleId, StockResultante = stockResultante
                });
            }
            tx.Commit();
            return (ventaId, entregas);
        }

        /// <summary>Ventas asociadas a una atención (la medicación que se le entregó), con su detalle.</summary>
        public List<VentaEncabezado_60MN> ObtenerPorAtencion(int atencionId)
        {
            List<VentaEncabezado_60MN> ventas = Conexion_60MN.EjecutarLista(
                SelectEncabezado + " WHERE v.AtencionID = @atencion ORDER BY v.FechaHora, v.VentaID", Mapear, ("@atencion", atencionId));
            CargarDetalle(ventas);
            return ventas;
        }

        /// <summary>Ventas del período [desde, hasta) y, opcionalmente, de un cliente, con su detalle.</summary>
        public List<VentaEncabezado_60MN> ListarVentas(DateTime desde, DateTime hasta, int? clienteId)
        {
            List<VentaEncabezado_60MN> ventas = Conexion_60MN.EjecutarLista(SelectEncabezado + @"
 WHERE v.FechaHora >= @desde AND v.FechaHora < @hasta AND (@cliente IS NULL OR v.ClienteID = @cliente)
 ORDER BY v.FechaHora DESC, v.VentaID DESC", Mapear, ("@desde", desde), ("@hasta", hasta), ("@cliente", clienteId));
            CargarDetalle(ventas);
            return ventas;
        }

        private static void CargarDetalle(List<VentaEncabezado_60MN> ventas)
        {
            if (ventas.Count == 0) return;
            Dictionary<int, VentaEncabezado_60MN> porId = ventas.ToDictionary(v => v.Id);
            string ids = string.Join(",", porId.Keys); // enteros propios, no datos del usuario
            List<VentaDetalle_60MN> detalle = Conexion_60MN.EjecutarLista($@"
SELECT d.VentaDetalleID, d.VentaID, d.ProductoID, p.Codigo, p.Nombre, p.Lote, d.Cantidad, d.PrecioUnitario, d.Subtotal, d.MovimientoID
  FROM VentaDetalle d JOIN Productos p ON p.ProductoID = d.ProductoID
 WHERE d.VentaID IN ({ids}) ORDER BY d.VentaDetalleID",
                r => new VentaDetalle_60MN
                {
                    Id = r.GetInt32(0), VentaId = r.GetInt32(1), ProductoId = r.GetInt32(2), Codigo = r.GetString(3),
                    Producto = r.GetString(4), Lote = r.GetString(5), Cantidad = r.GetInt32(6),
                    PrecioUnitario = r.GetDecimal(7), Subtotal = r.GetDecimal(8), MovimientoId = r.GetInt32(9)
                });
            foreach (VentaDetalle_60MN d in detalle)
                porId[d.VentaId].Detalle.Add(d);
        }

        private static VentaEncabezado_60MN Mapear(SqlDataReader r) => new VentaEncabezado_60MN
        {
            Id = r.GetInt32(0),
            ClienteId = r.GetInt32(1),
            AtencionId = r.IsDBNull(2) ? null : r.GetInt32(2),
            FechaHora = r.GetDateTime(3),
            UsuarioId = r.IsDBNull(4) ? null : r.GetInt32(4),
            Total = r.GetDecimal(5),
            Cliente = r.GetString(6),
            DocumentoCliente = r.GetString(7),
            Paciente = r.IsDBNull(8) ? string.Empty : r.GetString(8),
            Usuario = r.IsDBNull(9) ? "emergencia" : r.GetString(9)
        };
    }
}
