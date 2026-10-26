using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Legajo médico de una mascota (CUN05 a CUN07). Una sola historia por mascota.</summary>
    public class HistoriaClinica_60MN : IEntity_60MN
    {
        public const string SinAlergias = "Sin alergias registradas";

        public int Id { get; set; }

        /// <summary>Número generado por la base (columna calculada HC-00001).</summary>
        public string Numero { get; set; } = string.Empty;

        public int MascotaId { get; set; }

        public DateTime FechaApertura { get; set; }

        public decimal PesoActual { get; set; }

        public string Alergias { get; set; } = SinAlergias;

        public string? Observaciones { get; set; }

        /// <summary>Datos del paciente y del propietario para la cabecera del legajo.</summary>
        public Mascota_60MN Mascota { get; set; } = new Mascota_60MN();

        public Cliente_60MN Propietario { get; set; } = new Cliente_60MN();

        public bool TieneAlergias => !string.Equals(Alergias, SinAlergias, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Valores anteriores de una historia clínica, para el seguimiento evolutivo (CUN07 paso 6).</summary>
    public class HistoriaClinicaHistorial_60MN
    {
        public DateTime FechaHora { get; set; }

        public decimal PesoActual { get; set; }

        public string Alergias { get; set; } = string.Empty;

        public string? Observaciones { get; set; }

        /// <summary>MODIFICACION (CUN07) o ATENCION (el peso lo actualizó CUN08).</summary>
        public string Origen { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;
    }
}
