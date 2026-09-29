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

        /// <summary>Punto de extensión para re-traducir textos que arma el código (no el diseñador).</summary>
        protected virtual void AlCambiarIdioma() { }
    }
}
