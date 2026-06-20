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
    public partial class AltaFamilia : Form
    {

        BLL_60MN.ManejadorPerfilUsuarioBLL_60MN mpu = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN cryp = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();


        VetCare.MenuPrincipal mp = MenuPrincipal.Instance;
        public AltaFamilia()
        {
            InitializeComponent();
        }

        private void btAltaPerfil_Click(object sender, EventArgs e)
        {
            string nombrePerfil = txtDescripcionPerfil.Text;
            string descPerfil = txtDescripcionPerfil.Text;

            if ((txtDescripcionPerfil.Text == "") || (txtDescripcionPerfil.Text == ""))//no entra
            {
                MessageBox.Show("Verifique los datos", "Campos de Texto sin asignar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }


            else // Entra
            {


                int verificar = mpu.VerificarAltafamilia(nombrePerfil);

                if (verificar == 1) //ya existe
                {
                    MessageBox.Show("El nombre de Perfil de Usuario ya existe en la base de datos, " +
                                    "Verifique el mismo o cambie el nombre", "Error al Borrado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (verificar == 0)//se puede crear
                {
                    string rta = mpu._CrearPerfilUsuario(txtDescripcionPerfil.Text, txtDescripcionPerfil.Text);
                    MessageBox.Show("Se creo el Perfil de Usuario, configure las operaciones para el mismo", "Creacion de Perfil Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    log.Criticidad = 2;
                    log.Descripcion = txtDescripcionPerfil.Text + " " + txtDescripcionPerfil.Text;
                    log.FechayHora = DateTime.Now;
                    log.NombreOperacion = "Alta Perfil";
                    log.IngresarDatoBitacora(cryp.Encriptar(log.NombreOperacion), cryp.Encriptar(log.Descripcion), log.Criticidad, mp.Usuarioid);

                    txtDescripcionPerfil.Text = "";
                    txtDescripcionPerfil.Text = "";
                }

                else // hubo un quilombo con la BD
                {
                    MessageBox.Show("Hubo un error con la base de datos,contacte con el administrador del sistema", "Error de Proceso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
        }
    }
}
