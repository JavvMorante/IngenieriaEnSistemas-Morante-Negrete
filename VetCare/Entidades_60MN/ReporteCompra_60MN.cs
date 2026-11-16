namespace Entidades_60MN
{
    /// <summary>CUN15 Generar Reporte de Compras: criterios de búsqueda.</summary>
    public class FiltroReporteCompra_60MN
    {
        public DateTime Desde { get; set; }

        public DateTime Hasta { get; set; }

        public int? ProveedorId { get; set; }

        public EstadoOrden_60MN? Estado { get; set; }
    }

    /// <summary>Una orden de compra del período, con lo pagado y lo recibido.</summary>
    public class FilaReporteCompra_60MN
    {
        public int OrdenCompraId { get; set; }

        public string Numero { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; }

        public int ProveedorId { get; set; }

        public string Proveedor { get; set; } = string.Empty;

        public EstadoOrden_60MN Estado { get; set; }

        public string EstadoTexto => Estado.Descripcion();

        public decimal Total { get; set; }

        public decimal ImportePagado { get; set; }

        public DateTime? UltimaRecepcion { get; set; }

        public int UnidadesPedidas { get; set; }

        public int UnidadesRecibidas { get; set; }

        /// <summary>Unidades por recibir; las órdenes rechazadas o anuladas no cuentan.</summary>
        public int UnidadesPendientes { get; set; }
    }

    /// <summary>Subtotales de un proveedor.</summary>
    public class TotalProveedorCompra_60MN
    {
        public string Proveedor { get; set; } = string.Empty;

        public int Ordenes { get; set; }

        public decimal Comprado { get; set; }

        public decimal Pagado { get; set; }

        public int UnidadesPendientes { get; set; }
    }

    /// <summary>Resultado de CUN15: las órdenes, los subtotales por proveedor y los totales generales.</summary>
    public class ReporteCompras_60MN
    {
        public FiltroReporteCompra_60MN Filtro { get; set; } = new FiltroReporteCompra_60MN();

        public List<FilaReporteCompra_60MN> Filas { get; set; } = new List<FilaReporteCompra_60MN>();

        public List<TotalProveedorCompra_60MN> PorProveedor { get; set; } = new List<TotalProveedorCompra_60MN>();

        /// <summary>Total de las órdenes aprobadas o posteriores (sin rechazadas, anuladas ni pendientes de aprobación).</summary>
        public decimal TotalComprado { get; set; }

        public decimal TotalPagado { get; set; }

        public int UnidadesPendientes { get; set; }
    }
}
