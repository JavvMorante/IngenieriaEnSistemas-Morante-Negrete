using System.Text;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN12 Aprobar Orden de Compra (gerente): revisa la orden, su origen (solicitud y cotización
    /// elegida) y las advertencias de precio, y la aprueba o la rechaza con motivo.
    /// </summary>
    public partial class FrmAprobarOrden_60MN : FormBase_60MN
    {
        private readonly OrdenCompraBLL_60MN ordenBLL = new OrdenCompraBLL_60MN();

        public FrmAprobarOrden_60MN()
        {
            InitializeComponent();
            dgvOrdenes.AutoGenerateColumns = false;
            dgvDetalle.AutoGenerateColumns = false;
            txtMotivoRechazo.MaxLength = OrdenCompraBLL_60MN.LargoMotivo;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private OrdenCompra_60MN? Seleccionada => dgvOrdenes.CurrentRow?.DataBoundItem as OrdenCompra_60MN;

        private void FrmAprobarOrden_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            Cargar();
        }

        protected override void AlCambiarIdioma() => MostrarOrden();

        private void Cargar()
        {
            try
            {
                List<OrdenCompra_60MN> ordenes = ordenBLL.ListarPendientesDeAprobacion();
                dgvOrdenes.DataSource = ordenes;
                if (ordenes.Count == 0)
                    MostrarOrden();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void dgvOrdenes_SelectionChanged(object sender, EventArgs e) => MostrarOrden();

        /// <summary>Detalle de la orden y su origen: la solicitud del encargado y la cotización elegida.</summary>
        private void MostrarOrden()
        {
            OrdenCompra_60MN? orden = Seleccionada;
            dgvDetalle.DataSource = orden?.Detalle;
            if (orden == null)
            {
                txtOrigen.Text = T("msg.SinOrdenesPendientes", "No hay órdenes pendientes de aprobación.");
                return;
            }
            StringBuilder sb = new StringBuilder();
            try
            {
                SolicitudReposicion_60MN? solicitud = ordenBLL.ObtenerSolicitud(orden.SolicitudId);
                if (solicitud != null)
                {
                    sb.AppendLine($"{T("msg.Solicitud", "Solicitud")} {solicitud.Numero} - {solicitud.FechaHora:dd/MM/yyyy HH:mm} ({solicitud.Usuario})");
                    if (!string.IsNullOrEmpty(solicitud.Observaciones))
                        sb.AppendLine(solicitud.Observaciones);
                    foreach (SolicitudDetalle_60MN d in solicitud.Detalle)
                        sb.AppendLine($"  {d.CantidadPedida} x {d.Producto} (stock {d.StockActual}, {T("msg.Minimo", "mínimo")} {d.StockMinimo})");
                    sb.AppendLine();
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.Message);
            }
            sb.AppendLine($"{T("msg.Cotizacion", "Cotización")} {orden.NroCotizacion} - {orden.Proveedor}");
            sb.AppendLine($"{T("msg.Plazo", "Plazo de entrega")}: {orden.PlazoEntregaDias} {T("msg.Dias", "días")} · {orden.CondicionPago}");
            if (!string.IsNullOrEmpty(orden.MotivoEleccion))
                sb.AppendLine($"{T("msg.MotivoEleccion", "Motivo de elección")}: {orden.MotivoEleccion}");
            if (orden.TieneAdvertencias)
            {
                sb.AppendLine();
                sb.AppendLine($"⚠ {T("msg.ConAdvertencias", "Advertencias")}: {orden.Advertencias}");
            }
            txtOrigen.Text = sb.ToString();
        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            if (Seleccionada is not OrdenCompra_60MN orden) return;
            if (!Mensajes_60MN.Confirmar(string.Format(T("msg.ConfirmarAprobar", "¿Aprobar la orden {0} a {1} por $ {2:N2}?"),
                    orden.Numero, orden.Proveedor, orden.Total)))
                return;
            Aprobar(orden, confirmado: false);
        }

        /// <summary>Si la orden supera el tope mensual de compras, la BLL pide confirmación.</summary>
        private void Aprobar(OrdenCompra_60MN orden, bool confirmado)
        {
            try
            {
                ordenBLL.Aprobar(orden.Id, confirmado);
                Mensajes_60MN.Informar(string.Format(T("msg.OrdenAprobada", "La orden {0} quedó aprobada: el analista de compras ya puede enviarla y pagarla."), orden.Numero));
                Cargar();
            }
            catch (ConfirmacionRequeridaException_60MN ex)
            {
                if (Mensajes_60MN.Confirmar(ex.Message))
                    Aprobar(orden, confirmado: true);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
                Cargar();
            }
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            if (Seleccionada is not OrdenCompra_60MN orden) return;
            try
            {
                ordenBLL.Rechazar(orden.Id, txtMotivoRechazo.Text);
                txtMotivoRechazo.Clear();
                Mensajes_60MN.Informar(string.Format(T("msg.OrdenRechazada", "La orden {0} quedó rechazada: el analista puede corregirla o anularla."), orden.Numero));
                Cargar();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
