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
    public partial class ModificarUsuarios : Form
    {

        string clave = "";
        string claveencriptada = "";
        public ModificarUsuarios()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
            BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
            BLL_60MN.UsuarioBLL_60MN usulog = BLL_60MN.UsuarioBLL_60MN.DevolverInstancia();
            try
            {
                bool _habilitado;
                if (checkHabilitado.Checked)
                {
                    _habilitado = true;
                }
                else { _habilitado = false; }



                clave = crypt.CrearPassword(8);
                claveencriptada = crypt.Encriptar(clave);



                BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN(txtusuario.Text, txtapellido.Text, txtnombre.Text, txtemail.Text, int.Parse(txtdni.Text), _habilitado, claveencriptada);

                BLL_60MN.UsuarioBLL_60MN usu2 = new BLL_60MN.UsuarioBLL_60MN();
                //verificar si existe usuario
                usu2 = usu.TraerDatosUsuario(txtusuario.Text);

                if (usu2._Usuario != null)
                {
                    MessageBox.Show("el nombre de usuario: " + txtusuario.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                }

                else
                {


                    int oresult = usu.verificarDuplicidad(int.Parse(txtdni.Text), txtemail.Text, txtusuario.Text);

                    switch (oresult)
                    {
                        case 1://dni repetido
                            MessageBox.Show("el dni: " + txtdni.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);

                            break;
                        case 2://email repetido
                            MessageBox.Show("el email: " + txtemail.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                            break;
                        case 3://email y dni repetido
                            MessageBox.Show("el email: " + txtemail.Text + " y el dni:" + txtdni.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                            break;

                        case 5://usuario repetiro
                            MessageBox.Show("el usuario: " + txtusuario.Text + " ", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                            break;


                        case 4://no existe en la base
                            usu.Dar_Alta_Usuario();

                            MessageBox.Show("Usuario " + txtusuario.Text + " se dió de alta satisfactoriamente, se envia clave al correo", "Alta de usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            string[] lineas = { " Usuario: " + txtusuario.Text, " Clave: " + clave };

                            using (StreamWriter outputfile = new StreamWriter("C:\\Users\\Default\\Desktop\\Nueva carpeta\\Usuario.txt"))
                            {
                                foreach (string linea in lineas)
                                {
                                    outputfile.WriteLine(linea);
                                }

                            }

                            log.NombreOperacion = crypt.Encriptar("Alta Usuario");
                            log.Descripcion = crypt.Encriptar("Alta de " + txtusuario.Text + " realizada con Exito!");
                            log.Criticidad = 1;

                            string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, usulog.UsuarioID);


                            txtapellido.Clear();
                            txtdni.Clear();
                            txtemail.Clear();
                            txtnombre.Clear();
                            txtusuario.Clear();

                            break;

                        default: //ocurrio un error
                            MessageBox.Show("Ha ocurrido un error,code description: " + oresult.ToString(), "Error");
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtdni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
    }

