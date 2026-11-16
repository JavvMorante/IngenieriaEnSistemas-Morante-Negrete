using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// CUN14 Registrar Recepción de Mercadería (encargado de depósito): ingresa al stock lo que entregó
    /// el proveedor con su remito, contra una orden pagada. Cierra la orden cuando se recibió todo.
    /// </summary>
    public class RecepcionBLL_60MN
    {
        public const int LargoRemito = 30;
        public const int LargoObservacion = 200;

        /// <summary>Paso 3.2: con menos de estos días de vida útil se advierte "próximo a vencer".</summary>
        public const int DiasVencimientoCercano = 90;

        private readonly RecepcionDAL_60MN mapper = new RecepcionDAL_60MN();
        private readonly OrdenCompraDAL_60MN ordenes = new OrdenCompraDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>
        /// Paso 1: busca la orden por número (OC-00001, o solo el número) y verifica que esté PAGADA o
        /// RECIBIDA_PARCIAL. Devuelve la orden con lo pendiente de cada producto.
        /// </summary>
        public OrdenCompra_60MN ObtenerOrdenParaRecibir(string? numero)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Recepcion);
            string texto = (numero ?? string.Empty).Trim().ToUpperInvariant();
            if (texto.StartsWith("OC-")) texto = texto.Substring(3);
            if (!int.TryParse(texto, out int id) || id <= 0)
                throw new NegocioException_60MN("Ingrese el número de la orden de compra (por ejemplo, OC-00001).");

            OrdenCompra_60MN orden = ordenes.ObtenerPorNumero("OC-" + id.ToString("00000"))
                ?? throw new NegocioException_60MN($"No existe la orden de compra OC-{id:00000}.");
            if (orden.Estado != EstadoOrden_60MN.Pagada && orden.Estado != EstadoOrden_60MN.RecibidaParcial)
                throw new NegocioException_60MN(orden.Estado == EstadoOrden_60MN.Cerrada
                    ? $"La orden {orden.Numero} ya se recibió completa."
                    : $"La orden {orden.Numero} está {orden.EstadoTexto.ToLower()}: solo se recibe mercadería de órdenes pagadas.");
            return orden;
        }

        /// <summary>
        /// Pasos 2 a 8: valida remito, cantidades y vencimientos y registra todo en una sola transacción
        /// (recepción, movimientos INGRESO, stock, lote y vencimiento, orden, solicitud y alertas).
        /// Un vencimiento a menos de 90 días pide confirmación (confirmado = true para seguir).
        /// </summary>
        public ResultadoRecepcion_60MN Registrar(Recepcion_60MN recepcion, bool confirmado = false)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Recepcion);
            recepcion.UsuarioId = SesionActual_60MN.UsuarioId
                ?? throw new NegocioException_60MN("El usuario de emergencia no puede registrar recepciones.");

            OrdenCompra_60MN orden = ordenes.ObtenerPorId(recepcion.OrdenCompraId) ?? throw new NegocioException_60MN("La orden de compra no existe.");
            if (orden.Estado != EstadoOrden_60MN.Pagada && orden.Estado != EstadoOrden_60MN.RecibidaParcial)
                throw new NegocioException_60MN($"La orden {orden.Numero} está {orden.EstadoTexto.ToLower()}: no admite recepciones.");

            // 2.1: remito obligatorio y no repetido para el proveedor.
            recepcion.NroRemito = Validaciones_60MN.Normalizar(recepcion.NroRemito).ToUpperInvariant();
            if (recepcion.NroRemito.Length == 0)
                throw new NegocioException_60MN("Ingrese el número de remito del proveedor.");
            Validaciones_60MN.Largo(recepcion.NroRemito, LargoRemito, "El número de remito");
            Validaciones_60MN.Lote(recepcion.NroRemito, "El número de remito");
            if (mapper.ExisteRemito(orden.ProveedorId, recepcion.NroRemito))
                throw new NegocioException_60MN($"El remito {recepcion.NroRemito} de {orden.Proveedor} ya fue registrado en otra recepción.");

            recepcion.Observaciones = Validaciones_60MN.Normalizar(recepcion.Observaciones);
            if (recepcion.Observaciones.Length == 0) recepcion.Observaciones = null;
            else Validaciones_60MN.Largo(recepcion.Observaciones, 400, "La observación");

            if (!recepcion.Detalle.Any(d => d.CantidadRecibida > 0))
                throw new NegocioException_60MN("Ingrese la cantidad recibida de al menos un producto.");

            List<string> proximosAVencer = new List<string>();
            foreach (RecepcionDetalle_60MN d in recepcion.Detalle)
            {
                OrdenCompraDetalle_60MN item = orden.Detalle.FirstOrDefault(o => o.ProductoId == d.ProductoId)
                    ?? throw new NegocioException_60MN($"El producto {d.Producto} no está en la orden {orden.Numero}.");
                d.Producto = item.Producto;
                if (d.CantidadRecibida < 0)
                    throw new NegocioException_60MN($"La cantidad recibida de {d.Producto} no puede ser negativa.");
                // 3.1: no se recibe más de lo pendiente.
                if (d.CantidadRecibida > item.Pendiente)
                    throw new NegocioException_60MN($"La cantidad recibida de {d.Producto} ({d.CantidadRecibida}) supera lo pendiente de la orden ({item.Pendiente}).");

                d.Observacion = Validaciones_60MN.Normalizar(d.Observacion);
                if (d.Observacion.Length == 0) d.Observacion = null;
                else Validaciones_60MN.Largo(d.Observacion, LargoObservacion, $"La observación de {d.Producto}");

                if (d.CantidadRecibida == 0)
                {
                    // Sin ingreso: el detalle conserva el lote y vencimiento actuales del producto.
                    d.Lote = item.LoteActual;
                    d.FechaVencimiento = item.VencimientoActual;
                    continue;
                }

                d.Lote = (d.Lote ?? string.Empty).Trim().ToUpperInvariant();
                if (d.Lote.Length == 0)
                    throw new NegocioException_60MN($"Ingrese el lote recibido de {d.Producto}.");
                Validaciones_60MN.Largo(d.Lote, ProductoBLL_60MN.LargoLote, $"El lote de {d.Producto}");
                Validaciones_60MN.Lote(d.Lote, $"El lote de {d.Producto}");

                // 3.2: vencido se rechaza; a menos de 90 días se advierte.
                if (d.FechaVencimiento.Date < DateTime.Today)
                    throw new NegocioException_60MN($"El lote {d.Lote} de {d.Producto} está vencido ({d.FechaVencimiento:dd/MM/yyyy}): no se puede recibir.");
                if (d.FechaVencimiento.Date < DateTime.Today.AddDays(DiasVencimientoCercano))
                    proximosAVencer.Add($"- {d.Producto}, lote {d.Lote}: vence el {d.FechaVencimiento:dd/MM/yyyy}");
            }
            if (!confirmado && proximosAVencer.Count > 0)
                throw new ConfirmacionRequeridaException_60MN(
                    $"Estos lotes vencen en menos de {DiasVencimientoCercano} días:\n" + string.Join("\n", proximosAVencer) + "\n\n¿Desea recibirlos igual?");

            recepcion.FechaHora = DateTime.Now;
            ResultadoRecepcionDAL_60MN grabado;
            try
            {
                grabado = mapper.Registrar(recepcion, orden.Numero);
            }
            catch (InvalidOperationException ex)
            {
                throw new NegocioException_60MN(ex.Message);
            }

            dv.ActualizarFila(TablaDV_60MN.Recepcion, grabado.RecepcionId);
            foreach (int id in grabado.DetalleIds) dv.ActualizarFila(TablaDV_60MN.RecepcionDetalle, id);
            foreach (int id in grabado.MovimientoIds) dv.ActualizarFila(TablaDV_60MN.MovimientoStock, id);
            foreach (int id in grabado.ProductoIds) dv.ActualizarFila(TablaDV_60MN.Productos, id);
            foreach (int id in grabado.OrdenDetalleIds) dv.ActualizarFila(TablaDV_60MN.OrdenCompraDetalle, id);
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, orden.Id);
            if (grabado.SolicitudCerrada)
                dv.ActualizarFila(TablaDV_60MN.SolicitudReposicion, orden.SolicitudId);

            ResultadoRecepcion_60MN resultado = new ResultadoRecepcion_60MN
            {
                RecepcionId = grabado.RecepcionId,
                EstadoOrden = grabado.EstadoOrden,
                UnidadesIngresadas = recepcion.Detalle.Sum(d => d.CantidadRecibida)
            };
            resultado.Advertencias.AddRange(proximosAVencer.Select(a => a.TrimStart('-', ' ')));

            bitacora.Registrar(EventoSistema_60MN.MercaderiaRecibida,
                $"Recepción de la orden {orden.Numero} ({orden.Proveedor}), remito {recepcion.NroRemito}: " +
                string.Join(", ", recepcion.Detalle.Where(d => d.CantidadRecibida > 0).Select(d => $"{d.CantidadRecibida} x {d.Producto} (lote {d.Lote})")) +
                $". La orden queda {grabado.EstadoOrden.Descripcion().ToLower()}" +
                (grabado.SolicitudCerrada ? $" y se cierra la solicitud {orden.SolicitudNumero}." : ".") +
                (grabado.AlertasDesactivadas > 0 ? $" Alertas de reposición resueltas: {grabado.AlertasDesactivadas}." : string.Empty));
            return resultado;
        }
    }
}
