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
    public partial class ModificacionFamilia : Form
    {

        BLL_60MN.ManejadorPerfilUsuarioBLL_60MN mpu = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        VetCare.MenuPrincipal mp = MenuPrincipal.Instance;

        int _PerfilID;
        public ModificacionFamilia(int PerfilID)
        {
            InitializeComponent();
            _PerfilID = _PerfilID;
        }

        private void btnModificarPerfil_Click(object sender, EventArgs e)
        {
            if ((txtNombrePerfil.Text == "") || (txtDescripcionPerfil.Text == ""))//no entra
            {
                MessageBox.Show("Verifique los datos", "Campos de Texto sin asignar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                string rta = mpu.ModificarPerfilUsuario(txtNombrePerfil.Text, txtDescripcionPerfil.Text, _PerfilID);

                if (rta == "True")
                {
                    log.Criticidad = 2;
                    log.Descripcion = txtNombrePerfil.Text + " " + txtDescripcionPerfil.Text;
                    log.FechayHora = DateTime.Now;
                    log.NombreOperacion = "Modificacion Perfil";
                    log.IngresarDatoBitacora(crypt.Encriptar(log.NombreOperacion), crypt.Encriptar(log.Descripcion), log.Criticidad, mp.Usuarioid);

                    txtDescripcionPerfil.Text = "";
                    txtNombrePerfil.Text = "";

                    MessageBox.Show("Se modificó el perfil exitosamente", "Modificacion OK", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el perfil", "Modificacion Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }




            }
        }

        private void ModificacionFamilia_Load(object sender, EventArgs e)
        {
            DataTable DT = new DataTable();
            DT = mpu.BuscarPerfilUsuarios();
            DataRow[] dato = DT.Select("PerfilUsuarioID = " + _PerfilID);


            txtNombrePerfil.Text = dato[0].ItemArray[1].ToString();
            txtDescripcionPerfil.Text = dato[0].ItemArray[2].ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
