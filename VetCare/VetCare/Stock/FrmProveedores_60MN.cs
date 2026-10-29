using BLL_60MN;
using DAL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN11 Gestionar Proveedores (MAESTROS > Proveedores): búsqueda por razón social o CUIT,
    /// "Productos que provee" del proveedor seleccionado, alta, modificación, baja lógica y
    /// reactivación. Al abrir se listan todos los proveedores activos.
    /// </summary>
    public partial class FrmProveedores_60MN : FormBase_60MN
    {
        private readonly ProveedorBLL_60MN proveedorBLL = new ProveedorBLL_60MN();

        public FrmProveedores_60MN()
        {
            InitializeComponent();
            dgvProveedores.AutoGenerateColumns = false;
            dgvProductos.AutoGenerateColumns = false;
            txtValor.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                Buscar(null);
            };
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmProveedores_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            CargarCriterios();
            Buscar(null);
        }

        protected override void AlCambiarIdioma()
        {
            int seleccionado = Math.Max(cboCriterio.SelectedIndex, 0);
            CargarCriterios();
            cboCriterio.SelectedIndex = seleccionado;
            MostrarProductos(Seleccionado);
        }

        private void CargarCriterios()
        {
            cboCriterio.Items.Clear();
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioRazonSocial", "Razón social"), CriterioBusquedaProveedor_60MN.RazonSocial));
            cboCriterio.Items.Add(new OpcionCombo_60MN("CUIT", CriterioBusquedaProveedor_60MN.Cuit));
            cboCriterio.SelectedIndex = 0;
        }

        private CriterioBusquedaProveedor_60MN Criterio =>
            OpcionCombo_60MN.ValorDe<CriterioBusquedaProveedor_60MN>(cboCriterio) ?? CriterioBusquedaProveedor_60MN.RazonSocial;

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar(null);

        private void chkIncluirBajas_CheckedChanged(object sender, EventArgs e) => Buscar(Seleccionado?.Id);

        /// <summary>Con el valor vacío lista todos los proveedores.</summary>
        private void Buscar(int? seleccionarId)
        {
            try
            {
                List<Proveedor_60MN> proveedores = proveedorBLL.Buscar(Criterio, txtValor.Text, chkIncluirBajas.Checked);
                dgvProveedores.DataSource = proveedores;
                foreach (DataGridViewRow fila in dgvProveedores.Rows)
                    if (fila.DataBoundItem is Proveedor_60MN p && p.Id == seleccionarId)
                        dgvProveedores.CurrentCell = fila.Cells[0];
                if (proveedores.Count == 0)
                    MostrarProductos(null);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            ActualizarBotones();
        }

        private Proveedor_60MN? Seleccionado => dgvProveedores.CurrentRow?.DataBoundItem as Proveedor_60MN;

        private void dgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            MostrarProductos(Seleccionado);
            ActualizarBotones();
        }

        private void ActualizarBotones()
        {
            Proveedor_60MN? proveedor = Seleccionado;
            btnModificar.Enabled = proveedor != null && proveedor.Activo;
            btnDarBaja.Enabled = proveedor != null && proveedor.Activo;
            btnReactivar.Enabled = proveedor != null && !proveedor.Activo;
        }

        /// <summary>"Productos que provee": los activos primero; los dados de baja en gris.</summary>
        private void MostrarProductos(Proveedor_60MN? proveedor)
        {
            if (proveedor == null)
            {
                dgvProductos.DataSource = null;
                lblProductos.Text = T("msg.ProductosQueProvee", "Productos que provee");
                return;
            }
            try
            {
                List<Producto_60MN> productos = proveedorBLL.ListarProductos(proveedor.Id);
                dgvProductos.DataSource = productos;
                lblProductos.Text = $"{T("msg.ProductosQueProvee", "Productos que provee")}: {proveedor.RazonSocial} ({productos.Count})";
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in ((DataGridView)sender).Rows)
                if ((fila.DataBoundItem is Proveedor_60MN p && !p.Activo) || (fila.DataBoundItem is Producto_60MN pr && !pr.Activo))
                    fila.DefaultCellStyle.BackColor = Color.Gainsboro;
        }

        private void dgvProveedores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnModificar.Visible && btnModificar.Enabled)
                btnModificar.PerformClick();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using FrmProveedor_60MN formulario = new FrmProveedor_60MN(null);
            if (formulario.ShowDialog(this) != DialogResult.OK) return;
            // El proveedor nuevo queda a la vista y seleccionado.
            txtValor.Clear();
            Buscar(formulario.ProveedorId);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Proveedor_60MN? proveedor = Seleccionado;
            if (proveedor == null) return;
            using FrmProveedor_60MN formulario = new FrmProveedor_60MN(proveedor);
            if (formulario.ShowDialog(this) == DialogResult.OK)
                Buscar(proveedor.Id);
        }

        private void btnDarBaja_Click(object sender, EventArgs e) => CambiarEstado(false);

        private void btnReactivar_Click(object sender, EventArgs e) => CambiarEstado(true);

        /// <summary>Baja lógica: si provee productos activos se avisa; los productos conservan su proveedor.</summary>
        private void CambiarEstado(bool activo)
        {
            Proveedor_60MN? proveedor = Seleccionado;
            if (proveedor == null) return;
            string pregunta = activo
                ? $"¿Reactivar al proveedor {proveedor.RazonSocial}?"
                : proveedor.ProductosActivos > 0
                    ? $"El proveedor {proveedor.RazonSocial} provee {proveedor.ProductosActivos} producto(s) activo(s). " +
                      "Los productos conservan su proveedor, pero no podrá elegirse en productos nuevos. ¿Dar de baja de todos modos?"
                    : $"¿Dar de baja al proveedor {proveedor.RazonSocial}?";
            if (!Mensajes_60MN.Confirmar(pregunta)) return;
            try
            {
                proveedorBLL.CambiarEstado(proveedor.Id, activo);
                // El proveedor dado de baja sigue a la vista para poder reactivarlo.
                if (!activo && !chkIncluirBajas.Checked)
                    chkIncluirBajas.Checked = true;
                else
                    Buscar(proveedor.Id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
