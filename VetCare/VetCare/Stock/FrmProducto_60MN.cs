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

            // Filtro de teclado y largos según las reglas de la BLL; lo pegado con Ctrl+V se valida al salir del campo.
            txtNombre.MaxLength = ProductoBLL_60MN.LargoNombre;
            txtLote.MaxLength = ProductoBLL_60MN.LargoLote;
            txtLote.CharacterCasing = CharacterCasing.Upper;
            txtNombre.KeyPress += (s, e) => Filtrar(e, c => char.IsLetterOrDigit(c) || c == ' ' || ProductoBLL_60MN.SignosNombre.IndexOf(c) >= 0);
            txtLote.KeyPress += (s, e) => Filtrar(e, c => (c >= 'A' && c <= 'Z') || char.IsDigit(c) || c == '-' || c == '/');
            foreach (Control campo in Campos.Values)
                campo.Leave += (s, e) => MarcarCampo((Control)s);
        }

        private bool EsAlta => original == null;

        /// <summary>Control de la pantalla para cada dato del producto (nombre de la propiedad, como en los errores de la BLL).</summary>
        private Dictionary<string, Control> Campos => new Dictionary<string, Control>
        {
            [nameof(Producto_60MN.Nombre)] = txtNombre,
            [nameof(Producto_60MN.CategoriaId)] = cboCategoria,
            [nameof(Producto_60MN.ProveedorId)] = cboProveedor,
            [nameof(Producto_60MN.StockActual)] = nudStockActual,
            [nameof(Producto_60MN.StockMinimo)] = nudStockMinimo,
            [nameof(Producto_60MN.PrecioCosto)] = nudPrecioCosto,
            [nameof(Producto_60MN.PrecioVenta)] = nudPrecioVenta,
            [nameof(Producto_60MN.FechaVencimiento)] = dtpVencimiento,
            [nameof(Producto_60MN.Lote)] = txtLote
        };

        private static void Filtrar(KeyPressEventArgs e, Func<char, bool> permitido)
        {
            if (!char.IsControl(e.KeyChar) && !permitido(e.KeyChar))
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }

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
                // CUN11: solo proveedores activos; al modificar se conserva el actual aunque esté dado de baja.
                cboProveedor.DataSource = productoBLL.ListarProveedores(original?.ProveedorId);
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
                // CUN13: el stock mínimo sugerido de la configuración es el valor inicial.
                try { nudStockMinimo.Value = Math.Min(productoBLL.ObtenerStockMinimoSugerido(), (int)nudStockMinimo.Maximum); }
                catch (Exception ex) { Mensajes_60MN.MostrarError(ex); }
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

        private Producto_60MN DatosIngresados() => new Producto_60MN
        {
            Id = original?.Id ?? 0,
            Nombre = txtNombre.Text,
            CategoriaId = cboCategoria.SelectedValue is int categoria ? categoria : 0,
            ProveedorId = cboProveedor.SelectedValue is int proveedor ? proveedor : 0,
            StockActual = (int)nudStockActual.Value,
            StockMinimo = (int)nudStockMinimo.Value,
            PrecioCosto = nudPrecioCosto.Value,
            PrecioVenta = nudPrecioVenta.Value,
            FechaVencimiento = dtpVencimiento.Value.Date,
            Lote = txtLote.Text
        };

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Producto_60MN producto = DatosIngresados();
            if (!MostrarErrores(productoBLL.ValidarDatos(producto, EsAlta))) return;

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
            catch (DatosProductoInvalidosException_60MN ex)
            {
                MostrarErrores(ex.Errores);
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

        /// <summary>Pone en cada campo el error que informó la BLL y lleva el foco al primero. Devuelve true si no hay errores.</summary>
        private bool MostrarErrores(IReadOnlyDictionary<string, string> errores)
        {
            errorProvider.Clear();
            Control? primero = null;
            foreach (KeyValuePair<string, Control> campo in Campos)
            {
                if (!errores.TryGetValue(campo.Key, out string? motivo)) continue;
                errorProvider.SetError(campo.Value, motivo);
                primero ??= campo.Value;
            }
            primero?.Focus();
            return errores.Count == 0;
        }

        /// <summary>Al salir de nombre o lote se valida ese campo; los vacíos se avisan recién al aceptar.</summary>
        private void MarcarCampo(Control campo)
        {
            if (campo is not TextBox texto) return;
            string clave = Campos.First(c => c.Value == campo).Key;
            string? motivo = null;
            if (texto.Text.Trim().Length > 0)
                productoBLL.ValidarDatos(DatosIngresados(), EsAlta).TryGetValue(clave, out motivo);
            errorProvider.SetError(campo, motivo ?? string.Empty);
        }
    }
}
