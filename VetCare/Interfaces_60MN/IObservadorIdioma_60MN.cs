namespace Interfaces_60MN
{
    /// <summary>
    /// Observador del patrón Observer de multi-idioma. Cada formulario abierto
    /// se suscribe al GestorIdioma_60MN (sujeto) y es notificado cuando el
    /// usuario cambia el idioma, para volver a traducir sus textos.
    /// </summary>
    public interface IObservadorIdioma_60MN
    {
        void ActualizarIdioma(IIdioma_60MN idioma);
    }
}
