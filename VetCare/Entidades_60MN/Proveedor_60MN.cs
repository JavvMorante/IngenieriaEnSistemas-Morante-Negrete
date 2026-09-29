using Interfaces_60MN;

namespace Entidades_60MN
{
    public class Proveedor_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public string RazonSocial { get; set; } = string.Empty;

        public string Cuit { get; set; } = string.Empty;

        public override string ToString() => RazonSocial;
    }
}
