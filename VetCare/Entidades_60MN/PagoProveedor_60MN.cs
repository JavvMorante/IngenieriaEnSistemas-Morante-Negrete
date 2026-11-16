using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>
    /// CUN13 Registrar Pago al Proveedor. El pago se hace por transferencia en el home banking
    /// del banco: el sistema solo registra el comprobante.
    /// </summary>
    public class PagoProveedor_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int OrdenCompraId { get; set; }

        public string Banco { get; set; } = string.Empty;

        public string NroComprobante { get; set; } = string.Empty;

        public DateTime FechaPago { get; set; }

        public decimal Importe { get; set; }

        /// <summary>Motivo cuando el importe difiere del total de la orden.</summary>
        public string? Observaciones { get; set; }

        public int UsuarioId { get; set; }
    }
}
