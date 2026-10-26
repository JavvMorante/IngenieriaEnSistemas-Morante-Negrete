using System.Text;
using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Composite;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>
    /// Historia clínica de una mascota: CUN05 Crear (si todavía no tiene), CUN06
    /// Consultar (cabecera, atenciones y su detalle) y CUN07 Actualizar (peso, alergias
    /// y observaciones, con el historial de valores anteriores).
    /// </summary>
    public partial class FrmHistoriaClinica_60MN : FormBase_60MN
    {
        private readonly HistoriaClinicaBLL_60MN historiaBLL = new HistoriaClinicaBLL_60MN();
        private readonly int mascotaId;
        private HistoriaClinica_60MN? historia;
        private Mascota_60MN? mascota;
        private Cliente_60MN? propietario;
        private bool editando;

        public FrmHistoriaClinica_60MN() : this(0) { }

        public FrmHistoriaClinica_60MN(int mascotaId)
        {
            InitializeComponent();
            this.mascotaId = mascotaId;
            dgvAtenciones.AutoGenerateColumns = false;
            dgvEvolucion.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private bool EsAlta => historia == null;

        private void FrmHistoriaClinica_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            dtpNacimiento.MaxDate = DateTime.Today;
            Cargar();
        }

        private void Cargar()
        {
            try
            {
                if (historiaBLL.TieneHistoriaClinica(mascotaId))
                {
                    // CUN06 paso 3: cabecera del legajo (el acceso queda en la bitácora).
                    historia = historiaBLL.Consultar(mascotaId);
                    MostrarCabecera(historia!.Mascota, historia.Propietario);
                    MostrarDatos(historia);
                    CargarAtenciones();
                    dgvEvolucion.DataSource = historiaBLL.ListarHistorial(historia.Id);
                    ModoEdicion(false);
                }
                else
                {
                    // CUN05: la mascota no tiene historia, se abre el formulario de alta.
                    var (m, propietario) = historiaBLL.DatosParaAlta(mascotaId);
                    MostrarCabecera(m, propietario);
                    nudPeso.Value = 0;
                    chkSinAlergias.Checked = true;
                    ModoEdicion(true);
                }
                AlCambiarIdioma();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
                BeginInvoke(Close);
            }
        }

        protected override void AlCambiarIdioma()
        {
            if (mascota == null) return;
            Text = EsAlta
                ? T("FrmHistoriaClinica_60MN.TituloAlta", "Abrir historia clínica")
                : T("FrmHistoriaClinica_60MN", "Historia clínica");
            btnGuardar.Text = EsAlta
                ? T("FrmHistoriaClinica_60MN.btnAbrir", "Abrir historia clínica")
                : T("FrmHistoriaClinica_60MN.btnGuardar", "Guardar");
            lblNumero.Text = EsAlta
                ? T("msg.HistoriaNueva", "Historia clínica nueva")
                : $"{historia!.Numero} · {T("msg.Abierta", "abierta el")} {historia.FechaApertura:dd/MM/yyyy}";
            if (propietario != null)
                lblPropietario.Text = $"{T("msg.Propietario", "Propietario")}: {propietario.NombreCompleto} · {propietario.Documento} · {propietario.Telefono}";
            MostrarAlertaAlergias();

            // Detalle de la atención seleccionada (o el aviso de que no hay atenciones), en el idioma actual.
            if (!EsAlta)
            {
                if (dgvAtenciones.Rows.Count == 0)
                    txtDetalle.Text = T("msg.SinAtenciones", "No hay atenciones registradas.");
                else
                    dgvAtenciones_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void MostrarCabecera(Mascota_60MN m, Cliente_60MN propietario)
        {
            mascota = m;
            this.propietario = propietario;
            lblPaciente.Text = $"{m.Nombre} ({m.SexoTexto})";
            txtEspecie.Text = m.Especie;
            txtRaza.Text = m.Raza;
            dtpNacimiento.Value = m.FechaNacimiento > DateTime.Today ? DateTime.Today : m.FechaNacimiento;
            lblPropietario.Text = $"{T("msg.Propietario", "Propietario")}: {propietario.NombreCompleto} · {propietario.Documento} · {propietario.Telefono}";
        }

        private void MostrarDatos(HistoriaClinica_60MN hc)
        {
            nudPeso.Value = Math.Min(hc.PesoActual, nudPeso.Maximum);
            chkSinAlergias.Checked = !hc.TieneAlergias;
            txtAlergias.Text = hc.TieneAlergias ? hc.Alergias : string.Empty;
            txtObservaciones.Text = hc.Observaciones ?? string.Empty;
        }

        /// <summary>CUN07 4.1: una alergia registrada se destaca en la cabecera para las atenciones siguientes.</summary>
        private void MostrarAlertaAlergias()
        {
            lblAlertaAlergias.Text = historia != null && historia.TieneAlergias
                ? $"⚠ {T("msg.Alergias", "ALERGIAS")}: {historia.Alergias}"
                : string.Empty;
        }

        private void ModoEdicion(bool activo)
        {
            editando = activo;
            nudPeso.Enabled = activo;
            chkSinAlergias.Enabled = activo;
            txtAlergias.ReadOnly = !activo || chkSinAlergias.Checked;
            txtObservaciones.ReadOnly = !activo;
            btnGuardar.Visible = activo;
            btnCancelarEdicion.Visible = activo && !EsAlta;
            btnModificar.Enabled = !activo && !EsAlta;
            btnNuevaAtencion.Enabled = !activo && !EsAlta;
            btnImprimir.Enabled = !activo && !EsAlta;
            tabHistoria.Enabled = !EsAlta;

            // CUN07 3.1: especie, raza y fecha de nacimiento solo con permiso de administrador.
            bool datosPaciente = activo && !EsAlta && SessionManager_60MN.Instancia.TienePermiso(CodigosPatente_60MN.Administrador);
            txtEspecie.ReadOnly = !datosPaciente;
            txtRaza.ReadOnly = !datosPaciente;
            dtpNacimiento.Enabled = datosPaciente;
        }

        private void chkSinAlergias_CheckedChanged(object sender, EventArgs e)
        {
            // CUN05 5.2: si no se conocen alergias se marca "Sin alergias registradas".
            txtAlergias.ReadOnly = !editando || chkSinAlergias.Checked;
            if (chkSinAlergias.Checked) txtAlergias.Clear();
        }

        // CUN07 paso 2
        private void btnModificar_Click(object sender, EventArgs e) => ModoEdicion(true);

        private void btnCancelarEdicion_Click(object sender, EventArgs e)
        {
            // CUN07 5.2: se descartan los cambios.
            if (historia != null)
            {
                MostrarCabecera(historia.Mascota, historia.Propietario);
                MostrarDatos(historia);
            }
            ModoEdicion(false);
        }

        private string AlergiasIngresadas() => chkSinAlergias.Checked ? HistoriaClinica_60MN.SinAlergias : txtAlergias.Text;

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (nudPeso.Value <= 0)
            {
                Mensajes_60MN.Advertir("Ingrese el peso actual del paciente.");
                return;
            }
            if (!chkSinAlergias.Checked && string.IsNullOrWhiteSpace(txtAlergias.Text))
            {
                Mensajes_60MN.Advertir($"Indique las alergias o marque \"{HistoriaClinica_60MN.SinAlergias}\".");
                return;
            }
            try
            {
                if (EsAlta)
                {
                    HistoriaClinica_60MN creada = historiaBLL.Crear(mascotaId, nudPeso.Value, AlergiasIngresadas(), txtObservaciones.Text);
                    Mensajes_60MN.Informar($"Historia clínica {creada.Numero} creada correctamente.");
                }
                else
                {
                    bool alergiaNueva = !chkSinAlergias.Checked && txtAlergias.Text.Trim() != historia!.Alergias;
                    HistoriaClinica_60MN modificada = new HistoriaClinica_60MN
                    {
                        Id = historia!.Id,
                        PesoActual = nudPeso.Value,
                        Alergias = AlergiasIngresadas(),
                        Observaciones = txtObservaciones.Text
                    };
                    Mascota_60MN datosPaciente = new Mascota_60MN
                    {
                        Especie = txtEspecie.Text,
                        Raza = txtRaza.Text,
                        FechaNacimiento = dtpNacimiento.Value.Date
                    };
                    historiaBLL.Actualizar(modificada, datosPaciente);
                    Mensajes_60MN.Informar("Historia clínica actualizada correctamente." +
                        (alergiaNueva ? "\n\nLa alergia registrada se destacará en las próximas atenciones." : string.Empty));
                }
                Cargar();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // ------------------------------- Atenciones -------------------------------

        private void CargarAtenciones(int? seleccionarId = null)
        {
            List<Atencion_60MN> atenciones = historiaBLL.ListarAtenciones(historia!.Id);
            dgvAtenciones.DataSource = atenciones;
            if (atenciones.Count == 0)
            {
                // CUN06 4.1: sin atenciones se muestra solo la cabecera.
                txtDetalle.Text = T("msg.SinAtenciones", "No hay atenciones registradas.");
                btnVerInforme.Enabled = btnMedicacion.Enabled = false;
                return;
            }
            foreach (DataGridViewRow fila in dgvAtenciones.Rows)
                if (fila.DataBoundItem is Atencion_60MN a && a.Id == seleccionarId)
                    dgvAtenciones.CurrentCell = fila.Cells[0];
        }

        private Atencion_60MN? AtencionSeleccionada => dgvAtenciones.CurrentRow?.DataBoundItem as Atencion_60MN;

        /// <summary>CUN06 paso 6: detalle de la atención seleccionada.</summary>
        private void dgvAtenciones_SelectionChanged(object sender, EventArgs e)
        {
            Atencion_60MN? a = AtencionSeleccionada;
            btnVerInforme.Enabled = btnMedicacion.Enabled = a != null;
            if (a == null) return;
            txtDetalle.Text = DetalleAtencion(a).Replace("\n", Environment.NewLine);
        }

        private static string DetalleAtencion(Atencion_60MN a)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{a.FechaHora:dd/MM/yyyy HH:mm} · {a.Profesional}\n");
            sb.Append($"{T("msg.Peso", "Peso")} {a.Peso:0.##} kg · {T("msg.Temperatura", "Temp.")} {a.Temperatura:0.0} °C · " +
                      $"{T("msg.FrecuenciaCardiaca", "FC")} {a.FrecuenciaCardiaca} lpm\n\n");
            sb.Append($"{T("msg.Motivo", "Motivo")}: {a.Motivo}\n\n{T("msg.Diagnostico", "Diagnóstico")}: {a.Diagnostico}\n\n" +
                      $"{T("msg.Medicacion", "Medicación")}:\n");
            if (a.Medicacion.Count == 0)
                sb.Append($"  {T("msg.SinMedicacion", "(sin medicación)")}\n");
            foreach (ItemCarrito_60MN item in a.Medicacion)
                sb.Append($"  {item.Cantidad} x {item.Producto} ({T("msg.Lote", "lote")} {item.Lote})\n");
            return sb.ToString();
        }

        private void btnNuevaAtencion_Click(object sender, EventArgs e)
        {
            if (historia == null) return;
            // Al cerrar la atención se recarga la historia (nuevas atenciones, peso actualizado).
            VentanasMdi_60MN.Abrir(this, () => new FrmAtencion_60MN(historia), $"atencion:{historia.Id}", Cargar);
        }

        private void btnVerInforme_Click(object sender, EventArgs e)
        {
            if (AtencionSeleccionada is not Atencion_60MN a) return;
            VentanasMdi_60MN.Abrir(this, () => new FrmInformeAtencion_60MN(a.Id), $"informe:{a.Id}");
        }

        private void btnMedicacion_Click(object sender, EventArgs e)
        {
            if (AtencionSeleccionada is not Atencion_60MN a) return;
            VentanasMdi_60MN.Abrir(this, () => new FrmCarrito_60MN(a.Id), $"carrito:{a.Id}", () => CargarAtenciones(a.Id));
        }

        /// <summary>CUN06 6.1: impresión (o PDF) de la historia clínica completa.</summary>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (historia == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"VETCARE - HISTORIA CLÍNICA {historia.Numero}");
            sb.AppendLine(new string('=', 70));
            sb.AppendLine($"Paciente:    {historia.Mascota.Nombre} - {historia.Mascota.Especie}, {historia.Mascota.Raza}, {historia.Mascota.SexoTexto}");
            sb.AppendLine($"Nacimiento:  {historia.Mascota.FechaNacimiento:dd/MM/yyyy}");
            sb.AppendLine($"Propietario: {historia.Propietario.NombreCompleto} - {historia.Propietario.Documento} - {historia.Propietario.Telefono}");
            sb.AppendLine($"Peso actual: {historia.PesoActual:0.##} kg");
            sb.AppendLine($"Alergias:    {historia.Alergias}");
            sb.AppendLine($"Observaciones: {historia.Observaciones ?? "-"}");
            sb.AppendLine(new string('=', 70));
            foreach (Atencion_60MN a in (dgvAtenciones.DataSource as List<Atencion_60MN>) ?? new List<Atencion_60MN>())
            {
                sb.AppendLine(DetalleAtencion(a).TrimEnd());
                sb.AppendLine(new string('-', 70));
            }
            try
            {
                if (ImpresionTexto_60MN.Imprimir(this, "Historia clínica " + historia.Numero, sb.ToString()) != null)
                    Mensajes_60MN.Informar("Historia clínica enviada a la impresora.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
