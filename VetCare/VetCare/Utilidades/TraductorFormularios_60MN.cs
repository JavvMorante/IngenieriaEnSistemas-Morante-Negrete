using System.Runtime.CompilerServices;
using Servicios_60MN.Idioma;

namespace VetCare
{
    /// <summary>
    /// Traduce los textos de un formulario con el idioma actual del GestorIdioma_60MN.
    ///
    /// Para cada componente busca primero la clave "Formulario.Control" y luego
    /// la clave genérica "Control". Si no hay traducción se usa el texto
    /// original del diseñador (idioma base, español), que se guarda la primera
    /// vez para poder volver a él al cambiar de idioma.
    ///
    /// Se traducen: título del formulario, Label, Button, CheckBox, RadioButton,
    /// GroupBox, encabezados de DataGridView e ítems de MenuStrip. No se tocan
    /// TextBox/ComboBox (contienen datos) ni textos vacíos (los completa el código).
    /// </summary>
    internal static class TraductorFormularios_60MN
    {
        private static readonly ConditionalWeakTable<object, string> textosOriginales = new ConditionalWeakTable<object, string>();

        public static void Traducir(Form formulario)
        {
            formulario.Text = TextoPara(formulario, formulario.Text, formulario.Name, formulario.Name);
            TraducirControles(formulario.Controls, formulario.Name);
        }

        private static void TraducirControles(Control.ControlCollection controles, string formulario)
        {
            foreach (Control control in controles)
            {
                switch (control)
                {
                    case Label or ButtonBase or GroupBox:
                        control.Text = TextoPara(control, control.Text, formulario, control.Name);
                        break;
                    case DataGridView grilla:
                        foreach (DataGridViewColumn columna in grilla.Columns)
                            columna.HeaderText = TextoPara(columna, columna.HeaderText, formulario, columna.Name);
                        break;
                    case MenuStrip menu:
                        TraducirItems(menu.Items, formulario);
                        break;
                }
                if (control.HasChildren)
                    TraducirControles(control.Controls, formulario);
            }
        }

        private static void TraducirItems(ToolStripItemCollection items, string formulario)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripSeparator) continue;
                item.Text = TextoPara(item, item.Text ?? string.Empty, formulario, item.Name);
                if (item is ToolStripMenuItem menuItem)
                    TraducirItems(menuItem.DropDownItems, formulario);
            }
        }

        private static string TextoPara(object componente, string textoActual, string formulario, string nombre)
        {
            if (!textosOriginales.TryGetValue(componente, out string? original))
            {
                original = textoActual;
                textosOriginales.Add(componente, original);
            }
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(nombre))
                return textoActual;

            GestorIdioma_60MN gestor = GestorIdioma_60MN.Instancia;
            if (gestor.TryTraducir(formulario + "." + nombre, out string texto) || gestor.TryTraducir(nombre, out texto))
                return texto;
            return original;
        }

        /// <summary>Atajo para textos que arma el código (mensajes, barra de estado).</summary>
        public static string T(string clave, string porDefecto) => GestorIdioma_60MN.Instancia.Traducir(clave, porDefecto);
    }
}
