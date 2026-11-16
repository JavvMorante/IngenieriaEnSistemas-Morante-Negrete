using System.Globalization;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>CUN15 Generar Reporte de Compras (gerente o analista de compras).</summary>
    public class ReporteComprasBLL_60MN
    {
        /// <summary>Estados que cuentan como compra comprometida (aprobada o posterior).</summary>
        private static readonly EstadoOrden_60MN[] Comprometidas =
        {
            EstadoOrden_60MN.Aprobada, EstadoOrden_60MN.Enviada, EstadoOrden_60MN.Pagada,
            EstadoOrden_60MN.RecibidaParcial, EstadoOrden_60MN.Cerrada
        };

        private readonly ReporteComprasDAL_60MN mapper = new ReporteComprasDAL_60MN();
        private readonly CatalogoStockDAL_60MN catalogo = new CatalogoStockDAL_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Todos los proveedores (también los dados de baja) para el filtro.</summary>
        public List<Proveedor_60MN> ListarProveedores()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ReporteCompras);
            return catalogo.ListarProveedores(soloActivos: false);
        }

        public ReporteCompras_60MN Generar(FiltroReporteCompra_60MN filtro)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ReporteCompras);
            if (filtro.Desde.Date > filtro.Hasta.Date)
                throw new NegocioException_60MN("La fecha desde no puede ser posterior a la fecha hasta.");

            // Límite superior exclusivo: incluye todo el día "hasta".
            List<FilaReporteCompra_60MN> filas = mapper.Consultar(filtro.Desde.Date, filtro.Hasta.Date.AddDays(1), filtro.ProveedorId, filtro.Estado);
            ReporteCompras_60MN reporte = new ReporteCompras_60MN
            {
                Filtro = filtro,
                Filas = filas,
                PorProveedor = filas.GroupBy(f => f.Proveedor)
                    .Select(g => new TotalProveedorCompra_60MN
                    {
                        Proveedor = g.Key,
                        Ordenes = g.Count(),
                        Comprado = g.Where(f => Comprometidas.Contains(f.Estado)).Sum(f => f.Total),
                        Pagado = g.Sum(f => f.ImportePagado),
                        UnidadesPendientes = g.Sum(f => f.UnidadesPendientes)
                    })
                    .OrderByDescending(t => t.Comprado).ThenBy(t => t.Proveedor)
                    .ToList(),
                TotalComprado = filas.Where(f => Comprometidas.Contains(f.Estado)).Sum(f => f.Total),
                TotalPagado = filas.Sum(f => f.ImportePagado),
                UnidadesPendientes = filas.Sum(f => f.UnidadesPendientes)
            };

            bitacora.Registrar(EventoSistema_60MN.ReporteComprasGenerado,
                $"Reporte de compras del {filtro.Desde:dd/MM/yyyy} al {filtro.Hasta:dd/MM/yyyy}" +
                (filtro.ProveedorId.HasValue ? $", proveedor Id {filtro.ProveedorId}" : string.Empty) +
                (filtro.Estado.HasValue ? $", estado {filtro.Estado.Value.Descripcion()}" : string.Empty) +
                $": {filas.Count} orden(es), comprado $ {reporte.TotalComprado.ToString("N2", CultureInfo.GetCultureInfo("es-AR"))}.");
            return reporte;
        }
    }
}
