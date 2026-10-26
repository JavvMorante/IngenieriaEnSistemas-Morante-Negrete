using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUS12 Gestión de Backup. Cada sección se muestra según la patente del usuario
    /// (Tag SEG_BACKUP / SEG_RESTORE). Después de restaurar, BaseRestaurada queda en
    /// true y el menú principal cierra la sesión (paso 9).
    /// </summary>
    public partial class FrmBackupRestore_60MN : FormBase_60MN
    {
        private readonly BackupBLL_60MN backupBLL = new BackupBLL_60MN();

        /// <summary>True si se restauró la base: hay que cerrar la sesión.</summary>
        public bool BaseRestaurada { get; private set; }

        public FrmBackupRestore_60MN()
        {
            InitializeComponent();
            dgvRespaldos.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmBackupRestore_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            if (!grpRespaldo.Visible && grpRestaurar.Visible)
                grpRestaurar.Top = grpRespaldo.Top;
            try
            {
                txtCarpeta.Text = backupBLL.CarpetaPredeterminada();
                if (grpRestaurar.Visible)
                    CargarRespaldos();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        protected override void AlCambiarIdioma() =>
            lblArchivoNuevo.Text = $"{T("msg.ArchivoAGenerar", "Archivo a generar")}: {backupBLL.PatronNombreArchivo}";

        private void CargarRespaldos()
        {
            List<RespaldoBD_60MN> respaldos = backupBLL.ListarRespaldos();
            dgvRespaldos.DataSource = respaldos;
            if (respaldos.Count == 0)
                txtArchivo.Clear();
        }

        // ------------------------------- Respaldar -------------------------------

        private void btnExaminarCarpeta_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialogo = new FolderBrowserDialog
            {
                Description = T("msg.ElegirCarpetaBackup", "Carpeta de destino del backup"),
                SelectedPath = Directory.Exists(txtCarpeta.Text) ? txtCarpeta.Text : string.Empty
            };
            if (dialogo.ShowDialog(this) == DialogResult.OK)
                txtCarpeta.Text = dialogo.SelectedPath;
        }

        private void btnRespaldar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                RespaldoBD_60MN respaldo = backupBLL.Respaldar(txtCarpeta.Text);
                Cursor = Cursors.Default;
                Mensajes_60MN.Informar($"Backup generado correctamente.\n\n{respaldo.Ruta}\n{respaldo.Tamanio}" +
                                       (respaldo.Verificado ? " - verificado con checksum." : "."));
                if (grpRestaurar.Visible)
                    CargarRespaldos();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // ------------------------------- Restaurar -------------------------------

        private void dgvRespaldos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRespaldos.CurrentRow?.DataBoundItem is RespaldoBD_60MN respaldo)
                txtArchivo.Text = respaldo.Ruta;
        }

        private void btnExaminarArchivo_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialogo = new OpenFileDialog
            {
                Filter = "Backup de SQL Server (*.bak)|*.bak|Todos los archivos (*.*)|*.*",
                // Cualquier carpeta: VetCare copia el archivo a la carpeta de SQL Server antes de restaurar.
                InitialDirectory = Directory.Exists(txtCarpeta.Text)
                    ? txtCarpeta.Text
                    : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                CheckFileExists = true
            };
            if (dialogo.ShowDialog(this) == DialogResult.OK)
                txtArchivo.Text = dialogo.FileName;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            RespaldoBD_60MN respaldo;
            try
            {
                // 8.1: se rechaza un archivo que no sea un backup válido de la base, sin tocarla.
                Cursor = Cursors.WaitCursor;
                respaldo = backupBLL.ValidarArchivo(txtArchivo.Text);
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                Mensajes_60MN.MostrarError(ex);
                return;
            }

            // Pasos 6 y 7 (7.1: el administrador cancela y no se hace ningún cambio).
            if (!Mensajes_60MN.Confirmar(
                    $"Se reemplazará la base {respaldo.BaseDatos} por el backup del {respaldo.Fecha:dd/MM/yyyy HH:mm:ss}.\n\n" +
                    "Se perderán todos los cambios posteriores a esa fecha, incluidos los eventos de la bitácora, " +
                    "y al terminar se cerrará la sesión.\n\n¿Desea continuar?"))
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                ResultadoRestore_60MN resultado = backupBLL.Restaurar(respaldo.Ruta);
                Cursor = Cursors.Default;
                BaseRestaurada = true;
                Mensajes_60MN.Informar("Base de datos restaurada correctamente.\n\n" +
                    (resultado.Inconsistencias.Count == 0
                        ? "Los dígitos verificadores de la base restaurada son correctos."
                        : $"Se detectaron {resultado.Inconsistencias.Count} inconsistencia(s) de dígitos verificadores en la base restaurada.") +
                    "\n\nLa sesión se cerrará: vuelva a iniciar sesión.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
