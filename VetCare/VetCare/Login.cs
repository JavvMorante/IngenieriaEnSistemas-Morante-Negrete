using BLL_60MN;
using BLL_60MN.Seguridad_MN60;
using DAL_60MN;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VetCare
{
    public partial class Login : Form
    {

        BLL_60MN.Seguridad_MN60.DigitoVerificadorBLL_60MN dv = new BLL_60MN.Seguridad_MN60.DigitoVerificadorBLL_60MN();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        string var1 = "";


        public Login()
        {
            InitializeComponent();
        }

        private void chkCambiarClave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCambiarClave.Checked)
            {
                this.Size = new Size(458, 491);
            }

            else { this.Size = new Size(441, 304); }
        }



        private void btnSalir_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // Validamos primero que no dejen campos vacíos en la interfaz
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string claveEncriptada = crypt.Encriptar(txtPassword.Text);

            try
            {
                UsuarioBLL_60MN USU1 = new BLL_60MN.UsuarioBLL_60MN();

                // Intentamos buscar el usuario con user y password correctos
                USU1 = USU1.TraerDatosUsuario(txtUsuario.Text, claveEncriptada);

                // 1. CASO EXITOSO: Si encontró el usuario con esa clave y no devolvió un objeto vacío
                if (USU1 != null && !string.IsNullOrEmpty(USU1.Username))
                {
                    // Verificamos si ya estaba bloqueado por las dudas
                    if (USU1.Locked || USU1.LoginCount >= 3)
                    {
                        MessageBox.Show("El Usuario se encuentra Bloqueado", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    // LOGIN EXITOSO: Registramos en bitácora e ingresamos- agregar bitacora
                   
                   
                   /* log.NombreOperacion = crypt.Encriptar("Login");
                    log.Descripcion = crypt.Encriptar("Login Exitoso: " + txtUsuario.Text + " ");
                    log.Criticidad = 5;
                    log.Usuarioid = USU1.IdUsuario; // Usamos IdUsuario renovado

                    string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);
                   */
                    MenuPrincipal mp = MenuPrincipal.Instance;
                    mp.Usuarioid = USU1.IdUsuario; // Asignamos el ID correcto al menú principal
                    mp.Show();

                    this.Hide();
                }
                // 2. CASO FALLIDO: No se encontró la combinación User/Password
                else
                {
                    // Buscamos al usuario sólo por su nombre para ver si existe y erró la clave, o si ya está bloqueado
                    USU1 = new BLL_60MN.UsuarioBLL_60MN();
                    USU1 = USU1.TraerDatosUsuario(txtUsuario.Text);

                    if (USU1 == null || string.IsNullOrEmpty(USU1.Username))
                    {
                        // El usuario directamente no existe en la base de datos
                        MessageBox.Show("Usuario no Encontrado: Error 104", "ErrorUser", MessageBoxButtons.OK, MessageBoxIcon.Information);

                       /* log.NombreOperacion = crypt.Encriptar("Login");
                        log.Descripcion = crypt.Encriptar("Login Usuario no encontrado: " + txtUsuario.Text + " ");
                        log.Criticidad = 4;
                        log.Usuarioid = 0;
                        log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);*/
                    }
                    else
                    {
                        // El usuario EXISTE, pero la clave fue incorrecta
                        if (USU1.Locked || USU1.LoginCount >= 3)
                        {
                            MessageBox.Show("El Usuario se encuentra Bloqueado", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            MessageBox.Show("Clave incorrecta para el usuario.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            // Incrementamos el contador de intentos fallidos
                            USU1.SumarFlagIntentos(USU1.IdUsuario);

                            // Grabamos la alerta en la bitácora
                            /*
                            log.NombreOperacion = crypt.Encriptar("Login");
                            log.Descripcion = crypt.Encriptar("Clave incorrecta para usuario: " + txtUsuario.Text + " ");
                            log.Criticidad = 4;
                            log.Usuarioid = USU1.IdUsuario;
                            log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);*/
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar iniciar sesión: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Login_Load(object sender, EventArgs e)
        {
           
        }
    }
}

            
 