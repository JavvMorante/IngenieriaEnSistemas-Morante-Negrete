using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    public class ResultadoMovimiento_60MN
    {
        public int StockResultante { get; set; }

        /// <summary>Se generó una alerta de reposición nueva (paso 7).</summary>
        public bool AlertaGenerada { get; set; }

        /// <summary>El producto quedó en o debajo del mínimo pero ya tenía una alerta activa (flujo 7.1).</summary>
        public bool AlertaExistente { get; set; }

        public bool LoteDadoDeBaja { get; set; }
    }

    /// <summary>CUN14 Registrar Movimiento de Stock.</summary>
    public class MovimientoStockBLL_60MN
    {
        private readonly MovimientoStockDAL_60MN mapper = new MovimientoStockDAL_60MN();
        private readonly ProductoDAL_60MN productos = new ProductoDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Largo máximo del motivo (deja lugar al prefijo "Vencimiento del lote: " en la columna de 200).</summary>
        public const int LargoMotivo = 150;

        /// <summary>CUN14 paso 4: el motivo es obligatorio y descriptivo. Devuelve el error o null si es válido.</summary>
        public static string? ErrorMotivo(string? motivo)
        {
            motivo = Validaciones_60MN.Normalizar(motivo);
            if (motivo.Length == 0)
                return "Debe indicar el motivo del movimiento.";
            try
            {
                Validaciones_60MN.Largo(motivo, LargoMotivo, "El motivo");
                Validaciones_60MN.TextoLibre(motivo, "El motivo");
                return null;
            }
            catch (NegocioException_60MN ex)
            {
                return ex.Message;
            }
        }

        /// <param name="cantidad">Cantidad ingresada por el actor (siempre positiva).</param>
        /// <param name="restar">Solo para ajustes: true si el ajuste descuenta stock.</param>
        /// <param name="ajustePorVencimiento">Flujo 4.2: da de baja el lote completo y lo excluye de la venta.</param>
        public ResultadoMovimiento_60MN Registrar(int productoId, TipoMovimiento_60MN tipo, int cantidad, bool restar,
                                                  string motivo, bool ajustePorVencimiento)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.MovimientoStock);

            string? errorMotivo = ErrorMotivo(motivo);
            if (errorMotivo != null)
                throw new NegocioException_60MN(errorMotivo);
            motivo = Validaciones_60MN.Normalizar(motivo);

            Producto_60MN producto = productos.ObtenerPorId(productoId) ?? throw new NegocioException_60MN("El producto no existe.");
            if (!producto.Activo)
                throw new NegocioException_60MN("El producto está dado de baja: no admite movimientos.");

            int variacion;
            ajustePorVencimiento = ajustePorVencimiento && tipo == TipoMovimiento_60MN.Ajuste;
            if (ajustePorVencimiento)
            {
                variacion = -producto.StockActual;
            }
            else
            {
                if (cantidad <= 0)
                    throw new NegocioException_60MN("La cantidad debe ser mayor a cero.");
                variacion = tipo switch
                {
                    TipoMovimiento_60MN.Ingreso => cantidad,
                    TipoMovimiento_60MN.Egreso => -cantidad,
                    _ => restar ? -cantidad : cantidad
                };
                // Flujo 4.1: no se puede egresar más de lo disponible.
                if (producto.StockActual + variacion < 0)
                    throw new NegocioException_60MN(
                        $"La cantidad solicitada ({cantidad}) supera el stock disponible ({producto.StockActual}).");
            }

            MovimientoStock_60MN movimiento = new MovimientoStock_60MN
            {
                ProductoId = productoId,
                Tipo = tipo,
                Cantidad = variacion,
                StockAnterior = producto.StockActual,
                StockResultante = producto.StockActual + variacion,
                Motivo = ajustePorVencimiento ? "Vencimiento del lote: " + motivo : motivo,
                FechaHora = DateTime.Now,
                UsuarioId = SesionActual_60MN.UsuarioId
            };

            movimiento.Id = mapper.Registrar(movimiento, darDeBajaProducto: ajustePorVencimiento);
            if (movimiento.Id == 0)
                throw new NegocioException_60MN("El stock del producto fue modificado por otro usuario. Vuelva a intentarlo.");

            dv.ActualizarFila(TablaDV_60MN.MovimientoStock, movimiento.Id);
            dv.ActualizarFila(TablaDV_60MN.Productos, productoId);

            bitacora.Registrar(EventoSistema_60MN.MovimientoStock,
                $"{tipo} de {variacion:+#;-#;0} unidades de {producto.Codigo} \"{producto.Nombre}\" (stock {movimiento.StockAnterior} → {movimiento.StockResultante}). Motivo: {movimiento.Motivo}");

            producto.StockActual = movimiento.StockResultante;
            producto.Activo = !ajustePorVencimiento;
            if (ajustePorVencimiento)
                bitacora.Registrar(EventoSistema_60MN.ProductoBaja, $"Baja del lote {producto.Lote} de {producto.Codigo} por vencimiento.");

            bool yaTeniaAlerta = producto.Activo && producto.BajoStockMinimo && mapper.ExisteAlertaActiva(productoId);
            bool alertaGenerada = new ProductoBLL_60MN().EvaluarAlerta(producto);

            return new ResultadoMovimiento_60MN
            {
                StockResultante = movimiento.StockResultante,
                AlertaGenerada = alertaGenerada,
                AlertaExistente = yaTeniaAlerta,
                LoteDadoDeBaja = ajustePorVencimiento
            };
        }

        public List<MovimientoStock_60MN> ListarPorProducto(int productoId) => mapper.ListarPorProducto(productoId);
    }
}
