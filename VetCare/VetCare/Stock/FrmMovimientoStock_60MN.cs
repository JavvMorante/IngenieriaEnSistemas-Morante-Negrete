using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>CUN14 Registrar Movimiento de Stock (ingreso, egreso o ajuste).</summary>
    public partial class FrmMovimientoStock_60MN : FormBase_60MN
    {
        private readonly ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();
        private readonly MovimientoStockBLL_60MN movimientoBLL = new MovimientoStockBLL_60MN();
        private readonly int? productoInicial;

        public FrmMovimientoStock_60MN() : this(null) { }

        /// <summary>Abre el formulario con un producto ya seleccionado (desde la grilla de productos).</summary>
        public FrmMovimientoStock_60MN(int? productoId)
        {
            InitializeComponent();
            dgvMovimientos.AutoGenerateColumns = false;
            productoInicial = productoId;
        }

        private void FrmMovimientoStock_60MN_Load(object sender, EventArgs e) => CargarProductos(productoInicial);

        private void CargarProductos(int? seleccionarId)
        {
            try
            {
                List<Producto_60MN> productos = productoBLL.Buscar(null, null, incluirInactivos: false);
                cboProducto.DataSource = productos;
                int indice = seleccionarId.HasValue ? productos.FindIndex(p => p.Id == seleccionarId.Value) : -1;
                cboProducto.SelectedIndex = indice;
                if (indice < 0) MostrarProducto(null);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private Producto_60MN? ProductoSeleccionado => cboProducto.SelectedItem as Producto_60MN;

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e) => MostrarProducto(ProductoSeleccionado);

        /// <summary>Muestra stock disponible, mínimo, precio y vencimiento del producto y su historial.</summary>
        private void MostrarProducto(Producto_60MN? producto)
        {
            if (producto == null)
            {
                lblInfoProducto.Text = "Seleccione el producto y el lote.";
                dgvMovimientos.DataSource = null;
                return;
            }
            string vencido = producto.FechaVencimiento.Date < DateTime.Today ? "  (VENCIDO)" : string.Empty;
            lblInfoProducto.Text = $"Stock: {producto.StockActual}   Mínimo: {producto.StockMinimo}   " +
                                   $"Precio venta: $ {producto.PrecioVenta:N2}   Vence: {producto.FechaVencimiento:dd/MM/yyyy}{vencido}";
            lblInfoProducto.ForeColor = producto.BajoStockMinimo || vencido.Length > 0 ? Color.Firebrick : Color.DarkSlateGray;
            try
            {
                dgvMovimientos.DataSource = movimientoBLL.ListarPorProducto(producto.Id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void Tipo_CheckedChanged(object? sender, EventArgs e)
        {
            grpAjuste.Enabled = rdoAjuste.Checked;
            if (!rdoAjuste.Checked) chkVencimiento.Checked = false;
            nudCantidad.Enabled = !chkVencimiento.Checked;
        }

        private void chkVencimiento_CheckedChanged(object sender, EventArgs e)
        {
            nudCantidad.Enabled = !chkVencimiento.Checked;
            rdoSumar.Enabled = rdoRestar.Enabled = !chkVencimiento.Checked;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            Producto_60MN? producto = ProductoSeleccionado;
            bool valido = true;
            if (producto == null) { errorProvider.SetError(cboProducto, "Seleccione un producto."); valido = false; }
            if (!chkVencimiento.Checked && nudCantidad.Value <= 0) { errorProvider.SetError(nudCantidad, "La cantidad debe ser mayor a cero."); valido = false; }
            if (string.IsNullOrWhiteSpace(txtMotivo.Text)) { errorProvider.SetError(txtMotivo, "Indique el motivo."); valido = false; }
            if (!valido) return;

            TipoMovimiento_60MN tipo = rdoIngreso.Checked ? TipoMovimiento_60MN.Ingreso
                                     : rdoEgreso.Checked ? TipoMovimiento_60MN.Egreso
                                     : TipoMovimiento_60MN.Ajuste;

            // 4.2: el ajuste por vencimiento da de baja el lote completo.
            if (chkVencimiento.Checked &&
                !Mensajes_60MN.Confirmar($"Se dará de baja el lote {producto!.Lote} de \"{producto.Nombre}\" ({producto.StockActual} unidades) y quedará excluido de la venta. ¿Confirmar?"))
                return;

            try
            {
                ResultadoMovimiento_60MN resultado = movimientoBLL.Registrar(producto!.Id, tipo, (int)nudCantidad.Value,
                    rdoRestar.Checked, txtMotivo.Text, chkVencimiento.Checked);

                string mensaje = $"Movimiento registrado. Stock resultante: {resultado.StockResultante}.";
                if (resultado.LoteDadoDeBaja)
                    mensaje += "\nEl lote fue dado de baja por vencimiento.";
                Mensajes_60MN.Informar(mensaje);

                // Paso 7 / 7.1: alerta de reposición (sin duplicarla).
                if (resultado.AlertaGenerada)
                    Mensajes_60MN.Advertir($"ALERTA DE REPOSICIÓN: el stock de \"{producto.Nombre}\" quedó en {resultado.StockResultante} (mínimo {producto.StockMinimo}).");
                else if (resultado.AlertaExistente)
                    Mensajes_60MN.Advertir($"El producto sigue por debajo del stock mínimo (ya tenía una alerta de reposición activa).");

                nudCantidad.Value = 0;
                txtMotivo.Clear();
                chkVencimiento.Checked = false;
                CargarProductos(resultado.LoteDadoDeBaja ? null : producto.Id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // 5.1: cancelar no registra el movimiento ni altera el stock.
        private void btnCancelar_Click(object sender, EventArgs e) => Close();
    }
}
