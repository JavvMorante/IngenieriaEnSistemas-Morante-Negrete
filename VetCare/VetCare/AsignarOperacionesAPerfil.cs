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
    public partial class AsignarOperacionesAPerfil : Form
    {
        BLL_60MN.ManejadorPerfilUsuarioBLL_60MN MPU = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();
        List<string> listaoperaciones = new List<string>();
        int PerfilID;
        string NombrePerfil;
        public AsignarOperacionesAPerfil(int _PerfilID, string _NombrePerfil)
        {
            InitializeComponent();
            PerfilID = _PerfilID;
            NombrePerfil = _NombrePerfil;
        }

        private void AsignarOperacionesAPerfil_Load(object sender, EventArgs e)
        {
            //muestro lista de operaciones
            lblNombreUsuario.Text = this.NombrePerfil;
            listaoperaciones = MPU.MostrarListaOperaciones();
            foreach (string item in listaoperaciones)
            {
                ListOperaciones.Items.Add(item);
            }
            ListOperaciones.DisplayMember = "Descripcion";
            ListOperaciones.ValueMember = "Descripcion";


            //Muestro lista del Perfil de Usuario

            listaoperaciones.Clear(); // limpio lista y la reutilizo

            listaoperaciones = MPU.MostrarListaOperaciones(this.PerfilID);
            foreach (string item in listaoperaciones)
            {
                ListPerfilOperaciones.Items.Add(item);
                ListOperaciones.Items.Remove(item);
            }
            ListPerfilOperaciones.DisplayMember = "Descripcion";
            ListPerfilOperaciones.ValueMember = "Descripcion";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string op = ListOperaciones.SelectedItem.ToString();
                ListPerfilOperaciones.Items.Add(op);


                ListOperaciones.Items.Remove(ListOperaciones.SelectedItem);
                ListOperaciones.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Seleccione un Elemento!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDesagregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListPerfilOperaciones.SelectedItem.ToString() == "")
                {
                    MessageBox.Show("Seleccione un Elemento!");
                }
                else
                {
                    string op = ListPerfilOperaciones.SelectedItem.ToString();
                    ListOperaciones.Items.Add(op);

                    ListPerfilOperaciones.Items.Remove(ListPerfilOperaciones.SelectedItem);
                    ListPerfilOperaciones.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Seleccione un Elemento!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            List<string> listaoperacionesperfil = new List<string>();
            foreach (string item in ListPerfilOperaciones.Items)
            {
                listaoperacionesperfil.Add(item.ToString());


            }

            try
            {
                MPU.AsignarOperacionesalPerfil(this.PerfilID, listaoperacionesperfil);

                MessageBox.Show("Operaciones asignadas exitosamente", "Asignacion Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error en la Asignación", MessageBoxButtons.OK, MessageBoxIcon.None);

            }

        }
    }
}
