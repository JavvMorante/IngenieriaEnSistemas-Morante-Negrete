using Seguridad_60MN.Auditoria;

namespace VetCare
{
    /// <summary>
    /// Muestra un XML de bitácora ya analizado por la BLL: el texto XML tal cual está en
    /// el archivo (descripciones cifradas), el resultado de la verificación de firmas y
    /// los eventos en una grilla. En modo importación habilita "Importar eventos" solo
    /// si el archivo es válido y tiene eventos nuevos.
    /// </summary>
    public partial class FrmVisorXmlBitacora_60MN : FormBase_60MN
    {
        private readonly ResumenArchivoBitacora_60MN? resumen;
        private readonly bool modoImportacion;

        public FrmVisorXmlBitacora_60MN() : this(null, false) { }

        public FrmVisorXmlBitacora_60MN(ResumenArchivoBitacora_60MN? resumen, bool modoImportacion)
        {
            InitializeComponent();
            this.resumen = resumen;
            this.modoImportacion = modoImportacion;
            dgvEventos.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmVisorXmlBitacora_60MN_Load(object sender, EventArgs e)
        {
            if (resumen == null) return;
            txtXml.Text = resumen.Xml.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
            txtXml.SelectionStart = 0;
            dgvEventos.DataSource = resumen.Eventos;

            lstErrores.Items.Clear();
            foreach (string error in resumen.Errores)
                lstErrores.Items.Add(error);
            if (resumen.Valido)
            {
                // Sin errores: la lista no se muestra y el contenido ocupa su lugar.
                lstErrores.Visible = false;
                tabContenido.Height += tabContenido.Top - lstErrores.Top;
                tabContenido.Top = lstErrores.Top;
            }

            btnImportar.Visible = modoImportacion;
            btnImportar.Enabled = modoImportacion && resumen.Valido && resumen.Nuevos > 0;
        }

        /// <summary>Textos armados con los datos del archivo (se re-traducen al cambiar de idioma).</summary>
        protected override void AlCambiarIdioma()
        {
            if (resumen == null) return;
            lblArchivo.Text = resumen.NombreArchivo;

            if (resumen.Valido)
            {
                lblEstado.ForeColor = Color.DarkGreen;
                lblEstado.Text = "✔ " + T("msg.XmlValido", "Archivo válido: firmas HMAC-SHA256 correctas y contenido verificado.");
            }
            else
            {
                lblEstado.ForeColor = Color.Firebrick;
                lblEstado.Text = "✖ " + T("msg.XmlRechazado", "Archivo rechazado: no se puede importar.");
            }

            string rango = resumen.Desde.HasValue ? $"{resumen.Desde:dd/MM/yyyy HH:mm} - {resumen.Hasta:dd/MM/yyyy HH:mm}" : "-";
            lblDetalle.Text =
                $"{T("msg.XmlExportadoPor", "Exportado por")}: {resumen.ExportadoPor}   ·   {resumen.Exportado:dd/MM/yyyy HH:mm:ss}   ·   " +
                $"{T("msg.XmlVersion", "Versión")} {resumen.Version}   ·   {T("msg.XmlCifrado", "Descripciones cifradas")}: " +
                $"{(resumen.Cifrado ? T("msg.Si", "Sí") : T("msg.No", "No"))}\n" +
                $"{T("msg.XmlEventos", "Eventos")}: {resumen.Cantidad}   ·   {T("msg.XmlRango", "Período")}: {rango}" +
                (resumen.Valido ? $"   ·   {T("msg.XmlNuevos", "Nuevos")}: {resumen.Nuevos}   ·   {T("msg.XmlDuplicados", "Ya existentes")}: {resumen.Duplicados}" : string.Empty) +
                $"\nSHA-256: {resumen.HashSha256}";
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            if (resumen == null || !resumen.Valido) return;
            if (!Mensajes_60MN.Confirmar($"Se agregarán {resumen.Nuevos} evento(s) a la bitácora " +
                                         $"({resumen.Duplicados} ya existente(s) se omiten). ¿Desea continuar?"))
                return;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
