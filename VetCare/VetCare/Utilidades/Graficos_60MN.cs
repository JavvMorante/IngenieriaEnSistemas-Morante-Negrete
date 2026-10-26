using System.Drawing.Drawing2D;
using System.Globalization;

namespace VetCare
{
    /// <summary>
    /// Dibujo de los gráficos del Reporte Inteligente con GDI+. No se usa ninguna
    /// librería externa: todo se traza en el evento Paint de un Panel, de modo que el
    /// mismo código sirve para la pantalla y para la impresión, donde se recibe el
    /// Graphics de la página en lugar del control.
    /// </summary>
    internal static class Graficos_60MN
    {
        /// <summary>Paleta de las series. Se recorre de forma cíclica si hay más categorías.</summary>
        public static readonly Color[] Paleta =
        {
            Color.FromArgb(31, 119, 180), Color.FromArgb(44, 160, 44), Color.FromArgb(255, 127, 14),
            Color.FromArgb(148, 103, 189), Color.FromArgb(214, 39, 40), Color.FromArgb(23, 190, 207),
            Color.FromArgb(188, 189, 34), Color.FromArgb(140, 86, 75), Color.FromArgb(227, 119, 194),
            Color.FromArgb(127, 127, 127)
        };

        private static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("es-AR");
        private static readonly Color Ejes = Color.FromArgb(110, 110, 110);
        private static readonly Color Grilla = Color.FromArgb(226, 230, 234);
        private static readonly Color Texto = Color.FromArgb(50, 50, 50);

        public static Color ColorSerie(int indice) => Paleta[indice % Paleta.Length];

        private static void Preparar(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }

        /// <summary>Título del gráfico. Devuelve el alto consumido.</summary>
        private static int Titulo(Graphics g, Rectangle area, string texto)
        {
            if (string.IsNullOrEmpty(texto)) return 0;
            using Font fuente = new Font("Segoe UI", 11F, FontStyle.Bold);
            g.DrawString(texto, fuente, new SolidBrush(Texto), area.Left, area.Top);
            return (int)g.MeasureString(texto, fuente).Height + 6;
        }

        /// <summary>Mensaje centrado para cuando no hay datos que graficar.</summary>
        public static void Vacio(Graphics g, Rectangle area, string titulo, string mensaje)
        {
            Preparar(g);
            Titulo(g, area, titulo);
            using Font fuente = new Font("Segoe UI", 10F, FontStyle.Italic);
            using StringFormat centrado = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(mensaje, fuente, new SolidBrush(Color.Gray), area, centrado);
        }

        /// <summary>
        /// Barras apiladas: un grupo por período en el eje X y un tramo de color por
        /// categoría. Sirve para ver en qué meses sube la demanda y de qué categoría.
        /// </summary>
        /// <param name="valores">valores[periodo, serie] en unidades.</param>
        public static void BarrasApiladas(Graphics g, Rectangle area, string titulo,
                                          IList<string> periodos, IList<string> series, int[,] valores)
        {
            Preparar(g);
            int alto = Titulo(g, area, titulo);
            Rectangle marco = new Rectangle(area.Left, area.Top + alto, area.Width, area.Height - alto);

            using Font fEje = new Font("Segoe UI", 8F);
            using Font fLeyenda = new Font("Segoe UI", 8.5F);
            using Brush bTexto = new SolidBrush(Texto);
            using Pen pEje = new Pen(Ejes, 1F);
            using Pen pGrilla = new Pen(Grilla, 1F);

            // La leyenda ocupa una franja a la derecha; el resto es el área de trazado.
            int anchoLeyenda = Math.Min(190, marco.Width / 4);
            int margenIzq = 56, margenInf = 34, margenSup = 10;
            Rectangle plot = new Rectangle(marco.Left + margenIzq, marco.Top + margenSup,
                                           marco.Width - margenIzq - anchoLeyenda - 10,
                                           marco.Height - margenSup - margenInf);
            if (plot.Width <= 20 || plot.Height <= 20) return;

            // Máximo del eje Y: el total apilado más alto, redondeado hacia arriba.
            int maximo = 0;
            for (int p = 0; p < periodos.Count; p++)
            {
                int total = 0;
                for (int s = 0; s < series.Count; s++) total += valores[p, s];
                maximo = Math.Max(maximo, total);
            }
            if (maximo <= 0) maximo = 1;
            int paso = PasoEje(maximo);
            int tope = ((maximo + paso - 1) / paso) * paso;

            // Grilla horizontal y etiquetas del eje Y.
            for (int v = 0; v <= tope; v += paso)
            {
                int y = plot.Bottom - (int)((double)v / tope * plot.Height);
                g.DrawLine(pGrilla, plot.Left, y, plot.Right, y);
                string etiqueta = v.ToString("N0", Ar);
                SizeF medida = g.MeasureString(etiqueta, fEje);
                g.DrawString(etiqueta, fEje, bTexto, plot.Left - medida.Width - 6, y - medida.Height / 2);
            }
            g.DrawLine(pEje, plot.Left, plot.Top, plot.Left, plot.Bottom);
            g.DrawLine(pEje, plot.Left, plot.Bottom, plot.Right, plot.Bottom);

            // Barras.
            double anchoGrupo = (double)plot.Width / Math.Max(periodos.Count, 1);
            int anchoBarra = (int)Math.Max(6, Math.Min(46, anchoGrupo * 0.62));
            for (int p = 0; p < periodos.Count; p++)
            {
                int centro = plot.Left + (int)(anchoGrupo * (p + 0.5));
                int x = centro - anchoBarra / 2;
                int baseY = plot.Bottom;

                for (int s = 0; s < series.Count; s++)
                {
                    int v = valores[p, s];
                    if (v <= 0) continue;
                    int h = (int)Math.Round((double)v / tope * plot.Height);
                    if (h < 1) h = 1;
                    Rectangle tramo = new Rectangle(x, baseY - h, anchoBarra, h);
                    using (SolidBrush relleno = new SolidBrush(ColorSerie(s)))
                        g.FillRectangle(relleno, tramo);
                    if (h > 3)
                        using (Pen borde = new Pen(Color.FromArgb(70, Color.White), 1F))
                            g.DrawRectangle(borde, tramo);
                    baseY -= h;
                }

                // Total encima de la barra, si entra.
                int totalP = 0;
                for (int s = 0; s < series.Count; s++) totalP += valores[p, s];
                if (totalP > 0 && anchoBarra >= 18)
                {
                    string t = totalP.ToString("N0", Ar);
                    SizeF m = g.MeasureString(t, fEje);
                    g.DrawString(t, fEje, bTexto, centro - m.Width / 2, baseY - m.Height - 2);
                }

                // Etiqueta del período: rotada si los grupos quedan angostos.
                string periodo = periodos[p];
                SizeF mp = g.MeasureString(periodo, fEje);
                if (mp.Width <= anchoGrupo - 2)
                {
                    g.DrawString(periodo, fEje, bTexto, centro - mp.Width / 2, plot.Bottom + 6);
                }
                else
                {
                    GraphicsState estado = g.Save();
                    g.TranslateTransform(centro, plot.Bottom + 6);
                    g.RotateTransform(-45);
                    g.DrawString(periodo, fEje, bTexto, -mp.Width, 0);
                    g.Restore(estado);
                }
            }

            // Leyenda de categorías.
            int ly = plot.Top;
            int lx = plot.Right + 14;
            foreach (string serie in series)
            {
                int i = series.IndexOf(serie);
                using (SolidBrush b = new SolidBrush(ColorSerie(i)))
                    g.FillRectangle(b, lx, ly + 2, 11, 11);
                string nombre = Recortar(g, serie, fLeyenda, anchoLeyenda - 32);
                g.DrawString(nombre, fLeyenda, bTexto, lx + 16, ly);
                ly += 18;
                if (ly > plot.Bottom - 14) break;
            }
        }

        /// <summary>
        /// Barras horizontales ordenadas de mayor a menor. Se usa para el ranking de
        /// productos, donde los nombres son largos y no entran en un eje vertical.
        /// </summary>
        public static void BarrasHorizontales(Graphics g, Rectangle area, string titulo,
                                              IList<string> etiquetas, IList<int> valores,
                                              IList<string>? detalles = null)
        {
            Preparar(g);
            int alto = Titulo(g, area, titulo);
            Rectangle marco = new Rectangle(area.Left, area.Top + alto, area.Width, area.Height - alto);

            using Font fEje = new Font("Segoe UI", 8.5F);
            using Font fValor = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            using Brush bTexto = new SolidBrush(Texto);
            using Brush bSuave = new SolidBrush(Color.FromArgb(120, 120, 120));
            using Pen pEje = new Pen(Ejes, 1F);

            if (etiquetas.Count == 0) return;

            int anchoEtiqueta = Math.Min(210, marco.Width / 3);
            int margenDer = 72;
            Rectangle plot = new Rectangle(marco.Left + anchoEtiqueta + 8, marco.Top,
                                           marco.Width - anchoEtiqueta - 8 - margenDer, marco.Height - 8);
            if (plot.Width <= 20 || plot.Height <= 10) return;

            int maximo = Math.Max(1, valores.Max());
            double altoFila = (double)plot.Height / etiquetas.Count;
            int altoBarra = (int)Math.Max(5, Math.Min(26, altoFila * 0.64));

            g.DrawLine(pEje, plot.Left, plot.Top, plot.Left, plot.Bottom);

            for (int i = 0; i < etiquetas.Count; i++)
            {
                int centro = plot.Top + (int)(altoFila * (i + 0.5));
                int ancho = (int)Math.Round((double)valores[i] / maximo * plot.Width);
                if (ancho < 2) ancho = 2;
                Rectangle barra = new Rectangle(plot.Left + 1, centro - altoBarra / 2, ancho, altoBarra);

                using (SolidBrush b = new SolidBrush(ColorSerie(i)))
                    g.FillRectangle(b, barra);

                // Nombre del producto, recortado a la franja de la izquierda.
                string etiqueta = Recortar(g, etiquetas[i], fEje, anchoEtiqueta - 6);
                SizeF me = g.MeasureString(etiqueta, fEje);
                g.DrawString(etiqueta, fEje, bTexto, plot.Left - 6 - me.Width, centro - me.Height / 2);

                // Valor a la derecha de la barra y, si hay lugar, el detalle debajo.
                string valor = valores[i].ToString("N0", Ar);
                SizeF mv = g.MeasureString(valor, fValor);
                g.DrawString(valor, fValor, bTexto, barra.Right + 6, centro - mv.Height / 2);

                if (detalles != null && i < detalles.Count && altoFila >= 30)
                {
                    SizeF md = g.MeasureString(detalles[i], fEje);
                    g.DrawString(detalles[i], fEje, bSuave, barra.Right + 6 + mv.Width + 8, centro - md.Height / 2);
                }
            }
        }

        /// <summary>Paso del eje Y que deja entre 4 y 10 marcas con números redondos.</summary>
        private static int PasoEje(int maximo)
        {
            int[] bases = { 1, 2, 5 };
            int magnitud = 1;
            while (true)
            {
                foreach (int b in bases)
                {
                    int paso = b * magnitud;
                    if (maximo / paso <= 8) return paso;
                }
                magnitud *= 10;
                if (magnitud > 1_000_000) return magnitud;
            }
        }

        /// <summary>Recorta un texto con puntos suspensivos para que entre en el ancho dado.</summary>
        private static string Recortar(Graphics g, string texto, Font fuente, int ancho)
        {
            if (ancho <= 0 || g.MeasureString(texto, fuente).Width <= ancho) return texto;
            for (int largo = texto.Length - 1; largo > 1; largo--)
            {
                string corto = texto.Substring(0, largo) + "…";
                if (g.MeasureString(corto, fuente).Width <= ancho) return corto;
            }
            return "…";
        }
    }
}
