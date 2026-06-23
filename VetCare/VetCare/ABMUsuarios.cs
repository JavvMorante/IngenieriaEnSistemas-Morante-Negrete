using Microsoft.VisualBasic.Logging;
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
    public partial class ABMUsuarios : Form
    {

        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN cryp = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();

        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();

        public void cargar()
        {
            ABMUsuarios mu = new ABMUsuarios();
            mu.Load += new EventHandler(ABMUsuarios_Load);
        }
        public ABMUsuarios()
        {
            InitializeComponent();
        }
        DataGridViewButtonColumn uninstallButtonColumn = new DataGridViewButtonColumn();
        DataGridViewButtonColumn ModifyButtonColumn = new DataGridViewButtonColumn();

        public void ABMUsuarios_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer actualizar_automatico = new System.Windows.Forms.Timer();
            actualizar_automatico.Interval = 10000;
            actualizar_automatico.Tick += actualizar_automatico_Tick;
            actualizar_automatico.Enabled = true;

            BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN();
            DataTable dt = new DataTable();
            dt = usu.MostrarUsuarios();
            dgvUsuarios.DataSource = dt;

            // añado boton de borrar usuario

            uninstallButtonColumn.Name = "BorrarUsuario";
            uninstallButtonColumn.Text = "Borrar Usuario";
            uninstallButtonColumn.UseColumnTextForButtonValue = true;
            this.dgvUsuarios.Columns.Add(uninstallButtonColumn);

            dgvUsuarios.ReadOnly = true;

            // añado boton de modificar usuario

            ModifyButtonColumn.Name = "ModificarUsuario";
            ModifyButtonColumn.Text = "Modificar Usuario";
            ModifyButtonColumn.UseColumnTextForButtonValue = true;
            this.dgvUsuarios.Columns.Add(ModifyButtonColumn);

            dgvUsuarios.Columns["UsuarioId"].Visible = false;
            dgvUsuarios.Columns["Clave"].Visible = false;

            HelpProvider helpProvider = new HelpProvider();

            helpProvider.ResetShowHelp(this);
            helpProvider.HelpNamespace = Application.StartupPath + @"\AyudaVetCare.chm";// agregar ayuda
            helpProvider.SetHelpKeyword(this, "USUARIOS");
            helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);



        }

        private void actualizar_automatico_Tick(object sender, EventArgs e)
        {
            BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN();
            DataTable dt = new DataTable();
            dt = usu.MostrarUsuarios();
            dgvUsuarios.DataSource = dt;

            dgvUsuarios.AllowUserToAddRows = false;
        }

        private void borrarUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //aca poner accion de borrar usuario y grabar en la bitacora
            if (e.ColumnIndex == dgvUsuarios.Columns["BorrarUsuario"].Index)
            {
                //Do something with your button.
            }
        }

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }



        private void btSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            BLL_60MN.UsuarioBLL_60MN usu = new BLL_60MN.UsuarioBLL_60MN();

            if (e.ColumnIndex == dgvUsuarios.Columns["ModificarUsuario"].Index)
            {
                string usuid = dgvUsuarios.Rows[e.RowIndex].Cells["UsuarioID"].Value.ToString();
                ModifyUser_60MN mu = new ModifyUser_60MN(usuid);
                mu.Show();
            }
            else if (e.ColumnIndex == dgvUsuarios.Columns["BorrarUsuario"].Index)
            {
                string usuid = dgvUsuarios.Rows[e.RowIndex].Cells["UsuarioID"].Value.ToString();

                if ((MessageBox.Show("¿Esta seguro que desea Eliminar al usuario " + usu.Nombre + " de forma permanente?", "Eliminar Usuario",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                {
                    try
                    {
                        string Eliminar = usu.verificarPatentesEscenciales(Convert.ToInt16(usuid));

                        if (Eliminar == "True")
                        {
                            log.Criticidad = 2;
                            string a = dgvUsuarios.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                            string b = dgvUsuarios.Rows[e.RowIndex].Cells["Apellido"].Value.ToString();
                            log.Descripcion = a + " " + b;
                            log.FechayHora = DateTime.Now;
                            log.NombreOperacion = "Eliminar Usuario";

                            log.IngresarDatoBitacora(cryp.Encriptar(log.NombreOperacion), cryp.Encriptar(log.Descripcion), log.Criticidad, usu.UsuarioID);

                            string result = usu.EliminarUsuario(Convert.ToInt16(usuid));

                            // Recargar DataGrid
                            this.Load += new EventHandler(ABMUsuarios_Load);
                            MessageBox.Show("Usuario eliminado correctamente");
                        }
                        else
                        {
                            MessageBox.Show("Usuario con Patentes Unicas,no se puede eliminar!,Patentes: " + Eliminar, "Error al Borrado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void btnAltaUsuario_Click(object sender, EventArgs e)
        {
            ModificarUsuario alta = new ModificarUsuario();
            alta.Show();
        }
    }
}
