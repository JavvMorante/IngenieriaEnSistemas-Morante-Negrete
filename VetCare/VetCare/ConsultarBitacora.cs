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
    public partial class ConsultarBitacora : Form
    {

        BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN log = new BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN();
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN cryp = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();

        public ConsultarBitacora()
        {
            InitializeComponent();
        }

        private void ConsultarBitacora_Load(object sender, EventArgs e)
        {
            //cargar combo de usuarios
            DataTable datausuario = new DataTable();
            datausuario = log.traerUsuarios();

            cmbUsuario.Items.Add("Todos");

            foreach (DataRow item in datausuario.Rows)
            {
                cmbUsuario.Items.Add(item[0].ToString());
            }

        }
        public void Show(object sender, EventArgs e)
        {
            this.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExpExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();
                hoja_trabajo =
                    (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                //Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < dgvBitacora.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dgvBitacora.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 1, j + 1] = dgvBitacora.Rows[i].Cells[j].Value.ToString();
                    }
                }
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
            }


            MessageBox.Show("Excel Exportado con Exito", "Exportacion Excel OK", MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            //fin
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime fechadesde = Convert.ToDateTime(dtpdesde.Text);
                DateTime fechahasta = Convert.ToDateTime(dtphasta.Text);
                string criticidad = cmbCriticidad.Text;
                string usuario = cmbUsuario.Text;
                string sqlusuario = "";
                string sqlcriticidad = "";

                switch (usuario)
                {
                    case "":
                        MessageBox.Show("seccione un usuario", "Usuario Vacio", MessageBoxButtons.OK,
                      MessageBoxIcon.Hand);

                        break;

                    case "Todos":

                        sqlusuario = "select usuarioid from usuario";

                        break;


                    default:
                        sqlusuario = "select usuarioid from usuario where usuario like '" + usuario + "'";
                        break;
                }

                switch (criticidad)
                {
                    case "":
                        MessageBox.Show("seccione un usuario", "Usuario Vacio", MessageBoxButtons.OK,
                      MessageBoxIcon.Hand);
                        break;

                    case "Todas":
                        sqlcriticidad = "select distinct criticidad from bitacora";

                        break;

                    default:
                        sqlcriticidad = "select criticidad from bitacora where criticidad = " + Convert.ToInt16(criticidad) + "";
                        break;
                }

                //llenamos la bitacora con todo lo filtrado

                DataTable dt = new DataTable();

                dt = log.ConsultarBitacora(fechadesde, fechahasta, sqlcriticidad, sqlusuario);



                foreach (DataRow item in dt.Rows)
                {
                    item[0] = cryp.Desencriptar(item[0].ToString());

                    item[1] = cryp.Desencriptar(item[1].ToString());

                }

                dgvBitacora.DataSource = dt;
                dgvBitacora.ReadOnly = true;

                this.dgvBitacora.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dgvBitacora.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dgvBitacora.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dgvBitacora.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dgvBitacora.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;




            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
