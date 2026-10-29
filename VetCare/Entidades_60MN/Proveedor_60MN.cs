using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Proveedor de productos (CUN11 Gestionar Proveedores).</summary>
    public class Proveedor_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public string RazonSocial { get; set; } = string.Empty;

        /// <summary>CUIT con sus 11 dígitos, sin guiones (no se modifica después del alta).</summary>
        public string Cuit { get; set; } = string.Empty;

        /// <summary>Teléfono solo con dígitos.</summary>
        public string Telefono { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string Direccion { get; set; } = string.Empty;

        /// <summary>Persona de contacto (opcional).</summary>
        public string? Contacto { get; set; }

        public bool Activo { get; set; } = true;

        /// <summary>Cantidad de productos activos que provee (solo la completa la búsqueda).</summary>
        public int ProductosActivos { get; set; }

        /// <summary>CUIT con guiones: 30-71234567-1.</summary>
        public string CuitFormateado => Cuit.Length == 11 ? $"{Cuit.Substring(0, 2)}-{Cuit.Substring(2, 8)}-{Cuit.Substring(10)}" : Cuit;

        public string Estado => Activo ? "Activo" : "Baja";

        public override string ToString() => RazonSocial;
    }
}
