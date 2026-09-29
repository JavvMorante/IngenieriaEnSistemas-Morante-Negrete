namespace Interfaces_60MN
{
    /// <summary>Idioma disponible en el sistema.</summary>
    public interface IIdioma_60MN : IEntity_60MN
    {
        string Nombre { get; set; }

        /// <summary>Código ISO (es, en...).</summary>
        string Codigo { get; set; }

        /// <summary>
        /// El idioma base es aquel en el que están escritos los textos del
        /// diseñador (español): no necesita filas en la tabla Traduccion.
        /// </summary>
        bool EsBase { get; set; }
    }
}
