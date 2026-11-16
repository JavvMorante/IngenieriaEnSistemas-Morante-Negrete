using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>CUN14 Registrar Recepción de Mercadería: lo que entregó el proveedor con su remito.</summary>
    public class Recepcion_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int OrdenCompraId { get; set; }

        public string NroRemito { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; }

        public int UsuarioId { get; set; }

        public string? Observaciones { get; set; }

        public List<RecepcionDetalle_60MN> Detalle { get; } = new List<RecepcionDetalle_60MN>();
    }

    /// <summary>
    /// Producto recibido. Cada cantidad mayor a cero genera un movimiento INGRESO (MovimientoId)
    /// y actualiza el stock, el lote y el vencimiento del producto.
    /// </summary>
    public class RecepcionDetalle_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int RecepcionId { get; set; }

        public int ProductoId { get; set; }

        public string Producto { get; set; } = string.Empty;

        public int CantidadRecibida { get; set; }

        public string Lote { get; set; } = string.Empty;

        public DateTime FechaVencimiento { get; set; }

        public string? Observacion { get; set; }

        public int? MovimientoId { get; set; }
    }

    /// <summary>Resultado de CUN14: el estado en que quedó la orden y los avisos para el encargado.</summary>
    public class ResultadoRecepcion_60MN
    {
        public int RecepcionId { get; set; }

        public EstadoOrden_60MN EstadoOrden { get; set; }

        public int UnidadesIngresadas { get; set; }

        public List<string> Advertencias { get; } = new List<string>();
    }
}
