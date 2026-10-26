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
        }

        private bool EsAlta => original == null;

        private void FrmCliente_60MN_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Items.AddRange(ClienteBLL_60MN.TiposDocumento);
            cboTipoDocumento.SelectedIndex = 0;

            if (EsAlta)
            {
                txtNroDocumento.Text = documentoSugerido.All(char.IsDigit) ? documentoSugerido : string.Empty;
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;
            Cliente_60MN cliente = new Cliente_60MN
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
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>CUN01 5.2 / CUN02 5.2: marca en rojo los campos obligatorios vacíos.</summary>
        private bool ValidarCampos()
        {
            errorProvider.Clear();
            bool valido = true;
            foreach (TextBox campo in new[] { txtNroDocumento, txtNombre, txtApellido, txtDireccion, txtTelefono })
            {
                campo.BackColor = SystemColors.Window;
                if (!campo.ReadOnly && string.IsNullOrWhiteSpace(campo.Text))
                {
                    campo.BackColor = Color.MistyRose;
                    errorProvider.SetError(campo, "Campo obligatorio.");
                    valido = false;
                }
            }
            return valido;
        }
    }
}
