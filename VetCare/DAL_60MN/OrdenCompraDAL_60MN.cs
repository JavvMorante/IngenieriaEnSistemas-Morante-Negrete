using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Mapper de OrdenCompra y OrdenCompraDetalle (CUN11 a CUN13).</summary>
    public class OrdenCompraDAL_60MN
    {
        private const string SelectEncabezado = @"
SELECT o.OrdenCompraID, o.Numero, o.SolicitudID, s.Numero AS Solicitud, o.ProveedorID, pr.RazonSocial, o.FechaHora,
       o.NroCotizacion, o.PlazoEntregaDias, o.CondicionPago, o.MotivoEleccion, o.Advertencias, o.Total, o.Estado,
       o.UsuarioID, u.Usuario, o.AprobadaPor, ua.Usuario AS Aprobador, o.FechaAprobacion, o.MotivoRechazo
  FROM OrdenCompra o
  JOIN SolicitudReposicion s ON s.SolicitudID = o.SolicitudID
  JOIN Proveedor pr ON pr.ProveedorID = o.ProveedorID
  JOIN Usuario u ON u.UsuarioID = o.UsuarioID
  LEFT JOIN Usuario ua ON ua.UsuarioID = o.AprobadaPor";

        /// <summary>
        /// CUN11: graba la orden en PENDIENTE_APROBACION con su detalle y pasa la solicitud
        /// a EN_COMPRA, en una sola transacción. Devuelve el Id de la orden y los Id del detalle.
        /// </summary>
        public (int OrdenId, List<int> DetalleIds) Registrar(OrdenCompra_60MN orden)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();

            int id;
            using (SqlCommand enc = Conexion_60MN.Crear(@"
INSERT INTO OrdenCompra (SolicitudID, ProveedorID, FechaHora, NroCotizacion, PlazoEntregaDias, CondicionPago,
                         MotivoEleccion, Advertencias, Total, Estado, UsuarioID)
VALUES (@solicitud, @proveedor, @fecha, @cotizacion, @plazo, @condicion, @motivo, @advertencias, @total,
        'PENDIENTE_APROBACION', @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@solicitud", orden.SolicitudId), ("@proveedor", orden.ProveedorId), ("@fecha", orden.FechaHora),
                ("@cotizacion", orden.NroCotizacion), ("@plazo", orden.PlazoEntregaDias), ("@condicion", orden.CondicionPago),
                ("@motivo", orden.MotivoEleccion), ("@advertencias", orden.Advertencias), ("@total", orden.Total),
                ("@usuario", orden.UsuarioId)))
                id = Convert.ToInt32(enc.ExecuteScalar());

            List<int> detalleIds = new List<int>();
            foreach (OrdenCompraDetalle_60MN d in orden.Detalle)
            {
                using SqlCommand det = Conexion_60MN.Crear(@"
INSERT INTO OrdenCompraDetalle (OrdenCompraID, ProductoID, Cantidad, PrecioUnitario, Subtotal)
VALUES (@orden, @producto, @cantidad, @precio, @subtotal);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                    ("@orden", id), ("@producto", d.ProductoId), ("@cantidad", d.Cantidad), ("@precio", d.PrecioUnitario),
                    ("@subtotal", d.Subtotal));
                detalleIds.Add(Convert.ToInt32(det.ExecuteScalar()));
            }

            using (SqlCommand solicitud = Conexion_60MN.Crear(
                "UPDATE SolicitudReposicion SET Estado = 'EN_COMPRA' WHERE SolicitudID = @id AND Estado = 'PENDIENTE'",
                con, tx, ("@id", orden.SolicitudId)))
                solicitud.ExecuteNonQuery();

            tx.Commit();
            return (id, detalleIds);
        }

        /// <summary>
        /// CUN11 (orden rechazada): guarda la corrección y la devuelve a PENDIENTE_APROBACION. Los
        /// productos son los mismos; cambian proveedor, cotización, cantidades y precios.
        /// Devuelve false si la orden ya no estaba RECHAZADA.
        /// </summary>
        public bool Corregir(OrdenCompra_60MN orden)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();

            using (SqlCommand enc = Conexion_60MN.Crear(@"
UPDATE OrdenCompra
   SET ProveedorID = @proveedor, NroCotizacion = @cotizacion, PlazoEntregaDias = @plazo, CondicionPago = @condicion,
       MotivoEleccion = @motivo, Advertencias = @advertencias, Total = @total, Estado = 'PENDIENTE_APROBACION',
       AprobadaPor = NULL, FechaAprobacion = NULL, MotivoRechazo = NULL
 WHERE OrdenCompraID = @id AND Estado = 'RECHAZADA'", con, tx,
                ("@proveedor", orden.ProveedorId), ("@cotizacion", orden.NroCotizacion), ("@plazo", orden.PlazoEntregaDias),
                ("@condicion", orden.CondicionPago), ("@motivo", orden.MotivoEleccion), ("@advertencias", orden.Advertencias),
                ("@total", orden.Total), ("@id", orden.Id)))
            {
                if (enc.ExecuteNonQuery() == 0)
                {
                    tx.Rollback();
                    return false;
                }
            }

            foreach (OrdenCompraDetalle_60MN d in orden.Detalle)
            {
                using SqlCommand det = Conexion_60MN.Crear(@"
UPDATE OrdenCompraDetalle SET Cantidad = @cantidad, PrecioUnitario = @precio, Subtotal = @subtotal
 WHERE OrdenDetalleID = @id AND OrdenCompraID = @orden", con, tx,
                    ("@cantidad", d.Cantidad), ("@precio", d.PrecioUnitario), ("@subtotal", d.Subtotal), ("@id", d.Id), ("@orden", orden.Id));
                det.ExecuteNonQuery();
            }
            tx.Commit();
            return true;
        }

        /// <summary>CUN12 Aprobar: solo si sigue PENDIENTE_APROBACION (concurrencia). Devuelve false si cambió.</summary>
        public bool Aprobar(int id, int usuarioId, DateTime fecha)
        {
            return Conexion_60MN.EjecutarNonQuery(@"
UPDATE OrdenCompra SET Estado = 'APROBADA', AprobadaPor = @usuario, FechaAprobacion = @fecha, MotivoRechazo = NULL
 WHERE OrdenCompraID = @id AND Estado = 'PENDIENTE_APROBACION'",
                ("@usuario", usuarioId), ("@fecha", fecha), ("@id", id)) > 0;
        }

        /// <summary>CUN12 Rechazar: guarda quién decidió, cuándo y el motivo. Devuelve false si la orden cambió.</summary>
        public bool Rechazar(int id, int usuarioId, DateTime fecha, string motivo)
        {
            return Conexion_60MN.EjecutarNonQuery(@"
UPDATE OrdenCompra SET Estado = 'RECHAZADA', AprobadaPor = @usuario, FechaAprobacion = @fecha, MotivoRechazo = @motivo
 WHERE OrdenCompraID = @id AND Estado = 'PENDIENTE_APROBACION'",
                ("@usuario", usuarioId), ("@fecha", fecha), ("@motivo", motivo), ("@id", id)) > 0;
        }

        /// <summary>CUN13: la orden aprobada se envía al proveedor. Devuelve false si ya no estaba APROBADA.</summary>
        public bool MarcarEnviada(int id)
        {
            return Conexion_60MN.EjecutarNonQuery(
                "UPDATE OrdenCompra SET Estado = 'ENVIADA' WHERE OrdenCompraID = @id AND Estado = 'APROBADA'", ("@id", id)) > 0;
        }

        /// <summary>
        /// CUN11 Anular (antes de pagarla). El motivo queda en MotivoRechazo. Si la solicitud ya no
        /// tiene otras órdenes vigentes, vuelve a PENDIENTE para poder pedir esos productos de nuevo.
        /// Devuelve false si la orden ya no estaba en un estado anulable.
        /// </summary>
        public bool Anular(int id, string motivo, out bool solicitudReabierta)
        {
            solicitudReabierta = false;
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();

            using (SqlCommand orden = Conexion_60MN.Crear(@"
UPDATE OrdenCompra SET Estado = 'ANULADA', MotivoRechazo = @motivo
 WHERE OrdenCompraID = @id AND Estado IN ('PENDIENTE_APROBACION', 'RECHAZADA', 'APROBADA', 'ENVIADA')", con, tx,
                ("@motivo", motivo), ("@id", id)))
            {
                if (orden.ExecuteNonQuery() == 0)
                {
                    tx.Rollback();
                    return false;
                }
            }

            using (SqlCommand solicitud = Conexion_60MN.Crear(@"
UPDATE s SET Estado = 'PENDIENTE'
  FROM SolicitudReposicion s JOIN OrdenCompra o ON o.SolicitudID = s.SolicitudID
 WHERE o.OrdenCompraID = @id AND s.Estado = 'EN_COMPRA'
   AND NOT EXISTS (SELECT 1 FROM OrdenCompra otra WHERE otra.SolicitudID = s.SolicitudID AND otra.Estado <> 'ANULADA')",
                con, tx, ("@id", id)))
                solicitudReabierta = solicitud.ExecuteNonQuery() > 0;

            tx.Commit();
            return true;
        }

        public List<OrdenCompra_60MN> Listar(IEnumerable<EstadoOrden_60MN> estados, int? usuarioId = null)
        {
            // Los códigos de estado son constantes del sistema, no datos del usuario.
            string enEstados = string.Join(",", estados.Select(e => "'" + e.Codigo() + "'"));
            List<OrdenCompra_60MN> ordenes = Conexion_60MN.EjecutarLista(SelectEncabezado + $@"
 WHERE o.Estado IN ({enEstados}) AND (@usuario IS NULL OR o.UsuarioID = @usuario)
 ORDER BY o.FechaHora DESC, o.OrdenCompraID DESC", Mapear, ("@usuario", usuarioId));
            CargarDetalle(ordenes);
            return ordenes;
        }

        public OrdenCompra_60MN? ObtenerPorId(int id)
        {
            List<OrdenCompra_60MN> ordenes = Conexion_60MN.EjecutarLista(SelectEncabezado + " WHERE o.OrdenCompraID = @id", Mapear, ("@id", id));
            CargarDetalle(ordenes);
            return ordenes.FirstOrDefault();
        }

        public OrdenCompra_60MN? ObtenerPorNumero(string numero)
        {
            List<OrdenCompra_60MN> ordenes = Conexion_60MN.EjecutarLista(SelectEncabezado + " WHERE o.Numero = @numero", Mapear, ("@numero", numero));
            CargarDetalle(ordenes);
            return ordenes.FirstOrDefault();
        }

        /// <summary>CUN12: total de las órdenes aprobadas en el mes de la fecha (incluye las ya enviadas, pagadas o recibidas).</summary>
        public decimal TotalAprobadoEnElMes(DateTime fecha)
        {
            DateTime desde = new DateTime(fecha.Year, fecha.Month, 1);
            return Convert.ToDecimal(Conexion_60MN.EjecutarEscalar(@"
SELECT COALESCE(SUM(Total), 0) FROM OrdenCompra
 WHERE FechaAprobacion >= @desde AND FechaAprobacion < @hasta
   AND Estado IN ('APROBADA', 'ENVIADA', 'PAGADA', 'RECIBIDA_PARCIAL', 'CERRADA')",
                ("@desde", desde), ("@hasta", desde.AddMonths(1))) ?? 0m);
        }

        private static void CargarDetalle(List<OrdenCompra_60MN> ordenes)
        {
            if (ordenes.Count == 0) return;
            Dictionary<int, OrdenCompra_60MN> porId = ordenes.ToDictionary(o => o.Id);
            string ids = string.Join(",", porId.Keys); // enteros propios, no datos del usuario
            List<OrdenCompraDetalle_60MN> detalle = Conexion_60MN.EjecutarLista($@"
SELECT d.OrdenDetalleID, d.OrdenCompraID, d.ProductoID, p.Codigo, p.Nombre, d.Cantidad, d.PrecioUnitario, d.Subtotal,
       d.CantidadRecibida, p.PrecioCosto, p.Lote, p.FechaVencimiento
  FROM OrdenCompraDetalle d JOIN Productos p ON p.ProductoID = d.ProductoID
 WHERE d.OrdenCompraID IN ({ids}) ORDER BY d.OrdenDetalleID",
                r => new OrdenCompraDetalle_60MN
                {
                    Id = r.GetInt32(0), OrdenCompraId = r.GetInt32(1), ProductoId = r.GetInt32(2), Codigo = r.GetString(3),
                    Producto = r.GetString(4), Cantidad = r.GetInt32(5), PrecioUnitario = r.GetDecimal(6), Subtotal = r.GetDecimal(7),
                    CantidadRecibida = r.GetInt32(8), PrecioCostoActual = r.GetDecimal(9), LoteActual = r.GetString(10),
                    VencimientoActual = r.GetDateTime(11)
                });
            foreach (OrdenCompraDetalle_60MN d in detalle)
                porId[d.OrdenCompraId].Detalle.Add(d);
        }

        private static OrdenCompra_60MN Mapear(SqlDataReader r) => new OrdenCompra_60MN
        {
            Id = r.GetInt32(0),
            Numero = r.GetString(1),
            SolicitudId = r.GetInt32(2),
            SolicitudNumero = r.GetString(3),
            ProveedorId = r.GetInt32(4),
            Proveedor = r.GetString(5),
            FechaHora = r.GetDateTime(6),
            NroCotizacion = r.GetString(7),
            PlazoEntregaDias = r.GetInt32(8),
            CondicionPago = r.GetString(9),
            MotivoEleccion = r.TextoONulo("MotivoEleccion"),
            Advertencias = r.TextoONulo("Advertencias"),
            Total = r.GetDecimal(12),
            Estado = EstadosCompra_60MN.OrdenDesdeCodigo(r.GetString(13)),
            UsuarioId = r.GetInt32(14),
            Usuario = r.GetString(15),
            AprobadaPor = r.EnteroONulo("AprobadaPor"),
            AprobadaPorUsuario = r.TextoONulo("Aprobador"),
            FechaAprobacion = r.IsDBNull(18) ? null : r.GetDateTime(18),
            MotivoRechazo = r.TextoONulo("MotivoRechazo")
        };
    }
}
