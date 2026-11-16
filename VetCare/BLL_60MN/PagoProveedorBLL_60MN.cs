using System.Globalization;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// CUN13 Registrar Pago al Proveedor (analista de compras): envía la orden aprobada al proveedor
    /// y registra el comprobante de la transferencia, que se hace en el home banking del banco.
    /// </summary>
    public class PagoProveedorBLL_60MN
    {
        public const int LargoBanco = 60;
        public const int LargoComprobante = 40;
        public const int LargoObservaciones = 300;

        /// <summary>Bancos que se proponen en el combo editable (además de los ya usados).</summary>
        public static readonly string[] BancosFrecuentes =
        {
            "Banco de la Nación Argentina", "Banco de la Provincia de Buenos Aires", "Banco Galicia", "Banco Santander",
            "BBVA", "Banco Macro", "Banco Credicoop", "Banco Ciudad"
        };

        private static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("es-AR");

        private readonly PagoProveedorDAL_60MN mapper = new PagoProveedorDAL_60MN();
        private readonly OrdenCompraDAL_60MN ordenes = new OrdenCompraDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Paso 1: órdenes aprobadas (por enviar) o enviadas (por pagar).</summary>
        public List<OrdenCompra_60MN> ListarOrdenesAPagar()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.PagoProveedor);
            return ordenes.Listar(new[] { EstadoOrden_60MN.Aprobada, EstadoOrden_60MN.Enviada });
        }

        public List<string> ListarBancos()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.PagoProveedor);
            return BancosFrecuentes.Union(mapper.ListarBancosUsados(), StringComparer.OrdinalIgnoreCase).OrderBy(b => b).ToList();
        }

        /// <summary>
        /// Paso 2: la orden aprobada pasa a ENVIADA y se devuelve para imprimirla (PDF) y mandarla al
        /// proveedor. Una orden ya enviada se puede volver a imprimir sin cambiar su estado.
        /// </summary>
        public OrdenCompra_60MN MarcarEnviada(int ordenId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.PagoProveedor);
            OrdenCompra_60MN orden = ordenes.ObtenerPorId(ordenId) ?? throw new NegocioException_60MN("La orden de compra no existe.");
            if (orden.Estado == EstadoOrden_60MN.Enviada)
                return orden;
            if (orden.Estado != EstadoOrden_60MN.Aprobada)
                throw new NegocioException_60MN($"La orden {orden.Numero} está {orden.EstadoTexto.ToLower()}: solo se envían órdenes aprobadas.");

            if (!ordenes.MarcarEnviada(ordenId))
                throw new NegocioException_60MN($"La orden {orden.Numero} cambió de estado mientras tanto. Vuelva a consultarla.");
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, ordenId);
            bitacora.Registrar(EventoSistema_60MN.OrdenCompraEnviada, $"Envío de la orden {orden.Numero} al proveedor {orden.Proveedor}.");
            return ordenes.ObtenerPorId(ordenId)!;
        }

        /// <summary>
        /// Pasos 3 a 6: valida el comprobante y registra el pago; la orden pasa a PAGADA. Si el importe
        /// difiere del total de la orden pide confirmación (confirmado = true) y el motivo es obligatorio.
        /// </summary>
        public void Registrar(PagoProveedor_60MN pago, bool confirmado = false)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.PagoProveedor);
            pago.UsuarioId = SesionActual_60MN.UsuarioId
                ?? throw new NegocioException_60MN("El usuario de emergencia no puede registrar pagos.");

            OrdenCompra_60MN orden = ordenes.ObtenerPorId(pago.OrdenCompraId) ?? throw new NegocioException_60MN("La orden de compra no existe.");
            if (orden.Estado != EstadoOrden_60MN.Aprobada && orden.Estado != EstadoOrden_60MN.Enviada)
                throw new NegocioException_60MN($"La orden {orden.Numero} está {orden.EstadoTexto.ToLower()}: solo se pagan órdenes aprobadas o enviadas.");

            pago.Banco = Validaciones_60MN.Normalizar(pago.Banco);
            if (pago.Banco.Length == 0)
                throw new NegocioException_60MN("Indique el banco del pago.");
            Validaciones_60MN.Largo(pago.Banco, LargoBanco, "El banco");
            Validaciones_60MN.TextoLibre(pago.Banco, "El banco");

            pago.NroComprobante = Validaciones_60MN.Normalizar(pago.NroComprobante).ToUpperInvariant();
            if (pago.NroComprobante.Length == 0)
                throw new NegocioException_60MN("Indique el número de comprobante de la transferencia.");
            Validaciones_60MN.Largo(pago.NroComprobante, LargoComprobante, "El número de comprobante");
            Validaciones_60MN.Lote(pago.NroComprobante, "El número de comprobante");
            // 3.1: comprobante repetido para ese banco.
            if (mapper.ExisteComprobante(pago.Banco, pago.NroComprobante))
                throw new NegocioException_60MN($"El comprobante {pago.NroComprobante} del {pago.Banco} ya está registrado en otro pago.");

            // 3.2: la fecha no puede ser futura ni anterior a la aprobación.
            if (pago.FechaPago.Date > DateTime.Today)
                throw new NegocioException_60MN("La fecha del pago no puede ser futura.");
            if (orden.FechaAprobacion.HasValue && pago.FechaPago.Date < orden.FechaAprobacion.Value.Date)
                throw new NegocioException_60MN($"La fecha del pago no puede ser anterior a la aprobación de la orden ({orden.FechaAprobacion:dd/MM/yyyy}).");

            if (pago.Importe <= 0)
                throw new NegocioException_60MN("El importe debe ser mayor a cero.");
            pago.Observaciones = Validaciones_60MN.Normalizar(pago.Observaciones);
            // 3.3: importe distinto del total de la orden.
            if (pago.Importe != orden.Total)
            {
                if (!confirmado)
                    throw new ConfirmacionRequeridaException_60MN(
                        $"El importe ($ {pago.Importe.ToString("N2", Ar)}) es distinto del total de la orden {orden.Numero} " +
                        $"($ {orden.Total.ToString("N2", Ar)}). ¿Desea registrarlo igual?");
                if (pago.Observaciones.Length == 0)
                    throw new NegocioException_60MN("El importe es distinto del total de la orden: indique el motivo en las observaciones.");
            }
            if (pago.Observaciones.Length > 0)
            {
                Validaciones_60MN.Largo(pago.Observaciones, LargoObservaciones, "La observación");
                Validaciones_60MN.TextoLibre(pago.Observaciones, "La observación");
            }
            else
                pago.Observaciones = null;

            pago.Id = mapper.Registrar(pago);
            if (pago.Id == 0)
                throw new NegocioException_60MN($"La orden {orden.Numero} cambió de estado mientras se registraba el pago. Vuelva a consultarla.");
            dv.ActualizarFila(TablaDV_60MN.PagoProveedor, pago.Id);
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, orden.Id);

            bitacora.Registrar(EventoSistema_60MN.PagoProveedorRegistrado,
                $"Pago de la orden {orden.Numero} a {orden.Proveedor}: $ {pago.Importe.ToString("N2", Ar)} por transferencia " +
                $"({pago.Banco}, comprobante {pago.NroComprobante}, {pago.FechaPago:dd/MM/yyyy})." +
                (pago.Importe != orden.Total ? $" Difiere del total ($ {orden.Total.ToString("N2", Ar)}): {pago.Observaciones}" : string.Empty));
        }
    }
}
