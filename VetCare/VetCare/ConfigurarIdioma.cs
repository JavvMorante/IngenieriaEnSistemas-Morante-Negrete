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
    public partial class ConfigurarIdioma : Form
    {

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }
        public ConfigurarIdioma()
        {
            InitializeComponent();
        }

        private void ConfigurarIdioma_Load(object sender, EventArgs e)
        {

        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            BLL_60MN.IdiomaBLL_60MN idioma = new BLL_60MN.IdiomaBLL_60MN();
            BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN cryp = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();
            BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();

            try
            {


                DialogResult dialogResult = MessageBox.Show("Usted está a punto de cambiar el idioma ,Confirmar Operacion", "Cambio de Idioma", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //do something
                    if (cmbIdioma.Text == "Español")
                    {
                        idioma.IdiomaID = 1;
                        idioma.Descripcion = idioma.SetearIdioma(idioma.IdiomaID);
                    }
                    if (cmbIdioma.Text == "Ingles")
                    {
                        idioma.IdiomaID = 2;
                        idioma.Descripcion = idioma.SetearIdioma(idioma.IdiomaID);
                    }
                    MessageBox.Show("Se Cambió el idioma satisfactoriamente, por favor reinicie sesion para visualizar los cambios");


                    this.Close();
                }

                else if (dialogResult == DialogResult.No)
                {
                    //do something else

                }



            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
