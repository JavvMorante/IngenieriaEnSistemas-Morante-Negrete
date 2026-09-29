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
        BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN crypt = new BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN();




        public ConsultarBitacora()
        {
            InitializeComponent();
        }

        private void ConsultarBitacora_Load(object sender, EventArgs e)
        {

            //cargar combo de usuarios
            DataTable datausuario = new DataTable();
            DataTable dataCriticidad = new DataTable();

            datausuario = log.traerUsuarios();
            dataCriticidad = log.traerCriticidad();

            cmbUsuario.Items.Add("Todos");
            cmbCriticidad.Items.Add("Todas");


            foreach (DataRow item in datausuario.Rows)
            {
                cmbUsuario.Items.Add(item[0].ToString());
            }

            foreach (DataRow item in dataCriticidad.Rows)
            {
                cmbCriticidad.Items.Add(item[0].ToString());
            }

            cmbCriticidad.SelectedIndex = 0;
            cmbUsuario.SelectedIndex = 0;
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
            if (dgvBitacora.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog fichero = new SaveFileDialog();
            // Cambiamos el filtro a .csv para asegurar la compatibilidad total de columnas en Excel
            fichero.Filter = "Archivo separado por comas (*.csv)|*.csv";
            fichero.FileName = "Bitacora_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            if (fichero.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Usamos UTF-8 con BOM (siglas de Byte Order Mark) para que Excel detecte los acentos y eñes al toque
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fichero.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // 1. IMPORTANTE: Le indicamos a Excel que el separador de listas es el punto y coma
                        sw.WriteLine("sep=;");

                        // 2. Escribir las Cabeceras de la grilla
                        for (int i = 0; i < dgvBitacora.Columns.Count; i++)
                        {
                            string cabecera = dgvBitacora.Columns[i].HeaderText;
                            sw.Write(cabecera + (i == dgvBitacora.Columns.Count - 1 ? "" : ";"));
                        }
                        sw.WriteLine();

                        // 3. Escribir todas las Filas de datos
                        for (int i = 0; i < dgvBitacora.Rows.Count; i++)
                        {
                            if (dgvBitacora.Rows[i].IsNewRow) continue;

                            for (int j = 0; j < dgvBitacora.Columns.Count; j++)
                            {
                                var val = dgvBitacora.Rows[i].Cells[j].Value;
                                string celda = "";

                                if (val != null)
                                {
                                    celda = val.ToString();
                                    // Limpiamos saltos de línea o comas internas que puedan romper las columnas
                                    celda = celda.Replace("\n", " ").Replace("\r", " ").Replace(";", ",");
                                }

                                // Si es la columna de Fecha y Hora, la envolvemos en comillas para que Excel no la mutile
                                if (dgvBitacora.Columns[j].HeaderText.ToLower().Contains("fecha") || j == 4)
                                {
                                    sw.Write($"\"{celda}\"");
                                }
                                else
                                {
                                    sw.Write(celda);
                                }

                                // Agregamos el punto y coma si no es la última columna
                                if (j < dgvBitacora.Columns.Count - 1)
                                {
                                    sw.Write(";");
                                }
                            }
                            sw.WriteLine();
                        }
                    }

                    MessageBox.Show("Archivo exportado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                // Forzamos el formato universal YYYY-MM-DD para que SQL Server no falle
                // string fechadesde = dtpdesde.Value.ToString("yyyy-MM-dd 00:00:00");
                //string fechahasta = dtphasta.Value.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

                //  Usa .Value y dale un formato numérico universal que SQL entienda sí o sí
                string fechadesde = dtpdesde.Value.ToString("yyyyMMdd 00:00:00");
                string fechahasta = dtphasta.Value.AddDays(1).ToString("yyyyMMdd 00:00:00");

                string criticidad = cmbCriticidad.Text;
                string usuario = cmbUsuario.Text;
                string sqlusuario = "";
                string sqlcriticidad = "";

                // Validaciones previas
                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(criticidad))
                {
                    MessageBox.Show("Por favor seleccione una opción en los filtros de Usuario y Criticidad.", "Filtros vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Configuración de Subconsulta de Usuario
                if (usuario == "Todos")
                {
                    sqlusuario = "SELECT UsuarioID FROM Usuario";
                }
                else
                {
                    sqlusuario = $"SELECT UsuarioID FROM Usuario WHERE Usuario LIKE '{usuario}'";
                }

                // Configuración de Subconsulta de Criticidad
                if (criticidad == "Todas")
                {
                    sqlcriticidad = "SELECT DISTINCT Criticidad FROM Bitacora";
                }
                else
                {
                    // Si elige un número, pasamos directamente el número limpio
                    sqlcriticidad = Convert.ToInt16(criticidad).ToString();
                }

                // Instanciamos la lógica y traemos los datos de forma segura pasándole los strings de fecha
                DataTable dt = log.ConsultarBitacora(fechadesde, fechahasta, sqlcriticidad, sqlusuario);

                // Desencriptamos las columnas correspondientes
                foreach (DataRow item in dt.Rows)
                {
                    if (item[0] != DBNull.Value) item[0] = crypt.Desencriptar(item[0].ToString());
                    if (item[1] != DBNull.Value) item[1] = crypt.Desencriptar(item[1].ToString());
                }

                dgvBitacora.DataSource = dt;
                dgvBitacora.ReadOnly = true;

                // Autoajustar columnas
                for (int i = 0; i < dgvBitacora.Columns.Count; i++)
                {
                    dgvBitacora.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la consulta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}