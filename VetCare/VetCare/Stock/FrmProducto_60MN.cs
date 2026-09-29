using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Composite;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>CUN11 Registrar Producto y CUN12 Modificar Producto.</summary>
    public partial class FrmProducto_60MN : FormBase_60MN
    {
        private readonly ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();
        private readonly Producto_60MN? original;

        // Si el actor editó el precio de venta a mano, ya no se reemplaza por el sugerido (CUN11 paso 6).
        private bool ventaEditadaManualmente;
        private bool actualizandoVenta;
        private decimal precioSugerido;

        public int ProductoId { get; private set; }

        public FrmProducto_60MN() : this(null) { }

        public FrmProducto_60MN(Producto_60MN? producto)
        {
            InitializeComponent();
            original = producto;
        }

        private bool EsAlta => original == null;

        private void FrmProducto_60MN_Load(object sender, EventArgs e)
        {
            try
            {
                cboCategoria.DisplayMember = nameof(Categoria_60MN.Nombre);
                cboCategoria.ValueMember = nameof(Categoria_60MN.Id);
                cboCategoria.DataSource = productoBLL.ListarCategorias();
                cboCategoria.SelectedIndex = -1;
                cboProveedor.DisplayMember = nameof(Proveedor_60MN.RazonSocial);
                cboProveedor.ValueMember = nameof(Proveedor_60MN.Id);
                cboProveedor.DataSource = productoBLL.ListarProveedores();
                cboProveedor.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }

            if (EsAlta)
            {
                dtpVencimiento.MinDate = DateTime.Today;
                dtpVencimiento.Value = DateTime.Today.AddYears(1);
                return;
            }

            txtCodigo.Text = original!.Codigo;
            txtNombre.Text = original.Nombre;
            cboCategoria.SelectedValue = original.CategoriaId;
            cboProveedor.SelectedValue = original.ProveedorId;
            nudStockActual.Value = original.StockActual;
            nudStockMinimo.Value = original.StockMinimo;
            nudPrecioCosto.Value = original.PrecioCosto;
            dtpVencimiento.Value = original.FechaVencimiento;
            txtLote.Text = original.Lote;
            ventaEditadaManualmente = true;
            EstablecerVenta(original.PrecioVenta);

            // 5.1: el stock actual no se edita directamente, se ajusta con CUN14.
            nudStockActual.Enabled = false;
            ActualizarSugerido();
        }

        /// <summary>Título y nota según el modo (alta/modificación), traducidos al idioma actual.</summary>
        protected override void AlCambiarIdioma()
        {
            Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmProducto_60MN.TituloAlta", "Registrar producto")
                : TraductorFormularios_60MN.T("FrmProducto_60MN.TituloModificacion", "Modificar producto");
            lblNota.Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmProducto_60MN.NotaAlta", "El precio de venta se propone aplicando el margen de ganancia de la categoría o el general.")
                : TraductorFormularios_60MN.T("FrmProducto_60MN.NotaModificacion", "El stock actual no puede modificarse desde aquí: registre un movimiento de stock para que el ajuste quede justificado y trazado.");
        }

        private void PrecioBase_Changed(object? sender, EventArgs e) => ActualizarSugerido();

        /// <summary>CUN11 paso 5: precio de venta propuesto con el margen preconfigurado.</summary>
        private void ActualizarSugerido()
        {
            if (cboCategoria.SelectedValue is not int categoriaId)
            {
                lblMargen.Text = string.Empty;
                return;
            }
            try
            {
                decimal margen = productoBLL.ObtenerMargen(categoriaId);
                precioSugerido = ProductoBLL_60MN.CalcularPrecioVenta(nudPrecioCosto.Value, margen);
                lblMargen.Text = $"Margen {margen:0.##}%: precio sugerido $ {precioSugerido:N2}";
                if (!ventaEditadaManualmente)
                    EstablecerVenta(precioSugerido);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void EstablecerVenta(decimal valor)
        {
            actualizandoVenta = true;
            nudPrecioVenta.Value = Math.Min(Math.Max(valor, nudPrecioVenta.Minimum), nudPrecioVenta.Maximum);
            actualizandoVenta = false;
        }

        private void nudPrecioVenta_ValueChanged(object sender, EventArgs e)
        {
            if (!actualizandoVenta)
                ventaEditadaManualmente = true;
        }

        private void btnUsarSugerido_Click(object sender, EventArgs e)
        {
            ventaEditadaManualmente = false;
            ActualizarSugerido();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            Producto_60MN producto = new Producto_60MN
            {
                Id = original?.Id ?? 0,
                Nombre = txtNombre.Text,
                CategoriaId = (int)cboCategoria.SelectedValue!,
                ProveedorId = (int)cboProveedor.SelectedValue!,
                StockActual = (int)nudStockActual.Value,
                StockMinimo = (int)nudStockMinimo.Value,
                PrecioCosto = nudPrecioCosto.Value,
                PrecioVenta = nudPrecioVenta.Value,
                FechaVencimiento = dtpVencimiento.Value.Date,
                Lote = txtLote.Text
            };

            // CUN11 6.1 / CUN12 5.2: margen negativo.
            if (producto.PrecioVenta < producto.PrecioCosto &&
                !Mensajes_60MN.Confirmar("El precio de venta es inferior al precio de costo (margen negativo). ¿Desea continuar?"))
                return;

            try
            {
                bool alerta;
                if (EsAlta)
                {
                    alerta = productoBLL.Registrar(producto);
                    Mensajes_60MN.Informar($"Producto registrado correctamente con el código {producto.Codigo}.");
                }
                else
                {
                    alerta = productoBLL.Modificar(producto);
                    Mensajes_60MN.Informar("Producto modificado correctamente.");
                }
                // CUN11 4.3: stock mínimo mayor al actual.
                if (alerta)
                    Mensajes_60MN.Advertir("El stock actual está en o por debajo del stock mínimo: se generó la alerta de reposición.");

                ProductoId = producto.Id;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ProductoDuplicadoException_60MN ex) when (EsAlta)
            {
                OfrecerSumarStock(ex.Existente, producto.StockActual);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>CUN11 4.1: mismo nombre y lote → sumar la cantidad al stock existente en lugar de duplicar.</summary>
        private void OfrecerSumarStock(Producto_60MN existente, int cantidad)
        {
            if (cantidad <= 0 || !SessionManager_60MN.Instancia.TienePermiso(CodigosPatente_60MN.MovimientoStock))
            {
                Mensajes_60MN.Advertir($"Ya existe el producto {existente.Codigo} con el mismo nombre y lote. No se registró el alta.");
                return;
            }
            if (!Mensajes_60MN.Confirmar($"Ya existe el producto {existente.Codigo} \"{existente.Nombre}\" (lote {existente.Lote}) con stock {existente.StockActual}.\n\n" +
                                         $"¿Desea sumar {cantidad} unidad(es) al stock existente en lugar de duplicar el registro?"))
                return;
            try
            {
                ResultadoMovimiento_60MN resultado = new MovimientoStockBLL_60MN().Registrar(existente.Id, TipoMovimiento_60MN.Ingreso,
                    cantidad, false, "Ingreso de mercadería sobre un lote ya registrado", false);
                Mensajes_60MN.Informar($"Se registró el ingreso. Stock actual de {existente.Codigo}: {resultado.StockResultante}.");
                ProductoId = existente.Id;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private bool ValidarCampos()
        {
            errorProvider.Clear();
            bool valido = true;
            void Marcar(Control control, string mensaje)
            {
                errorProvider.SetError(control, mensaje);
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text)) Marcar(txtNombre, "Campo obligatorio.");
            if (cboCategoria.SelectedIndex < 0) Marcar(cboCategoria, "Seleccione una categoría.");
            if (cboProveedor.SelectedIndex < 0) Marcar(cboProveedor, "Seleccione un proveedor.");
            if (nudPrecioCosto.Value <= 0) Marcar(nudPrecioCosto, "Ingrese el precio de costo.");
            if (nudPrecioVenta.Value <= 0) Marcar(nudPrecioVenta, "Ingrese el precio de venta.");
            if (string.IsNullOrWhiteSpace(txtLote.Text)) Marcar(txtLote, "Campo obligatorio.");
            // CUN11 4.2: vencimiento anterior a hoy.
            if (EsAlta && dtpVencimiento.Value.Date < DateTime.Today) Marcar(dtpVencimiento, "La fecha de vencimiento es anterior a la fecha actual.");
            return valido;
        }
    }
}
