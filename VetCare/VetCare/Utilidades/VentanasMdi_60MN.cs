using System.Runtime.CompilerServices;

namespace VetCare
{
    /// <summary>
    /// Abre las pantallas de trabajo como hijas del menú principal (MDI), igual que las
    /// opciones del menú, en lugar de ventanas sueltas que pueden quedar fuera de él.
    ///
    /// - Si ya hay una ventana abierta del mismo tipo y con la misma clave (por ejemplo,
    ///   la historia clínica de la misma mascota), la trae al frente en vez de abrir otra.
    /// - La centra dentro del área del menú principal.
    /// - alCerrar reemplaza lo que antes se hacía después de ShowDialog (recargar datos);
    ///   solo se ejecuta si la pantalla que la abrió sigue abierta.
    ///
    /// Los diálogos cortos de carga (alta de cliente, mascota, usuario, confirmaciones)
    /// siguen siendo modales: es lo correcto en WinForms y un modal no puede ser hijo MDI.
    /// </summary>
    internal static class VentanasMdi_60MN
    {
        private static readonly ConditionalWeakTable<Form, string> claves = new ConditionalWeakTable<Form, string>();

        public static T Abrir<T>(Form origen, Func<T> crear, string? clave = null, Action? alCerrar = null) where T : Form
        {
            Form? principal = origen.IsMdiContainer ? origen : origen.MdiParent;

            if (principal != null)
            {
                T? abierto = principal.MdiChildren.OfType<T>().FirstOrDefault(f => ClaveDe(f) == clave);
                if (abierto != null)
                {
                    if (abierto.WindowState == FormWindowState.Minimized)
                        abierto.WindowState = FormWindowState.Normal;
                    abierto.Activate();
                    return abierto;
                }
            }

            T formulario = crear();
            if (clave != null)
                claves.Add(formulario, clave);
            CerrarConBotonCancelar(formulario);
            if (alCerrar != null)
                formulario.FormClosed += (s, e) => { if (!origen.IsDisposed) alCerrar(); };

            if (principal == null)
            {
                // Sin menú principal (no debería ocurrir): ventana con dueño, al menos queda por encima de quien la abrió.
                formulario.Show(origen);
                return formulario;
            }

            formulario.MdiParent = principal;
            formulario.StartPosition = FormStartPosition.Manual;
            // El tamaño definitivo se conoce en Load (después del escalado por DPI): recién ahí se centra.
            formulario.Load += (s, e) => Centrar(formulario, principal);
            formulario.Show();
            return formulario;
        }

        private static string? ClaveDe(Form formulario) => claves.TryGetValue(formulario, out string clave) ? clave : null;

        /// <summary>
        /// En una ventana no modal, un botón con DialogResult no la cierra (solo lo hace en
        /// ShowDialog). Para que "Cerrar" y la tecla Esc sigan funcionando, el botón
        /// Cancelar del formulario pasa a cerrarlo.
        /// </summary>
        private static void CerrarConBotonCancelar(Form formulario)
        {
            if (formulario.CancelButton is Button boton && boton.DialogResult != DialogResult.None)
                boton.Click += (s, e) => { if (!formulario.IsDisposed) formulario.Close(); };
        }

        private static void Centrar(Form formulario, Form principal)
        {
            MdiClient? area = principal.Controls.OfType<MdiClient>().FirstOrDefault();
            if (area == null) return;
            formulario.Location = new Point(
                Math.Max(0, (area.ClientSize.Width - formulario.Width) / 2),
                Math.Max(0, (area.ClientSize.Height - formulario.Height) / 2));
        }
    }
}
