using BLL_60MN;
using DAL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// Selector de cliente: busca clientes activos con la misma búsqueda de CUN01
    /// (documento, apellido o nombre de mascota) y devuelve el elegido en Seleccionado.
    /// Lo usan CUN02 Gestionar Mascotas (una mascota no puede existir sin cliente) y
    /// CUN05 (el carrito abierto desde VENTAS > Carrito).
    /// </summary>
    public partial class FrmBuscarCliente_60MN : FormBase_60MN
    {
        private readonly ClienteBLL_60MN clienteBLL = new ClienteBLL_60MN();
        private readonly string? claveTitulo;
        private readonly string? tituloPorDefecto;

        /// <summary>Cliente elegido (con DialogResult.OK).</summary>
        public Cliente_60MN? Seleccionado { get; private set; }

        public FrmBuscarCliente_60MN() : this(null, null) { }

        /// <param name="claveTitulo">Clave de traducción del título; null deja el del diseñador.</param>
        public FrmBuscarCliente_60MN(string? claveTitulo, string? tituloPorDefecto)
        {
            InitializeComponent();
            this.claveTitulo = claveTitulo;
            this.tituloPorDefecto = tituloPorDefecto;
            dgvClientes.AutoGenerateColumns = false;
            txtValor.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                Buscar();
            };
            dgvClientes.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.Handled = true;
                Elegir();
            };
            dgvClientes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) Elegir(); };
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmBuscarCliente_60MN_Load(object sender, EventArgs e) => CargarCriterios();

        protected override void AlCambiarIdioma()
        {
            int seleccionado = Math.Max(cboCriterio.SelectedIndex, 0);
            CargarCriterios();
            cboCriterio.SelectedIndex = seleccionado;
            if (claveTitulo != null)
                lblTitulo.Text = T(claveTitulo, tituloPorDefecto ?? lblTitulo.Text);
        }

        private void CargarCriterios()
        {
            cboCriterio.Items.Clear();
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioDocumento", "Documento"), CriterioBusquedaCliente_60MN.Documento));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioApellido", "Apellido"), CriterioBusquedaCliente_60MN.Apellido));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioMascota", "Nombre de mascota"), CriterioBusquedaCliente_60MN.NombreMascota));
            cboCriterio.SelectedIndex = 0;
        }

        private CriterioBusquedaCliente_60MN Criterio =>
            OpcionCombo_60MN.ValorDe<CriterioBusquedaCliente_60MN>(cboCriterio) ?? CriterioBusquedaCliente_60MN.Documento;

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        private void Buscar()
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text))
            {
                Mensajes_60MN.Advertir("Ingrese el valor a buscar.");
                txtValor.Focus();
                return;
            }
            try
            {
                // Solo clientes activos: a un cliente dado de baja no se le agregan mascotas.
                List<Cliente_60MN> clientes = clienteBLL.Buscar(Criterio, txtValor.Text, incluirBajas: false);
                dgvClientes.DataSource = clientes;
                btnAceptar.Enabled = clientes.Count > 0;
                if (clientes.Count == 0)
                {
                    Mensajes_60MN.Informar(T("msg.SinClientesActivos",
                        "No se encontraron clientes activos. Registre el cliente desde MAESTROS > Clientes."));
                    return;
                }
                dgvClientes.Focus();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e) => Elegir();

        private void Elegir()
        {
            if (!(dgvClientes.CurrentRow?.DataBoundItem is Cliente_60MN cliente))
            {
                Mensajes_60MN.Advertir("Seleccione un cliente.");
                return;
            }
            Seleccionado = cliente;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
