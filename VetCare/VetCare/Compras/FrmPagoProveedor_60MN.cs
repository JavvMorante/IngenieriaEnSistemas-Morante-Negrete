using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN13 Registrar Pago al Proveedor (analista de compras): envía la orden aprobada al proveedor
    /// (impresa o en PDF) y registra el comprobante de la transferencia hecha en el home banking.
    /// </summary>
    public partial class FrmPagoProveedor_60MN : FormBase_60MN
    {
        private readonly PagoProveedorBLL_60MN pagoBLL = new PagoProveedorBLL_60MN();

        public FrmPagoProveedor_60MN()
        {
            InitializeComponent();
            dgvOrdenes.AutoGenerateColumns = false;
            cboBanco.MaxLength = PagoProveedorBLL_60MN.LargoBanco;
            txtComprobante.MaxLength = PagoProveedorBLL_60MN.LargoComprobante;
            txtObservaciones.MaxLength = PagoProveedorBLL_60MN.LargoObservaciones;
            dtpFecha.MaxDate = DateTime.Today;
            txtComprobante.KeyPress += (s, e) =>
            {
                char c = char.ToUpperInvariant(e.KeyChar);
                if (!char.IsControl(c) && !((c >= 'A' && c <= 'Z') || char.IsDigit(c) || c == '-' || c == '/'))
                {
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                }
            };
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private OrdenCompra_60MN? Seleccionada => dgvOrdenes.CurrentRow?.DataBoundItem as OrdenCompra_60MN;

        private void FrmPagoProveedor_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            try
            {
                cboBanco.Items.AddRange(pagoBLL.ListarBancos().ToArray<object>());
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            Cargar();
        }

        /// <summary>Paso 1: órdenes aprobadas (para enviar) o enviadas (para pagar).</summary>
        private void Cargar()
        {
            try
            {
                List<OrdenCompra_60MN> ordenes = pagoBLL.ListarOrdenesAPagar();
                dgvOrdenes.DataSource = ordenes;
                grpPago.Enabled = ordenes.Count > 0;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Propone el total de la orden y la fecha de hoy.</summary>
        private void dgvOrdenes_SelectionChanged(object sender, EventArgs e)
        {
            if (Seleccionada is not OrdenCompra_60MN orden) return;
            nudImporte.Value = Math.Min(orden.Total, nudImporte.Maximum);
            dtpFecha.Value = DateTime.Today;
            txtComprobante.Clear();
            txtObservaciones.Clear();
        }

        /// <summary>Paso 2: la orden pasa a ENVIADA y se imprime (o se guarda en PDF) para mandarla al proveedor.</summary>
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (Seleccionada is not OrdenCompra_60MN seleccionada) return;
            try
            {
                OrdenCompra_60MN orden = pagoBLL.MarcarEnviada(seleccionada.Id);
                string? impresora = ImpresionTexto_60MN.Imprimir(this, $"{T("msg.OrdenDeCompra", "Orden de compra")} {orden.Numero}",
                    DocumentosCompra_60MN.OrdenDeCompra(orden), ImpresionTexto_60MN.ImpresoraPdf);
                Mensajes_60MN.Informar(impresora == null
                    ? string.Format(T("msg.OrdenEnviadaSinImprimir", "La orden {0} quedó como enviada. Puede volver a imprimirla con el mismo botón."), orden.Numero)
                    : string.Format(T("msg.OrdenEnviada", "La orden {0} quedó como enviada ({1})."), orden.Numero, impresora));
                Cargar();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (Seleccionada is not OrdenCompra_60MN orden)
            {
                Mensajes_60MN.Advertir(T("msg.ElegirOrden", "Seleccione la orden de compra."));
                return;
            }
            Registrar(new PagoProveedor_60MN
            {
                OrdenCompraId = orden.Id,
                Banco = cboBanco.Text,
                NroComprobante = txtComprobante.Text,
                FechaPago = dtpFecha.Value.Date,
                Importe = nudImporte.Value,
                Observaciones = txtObservaciones.Text
            }, orden, confirmado: false);
        }

        /// <summary>Pasos 3 a 6. Si el importe difiere del total, la BLL pide confirmación (y el motivo).</summary>
        private void Registrar(PagoProveedor_60MN pago, OrdenCompra_60MN orden, bool confirmado)
        {
            try
            {
                pagoBLL.Registrar(pago, confirmado);
                Mensajes_60MN.Informar(string.Format(T("msg.PagoRegistrado",
                    "Se registró el pago de la orden {0}: queda pagada y el depósito ya puede recibir la mercadería."), orden.Numero));
                txtComprobante.Clear();
                txtObservaciones.Clear();
                Cargar();
            }
            catch (ConfirmacionRequeridaException_60MN ex)
            {
                if (Mensajes_60MN.Confirmar(ex.Message))
                    Registrar(pago, orden, confirmado: true);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
