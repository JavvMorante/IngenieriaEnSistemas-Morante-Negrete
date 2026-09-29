using BLL_60MN;

namespace VetCare
{
    /// <summary>CUS07 Cambiar Contraseña (también invocado por CUS05 en el primer ingreso).</summary>
    public partial class FrmCambiarClave_60MN : FormBase_60MN
    {
        private readonly UsuarioBLL_60MN usuarioBLL = new UsuarioBLL_60MN();

        private readonly bool obligatorio;

        public FrmCambiarClave_60MN() : this(false) { }

        public FrmCambiarClave_60MN(bool obligatorio)
        {
            InitializeComponent();
            this.obligatorio = obligatorio;
        }

        protected override void AlCambiarIdioma()
        {
            if (obligatorio)
                lblInfo.Text = TraductorFormularios_60MN.T("FrmCambiarClave_60MN.lblInfoPrimerIngreso",
                    "Primer ingreso: reemplace la contraseña inicial.");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool faltanDatos = false;
            foreach (TextBox campo in new[] { txtActual, txtNueva, txtConfirmacion })
                if (campo.Text.Length == 0)
                {
                    errorProvider.SetError(campo, "Campo obligatorio.");
                    faltanDatos = true;
                }
            if (faltanDatos) return;

            try
            {
                usuarioBLL.CambiarClave(txtActual.Text, txtNueva.Text, txtConfirmacion.Text);
                Mensajes_60MN.Informar("Contraseña cambiada con éxito.");
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
