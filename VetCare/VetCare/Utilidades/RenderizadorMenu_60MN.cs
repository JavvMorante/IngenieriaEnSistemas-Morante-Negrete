namespace VetCare
{
    /// <summary>
    /// Dibujo del menú principal. Los ítems de la barra (ADMIN, MAESTROS, ...) tienen texto
    /// blanco sobre el verde azulado de la barra; el dibujo estándar de Windows pinta el
    /// ítem señalado o abierto con un fondo celeste claro, y el texto blanco se vuelve
    /// ilegible. Acá ese fondo se pinta en un tono más oscuro de la misma barra. Los
    /// desplegables mantienen el dibujo estándar (texto oscuro sobre fondo claro).
    /// El diseño del menú (ítems, textos, fuentes, patentes en Tag) sigue en el diseñador.
    /// </summary>
    internal sealed class RenderizadorMenu_60MN : ToolStripProfessionalRenderer
    {
        /// <summary>Fondo de un ítem de la barra señalado con el mouse.</summary>
        private static readonly Color Senalado = Color.FromArgb(0, 128, 128);

        /// <summary>Fondo de un ítem de la barra con su desplegable abierto.</summary>
        private static readonly Color Abierto = Color.FromArgb(0, 70, 70);

        /// <summary>Línea inferior del ítem abierto, para marcar cuál está desplegado.</summary>
        private static readonly Color Acento = Color.FromArgb(128, 222, 234);

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.IsOnDropDown || !(e.Item.Selected || e.Item.Pressed))
            {
                base.OnRenderMenuItemBackground(e);
                return;
            }

            Rectangle area = new Rectangle(Point.Empty, e.Item.Size);
            using (SolidBrush fondo = new SolidBrush(e.Item.Pressed ? Abierto : Senalado))
                e.Graphics.FillRectangle(fondo, area);
            if (e.Item.Pressed)
                using (SolidBrush linea = new SolidBrush(Acento))
                    e.Graphics.FillRectangle(linea, 0, area.Height - 3, area.Width, 3);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            // En la barra el texto queda siempre blanco (también señalado o abierto).
            if (!e.Item.IsOnDropDown && e.Item.Owner is MenuStrip)
                e.TextColor = Color.White;
            base.OnRenderItemText(e);
        }
    }
}
