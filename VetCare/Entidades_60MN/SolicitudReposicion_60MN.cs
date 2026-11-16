using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>CUN10 Generar Solicitud de Reposición: lo que el encargado de depósito pide reponer.</summary>
    public class SolicitudReposicion_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        /// <summary>Número generado por la base (columna calculada SR-00001).</summary>
        public string Numero { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; }

        public int UsuarioId { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public EstadoSolicitud_60MN Estado { get; set; } = EstadoSolicitud_60MN.Pendiente;

        public string? Observaciones { get; set; }

        public List<SolicitudDetalle_60MN> Detalle { get; } = new List<SolicitudDetalle_60MN>();

        public string EstadoTexto => Estado.Descripcion();

        /// <summary>Unidades que todavía no están en una orden de compra vigente.</summary>
        public int UnidadesPendientes => Detalle.Sum(d => d.PendienteDePedir);

        public override string ToString() => $"{Numero} - {FechaHora:dd/MM/yyyy} ({Detalle.Count} producto(s))";
    }

    /// <summary>
    /// Ítem de una solicitud de reposición. También representa un producto en alerta
    /// mientras se arma la solicitud (CantidadSugerida y Incluido son datos de pantalla).
    /// </summary>
    public class SolicitudDetalle_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int SolicitudId { get; set; }

        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Producto { get; set; } = string.Empty;

        /// <summary>Proveedor habitual del producto (Productos.ProveedorID).</summary>
        public string ProveedorHabitual { get; set; } = string.Empty;

        /// <summary>Stock y mínimo al momento de generar la solicitud.</summary>
        public int StockActual { get; set; }

        public int StockMinimo { get; set; }

        /// <summary>2 x mínimo - stock, como mínimo 1.</summary>
        public int CantidadSugerida { get; set; }

        public int CantidadPedida { get; set; }

        /// <summary>Unidades ya incluidas en órdenes de compra no anuladas ni rechazadas.</summary>
        public int CantidadEnOrdenes { get; set; }

        public int PendienteDePedir => Math.Max(CantidadPedida - CantidadEnOrdenes, 0);

        /// <summary>Precio de costo actual del producto: referencia para las cotizaciones (CUN11).</summary>
        public decimal PrecioCostoActual { get; set; }

        /// <summary>Marca de la grilla de CUN10: el producto se incluye en la solicitud.</summary>
        public bool Incluido { get; set; } = true;

        public bool EnAlerta { get; set; }
    }
}
