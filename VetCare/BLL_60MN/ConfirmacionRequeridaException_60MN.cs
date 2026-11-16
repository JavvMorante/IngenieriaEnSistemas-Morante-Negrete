using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// Una regla de negocio pide confirmar antes de seguir (por ejemplo, el precio de venta queda
    /// por debajo del costo o la compra supera el tope mensual). La GUI muestra el mensaje y, si el
    /// usuario confirma, repite la operación con confirmado = true.
    /// </summary>
    public class ConfirmacionRequeridaException_60MN : NegocioException_60MN
    {
        public ConfirmacionRequeridaException_60MN(string mensaje) : base(mensaje) { }
    }
}
