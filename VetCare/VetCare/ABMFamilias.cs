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
    public partial class ABMFamilias : Form
    {

        DataGridViewButtonColumn uninstallButtonColumn = new DataGridViewButtonColumn();
        DataGridViewButtonColumn ModifyButtonColumn = new DataGridViewButtonColumn();
        DataGridViewButtonColumn AsignarOperaciones = new DataGridViewButtonColumn();
        public ABMFamilias()
        {
            InitializeComponent();
        }

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }

        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        MenuPrincipal mp = MenuPrincipal.Instance;

        public void cargar()
        {

            ABMFamilias mu = new ABMFamilias();
            mu.Load += new EventHandler(ABMFamilias_Load);

        }




        private void btAltaFamilia_Click(object sender, EventArgs e)
        {
            AltaFamilia AT = new AltaFamilia();
            AT.Show();
        }

        private void ABMFamilias_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer actualizar_automatico = new System.Windows.Forms.Timer();
            actualizar_automatico.Interval = 3000;
            actualizar_automatico.Tick += actualizar_automatico_Tick;
            actualizar_automatico.Enabled = true;

            //dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //traigo usuarios y los cargo
            BLL_60MN.ManejadorPerfilUsuarioBLL_60MN mpf = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();
            DataTable dt = new DataTable();
            dt = mpf.BuscarPerfilUsuarios();
            dgvPerfilUsuario.DataSource = dt;


            //añado boton borrar usuario

            uninstallButtonColumn.Name = "BorrarPerfil";
            uninstallButtonColumn.Text = "BorrarPerfil";
            uninstallButtonColumn.UseColumnTextForButtonValue = true; //dont forget this line
            this.dgvPerfilUsuario.Columns.Add(uninstallButtonColumn);


            dgvPerfilUsuario.ReadOnly = true;

            //añado boton Modificar usuario

            ModifyButtonColumn.Name = "ModificarPerfil";
            ModifyButtonColumn.Text = "ModificarPerfil";
            ModifyButtonColumn.UseColumnTextForButtonValue = true; //dont forget this line
            this.dgvPerfilUsuario.Columns.Add(ModifyButtonColumn);

            //añado boton asignar operaciones
            AsignarOperaciones.Name = "AsignarOperaciones";
            AsignarOperaciones.Text = "AsignarOperaciones";
            AsignarOperaciones.UseColumnTextForButtonValue = true;
            AsignarOperaciones.Width = 110;
            this.dgvPerfilUsuario.Columns.Add(AsignarOperaciones);


            dgvPerfilUsuario.Columns["PerfilUsuarioID"].Visible = false;
            dgvPerfilUsuario.Columns["DVH"].Visible = false;

        }

        private void actualizar_automatico_Tick(object sender, EventArgs e)
        {


            //traigo usuarios y los cargo
            BLL_60MN.ManejadorPerfilUsuarioBLL_60MN mpu = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();
            DataTable dt = new DataTable();

            dt.Clear();

            dt = mpu.BuscarPerfilUsuarios();
            dgvPerfilUsuario.DataSource = dt;

            dgvPerfilUsuario.AllowUserToAddRows = false;
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPerfilUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BLL_60MN.ManejadorPerfilUsuarioBLL_60MN MPU = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();
            if (e.ColumnIndex == dgvPerfilUsuario.Columns["ModificarPerfil"].Index)
            {
                //Modify 

                string PerfilID = dgvPerfilUsuario.Rows[e.RowIndex].Cells["PerfilUsuarioID"].Value.ToString();

                ModificacionFamilia mu = new ModificacionFamilia(Convert.ToInt16(PerfilID));
                mu.Show();//mostrar form modificar


            }

            else if (e.ColumnIndex == dgvPerfilUsuario.Columns["BorrarPerfil"].Index)
            {
                //delete it!

                string PerfilID = dgvPerfilUsuario.Rows[e.RowIndex].Cells["PerfilUsuarioID"].Value.ToString();



                if ((MessageBox.Show("¿Esta seguro que desea Eliminar el perfil de forma permanente?", "Eliminar Perfil Usuario",
    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    try
                    {


                        string Eliminar = MPU.EliminarPerfilUsuario(Convert.ToInt16(PerfilID));

                        if (Eliminar == "True")
                        {

                            log.Criticidad = 2;
                            string a = dgvPerfilUsuario.Rows[e.RowIndex].Cells[3].Value.ToString();
                            string b = dgvPerfilUsuario.Rows[e.RowIndex].Cells[3].Value.ToString();
                            log.Descripcion = a + " " + b;
                            log.FechayHora = DateTime.Now;
                            log.NombreOperacion = "Eliminar Perfil";

                            log.IngresarDatoBitacora(crypt.Encriptar(log.NombreOperacion), crypt.Encriptar(log.Descripcion), log.Criticidad, mp.Usuarioid);

                            // Recargar DataGrid
                            this.Load += new EventHandler(ABMFamilias_Load);
                            MessageBox.Show("Perfil eliminado correctamente");

                        }
                        else
                        {
                            MessageBox.Show("Error en la eliminacion de Perfil " + Eliminar, "Error al Borrado", MessageBoxButtons.OK, MessageBoxIcon.Stop);


                        }



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());

                    }

                }

            }




            //Boton Asignar Operaciones
            else if (e.ColumnIndex == dgvPerfilUsuario.Columns["AsignarOperaciones"].Index)
            {

                string PerfilID = dgvPerfilUsuario.Rows[e.RowIndex].Cells["PerfilUsuarioID"].Value.ToString();
                string NombrePerfil = dgvPerfilUsuario.Rows[e.RowIndex].Cells["NombrePerfil"].Value.ToString();

                AsignarOperacionesAPerfil aop = new AsignarOperacionesAPerfil(Convert.ToInt16(PerfilID), NombrePerfil);
                aop.Show();//mostrar form modificar






            }
        }
    }
}