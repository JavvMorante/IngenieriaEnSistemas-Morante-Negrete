using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace VetCare
{
    public partial class ModifyUser_60MN : Form
    {

        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();

        public string Dato { get; set; }

        public ModifyUser_60MN()
        {
            InitializeComponent();
        }
        public ModifyUser_60MN(string dato)
        {
            InitializeComponent();
            Dato = dato;
        }

        private void ModifyUser_60MN_Load(object sender, EventArgs e)
        {
            usu = usu.TraerDatosUsuariobyID(Convert.ToInt16(Dato));
           

            txtNombre.Text = usu.Nombre.ToString();
            txtDNI.Text = usu.Dni.ToString();
            txtEmail.Text = usu.Email.ToString();
            txtApellido.Text = usu.Apellido.ToString();
            txtUsuario.Text = usu._Usuario.ToString();

            if (usu.Habilitado.ToString() == "true")
            {
                chkHabilitado.Checked = true;
            }
            else
            {
                chkHabilitado.Checked = false;
            }


        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ABMUsuarios mu = new ABMUsuarios();

            mu.cargar();
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
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
                case 5://Usuario repetido
                    MessageBox.Show("el Usuario: " + txtUsuario.Text + " !", "Duplicidad de datos", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                    break;
                case 4: //modifica*/
                    try
                    {
                        bool check;
                        if (chkHabilitado.Checked)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                        }
                        string result = usu.ModificarDatosUsuario(txtUsuario.Text, txtApellido.Text, txtNombre.Text, txtEmail.Text,
                                    Convert.ToInt64(txtDNI.Text), check, usu.UsuarioID);

                        MessageBox.Show(result);

                        log.Criticidad = 3;
                        log.Descripcion = txtUsuario.Text + " " + txtApellido.Text + " " + txtNombre.Text + " " + txtEmail.Text + " "
                        + txtDNI.Text + " " + check + " " + usu.UsuarioID;
                        log.FechayHora = DateTime.Now;
                        log.NombreOperacion = "Modificar Usuario";

                        log.IngresarDatoBitacora(crypt.Encriptar(log.NombreOperacion), crypt.Encriptar(log.Descripcion), log.Criticidad, usu.UsuarioID);

                        ABMUsuarios abmusu = new ABMUsuarios();
                        abmusu.Load += new EventHandler(abmusu.ABMUsuarios_Load);

                        this.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    break;


            }
        }

        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
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
