using Interfaces_60MN;

namespace Servicios_60MN.Idioma
{
    /// <summary>
    /// Sujeto del patrón Observer de multi-idioma (Singleton). Mantiene el
    /// idioma actual y su diccionario de traducciones (clave → texto) y
    /// notifica a los formularios suscriptos cada vez que el idioma cambia.
    /// No accede a la base: la BLL (IdiomaBLL_60MN) le entrega las traducciones.
    /// </summary>
    public sealed class GestorIdioma_60MN
    {
        private static GestorIdioma_60MN? instancia;
        private static readonly object candado = new object();

        private readonly List<IObservadorIdioma_60MN> observadores = new List<IObservadorIdioma_60MN>();
        private Dictionary<string, string> traducciones = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private GestorIdioma_60MN() { }

        public static GestorIdioma_60MN Instancia
        {
            get
            {
                lock (candado)
                {
                    return instancia ??= new GestorIdioma_60MN();
                }
            }
        }

        public IIdioma_60MN? IdiomaActual { get; private set; }

        public void Suscribir(IObservadorIdioma_60MN observador)
        {
            if (!observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void Desuscribir(IObservadorIdioma_60MN observador)
        {
            observadores.Remove(observador);
        }

        /// <summary>Cambia el idioma actual y notifica a todos los observadores.</summary>
        public void CambiarIdioma(IIdioma_60MN idioma, IDictionary<string, string> nuevasTraducciones)
        {
            IdiomaActual = idioma;
            traducciones = new Dictionary<string, string>(nuevasTraducciones, StringComparer.OrdinalIgnoreCase);
            Notificar();
        }

        private void Notificar()
        {
            // Se recorre una copia: un observador puede desuscribirse mientras se lo notifica.
            foreach (IObservadorIdioma_60MN observador in observadores.ToList())
                observador.ActualizarIdioma(IdiomaActual!);
        }

        /// <summary>Busca la traducción de una clave en el idioma actual.</summary>
        public bool TryTraducir(string clave, out string texto)
        {
            return traducciones.TryGetValue(clave, out texto!);
        }

        /// <summary>Traducción de la clave, o el texto por defecto (idioma base) si no existe.</summary>
        public string Traducir(string clave, string porDefecto)
        {
            return TryTraducir(clave, out string texto) ? texto : porDefecto;
        }
    }
}
