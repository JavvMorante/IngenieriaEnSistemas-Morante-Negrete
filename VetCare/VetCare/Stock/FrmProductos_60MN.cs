using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN09 Administrar Productos: búsqueda, alta, modificación, baja y reactivación de productos,
    /// y el historial de versiones (CUS10). El stock cambia con ventas (CUN05) y recepciones (CUN14).
    /// </summary>
    public partial class FrmProductos_60MN : FormBase_60MN
    {
        private readonly ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();

        // Evita buscar mientras se cargan los combos.
        private bool cargando;

        public FrmProductos_60MN()
        {
            InitializeComponent();
            dgvProductos.AutoGenerateColumns = false;
            TextoGuia_60MN.Asignar(txtBuscar, "Código, nombre o lote");
        }

        private void FrmProductos_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            cargando = true;
            try
            {
                cboCategoria.Items.Add(OpcionCombo_60MN.Todos);
                foreach (Categoria_60MN categoria in productoBLL.ListarCategorias())
                    cboCategoria.Items.Add(new OpcionCombo_60MN(categoria.Nombre, categoria.Id));
                cboCategoria.SelectedIndex = 0;

                // Si hay productos en alerta de reposición, la pantalla abre mostrando solo esos.
                CargarFiltroStock(productoBLL.ContarAlertasActivas() > 0 ? 1 : 0);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            Buscar();
        }

        /// <summary>Opciones del filtro de stock: todos los productos o solo los que están en alerta de reposición.</summary>
        private void CargarFiltroStock(int seleccionado)
        {
            cboFiltroStock.Items.Clear();
            cboFiltroStock.Items.Add(new OpcionCombo_60MN(TraductorFormularios_60MN.T("msg.Todos", "(Todos)"), false));
            cboFiltroStock.Items.Add(new OpcionCombo_60MN(TraductorFormularios_60MN.T("msg.EnAlertaStock", "En alerta de stock"), true));
            cboFiltroStock.SelectedIndex = seleccionado;
        }

        protected override void AlCambiarIdioma()
        {
            if (cboFiltroStock.Items.Count == 0) return;
            bool previo = cargando;
            cargando = true;
            CargarFiltroStock(Math.Max(cboFiltroStock.SelectedIndex, 0));
            cargando = previo;
        }

        private bool SoloEnAlerta => OpcionCombo_60MN.ValorDe<bool>(cboFiltroStock) ?? false;

        private void cboFiltroStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando) Buscar();
        }

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        public void Buscar(int? seleccionarId = null)
        {
            try
            {
                List<Producto_60MN> productos = productoBLL.Buscar(txtBuscar.Text, OpcionCombo_60MN.ValorDe<int>(cboCategoria), chkInactivos.Checked);
                // En alerta de reposición: activos con el stock en o debajo del mínimo.
                if (SoloEnAlerta)
                    productos = productos.Where(p => p.Activo && p.BajoStockMinimo).ToList();
                dgvProductos.DataSource = productos;
                if (seleccionarId.HasValue)
                    foreach (DataGridViewRow fila in dgvProductos.Rows)
                        if (fila.DataBoundItem is Producto_60MN p && p.Id == seleccionarId.Value)
                            dgvProductos.CurrentCell = fila.Cells[0];

                int alertas = productoBLL.ContarAlertasActivas();
                lblAlertas.Text = alertas > 0 ? $"⚠ {alertas} producto(s) con alerta de reposición (stock en o debajo del mínimo)." : string.Empty;
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

        // CUN09 alta
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using FrmProducto_60MN formulario = new FrmProducto_60MN();
            if (formulario.ShowDialog(this) == DialogResult.OK)
                Buscar(formulario.ProductoId);
        }

        // CUN09 modificación
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


        /// <summary>
        /// CUS10 desde el maestro: historial de precios y datos del producto (Productos_C), con
        /// comparar versiones y volver a una versión anterior.
        /// </summary>
        private void btnHistorial_Click(object sender, EventArgs e)
        {
            Producto_60MN? producto = Seleccionado();
            if (producto == null) return;
            using FrmBitacoraCambios_60MN formulario = new FrmBitacoraCambios_60MN(producto.Codigo);
            formulario.ShowDialog(this);
            Buscar(producto.Id);
        }
    }
}
