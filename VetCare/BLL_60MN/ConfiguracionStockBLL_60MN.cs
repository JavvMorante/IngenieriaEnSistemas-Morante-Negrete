using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>Impacto de una configuración antes de confirmarla (CUN13, flujos 4.1 y 6.1).</summary>
    public class ImpactoConfiguracion_60MN
    {
        /// <summary>Productos activos cuyo precio de venta cambiaría al recalcular.</summary>
        public int ProductosARecalcular { get; set; }

        /// <summary>Productos que quedarían en o por debajo del nuevo stock mínimo y hoy no lo están.</summary>
        public int AlertasNuevas { get; set; }
    }

    /// <summary>CUN13 Configurar Margen y Alerta de Stock Mínimo.</summary>
    public class ConfiguracionStockBLL_60MN
    {
        private readonly CatalogoStockDAL_60MN catalogo = new CatalogoStockDAL_60MN();
        private readonly ProductoDAL_60MN productos = new ProductoDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Paso 2: margen general vigente, stock mínimo sugerido y márgenes por categoría.</summary>
        public ConfiguracionStock_60MN Obtener()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ConfiguracionStock);
            return new ConfiguracionStock_60MN
            {
                MargenGeneral = catalogo.ObtenerMargenGeneral(),
                StockMinimoSugerido = catalogo.ObtenerStockMinimoSugerido(),
                Categorias = catalogo.ListarCategorias()
            };
        }

        /// <summary>Margen fuera de 0 a 100%: la GUI pide confirmación explícita (flujo 3.1).</summary>
        public static bool MargenRequiereConfirmacion(decimal margen) => margen < 0 || margen > 100;

        /// <summary>Calcula cuántos productos cambiarían de precio y cuántas alertas se dispararían, sin guardar nada.</summary>
        public ImpactoConfiguracion_60MN Simular(ConfiguracionStock_60MN config, bool recalcularPrecios, bool aplicarStockMinimo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ConfiguracionStock);
            List<Producto_60MN> activos = productos.Buscar(null, null, incluirInactivos: false);
            return new ImpactoConfiguracion_60MN
            {
                ProductosARecalcular = recalcularPrecios ? activos.Count(p => NuevoPrecio(p, config) != p.PrecioVenta) : 0,
                AlertasNuevas = aplicarStockMinimo
                    ? activos.Count(p => p.StockActual <= config.StockMinimoSugerido && p.StockActual > p.StockMinimo)
                    : 0
            };
        }

        /// <summary>
        /// Paso 6: guarda los parámetros y, si se pide, recalcula los precios de venta de
        /// los productos alcanzados y aplica el stock mínimo sugerido. Cada producto
        /// modificado actualiza su DVH y queda registrado por el trigger en Productos_C.
        /// </summary>
        public ImpactoConfiguracion_60MN Guardar(ConfiguracionStock_60MN config, bool recalcularPrecios, bool aplicarStockMinimo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ConfiguracionStock);
            if (config.MargenGeneral < -100 || config.MargenGeneral > 1000)
                throw new NegocioException_60MN("El margen general debe estar entre -100% y 1000%.");
            if (config.Categorias.Any(c => c.Margen.HasValue && (c.Margen < -100 || c.Margen > 1000)))
                throw new NegocioException_60MN("Los márgenes por categoría deben estar entre -100% y 1000%.");
            if (config.StockMinimoSugerido < 0)
                throw new NegocioException_60MN("El stock mínimo sugerido no puede ser negativo.");

            ImpactoConfiguracion_60MN impacto = Simular(config, recalcularPrecios, aplicarStockMinimo);
            catalogo.GuardarConfiguracion(config.MargenGeneral, config.StockMinimoSugerido, config.Categorias);

            int usuario = SesionActual_60MN.UsuarioId ?? 0;
            ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();
            foreach (Producto_60MN p in productos.Buscar(null, null, incluirInactivos: false))
            {
                bool cambio = false;
                decimal nuevoPrecio = NuevoPrecio(p, config);
                if (recalcularPrecios && nuevoPrecio != p.PrecioVenta)
                {
                    productos.ActualizarPrecioVenta(p.Id, nuevoPrecio, usuario);
                    cambio = true;
                }
                if (aplicarStockMinimo && p.StockMinimo != config.StockMinimoSugerido)
                {
                    productos.ActualizarStockMinimo(p.Id, config.StockMinimoSugerido, usuario);
                    p.StockMinimo = config.StockMinimoSugerido;
                    cambio = true;
                }
                if (cambio)
                {
                    dv.ActualizarFila(TablaDV_60MN.Productos, p.Id);
                    if (aplicarStockMinimo)
                        productoBLL.EvaluarAlerta(p);
                }
            }

            bitacora.Registrar(EventoSistema_60MN.ConfiguracionStockModificada,
                $"Margen general {config.MargenGeneral:0.##}%, stock mínimo sugerido {config.StockMinimoSugerido}" +
                (recalcularPrecios ? $", {impacto.ProductosARecalcular} precio(s) recalculado(s)" : string.Empty) +
                (aplicarStockMinimo ? $", stock mínimo aplicado ({impacto.AlertasNuevas} alerta(s) nueva(s))" : string.Empty) + ".");
            return impacto;
        }

        private static decimal NuevoPrecio(Producto_60MN p, ConfiguracionStock_60MN config)
        {
            decimal margen = config.Categorias.FirstOrDefault(c => c.Id == p.CategoriaId)?.Margen ?? config.MargenGeneral;
            return ProductoBLL_60MN.CalcularPrecioVenta(p.PrecioCosto, margen);
        }
    }
}
