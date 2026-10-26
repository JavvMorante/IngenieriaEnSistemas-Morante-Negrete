using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>Resultado completo del Reporte Inteligente para un período.</summary>
    public class ResultadoReporteInteligente_60MN
    {
        public DateTime Desde { get; set; }

        public DateTime Hasta { get; set; }

        /// <summary>Demanda estacional: unidades egresadas por mes y categoría (RF2).</summary>
        public List<DemandaMensual_60MN> Demanda { get; set; } = new List<DemandaMensual_60MN>();

        /// <summary>Ranking de productos entregados en atenciones (RF1 + RF2).</summary>
        public List<ProductoEntregado_60MN> TopProductos { get; set; } = new List<ProductoEntregado_60MN>();

        public bool SinDatos => Demanda.Count == 0 && TopProductos.Count == 0;

        public int UnidadesEgresadas => Demanda.Sum(d => d.Cantidad);

        public int UnidadesEntregadas => TopProductos.Sum(p => p.Cantidad);

        public decimal ImporteEntregado => TopProductos.Sum(p => p.Importe);

        /// <summary>Categorías presentes en la demanda, en orden alfabético (series del gráfico).</summary>
        public List<string> Categorias =>
            Demanda.Select(d => d.Categoria).Distinct().OrderBy(n => n).ToList();

        /// <summary>Períodos presentes en la demanda, en orden cronológico (eje X del gráfico).</summary>
        public List<string> Periodos =>
            Demanda.OrderBy(d => d.ClavePeriodo).Select(d => d.Periodo).Distinct().ToList();

        /// <summary>Unidades de una categoría en un período; cero si esa combinación no tuvo egresos.</summary>
        public int Cantidad(string periodo, string categoria) =>
            Demanda.Where(d => d.Periodo == periodo && d.Categoria == categoria).Sum(d => d.Cantidad);
    }

    /// <summary>
    /// Reporte Inteligente. Cruza los dos requerimientos funcionales del trabajo:
    /// la demanda estacional de insumos por categoría, que sale de los movimientos de
    /// stock (RF2), y el ranking de productos entregados en atenciones, que sale del
    /// detalle de las atenciones (RF1). Es de solo consulta.
    /// </summary>
    public class ReporteInteligenteBLL_60MN
    {
        /// <summary>Cantidad de productos del ranking cuando no se indica otra.</summary>
        public const int TopPorDefecto = 10;

        /// <summary>Tope del ranking: más de esto no entra legible en el gráfico.</summary>
        public const int TopMaximo = 20;

        /// <summary>Período máximo admitido, para que el eje del gráfico siga siendo legible.</summary>
        public const int MesesMaximos = 36;

        private readonly ReporteInteligenteDAL_60MN mapper = new ReporteInteligenteDAL_60MN();

        public ResultadoReporteInteligente_60MN Generar(DateTime desde, DateTime hasta, int top = TopPorDefecto)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Reportes);

            desde = desde.Date;
            hasta = hasta.Date;

            if (desde > hasta)
                throw new NegocioException_60MN("El rango es inválido: la fecha desde es posterior a la fecha hasta.");
            if (desde > DateTime.Today)
                throw new NegocioException_60MN("El rango no puede comenzar en una fecha futura.");
            if (MesesEntre(desde, hasta) > MesesMaximos)
                throw new NegocioException_60MN(
                    $"El período abarca más de {MesesMaximos} meses y el gráfico quedaría ilegible. Acote el rango.");

            if (top < 1) top = TopPorDefecto;
            if (top > TopMaximo) top = TopMaximo;

            // El límite superior es exclusivo para incluir todas las horas del día "hasta".
            DateTime limite = hasta.AddDays(1);

            ResultadoReporteInteligente_60MN reporte = new ResultadoReporteInteligente_60MN
            {
                Desde = desde,
                Hasta = hasta,
                Demanda = mapper.DemandaPorMesYCategoria(desde, limite),
                TopProductos = mapper.TopProductosEntregados(desde, limite, top)
            };

            new BitacoraBLL_60MN().Registrar(EventoSistema_60MN.ReporteInteligenteGenerado,
                $"Reporte inteligente del {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}: " +
                $"{reporte.UnidadesEgresadas} unidad(es) egresada(s) en {reporte.Demanda.Count} combinación(es) " +
                $"de mes y categoría; top {reporte.TopProductos.Count} producto(s) entregado(s) en atenciones.");

            return reporte;
        }

        /// <summary>Cantidad de meses que abarca el rango, contando el primero y el último.</summary>
        private static int MesesEntre(DateTime desde, DateTime hasta) =>
            (hasta.Year - desde.Year) * 12 + hasta.Month - desde.Month + 1;
    }
}
