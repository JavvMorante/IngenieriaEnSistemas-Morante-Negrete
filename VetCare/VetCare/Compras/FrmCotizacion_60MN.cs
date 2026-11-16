using System.ComponentModel;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN11 paso 3: el analista carga la cotización que mandó un proveedor (fuera del sistema).
    /// En la corrección de una orden rechazada también se pueden cambiar las cantidades.
    /// </summary>
    public partial class FrmCotizacion_60MN : FormBase_60MN
    {
        /// <summary>Un renglón de la grilla de precios.</summary>
        public class FilaPrecio_60MN
        {
            public int ProductoId { get; set; }

            public string Producto { get; set; } = string.Empty;

            public int Cantidad { get; set; }

            public decimal Precio { get; set; }

            public decimal CostoActual { get; set; }

            public decimal Subtotal => Math.Round(Cantidad * Precio, 2);
        }

        /// <summary>Condiciones de pago habituales (el combo admite escribir otra).</summary>
        private static readonly string[] Condiciones =
            { "Contado", "Transferencia anticipada", "Transferencia a 15 días", "Transferencia a 30 días", "Cuenta corriente a 30 días" };

        private readonly List<Proveedor_60MN> proveedores;
        private readonly BindingList<FilaPrecio_60MN> filas;
        private readonly bool permiteCantidades;
        private readonly Cotizacion_60MN? inicial;

        /// <summary>La cotización cargada (al aceptar).</summary>
        public Cotizacion_60MN Cotizacion { get; private set; } = new Cotizacion_60MN();

        /// <summary>Cantidad de cada producto (ProductoId → cantidad); cambia solo en la corrección.</summary>
        public Dictionary<int, int> Cantidades => filas.ToDictionary(f => f.ProductoId, f => f.Cantidad);

        public FrmCotizacion_60MN() : this(new List<Proveedor_60MN>(), new List<FilaPrecio_60MN>(), false, null) { }

        /// <param name="permiteCantidades">true en la corrección de una orden rechazada.</param>
        /// <param name="inicial">Datos que se proponen (la cotización de la orden que se corrige).</param>
        public FrmCotizacion_60MN(List<Proveedor_60MN> proveedores, List<FilaPrecio_60MN> filas, bool permiteCantidades, Cotizacion_60MN? inicial)
        {
            InitializeComponent();
            this.proveedores = proveedores;
            this.filas = new BindingList<FilaPrecio_60MN>(filas);
            this.permiteCantidades = permiteCantidades;
            this.inicial = inicial;
            dgvPrecios.AutoGenerateColumns = false;
            txtNroCotizacion.MaxLength = OrdenCompraBLL_60MN.LargoNroCotizacion;
            cboCondicion.MaxLength = OrdenCompraBLL_60MN.LargoCondicionPago;
            nudPlazo.Maximum = OrdenCompraBLL_60MN.PlazoMaximoDias;
            txtNroCotizacion.KeyPress += (s, e) =>
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

        private void FrmCotizacion_60MN_Load(object sender, EventArgs e)
        {
            cboProveedor.DisplayMember = nameof(Proveedor_60MN.RazonSocial);
            cboProveedor.ValueMember = nameof(Proveedor_60MN.Id);
            cboProveedor.DataSource = proveedores;
            cboProveedor.SelectedIndex = -1;
            cboCondicion.Items.AddRange(Condiciones);
            colCantidad.ReadOnly = !permiteCantidades;
            if (permiteCantidades)
                colCantidad.DefaultCellStyle.BackColor = Color.LightYellow;
            dgvPrecios.DataSource = filas;

            if (inicial != null)
            {
                cboProveedor.SelectedValue = inicial.ProveedorId;
                txtNroCotizacion.Text = inicial.NroCotizacion;
                nudPlazo.Value = Math.Min(inicial.PlazoEntregaDias, (int)nudPlazo.Maximum);
                cboCondicion.Text = inicial.CondicionPago;
            }
            MostrarTotal();
        }

        protected override void AlCambiarIdioma()
        {
            if (permiteCantidades)
                Text = T("FrmCotizacion_60MN.TituloCorreccion", "Corregir orden rechazada");
            MostrarTotal();
        }

        private void MostrarTotal() =>
            lblTotal.Text = $"{T("msg.TotalCotizacion", "Total de la cotización")}: $ {filas.Sum(f => f.Subtotal):N2}";

        private void dgvPrecios_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvPrecios.InvalidateRow(e.RowIndex);
            MostrarTotal();
        }

        private void dgvPrecios_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Mensajes_60MN.Advertir(T("msg.NumeroInvalido", "Ingrese un número válido (los decimales van con coma)."));
            e.Cancel = true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            dgvPrecios.EndEdit();
            if (cboProveedor.SelectedItem is not Proveedor_60MN proveedor)
            {
                Mensajes_60MN.Advertir(T("msg.ElegirProveedor", "Seleccione el proveedor de la cotización."));
                return;
            }
            if (txtNroCotizacion.Text.Trim().Length == 0)
            {
                Mensajes_60MN.Advertir(T("msg.NroCotizacionObligatorio", "Ingrese el número de la cotización."));
                txtNroCotizacion.Focus();
                return;
            }
            if (cboCondicion.Text.Trim().Length == 0)
            {
                Mensajes_60MN.Advertir(T("msg.CondicionObligatoria", "Ingrese la condición de pago."));
                cboCondicion.Focus();
                return;
            }
            FilaPrecio_60MN? invalida = filas.FirstOrDefault(f => f.Precio <= 0 || f.Cantidad <= 0);
            if (invalida != null)
            {
                Mensajes_60MN.Advertir(string.Format(T("msg.PrecioObligatorio", "Ingrese un precio y una cantidad mayores a cero para {0}."), invalida.Producto));
                return;
            }

            Cotizacion = new Cotizacion_60MN
            {
                ProveedorId = proveedor.Id,
                Proveedor = proveedor.RazonSocial,
                NroCotizacion = txtNroCotizacion.Text.Trim().ToUpperInvariant(),
                PlazoEntregaDias = (int)nudPlazo.Value,
                CondicionPago = cboCondicion.Text.Trim()
            };
            foreach (FilaPrecio_60MN f in filas)
                Cotizacion.Precios[f.ProductoId] = f.Precio;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
