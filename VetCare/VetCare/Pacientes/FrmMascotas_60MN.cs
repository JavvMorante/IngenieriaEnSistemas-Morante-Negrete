using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN04 Gestionar Mascota: alta o modificación de una mascota del cliente. Desde
    /// CUN01 se usa sin guardar (persistir = false): devuelve la mascota en Resultado y
    /// el alta se hace junto con la del cliente.
    /// </summary>
    public partial class FrmMascotas_60MN : FormBase_60MN
    {
        private readonly ClienteBLL_60MN clienteBLL = new ClienteBLL_60MN();
        private readonly int clienteId;
        private readonly Mascota_60MN? original;
        private readonly bool persistir;

        public Mascota_60MN? Resultado { get; private set; }

        public FrmMascotas_60MN() : this(0, null, false) { }

        public FrmMascotas_60MN(int clienteId, Mascota_60MN? mascota, bool persistir)
        {
            InitializeComponent();
            this.clienteId = clienteId;
            original = mascota;
            this.persistir = persistir;
        }

        private bool EsAlta => original == null;

        private void FrmMascotas_60MN_Load(object sender, EventArgs e)
        {
            // CUN04 5.2: no se admite una fecha de nacimiento futura.
            dtpNacimiento.MaxDate = DateTime.Today;
            dtpNacimiento.Value = DateTime.Today;
            if (EsAlta) return;

            txtNombre.Text = original!.Nombre;
            cboEspecie.Text = original.Especie;
            txtRaza.Text = original.Raza;
            rdoHembra.Checked = original.Sexo == "H";
            rdoMacho.Checked = original.Sexo != "H";
            dtpNacimiento.Value = original.FechaNacimiento > DateTime.Today ? DateTime.Today : original.FechaNacimiento;
        }

        protected override void AlCambiarIdioma()
        {
            Text = EsAlta
                ? TraductorFormularios_60MN.T("FrmMascotas_60MN.TituloAlta", "Añadir mascota")
                : TraductorFormularios_60MN.T("FrmMascotas_60MN.TituloModificacion", "Modificar mascota");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool valido = true;
            foreach (Control campo in new Control[] { txtNombre, cboEspecie, txtRaza })
                if (string.IsNullOrWhiteSpace(campo.Text))
                {
                    errorProvider.SetError(campo, "Campo obligatorio.");
                    valido = false;
                }
            if (!valido) return;

            Mascota_60MN mascota = new Mascota_60MN
            {
                Id = original?.Id ?? 0,
                ClienteId = clienteId,
                Nombre = txtNombre.Text.Trim(),
                Especie = cboEspecie.Text.Trim(),
                Raza = txtRaza.Text.Trim(),
                Sexo = rdoHembra.Checked ? "H" : "M",
                FechaNacimiento = dtpNacimiento.Value.Date
            };

            if (!persistir)
            {
                Resultado = mascota;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            try
            {
                // 5.1: ya existe una mascota con ese nombre para el cliente → confirmación.
                bool nombreCambio = EsAlta || !string.Equals(original!.Nombre, mascota.Nombre, StringComparison.OrdinalIgnoreCase);
                if (nombreCambio && clienteBLL.ExisteMascotaConNombre(clienteId, mascota.Nombre, mascota.Id) &&
                    !Mensajes_60MN.Confirmar($"El cliente ya tiene una mascota llamada {mascota.Nombre}. ¿Desea registrarla igualmente?"))
                    return;

                if (EsAlta)
                    clienteBLL.RegistrarMascota(mascota);
                else
                    clienteBLL.ModificarMascota(mascota);
                Mensajes_60MN.Informar(EsAlta ? "Mascota registrada correctamente." : "Mascota modificada correctamente.");
                Resultado = mascota;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
