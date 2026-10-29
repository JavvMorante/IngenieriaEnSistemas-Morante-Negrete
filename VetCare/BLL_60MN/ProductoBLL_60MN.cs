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

    /// <summary>
    /// CUN11 / CUN12: uno o más datos del producto son inválidos. Errores va por campo
    /// (nombre de la propiedad de Producto_60MN → motivo) para marcarlos en la GUI.
    /// </summary>
    public class DatosProductoInvalidosException_60MN : NegocioException_60MN
    {
        public IReadOnlyDictionary<string, string> Errores { get; }

        public DatosProductoInvalidosException_60MN(IReadOnlyDictionary<string, string> errores)
            : base("Revise los datos marcados:\n" + string.Join("\n", errores.Values))
        {
            Errores = errores;
        }
    }

    /// <summary>CUN11 Registrar Producto y CUN12 Modificar Producto.</summary>
    public class ProductoBLL_60MN
    {
        // Largos máximos (la GUI los usa como MaxLength); menores que las columnas de Productos.
        public const int LargoNombre = 80;
        public const int LargoLote = 20;
        /// <summary>Signos que admite el nombre de un producto, además de letras, números y espacios.</summary>
        public const string SignosNombre = Validaciones_60MN.SignosProducto;

        private readonly ProductoDAL_60MN mapper = new ProductoDAL_60MN();
        private readonly CatalogoStockDAL_60MN catalogo = new CatalogoStockDAL_60MN();
        private readonly ProveedorDAL_60MN proveedores = new ProveedorDAL_60MN();
        private readonly MovimientoStockDAL_60MN alertas = new MovimientoStockDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        public List<Categoria_60MN> ListarCategorias() => catalogo.ListarCategorias();

        /// <summary>
        /// Proveedores activos para el alta y la modificación de productos (CUN11: los dados de
        /// baja no se ofrecen). ademasId agrega el proveedor actual del producto que se modifica.
        /// </summary>
        public List<Proveedor_60MN> ListarProveedores(int? ademasId = null) => catalogo.ListarProveedores(soloActivos: true, ademasId);

        /// <summary>Todos los proveedores, incluso los dados de baja (filtros de reportes).</summary>
        public List<Proveedor_60MN> ListarTodosLosProveedores() => catalogo.ListarProveedores(soloActivos: false);

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
        /// <summary>Stock mínimo que se propone al registrar un producto (parametrizado en CUN13).</summary>
        public int ObtenerStockMinimoSugerido() => catalogo.ObtenerStockMinimoSugerido();

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
            Validar(producto, esAlta: true);
            ProveedorActivo(producto.ProveedorId);

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
            Validar(producto, esAlta: false);

            Producto_60MN actual = mapper.ObtenerPorId(producto.Id) ?? throw new NegocioException_60MN("El producto ya no existe.");
            if (!actual.Activo)
                throw new NegocioException_60MN("El producto se encuentra dado de baja: no se puede editar.");
            // Puede conservar su proveedor aunque esté dado de baja, pero no pasar a uno dado de baja.
            if (producto.ProveedorId != actual.ProveedorId)
                ProveedorActivo(producto.ProveedorId);
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

        private void ProveedorActivo(int proveedorId)
        {
            Proveedor_60MN proveedor = proveedores.ObtenerPorId(proveedorId)
                ?? throw new DatosProductoInvalidosException_60MN(new Dictionary<string, string>
                    { [nameof(Producto_60MN.ProveedorId)] = "El proveedor no existe." });
            if (!proveedor.Activo)
                throw new DatosProductoInvalidosException_60MN(new Dictionary<string, string>
                    { [nameof(Producto_60MN.ProveedorId)] = $"El proveedor {proveedor.RazonSocial} está dado de baja." });
        }

        private static void Normalizar(Producto_60MN p)
        {
            p.Nombre = Validaciones_60MN.Normalizar(p.Nombre);
            p.Lote = (p.Lote ?? string.Empty).Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Valida los datos del producto sin guardarlos y devuelve los errores por campo
        /// (vacío si todo es correcto). La GUI lo usa para marcar cada campo; Registrar y
        /// Modificar aplican las mismas reglas.
        /// </summary>
        public IReadOnlyDictionary<string, string> ValidarDatos(Producto_60MN producto, bool esAlta)
        {
            Producto_60MN copia = new Producto_60MN
            {
                Nombre = producto.Nombre, Lote = producto.Lote, CategoriaId = producto.CategoriaId, ProveedorId = producto.ProveedorId,
                StockActual = producto.StockActual, StockMinimo = producto.StockMinimo, PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta, FechaVencimiento = producto.FechaVencimiento
            };
            Normalizar(copia);
            return ErroresProducto(copia, esAlta);
        }

        private static void Validar(Producto_60MN p, bool esAlta)
        {
            Dictionary<string, string> errores = ErroresProducto(p, esAlta);
            if (errores.Count > 0)
                throw new DatosProductoInvalidosException_60MN(errores);
        }

        private static Dictionary<string, string> ErroresProducto(Producto_60MN p, bool esAlta)
        {
            Dictionary<string, string> errores = new Dictionary<string, string>();
            void Revisar(string campo, Action regla)
            {
                try { regla(); }
                catch (NegocioException_60MN ex) { errores[campo] = ex.Message; }
            }

            if (p.Nombre.Length == 0)
                errores[nameof(Producto_60MN.Nombre)] = "Campo obligatorio.";
            else
                Revisar(nameof(Producto_60MN.Nombre), () =>
                {
                    Validaciones_60MN.Largo(p.Nombre, LargoNombre, "El nombre");
                    Validaciones_60MN.NombreProducto(p.Nombre, "El nombre");
                });

            if (p.Lote.Length == 0)
                errores[nameof(Producto_60MN.Lote)] = "Campo obligatorio.";
            else
                Revisar(nameof(Producto_60MN.Lote), () =>
                {
                    Validaciones_60MN.Largo(p.Lote, LargoLote, "El lote");
                    Validaciones_60MN.Lote(p.Lote, "El lote");
                });

            if (p.CategoriaId <= 0)
                errores[nameof(Producto_60MN.CategoriaId)] = "Seleccione una categoría.";
            if (p.ProveedorId <= 0)
                errores[nameof(Producto_60MN.ProveedorId)] = "Seleccione un proveedor.";
            if (esAlta && p.StockActual < 0)
                errores[nameof(Producto_60MN.StockActual)] = "El stock actual no puede ser negativo.";
            if (p.StockMinimo < 0)
                errores[nameof(Producto_60MN.StockMinimo)] = "El stock mínimo no puede ser negativo.";
            if (p.PrecioCosto <= 0)
                errores[nameof(Producto_60MN.PrecioCosto)] = "El precio de costo debe ser mayor a cero.";
            if (p.PrecioVenta <= 0)
                errores[nameof(Producto_60MN.PrecioVenta)] = "El precio de venta debe ser mayor a cero.";
            // CUN11 4.2: no se da de alta un producto ya vencido.
            if (esAlta && p.FechaVencimiento.Date < DateTime.Today)
                errores[nameof(Producto_60MN.FechaVencimiento)] = "La fecha de vencimiento es anterior a la fecha actual: no se puede registrar el producto.";
            return errores;
        }
    }
}
