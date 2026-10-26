using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Paciente de la veterinaria, registrado a nombre de un cliente (CUN04).</summary>
    public class Mascota_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Especie { get; set; } = string.Empty;

        public string Raza { get; set; } = string.Empty;

        /// <summary>"M" (macho) o "H" (hembra).</summary>
        public string Sexo { get; set; } = "M";

        public DateTime FechaNacimiento { get; set; } = DateTime.Today;

        public bool Activo { get; set; } = true;

        /// <summary>Número de historia clínica (HC-00001) o vacío si todavía no tiene.</summary>
        public string NumeroHistoriaClinica { get; set; } = string.Empty;

        public string SexoTexto => Sexo == "H" ? "Hembra" : "Macho";

        public string Estado => Activo ? "Activa" : "Baja";

        public bool TieneHistoriaClinica => NumeroHistoriaClinica.Length > 0;

        public override string ToString() => $"{Nombre} ({Especie})";
    }
}
