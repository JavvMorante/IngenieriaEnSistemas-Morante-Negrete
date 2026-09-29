using Interfaces_60MN;

namespace Entidades_60MN
{
    public enum TipoMovimiento_60MN
    {
        Ingreso,
        Egreso,
        Ajuste
    }

    /// <summary>Ingreso, egreso o ajuste de existencias de un producto (CUN14).</summary>
    public class MovimientoStock_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public TipoMovimiento_60MN Tipo { get; set; }

        /// <summary>Cantidad con signo: positiva suma stock, negativa lo resta.</summary>
        public int Cantidad { get; set; }

        public int StockAnterior { get; set; }

        public int StockResultante { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; }

        public int? UsuarioId { get; set; }
    }
}
