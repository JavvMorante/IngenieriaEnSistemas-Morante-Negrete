using System.Globalization;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// VENTAS > Ventas registradas (CUN05): consulta de las ventas del carrito por período y,
    /// opcionalmente, por cliente, con el detalle de la venta seleccionada. Solo consulta:
    /// el sistema no factura ni anula ventas.
    /// </summary>
    public partial class FrmVentas_60MN : FormBase_60MN
    {
        private readonly VentaBLL_60MN ventaBLL = new VentaBLL_60MN();
        private Cliente_60MN? cliente;

        public FrmVentas_60MN()
        {
            InitializeComponent();
            dgvVentas.AutoGenerateColumns = false;
            dgvDetalle.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("es-AR");

        private void FrmVentas_60MN_Load(object sender, EventArgs e)
        {
            dtpHasta.Value = DateTime.Today;
            dtpDesde.Value = DateTime.Today.AddDays(-30);
            MostrarCliente();
            Buscar();
        }

        protected override void AlCambiarIdioma()
        {
            MostrarCliente();
            ActualizarResumen();
        }

        private void MostrarCliente()
        {
            txtCliente.Text = cliente == null ? T("msg.TodosLosClientes", "(todos los clientes)") : $"{cliente.NombreCompleto} - {cliente.Documento}";
            btnQuitarCliente.Enabled = cliente != null;
        }

        private void btnElegirCliente_Click(object sender, EventArgs e)
        {
            using FrmBuscarCliente_60MN selector = new FrmBuscarCliente_60MN("msg.ElegirClienteConsulta", "Elija el cliente a consultar");
            if (selector.ShowDialog(this) != DialogResult.OK || selector.Seleccionado == null) return;
            cliente = selector.Seleccionado;
            MostrarCliente();
            Buscar();
        }

        private void btnQuitarCliente_Click(object sender, EventArgs e)
        {
            cliente = null;
            MostrarCliente();
            Buscar();
        }

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        private void Buscar()
        {
            try
            {
                dgvVentas.DataSource = ventaBLL.ListarVentas(dtpDesde.Value, dtpHasta.Value, cliente?.Id);
                if (dgvVentas.Rows.Count == 0)
                    dgvDetalle.DataSource = null;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            ActualizarResumen();
        }

        private List<VentaEncabezado_60MN> Ventas => dgvVentas.DataSource as List<VentaEncabezado_60MN> ?? new List<VentaEncabezado_60MN>();

        private void ActualizarResumen()
        {
            List<VentaEncabezado_60MN> ventas = Ventas;
            lblResumen.Text = string.Format(T("msg.ResumenVentas", "{0} venta(s) · {1} unidad(es) · Total $ {2}"),
                ventas.Count, ventas.Sum(v => v.CantidadItems), ventas.Sum(v => v.Total).ToString("N2", Ar));
        }

        private void dgvVentas_SelectionChanged(object sender, EventArgs e)
        {
            VentaEncabezado_60MN? venta = dgvVentas.CurrentRow?.DataBoundItem as VentaEncabezado_60MN;
            dgvDetalle.DataSource = venta?.Detalle.ToList();
            lblDetalle.Text = venta == null
                ? T("msg.DetalleVenta", "Detalle de la venta")
                : $"{T("msg.DetalleVenta", "Detalle de la venta")} {venta.Numero}";
        }
    }
}
