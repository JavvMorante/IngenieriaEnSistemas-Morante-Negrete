namespace Entidades_60MN
{
    /// <summary>Parámetros del módulo de stock (CUN13): margen general y stock mínimo sugerido.</summary>
    public class ConfiguracionStock_60MN
    {
        /// <summary>Margen de ganancia general en % (por ejemplo 30).</summary>
        public decimal MargenGeneral { get; set; }

        /// <summary>Stock mínimo que se propone al registrar un producto.</summary>
        public int StockMinimoSugerido { get; set; }

        /// <summary>Margen particular por categoría (null = usa el margen general).</summary>
        public List<Categoria_60MN> Categorias { get; set; } = new List<Categoria_60MN>();
    }
}
