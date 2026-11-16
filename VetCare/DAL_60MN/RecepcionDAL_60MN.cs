using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Filas que tocó una recepción, para que la BLL actualice sus dígitos verificadores.</summary>
    public class ResultadoRecepcionDAL_60MN
    {
        public int RecepcionId { get; set; }

        public List<int> DetalleIds { get; } = new List<int>();

        public List<int> MovimientoIds { get; } = new List<int>();

        public List<int> ProductoIds { get; } = new List<int>();

        public List<int> OrdenDetalleIds { get; } = new List<int>();

        public EstadoOrden_60MN EstadoOrden { get; set; }

        public bool SolicitudCerrada { get; set; }

        /// <summary>Productos cuya alerta de reposición se desactivó porque el stock superó el mínimo.</summary>
        public int AlertasDesactivadas { get; set; }
    }

    /// <summary>Mapper de Recepcion y RecepcionDetalle (CUN14).</summary>
    public class RecepcionDAL_60MN
    {
        /// <summary>El remito no se repite para un mismo proveedor (el proveedor está en la orden).</summary>
        public bool ExisteRemito(int proveedorId, string nroRemito)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(@"
SELECT COUNT(*) FROM Recepcion r JOIN OrdenCompra o ON o.OrdenCompraID = r.OrdenCompraID
 WHERE o.ProveedorID = @proveedor AND r.NroRemito = @remito",
                ("@proveedor", proveedorId), ("@remito", nroRemito))) > 0;
        }

        /// <summary>
        /// CUN14 pasos 4 a 8, en UNA transacción: graba la recepción y, por cada producto recibido,
        /// el movimiento INGRESO, el nuevo stock, lote y vencimiento del producto (condicionado al
        /// stock leído), el detalle y lo recibido de la orden. Después cierra o deja parcial la
        /// orden (y la solicitud) y desactiva las alertas que dejaron de cumplirse.
        /// Ante cualquier conflicto lanza InvalidOperationException y no queda nada grabado.
        /// </summary>
        public ResultadoRecepcionDAL_60MN Registrar(Recepcion_60MN recepcion, string numeroOrden)
        {
            ResultadoRecepcionDAL_60MN resultado = new ResultadoRecepcionDAL_60MN();
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();

            using (SqlCommand enc = Conexion_60MN.Crear(@"
INSERT INTO Recepcion (OrdenCompraID, NroRemito, FechaHora, UsuarioID, Observaciones)
VALUES (@orden, @remito, @fecha, @usuario, @observaciones);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@orden", recepcion.OrdenCompraId), ("@remito", recepcion.NroRemito), ("@fecha", recepcion.FechaHora),
                ("@usuario", recepcion.UsuarioId), ("@observaciones", recepcion.Observaciones)))
                resultado.RecepcionId = Convert.ToInt32(enc.ExecuteScalar());

            foreach (RecepcionDetalle_60MN d in recepcion.Detalle)
            {
                int? movimientoId = null;
                if (d.CantidadRecibida > 0)
                {
                    int stockAnterior;
                    using (SqlCommand leer = Conexion_60MN.Crear(
                        "SELECT StockActual FROM Productos WITH (UPDLOCK, ROWLOCK) WHERE ProductoID = @id", con, tx, ("@id", d.ProductoId)))
                    {
                        object? valor = leer.ExecuteScalar();
                        if (valor == null || valor is DBNull)
                            throw new InvalidOperationException($"El producto {d.Producto} ya no existe.");
                        stockAnterior = Convert.ToInt32(valor);
                    }
                    int stockResultante = stockAnterior + d.CantidadRecibida;

                    // Concurrencia optimista: solo si el stock sigue siendo el leído.
                    using (SqlCommand producto = Conexion_60MN.Crear(@"
UPDATE Productos SET StockActual = @nuevo, Lote = @lote, FechaVencimiento = @vencimiento, UsuarioModificacion = @usuario
 WHERE ProductoID = @id AND StockActual = @anterior", con, tx,
                        ("@nuevo", stockResultante), ("@lote", d.Lote), ("@vencimiento", d.FechaVencimiento.Date),
                        ("@usuario", recepcion.UsuarioId), ("@id", d.ProductoId), ("@anterior", stockAnterior)))
                    {
                        if (producto.ExecuteNonQuery() == 0)
                            throw new InvalidOperationException($"El stock de {d.Producto} fue modificado por otro usuario. Vuelva a intentarlo.");
                    }

                    using (SqlCommand mov = Conexion_60MN.Crear(@"
INSERT INTO MovimientoStock (ProductoID, Tipo, Cantidad, StockAnterior, StockResultante, Motivo, FechaHora, UsuarioID)
VALUES (@producto, 'INGRESO', @cantidad, @anterior, @resultante, @motivo, @fecha, @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                        ("@producto", d.ProductoId), ("@cantidad", d.CantidadRecibida), ("@anterior", stockAnterior),
                        ("@resultante", stockResultante), ("@motivo", $"Recepción {numeroOrden} / remito {recepcion.NroRemito}"),
                        ("@fecha", recepcion.FechaHora), ("@usuario", recepcion.UsuarioId)))
                        movimientoId = Convert.ToInt32(mov.ExecuteScalar());
                    resultado.MovimientoIds.Add(movimientoId.Value);
                    resultado.ProductoIds.Add(d.ProductoId);

                    using (SqlCommand recibido = Conexion_60MN.Crear(@"
UPDATE OrdenCompraDetalle SET CantidadRecibida = CantidadRecibida + @cantidad
OUTPUT inserted.OrdenDetalleID
 WHERE OrdenCompraID = @orden AND ProductoID = @producto AND CantidadRecibida + @cantidad <= Cantidad", con, tx,
                        ("@cantidad", d.CantidadRecibida), ("@orden", recepcion.OrdenCompraId), ("@producto", d.ProductoId)))
                    {
                        object? detalleOrden = recibido.ExecuteScalar();
                        if (detalleOrden == null || detalleOrden is DBNull)
                            throw new InvalidOperationException($"La cantidad recibida de {d.Producto} supera lo pendiente de la orden (otra recepción se registró mientras tanto).");
                        resultado.OrdenDetalleIds.Add(Convert.ToInt32(detalleOrden));
                    }

                    // Paso 8: la alerta deja de cumplirse si el stock supera el mínimo.
                    using SqlCommand alerta = Conexion_60MN.Crear(@"
UPDATE a SET Activa = 0
  FROM AlertaReposicion a JOIN Productos p ON p.ProductoID = a.ProductoID
 WHERE a.ProductoID = @id AND a.Activa = 1 AND p.StockActual > p.StockMinimo", con, tx, ("@id", d.ProductoId));
                    resultado.AlertasDesactivadas += alerta.ExecuteNonQuery();
                }

                using SqlCommand det = Conexion_60MN.Crear(@"
INSERT INTO RecepcionDetalle (RecepcionID, ProductoID, CantidadRecibida, Lote, FechaVencimiento, Observacion, MovimientoID)
VALUES (@recepcion, @producto, @cantidad, @lote, @vencimiento, @observacion, @movimiento);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                    ("@recepcion", resultado.RecepcionId), ("@producto", d.ProductoId), ("@cantidad", d.CantidadRecibida),
                    ("@lote", d.Lote), ("@vencimiento", d.FechaVencimiento.Date), ("@observacion", d.Observacion),
                    ("@movimiento", movimientoId));
                resultado.DetalleIds.Add(Convert.ToInt32(det.ExecuteScalar()));
            }

            // Paso 7: CERRADA si se recibió todo; si no, RECIBIDA_PARCIAL.
            int pendiente;
            using (SqlCommand leer = Conexion_60MN.Crear(
                "SELECT SUM(Cantidad - CantidadRecibida) FROM OrdenCompraDetalle WHERE OrdenCompraID = @orden", con, tx,
                ("@orden", recepcion.OrdenCompraId)))
                pendiente = Convert.ToInt32(leer.ExecuteScalar());
            resultado.EstadoOrden = pendiente == 0 ? EstadoOrden_60MN.Cerrada : EstadoOrden_60MN.RecibidaParcial;

            using (SqlCommand orden = Conexion_60MN.Crear(@"
UPDATE OrdenCompra SET Estado = @estado WHERE OrdenCompraID = @orden AND Estado IN ('PAGADA', 'RECIBIDA_PARCIAL')", con, tx,
                ("@estado", resultado.EstadoOrden.Codigo()), ("@orden", recepcion.OrdenCompraId)))
            {
                if (orden.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"La orden {numeroOrden} cambió de estado mientras se registraba la recepción.");
            }

            // La solicitud se cierra cuando todas sus órdenes están cerradas (o anuladas) y no queda nada por pedir.
            if (resultado.EstadoOrden == EstadoOrden_60MN.Cerrada)
            {
                using SqlCommand solicitud = Conexion_60MN.Crear(@"
UPDATE s SET Estado = 'CERRADA'
  FROM SolicitudReposicion s JOIN OrdenCompra o ON o.SolicitudID = s.SolicitudID
 WHERE o.OrdenCompraID = @orden AND s.Estado = 'EN_COMPRA'
   AND NOT EXISTS (SELECT 1 FROM OrdenCompra otra WHERE otra.SolicitudID = s.SolicitudID AND otra.Estado NOT IN ('CERRADA', 'ANULADA'))
   AND NOT EXISTS (SELECT 1 FROM SolicitudReposicionDetalle d
                    WHERE d.SolicitudID = s.SolicitudID
                      AND d.CantidadPedida > COALESCE((SELECT SUM(od.Cantidad)
                                                         FROM OrdenCompraDetalle od JOIN OrdenCompra oc ON oc.OrdenCompraID = od.OrdenCompraID
                                                        WHERE oc.SolicitudID = s.SolicitudID AND od.ProductoID = d.ProductoID
                                                          AND oc.Estado <> 'ANULADA'), 0))", con, tx,
                    ("@orden", recepcion.OrdenCompraId));
                resultado.SolicitudCerrada = solicitud.ExecuteNonQuery() > 0;
            }

            tx.Commit();
            return resultado;
        }
    }
}
