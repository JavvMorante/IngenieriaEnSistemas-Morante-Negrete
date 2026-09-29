using Interfaces_60MN;

namespace Entidades_60MN
{
    public class Categoria_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        /// <summary>Margen de ganancia particular de la categoría (%). Null = usa el margen general.</summary>
        public decimal? Margen { get; set; }

        public override string ToString() => Nombre;
    }
}
