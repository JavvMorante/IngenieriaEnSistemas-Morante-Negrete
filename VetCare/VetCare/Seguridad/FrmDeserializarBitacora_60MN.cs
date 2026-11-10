using Seguridad_60MN.Auditoria;

namespace VetCare
{
    /// <summary>
    /// Deserializar XML (CUS09): muestra en una grilla los eventos de un XML de bitácora
    /// exportado antes. La BLL ya leyó y validó el archivo; esta pantalla solo lo consulta.
    /// </summary>
    public partial class FrmDeserializarBitacora_60MN : FormBase_60MN
    {
        private readonly ResumenArchivoBitacora_60MN? resumen;

        public FrmDeserializarBitacora_60MN() : this(null) { }

        public FrmDeserializarBitacora_60MN(ResumenArchivoBitacora_60MN? resumen)
        {
            InitializeComponent();
            this.resumen = resumen;
            dgvEventos.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmDeserializarBitacora_60MN_Load(object sender, EventArgs e)
        {
            if (resumen != null)
                dgvEventos.DataSource = resumen.Eventos;
        }

        /// <summary>Datos del archivo (se re-traducen al cambiar de idioma).</summary>
        protected override void AlCambiarIdioma()
        {
            if (resumen == null) return;
            lblArchivo.Text =
                $"{resumen.NombreArchivo}   ·   {T("msg.XmlExportadoPor", "Exportado por")}: {resumen.ExportadoPor}   ·   " +
                $"{resumen.Exportado:dd/MM/yyyy HH:mm:ss}   ·   {T("msg.XmlEventos", "Eventos")}: {resumen.Cantidad}";
        }
    }
}
