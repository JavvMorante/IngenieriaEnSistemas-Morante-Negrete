using Interfaces_60MN;

namespace Entidades_60MN
{
    public class Idioma_60MN : IIdioma_60MN
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Codigo { get; set; } = string.Empty;

        /// <summary>Idioma base (español): sus textos son los del diseñador. Es el idioma del Login.</summary>
        public bool EsBase { get; set; }

        public override string ToString() => Nombre;
    }
}
