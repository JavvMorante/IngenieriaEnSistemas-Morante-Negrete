using BLL_60MN.Seguridad_MN60;
using Microsoft.VisualBasic;
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
    public partial class Login : Form
    {

        BLL_60MN.Seguridad_60MN.DigitoVerificador_60MN dv = new BLL_60MN.Seguridad_60MN.DigitoVerificador_60MN();
        BLL_60MN.Seguridad_60MN.Bitacora_60MN log = new BLL_60MN.Seguridad_60MN.Bitacora_60MN();
        BLL_60MN.Seguridad_60MN.Encriptacion_60MN crypt = new BLL_60MN.Seguridad_60MN.Encriptacion_60MN();
        string var1 = "";


        public Login()
        {
            InitializeComponent();
        }

        private void chkCambiarClave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCambiarClave.Checked)
            {
                this.Size = new Size(458, 491);
            }

            else { this.Size = new Size(441, 304); }
        }



        private void btnSalir_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
          var1 = crypt.Encriptar(txtPassword.Text);

            try
            {
                UsuarioBLL_60MN USU1 = new BLL_60MN.UsuarioBLL_60MN();

                USU1 = USU1.TraerDatosUsuario(txtUsuario.Text, var1);
            }

        }
    }
}
