using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>Mensajes estándar de la GUI y habilitación de controles según las patentes de la sesión.</summary>
    internal static class Mensajes_60MN
    {
        private const string Titulo = "VetCare";

        public static void Informar(string mensaje) =>
            MessageBox.Show(mensaje, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Advertir(string mensaje) =>
            MessageBox.Show(mensaje, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static bool Confirmar(string mensaje) =>
            MessageBox.Show(mensaje, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

        /// <summary>
        /// Muestra una excepción: las reglas de negocio como advertencia y los
        /// errores técnicos como error (con el detalle para diagnóstico).
        /// </summary>
        public static void MostrarError(Exception ex)
        {
            if (ex is NegocioException_60MN || ex is LoginException_60MN)
                MessageBox.Show(ex.Message, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show(TraductorFormularios_60MN.T("msg.ErrorInesperado", "Ocurrió un error inesperado:") + "\n\n" + ex.Message, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Recorre los controles y oculta los que tienen en Tag un código de
        /// patente (o varios separados por '|') que el usuario no posee. Así
        /// la asociación control-patente se define desde la vista de diseño.
        /// </summary>
        public static void AplicarPermisos(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (control.Tag is string tag && tag.Length > 0)
                    control.Visible = TieneAlguno(tag);
                if (control.HasChildren)
                    AplicarPermisos(control);
            }
        }

        public static bool TieneAlguno(string codigosSeparadosPorBarra) =>
            codigosSeparadosPorBarra.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                    .Any(SessionManager_60MN.Instancia.TienePermiso);
    }
}
