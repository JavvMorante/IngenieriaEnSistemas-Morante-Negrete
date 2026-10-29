using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Venta registrada al confirmar el carrito (CUN05). El sistema no factura.</summary>
    public class VentaEncabezado_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        /// <summary>Atención a la que corresponde la medicación (null: venta sin atención).</summary>
        public int? AtencionId { get; set; }

        public DateTime FechaHora { get; set; }

        public int? UsuarioId { get; set; }

        public decimal Total { get; set; }

        public List<VentaDetalle_60MN> Detalle { get; } = new List<VentaDetalle_60MN>();

        // Contexto para la consulta de ventas
        public string Cliente { get; set; } = string.Empty;

        public string DocumentoCliente { get; set; } = string.Empty;

        /// <summary>Paciente de la atención, o vacío si la venta no tiene atención.</summary>
        public string Paciente { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public string Numero => $"V-{Id:00000}";

        public string AtencionTexto => AtencionId.HasValue ? $"Nº {AtencionId} ({Paciente})" : "-";

        public int CantidadItems => Detalle.Sum(d => d.Cantidad);
    }
}
