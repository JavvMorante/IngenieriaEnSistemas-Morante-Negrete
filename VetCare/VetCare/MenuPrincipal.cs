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
        private static MenuPrincipal instance;
        public int Usuarioid;


        public MenuPrincipal()
        {
            InitializeComponent();
        }

        public static MenuPrincipal Instance
        {
            get
            {
                if (instance == null)
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
    }
}
