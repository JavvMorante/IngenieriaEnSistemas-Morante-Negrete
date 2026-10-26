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

        /// <summary>Cantidad de veces que se emitió el informe (CUN10, reimpresión 1.1).</summary>
        public int InformesEmitidos { get; set; }

        /// <summary>Medicación entregada en el carrito (CUN09).</summary>
        public List<ItemCarrito_60MN> Medicacion { get; } = new List<ItemCarrito_60MN>();

        // Contexto para listados e informes
        public string NumeroHistoriaClinica { get; set; } = string.Empty;

        public string Paciente { get; set; } = string.Empty;

        public string Propietario { get; set; } = string.Empty;

        public string Descripcion => $"{FechaHora:dd/MM/yyyy HH:mm} - {Paciente} ({Propietario}) - {Motivo}";

        public override string ToString() => Descripcion;
    }

    /// <summary>Producto entregado en una atención (fila de AtencionDetalle, CUN09).</summary>
    public class ItemCarrito_60MN
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Producto { get; set; } = string.Empty;

        public string Lote { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
