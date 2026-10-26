namespace VetCare
{
    /// <summary>
    /// Panel con doble buffer para dibujar gráficos. Un Panel común repinta directo
    /// sobre la pantalla y los trazos de GDI+ parpadean al redimensionar la ventana;
    /// con DoubleBuffered el dibujo se arma en memoria y se copia de una sola vez.
    /// </summary>
    internal class PanelGrafico_60MN : Panel
    {
        public PanelGrafico_60MN()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.White;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }
    }
}
