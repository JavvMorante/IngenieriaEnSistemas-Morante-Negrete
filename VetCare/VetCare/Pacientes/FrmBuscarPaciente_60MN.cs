using BLL_60MN;
using DAL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// Selector de paciente reutilizable: busca mascotas por su nombre o especie, o por el
    /// documento o apellido del dueño, y devuelve la elegida en Seleccionada. Lo abren
    /// PACIENTES > Historias clínicas (CUN03) y PACIENTES > Registrar atención (CUN04).
    /// </summary>
    public partial class FrmBuscarPaciente_60MN : FormBase_60MN
    {
        private readonly ClienteBLL_60MN clienteBLL = new ClienteBLL_60MN();
        private readonly string claveProposito;
        private readonly string propositoPorDefecto;

        /// <summary>Mascota elegida (con DialogResult.OK).</summary>
        public Mascota_60MN? Seleccionada { get; private set; }

        public FrmBuscarPaciente_60MN() : this("msg.ElegirPaciente", "Busque y elija el paciente.") { }

        /// <param name="claveProposito">Clave de traducción del texto que explica para qué se elige el paciente.</param>
        public FrmBuscarPaciente_60MN(string claveProposito, string propositoPorDefecto)
        {
            InitializeComponent();
            dgvPacientes.AutoGenerateColumns = false;
            this.claveProposito = claveProposito;
            this.propositoPorDefecto = propositoPorDefecto;
            txtValor.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                Buscar();
            };
            dgvPacientes.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.Handled = true;
                Elegir();
            };
            dgvPacientes.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) Elegir(); };
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmBuscarPaciente_60MN_Load(object sender, EventArgs e) => CargarCriterios();

        protected override void AlCambiarIdioma()
        {
            int seleccionado = Math.Max(cboCriterio.SelectedIndex, 0);
            CargarCriterios();
            cboCriterio.SelectedIndex = seleccionado;
            lblProposito.Text = T(claveProposito, propositoPorDefecto);
        }

        private void CargarCriterios()
        {
            cboCriterio.Items.Clear();
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioMascota", "Nombre de mascota"), CriterioBusquedaMascota_60MN.NombreMascota));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioDocumento", "Documento"), CriterioBusquedaMascota_60MN.Documento));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioApellido", "Apellido"), CriterioBusquedaMascota_60MN.Apellido));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioEspecie", "Especie"), CriterioBusquedaMascota_60MN.Especie));
            cboCriterio.SelectedIndex = 0;
        }

        private CriterioBusquedaMascota_60MN Criterio =>
            OpcionCombo_60MN.ValorDe<CriterioBusquedaMascota_60MN>(cboCriterio) ?? CriterioBusquedaMascota_60MN.NombreMascota;

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        private void Buscar()
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text))
            {
                Mensajes_60MN.Advertir("Ingrese el valor a buscar.");
                txtValor.Focus();
                return;
            }
            try
            {
                List<Mascota_60MN> mascotas = clienteBLL.BuscarMascotas(Criterio, txtValor.Text, chkIncluirBajas.Checked);
                dgvPacientes.DataSource = mascotas;
                btnAceptar.Enabled = mascotas.Count > 0;
                if (mascotas.Count == 0)
                {
                    Mensajes_60MN.Informar(T("msg.SinPacientes", "No se encontraron pacientes con ese criterio."));
                    return;
                }
                dgvPacientes.Focus();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void dgvPacientes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvPacientes.Rows)
                if (fila.DataBoundItem is Mascota_60MN m && !m.Activo)
                    fila.DefaultCellStyle.BackColor = Color.Gainsboro;
        }

        private void btnAceptar_Click(object sender, EventArgs e) => Elegir();

        private void Elegir()
        {
            if (!(dgvPacientes.CurrentRow?.DataBoundItem is Mascota_60MN mascota))
            {
                Mensajes_60MN.Advertir("Seleccione un paciente.");
                return;
            }
            Seleccionada = mascota;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
