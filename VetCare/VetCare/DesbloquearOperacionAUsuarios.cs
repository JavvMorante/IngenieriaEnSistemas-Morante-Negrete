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
    public partial class DesbloquearOperacionAUsuarios : Form
    {

        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        List<string> listaoperaciones = new List<string>();
        BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN();
        string NombreUsuario = "";
        BLL_60MN.ManejadorPerfilUsuarioBLL_60MN mpu = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }
        public DesbloquearOperacionAUsuarios()
        {
            InitializeComponent();
        }

        private void DesbloquearOperacionAUsuarios_Load(object sender, EventArgs e)
        {
            //cargar combo de usuarios
            DataTable datausuario = new DataTable();
            datausuario = log.traerUsuarios();


            foreach (DataRow item in datausuario.Rows)
            {
                cmbUsuario.Items.Add(item[0].ToString());
            }

        }

        private void cmbUsuario_SelectedValueChanged(object sender, EventArgs e)
        {
            //lleno el CheckedListBox con las operaciones

            NombreUsuario = cmbUsuario.Text;
            chklistOperaciones.Items.Clear();
            listaoperaciones = usu.MostrarOperacionesBloqueadas(NombreUsuario);

            foreach (string item in listaoperaciones)
            {
                chklistOperaciones.Items.Add(item.ToString());

            }
            chklistOperaciones.DisplayMember = "Descripcion";
            chklistOperaciones.ValueMember = "Descripcion";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            NombreUsuario = cmbUsuario.Text;
            try
            {

                if (cmbUsuario.Text == "")
                {
                    MessageBox.Show("Asigne a un Usuario!");
                }
                else
                {
                    if (chklistOperaciones.CheckedItems.Count > 0)
                    {
                        foreach (string item in chklistOperaciones.CheckedItems)
                        {

                            string patente = item.ToString();

                            mpu.DesbloqueaOperacionaUsuario(NombreUsuario, patente);


                        }

                        MessageBox.Show("Ejecución Finalizada", "Ejecución", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("Complete los Campos!", "Ejecución Fallida", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    }



                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
