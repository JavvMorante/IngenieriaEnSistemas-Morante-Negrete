using System;
using System.Drawing;
using System.Windows.Forms;

namespace VetCareProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            string[] opciones = {
                "Administración",
                "Pacientes",
                "Turnos",
                "Historias Clínicas",
                "Servicios",
                "Productos / Stock",
                "Ventas",
                "Cobranzas",
                "Reportes",
                "Configuración",
                "Ayuda"
            };

        }
    }
}