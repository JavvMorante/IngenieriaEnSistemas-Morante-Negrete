using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>CUN01 Registrar Cliente (con sus mascotas, «include» CUN04) y CUN02 Modificar Cliente.</summary>
    public partial class FrmCliente_60MN : FormBase_60MN
    {
        private readonly ClienteBLL_60MN clienteBLL = new ClienteBLL_60MN();
        private readonly Cliente_60MN? original;
        private readonly string documentoSugerido;
        private readonly List<Mascota_60MN> mascotasNuevas = new List<Mascota_60MN>();

        /// <summary>Cliente registrado o modificado; en el flujo 5.1, el cliente que ya existía con ese documento.</summary>
        public int ClienteId { get; private set; }

        public FrmCliente_60MN() : this(null, string.Empty) { }

        public FrmCliente_60MN(Cliente_60MN? cliente, string documentoSugerido)
        {
            InitializeComponent();
            original = cliente;
            this.documentoSugerido = documentoSugerido;

            // Filtro de teclado: evita tipear caracteres que la BLL rechazaría. Lo pegado
            // con Ctrl+V no pasa por acá, por eso cada campo se valida igual al salir de él.
            txtNroDocumento.KeyPress += (s, e) => Filtrar(e, c => char.IsDigit(c) ||
                (ReglaActual.AdmiteLetras && c >= 'A' && c <= 'Z') || (ReglaActual.AdmiteGuiones && c == '-'));
            txtNroDocumento.CharacterCasing = CharacterCasing.Upper;
            ayudaDocumento = new ToolTip(components);
            cboTipoDocumento.SelectedIndexChanged += (s, e) => AplicarReglaDocumento();
            txtTelefono.KeyPress += (s, e) => Filtrar(e, char.IsDigit);
            txtNombre.KeyPress += (s, e) => Filtrar(e, EsDeNombre);
            txtApellido.KeyPress += (s, e) => Filtrar(e, EsDeNombre);
            txtDireccion.KeyPress += (s, e) => Filtrar(e, c => char.IsLetterOrDigit(c) || c == ' ' || ".,-/°º#'".IndexOf(c) >= 0);
            txtEmail.KeyPress += (s, e) => Filtrar(e, c => !char.IsWhiteSpace(c));
            foreach (Control campo in Campos.Values)
                campo.Leave += (s, e) => MarcarCampo((Control)s);
        }

        private bool EsAlta => original == null;

        private readonly ToolTip ayudaDocumento;

        private ReglaDocumento_60MN ReglaActual => ClienteBLL_60MN.ReglaDocumento(cboTipoDocumento.SelectedItem as string ?? "DNI");

        /// <summary>Cada tipo de documento tiene su formato (DNI, CUIL, pasaporte...): ajusta el largo, la ayuda y revalida el número.</summary>
        private void AplicarReglaDocumento()
        {
            ReglaDocumento_60MN regla = ReglaActual;
            txtNroDocumento.MaxLength = regla.LargoTipeo;
            string ayuda = $"{regla.Tipo}: {regla.Ayuda}";
            ayudaDocumento.SetToolTip(txtNroDocumento, ayuda);
            ayudaDocumento.SetToolTip(cboTipoDocumento, ayuda);
            if (txtNroDocumento.Text.Length > 0 && !txtNroDocumento.ReadOnly)
                MarcarCampo(txtNroDocumento);
        }

        /// <summary>Control de la pantalla para cada dato del cliente (nombre de la propiedad, como en los errores de la BLL).</summary>
        private Dictionary<string, Control> Campos => new Dictionary<string, Control>
        {
            [nameof(Cliente_60MN.TipoDocumento)] = cboTipoDocumento,
            [nameof(Cliente_60MN.NroDocumento)] = txtNroDocumento,
            [nameof(Cliente_60MN.Nombre)] = txtNombre,
            [nameof(Cliente_60MN.Apellido)] = txtApellido,
            [nameof(Cliente_60MN.Direccion)] = txtDireccion,
            [nameof(Cliente_60MN.Telefono)] = txtTelefono,
            [nameof(Cliente_60MN.Email)] = txtEmail
        };

        private static bool EsDeNombre(char c) => char.IsLetter(c) || c == ' ' || c == '\'' || c == '-' || c == '.';

        private static void Filtrar(KeyPressEventArgs e, Func<char, bool> permitido)
        {
            if (!char.IsControl(e.KeyChar) && !permitido(e.KeyChar))
            {
                e.Handled = true;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private void FrmCliente_60MN_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Items.AddRange(ClienteBLL_60MN.TiposDocumento);
            cboTipoDocumento.SelectedIndex = 0;
            txtNombre.MaxLength = ClienteBLL_60MN.LargoNombre;
            txtApellido.MaxLength = ClienteBLL_60MN.LargoApellido;
            txtDireccion.MaxLength = ClienteBLL_60MN.LargoDireccion;
            txtTelefono.MaxLength = ClienteBLL_60MN.LargoTelefono;
            txtEmail.MaxLength = ClienteBLL_60MN.LargoEmail;

            if (EsAlta)
            {
                txtNroDocumento.Text = documentoSugerido.All(char.IsDigit) && documentoSugerido.Length <= ReglaActual.LargoTipeo
                    ? documentoSugerido : string.Empty;
                return;
            }

            cboTipoDocumento.SelectedItem = original!.TipoDocumento;
            txtNroDocumento.Text = original.NroDocumento;
            txtNombre.Text = original.Nombre;
            txtApellido.Text = original.Apellido;
            txtDireccion.Text = original.Direccion;
            txtTelefono.Text = original.Telefono;
            txtEmail.Text = original.Email ?? string.Empty;
            // CUN02 5.1: el documento es la clave identificatoria y no se modifica.
            cboTipoDocumento.Enabled = false;
            txtNroDocumento.ReadOnly = true;
            // Las mascotas de un cliente existente se gestionan desde su ficha (CUN04).
            grpMascotas.Visible = false;
            lblNota.Visible = false;
            btnAceptar.Top = btnCancelar.Top = grpMascotas.Top;
            ClientSize = new Size(ClientSize.Width, btnAceptar.Bottom + 20);
        }

        protected override void AlCambiarIdioma()
        {
            Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmCliente_60MN.TituloAlta", "Registrar cliente")
                : TraductorFormularios_60MN.T("FrmCliente_60MN.TituloModificacion", "Modificar cliente");
        }

        // CUN01 paso 6: una o varias mascotas a nombre del cliente.
        private void btnAgregarMascota_Click(object sender, EventArgs e)
        {
            using FrmMascotas_60MN formulario = new FrmMascotas_60MN(0, null, persistir: false);
            if (formulario.ShowDialog(this) != DialogResult.OK || formulario.Resultado == null) return;
            Mascota_60MN mascota = formulario.Resultado;
            // CUN04 5.1: nombre repetido para el mismo cliente.
            if (mascotasNuevas.Any(m => string.Equals(m.Nombre, mascota.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                Mensajes_60MN.Advertir($"Ya cargó una mascota llamada {mascota.Nombre}.");
                return;
            }
            mascotasNuevas.Add(mascota);
            lstMascotas.Items.Add($"{mascota.Nombre} - {mascota.Especie}, {mascota.Raza} ({mascota.SexoTexto}, {mascota.FechaNacimiento:dd/MM/yyyy})");
        }

        private void btnQuitarMascota_Click(object sender, EventArgs e)
        {
            int indice = lstMascotas.SelectedIndex;
            if (indice < 0) return;
            mascotasNuevas.RemoveAt(indice);
            lstMascotas.Items.RemoveAt(indice);
        }

        private Cliente_60MN DatosIngresados() => new Cliente_60MN
        {
            Id = original?.Id ?? 0,
            TipoDocumento = cboTipoDocumento.SelectedItem as string ?? "DNI",
            NroDocumento = txtNroDocumento.Text,
            Nombre = txtNombre.Text,
            Apellido = txtApellido.Text,
            Direccion = txtDireccion.Text,
            Telefono = txtTelefono.Text,
            Email = txtEmail.Text
        };

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Cliente_60MN cliente = DatosIngresados();
            if (!MostrarErrores(clienteBLL.ValidarDatos(cliente, EsAlta))) return;

            try
            {
                if (EsAlta)
                {
                    clienteBLL.RegistrarCliente(cliente, mascotasNuevas);
                    Mensajes_60MN.Informar("Cliente registrado correctamente.");
                }
                else
                {
                    clienteBLL.ModificarCliente(cliente);
                    Mensajes_60MN.Informar("Cliente modificado correctamente.");
                }
                ClienteId = cliente.Id;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ClienteDuplicadoException_60MN ex)
            {
                // CUN01 5.1: el cliente ya existe; se deriva a CUN03 mostrando su ficha.
                if (Mensajes_60MN.Confirmar(ex.Message + "\n\n¿Desea ver la ficha de ese cliente?"))
                {
                    ClienteId = ex.Existente.Id;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (DatosClienteInvalidosException_60MN ex)
            {
                MostrarErrores(ex.Errores);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>
        /// CUN01 5.2 / CUN02 5.2: pone en cada campo el error que informó la BLL (qué está
        /// mal y por qué) y lleva el foco al primero. Devuelve true si no hay errores.
        /// </summary>
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
            string clave = Campos.First(c => c.Value == campo).Key;
            string? motivo = null;
            if (campo.Text.Trim().Length > 0)
                clienteBLL.ValidarDatos(DatosIngresados(), EsAlta).TryGetValue(clave, out motivo);
            errorProvider.SetError(campo, motivo ?? string.Empty);
        }
    }
}
