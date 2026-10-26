using System.Runtime.InteropServices;

namespace VetCare
{
    /// <summary>
    /// Texto guía en gris dentro de un TextBox vacío (por ejemplo "Código o nombre").
    /// En .NET Framework 4.8 TextBox no tiene la propiedad PlaceholderText, así que se
    /// usa el mensaje nativo EM_SETCUEBANNER del control de edición de Windows.
    /// </summary>
    internal static class TextoGuia_60MN
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public static void Asignar(TextBox cuadro, string texto)
        {
            if (cuadro.IsHandleCreated)
                Aplicar(cuadro, texto);
            else
                cuadro.HandleCreated += (s, e) => Aplicar(cuadro, texto);
        }

        // wParam = 1: el texto se sigue mostrando mientras el cuadro tiene el foco y está vacío.
        private static void Aplicar(TextBox cuadro, string texto) =>
            SendMessage(cuadro.Handle, EM_SETCUEBANNER, (IntPtr)1, texto);
    }
}
