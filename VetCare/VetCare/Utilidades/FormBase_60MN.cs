using System.ComponentModel;
using Interfaces_60MN;
using Servicios_60MN.Idioma;

namespace VetCare
{
    /// <summary>
    /// Formulario base de VetCare. Es el OBSERVADOR concreto del patrón Observer
    /// de multi-idioma: al cargarse se suscribe al GestorIdioma_60MN (sujeto) y
    /// se traduce; si el idioma cambia mientras está abierto, el gestor lo
    /// notifica mediante ActualizarIdioma y se vuelve a traducir; al cerrarse
    /// se desuscribe. Todos los formularios del sistema heredan de esta clase,
    /// por lo que se siguen diseñando normalmente desde Visual Studio.
    /// </summary>
    public class FormBase_60MN : Form, IObservadorIdioma_60MN
    {
        private static bool EnDiseno => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        /// <summary>
        /// Fuente base de todas las pantallas. En .NET Framework la fuente por defecto es
        /// Microsoft Sans Serif 8.25; las pantallas se diseñaron con Segoe UI 9 y escalan
        /// según la fuente (AutoScaleMode.Font), así que se fija acá para que todas
        /// conserven el tamaño y el aspecto con que fueron diseñadas.
        /// </summary>
        public FormBase_60MN()
        {
            Font = new Font("Segoe UI", 9F);
            if (IconoVetCare != null)
                Icon = IconoVetCare;
        }

        /// <summary>
        /// Ícono de VetCare (Recursos/VetCare.ico, embebido en el ejecutable) para la barra de título
        /// de todas las ventanas. Se carga una sola vez; en el diseñador o si falta queda el de Windows.
        /// </summary>
        private static readonly Icon? IconoVetCare = CargarIcono();

        private static Icon? CargarIcono()
        {
            try
            {
                using Stream? recurso = typeof(FormBase_60MN).Assembly.GetManifestResourceStream("VetCare.ico");
                return recurso == null ? null : new Icon(recurso);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode && !EnDiseno)
            {
                GestorIdioma_60MN.Instancia.Suscribir(this);
                TraductorFormularios_60MN.Traducir(this);
            }
            base.OnLoad(e);
            if (!DesignMode && !EnDiseno)
                AlCambiarIdioma();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            GestorIdioma_60MN.Instancia.Desuscribir(this);
            base.OnFormClosed(e);
        }

        public void ActualizarIdioma(IIdioma_60MN idioma)
        {
            if (IsDisposed) return;
            TraductorFormularios_60MN.Traducir(this);
            AlCambiarIdioma();
        }

        /// <summary>
        /// Ayuda en línea (G05): F1 en cualquier formulario abre el tema de la pantalla activa.
        /// La tecla llega primero al formulario que tiene el foco (una ventana hija o un
        /// diálogo), así que cada pantalla abre su propio tema.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1 && !(this is FrmAyuda_60MN) && !DesignMode && !EnDiseno)
            {
                FrmAyuda_60MN.Mostrar(this);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>Punto de extensión para re-traducir textos que arma el código (no el diseñador).</summary>
        protected virtual void AlCambiarIdioma() { }
    }
}
