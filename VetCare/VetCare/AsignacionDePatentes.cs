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
    public partial class AsignacionDePatentes : Form
    {

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }

        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        public AsignacionDePatentes()
        {
            InitializeComponent();
        }

        private void btSeleccionar_Click(object sender, EventArgs e)
        {
            if (cmbUsuario.Text != "")
            {
                string usuario = cmbUsuario.Text;
                AsignarOperacionesaUsuario AOU = new AsignarOperacionesaUsuario(usuario);
                AOU.Show();

            }
            else
            {
                MessageBox.Show("Seleccione un usuario", "Campos de Texto sin asignar", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AsignacionDePatentes_Load(object sender, EventArgs e)
        {
            DataTable datausuario = new DataTable();
            datausuario = log.traerUsuarios();

            foreach (DataRow item in datausuario.Rows)
            {
                cmbUsuario.Items.Add(item[0].ToString());
            }
        }
    }
}
