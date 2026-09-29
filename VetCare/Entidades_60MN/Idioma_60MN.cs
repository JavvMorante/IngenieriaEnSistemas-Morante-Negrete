using Interfaces_60MN;

namespace Entidades_60MN
{
    public class Idioma_60MN : IIdioma_60MN
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Codigo { get; set; } = string.Empty;

        public bool EsBase { get; set; }

        /// <summary>Idioma con el que arranca la pantalla de login (el último elegido).</summary>
        public bool Predeterminado { get; set; }

        public override string ToString() => Nombre;
    }
}
