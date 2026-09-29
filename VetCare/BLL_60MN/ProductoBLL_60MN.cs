using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>CUN11 (flujo 4.1): ya existe un producto con el mismo nombre y lote.</summary>
    public class ProductoDuplicadoException_60MN : NegocioException_60MN
    {
        public Producto_60MN Existente { get; }

        public ProductoDuplicadoException_60MN(Producto_60MN existente)
            : base($"Ya existe el producto \"{existente.Nombre}\" con el lote {existente.Lote} ({existente.Codigo}).")
        {
            Existente = existente;
        }
    }

    /// <summary>CUN11 Registrar Producto y CUN12 Modificar Producto.</summary>
    public class ProductoBLL_60MN
    {
        private readonly ProductoDAL_60MN mapper = new ProductoDAL_60MN();
        private readonly CatalogoStockDAL_60MN catalogo = new CatalogoStockDAL_60MN();
        private readonly MovimientoStockDAL_60MN alertas = new MovimientoStockDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        public List<Categoria_60MN> ListarCategorias() => catalogo.ListarCategorias();

        public List<Proveedor_60MN> ListarProveedores() => catalogo.ListarProveedores();

        public List<Producto_60MN> Buscar(string? texto, int? categoriaId, bool incluirInactivos)
        {
            if (!new[] { CodigosPatente_60MN.ProductoAlta, CodigosPatente_60MN.ProductoModificar, CodigosPatente_60MN.MovimientoStock }
                    .Any(SessionManager_60MN.Instancia.TienePermiso))
                throw new NegocioException_60MN("No tiene permiso para consultar productos.");
            return mapper.Buscar(texto, categoriaId, incluirInactivos);
        }

        public Producto_60MN? ObtenerPorId(int id) => mapper.ObtenerPorId(id);

        public int ContarAlertasActivas() => alertas.ContarAlertasActivas();

        /// <summary>Margen aplicable: el particular de la categoría o, si no tiene, el margen general (CUN11 paso 5).</summary>
        public decimal ObtenerMargen(int categoriaId)
        {
            Categoria_60MN? categoria = catalogo.ListarCategorias().FirstOrDefault(c => c.Id == categoriaId);
            return categoria?.Margen ?? catalogo.ObtenerMargenGeneral();
        }

        public static decimal CalcularPrecioVenta(decimal precioCosto, decimal margen) =>
            Math.Round(precioCosto * (1 + margen / 100m), 2, MidpointRounding.AwayFromZero);

        /// <summary>CUN11. Devuelve true si el alta disparó la alerta de reposición (flujo 4.3).</summary>
        public bool Registrar(Producto_60MN producto)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ProductoAlta);
            Normalizar(producto);
            Validar(producto);
            if (producto.StockActual < 0)
                throw new NegocioException_60MN("El stock actual no puede ser negativo.");
            if (producto.FechaVencimiento.Date < DateTime.Today)
                throw new NegocioException_60MN("La fecha de vencimiento es anterior a la fecha actual: no se puede registrar el producto.");

            Producto_60MN? duplicado = mapper.ObtenerPorNombreYLote(producto.Nombre, producto.Lote);
            if (duplicado != null)
                throw new ProductoDuplicadoException_60MN(duplicado);

            producto.Id = mapper.Insertar(producto, UsuarioParaAuditoria);
            dv.ActualizarFila(TablaDV_60MN.Productos, producto.Id);
            producto.Codigo = mapper.ObtenerPorId(producto.Id)?.Codigo ?? string.Empty;

            bitacora.Registrar(EventoSistema_60MN.ProductoCreado,
                $"Alta del producto {producto.Codigo} \"{producto.Nombre}\" lote {producto.Lote}, stock {producto.StockActual}.");
            return EvaluarAlerta(producto);
        }

        /// <summary>CUN12. El stock actual no se modifica acá (flujo 5.1): se ajusta con CUN14.</summary>
        public bool Modificar(Producto_60MN producto)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ProductoModificar);
            Normalizar(producto);
            Validar(producto);

            Producto_60MN actual = mapper.ObtenerPorId(producto.Id) ?? throw new NegocioException_60MN("El producto ya no existe.");
            if (!actual.Activo)
                throw new NegocioException_60MN("El producto se encuentra dado de baja: no se puede editar.");
            if (mapper.ObtenerPorNombreYLote(producto.Nombre, producto.Lote, producto.Id) != null)
                throw new NegocioException_60MN("Ya existe otro producto con el mismo nombre y lote.");

            mapper.Modificar(producto, UsuarioParaAuditoria);
            dv.ActualizarFila(TablaDV_60MN.Productos, producto.Id);

            bitacora.Registrar(EventoSistema_60MN.ProductoModificado, $"Modificación del producto {actual.Codigo} \"{producto.Nombre}\".");
            producto.StockActual = actual.StockActual;
            return EvaluarAlerta(producto);
        }

        public void CambiarEstado(int productoId, bool activo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ProductoModificar);
            Producto_60MN actual = mapper.ObtenerPorId(productoId) ?? throw new NegocioException_60MN("El producto ya no existe.");
            if (actual.Activo == activo)
                throw new NegocioException_60MN(activo ? "El producto ya está activo." : "El producto ya está dado de baja.");

            mapper.CambiarActivo(productoId, activo, UsuarioParaAuditoria);
            dv.ActualizarFila(TablaDV_60MN.Productos, productoId);
            if (!activo) alertas.DesactivarAlertas(productoId);

            bitacora.Registrar(activo ? EventoSistema_60MN.ProductoModificado : EventoSistema_60MN.ProductoBaja,
                $"{(activo ? "Reactivación" : "Baja lógica")} del producto {actual.Codigo} \"{actual.Nombre}\".");
        }

        /// <summary>
        /// Recuperación de una versión previa desde la Bitácora de Cambios
        /// (CUS10): restaura los datos comerciales de esa versión. El stock no
        /// se restaura porque solo cambia mediante movimientos trazables.
        /// </summary>
        public void RestaurarVersion(int productoCId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraCambios);
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ProductoModificar);

            ProductoHistorial_60MN version = new BitacoraCambiosDAL_60MN().ObtenerVersion(productoCId)
                ?? throw new NegocioException_60MN("La versión seleccionada no existe.");
            if (version.Act)
                throw new NegocioException_60MN("La versión seleccionada ya es la vigente.");
            if (version.Operacion == "ELIMINACION")
                throw new NegocioException_60MN("No se puede restaurar un registro eliminado.");

            Producto_60MN actual = mapper.ObtenerPorId(version.ProductoId) ?? throw new NegocioException_60MN("El producto ya no existe.");
            if (!actual.Activo)
                throw new NegocioException_60MN("El producto está dado de baja: reactívelo antes de restaurar una versión.");

            actual.Nombre = version.Nombre;
            actual.CategoriaId = version.CategoriaId;
            actual.ProveedorId = version.ProveedorId;
            actual.StockMinimo = version.StockMinimo;
            actual.PrecioCosto = version.PrecioCosto;
            actual.PrecioVenta = version.PrecioVenta;
            actual.FechaVencimiento = version.FechaVencimiento;
            actual.Lote = version.Lote;
            if (mapper.ObtenerPorNombreYLote(actual.Nombre, actual.Lote, actual.Id) != null)
                throw new NegocioException_60MN("No se puede restaurar: otro producto ya usa ese nombre y lote.");

            mapper.Modificar(actual, UsuarioParaAuditoria);
            dv.ActualizarFila(TablaDV_60MN.Productos, actual.Id);
            bitacora.Registrar(EventoSistema_60MN.ProductoVersionRestaurada,
                $"Se restauró la versión del {version.FechaHora:dd/MM/yyyy HH:mm:ss} del producto {actual.Codigo}.");
            EvaluarAlerta(actual);
        }

        /// <summary>Genera la alerta de reposición si el stock quedó en o debajo del mínimo (sin duplicarla).</summary>
        internal bool EvaluarAlerta(Producto_60MN producto)
        {
            if (!producto.Activo || producto.StockActual > producto.StockMinimo)
            {
                alertas.DesactivarAlertas(producto.Id);
                return false;
            }
            if (alertas.ExisteAlertaActiva(producto.Id))
                return false;

            alertas.CrearAlerta(producto.Id);
            bitacora.Registrar(EventoSistema_60MN.AlertaReposicion,
                $"Alerta de reposición: {producto.Codigo} \"{producto.Nombre}\" tiene stock {producto.StockActual} (mínimo {producto.StockMinimo}).");
            return true;
        }

        private static int UsuarioParaAuditoria => SesionActual_60MN.UsuarioId ?? 0;

        private static void Normalizar(Producto_60MN p)
        {
            p.Nombre = (p.Nombre ?? string.Empty).Trim();
            p.Lote = (p.Lote ?? string.Empty).Trim();
        }

        private static void Validar(Producto_60MN p)
        {
            if (p.Nombre.Length == 0 || p.Lote.Length == 0 || p.CategoriaId <= 0 || p.ProveedorId <= 0)
                throw new NegocioException_60MN("Complete todos los campos obligatorios.");
            if (p.StockMinimo < 0)
                throw new NegocioException_60MN("El stock mínimo no puede ser negativo.");
            if (p.PrecioCosto <= 0)
                throw new NegocioException_60MN("El precio de costo debe ser mayor a cero.");
            if (p.PrecioVenta <= 0)
                throw new NegocioException_60MN("El precio de venta debe ser mayor a cero.");
        }
    }
}
