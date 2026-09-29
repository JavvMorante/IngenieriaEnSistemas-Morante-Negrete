namespace VetCare
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada. Muestra el Login (CUS05) y, si el ingreso es
        /// correcto, el menú principal. Al cerrar sesión (CUS06) se vuelve al
        /// Login; al salir se termina la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Idioma predeterminado: el último elegido en el Login o desde USUARIO > Cambiar idioma.
            try
            {
                new BLL_60MN.IdiomaBLL_60MN().AplicarPredeterminado();
            }
            catch (Exception)
            {
                // Sin base de datos se muestra el idioma base (español); el Login informará el error de conexión.
            }

            while (true)
            {
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
    }
}
