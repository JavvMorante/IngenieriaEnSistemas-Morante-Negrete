using System.Drawing.Printing;

namespace VetCare
{
    /// <summary>
    /// Imprime un texto de ancho fijo (informes y reportes) con paginado. El usuario
    /// elige la impresora en el diálogo estándar; con "Microsoft Print to PDF" el
    /// documento se guarda como archivo PDF (CUN06 6.1, CUN10 paso 5, CUN15 paso 6).
    /// </summary>
    internal static class ImpresionTexto_60MN
    {
        /// <summary>Devuelve el nombre de la impresora usada, o null si el usuario canceló (CUN10 4.1).</summary>
        public static string? Imprimir(IWin32Window propietario, string titulo, string texto)
        {
            string[] lineas = texto.Replace("\r\n", "\n").Split('\n');
            int siguiente = 0;
            using Font fuente = new Font("Consolas", 9F);
            using PrintDocument documento = new PrintDocument { DocumentName = titulo };
            documento.BeginPrint += (s, e) => siguiente = 0;
            documento.PrintPage += (s, e) =>
            {
                float alto = fuente.GetHeight(e.Graphics!);
                float y = e.MarginBounds.Top;
                while (siguiente < lineas.Length && y + alto <= e.MarginBounds.Bottom)
                {
                    e.Graphics!.DrawString(lineas[siguiente], fuente, Brushes.Black, e.MarginBounds.Left, y);
                    y += alto;
                    siguiente++;
                }
                e.HasMorePages = siguiente < lineas.Length;
            };

            using PrintDialog dialogo = new PrintDialog { Document = documento, UseEXDialog = true, AllowSomePages = false };
            if (dialogo.ShowDialog(propietario) != DialogResult.OK)
                return null;
            documento.Print();
            return documento.PrinterSettings.PrinterName;
        }

        /// <summary>Alternativa cuando falla la impresora (CUN10 5.1): guardar el texto en un archivo.</summary>
        public static string? GuardarComoTexto(IWin32Window propietario, string nombreSugerido, string texto)
        {
            using SaveFileDialog dialogo = new SaveFileDialog
            {
                FileName = nombreSugerido,
                Filter = "Archivo de texto (*.txt)|*.txt",
                DefaultExt = "txt"
            };
            if (dialogo.ShowDialog(propietario) != DialogResult.OK)
                return null;
            File.WriteAllText(dialogo.FileName, texto, System.Text.Encoding.UTF8);
            return dialogo.FileName;
        }
    }
}
