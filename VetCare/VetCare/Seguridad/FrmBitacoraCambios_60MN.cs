using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUS10 Consultar Bitácora de Cambios: historial de Productos_C, que
    /// alimenta el trigger TR_Productos_BitacoraCambios desde CUN11, CUN12 y CUN14.
    /// </summary>
    public partial class FrmBitacoraCambios_60MN : FormBase_60MN
    {
        private readonly BitacoraCambiosBLL_60MN bitacoraCambiosBLL = new BitacoraCambiosBLL_60MN();

        public FrmBitacoraCambios_60MN()
        {
            InitializeComponent();
            dgvCambios.AutoGenerateColumns = false;
        }

        private void FrmBitacoraCambios_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            Buscar();
        }

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        private void Buscar()
        {
            DateTime? desde = dtpDesde.Checked ? dtpDesde.Value.Date : null;
            DateTime? hasta = dtpHasta.Checked ? dtpHasta.Value.Date : null;
            // 3.1: rango de fechas inválido.
            if (desde.HasValue && hasta.HasValue && desde > hasta)
            {
                Mensajes_60MN.Advertir("El rango de fechas es inválido: la fecha de inicio es posterior a la de fin.");
                return;
            }

            try
            {
                List<ProductoHistorial_60MN> cambios = bitacoraCambiosBLL.Consultar(desde, hasta, txtCodigo.Text, txtNombre.Text);
                dgvCambios.DataSource = cambios;
                // 4.1: sin registros.
                if (cambios.Count == 0)
                    Mensajes_60MN.Informar("No existen cambios para los filtros aplicados.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Paso 7: la versión vigente de cada producto (Act = 1) se destaca en verde.</summary>
        private void dgvCambios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvCambios.Rows)
                if (fila.DataBoundItem is ProductoHistorial_60MN version && version.Act)
                {
                    fila.DefaultCellStyle.BackColor = Color.Honeydew;
                    fila.DefaultCellStyle.Font = new Font(dgvCambios.Font, FontStyle.Bold);
                }
        }

        private List<ProductoHistorial_60MN> Seleccionadas() =>
            dgvCambios.SelectedRows.Cast<DataGridViewRow>()
                      .Select(f => f.DataBoundItem).OfType<ProductoHistorial_60MN>()
                      .OrderBy(v => v.FechaHora).ThenBy(v => v.Id)
                      .ToList();

        // 6.1: comparar dos versiones de un mismo producto.
        private void btnComparar_Click(object sender, EventArgs e)
        {
            List<ProductoHistorial_60MN> versiones = Seleccionadas();
            if (versiones.Count != 2 || versiones[0].ProductoId != versiones[1].ProductoId)
            {
                Mensajes_60MN.Advertir("Seleccione exactamente dos versiones del mismo producto (Ctrl + clic).");
                return;
            }
            using FrmCompararVersiones_60MN formulario = new FrmCompararVersiones_60MN(
                versiones[0], versiones[1], bitacoraCambiosBLL.Comparar(versiones[0], versiones[1]));
            formulario.ShowDialog(this);
        }

        // Recuperación de versiones previas.
        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            List<ProductoHistorial_60MN> versiones = Seleccionadas();
            if (versiones.Count != 1)
            {
                Mensajes_60MN.Advertir("Seleccione una única versión para restaurar.");
                return;
            }
            ProductoHistorial_60MN version = versiones[0];
            if (!Mensajes_60MN.Confirmar($"¿Restaurar los datos del producto {version.Codigo} a la versión del {version.FechaHora:dd/MM/yyyy HH:mm:ss}?\n" +
                                         "El stock actual no se modifica (se ajusta solo con movimientos de stock)."))
                return;
            try
            {
                new ProductoBLL_60MN().RestaurarVersion(version.Id);
                Mensajes_60MN.Informar("Versión restaurada correctamente.");
                Buscar();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
