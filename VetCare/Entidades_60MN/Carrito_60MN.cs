namespace Entidades_60MN
{
    /// <summary>
    /// CUN05 Registrar Medicación en Carrito: el carrito vive en memoria mientras se arma y
    /// recién al confirmarlo se graba como venta (VentaEncabezado y VentaDetalle).
    /// La atención es opcional: también se le vende a un cliente sin atención.
    /// </summary>
    public class Carrito_60MN
    {
        private readonly List<ItemCarrito_60MN> items = new List<ItemCarrito_60MN>();

        public int ClienteId { get; set; }

        public int? AtencionId { get; set; }

        public IReadOnlyList<ItemCarrito_60MN> Items => items;

        public decimal Total => items.Sum(i => i.Subtotal);

        /// <summary>Agrega el producto o, si ya está en el carrito, le suma la cantidad.</summary>
        public void Agregar(ItemCarrito_60MN item)
        {
            ItemCarrito_60MN? existente = Buscar(item.ProductoId);
            if (existente != null)
                existente.Cantidad += item.Cantidad;
            else
                items.Add(item);
        }

        public void Quitar(int productoId) => items.RemoveAll(i => i.ProductoId == productoId);

        public ItemCarrito_60MN? Buscar(int productoId) => items.FirstOrDefault(i => i.ProductoId == productoId);
    }

    /// <summary>Producto del carrito o entregado en una atención (fila de VentaDetalle).</summary>
    public class ItemCarrito_60MN
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Producto { get; set; } = string.Empty;

        public string Lote { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
