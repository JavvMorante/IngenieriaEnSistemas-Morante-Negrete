using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>Resultado del Reporte de Atenciones para un período y sus filtros.</summary>
    public class ResultadoReporteAtenciones_60MN
    {
        public DateTime Desde { get; set; }

        public DateTime Hasta { get; set; }

        public List<FilaReporteAtencion_60MN> Atenciones { get; set; } = new List<FilaReporteAtencion_60MN>();

        /// <summary>Atenciones por profesional, de mayor a menor.</summary>
        public List<TotalProfesional_60MN> PorProfesional { get; set; } = new List<TotalProfesional_60MN>();

        public bool SinDatos => Atenciones.Count == 0;

        /// <summary>Pacientes distintos atendidos en el período.</summary>
        public int Pacientes => Atenciones.Select(a => a.NumeroHistoriaClinica).Distinct().Count();
    }

    /// <summary>
    /// CUN07 Generar Reporte de Atenciones: atenciones de un período, con filtros por
    /// veterinario y especie, y totales por profesional. Es de solo consulta.
    /// </summary>
    public class ReporteAtencionesBLL_60MN
    {
        private readonly AtencionDAL_60MN mapper = new AtencionDAL_60MN();

        /// <summary>Paso 2: profesionales para el filtro "veterinario".</summary>
        public List<ProfesionalAtencion_60MN> ListarProfesionales()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Reportes);
            return mapper.ListarProfesionales();
        }

        /// <summary>Paso 2: especies para el filtro.</summary>
        public List<string> ListarEspecies()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Reportes);
            return mapper.ListarEspecies();
        }

        public ResultadoReporteAtenciones_60MN Generar(DateTime desde, DateTime hasta, int? usuarioId, string? especie)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Reportes);

            desde = desde.Date;
            hasta = hasta.Date;
            if (desde > hasta)
                throw new NegocioException_60MN("El rango es inválido: la fecha desde es posterior a la fecha hasta.");
            if (desde > DateTime.Today)
                throw new NegocioException_60MN("El rango no puede comenzar en una fecha futura.");
            especie = string.IsNullOrWhiteSpace(especie) ? null : especie!.Trim();

            // El límite superior es exclusivo para incluir todas las horas del día "hasta".
            List<FilaReporteAtencion_60MN> filas = mapper.ListarPorPeriodo(desde, hasta.AddDays(1), usuarioId, especie);
            ResultadoReporteAtenciones_60MN reporte = new ResultadoReporteAtenciones_60MN
            {
                Desde = desde,
                Hasta = hasta,
                Atenciones = filas,
                PorProfesional = filas
                    .GroupBy(f => f.Profesional)
                    .Select(g => new TotalProfesional_60MN
                    {
                        Profesional = g.Key,
                        Atenciones = g.Count(),
                        Pacientes = g.Select(f => f.NumeroHistoriaClinica).Distinct().Count(),
                        Porcentaje = Math.Round(100m * g.Count() / filas.Count, 1)
                    })
                    .OrderByDescending(t => t.Atenciones).ThenBy(t => t.Profesional)
                    .ToList()
            };

            new BitacoraBLL_60MN().Registrar(EventoSistema_60MN.ReporteAtencionesGenerado,
                $"Reporte de atenciones del {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}" +
                (usuarioId.HasValue ? $", profesional Id {usuarioId}" : string.Empty) +
                (especie != null ? $", especie {especie}" : string.Empty) +
                $": {filas.Count} atención(es) de {reporte.Pacientes} paciente(s), {reporte.PorProfesional.Count} profesional(es).");
            return reporte;
        }
    }
}
