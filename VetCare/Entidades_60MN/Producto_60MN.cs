using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Medicamento, insumo o producto veterinario del inventario (PN4).</summary>
    public class Producto_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        /// <summary>Código generado por la base (columna calculada PRD-00001).</summary>
        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public int CategoriaId { get; set; }

        public string CategoriaNombre { get; set; } = string.Empty;

        public int ProveedorId { get; set; }

        public string ProveedorNombre { get; set; } = string.Empty;

        public int StockActual { get; set; }

        public int StockMinimo { get; set; }

        public decimal PrecioCosto { get; set; }

        public decimal PrecioVenta { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string Lote { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public bool BajoStockMinimo => StockActual <= StockMinimo;

        public string Estado => Activo ? "Activo" : "Baja";

        public override string ToString() => $"{Codigo} - {Nombre} (Lote {Lote})";
    }
}
