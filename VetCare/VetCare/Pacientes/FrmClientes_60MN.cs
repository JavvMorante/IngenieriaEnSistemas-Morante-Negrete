using BLL_60MN;
using DAL_60MN;
using Entidades_60MN;
using Servicios_60MN.Composite;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>
    /// Gestión de Pacientes (MAESTROS > Clientes). CUN03 Consultar Cliente y punto de
    /// entrada a CUN01/CUN02 (cliente), CUN04 (mascotas), CUN05 a CUN07 (historia
    /// clínica) y CUN08 (nueva atención) sobre la mascota seleccionada.
    /// </summary>
    public partial class FrmClientes_60MN : FormBase_60MN
    {
        private readonly ClienteBLL_60MN clienteBLL = new ClienteBLL_60MN();
        private Cliente_60MN? clienteActual;

        public FrmClientes_60MN()
        {
            InitializeComponent();
            dgvClientes.AutoGenerateColumns = false;
            dgvMascotas.AutoGenerateColumns = false;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmClientes_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            CargarCriterios();
            MostrarCliente(null);
        }

        protected override void AlCambiarIdioma()
        {
            int seleccionado = Math.Max(cboCriterio.SelectedIndex, 0);
            CargarCriterios();
            cboCriterio.SelectedIndex = seleccionado;
            MostrarCliente(clienteActual);
        }

        /// <summary>CUN03 paso 2: tipo y número de documento, apellido o nombre de la mascota.</summary>
        private void CargarCriterios()
        {
            cboCriterio.Items.Clear();
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioDocumento", "Documento"), CriterioBusquedaCliente_60MN.Documento));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioApellido", "Apellido"), CriterioBusquedaCliente_60MN.Apellido));
            cboCriterio.Items.Add(new OpcionCombo_60MN(T("msg.CriterioMascota", "Nombre de mascota"), CriterioBusquedaCliente_60MN.NombreMascota));
            cboCriterio.SelectedIndex = 0;
        }

        private CriterioBusquedaCliente_60MN Criterio =>
            OpcionCombo_60MN.ValorDe<CriterioBusquedaCliente_60MN>(cboCriterio) ?? CriterioBusquedaCliente_60MN.Documento;

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar(null);

        public void Buscar(int? seleccionarId)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text))
            {
                Mensajes_60MN.Advertir("Ingrese el valor a buscar.");
                txtValor.Focus();
                return;
            }
            try
            {
                List<Cliente_60MN> clientes = clienteBLL.Buscar(Criterio, txtValor.Text, chkIncluirBajas.Checked);
                dgvClientes.DataSource = clientes;
                if (clientes.Count == 0)
                {
                    MostrarCliente(null);
                    // 4.1: sin coincidencias, se ofrece dar de alta al cliente (CUN01).
                    if (btnNuevoCliente.Visible && Mensajes_60MN.Confirmar("No se encontraron coincidencias. ¿Desea registrar un cliente nuevo?"))
                        btnNuevoCliente.PerformClick();
                    return;
                }
                foreach (DataGridViewRow fila in dgvClientes.Rows)
                    if (fila.DataBoundItem is Cliente_60MN c && c.Id == seleccionarId)
                        dgvClientes.CurrentCell = fila.Cells[0];
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow?.DataBoundItem is Cliente_60MN seleccionado && seleccionado.Id != clienteActual?.Id)
                CargarFicha(seleccionado.Id);
        }

        /// <summary>CUN03 paso 6: ficha del cliente y listado de sus mascotas.</summary>
        private void CargarFicha(int clienteId)
        {
            try
            {
                MostrarCliente(clienteBLL.ObtenerCliente(clienteId));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void MostrarCliente(Cliente_60MN? cliente)
        {
            clienteActual = cliente;
            bool hay = cliente != null;
            btnModificarCliente.Enabled = hay && cliente!.Activo;
            btnBajaCliente.Enabled = hay && cliente!.Activo;
            btnReactivarCliente.Enabled = hay && !cliente!.Activo;
            btnAnadirMascota.Enabled = hay && cliente!.Activo;
            dgvMascotas.DataSource = hay ? cliente!.Mascotas.ToList() : null;

            if (!hay)
            {
                lblFicha.ForeColor = SystemColors.GrayText;
                lblFicha.Text = T("msg.SeleccioneCliente", "Busque y seleccione un cliente para ver su ficha.");
                return;
            }
            lblFicha.ForeColor = cliente!.Activo ? SystemColors.ControlText : Color.Firebrick;
            lblFicha.Text =
                $"{cliente.NombreCompleto}\n{cliente.Documento}\n\n" +
                $"{T("msg.Direccion", "Dirección")}: {cliente.Direccion}\n" +
                $"{T("msg.Telefono", "Teléfono")}: {cliente.Telefono}\n" +
                $"Email: {cliente.Email ?? "-"}\n\n" +
                // 5.1: cliente dado de baja; 6.1: sin mascotas.
                (!cliente.Activo ? T("msg.ClienteDeBaja", "CLIENTE DADO DE BAJA: puede reactivarse.") + "\n" : string.Empty) +
                (cliente.Mascotas.Count == 0 ? T("msg.SinMascotas", "El cliente no tiene mascotas: use \"Añadir mascota\".") : string.Empty);
        }

        private void dgvClientes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvClientes.Rows)
                if (fila.DataBoundItem is Cliente_60MN c && !c.Activo)
                    fila.DefaultCellStyle.BackColor = Color.Gainsboro;
        }

        private void dgvMascotas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvMascotas.Rows)
                if (fila.DataBoundItem is Mascota_60MN m && !m.Activo)
                    fila.DefaultCellStyle.BackColor = Color.Gainsboro;
        }

        private void RefrescarFicha()
        {
            if (clienteActual != null)
                CargarFicha(clienteActual.Id);
        }

        // ------------------------------- Cliente -------------------------------

        // CUN01
        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            using FrmCliente_60MN formulario = new FrmCliente_60MN(null,
                Criterio == CriterioBusquedaCliente_60MN.Documento ? txtValor.Text.Trim() : string.Empty);
            if (formulario.ShowDialog(this) != DialogResult.OK) return;
            // El cliente nuevo (o el existente, si el documento estaba registrado: flujo 5.1) queda seleccionado.
            Cliente_60MN? cliente = clienteBLL.ObtenerCliente(formulario.ClienteId);
            if (cliente == null) return;
            cboCriterio.SelectedIndex = 0;
            txtValor.Text = cliente.NroDocumento;
            chkIncluirBajas.Checked = !cliente.Activo;
            Buscar(cliente.Id);
            MostrarCliente(cliente);
        }

        // CUN02
        private void btnModificarCliente_Click(object sender, EventArgs e)
        {
            if (clienteActual == null) return;
            using FrmCliente_60MN formulario = new FrmCliente_60MN(clienteActual, string.Empty);
            if (formulario.ShowDialog(this) == DialogResult.OK)
            {
                Buscar(clienteActual.Id);
                RefrescarFicha();
            }
        }

        private void btnBajaCliente_Click(object sender, EventArgs e) => CambiarEstadoCliente(false);

        private void btnReactivarCliente_Click(object sender, EventArgs e) => CambiarEstadoCliente(true);

        private void CambiarEstadoCliente(bool activo)
        {
            if (clienteActual == null) return;
            string pregunta = activo
                ? $"¿Reactivar al cliente {clienteActual.NombreCompleto}?"
                : $"¿Dar de baja al cliente {clienteActual.NombreCompleto}? Sus mascotas e historias clínicas se conservan.";
            if (!Mensajes_60MN.Confirmar(pregunta)) return;
            try
            {
                clienteBLL.CambiarEstadoCliente(clienteActual.Id, activo);
                int id = clienteActual.Id;
                if (!activo) chkIncluirBajas.Checked = true;
                Buscar(id);
                CargarFicha(id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // ------------------------------- Mascotas (CUN04) -------------------------------

        private Mascota_60MN? MascotaSeleccionada()
        {
            if (dgvMascotas.CurrentRow?.DataBoundItem is Mascota_60MN mascota)
                return mascota;
            Mensajes_60MN.Advertir("Seleccione una mascota.");
            return null;
        }

        private void btnAnadirMascota_Click(object sender, EventArgs e)
        {
            if (clienteActual == null) return;
            using FrmMascotas_60MN formulario = new FrmMascotas_60MN(clienteActual.Id, null, persistir: true);
            if (formulario.ShowDialog(this) == DialogResult.OK)
                RefrescarFicha();
        }

        private void btnModificarMascota_Click(object sender, EventArgs e)
        {
            Mascota_60MN? mascota = MascotaSeleccionada();
            if (mascota == null) return;
            if (!mascota.Activo)
            {
                Mensajes_60MN.Advertir("La mascota está dada de baja.");
                return;
            }
            using FrmMascotas_60MN formulario = new FrmMascotas_60MN(mascota.ClienteId, mascota, persistir: true);
            if (formulario.ShowDialog(this) == DialogResult.OK)
                RefrescarFicha();
        }

        private void btnBajaMascota_Click(object sender, EventArgs e)
        {
            Mascota_60MN? mascota = MascotaSeleccionada();
            if (mascota == null) return;
            bool activar = !mascota.Activo;
            // 3.1: baja lógica, el historial de atenciones se conserva.
            string pregunta = activar
                ? $"¿Reactivar a {mascota.Nombre}?"
                : $"¿Dar de baja a {mascota.Nombre}?" + (mascota.TieneHistoriaClinica ? $" Su historia clínica {mascota.NumeroHistoriaClinica} se conserva íntegra." : string.Empty);
            if (!Mensajes_60MN.Confirmar(pregunta)) return;
            try
            {
                clienteBLL.CambiarEstadoMascota(mascota.Id, activar);
                RefrescarFicha();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // ------------------------------- Historia clínica y atención -------------------------------

        private void dgvMascotas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnHistoriaClinica.Visible)
                btnHistoriaClinica.PerformClick();
        }

        // CUN05 / CUN06
        private void btnHistoriaClinica_Click(object sender, EventArgs e)
        {
            Mascota_60MN? mascota = MascotaSeleccionada();
            if (mascota == null) return;
            if (!mascota.TieneHistoriaClinica && !PuedeAbrirHistoria(mascota, "La mascota no tiene historia clínica. ¿Desea abrirla ahora?"))
                return;
            AbrirHistoria(mascota);
        }

        // CUN08
        private void btnNuevaAtencion_Click(object sender, EventArgs e)
        {
            Mascota_60MN? mascota = MascotaSeleccionada();
            if (mascota == null) return;
            if (!mascota.TieneHistoriaClinica)
            {
                // CUN08 1.1: sin historia clínica se deriva a CUN05.
                if (PuedeAbrirHistoria(mascota, "La mascota no posee historia clínica. Debe crearse antes de registrar la atención. ¿Abrirla ahora?"))
                    AbrirHistoria(mascota);
                return;
            }
            try
            {
                HistoriaClinica_60MN? hc = new HistoriaClinicaBLL_60MN().Consultar(mascota.Id);
                if (hc == null) return;
                using FrmAtencion_60MN formulario = new FrmAtencion_60MN(hc);
                formulario.ShowDialog(this);
                RefrescarFicha();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private bool PuedeAbrirHistoria(Mascota_60MN mascota, string pregunta)
        {
            if (!SessionManager_60MN.Instancia.TienePermiso(CodigosPatente_60MN.HistoriaClinicaAlta))
            {
                Mensajes_60MN.Advertir("La mascota no tiene historia clínica y usted no tiene permiso para crearla.");
                return false;
            }
            if (!mascota.Activo)
            {
                Mensajes_60MN.Advertir("La mascota está dada de baja.");
                return false;
            }
            return Mensajes_60MN.Confirmar(pregunta);
        }

        private void AbrirHistoria(Mascota_60MN mascota)
        {
            using FrmHistoriaClinica_60MN formulario = new FrmHistoriaClinica_60MN(mascota.Id);
            formulario.ShowDialog(this);
            RefrescarFicha();
        }
    }
}
