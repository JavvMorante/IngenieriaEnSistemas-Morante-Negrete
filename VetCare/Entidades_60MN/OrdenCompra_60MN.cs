using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>
    /// CUN11 Registrar Orden de Compra: la cotización elegida de una solicitud de reposición.
    /// La aprueba o rechaza el gerente (CUN12), se paga (CUN13) y se recibe (CUN14).
    /// </summary>
    public class OrdenCompra_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        /// <summary>Número generado por la base (columna calculada OC-00001).</summary>
        public string Numero { get; set; } = string.Empty;

        public int SolicitudId { get; set; }

        public string SolicitudNumero { get; set; } = string.Empty;

        public int ProveedorId { get; set; }

        public string Proveedor { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; }

        public string NroCotizacion { get; set; } = string.Empty;

        public int PlazoEntregaDias { get; set; }

        public string CondicionPago { get; set; } = string.Empty;

        /// <summary>Obligatorio si no se eligió la cotización más barata.</summary>
        public string? MotivoEleccion { get; set; }

        /// <summary>Avisos para el gerente (por ejemplo, un precio más de 20 % por encima del costo actual).</summary>
        public string? Advertencias { get; set; }

        public decimal Total { get; set; }

        public EstadoOrden_60MN Estado { get; set; } = EstadoOrden_60MN.PendienteAprobacion;

        public int UsuarioId { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public int? AprobadaPor { get; set; }

        public string? AprobadaPorUsuario { get; set; }

        public DateTime? FechaAprobacion { get; set; }

        public string? MotivoRechazo { get; set; }

        public List<OrdenCompraDetalle_60MN> Detalle { get; } = new List<OrdenCompraDetalle_60MN>();

        public string EstadoTexto => Estado.Descripcion();

        public bool TieneAdvertencias => !string.IsNullOrWhiteSpace(Advertencias);

        public int UnidadesPendientes => Detalle.Sum(d => d.Pendiente);

        public override string ToString() => $"{Numero} - {Proveedor} ($ {Total:N2})";
    }

    /// <summary>Ítem de una orden de compra. CantidadRecibida suma lo ingresado por las recepciones (CUN14).</summary>
    public class OrdenCompraDetalle_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int OrdenCompraId { get; set; }

        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Producto { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }

        public int CantidadRecibida { get; set; }

        public int Pendiente => Cantidad - CantidadRecibida;

        /// <summary>Precio de costo actual del producto (para comparar con el precio cotizado).</summary>
        public decimal PrecioCostoActual { get; set; }

        /// <summary>Lote y vencimiento actuales del producto: se proponen al recibir (CUN14).</summary>
        public string LoteActual { get; set; } = string.Empty;

        public DateTime VencimientoActual { get; set; }
    }

    /// <summary>
    /// Cotización que el proveedor envía (paso 3 del proceso). Vive solo en memoria mientras el
    /// analista compara: se persiste únicamente la elegida, como orden de compra.
    /// </summary>
    public class Cotizacion_60MN
    {
        public int ProveedorId { get; set; }

        public string Proveedor { get; set; } = string.Empty;

        public string NroCotizacion { get; set; } = string.Empty;

        public int PlazoEntregaDias { get; set; }

        public string CondicionPago { get; set; } = string.Empty;

        /// <summary>Precio unitario cotizado por producto (ProductoId → precio).</summary>
        public Dictionary<int, decimal> Precios { get; } = new Dictionary<int, decimal>();

        /// <summary>Total con las cantidades pendientes de la solicitud (lo calcula la BLL al comparar).</summary>
        public decimal Total { get; set; }

        /// <summary>La de menor costo total entre las comparadas.</summary>
        public bool EsLaMasBarata { get; set; }
    }
}
