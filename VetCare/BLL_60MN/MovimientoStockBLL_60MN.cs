using DAL_60MN;
using Entidades_60MN;
using Servicios_60MN.Composite;

namespace BLL_60MN
{
    /// <summary>
    /// Movimientos de stock (tabla MovimientoStock). Desde V2.5 no se registran a mano: los EGRESO
    /// los genera la venta (CUN05, VentaDAL_60MN) y los INGRESO la recepción de mercadería
    /// (CUN14, RecepcionDAL_60MN), cada uno dentro de la transacción de su caso de uso.
    /// </summary>
    public class MovimientoStockBLL_60MN
    {
        private readonly MovimientoStockDAL_60MN mapper = new MovimientoStockDAL_60MN();

        /// <summary>Movimientos de un producto, del más reciente al más antiguo.</summary>
        public List<MovimientoStock_60MN> ListarPorProducto(int productoId)
        {
            SesionActual_60MN.RequiereAlgunPermiso(CodigosPatente_60MN.ProductoAlta, CodigosPatente_60MN.ProductoModificar,
                CodigosPatente_60MN.Recepcion);
            return mapper.ListarPorProducto(productoId);
        }
    }
}
