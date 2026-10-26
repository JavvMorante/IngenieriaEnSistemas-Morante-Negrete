using System.Globalization;
using System.Text;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>CUN15 Generar Reporte de Existencias (generales, bajo stock mínimo o próximos a vencer).</summary>
    public partial class FrmReporteExistencias_60MN : FormBase_60MN
    {
        private readonly ReporteExistenciasBLL_60MN reporteBLL = new ReporteExistenciasBLL_60MN();
        private readonly ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();
        private List<Producto_60MN> resultado = new List<Producto_60MN>();
        private string criterios = string.Empty;

        public FrmReporteExistencias_60MN()
        {
            InitializeComponent();
            dgvReporte.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        /// <summary>Paso 2: tipos de reporte y filtros disponibles.</summary>
        private void FrmReporteExistencias_60MN_Load(object sender, EventArgs e)
        {
            CargarTipos();
            try
            {
                cboCategoria.Items.Add(OpcionCombo_60MN.Todos);
                foreach (Categoria_60MN c in productoBLL.ListarCategorias())
                    cboCategoria.Items.Add(new OpcionCombo_60MN(c.Nombre, c.Id));
                cboCategoria.SelectedIndex = 0;

                cboProveedor.Items.Add(OpcionCombo_60MN.Todos);
                foreach (Proveedor_60MN p in productoBLL.ListarProveedores())
                    cboProveedor.Items.Add(new OpcionCombo_60MN(p.ToString(), p.Id));
                cboProveedor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            dtpDesde.Value = DateTime.Today;
            dtpHasta.Value = DateTime.Today.AddDays(ReporteExistenciasBLL_60MN.DiasProximosAVencer);
        }

        protected override void AlCambiarIdioma() => CargarTipos();

        private void CargarTipos()
        {
            int seleccionado = Math.Max(cboTipo.SelectedIndex, 0);
            cboTipo.Items.Clear();
            cboTipo.Items.Add(new OpcionCombo_60MN(T("msg.ExistenciasGenerales", "Existencias generales"), TipoReporteExistencias_60MN.ExistenciasGenerales));
            cboTipo.Items.Add(new OpcionCombo_60MN(T("msg.BajoStockMinimo", "Bajo stock mínimo"), TipoReporteExistencias_60MN.BajoStockMinimo));
            cboTipo.Items.Add(new OpcionCombo_60MN(T("msg.ProximosAVencer", "Próximos a vencer"), TipoReporteExistencias_60MN.ProximosAVencer));
            cboTipo.SelectedIndex = seleccionado;
        }

        private TipoReporteExistencias_60MN TipoSeleccionado =>
            OpcionCombo_60MN.ValorDe<TipoReporteExistencias_60MN>(cboTipo) ?? TipoReporteExistencias_60MN.ExistenciasGenerales;

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // "Próximos a vencer" usa el rango de vencimiento.
            if (TipoSeleccionado == TipoReporteExistencias_60MN.ProximosAVencer)
                chkVencimiento.Checked = true;
        }

        private void chkVencimiento_CheckedChanged(object sender, EventArgs e) =>
            dtpDesde.Enabled = dtpHasta.Enabled = chkVencimiento.Checked;

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            DateTime? desde = chkVencimiento.Checked ? dtpDesde.Value.Date : null;
            DateTime? hasta = chkVencimiento.Checked ? dtpHasta.Value.Date : null;
            try
            {
                // Paso 4 (flujo 3.1: rango de fechas inválido lo valida la BLL).
                resultado = reporteBLL.Generar(TipoSeleccionado, OpcionCombo_60MN.ValorDe<int>(cboCategoria),
                                               OpcionCombo_60MN.ValorDe<int>(cboProveedor), desde, hasta);
                dgvReporte.DataSource = resultado;
                criterios = $"{cboTipo.Text} · {lblCategoria.Text}: {cboCategoria.Text} · {lblProveedor.Text}: {cboProveedor.Text}" +
                            (desde.HasValue ? $" · {chkVencimiento.Text} {desde:dd/MM/yyyy} {lblHasta.Text} {hasta:dd/MM/yyyy}" : string.Empty);

                // 4.1: sin resultados.
                if (resultado.Count == 0)
                {
                    lblResumen.Text = T("msg.SinResultados", "No se encontraron productos con los criterios indicados.");
                    btnImprimir.Enabled = btnExportar.Enabled = false;
                    return;
                }
                // Paso 5: totales y valorización.
                lblResumen.Text = $"{resultado.Count} producto(s) · {resultado.Sum(p => p.StockActual)} unidad(es) · " +
                                  $"Valorización $ {resultado.Sum(p => p.Valorizacion).ToString("N2", CultureInfo.GetCultureInfo("es-AR"))}";
                btnImprimir.Enabled = btnExportar.Enabled = true;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void dgvReporte_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvReporte.Rows)
            {
                if (fila.DataBoundItem is not Producto_60MN p) continue;
                if (p.Vencido)
                    fila.DefaultCellStyle.BackColor = Color.MistyRose;
                else if (p.BajoStockMinimo)
                    fila.DefaultCellStyle.BackColor = Color.LightYellow;
            }
        }

        /// <summary>Paso 6: impresión o PDF.</summary>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            CultureInfo ar = CultureInfo.GetCultureInfo("es-AR");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VETCARE - REPORTE DE EXISTENCIAS");
            sb.AppendLine(criterios);
            sb.AppendLine($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(new string('=', 96));
            sb.AppendLine($"{"Código",-10}{"Nombre",-30}{"Lote",-10}{"Stock",7}{"Mín.",6}  {"Vence",-10}{"Costo",11}{"Valoriz.",12}");
            sb.AppendLine(new string('-', 96));
            foreach (Producto_60MN p in resultado)
            {
                string nombre = p.Nombre.Length > 29 ? p.Nombre.Substring(0, 29) : p.Nombre;
                sb.AppendLine($"{p.Codigo,-10}{nombre,-30}{p.Lote,-10}{p.StockActual,7}{p.StockMinimo,6}  {p.FechaVencimiento,-10:dd/MM/yyyy}" +
                              $"{p.PrecioCosto.ToString("N2", ar),11}{p.Valorizacion.ToString("N2", ar),12}");
            }
            sb.AppendLine(new string('=', 96));
            sb.AppendLine(lblResumen.Text);
            try
            {
                if (ImpresionTexto_60MN.Imprimir(this, "Reporte de existencias", sb.ToString()) != null)
                    Mensajes_60MN.Informar("Reporte enviado a la impresora.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dialogo = new SaveFileDialog
            {
                FileName = $"Existencias_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Filter = "CSV (*.csv)|*.csv",
                DefaultExt = "csv"
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;

            static string Campo(string valor) => "\"" + valor.Replace("\"", "\"\"") + "\"";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Codigo;Nombre;Categoria;Proveedor;Lote;Stock;StockMinimo;Vencimiento;Costo;Valorizacion");
            foreach (Producto_60MN p in resultado)
                sb.AppendLine(string.Join(";", Campo(p.Codigo), Campo(p.Nombre), Campo(p.CategoriaNombre), Campo(p.ProveedorNombre),
                    Campo(p.Lote), p.StockActual, p.StockMinimo, p.FechaVencimiento.ToString("dd/MM/yyyy"),
                    p.PrecioCosto.ToString("0.00", CultureInfo.GetCultureInfo("es-AR")),
                    p.Valorizacion.ToString("0.00", CultureInfo.GetCultureInfo("es-AR"))));
            try
            {
                File.WriteAllText(dialogo.FileName, sb.ToString(), new UTF8Encoding(true));
                Mensajes_60MN.Informar("Reporte exportado correctamente.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
