using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN11 Gestionar Proveedores: alta o modificación de un proveedor. El CUIT solo se
    /// carga en el alta (después no se modifica). Las reglas están en ProveedorBLL_60MN:
    /// la pantalla filtra lo que se tipea y marca en cada campo el error que informa la BLL.
    /// </summary>
    public partial class FrmProveedor_60MN : FormBase_60MN
    {
        private readonly ProveedorBLL_60MN proveedorBLL = new ProveedorBLL_60MN();
        private readonly Proveedor_60MN? original;

        /// <summary>Id del proveedor guardado (con DialogResult.OK).</summary>
        public int ProveedorId { get; private set; }

        public FrmProveedor_60MN() : this(null) { }

        public FrmProveedor_60MN(Proveedor_60MN? proveedor)
        {
            InitializeComponent();
            original = proveedor;

            txtRazonSocial.MaxLength = ProveedorBLL_60MN.LargoRazonSocial;
            txtCuit.MaxLength = 13;
            txtTelefono.MaxLength = ProveedorBLL_60MN.LargoTelefono;
            txtDireccion.MaxLength = ProveedorBLL_60MN.LargoDireccion;
            txtEmail.MaxLength = ProveedorBLL_60MN.LargoEmail;
            txtContacto.MaxLength = ProveedorBLL_60MN.LargoContacto;

            // Filtro de teclado: evita tipear caracteres que la BLL rechazaría. Lo pegado
            // con Ctrl+V no pasa por acá, por eso cada campo se valida igual al salir de él.
            txtCuit.KeyPress += (s, e) => Filtrar(e, c => char.IsDigit(c) || c == '-');
            txtTelefono.KeyPress += (s, e) => Filtrar(e, char.IsDigit);
            txtRazonSocial.KeyPress += (s, e) => Filtrar(e, c => char.IsLetterOrDigit(c) || c == ' ' || ".,-&'()/".IndexOf(c) >= 0);
            txtDireccion.KeyPress += (s, e) => Filtrar(e, c => char.IsLetterOrDigit(c) || c == ' ' || ".,-/°º#'".IndexOf(c) >= 0);
            txtEmail.KeyPress += (s, e) => Filtrar(e, c => !char.IsWhiteSpace(c));
            txtContacto.KeyPress += (s, e) => Filtrar(e, c => char.IsLetter(c) || c == ' ' || c == '\'' || c == '-' || c == '.');
            foreach (Control campo in Campos.Values)
                campo.Leave += (s, e) => MarcarCampo((Control)s);
        }

        private bool EsAlta => original == null;

        /// <summary>Control de la pantalla para cada dato del proveedor (nombre de la propiedad, como en los errores de la BLL).</summary>
        private Dictionary<string, Control> Campos => new Dictionary<string, Control>
        {
            [nameof(Proveedor_60MN.RazonSocial)] = txtRazonSocial,
            [nameof(Proveedor_60MN.Cuit)] = txtCuit,
            [nameof(Proveedor_60MN.Telefono)] = txtTelefono,
            [nameof(Proveedor_60MN.Direccion)] = txtDireccion,
            [nameof(Proveedor_60MN.Email)] = txtEmail,
            [nameof(Proveedor_60MN.Contacto)] = txtContacto
        };

        private static void Filtrar(KeyPressEventArgs e, Func<char, bool> permitido)
        {
            if (!char.IsControl(e.KeyChar) && !permitido(e.KeyChar))
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private void FrmProveedor_60MN_Load(object sender, EventArgs e)
        {
            if (EsAlta) return;
            txtRazonSocial.Text = original!.RazonSocial;
            txtCuit.Text = original.CuitFormateado;
            txtCuit.ReadOnly = true;
            txtCuit.TabStop = false;
            txtTelefono.Text = original.Telefono;
            txtDireccion.Text = original.Direccion;
            txtEmail.Text = original.Email ?? string.Empty;
            txtContacto.Text = original.Contacto ?? string.Empty;
        }

        protected override void AlCambiarIdioma()
        {
            Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmProveedor_60MN.TituloAlta", "Registrar proveedor")
                : TraductorFormularios_60MN.T("FrmProveedor_60MN.TituloModificacion", "Modificar proveedor");
            lblNota.Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmProveedor_60MN.NotaAlta", "El CUIT se ingresa con o sin guiones y no podrá modificarse después.")
                : TraductorFormularios_60MN.T("FrmProveedor_60MN.NotaModificacion", "El CUIT no se modifica.");
        }

        private Proveedor_60MN DatosIngresados() => new Proveedor_60MN
        {
            Id = original?.Id ?? 0,
            RazonSocial = txtRazonSocial.Text,
            Cuit = original?.Cuit ?? txtCuit.Text,
            Telefono = txtTelefono.Text,
            Direccion = txtDireccion.Text,
            Email = txtEmail.Text,
            Contacto = txtContacto.Text
        };

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Proveedor_60MN proveedor = DatosIngresados();
            if (!MostrarErrores(proveedorBLL.ValidarDatos(proveedor, EsAlta))) return;
            try
            {
                if (EsAlta)
                {
                    proveedorBLL.Registrar(proveedor);
                    Mensajes_60MN.Informar($"Proveedor {proveedor.RazonSocial} registrado.");
                }
                else
                {
                    proveedorBLL.Modificar(proveedor);
                    Mensajes_60MN.Informar($"Proveedor {proveedor.RazonSocial} modificado.");
                }
                ProveedorId = proveedor.Id;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (DatosProveedorInvalidosException_60MN ex)
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

        /// <summary>Al salir de un campo se valida solo ese campo; si quedó vacío se avisa recién al aceptar.</summary>
        private void MarcarCampo(Control campo)
        {
            if (campo is TextBox { ReadOnly: true }) return;
            string clave = Campos.First(c => c.Value == campo).Key;
            string? motivo = null;
            if (campo.Text.Trim().Length > 0)
                proveedorBLL.ValidarDatos(DatosIngresados(), EsAlta).TryGetValue(clave, out motivo);
            errorProvider.SetError(campo, motivo ?? string.Empty);
        }
    }
}
