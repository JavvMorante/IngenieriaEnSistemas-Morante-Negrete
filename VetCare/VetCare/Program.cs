using System.Runtime.InteropServices;

namespace VetCare
{
    internal static class Program
    {
        /// <summary>
        /// Marca el proceso como "DPI aware": con el escalado de pantalla de Windows
        /// (125 %, 150 %) los formularios se escalan con su fuente en lugar de que
        /// Windows estire la imagen y la muestre borrosa.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// Punto de entrada. Muestra el Login (CUS05) y, si el ingreso es
        /// correcto, el menú principal. Al cerrar sesión (CUS06) se vuelve al
        /// Login; al salir se termina la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicialización estándar de WinForms en .NET Framework 4.8.
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                // El Login abre siempre en el idioma base (español), también después de un
                // Re-Login o Logout. El idioma de cada usuario se aplica recién al ingresar.
                AplicarIdiomaBase();

                using (FrmLogin_60MN login = new FrmLogin_60MN())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        return;
                }

                using (FrmMenuPrincipal_60MN menu = new FrmMenuPrincipal_60MN())
                {
                    Application.Run(menu);
                    if (!menu.SesionCerrada)
                        return;
                }
            }
        }

        private static void AplicarIdiomaBase()
        {
            try
            {
                new BLL_60MN.IdiomaBLL_60MN().AplicarBase();
            }
            catch (Exception)
            {
                // Sin base de datos se muestran los textos del diseñador (español); el Login informará el error de conexión.
            }
        }
    }
}
