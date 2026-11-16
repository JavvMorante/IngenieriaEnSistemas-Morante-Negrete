using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// CUN10 Generar Solicitud de Reposición (encargado de depósito): parte de los productos en
    /// alerta de stock mínimo, propone cuánto pedir y deja la solicitud PENDIENTE para compras.
    /// </summary>
    public class SolicitudReposicionBLL_60MN
    {
        public const int LargoObservaciones = 400;

        private readonly SolicitudReposicionDAL_60MN mapper = new SolicitudReposicionDAL_60MN();
        private readonly ProductoDAL_60MN productos = new ProductoDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Cantidad sugerida: lo necesario para llegar al doble del mínimo (como mínimo 1).</summary>
        public static int CantidadSugerida(int stockActual, int stockMinimo) => Math.Max(2 * stockMinimo - stockActual, 1);

        /// <summary>Paso 1: productos activos con alerta activa que todavía no se pidieron.</summary>
        public List<SolicitudDetalle_60MN> ListarProductosEnAlerta()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.SolicitudReposicion);
            List<SolicitudDetalle_60MN> items = mapper.ListarProductosEnAlerta();
            foreach (SolicitudDetalle_60MN item in items)
                item.CantidadPedida = item.CantidadSugerida = CantidadSugerida(item.StockActual, item.StockMinimo);
            return items;
        }

        /// <summary>Flujo alternativo: agregar a la solicitud un producto activo que no está en alerta.</summary>
        public List<SolicitudDetalle_60MN> BuscarProductos(string? texto)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.SolicitudReposicion);
            return productos.Buscar(texto, null, incluirInactivos: false)
                .Select(p => new SolicitudDetalle_60MN
                {
                    ProductoId = p.Id, Codigo = p.Codigo, Producto = p.Nombre, ProveedorHabitual = p.ProveedorNombre,
                    StockActual = p.StockActual, StockMinimo = p.StockMinimo, PrecioCostoActual = p.PrecioCosto,
                    CantidadSugerida = CantidadSugerida(p.StockActual, p.StockMinimo),
                    CantidadPedida = CantidadSugerida(p.StockActual, p.StockMinimo),
                    EnAlerta = p.BajoStockMinimo
                })
                .ToList();
        }

        /// <summary>
        /// Pasos 4 a 7: valida y graba la solicitud con los ítems marcados. Si algún producto ya está
        /// en una solicitud o una orden abierta, pide confirmación (confirmado = true para seguir).
        /// En la misma transacción marca las alertas como atendidas.
        /// </summary>
        public SolicitudReposicion_60MN Registrar(IEnumerable<SolicitudDetalle_60MN> items, string? observaciones, bool confirmado = false)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.SolicitudReposicion);
            int usuarioId = SesionActual_60MN.UsuarioId
                ?? throw new NegocioException_60MN("El usuario de emergencia no puede generar solicitudes de reposición.");

            List<SolicitudDetalle_60MN> pedidos = items.Where(i => i.Incluido).ToList();
            if (pedidos.Count == 0)
                throw new NegocioException_60MN("Seleccione al menos un producto para la solicitud.");
            SolicitudDetalle_60MN? sinCantidad = pedidos.FirstOrDefault(i => i.CantidadPedida <= 0);
            if (sinCantidad != null)
                throw new NegocioException_60MN($"La cantidad a pedir de {sinCantidad.Producto} debe ser mayor a cero.");
            if (pedidos.GroupBy(i => i.ProductoId).Any(g => g.Count() > 1))
                throw new NegocioException_60MN("Un producto figura más de una vez en la solicitud.");

            observaciones = Validaciones_60MN.Normalizar(observaciones);
            if (observaciones.Length > 0)
            {
                Validaciones_60MN.Largo(observaciones, LargoObservaciones, "La observación");
                Validaciones_60MN.TextoLibre(observaciones, "La observación");
            }

            // Foto del stock al momento de pedir (no la que trae la pantalla).
            foreach (SolicitudDetalle_60MN item in pedidos)
            {
                Producto_60MN p = productos.ObtenerPorId(item.ProductoId) ?? throw new NegocioException_60MN("Un producto de la solicitud ya no existe.");
                if (!p.Activo)
                    throw new NegocioException_60MN($"El producto {p.Codigo} \"{p.Nombre}\" está dado de baja.");
                item.Codigo = p.Codigo;
                item.Producto = p.Nombre;
                item.StockActual = p.StockActual;
                item.StockMinimo = p.StockMinimo;
            }

            if (!confirmado)
            {
                List<string> repetidos = mapper.ProductosYaEnCompra(pedidos.Select(i => i.ProductoId))
                    .Select(r => $"- {pedidos.First(i => i.ProductoId == r.ProductoId).Producto}: ya está en {r.Documento}")
                    .ToList();
                if (repetidos.Count > 0)
                    throw new ConfirmacionRequeridaException_60MN(
                        "Estos productos ya están en una compra en curso:\n" + string.Join("\n", repetidos) + "\n\n¿Desea pedirlos igual?");
            }

            SolicitudReposicion_60MN solicitud = new SolicitudReposicion_60MN
            {
                FechaHora = DateTime.Now,
                UsuarioId = usuarioId,
                Usuario = SesionActual_60MN.NombreUsuario,
                Observaciones = observaciones.Length == 0 ? null : observaciones
            };
            solicitud.Detalle.AddRange(pedidos);

            (int id, List<int> detalleIds) = mapper.Registrar(solicitud);
            dv.ActualizarFila(TablaDV_60MN.SolicitudReposicion, id);
            foreach (int detalleId in detalleIds)
                dv.ActualizarFila(TablaDV_60MN.SolicitudReposicionDetalle, detalleId);

            SolicitudReposicion_60MN grabada = mapper.ObtenerPorId(id)!;
            bitacora.Registrar(EventoSistema_60MN.SolicitudReposicionGenerada,
                $"Solicitud de reposición {grabada.Numero}: " +
                string.Join(", ", pedidos.Select(i => $"{i.CantidadPedida} x {i.Producto}")) + ".");
            return grabada;
        }
    }
}
