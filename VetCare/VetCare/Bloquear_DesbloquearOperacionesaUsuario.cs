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
    public partial class Bloquear_DesbloquearOperacionesaUsuario : Form
    {

        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN cryp = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        List<string> listaoperaciones = new List<string>();
        BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN();
        string NombreUsuario = "";
        BLL_60MN.ManejadorPerfilUsuarioBLL_60MN mpu = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }

        public Bloquear_DesbloquearOperacionesaUsuario()
        {
            InitializeComponent();
        }

        private void Bloquear_DesbloquearOperacionesaUsuario_Load(object sender, EventArgs e)
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
            chkListOperaciones.Items.Clear();
            listaoperaciones = usu.MostraroperacionUsuario(NombreUsuario);

            foreach (string item in listaoperaciones)
            {
                chkListOperaciones.Items.Add(item.ToString());

            }
            chkListOperaciones.DisplayMember = "Descripcion";
            chkListOperaciones.ValueMember = "Descripcion";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("¿Esta seguro que desea bloquearle la operación a " + NombreUsuario + "?", "Bloqueo de Operaciones",
MessageBoxButtons.YesNo, MessageBoxIcon.Question,
MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
            {
                try
                {
                    if (cmbUsuario.Text == "")
                    {
                        MessageBox.Show("Asigne a un Usuario!");
                    }
                    else
                    {

                        foreach (string item in chkListOperaciones.CheckedItems)
                        {

                            //verificar Patentes Unicas
                            string patente = item.ToString();

                            string Eliminar = usu.verificarPatentesBloqueo(NombreUsuario, patente);

                            if (Eliminar == "True")
                            {
                                //se puede bloquear
                                //Bloquear Patentes

                                mpu.BloqueaOperacionUsuario(NombreUsuario, patente);


                            }
                            else
                            {

                                MessageBox.Show("Usuario con Patente Unica[" + patente + "],no se puede Bloquear!", "No se puede Bloquear", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                            }
                        }

                        MessageBox.Show("Ejecución Finalizada", "Ejecución", MessageBoxButtons.OK, MessageBoxIcon.Information);



                    }


                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());

                }

            }
        }
    }
}
