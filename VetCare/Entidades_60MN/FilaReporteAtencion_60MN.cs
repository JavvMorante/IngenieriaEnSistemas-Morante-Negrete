namespace Entidades_60MN
{
    /// <summary>Fila del Reporte de Atenciones (CUN07): una atención del período con su contexto.</summary>
    public class FilaReporteAtencion_60MN
    {
        public int AtencionId { get; set; }

        public DateTime FechaHora { get; set; }

        public string NumeroHistoriaClinica { get; set; } = string.Empty;

        public string Paciente { get; set; } = string.Empty;

        public string Especie { get; set; } = string.Empty;

        public string Propietario { get; set; } = string.Empty;

        public int? UsuarioId { get; set; }

        public string Profesional { get; set; } = string.Empty;

        public string Motivo { get; set; } = string.Empty;
    }

    /// <summary>Totales del Reporte de Atenciones por profesional.</summary>
    public class TotalProfesional_60MN
    {
        public string Profesional { get; set; } = string.Empty;

        public int Atenciones { get; set; }

        /// <summary>Pacientes distintos atendidos.</summary>
        public int Pacientes { get; set; }

        public decimal Porcentaje { get; set; }
    }

    /// <summary>Profesional que registró atenciones (filtro "veterinario" del reporte).</summary>
    public class ProfesionalAtencion_60MN
    {
        public int UsuarioId { get; set; }

        public string Profesional { get; set; } = string.Empty;

        public override string ToString() => Profesional;
    }
}
