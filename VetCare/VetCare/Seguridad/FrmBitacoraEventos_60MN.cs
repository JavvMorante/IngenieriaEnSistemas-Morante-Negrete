using BLL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;

namespace VetCare
{
    /// <summary>CUS09 Auditar Eventos (Bitácora de Eventos).</summary>
    public partial class FrmBitacoraEventos_60MN : FormBase_60MN
    {
        private readonly BitacoraBLL_60MN bitacoraBLL = new BitacoraBLL_60MN();
        private List<Usuario_60MN> usuarios = new List<Usuario_60MN>();

        // Filtros de la última búsqueda: Exportar XML exporta exactamente lo que se ve en la grilla.
        private FiltroBitacora_60MN? ultimoFiltro;

        public FrmBitacoraEventos_60MN()
        {
            InitializeComponent();
            dgvEventos.AutoGenerateColumns = false;
        }

        /// <summary>Paso 2: tipifica módulos, eventos y criticidades en listas desplegables.</summary>
        private void FrmBitacoraEventos_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            cboCriticidad.Items.Add(OpcionCombo_60MN.Todos);
            string[] descripciones = { "", "1 - Muy alta", "2 - Alta", "3 - Media", "4 - Baja", "5 - Informativa" };
            for (int nivel = 1; nivel <= 5; nivel++)
                cboCriticidad.Items.Add(new OpcionCombo_60MN(descripciones[nivel], nivel));

            cboModulo.Items.Add(OpcionCombo_60MN.Todos);
            foreach (ModuloSistema_60MN modulo in Enum.GetValues<ModuloSistema_60MN>())
                cboModulo.Items.Add(new OpcionCombo_60MN(modulo.ToString(), modulo));

            cboUsuario.Items.Add(OpcionCombo_60MN.Todos);
            try
            {
                usuarios = bitacoraBLL.ListarUsuariosParaFiltro();
                foreach (Usuario_60MN u in usuarios)
                    cboUsuario.Items.Add(new OpcionCombo_60MN($"{u.NombreUsuario} ({u.Apellido})", u.Id));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }

            LimpiarFiltros();
            Buscar();
        }

        private void cboModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModuloSistema_60MN? modulo = OpcionCombo_60MN.ValorDe<ModuloSistema_60MN>(cboModulo);
            IEnumerable<EventoSistema_60MN> eventos = modulo.HasValue
                ? CatalogoEventos_60MN.EventosDe(modulo.Value)
                : Enum.GetValues<EventoSistema_60MN>();

            cboEvento.Items.Clear();
            cboEvento.Items.Add(OpcionCombo_60MN.Todos);
            foreach (EventoSistema_60MN evento in eventos.OrderBy(ev => ev.ToString()))
                cboEvento.Items.Add(new OpcionCombo_60MN(evento.ToString(), evento));
            cboEvento.SelectedIndex = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        private void Buscar()
        {
            FiltroBitacora_60MN filtro = new FiltroBitacora_60MN
            {
                Desde = dtpDesde.Checked ? dtpDesde.Value.Date : null,
                Hasta = dtpHasta.Checked ? dtpHasta.Value.Date : null,
                Modulo = OpcionCombo_60MN.ValorDe<ModuloSistema_60MN>(cboModulo),
                Evento = OpcionCombo_60MN.ValorDe<EventoSistema_60MN>(cboEvento),
                Criticidad = OpcionCombo_60MN.ValorDe<int>(cboCriticidad),
                UsuarioId = OpcionCombo_60MN.ValorDe<int>(cboUsuario)
            };

            // 3.1: fecha de inicio posterior a la de fin.
            if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde > filtro.Hasta)
            {
                Mensajes_60MN.Advertir("La fecha de inicio no puede ser posterior a la fecha de fin.");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                List<RegistroBitacora_60MN> eventos = bitacoraBLL.Consultar(filtro);
                ultimoFiltro = filtro;
                dgvEventos.DataSource = eventos;
                lblResultados.Text = $"{eventos.Count} evento(s)";
                // 4.1: sin resultados.
                if (eventos.Count == 0)
                    Mensajes_60MN.Informar("No se encontraron eventos.");
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

        private void btnLimpiar_Click(object sender, EventArgs e) => LimpiarFiltros();

        private void LimpiarFiltros()
        {
            dtpDesde.Value = dtpHasta.Value = DateTime.Today;
            dtpDesde.Checked = dtpHasta.Checked = false;
            cboCriticidad.SelectedIndex = 0;
            cboModulo.SelectedIndex = 0;
            cboUsuario.SelectedIndex = 0;
        }

        /// <summary>Paso 6: al seleccionar un registro se muestra nombre y apellido del usuario del evento.</summary>
        private void dgvEventos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEventos.CurrentRow?.DataBoundItem is not RegistroBitacora_60MN evento)
            {
                lblDetalleUsuario.Text = string.Empty;
                return;
            }
            Usuario_60MN? usuario = evento.UsuarioId.HasValue ? usuarios.FirstOrDefault(u => u.Id == evento.UsuarioId.Value) : null;
            lblDetalleUsuario.Text = usuario != null
                ? $"Usuario del evento: {usuario.Nombre} {usuario.Apellido} ({usuario.NombreUsuario})"
                : $"Usuario del evento: {evento.Usuario} (sin registro de usuario asociado)";
        }

        // ------------------------- Exportar / importar XML -------------------------

        private const string FiltroArchivosXml = "Bitácora VetCare (*.xml)|*.xml";

        /// <summary>Exporta a XML los eventos de la última búsqueda (descripción cifrada, archivo firmado).</summary>
        private void btnExportarXml_Click(object sender, EventArgs e)
        {
            if (ultimoFiltro == null || dgvEventos.Rows.Count == 0)
            {
                Mensajes_60MN.Advertir("Busque primero los eventos a exportar: se exporta lo que muestra la grilla.");
                return;
            }
            using SaveFileDialog dialogo = new SaveFileDialog
            {
                Filter = FiltroArchivosXml,
                DefaultExt = "xml",
                FileName = $"Bitacora_VetCare_{DateTime.Now:yyyyMMdd_HHmmss}.xml"
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                ResumenArchivoBitacora_60MN resumen = bitacoraBLL.Exportar(ultimoFiltro, dialogo.FileName);
                Cursor = Cursors.Default;
                if (Mensajes_60MN.Confirmar($"Se exportaron {resumen.Cantidad} evento(s) a {resumen.NombreArchivo}.\n\n¿Desea ver el XML generado?"))
                    MostrarXml(resumen, modoImportacion: false);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Abre un XML de bitácora solo para verlo y comprobar sus firmas (no importa nada).</summary>
        private void btnVerXml_Click(object sender, EventArgs e)
        {
            string? ruta = ElegirArchivo();
            if (ruta == null) return;
            try
            {
                MostrarXml(bitacoraBLL.AnalizarArchivo(ruta), modoImportacion: false);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>
        /// Importar: vista previa con la validación completa y, si el usuario confirma,
        /// la BLL vuelve a leer y validar el archivo antes de insertar.
        /// </summary>
        private void btnImportarXml_Click(object sender, EventArgs e)
        {
            string? ruta = ElegirArchivo();
            if (ruta == null) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                ResumenArchivoBitacora_60MN resumen = bitacoraBLL.AnalizarArchivo(ruta);
                Cursor = Cursors.Default;
                if (!resumen.Valido)
                    bitacoraBLL.RegistrarRechazo(resumen);
                else if (resumen.Nuevos == 0)
                    Mensajes_60MN.Informar("Todos los eventos del archivo ya están en la bitácora: no hay nada para importar.");

                if (MostrarXml(resumen, modoImportacion: true) != DialogResult.OK)
                    return;

                Cursor = Cursors.WaitCursor;
                ResultadoImportacionBitacora_60MN resultado = bitacoraBLL.Importar(resumen.Ruta, resumen.HashSha256);
                Cursor = Cursors.Default;
                Mensajes_60MN.Informar($"Importación finalizada: {resultado.Importados} evento(s) agregado(s), " +
                                       $"{resultado.Duplicados} ya existente(s) omitido(s).");
                Buscar();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private string? ElegirArchivo()
        {
            using OpenFileDialog dialogo = new OpenFileDialog { Filter = FiltroArchivosXml, CheckFileExists = true };
            return dialogo.ShowDialog(this) == DialogResult.OK ? dialogo.FileName : null;
        }

        private DialogResult MostrarXml(ResumenArchivoBitacora_60MN resumen, bool modoImportacion)
        {
            using FrmVisorXmlBitacora_60MN visor = new FrmVisorXmlBitacora_60MN(resumen, modoImportacion);
            return visor.ShowDialog(this);
        }

        private void dgvEventos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvEventos.Rows)
                if (fila.DataBoundItem is RegistroBitacora_60MN evento)
                    fila.DefaultCellStyle.BackColor = evento.Criticidad switch
                    {
                        1 => Color.MistyRose,
                        2 => Color.LightYellow,
                        _ => Color.Empty
                    };
        }
    }
}
