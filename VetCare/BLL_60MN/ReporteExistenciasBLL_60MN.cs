using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    public enum TipoReporteExistencias_60MN
    {
        ExistenciasGenerales,
        BajoStockMinimo,
        ProximosAVencer
    }

    /// <summary>CUN15 Generar Reporte de Existencias.</summary>
    public class ReporteExistenciasBLL_60MN
    {
        /// <summary>Horizonte por defecto de "próximos a vencer" cuando no se indica una fecha hasta.</summary>
        public const int DiasProximosAVencer = 60;

        public List<Producto_60MN> Generar(TipoReporteExistencias_60MN tipo, int? categoriaId, int? proveedorId,
                                           DateTime? venceDesde, DateTime? venceHasta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ReporteExistencias);
            // 3.1: rango de fechas inválido.
            if (venceDesde.HasValue && venceHasta.HasValue && venceDesde.Value.Date > venceHasta.Value.Date)
                throw new NegocioException_60MN("El rango de vencimiento es inválido: la fecha desde es posterior a la fecha hasta.");

            if (tipo == TipoReporteExistencias_60MN.ProximosAVencer && !venceHasta.HasValue)
                venceHasta = DateTime.Today.AddDays(DiasProximosAVencer);

            List<Producto_60MN> resultado = new ProductoDAL_60MN().ReporteExistencias(
                tipo == TipoReporteExistencias_60MN.BajoStockMinimo, categoriaId, proveedorId, venceDesde, venceHasta);

            new BitacoraBLL_60MN().Registrar(EventoSistema_60MN.ReporteExistenciasGenerado,
                $"Reporte de existencias ({tipo}): {resultado.Count} producto(s), valorización $ {resultado.Sum(p => p.Valorizacion):N2}.");
            return resultado;
        }
    }
}
