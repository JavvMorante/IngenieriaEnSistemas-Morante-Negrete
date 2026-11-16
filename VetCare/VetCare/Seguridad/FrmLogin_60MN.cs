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

        /// <summary>El Login abre siempre en el idioma base (español) y no permite cambiarlo.</summary>
        public FrmLogin_60MN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Al ingresar se le aplica al usuario su último idioma guardado (USUARIO > Cambiar idioma).
        /// Todas las pantallas se traducen solas (Observer).
        /// </summary>
        private void AplicarIdiomaDelUsuario()
        {
            try
            {
                idiomaBLL.AplicarIdiomaAlIngresar();
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
