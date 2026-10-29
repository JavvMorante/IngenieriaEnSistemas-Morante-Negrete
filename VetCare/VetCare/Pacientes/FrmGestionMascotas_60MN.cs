using BLL_60MN;
using DAL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN02 Gestionar Mascotas (MAESTROS > Mascotas), «extend» de CUN01: busca mascotas por
    /// su nombre o especie o por el documento o apellido del dueño, y permite añadir (eligiendo
    /// antes el cliente), modificar, dar de baja y reactivar. Las mismas operaciones siguen
    /// disponibles desde la ficha del cliente (FrmClientes_60MN).
    /// </summary>
    public partial class FrmGestionMascotas_60MN : FormBase_60MN
    {
        private readonly ClienteBLL_60MN clienteBLL = new ClienteBLL_60MN();

        // Última búsqueda realizada: se repite después de cada operación para refrescar la grilla.
        private CriterioBusquedaMascota_60MN? ultimoCriterio;
        private string ultimoValor = string.Empty;

        public FrmGestionMascotas_60MN()
        {
            InitializeComponent();
            dgvMascotas.AutoGenerateColumns = false;
            txtValor.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                btnBuscar.PerformClick();
            };
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmGestionMascotas_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            CargarCriterios();
            ActualizarBotones();
        }

        protected override void AlCambiarIdioma()
        {
            int seleccionado = Math.Max(cboCriterio.SelectedIndex, 0);
            CargarCriterios();
            cboCriterio.SelectedIndex = seleccionado;
        }

        private void CargarCriterios()
        {
            cboCriterio.Items.Clear();
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioMascota", "Nombre de mascota"), CriterioBusquedaMascota_60MN.NombreMascota));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioEspecie", "Especie"), CriterioBusquedaMascota_60MN.Especie));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioDocumento", "Documento"), CriterioBusquedaMascota_60MN.Documento));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioApellido", "Apellido"), CriterioBusquedaMascota_60MN.Apellido));
            cboCriterio.SelectedIndex = 0;
        }

        private CriterioBusquedaMascota_60MN Criterio =>
            OpcionCombo_60MN.ValorDe<CriterioBusquedaMascota_60MN>(cboCriterio) ?? CriterioBusquedaMascota_60MN.NombreMascota;

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text))
            {
                Mensajes_60MN.Advertir("Ingrese el valor a buscar.");
                txtValor.Focus();
                return;
            }
            ultimoCriterio = Criterio;
            ultimoValor = txtValor.Text;
            Buscar(null, avisarSinResultados: true);
        }

        private void chkIncluirBajas_CheckedChanged(object sender, EventArgs e)
        {
            if (ultimoCriterio != null)
                Buscar(MascotaActual?.Id, avisarSinResultados: false);
        }

        private void Buscar(int? seleccionarId, bool avisarSinResultados)
        {
            if (ultimoCriterio == null) return;
            try
            {
                List<Mascota_60MN> mascotas = clienteBLL.BuscarMascotas(ultimoCriterio.Value, ultimoValor, chkIncluirBajas.Checked);
                dgvMascotas.DataSource = mascotas;
                lblResultados.Text = string.Format(T("msg.MascotasEncontradas", "{0} mascota(s) encontrada(s)."), mascotas.Count);
                foreach (DataGridViewRow fila in dgvMascotas.Rows)
                    if (fila.DataBoundItem is Mascota_60MN m && m.Id == seleccionarId)
                        dgvMascotas.CurrentCell = fila.Cells[0];
                if (mascotas.Count == 0 && avisarSinResultados)
                    Mensajes_60MN.Informar(T("msg.SinMascotasBusqueda", "No se encontraron mascotas. Para registrar una, use \"Añadir\"."));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            ActualizarBotones();
        }

        private Mascota_60MN? MascotaActual => dgvMascotas.CurrentRow?.DataBoundItem as Mascota_60MN;

        private void dgvMascotas_SelectionChanged(object sender, EventArgs e) => ActualizarBotones();

        private void ActualizarBotones()
        {
            Mascota_60MN? mascota = MascotaActual;
            btnModificar.Enabled = mascota != null && mascota.Activo;
            btnBaja.Enabled = mascota != null && mascota.Activo;
            btnReactivar.Enabled = mascota != null && !mascota.Activo;
        }

        private void dgvMascotas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvMascotas.Rows)
                if (fila.DataBoundItem is Mascota_60MN m && !m.Activo)
                    fila.DefaultCellStyle.BackColor = Color.Gainsboro;
        }

        private void dgvMascotas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnModificar.Visible && btnModificar.Enabled)
                btnModificar.PerformClick();
        }

        /// <summary>Paso 1: una mascota no puede existir sin cliente, así que primero se elige el dueño.</summary>
        private void btnAnadir_Click(object sender, EventArgs e)
        {
            Cliente_60MN? cliente;
            using (FrmBuscarCliente_60MN selector = new FrmBuscarCliente_60MN())
            {
                if (selector.ShowDialog(this) != DialogResult.OK) return;
                cliente = selector.Seleccionado;
            }
            if (cliente == null) return;

            using FrmMascotas_60MN formulario = new FrmMascotas_60MN(cliente.Id, null, persistir: true);
            if (formulario.ShowDialog(this) != DialogResult.OK || formulario.Resultado == null) return;

            // La mascota nueva queda a la vista y seleccionada.
            Mascota_60MN nueva = formulario.Resultado;
            cboCriterio.SelectedIndex = 0;
            txtValor.Text = nueva.Nombre;
            ultimoCriterio = CriterioBusquedaMascota_60MN.NombreMascota;
            ultimoValor = nueva.Nombre;
            Buscar(nueva.Id, avisarSinResultados: false);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Mascota_60MN? mascota = MascotaActual;
            if (mascota == null) return;
            if (!mascota.Activo)
            {
                Mensajes_60MN.Advertir("La mascota está dada de baja.");
                return;
            }
            using FrmMascotas_60MN formulario = new FrmMascotas_60MN(mascota.ClienteId, mascota, persistir: true);
            if (formulario.ShowDialog(this) == DialogResult.OK)
                Buscar(mascota.Id, avisarSinResultados: false);
        }

        private void btnBaja_Click(object sender, EventArgs e) => CambiarEstado(false);

        private void btnReactivar_Click(object sender, EventArgs e) => CambiarEstado(true);

        /// <summary>Flujo 3.1: baja lógica; la historia clínica y sus atenciones se conservan.</summary>
        private void CambiarEstado(bool activar)
        {
            Mascota_60MN? mascota = MascotaActual;
            if (mascota == null) return;
            string pregunta = activar
                ? $"¿Reactivar a {mascota.Nombre} ({mascota.Propietario})?"
                : $"¿Dar de baja a {mascota.Nombre} ({mascota.Propietario})?" +
                  (mascota.TieneHistoriaClinica ? $" Su historia clínica {mascota.NumeroHistoriaClinica} se conserva íntegra." : string.Empty);
            if (!Mensajes_60MN.Confirmar(pregunta)) return;
            try
            {
                clienteBLL.CambiarEstadoMascota(mascota.Id, activar);
                // La mascota dada de baja sigue a la vista para poder reactivarla.
                if (!activar && !chkIncluirBajas.Checked)
                    chkIncluirBajas.Checked = true;
                else
                    Buscar(mascota.Id, avisarSinResultados: false);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
