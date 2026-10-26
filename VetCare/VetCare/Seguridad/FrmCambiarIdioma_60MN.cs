using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Idioma;

namespace VetCare
{
    /// <summary>
    /// Cambio de idioma desde el menú USUARIO. Al aceptar, el GestorIdioma_60MN
    /// notifica a todos los formularios abiertos (Observer) y el idioma queda
    /// guardado como el del usuario logueado (se le aplica en cada inicio de sesión).
    /// </summary>
    public partial class FrmCambiarIdioma_60MN : FormBase_60MN
    {
        private readonly IdiomaBLL_60MN idiomaBLL = new IdiomaBLL_60MN();

        public FrmCambiarIdioma_60MN()
        {
            InitializeComponent();
        }

        private void FrmCambiarIdioma_60MN_Load(object sender, EventArgs e)
        {
            try
            {
                List<Idioma_60MN> idiomas = idiomaBLL.Listar();
                cboIdioma.DataSource = idiomas;
                int actual = GestorIdioma_60MN.Instancia.IdiomaActual?.Id ?? 0;
                cboIdioma.SelectedIndex = Math.Max(0, idiomas.FindIndex(i => i.Id == actual));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cboIdioma.SelectedItem is not Idioma_60MN idioma) return;
            try
            {
                idiomaBLL.CambiarIdiomaUsuario(idioma);
                Mensajes_60MN.Informar(TraductorFormularios_60MN.T("msg.IdiomaGuardado",
                    "Idioma cambiado. Quedó guardado como su idioma y se aplicará cada vez que inicie sesión."));
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
