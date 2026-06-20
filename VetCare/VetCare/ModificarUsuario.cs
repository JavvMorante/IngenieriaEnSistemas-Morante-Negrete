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
    public partial class ModificarUsuario : Form
    {

        string clave = "";
        string claveencriptada = "";
        public ModificarUsuario()
        {
            InitializeComponent();
        }

        private void ModificarUsuario_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN cryp = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
            BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
            BLL_60MN.UsuarioBLL_60MN usulog = BLL_60MN.UsuarioBLL_60MN.DevolverInstancia();
            try
            {
                bool _habilitado;
                if (chkHabilitado.Checked)
                {
                    _habilitado = true;
                }
                else { _habilitado = false; }



                clave = cryp.CrearPassword(8);
                claveencriptada = cryp.Encriptar(clave);



                BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN(txtUsuario.Text, txtApellido.Text, txtNombre.Text, txtEmail.Text, int.Parse(txtDNI.Text), _habilitado, claveencriptada);

                BLL_60MN.UsuarioBLL_60MN usu2 = new BLL_60MN.UsuarioBLL_60MN();
                //verificar si existe usuario
                usu2 = usu.TraerDatosUsuario(txtUsuario.Text);

                if (usu2._Usuario != null)
                {
                    MessageBox.Show("el nombre de usuario: " + txtUsuario.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                }

                else
                {


                    int oresult = usu.verificarDuplicidad(int.Parse(txtDNI.Text), txtEmail.Text, txtUsuario.Text);

                    switch (oresult)
                    {
                        case 1://dni repetido
                            MessageBox.Show("el dni: " + txtDNI.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);

                            break;
                        case 2://email repetido
                            MessageBox.Show("el email: " + txtEmail.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                            break;
                        case 3://email y dni repetido
                            MessageBox.Show("el email: " + txtEmail.Text + " y el dni:" + txtDNI.Text + " ya existe en la base de datos,verifique los usuarios", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                            break;

                        case 5://usuario repetiro
                            MessageBox.Show("el usuario: " + txtUsuario.Text + " ", "Duplicidad de datos", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                            break;


                        case 4://no existe en la base
                            usu.Dar_Alta_Usuario();

                            MessageBox.Show("Usuario " + txtUsuario.Text + " se dió de alta satisfactoriamente, se envia clave al correo", "Alta de usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            string[] lineas = { " Usuario: " + txtUsuario.Text, " Clave: " + clave };

                            using (StreamWriter outputfile = new StreamWriter("C:\\Users\\Default\\Desktop\\Nueva carpeta\\Usuario.txt"))
                            {
                                foreach (string linea in lineas)
                                {
                                    outputfile.WriteLine(linea);
                                }

                            }

                            log.NombreOperacion = cryp.Encriptar("Alta Usuario");
                            log.Descripcion = cryp.Encriptar("Alta de " + txtUsuario.Text + " realizada con Exito!");
                            log.Criticidad = 1;

                            string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, usulog.UsuarioID);


                            txtApellido.Clear();
                            txtDNI.Clear();
                            txtEmail.Clear();
                            txtNombre.Clear();
                            txtUsuario.Clear();

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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
