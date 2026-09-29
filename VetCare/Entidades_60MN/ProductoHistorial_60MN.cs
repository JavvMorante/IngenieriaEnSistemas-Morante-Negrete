namespace Entidades_60MN
{
    /// <summary>Versión de un producto registrada por el trigger en Productos_C (Bitácora de Cambios, CUS10).</summary>
    public class ProductoHistorial_60MN
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string Proveedor { get; set; } = string.Empty;

        public int CategoriaId { get; set; }

        public int ProveedorId { get; set; }

        public int StockActual { get; set; }

        public int StockMinimo { get; set; }

        public decimal PrecioCosto { get; set; }

        public decimal PrecioVenta { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string Lote { get; set; } = string.Empty;

        public bool ActivoProducto { get; set; }

        /// <summary>ALTA, MODIFICACION, MOVIMIENTO, BAJA o ELIMINACION.</summary>
        public string Operacion { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; }

        public string Usuario { get; set; } = string.Empty;

        /// <summary>1 = versión vigente del producto.</summary>
        public bool Act { get; set; }
    }
}
