using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>CUN13 Configurar Margen y Alerta de Stock Mínimo.</summary>
    public partial class FrmConfiguracionStock_60MN : FormBase_60MN
    {
        private readonly ConfiguracionStockBLL_60MN configuracionBLL = new ConfiguracionStockBLL_60MN();
        private ConfiguracionStock_60MN? configuracion;

        public FrmConfiguracionStock_60MN()
        {
            InitializeComponent();
            dgvCategorias.AutoGenerateColumns = false;
            // Celda vacía = null (la categoría usa el margen general).
            colMargenCategoria.DefaultCellStyle.DataSourceNullValue = null;
        }

        /// <summary>Paso 2: parámetros vigentes.</summary>
        private void FrmConfiguracionStock_60MN_Load(object sender, EventArgs e)
        {
            try
            {
                configuracion = configuracionBLL.Obtener();
                nudMargen.Value = Math.Min(Math.Max(configuracion.MargenGeneral, nudMargen.Minimum), nudMargen.Maximum);
                nudStockMinimo.Value = Math.Min(Math.Max(configuracion.StockMinimoSugerido, 0), (int)nudStockMinimo.Maximum);
                dgvCategorias.DataSource = configuracion.Categorias;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
                BeginInvoke(Close);
            }
        }

        private void dgvCategorias_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != colMargenCategoria.Index) return;
            string texto = Convert.ToString(e.FormattedValue)?.Trim() ?? string.Empty;
            // 5.1: margen no numérico.
            if (texto.Length > 0 && !decimal.TryParse(texto, out _))
            {
                Mensajes_60MN.Advertir("El margen debe ser un valor numérico.");
                e.Cancel = true;
            }
        }

        private void dgvCategorias_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Mensajes_60MN.Advertir("El margen debe ser un valor numérico.");
            e.Cancel = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (configuracion == null) return;
            if (!dgvCategorias.EndEdit()) return;

            configuracion.MargenGeneral = nudMargen.Value;
            configuracion.StockMinimoSugerido = (int)nudStockMinimo.Value;

            // 3.1: margen fuera del rango habitual (0 a 100%) → confirmación explícita.
            IEnumerable<decimal> margenes = configuracion.Categorias.Where(c => c.Margen.HasValue).Select(c => c.Margen!.Value)
                                                         .Append(configuracion.MargenGeneral);
            if (margenes.Any(ConfiguracionStockBLL_60MN.MargenRequiereConfirmacion) &&
                !Mensajes_60MN.Confirmar("Hay márgenes fuera del rango habitual (0% a 100%). ¿Desea continuar?"))
                return;

            try
            {
                // 4.1 / 6.1: impacto antes de confirmar.
                ImpactoConfiguracion_60MN impacto = configuracionBLL.Simular(configuracion, chkRecalcular.Checked, chkAplicarMinimo.Checked);
                string detalle = string.Empty;
                if (chkRecalcular.Checked)
                    detalle += $"\n- Se recalculará el precio de venta de {impacto.ProductosARecalcular} producto(s).";
                if (chkAplicarMinimo.Checked)
                    detalle += $"\n- {impacto.AlertasNuevas} producto(s) quedarán en alerta de reposición.";
                if (!Mensajes_60MN.Confirmar("¿Confirma la nueva configuración?" + detalle))
                    return;

                configuracionBLL.Guardar(configuracion, chkRecalcular.Checked, chkAplicarMinimo.Checked);
                Mensajes_60MN.Informar("Configuración guardada correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();
    }
}
