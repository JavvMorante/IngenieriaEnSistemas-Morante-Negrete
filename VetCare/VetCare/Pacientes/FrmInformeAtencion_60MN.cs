using BLL_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN10 Emitir Informe de Atención: vista previa del informe y emisión impresa o en
    /// PDF (Microsoft Print to PDF). Cada emisión o reimpresión queda en la bitácora.
    /// </summary>
    public partial class FrmInformeAtencion_60MN : FormBase_60MN
    {
        private readonly AtencionBLL_60MN atencionBLL = new AtencionBLL_60MN();
        private readonly int atencionId;

        public FrmInformeAtencion_60MN() : this(0) { }

        public FrmInformeAtencion_60MN(int atencionId)
        {
            InitializeComponent();
            this.atencionId = atencionId;
        }

        private void FrmInformeAtencion_60MN_Load(object sender, EventArgs e) => Componer();

        /// <summary>Paso 2: vista previa con los datos de la atención (flujo 3.1 si el diagnóstico está incompleto).</summary>
        private void Componer()
        {
            try
            {
                txtInforme.Text = atencionBLL.ComponerInforme(atencionId).Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                txtInforme.SelectionStart = 0;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
                BeginInvoke(Close);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                string? impresora = ImpresionTexto_60MN.Imprimir(this, $"Informe de atención Nº {atencionId}", txtInforme.Text);
                if (impresora == null) return;
                Emitido(impresora);
            }
            catch (Exception ex)
            {
                // 4.1: falla de la impresora; el informe puede guardarse como archivo.
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string? archivo = ImpresionTexto_60MN.GuardarComoTexto(this, $"Informe_Atencion_{atencionId}.txt", txtInforme.Text);
                if (archivo == null) return;
                Emitido(Path.GetFileName(archivo));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void Emitido(string destino)
        {
            atencionBLL.RegistrarEmisionInforme(atencionId, destino);
            Mensajes_60MN.Informar("Informe emitido correctamente.");
            // La siguiente emisión se registra como reimpresión.
            Componer();
        }
    }
}
