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
    public partial class DigitosVerificadores : Form
    {

        string rta;
        BLL_60MN.UsuarioBLL_60MN usu = BLL_60MN.UsuarioBLL_60MN.DevolverInstancia();
        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN cryp = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();

        BLL_60MN.Seguridad_MN60.DigitoVerificadorBLL_60MN digitos = new BLL_60MN.Seguridad_MN60.DigitoVerificadorBLL_60MN();

        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }
        public DigitosVerificadores()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rta = digitos.RecalcularDVH();


            MessageBox.Show("Digitos verificadores: " + rta.ToString(), "Digitos Verificadores", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);


            log.NombreOperacion = cryp.Encriptar("Recalculo de Digitos Verificadores");
            log.Descripcion = cryp.Encriptar("Recalculo de digitos realizado con Exito!");
            log.Criticidad = 1;
            log.Usuarioid = 0;

            string bita = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);


        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            rta = digitos.CalcularDigitosVerificadores();

            MessageBox.Show("Digitos verificadores: " + rta.ToString(), "Digitos Verificadores", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);


            log.NombreOperacion = cryp.Encriptar("Consultar Digitos Verificadores");
            log.Descripcion = cryp.Encriptar("Consulta de  digitos realizado con Exito!");
            log.Criticidad = 1;
            log.Usuarioid = 0;

            string bita = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
