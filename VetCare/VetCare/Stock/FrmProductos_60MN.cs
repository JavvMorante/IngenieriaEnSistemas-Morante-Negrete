using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// Módulo de Stock: búsqueda de productos (CUN12 pasos 1-2) y acceso a
    /// CUN11 Registrar Producto, CUN12 Modificar Producto y CUN14 Registrar Movimiento.
    /// </summary>
    public partial class FrmProductos_60MN : FormBase_60MN
    {
        private readonly ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();

        public FrmProductos_60MN()
        {
            InitializeComponent();
            dgvProductos.AutoGenerateColumns = false;
        }

        private void FrmProductos_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            try
            {
                cboCategoria.Items.Add(OpcionCombo_60MN.Todos);
                foreach (Categoria_60MN categoria in productoBLL.ListarCategorias())
                    cboCategoria.Items.Add(new OpcionCombo_60MN(categoria.Nombre, categoria.Id));
                cboCategoria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            Buscar();
        }

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        public void Buscar(int? seleccionarId = null)
        {
            try
            {
                List<Producto_60MN> productos = productoBLL.Buscar(txtBuscar.Text, OpcionCombo_60MN.ValorDe<int>(cboCategoria), chkInactivos.Checked);
                dgvProductos.DataSource = productos;
                if (seleccionarId.HasValue)
                    foreach (DataGridViewRow fila in dgvProductos.Rows)
                        if (fila.DataBoundItem is Producto_60MN p && p.Id == seleccionarId.Value)
                            dgvProductos.CurrentCell = fila.Cells[0];

                int alertas = productoBLL.ContarAlertasActivas();
                lblAlertas.Text = alertas > 0 ? $"⚠ {alertas} producto(s) con alerta de reposición (stock en o por debajo del mínimo)." : string.Empty;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Destaca productos bajo el stock mínimo, vencidos o dados de baja.</summary>
        private void dgvProductos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if (fila.DataBoundItem is not Producto_60MN p) continue;
                if (!p.Activo)
                    fila.DefaultCellStyle.BackColor = Color.Gainsboro;
                else if (p.FechaVencimiento.Date < DateTime.Today)
                    fila.DefaultCellStyle.BackColor = Color.MistyRose;
                else if (p.BajoStockMinimo)
                    fila.DefaultCellStyle.BackColor = Color.LightYellow;
            }
        }

        private Producto_60MN? Seleccionado()
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is Producto_60MN producto)
                return producto;
            Mensajes_60MN.Advertir("Debe seleccionar un producto de la grilla.");
            return null;
        }

        // CUN11
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using FrmProducto_60MN formulario = new FrmProducto_60MN();
            if (formulario.ShowDialog(this) == DialogResult.OK)
                Buscar(formulario.ProductoId);
        }

        // CUN12
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Producto_60MN? producto = Seleccionado();
            if (producto == null) return;
            // 3.1: producto dado de baja.
            if (!producto.Activo)
            {
                Mensajes_60MN.Advertir("El producto se encuentra dado de baja: no se puede editar.");
                return;
            }
            using FrmProducto_60MN formulario = new FrmProducto_60MN(producto);
            if (formulario.ShowDialog(this) == DialogResult.OK)
                Buscar(producto.Id);
        }

        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnModificar.Visible)
                btnModificar.PerformClick();
        }

        private void btnDarBaja_Click(object sender, EventArgs e) => CambiarEstado(false);

        private void btnReactivar_Click(object sender, EventArgs e) => CambiarEstado(true);

        private void CambiarEstado(bool activo)
        {
            Producto_60MN? producto = Seleccionado();
            if (producto == null) return;
            if (!Mensajes_60MN.Confirmar($"¿{(activo ? "Reactivar" : "Dar de baja")} el producto {producto.Codigo} \"{producto.Nombre}\"?"))
                return;
            try
            {
                productoBLL.CambiarEstado(producto.Id, activo);
                Buscar(producto.Id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // CUN14
        private void btnMovimiento_Click(object sender, EventArgs e)
        {
            Producto_60MN? producto = Seleccionado();
            if (producto == null) return;
            using FrmMovimientoStock_60MN formulario = new FrmMovimientoStock_60MN(producto.Id);
            formulario.ShowDialog(this);
            Buscar(producto.Id);
        }
    }
}
