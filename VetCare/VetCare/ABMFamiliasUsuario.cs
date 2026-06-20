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
    public partial class ABMPerfilesUsuario : Form
    {

        DataTable dt = new DataTable();

        public ABMPerfilesUsuario()
        {
            InitializeComponent();
        }

        private void btAlta_Click(object sender, EventArgs e)
        {

        }

        private void btSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ABMFamiliasUsuario_Load(object sender, EventArgs e)
        {
            BLL_60MN.ManejadorPerfilUsuarioBLL_60MN pu = new BLL_60MN.ManejadorPerfilUsuarioBLL_60MN();

            dt = pu.BuscarPerfilUsuarios();
            dgvPerfiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPerfiles.DataSource = dt;

        }

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
