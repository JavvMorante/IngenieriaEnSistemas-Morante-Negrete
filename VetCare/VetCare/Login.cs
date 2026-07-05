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
            var1 = crypt.Encriptar(txtClave.Text);


            try
            {
                BLL_60MN.UsuarioBLL_60MN USU1 = new BLL_60MN.UsuarioBLL_60MN();

                USU1 = USU1.TraerDatosUsuario(txtUsuario.Text, var1);

                if (USU1.FlagIntentosLogin >= 3)
                {
                    MessageBox.Show("El Usuario se encuentra Bloqueado");


                }
                else
                {

                    if (USU1._Usuario == null)
                    {
                        USU1 = USU1.TraerDatosUsuario(txtUsuario.Text);

                        if (USU1._Usuario == null)
                        {

                            MessageBox.Show("Usuario no Encontrado:Error 104", "ErrorUser", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            log.NombreOperacion = crypt.Encriptar("Login");
                            log.Descripcion = crypt.Encriptar("Login Usuario no encontrado" + txtUsuario.Text + " ");
                            log.Criticidad = 4;
                            log.Usuarioid = 0;
                            string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);

                        }
                        else
                        {
                            switch (USU1.Nombre.ToString())
                            {
                                case "null":
                                    MessageBox.Show("Usuario no Encontrado:Error 105", "ErrorUser", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;
                                case "":
                                    MessageBox.Show("Complete el campo usuario por favor", "ErrorUser", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    log.NombreOperacion = crypt.Encriptar("Login");
                                    log.Descripcion = crypt.Encriptar("Login Usuario no encontrado" + txtUsuario.Text + " ");
                                    log.Criticidad = 4;
                                    log.Usuarioid = 0;
                                    string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);

                                    break;

                                default:

                                    MessageBox.Show("Clave incorrecta para usuario ");
                                    if (USU1.FlagIntentosLogin >= 3)
                                    {
                                        MessageBox.Show("El Usuario se encuentra Bloqueado");

                                    }
                                    else
                                    {
                                        log.NombreOperacion = crypt.Encriptar("Login");
                                        log.Descripcion = crypt.Encriptar("Clave incorrecta para usuario" + txtUsuario.Text + " ");
                                        log.Criticidad = 4;
                                        log.Usuarioid = USU1.UsuarioID;
                                        rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);
                                        USU1.SumarFlagIntentos(USU1.UsuarioID);
                                    }
                                    break;
                            }
                        }

                    }
                    else
                    {

                        if(!USU1.)

                        log.NombreOperacion = crypt.Encriptar("Login");
                        log.Descripcion = crypt.Encriptar("Login Exitoso: " + txtUsuario.Text + " ");
                        log.Criticidad = 5;
                        log.Usuarioid = USU1.UsuarioID;



                        //USUARIO Y CLAVE CORRECTOS
                        string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);
                        MenuPrincipal mp = MenuPrincipal.Instance;

                        mp.Usuarioid = USU1.UsuarioID; //asigno var usuarioid en mp

                        mp.Show();


                        this.Hide();


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

            
 