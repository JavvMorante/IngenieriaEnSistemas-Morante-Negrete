using BLL_60MN;
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
    public partial class LogOut : Form
    {

        BLL_60MN.UsuarioBLL_60MN usu = BLL_60MN.UsuarioBLL_60MN.DevolverInstancia();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        public LogOut()
        {
            InitializeComponent();
        }

        private void LogOut_Load(object sender, EventArgs e)
        {
            BLL_60MN.IdiomaBLL_60MN idi = new BLL_60MN.IdiomaBLL_60MN();
            string idiom = idi.CargarIdioma();

            switch (idiom)
            {
                case "Español":



                    lblConfirm.Text = Idioma.Espanol.lblConfirm.ToString();
                    btn_cancelar.Text = Idioma.Espanol.btn_cancelar.ToString();
                    btn_salir.Text = Idioma.Espanol.btn_salir.ToString();

                    break;

                case "Ingles":

                    lblConfirm.Text = Idioma.Ingles.lblConfirm.ToString();
                    btn_cancelar.Text = Idioma.Ingles.btn_cancelar.ToString();
                    btn_salir.Text = Idioma.Ingles.btn_salir.ToString();

                    break;

                default:
                    break;

            }
        }

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_salir_Click(object sender, EventArgs e)
        {
            log.NombreOperacion = crypt.Encriptar("LogOut");
            log.Descripcion = crypt.Encriptar("LogOut Usuario " + usu.UsuarioID + " ");
            log.Criticidad = 4;
            log.Usuarioid = usu.UsuarioID;
            string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);



            Environment.Exit(1);

        }
    }
}
