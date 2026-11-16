using System.Globalization;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// CUN11 Registrar Orden de Compra (analista de compras) y CUN12 Aprobar Orden de Compra (gerente).
    /// El analista compara las cotizaciones que mandaron los proveedores (fuera del sistema) y
    /// registra la elegida como orden; el gerente la aprueba o la rechaza con motivo.
    /// </summary>
    public class OrdenCompraBLL_60MN
    {
        public const int LargoNroCotizacion = 30;
        public const int LargoCondicionPago = 60;
        public const int LargoMotivo = 300;
        public const int PlazoMaximoDias = 365;

        /// <summary>CUN11: un precio que supera el costo actual en más de este porcentaje queda como advertencia.</summary>
        public const decimal PorcentajeAdvertenciaPrecio = 20m;

        private static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("es-AR");

        /// <summary>Estados en los que la orden todavía se puede anular (antes de pagarla).</summary>
        private static readonly EstadoOrden_60MN[] Anulables =
            { EstadoOrden_60MN.PendienteAprobacion, EstadoOrden_60MN.Rechazada, EstadoOrden_60MN.Aprobada, EstadoOrden_60MN.Enviada };

        private readonly OrdenCompraDAL_60MN mapper = new OrdenCompraDAL_60MN();
        private readonly SolicitudReposicionDAL_60MN solicitudes = new SolicitudReposicionDAL_60MN();
        private readonly CatalogoStockDAL_60MN catalogo = new CatalogoStockDAL_60MN();
        private readonly ProveedorDAL_60MN proveedores = new ProveedorDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        // ------------------------------------------------------------------
        // CUN11 Registrar Orden de Compra
        // ------------------------------------------------------------------

        /// <summary>Paso 1: solicitudes PENDIENTE o EN_COMPRA a las que todavía les falta pedir algo.</summary>
        public List<SolicitudReposicion_60MN> ListarSolicitudesPendientes()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.OrdenCompra);
            return solicitudes.ListarVigentes().Where(s => s.UnidadesPendientes > 0).ToList();
        }

        public SolicitudReposicion_60MN? ObtenerSolicitud(int id)
        {
            SesionActual_60MN.RequiereAlgunPermiso(CodigosPatente_60MN.OrdenCompra, CodigosPatente_60MN.AprobarOrden);
            return solicitudes.ObtenerPorId(id);
        }

        /// <summary>Proveedores activos a los que se les puede pedir cotización.</summary>
        public List<Proveedor_60MN> ListarProveedores()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.OrdenCompra);
            return catalogo.ListarProveedores(soloActivos: true);
        }

        /// <summary>
        /// Paso 4: calcula el total de cada cotización con lo que falta pedir de la solicitud y marca
        /// la de menor costo. Cada cotización tiene que tener precio para todos esos productos.
        /// </summary>
        public List<Cotizacion_60MN> Comparar(SolicitudReposicion_60MN solicitud, IEnumerable<Cotizacion_60MN> cotizaciones)
        {
            List<Cotizacion_60MN> lista = cotizaciones.ToList();
            List<SolicitudDetalle_60MN> aPedir = solicitud.Detalle.Where(d => d.PendienteDePedir > 0).ToList();
            foreach (Cotizacion_60MN c in lista)
            {
                ValidarCotizacion(c, aPedir);
                c.Total = aPedir.Sum(d => Math.Round(d.PendienteDePedir * c.Precios[d.ProductoId], 2));
                c.EsLaMasBarata = false;
            }
            if (lista.Count > 0)
            {
                decimal minimo = lista.Min(c => c.Total);
                foreach (Cotizacion_60MN c in lista.Where(c => c.Total == minimo))
                    c.EsLaMasBarata = true;
            }
            return lista;
        }

        /// <summary>Arma la orden con la cotización elegida y lo que falta pedir de la solicitud.</summary>
        public OrdenCompra_60MN ArmarOrden(SolicitudReposicion_60MN solicitud, Cotizacion_60MN elegida, string? motivoEleccion)
        {
            OrdenCompra_60MN orden = new OrdenCompra_60MN
            {
                SolicitudId = solicitud.Id, SolicitudNumero = solicitud.Numero, ProveedorId = elegida.ProveedorId,
                Proveedor = elegida.Proveedor, NroCotizacion = elegida.NroCotizacion, PlazoEntregaDias = elegida.PlazoEntregaDias,
                CondicionPago = elegida.CondicionPago, MotivoEleccion = motivoEleccion
            };
            foreach (SolicitudDetalle_60MN d in solicitud.Detalle.Where(d => d.PendienteDePedir > 0))
            {
                decimal precio = elegida.Precios.TryGetValue(d.ProductoId, out decimal p) ? p : 0m;
                orden.Detalle.Add(new OrdenCompraDetalle_60MN
                {
                    ProductoId = d.ProductoId, Codigo = d.Codigo, Producto = d.Producto, Cantidad = d.PendienteDePedir,
                    PrecioUnitario = precio, Subtotal = Math.Round(d.PendienteDePedir * precio, 2), PrecioCostoActual = d.PrecioCostoActual
                });
            }
            orden.Total = orden.Detalle.Sum(d => d.Subtotal);
            return orden;
        }

        /// <summary>
        /// Pasos 5 a 8: valida la orden armada con la cotización elegida y la registra PENDIENTE_APROBACION.
        /// cotizaciones son todas las comparadas: si la elegida no es la más barata, el motivo es obligatorio.
        /// </summary>
        public OrdenCompra_60MN Registrar(OrdenCompra_60MN orden, IEnumerable<Cotizacion_60MN> cotizaciones)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.OrdenCompra);
            orden.UsuarioId = SesionActual_60MN.UsuarioId
                ?? throw new NegocioException_60MN("El usuario de emergencia no puede registrar órdenes de compra.");

            SolicitudReposicion_60MN solicitud = solicitudes.ObtenerPorId(orden.SolicitudId)
                ?? throw new NegocioException_60MN("La solicitud de reposición no existe.");
            if (solicitud.Estado != EstadoSolicitud_60MN.Pendiente && solicitud.Estado != EstadoSolicitud_60MN.EnCompra)
                throw new NegocioException_60MN($"La solicitud {solicitud.Numero} está {solicitud.EstadoTexto.ToLower()}: no admite órdenes.");

            Validar(orden, solicitud, ordenACorregir: null);
            ExigirMotivoSiNoEsLaMasBarata(orden, solicitud, cotizaciones);
            orden.Advertencias = CalcularAdvertencias(orden);
            orden.FechaHora = DateTime.Now;

            (int id, List<int> detalleIds) = mapper.Registrar(orden);
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, id);
            foreach (int detalleId in detalleIds)
                dv.ActualizarFila(TablaDV_60MN.OrdenCompraDetalle, detalleId);
            dv.ActualizarFila(TablaDV_60MN.SolicitudReposicion, solicitud.Id);

            OrdenCompra_60MN grabada = mapper.ObtenerPorId(id)!;
            bitacora.Registrar(EventoSistema_60MN.OrdenCompraRegistrada,
                $"Orden de compra {grabada.Numero} a {grabada.Proveedor} (solicitud {solicitud.Numero}, cotización {grabada.NroCotizacion}) " +
                $"por $ {grabada.Total.ToString("N2", Ar)}." + (grabada.TieneAdvertencias ? " Con advertencias de precio." : string.Empty));
            return grabada;
        }

        /// <summary>
        /// Orden RECHAZADA: el analista corrige proveedor, cotización, cantidades o precios y la orden
        /// vuelve a PENDIENTE_APROBACION. Los productos son los mismos de la orden original.
        /// </summary>
        public OrdenCompra_60MN Corregir(OrdenCompra_60MN orden, IEnumerable<Cotizacion_60MN> cotizaciones)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.OrdenCompra);
            if (SesionActual_60MN.UsuarioId == null)
                throw new NegocioException_60MN("El usuario de emergencia no puede corregir órdenes de compra.");

            OrdenCompra_60MN actual = mapper.ObtenerPorId(orden.Id) ?? throw new NegocioException_60MN("La orden de compra no existe.");
            if (actual.Estado != EstadoOrden_60MN.Rechazada)
                throw new NegocioException_60MN($"Solo se corrigen órdenes rechazadas: la orden {actual.Numero} está {actual.EstadoTexto.ToLower()}.");
            if (orden.Detalle.Count != actual.Detalle.Count ||
                orden.Detalle.Any(d => actual.Detalle.All(a => a.Id != d.Id || a.ProductoId != d.ProductoId)))
                throw new NegocioException_60MN("La corrección no puede agregar ni quitar productos: anule la orden y genere otra.");

            SolicitudReposicion_60MN solicitud = solicitudes.ObtenerPorId(actual.SolicitudId)!;
            orden.SolicitudId = actual.SolicitudId;
            Validar(orden, solicitud, actual);
            ExigirMotivoSiNoEsLaMasBarata(orden, solicitud, cotizaciones);
            orden.Advertencias = CalcularAdvertencias(orden);

            if (!mapper.Corregir(orden))
                throw new NegocioException_60MN($"La orden {actual.Numero} cambió de estado mientras se corregía. Vuelva a consultarla.");
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, orden.Id);
            foreach (OrdenCompraDetalle_60MN d in orden.Detalle)
                dv.ActualizarFila(TablaDV_60MN.OrdenCompraDetalle, d.Id);

            OrdenCompra_60MN corregida = mapper.ObtenerPorId(orden.Id)!;
            bitacora.Registrar(EventoSistema_60MN.OrdenCompraCorregida,
                $"Corrección de la orden rechazada {corregida.Numero} (antes: \"{actual.MotivoRechazo}\"). " +
                $"Proveedor {corregida.Proveedor}, total $ {corregida.Total.ToString("N2", Ar)}: vuelve a aprobación.");
            return corregida;
        }

        /// <summary>Anula una orden que todavía no se pagó. Si la solicitud queda sin órdenes, vuelve a PENDIENTE.</summary>
        public void Anular(int ordenId, string? motivo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.OrdenCompra);
            motivo = MotivoObligatorio(motivo, "de la anulación");

            OrdenCompra_60MN orden = mapper.ObtenerPorId(ordenId) ?? throw new NegocioException_60MN("La orden de compra no existe.");
            if (!Anulables.Contains(orden.Estado))
                throw new NegocioException_60MN($"La orden {orden.Numero} está {orden.EstadoTexto.ToLower()}: ya no se puede anular.");

            if (!mapper.Anular(ordenId, motivo, out bool solicitudReabierta))
                throw new NegocioException_60MN($"La orden {orden.Numero} cambió de estado mientras tanto. Vuelva a consultarla.");
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, ordenId);
            if (solicitudReabierta)
                dv.ActualizarFila(TablaDV_60MN.SolicitudReposicion, orden.SolicitudId);

            bitacora.Registrar(EventoSistema_60MN.OrdenCompraAnulada,
                $"Anulación de la orden {orden.Numero} ({orden.EstadoTexto}) a {orden.Proveedor}. Motivo: {motivo}" +
                (solicitudReabierta ? $" La solicitud {orden.SolicitudNumero} vuelve a pendiente." : string.Empty));
        }

        /// <summary>"Mis órdenes": las órdenes registradas por el usuario actual (las rechazadas se corrigen o anulan).</summary>
        public List<OrdenCompra_60MN> ListarMisOrdenes()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.OrdenCompra);
            return mapper.Listar((EstadoOrden_60MN[])Enum.GetValues(typeof(EstadoOrden_60MN)), SesionActual_60MN.UsuarioId ?? -1);
        }

        public OrdenCompra_60MN? ObtenerPorId(int id)
        {
            SesionActual_60MN.RequiereAlgunPermiso(CodigosPatente_60MN.OrdenCompra, CodigosPatente_60MN.AprobarOrden,
                CodigosPatente_60MN.PagoProveedor, CodigosPatente_60MN.Recepcion);
            return mapper.ObtenerPorId(id);
        }

        // ------------------------------------------------------------------
        // CUN12 Aprobar Orden de Compra
        // ------------------------------------------------------------------

        public List<OrdenCompra_60MN> ListarPendientesDeAprobacion()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.AprobarOrden);
            return mapper.Listar(new[] { EstadoOrden_60MN.PendienteAprobacion });
        }

        /// <summary>
        /// Aprueba la orden. Quien la registró no puede aprobarla (separación de funciones). Si existe
        /// TopeCompraMensual y la orden lo hace superar, pide confirmación (confirmado = true para seguir).
        /// </summary>
        public void Aprobar(int ordenId, bool confirmado = false)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.AprobarOrden);
            OrdenCompra_60MN orden = OrdenParaDecidir(ordenId, out int gerenteId);

            decimal? tope = catalogo.ObtenerTopeCompraMensual();
            if (!confirmado && tope.HasValue)
            {
                decimal aprobado = mapper.TotalAprobadoEnElMes(DateTime.Today);
                if (aprobado + orden.Total > tope.Value)
                    throw new ConfirmacionRequeridaException_60MN(
                        $"Con esta orden las compras aprobadas del mes suman $ {(aprobado + orden.Total).ToString("N2", Ar)} " +
                        $"y superan el tope mensual de $ {tope.Value.ToString("N2", Ar)}. ¿Desea aprobarla igual?");
            }

            if (!mapper.Aprobar(ordenId, gerenteId, DateTime.Now))
                throw new NegocioException_60MN($"La orden {orden.Numero} cambió de estado mientras tanto (otro usuario la aprobó, rechazó o anuló).");
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, ordenId);
            bitacora.Registrar(EventoSistema_60MN.OrdenCompraAprobada,
                $"Aprobación de la orden {orden.Numero} a {orden.Proveedor} por $ {orden.Total.ToString("N2", Ar)} " +
                $"(registrada por {orden.Usuario})." + (orden.TieneAdvertencias ? " Tenía advertencias de precio." : string.Empty));
        }

        /// <summary>Rechaza la orden con motivo obligatorio; el analista la puede corregir o anular.</summary>
        public void Rechazar(int ordenId, string? motivo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.AprobarOrden);
            motivo = MotivoObligatorio(motivo, "del rechazo");
            OrdenCompra_60MN orden = OrdenParaDecidir(ordenId, out int gerenteId);

            if (!mapper.Rechazar(ordenId, gerenteId, DateTime.Now, motivo))
                throw new NegocioException_60MN($"La orden {orden.Numero} cambió de estado mientras tanto (otro usuario la aprobó, rechazó o anuló).");
            dv.ActualizarFila(TablaDV_60MN.OrdenCompra, ordenId);
            bitacora.Registrar(EventoSistema_60MN.OrdenCompraRechazada,
                $"Rechazo de la orden {orden.Numero} a {orden.Proveedor} por $ {orden.Total.ToString("N2", Ar)}. Motivo: {motivo}");
        }

        private OrdenCompra_60MN OrdenParaDecidir(int ordenId, out int gerenteId)
        {
            gerenteId = SesionActual_60MN.UsuarioId
                ?? throw new NegocioException_60MN("El usuario de emergencia no puede aprobar ni rechazar órdenes de compra.");
            OrdenCompra_60MN orden = mapper.ObtenerPorId(ordenId) ?? throw new NegocioException_60MN("La orden de compra no existe.");
            if (orden.Estado != EstadoOrden_60MN.PendienteAprobacion)
                throw new NegocioException_60MN($"La orden {orden.Numero} ya no está pendiente de aprobación (está {orden.EstadoTexto.ToLower()}).");
            // Separación de funciones: quien compra no aprueba su propia compra.
            if (orden.UsuarioId == gerenteId)
                throw new NegocioException_60MN($"Usted registró la orden {orden.Numero}: la debe aprobar o rechazar otro usuario (separación de funciones).");
            return orden;
        }

        // ------------------------------------------------------------------
        // Validaciones
        // ------------------------------------------------------------------

        private void Validar(OrdenCompra_60MN orden, SolicitudReposicion_60MN solicitud, OrdenCompra_60MN? ordenACorregir)
        {
            Proveedor_60MN proveedor = proveedores.ObtenerPorId(orden.ProveedorId) ?? throw new NegocioException_60MN("Seleccione el proveedor de la cotización.");
            if (!proveedor.Activo)
                throw new NegocioException_60MN($"El proveedor {proveedor.RazonSocial} está dado de baja.");
            orden.Proveedor = proveedor.RazonSocial;

            orden.NroCotizacion = Validaciones_60MN.Normalizar(orden.NroCotizacion).ToUpperInvariant();
            if (orden.NroCotizacion.Length == 0)
                throw new NegocioException_60MN("El número de cotización es obligatorio.");
            Validaciones_60MN.Largo(orden.NroCotizacion, LargoNroCotizacion, "El número de cotización");
            Validaciones_60MN.Lote(orden.NroCotizacion, "El número de cotización");

            orden.CondicionPago = Validaciones_60MN.Normalizar(orden.CondicionPago);
            if (orden.CondicionPago.Length == 0)
                throw new NegocioException_60MN("La condición de pago es obligatoria.");
            Validaciones_60MN.Largo(orden.CondicionPago, LargoCondicionPago, "La condición de pago");
            Validaciones_60MN.TextoLibre(orden.CondicionPago, "La condición de pago");

            if (orden.PlazoEntregaDias < 0 || orden.PlazoEntregaDias > PlazoMaximoDias)
                throw new NegocioException_60MN($"El plazo de entrega debe estar entre 0 y {PlazoMaximoDias} días.");
            if (orden.Detalle.Count == 0)
                throw new NegocioException_60MN("La orden no tiene productos: la solicitud ya está totalmente pedida.");

            foreach (OrdenCompraDetalle_60MN d in orden.Detalle)
            {
                SolicitudDetalle_60MN? item = solicitud.Detalle.FirstOrDefault(s => s.ProductoId == d.ProductoId)
                    ?? throw new NegocioException_60MN($"El producto {d.Producto} no está en la solicitud {solicitud.Numero}.");
                d.Codigo = item.Codigo;
                d.Producto = item.Producto;
                d.PrecioCostoActual = item.PrecioCostoActual;
                if (d.Cantidad <= 0)
                    throw new NegocioException_60MN($"La cantidad de {d.Producto} debe ser mayor a cero.");
                if (d.PrecioUnitario <= 0)
                    throw new NegocioException_60MN($"El precio de {d.Producto} debe ser mayor a cero.");
                // En una corrección, lo que ya tenía esta orden vuelve a estar disponible.
                int disponible = item.PendienteDePedir + (ordenACorregir?.Detalle.FirstOrDefault(a => a.ProductoId == d.ProductoId)?.Cantidad ?? 0);
                if (d.Cantidad > disponible)
                    throw new NegocioException_60MN($"La cantidad de {d.Producto} ({d.Cantidad}) supera lo que falta pedir de la solicitud ({disponible}).");
                d.Subtotal = Math.Round(d.Cantidad * d.PrecioUnitario, 2);
            }
            orden.Total = orden.Detalle.Sum(d => d.Subtotal);

            orden.MotivoEleccion = Validaciones_60MN.Normalizar(orden.MotivoEleccion);
            if (orden.MotivoEleccion.Length == 0)
                orden.MotivoEleccion = null;
            else
            {
                Validaciones_60MN.Largo(orden.MotivoEleccion, LargoMotivo, "El motivo de elección");
                Validaciones_60MN.TextoLibre(orden.MotivoEleccion, "El motivo de elección");
            }
        }

        /// <summary>Paso 5.1: si la cotización elegida no es la de menor costo, el motivo de elección es obligatorio.</summary>
        private void ExigirMotivoSiNoEsLaMasBarata(OrdenCompra_60MN orden, SolicitudReposicion_60MN solicitud, IEnumerable<Cotizacion_60MN> cotizaciones)
        {
            // Se comparan con las mismas cantidades de la orden, para que la comparación sea justa;
            // solo cuentan las cotizaciones que tienen precio para todos sus productos.
            List<Cotizacion_60MN> comparadas = cotizaciones
                .Where(c => orden.Detalle.All(d => c.Precios.TryGetValue(d.ProductoId, out decimal p) && p > 0))
                .ToList();
            if (comparadas.Count == 0) return;
            decimal TotalCon(Cotizacion_60MN c) => orden.Detalle.Sum(d => Math.Round(d.Cantidad * c.Precios[d.ProductoId], 2));
            decimal minimo = comparadas.Min(TotalCon);
            if (orden.Total > minimo && orden.MotivoEleccion == null)
                throw new NegocioException_60MN(
                    $"La cotización elegida ($ {orden.Total.ToString("N2", Ar)}) no es la de menor costo ($ {minimo.ToString("N2", Ar)}): indique el motivo de la elección.");
        }

        /// <summary>Paso 6: precios que superan en más de 20 % el costo actual del producto (los ve el gerente al aprobar).</summary>
        private static string? CalcularAdvertencias(OrdenCompra_60MN orden)
        {
            List<string> avisos = new List<string>();
            foreach (OrdenCompraDetalle_60MN d in orden.Detalle.Where(d => d.PrecioCostoActual > 0))
            {
                decimal aumento = (d.PrecioUnitario / d.PrecioCostoActual - 1) * 100m;
                if (aumento > PorcentajeAdvertenciaPrecio)
                    avisos.Add($"{d.Producto}: $ {d.PrecioUnitario.ToString("N2", Ar)} supera en {aumento.ToString("0.#", Ar)} % el costo actual ($ {d.PrecioCostoActual.ToString("N2", Ar)})");
            }
            string texto = string.Join("; ", avisos);
            return texto.Length == 0 ? null : (texto.Length > 400 ? texto.Substring(0, 397) + "..." : texto);
        }

        private static void ValidarCotizacion(Cotizacion_60MN c, List<SolicitudDetalle_60MN> aPedir)
        {
            if (string.IsNullOrWhiteSpace(c.NroCotizacion))
                throw new NegocioException_60MN($"Falta el número de la cotización de {c.Proveedor}.");
            foreach (SolicitudDetalle_60MN d in aPedir)
                if (!c.Precios.TryGetValue(d.ProductoId, out decimal precio) || precio <= 0)
                    throw new NegocioException_60MN($"La cotización {c.NroCotizacion} de {c.Proveedor} no tiene un precio válido para {d.Producto}.");
        }

        private static string MotivoObligatorio(string? motivo, string de)
        {
            motivo = Validaciones_60MN.Normalizar(motivo);
            if (motivo.Length == 0)
                throw new NegocioException_60MN($"Indique el motivo {de}.");
            Validaciones_60MN.Largo(motivo, LargoMotivo, "El motivo");
            Validaciones_60MN.TextoLibre(motivo, "El motivo");
            return motivo;
        }
    }
}
