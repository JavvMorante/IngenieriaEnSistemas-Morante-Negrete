namespace Entidades_60MN
{
    /// <summary>
    /// Unidades que salieron del stock de una categoría en un mes. Alimenta el gráfico
    /// de demanda estacional del Reporte Inteligente (RF2 Gestión de Stock).
    /// </summary>
    public class DemandaMensual_60MN
    {
        public int Anio { get; set; }

        public int Mes { get; set; }

        public int CategoriaId { get; set; }

        public string Categoria { get; set; } = string.Empty;

        /// <summary>Unidades egresadas, siempre en positivo.</summary>
        public int Cantidad { get; set; }

        /// <summary>Etiqueta del período, por ejemplo "03/2026".</summary>
        public string Periodo => $"{Mes:00}/{Anio}";

        /// <summary>Clave ordenable del período (año * 100 + mes).</summary>
        public int ClavePeriodo => Anio * 100 + Mes;

        public override string ToString() => $"{Periodo} · {Categoria}: {Cantidad}";
    }

    /// <summary>
    /// Producto entregado en atenciones dentro del período. Alimenta el ranking del
    /// Reporte Inteligente, que cruza RF1 (Gestión de Pacientes) con RF2 (Gestión de Stock).
    /// </summary>
    public class ProductoEntregado_60MN
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        /// <summary>Unidades entregadas en el período.</summary>
        public int Cantidad { get; set; }

        /// <summary>Importe total al precio de venta del momento de la entrega.</summary>
        public decimal Importe { get; set; }

        /// <summary>Cantidad de atenciones distintas en las que se entregó el producto.</summary>
        public int Atenciones { get; set; }

        public override string ToString() => $"{Nombre} ({Cantidad})";
    }
}
