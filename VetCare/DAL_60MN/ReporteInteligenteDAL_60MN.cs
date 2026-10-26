using Entidades_60MN;

namespace DAL_60MN
{
    /// <summary>
    /// Consultas del Reporte Inteligente. Son de solo lectura: no modifican ninguna
    /// tabla y por lo tanto no intervienen en los dígitos verificadores.
    /// </summary>
    public class ReporteInteligenteDAL_60MN
    {
        /// <summary>
        /// Unidades que salieron del stock, agrupadas por mes y categoría. Se toman los
        /// movimientos de tipo EGRESO, cuya Cantidad se guarda con signo negativo, y se
        /// devuelven en positivo para poder graficarlas.
        /// </summary>
        public List<DemandaMensual_60MN> DemandaPorMesYCategoria(DateTime desde, DateTime hasta)
        {
            const string sql = @"
SELECT YEAR(m.FechaHora) AS Anio, MONTH(m.FechaHora) AS Mes, c.CategoriaID, c.Nombre AS Categoria,
       SUM(-m.Cantidad) AS Cantidad
  FROM MovimientoStock m
  JOIN Productos p ON p.ProductoID  = m.ProductoID
  JOIN Categoria c ON c.CategoriaID = p.CategoriaID
 WHERE m.Tipo = 'EGRESO'
   AND m.FechaHora >= @desde
   AND m.FechaHora <  @hasta
 GROUP BY YEAR(m.FechaHora), MONTH(m.FechaHora), c.CategoriaID, c.Nombre
HAVING SUM(-m.Cantidad) > 0
 ORDER BY YEAR(m.FechaHora), MONTH(m.FechaHora), c.Nombre";

            return Conexion_60MN.EjecutarLista(sql, r => new DemandaMensual_60MN
            {
                Anio = r.GetInt32(0),
                Mes = r.GetInt32(1),
                CategoriaId = r.GetInt32(2),
                Categoria = r.GetString(3),
                Cantidad = r.GetInt32(4)
            }, ("@desde", desde), ("@hasta", hasta));
        }

        /// <summary>
        /// Productos entregados en atenciones dentro del período (CUN09), ordenados por
        /// cantidad descendente. Se consulta AtencionDetalle y no MovimientoStock porque
        /// aquí interesa el consumo clínico real y no los ingresos ni los ajustes.
        /// </summary>
        public List<ProductoEntregado_60MN> TopProductosEntregados(DateTime desde, DateTime hasta, int top)
        {
            const string sql = @"
SELECT TOP (@top) p.ProductoID, p.Codigo, p.Nombre, c.Nombre AS Categoria,
       SUM(d.Cantidad)                    AS Cantidad,
       SUM(d.Cantidad * d.PrecioUnitario) AS Importe,
       COUNT(DISTINCT d.AtencionID)       AS Atenciones
  FROM AtencionDetalle d
  JOIN Atencion  a ON a.AtencionID  = d.AtencionID
  JOIN Productos p ON p.ProductoID  = d.ProductoID
  JOIN Categoria c ON c.CategoriaID = p.CategoriaID
 WHERE a.FechaHora >= @desde
   AND a.FechaHora <  @hasta
 GROUP BY p.ProductoID, p.Codigo, p.Nombre, c.Nombre
 ORDER BY SUM(d.Cantidad) DESC, p.Nombre";

            return Conexion_60MN.EjecutarLista(sql, r => new ProductoEntregado_60MN
            {
                ProductoId = r.GetInt32(0),
                Codigo = r.GetString(1),
                Nombre = r.GetString(2),
                Categoria = r.GetString(3),
                Cantidad = r.GetInt32(4),
                Importe = r.GetDecimal(5),
                Atenciones = r.GetInt32(6)
            }, ("@top", top), ("@desde", desde), ("@hasta", hasta));
        }
    }
}
