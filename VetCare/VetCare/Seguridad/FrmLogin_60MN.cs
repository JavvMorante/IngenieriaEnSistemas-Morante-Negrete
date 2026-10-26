using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>CUS05 Iniciar Sesión.</summary>
    public partial class FrmLogin_60MN : FormBase_60MN
    {
        private readonly UsuarioBLL_60MN usuarioBLL = new UsuarioBLL_60MN();
        private readonly IdiomaBLL_60MN idiomaBLL = new IdiomaBLL_60MN();
        private bool cargandoIdiomas;

        // True si el usuario eligió un idioma en el combo antes de ingresar: esa elección se guarda como su idioma.
        private bool idiomaElegidoEnLogin;

        public FrmLogin_60MN()
        {
            InitializeComponent();
        }

        /// <summary>Carga los idiomas y selecciona el actual (el Login abre siempre en el idioma base, español).</summary>
        private void FrmLogin_60MN_Load(object sender, EventArgs e)
        {
            try
            {
                cargandoIdiomas = true;
                List<Idioma_60MN> idiomas = idiomaBLL.Listar();
                cboIdioma.DataSource = idiomas;
                int actual = Servicios_60MN.Idioma.GestorIdioma_60MN.Instancia.IdiomaActual?.Id ?? 0;
                cboIdioma.SelectedIndex = Math.Max(0, idiomas.FindIndex(i => i.Id == actual));
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
            finally
            {
                cargandoIdiomas = false;
            }
        }

        /// <summary>Cambio de idioma: se aplica en el momento a esta pantalla (Observer).</summary>
        private void cboIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoIdiomas || cboIdioma.SelectedItem is not Idioma_60MN idioma) return;
            idiomaElegidoEnLogin = true;
            try
            {
                idiomaBLL.Aplicar(idioma);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>
        /// Al ingresar: si eligió un idioma en el combo, se guarda como SU idioma; si no, se
        /// le aplica su último idioma guardado. Todas las pantallas se traducen solas (Observer).
        /// </summary>
        private void AplicarIdiomaDelUsuario()
        {
            try
            {
                idiomaBLL.AplicarIdiomaAlIngresar(idiomaElegidoEnLogin ? cboIdioma.SelectedItem as Idioma_60MN : null);
            }
            catch (Exception)
            {
                // No impedir el ingreso por no poder aplicar o guardar la preferencia de idioma.
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            lblMensaje.Text = string.Empty;

            // 1.1: no se permite el acceso si ya hay una sesión iniciada.
            if (SessionManager_60MN.Instancia.EstaLogueado)
            {
                lblMensaje.Text = TraductorFormularios_60MN.T("msg.SesionYaIniciada", "Ya existe una sesión iniciada.");
                return;
            }

            // 3.1: campos vacíos.
            bool faltanDatos = false;
            if (string.IsNullOrWhiteSpace(txtUsuario.Text)) { errorProvider.SetError(txtUsuario, "Ingrese el usuario."); faltanDatos = true; }
            if (string.IsNullOrEmpty(txtClave.Text)) { errorProvider.SetError(txtClave, "Ingrese la contraseña."); faltanDatos = true; }
            if (faltanDatos) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                Usuario_60MN usuario = usuarioBLL.Login(txtUsuario.Text, txtClave.Text);

                // «extend» CUS07: en el primer ingreso se debe cambiar la contraseña inicial.
                if (usuario.PrimerIngreso)
                {
                    Mensajes_60MN.Informar("Es su primer ingreso: debe cambiar la contraseña inicial antes de continuar.");
                    using FrmCambiarClave_60MN cambio = new FrmCambiarClave_60MN(obligatorio: true);
                    if (cambio.ShowDialog(this) != DialogResult.OK)
                    {
                        usuarioBLL.Logout();
                        lblMensaje.Text = TraductorFormularios_60MN.T("msg.DebeCambiarClave", "Debe cambiar la contraseña para poder ingresar.");
                        txtClave.Clear();
                        return;
                    }
                }

                if (SessionManager_60MN.Instancia.EsEmergencia)
                    Mensajes_60MN.Advertir("Ingresó con el USUARIO DE EMERGENCIA porque no hay un administrador activo en la base.\n\n" +
                                           "Cree o reactive un administrador desde ADMIN > Usuarios.");

                if (SessionManager_60MN.Instancia.IntegridadComprometida)
                    Mensajes_60MN.Advertir("Se detectaron inconsistencias en los dígitos verificadores de la base de datos.\n\n" +
                                           "Revíselas desde ADMIN > Dígitos verificadores.");

                AplicarIdiomaDelUsuario();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (LoginException_60MN ex)
            {
                lblMensaje.Text = ex.Message;
                txtClave.Clear();
                txtClave.Focus();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
