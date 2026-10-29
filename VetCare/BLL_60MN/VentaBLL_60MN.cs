using System.Globalization;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>Resultado de confirmar el carrito: la venta grabada y los productos que quedaron con alerta.</summary>
    public sealed class ResultadoVenta_60MN
    {
        public int VentaId { get; }

        public IReadOnlyList<string> Alertas { get; }

        internal ResultadoVenta_60MN(int ventaId, IReadOnlyList<string> alertas)
        {
            VentaId = ventaId;
            Alertas = alertas;
        }
    }

    /// <summary>
    /// CUN05 Registrar Medicación en Carrito: el carrito se arma en memoria (Carrito_60MN) y al
    /// confirmarlo se graba la venta con sus egresos de stock («include» CUN09) en una sola
    /// transacción. Se abre desde una atención o desde VENTAS > Carrito eligiendo un cliente,
    /// con o sin atención. El sistema no factura.
    /// </summary>
    public class VentaBLL_60MN
    {
        /// <summary>4.2: días antes del vencimiento en que se advierte "próximo a vencer".</summary>
        public const int DiasProximoAVencer = 30;

        /// <summary>Días hacia atrás de las atenciones que se ofrecen para asociar la venta.</summary>
        public const int DiasAtencionesRecientes = 30;

        private readonly VentaDAL_60MN mapper = new VentaDAL_60MN();
        private readonly AtencionDAL_60MN atenciones = new AtencionDAL_60MN();
        private readonly ClienteDAL_60MN clientes = new ClienteDAL_60MN();
        private readonly ProductoDAL_60MN productos = new ProductoDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        public Atencion_60MN? ObtenerAtencion(int atencionId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            return atenciones.ObtenerPorId(atencionId);
        }

        public Cliente_60MN? ObtenerCliente(int clienteId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            return clientes.ObtenerPorId(clienteId);
        }

        /// <summary>Paso 2: atenciones recientes de las mascotas del cliente, a las que se puede asociar la venta.</summary>
        public List<Atencion_60MN> ListarAtencionesRecientes(int clienteId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            return atenciones.ListarRecientes(DiasAtencionesRecientes, clienteId);
        }

        /// <summary>Pasos 3 y 4: productos activos con stock, precio y vencimiento.</summary>
        public List<Producto_60MN> BuscarProductos(string? texto, int? categoriaId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            return productos.Buscar(texto, categoriaId, incluirInactivos: false).Where(p => p.StockActual > 0).ToList();
        }

        /// <summary>
        /// Paso 9: valida el carrito y graba la venta (encabezado, detalle y un egreso de stock
        /// por ítem) en una sola transacción. Devuelve la venta y los productos con alerta de reposición.
        /// </summary>
        public ResultadoVenta_60MN RegistrarVenta(Carrito_60MN carrito)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            if (carrito.Items.Count == 0)
                throw new NegocioException_60MN("El carrito está vacío.");

            Cliente_60MN cliente = clientes.ObtenerPorId(carrito.ClienteId) ?? throw new NegocioException_60MN("Seleccione el cliente de la venta.");
            if (!cliente.Activo)
                throw new NegocioException_60MN($"El cliente {cliente.NombreCompleto} está dado de baja.");
            Atencion_60MN? atencion = null;
            if (carrito.AtencionId.HasValue)
            {
                atencion = atenciones.ObtenerPorId(carrito.AtencionId.Value) ?? throw new NegocioException_60MN("La atención no existe.");
                if (atencion.ClienteId != cliente.Id)
                    throw new NegocioException_60MN($"La atención Nº {atencion.Id} es de una mascota de otro cliente ({atencion.Propietario}).");
            }

            foreach (ItemCarrito_60MN item in carrito.Items)
            {
                Producto_60MN p = productos.ObtenerPorId(item.ProductoId) ?? throw new NegocioException_60MN("Un producto del carrito ya no existe.");
                // 5.1: cantidad menor o igual a cero.
                if (item.Cantidad <= 0)
                    throw new NegocioException_60MN($"La cantidad de {p.Nombre} debe ser mayor a cero.");
                if (!p.Activo)
                    throw new NegocioException_60MN($"El producto {p.Codigo} está dado de baja.");
                // 4.2: no se entrega medicación vencida.
                if (p.Vencido)
                    throw new NegocioException_60MN($"El producto {p.Codigo} \"{p.Nombre}\" (lote {p.Lote}) está vencido.");
                // 4.1: stock insuficiente (también se controla dentro de la transacción).
                if (item.Cantidad > p.StockActual)
                    throw new NegocioException_60MN($"Stock insuficiente de {p.Nombre}: disponible {p.StockActual}, solicitado {item.Cantidad}.");
                // El precio es siempre el vigente del producto, no el que trae la pantalla.
                item.PrecioUnitario = p.PrecioVenta;
                item.Codigo = p.Codigo;
                item.Producto = p.Nombre;
                item.Lote = p.Lote;
            }

            string motivo = atencion != null
                ? $"Medicación entregada en la atención Nº {atencion.Id}"
                : $"Venta a {cliente.NombreCompleto}";
            (int ventaId, List<EntregaCarrito_60MN> entregas) resultado;
            try
            {
                resultado = mapper.Registrar(carrito, SesionActual_60MN.UsuarioId, motivo);
            }
            catch (InvalidOperationException ex)
            {
                throw new NegocioException_60MN(ex.Message);
            }

            dv.ActualizarFila(TablaDV_60MN.VentaEncabezado, resultado.ventaId);
            foreach (EntregaCarrito_60MN entrega in resultado.entregas)
            {
                dv.ActualizarFila(TablaDV_60MN.MovimientoStock, entrega.MovimientoId);
                dv.ActualizarFila(TablaDV_60MN.VentaDetalle, entrega.DetalleId);
                dv.ActualizarFila(TablaDV_60MN.Productos, entrega.ProductoId);
            }

            // Postcondición: si alguna existencia alcanza el stock mínimo se dispara la alerta (sin duplicarla).
            ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();
            List<string> alertas = new List<string>();
            foreach (int productoId in resultado.entregas.Select(e => e.ProductoId).Distinct())
            {
                Producto_60MN p = productos.ObtenerPorId(productoId)!;
                if (productoBLL.EvaluarAlerta(p) || p.BajoStockMinimo)
                    alertas.Add($"{p.Nombre} (stock {p.StockActual}, mínimo {p.StockMinimo})");
            }

            bitacora.Registrar(EventoSistema_60MN.VentaRegistrada,
                $"Venta V-{resultado.ventaId:00000} a {cliente.NombreCompleto}" +
                (atencion != null ? $" (atención Nº {atencion.Id}, {atencion.Paciente})" : " (sin atención)") + ": " +
                string.Join(", ", carrito.Items.Select(i => $"{i.Cantidad} x {i.Producto}")) +
                $". Total $ {carrito.Total.ToString("N2", CultureInfo.GetCultureInfo("es-AR"))}.");
            return new ResultadoVenta_60MN(resultado.ventaId, alertas);
        }

        /// <summary>Ventas asociadas a una atención (la medicación que se le entregó).</summary>
        public List<VentaEncabezado_60MN> ObtenerPorAtencion(int atencionId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            return mapper.ObtenerPorAtencion(atencionId);
        }

        /// <summary>VENTAS > Ventas registradas: consulta por período (fechas inclusive) y, opcionalmente, por cliente.</summary>
        public List<VentaEncabezado_60MN> ListarVentas(DateTime desde, DateTime hasta, int? clienteId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            if (desde.Date > hasta.Date)
                throw new NegocioException_60MN("La fecha desde no puede ser posterior a la fecha hasta.");
            if (desde.Date > DateTime.Today)
                throw new NegocioException_60MN("La fecha desde no puede ser futura.");

            // Límite superior exclusivo: incluye todo el día "hasta".
            List<VentaEncabezado_60MN> ventas = mapper.ListarVentas(desde.Date, hasta.Date.AddDays(1), clienteId);
            bitacora.Registrar(EventoSistema_60MN.VentasConsultadas,
                $"Consulta de ventas del {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}" +
                (clienteId.HasValue ? $" del cliente Id {clienteId}" : string.Empty) + $" ({ventas.Count} venta(s)).");
            return ventas;
        }
    }
}
