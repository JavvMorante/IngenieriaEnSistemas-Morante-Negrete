using Interfaces_60MN;

namespace Entidades_60MN
{
    /// <summary>Consulta médica registrada en la historia clínica (CUN08).</summary>
    public class Atencion_60MN : IEntity_60MN
    {
        public int Id { get; set; }

        public int HistoriaClinicaId { get; set; }

        public DateTime FechaHora { get; set; }

        public int? UsuarioId { get; set; }

        /// <summary>Profesional de la sesión activa que registró la atención.</summary>
        public string Profesional { get; set; } = string.Empty;

        public decimal Peso { get; set; }

        /// <summary>Temperatura en °C.</summary>
        public decimal Temperatura { get; set; }

        /// <summary>Frecuencia cardíaca en latidos por minuto.</summary>
        public int FrecuenciaCardiaca { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public string Diagnostico { get; set; } = string.Empty;

        /// <summary>
        /// Indicaciones o tratamiento (qué comprar, dosis, frecuencia). Opcional: la atención puede
        /// cerrarse sin medicación; lo que efectivamente se entrega va en Medicacion (CUN09).
        /// </summary>
        public string? Indicaciones { get; set; }

        public bool TieneIndicaciones => !string.IsNullOrWhiteSpace(Indicaciones);

        /// <summary>Cantidad de veces que se emitió el informe (CUN10, reimpresión 1.1).</summary>
        public int InformesEmitidos { get; set; }

        /// <summary>Medicación entregada: los ítems de las ventas asociadas a la atención (CUN05).</summary>
        public List<ItemCarrito_60MN> Medicacion { get; } = new List<ItemCarrito_60MN>();

        // Contexto para listados e informes
        /// <summary>Dueño de la mascota: cliente de la venta de la medicación.</summary>
        public int ClienteId { get; set; }

        public string NumeroHistoriaClinica { get; set; } = string.Empty;

        public string Paciente { get; set; } = string.Empty;

        public string Propietario { get; set; } = string.Empty;

        public string Descripcion => $"{FechaHora:dd/MM/yyyy HH:mm} - {Paciente} ({Propietario}) - {Motivo}";

        public override string ToString() => Descripcion;
    }
}
