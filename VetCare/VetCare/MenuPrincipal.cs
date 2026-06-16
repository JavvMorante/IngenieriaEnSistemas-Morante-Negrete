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
    public partial class MenuPrincipal : Form
    {
        private static MenuPrincipal? instance;
        public int Usuarioid;

        public MenuPrincipal()
        {
            InitializeComponent();
            // Vinculamos el evento de cierre de forma manual para asegurar el fin de la app
            this.FormClosed += MenuPrincipal_FormClosed;
        }

        public static MenuPrincipal Instance
        {
            get
            {
                // Agregamos la validación IsDisposed por si el formulario se cerró y se vuelve a invocar
                if (instance == null || instance.IsDisposed)
                {
                    instance = new MenuPrincipal();
                }
                return instance;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        // CONTROL AL CERRAR EL MENÚ: Cierra definitivamente la aplicación en segundo plano
        private void MenuPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void gestionDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ABMUsuarios aBMUsuarios = new ABMUsuarios();
            aBMUsuarios.Show();
            this.Hide();

        }
    }
}