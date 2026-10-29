using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Ítem de una venta (CUN05); cada uno tiene su egreso de stock (MovimientoID, CUN09).</summary>
    public class VentaDetalle_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Producto { get; set; } = string.Empty;

        public string Lote { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }

        public int MovimientoId { get; set; }
    }
}
