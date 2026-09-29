namespace Servicios_60MN.Excepciones
{
    /// <summary>
    /// Violación de una regla de negocio (flujo alternativo de un caso de uso).
    /// La GUI la muestra como mensaje al usuario; no es un error técnico.
    /// </summary>
    public class NegocioException_60MN : Exception
    {
        public NegocioException_60MN(string mensaje) : base(mensaje) { }
    }
}
