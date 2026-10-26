using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Propietario de una o varias mascotas (CUN01 a CUN03).</summary>
    public class Cliente_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        /// <summary>DNI, LE, LC, CI o PAS. Junto con el número identifica al cliente (no se modifica, CUN02 5.1).</summary>
        public string TipoDocumento { get; set; } = "DNI";

        public string NroDocumento { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string? Email { get; set; }

        public bool Activo { get; set; } = true;

        /// <summary>Mascotas del cliente (relación uno a varios).</summary>
        public List<Mascota_60MN> Mascotas { get; } = new List<Mascota_60MN>();

        public string Documento => $"{TipoDocumento} {NroDocumento}";

        public string NombreCompleto => $"{Apellido}, {Nombre}";

        public string Estado => Activo ? "Activo" : "Baja";

        public override string ToString() => $"{NombreCompleto} ({Documento})";
    }
}
