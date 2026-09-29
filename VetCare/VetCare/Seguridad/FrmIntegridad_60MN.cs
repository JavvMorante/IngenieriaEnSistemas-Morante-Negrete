using BLL_60MN;
using Seguridad_60MN.Integridad;

namespace VetCare
{
    /// <summary>Verificación y recálculo de los dígitos verificadores horizontales y verticales.</summary>
    public partial class FrmIntegridad_60MN : FormBase_60MN
    {
        private readonly IntegridadBLL_60MN integridadBLL = new IntegridadBLL_60MN();

        public FrmIntegridad_60MN()
        {
            InitializeComponent();
            dgvInconsistencias.AutoGenerateColumns = false;
        }

        private void FrmIntegridad_60MN_Load(object sender, EventArgs e) => Verificar();

        private void btnVerificar_Click(object sender, EventArgs e) => Verificar();

        private void Verificar()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                List<InconsistenciaDV_60MN> inconsistencias = integridadBLL.Verificar();
                dgvInconsistencias.DataSource = inconsistencias;
                bool integra = inconsistencias.Count == 0;
                lblEstado.Text = integra
                    ? "✔ La base de datos está íntegra."
                    : $"✖ Se detectaron {inconsistencias.Count} inconsistencia(s).";
                lblEstado.ForeColor = integra ? Color.DarkGreen : Color.Firebrick;
                btnRecalcular.Enabled = !integra;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            if (!Mensajes_60MN.Confirmar("Recalcular los dígitos verificadores acepta el estado ACTUAL de la base como válido.\n\n" +
                                         "Hágalo solo después de revisar y corregir las inconsistencias detectadas. ¿Continuar?"))
                return;
            try
            {
                integridadBLL.RecalcularTodo();
                (MdiParent as FrmMenuPrincipal_60MN)?.ActualizarEstadoIntegridad();
                Mensajes_60MN.Informar("Dígitos verificadores recalculados correctamente.");
                Verificar();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
