using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN08 Registrar Atención. Una vez registrada se habilitan «extend» CUN09
    /// (medicación en carrito) y CUN10 (informe de atención).
    /// </summary>
    public partial class FrmAtencion_60MN : FormBase_60MN
    {
        private readonly AtencionBLL_60MN atencionBLL = new AtencionBLL_60MN();
        private readonly HistoriaClinica_60MN? historia;
        private Atencion_60MN? registrada;

        public FrmAtencion_60MN() : this(null) { }

        public FrmAtencion_60MN(HistoriaClinica_60MN? historia)
        {
            InitializeComponent();
            this.historia = historia;
        }

        private void FrmAtencion_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            if (historia == null) return;
            lblPaciente.Text = $"{historia.Numero} · {historia.Mascota.Nombre} ({historia.Mascota.Especie}, {historia.Mascota.Raza}) · " +
                               $"{historia.Propietario.NombreCompleto}";
            // Paso 3: profesional de la sesión activa.
            txtProfesional.Text = AtencionBLL_60MN.ProfesionalActual();
            nudPeso.Value = Math.Min(historia.PesoActual, nudPeso.Maximum);
        }

        /// <summary>CUN07 4.1: las alergias se destacan al registrar la atención (texto en el idioma actual).</summary>
        protected override void AlCambiarIdioma()
        {
            if (historia == null) return;
            lblAlergias.Text = historia.TieneAlergias
                ? $"⚠ {TraductorFormularios_60MN.T("msg.Alergias", "ALERGIAS")}: {historia.Alergias}"
                : historia.Alergias;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (historia == null) return;
            if (string.IsNullOrWhiteSpace(txtMotivo.Text) || string.IsNullOrWhiteSpace(txtDiagnostico.Text))
            {
                Mensajes_60MN.Advertir("Complete el motivo de consulta y el diagnóstico.");
                return;
            }
            try
            {
                // 4.1: el peso difiere significativamente del último registrado.
                if (atencionBLL.PesoDifiereSignificativamente(historia.Id, nudPeso.Value) &&
                    !Mensajes_60MN.Confirmar($"El peso ingresado ({nudPeso.Value:0.##} kg) difiere más de un " +
                        $"{AtencionBLL_60MN.DiferenciaPesoSignificativa:P0} del último registrado ({historia.PesoActual:0.##} kg). ¿Es correcto?"))
                {
                    nudPeso.Focus();
                    return;
                }

                registrada = atencionBLL.Registrar(new Atencion_60MN
                {
                    HistoriaClinicaId = historia.Id,
                    Peso = nudPeso.Value,
                    Temperatura = nudTemperatura.Value,
                    FrecuenciaCardiaca = (int)nudFrecuencia.Value,
                    Motivo = txtMotivo.Text,
                    Diagnostico = txtDiagnostico.Text
                });
                Mensajes_60MN.Informar($"Atención Nº {registrada.Id} registrada en la historia {historia.Numero}.");

                // La atención ya no se edita: se habilitan medicación e informe.
                grpSignos.Enabled = false;
                txtMotivo.ReadOnly = txtDiagnostico.ReadOnly = true;
                btnRegistrar.Enabled = false;
                btnMedicacion.Enabled = btnInforme.Enabled = true;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnMedicacion_Click(object sender, EventArgs e)
        {
            if (registrada == null) return;
            int atencionId = registrada.Id;
            VentanasMdi_60MN.Abrir(this, () => new FrmCarrito_60MN(atencionId), $"carrito:{atencionId}");
        }

        private void btnInforme_Click(object sender, EventArgs e)
        {
            if (registrada == null) return;
            int atencionId = registrada.Id;
            VentanasMdi_60MN.Abrir(this, () => new FrmInformeAtencion_60MN(atencionId), $"informe:{atencionId}");
        }
    }
}
