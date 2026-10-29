using System.Text;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN07 Generar Reporte de Atenciones (REPORTES > Reporte de atenciones): atenciones de un
    /// período filtradas por veterinario y especie, con los totales por profesional. Se imprime
    /// (o se guarda en PDF con "Microsoft Print to PDF") y se exporta a CSV.
    /// </summary>
    public partial class FrmReporteAtenciones_60MN : FormBase_60MN
    {
        private readonly ReporteAtencionesBLL_60MN reporteBLL = new ReporteAtencionesBLL_60MN();
        private ResultadoReporteAtenciones_60MN? resultado;
        private string criterios = string.Empty;

        public FrmReporteAtenciones_60MN()
        {
            InitializeComponent();
            dgvAtenciones.AutoGenerateColumns = false;
            dgvProfesionales.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        /// <summary>Paso 2: el período arranca en el mes en curso; filtros de veterinario y especie.</summary>
        private void FrmReporteAtenciones_60MN_Load(object sender, EventArgs e)
        {
            DateTime hoy = DateTime.Today;
            dtpHasta.Value = hoy;
            dtpDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
            try
            {
                cboProfesional.Items.Add(OpcionCombo_60MN.Todos);
                foreach (ProfesionalAtencion_60MN p in reporteBLL.ListarProfesionales())
                    cboProfesional.Items.Add(new OpcionCombo_60MN(p.Profesional, p.UsuarioId));
                cboProfesional.SelectedIndex = 0;

                cboEspecie.Items.Add(OpcionCombo_60MN.Todos);
                foreach (string especie in reporteBLL.ListarEspecies())
                    cboEspecie.Items.Add(new OpcionCombo_60MN(especie, especie));
                cboEspecie.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        protected override void AlCambiarIdioma()
        {
            if (resultado != null) MostrarResumen();
        }

        private string? EspecieElegida => (cboEspecie.SelectedItem as OpcionCombo_60MN)?.Valor as string;

        /// <summary>Paso 3: la BLL valida el rango y registra el evento en la bitácora.</summary>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                resultado = reporteBLL.Generar(dtpDesde.Value, dtpHasta.Value, OpcionCombo_60MN.ValorDe<int>(cboProfesional), EspecieElegida);
                criterios = $"{lblDesde.Text} {resultado.Desde:dd/MM/yyyy} {lblHasta.Text} {resultado.Hasta:dd/MM/yyyy} · " +
                            $"{lblProfesional.Text}: {cboProfesional.Text} · {lblEspecie.Text}: {cboEspecie.Text}";
                dgvAtenciones.DataSource = resultado.Atenciones;
                dgvProfesionales.DataSource = resultado.PorProfesional;
                btnImprimir.Enabled = btnExportar.Enabled = !resultado.SinDatos;
                MostrarResumen();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Paso 5: totales del período; 4.1: sin atenciones en el período.</summary>
        private void MostrarResumen()
        {
            if (resultado == null) return;
            lblResumen.Text = resultado.SinDatos
                ? T("msg.SinAtencionesPeriodo", "No se registraron atenciones en el período con los filtros indicados.")
                : string.Format(T("msg.ResumenAtenciones", "{0} atención(es) · {1} paciente(s) · {2} profesional(es)"),
                    resultado.Atenciones.Count, resultado.Pacientes, resultado.PorProfesional.Count);
        }

        /// <summary>Paso 6: impresión o PDF (eligiendo "Microsoft Print to PDF").</summary>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (resultado == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VETCARE - REPORTE DE ATENCIONES");
            sb.AppendLine(criterios);
            sb.AppendLine($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(new string('=', 96));
            sb.AppendLine($"{"Fecha",-17}{"HC",-9}{"Paciente",-14}{"Especie",-10}{"Propietario",-20}{"Profesional",-16}Motivo");
            sb.AppendLine(new string('-', 96));
            static string Corto(string texto, int largo) => texto.Length > largo - 1 ? texto.Substring(0, largo - 1) : texto;
            foreach (FilaReporteAtencion_60MN f in resultado.Atenciones)
                sb.AppendLine($"{f.FechaHora,-17:dd/MM/yyyy HH:mm}{f.NumeroHistoriaClinica,-9}{Corto(f.Paciente, 14),-14}{Corto(f.Especie, 10),-10}" +
                              $"{Corto(f.Propietario, 20),-20}{Corto(f.Profesional, 16),-16}{Corto(f.Motivo, 40)}");
            sb.AppendLine(new string('=', 96));
            sb.AppendLine("Atenciones por profesional:");
            foreach (TotalProfesional_60MN t in resultado.PorProfesional)
                sb.AppendLine($"  {Corto(t.Profesional, 40),-40}{t.Atenciones,6} atención(es){t.Pacientes,6} paciente(s){t.Porcentaje,8:0.0} %");
            sb.AppendLine();
            sb.AppendLine(lblResumen.Text);
            try
            {
                if (ImpresionTexto_60MN.Imprimir(this, "Reporte de atenciones", sb.ToString()) != null)
                    Mensajes_60MN.Informar("Reporte enviado a la impresora.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (resultado == null) return;
            using SaveFileDialog dialogo = new SaveFileDialog
            {
                FileName = $"Atenciones_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Filter = "CSV (*.csv)|*.csv",
                DefaultExt = "csv"
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;

            static string Campo(string valor) => "\"" + valor.Replace("\"", "\"\"") + "\"";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Fecha;HistoriaClinica;Paciente;Especie;Propietario;Profesional;Motivo");
            foreach (FilaReporteAtencion_60MN f in resultado.Atenciones)
                sb.AppendLine(string.Join(";", f.FechaHora.ToString("dd/MM/yyyy HH:mm"), Campo(f.NumeroHistoriaClinica), Campo(f.Paciente),
                    Campo(f.Especie), Campo(f.Propietario), Campo(f.Profesional), Campo(f.Motivo)));
            try
            {
                File.WriteAllText(dialogo.FileName, sb.ToString(), new UTF8Encoding(true));
                Mensajes_60MN.Informar("Reporte exportado correctamente.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
