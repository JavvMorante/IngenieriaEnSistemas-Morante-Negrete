using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN15 Generar Reporte de Compras (gerente o analista de compras): órdenes del período con lo
    /// pagado y lo recibido, subtotales por proveedor y totales. Se imprime o se guarda en PDF.
    /// </summary>
    public partial class FrmReporteCompras_60MN : FormBase_60MN
    {
        private readonly ReporteComprasBLL_60MN reporteBLL = new ReporteComprasBLL_60MN();
        private ReporteCompras_60MN? reporte;

        public FrmReporteCompras_60MN()
        {
            InitializeComponent();
            dgvOrdenes.AutoGenerateColumns = false;
            dgvTotales.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmReporteCompras_60MN_Load(object sender, EventArgs e)
        {
            DateTime hoy = DateTime.Today;
            dtpDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
            dtpHasta.Value = hoy;
            try
            {
                cboProveedor.Items.Add(OpcionCombo_60MN.Todos);
                foreach (Proveedor_60MN p in reporteBLL.ListarProveedores())
                    cboProveedor.Items.Add(new OpcionCombo_60MN(p.RazonSocial, p.Id));
                cboProveedor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            CargarEstados();
            MostrarTotales();
        }

        /// <summary>Estados con su descripción; se re-arma al cambiar de idioma conservando la elección.</summary>
        private void CargarEstados()
        {
            int seleccion = Math.Max(cboEstado.SelectedIndex, 0);
            cboEstado.Items.Clear();
            cboEstado.Items.Add(OpcionCombo_60MN.Todos);
            foreach (EstadoOrden_60MN estado in Enum.GetValues(typeof(EstadoOrden_60MN)))
                cboEstado.Items.Add(new OpcionCombo_60MN(T("estadoOrden." + estado, estado.Descripcion()), estado));
            cboEstado.SelectedIndex = seleccion;
        }

        protected override void AlCambiarIdioma()
        {
            if (cboEstado.Items.Count > 0)
                CargarEstados();
            MostrarTotales();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                reporte = reporteBLL.Generar(new FiltroReporteCompra_60MN
                {
                    Desde = dtpDesde.Value.Date,
                    Hasta = dtpHasta.Value.Date,
                    ProveedorId = OpcionCombo_60MN.ValorDe<int>(cboProveedor),
                    Estado = OpcionCombo_60MN.ValorDe<EstadoOrden_60MN>(cboEstado)
                });
                dgvOrdenes.DataSource = reporte.Filas;
                dgvTotales.DataSource = reporte.PorProveedor;
                if (reporte.Filas.Count == 0)
                    Mensajes_60MN.Informar(T("msg.SinCompras", "No hay órdenes de compra para los filtros elegidos."));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            MostrarTotales();
        }

        private void MostrarTotales()
        {
            lblTotales.Text = reporte == null
                ? string.Empty
                : $"{T("msg.TotalComprado", "Total comprado (aprobado o posterior)")}: $ {reporte.TotalComprado:N2}\n" +
                  $"{T("msg.TotalPagado", "Total pagado")}: $ {reporte.TotalPagado:N2}\n" +
                  $"{T("msg.PendienteRecibir", "Pendiente de recibir")}: {reporte.UnidadesPendientes} {T("msg.Unidades", "unidad(es)")}";
        }

        private void btnExportarPdf_Click(object sender, EventArgs e) => Imprimir(ImpresionTexto_60MN.ImpresoraPdf);

        private void btnImprimir_Click(object sender, EventArgs e) => Imprimir(null);

        /// <summary>Imprime el reporte generado; con "Microsoft Print to PDF" se guarda como archivo PDF.</summary>
        private void Imprimir(string? impresora)
        {
            if (reporte == null)
            {
                Mensajes_60MN.Advertir(T("msg.GenerarPrimero", "Genere el reporte primero."));
                return;
            }
            try
            {
                string texto = DocumentosCompra_60MN.ReporteDeCompras(reporte, cboProveedor.Text, cboEstado.Text);
                ImpresionTexto_60MN.Imprimir(this, T("FrmReporteCompras_60MN", "Reporte de compras"), texto, impresora);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
