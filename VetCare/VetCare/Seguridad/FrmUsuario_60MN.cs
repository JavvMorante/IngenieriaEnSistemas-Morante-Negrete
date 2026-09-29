using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Composite;

namespace VetCare
{
    /// <summary>Formulario de alta (CUS01) y modificación (CUS02) de un usuario.</summary>
    public partial class FrmUsuario_60MN : FormBase_60MN
    {
        private readonly UsuarioBLL_60MN usuarioBLL = new UsuarioBLL_60MN();
        private readonly Usuario_60MN? original;

        /// <summary>Id del usuario creado o modificado.</summary>
        public int UsuarioId { get; private set; }

        /// <summary>Alta de usuario.</summary>
        public FrmUsuario_60MN() : this(null) { }

        /// <summary>Modificación del usuario indicado (o alta si es null).</summary>
        public FrmUsuario_60MN(Usuario_60MN? usuario)
        {
            InitializeComponent();
            original = usuario;
        }

        private bool EsAlta => original == null;

        private void FrmUsuario_60MN_Load(object sender, EventArgs e)
        {
            try
            {
                cboFamilia.DisplayMember = nameof(Familia_60MN.Nombre);
                cboFamilia.ValueMember = nameof(Familia_60MN.Id);
                cboFamilia.DataSource = new PermisoBLL_60MN().ListarFamilias();
                cboFamilia.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }

            if (EsAlta)
                return;

            txtApellido.Text = original!.Apellido;
            txtNombre.Text = original.Nombre;
            txtDni.Text = original.Dni;
            txtEmail.Text = original.Email;
            txtUsuario.Text = original.NombreUsuario;
            txtDni.ReadOnly = true;
            txtUsuario.ReadOnly = true;

            // La familia se elige en el alta; después, familias y patentes individuales
            // se administran con el botón "Permisos" de Gestión de usuarios.
            lblFamilia.Visible = cboFamilia.Visible = false;
        }

        /// <summary>Título y nota según el modo (alta/modificación), traducidos al idioma actual.</summary>
        protected override void AlCambiarIdioma()
        {
            if (EsAlta)
            {
                Text = TraductorFormularios_60MN.T("FrmUsuario_60MN.TituloAlta", "Añadir usuario");
                return;
            }
            Text = TraductorFormularios_60MN.T("FrmUsuario_60MN.TituloModificacion", "Modificar usuario");
            lblNota.Text = TraductorFormularios_60MN.T("FrmUsuario_60MN.NotaModificacion",
                "El DNI y el nombre de usuario no pueden modificarse. Las familias y patentes individuales se asignan con el botón Permisos.");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            Usuario_60MN usuario = new Usuario_60MN
            {
                Id = original?.Id ?? 0,
                Apellido = txtApellido.Text,
                Nombre = txtNombre.Text,
                Dni = txtDni.Text,
                Email = txtEmail.Text,
                NombreUsuario = txtUsuario.Text
            };

            try
            {
                if (EsAlta)
                {
                    string claveInicial = usuarioBLL.CrearUsuario(usuario, cboFamilia.SelectedValue is int familia ? familia : 0);
                    UsuarioId = usuario.Id;
                    Mensajes_60MN.Informar("Usuario añadido correctamente.");
                    MostrarClaveInicial(this, usuario.NombreUsuario, claveInicial);
                }
                else
                {
                    usuarioBLL.ModificarUsuario(usuario);
                    UsuarioId = usuario.Id;
                    Mensajes_60MN.Informar("Usuario modificado correctamente.");
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Marca en rojo los campos obligatorios vacíos y valida el formato del mail (flujos 5.2 y 5.3).</summary>
        private bool ValidarCampos()
        {
            errorProvider.Clear();
            bool valido = true;
            foreach (TextBox campo in new[] { txtApellido, txtNombre, txtDni, txtEmail, txtUsuario })
            {
                campo.BackColor = SystemColors.Window;
                if (!campo.ReadOnly && string.IsNullOrWhiteSpace(campo.Text))
                {
                    campo.BackColor = Color.MistyRose;
                    errorProvider.SetError(campo, "Campo obligatorio.");
                    valido = false;
                }
            }
            if (EsAlta && cboFamilia.SelectedIndex < 0)
            {
                errorProvider.SetError(cboFamilia, "Seleccione la familia (tipo de usuario).");
                valido = false;
            }
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !UsuarioBLL_60MN.EsEmailValido(txtEmail.Text.Trim()))
            {
                txtEmail.BackColor = Color.MistyRose;
                errorProvider.SetError(txtEmail, "El formato del mail es inválido.");
                valido = false;
            }
            return valido;
        }

        /// <summary>
        /// Muestra una única vez la contraseña inicial generada: no hay envío
        /// de mails (G02) y el sistema solo guarda su hash.
        /// </summary>
        public static void MostrarClaveInicial(IWin32Window propietario, string usuario, string clave)
        {
            MessageBox.Show(propietario,
                $"Contraseña inicial del usuario \"{usuario}\":\n\n{clave}\n\n" +
                "Comuníquela personalmente. No volverá a mostrarse y deberá cambiarse en el primer ingreso.\n" +
                "(Ctrl+C copia este mensaje)",
                "VetCare - Contraseña inicial", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
