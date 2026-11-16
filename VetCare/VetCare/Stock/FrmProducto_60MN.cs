using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN09 Administrar Productos (alta y modificación). El margen es del producto: en el alta lo
    /// propone la categoría. El stock no se edita: arranca en 0 y cambia con ventas y recepciones.
    /// </summary>
    public partial class FrmProducto_60MN : FormBase_60MN
    {
        private readonly ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();
        private readonly Producto_60MN? original;

        // Si el actor editó el precio de venta a mano, ya no se reemplaza por el sugerido.
        private bool ventaEditadaManualmente;
        private bool actualizandoVenta;
        // Si el actor editó el margen, cambiar de categoría ya no lo reemplaza por el de la categoría.
        private bool margenEditadoManualmente;
        private bool actualizandoMargen;
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
            nudMargen.Maximum = ProductoBLL_60MN.MargenMaximo;
            nudMargen.ValueChanged += (s, e) => { if (!actualizandoMargen) margenEditadoManualmente = true; };
            // El stock solo cambia por ventas (CUN05) y recepciones (CUN14).
            nudStockActual.Enabled = false;
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
            [nameof(Producto_60MN.Margen)] = nudMargen,
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
                // CUN09: solo proveedores activos; al modificar se conserva el actual aunque esté dado de baja.
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
                nudStockActual.Value = 0;
                // El stock mínimo sugerido de la configuración es el valor inicial.
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
            EstablecerMargen(Math.Min(original.Margen, nudMargen.Maximum));
            margenEditadoManualmente = true;
            dtpVencimiento.Value = original.FechaVencimiento;
            txtLote.Text = original.Lote;
            ventaEditadaManualmente = true;
            EstablecerVenta(original.PrecioVenta);

            ActualizarSugerido();
        }

        /// <summary>Título y nota según el modo (alta/modificación), traducidos al idioma actual.</summary>
        protected override void AlCambiarIdioma()
        {
            Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmProducto_60MN.TituloAlta", "Registrar producto")
                : TraductorFormularios_60MN.T("FrmProducto_60MN.TituloModificacion", "Modificar producto");
            lblNota.Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmProducto_60MN.NotaAlta", "El margen se propone según la categoría y el precio de venta se calcula con él (puede editarlo). El stock arranca en 0: se suma al recibir mercadería.")
                : TraductorFormularios_60MN.T("FrmProducto_60MN.NotaModificacion", "El stock actual no se modifica desde aquí: cambia con las ventas y con la recepción de mercadería.");
        }

        /// <summary>Cambió la categoría, el costo o el margen: en el alta la categoría propone el margen.</summary>
        private void PrecioBase_Changed(object? sender, EventArgs e)
        {
            if (sender == cboCategoria && EsAlta && !margenEditadoManualmente && cboCategoria.SelectedValue is int categoriaId)
            {
                try { EstablecerMargen(Math.Min(productoBLL.ObtenerMargen(categoriaId), nudMargen.Maximum)); }
                catch (Exception ex) { Mensajes_60MN.MostrarError(ex); }
            }
            ActualizarSugerido();
        }

        /// <summary>Precio de venta sugerido: costo x (1 + margen / 100). Si no se editó a mano, se aplica.</summary>
        private void ActualizarSugerido()
        {
            precioSugerido = ProductoBLL_60MN.CalcularPrecioVenta(nudPrecioCosto.Value, nudMargen.Value);
            lblMargen.Text = string.Format(TraductorFormularios_60MN.T("msg.PrecioSugerido", "Precio sugerido con {0:0.##} % de margen: $ {1:N2}"),
                nudMargen.Value, precioSugerido);
            if (!ventaEditadaManualmente)
                EstablecerVenta(precioSugerido);
        }

        private void EstablecerMargen(decimal valor)
        {
            actualizandoMargen = true;
            nudMargen.Value = Math.Max(valor, nudMargen.Minimum);
            actualizandoMargen = false;
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
            Margen = nudMargen.Value,
            FechaVencimiento = dtpVencimiento.Value.Date,
            Lote = txtLote.Text
        };

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Producto_60MN producto = DatosIngresados();
            if (!MostrarErrores(productoBLL.ValidarDatos(producto, EsAlta))) return;

            Guardar(producto, confirmado: false);
        }

        /// <summary>Guarda; si la BLL pide confirmación (precio de venta menor al costo), pregunta y repite.</summary>
        private void Guardar(Producto_60MN producto, bool confirmado)
        {
            try
            {
                bool alerta;
                if (EsAlta)
                {
                    alerta = productoBLL.Registrar(producto, confirmado);
                    Mensajes_60MN.Informar($"Producto registrado correctamente con el código {producto.Codigo}.");
                }
                else
                {
                    alerta = productoBLL.Modificar(producto, confirmado);
                    Mensajes_60MN.Informar("Producto modificado correctamente.");
                }
                // Stock en o por debajo del mínimo (en el alta, siempre que el mínimo sea mayor a 0).
                if (alerta)
                    Mensajes_60MN.Advertir("El stock actual está en o por debajo del stock mínimo: se generó la alerta de reposición.");

                ProductoId = producto.Id;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ConfirmacionRequeridaException_60MN ex)
            {
                if (Mensajes_60MN.Confirmar(ex.Message))
                    Guardar(producto, confirmado: true);
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
