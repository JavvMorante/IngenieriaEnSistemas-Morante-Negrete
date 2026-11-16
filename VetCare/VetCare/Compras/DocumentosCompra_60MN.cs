using System.Text;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// Texto de ancho fijo de los documentos de compras, para imprimir o guardar como PDF con
    /// ImpresionTexto_60MN: la orden de compra que se envía al proveedor (CUN13) y el reporte de compras (CUN15).
    /// </summary>
    internal static class DocumentosCompra_60MN
    {
        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private static string Recortar(string texto, int largo) => texto.Length <= largo ? texto : texto.Substring(0, largo - 1) + "…";

        public static string OrdenDeCompra(OrdenCompra_60MN orden)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VetCare - " + T("msg.OrdenDeCompraTitulo", "ORDEN DE COMPRA") + " " + orden.Numero);
            sb.AppendLine(new string('=', 86));
            sb.AppendLine($"{T("msg.Proveedor", "Proveedor"),-20}: {orden.Proveedor}");
            sb.AppendLine($"{T("msg.Fecha", "Fecha"),-20}: {orden.FechaHora:dd/MM/yyyy}");
            sb.AppendLine($"{T("msg.Cotizacion", "Cotización"),-20}: {orden.NroCotizacion}");
            sb.AppendLine($"{T("msg.Plazo", "Plazo de entrega"),-20}: {orden.PlazoEntregaDias} {T("msg.Dias", "días")}");
            sb.AppendLine($"{T("msg.CondicionPago", "Condición de pago"),-20}: {orden.CondicionPago}");
            if (orden.FechaAprobacion.HasValue)
                sb.AppendLine($"{T("msg.Aprobada", "Aprobada"),-20}: {orden.FechaAprobacion:dd/MM/yyyy} ({orden.AprobadaPorUsuario})");
            sb.AppendLine(new string('-', 86));
            sb.AppendLine($"{"Código",-10}{T("msg.Producto", "Producto"),-42}{T("msg.Cant", "Cant."),7}{T("msg.PrecioUnit", "Precio u."),13}{"Subtotal",14}");
            sb.AppendLine(new string('-', 86));
            foreach (OrdenCompraDetalle_60MN d in orden.Detalle)
                sb.AppendLine($"{d.Codigo,-10}{Recortar(d.Producto, 41),-42}{d.Cantidad,7}{d.PrecioUnitario,13:N2}{d.Subtotal,14:N2}");
            sb.AppendLine(new string('-', 86));
            sb.AppendLine($"{"TOTAL $",72}{orden.Total,14:N2}");
            sb.AppendLine();
            sb.AppendLine(T("msg.PieOrden", "Entregar la mercadería con remito que indique este número de orden."));
            return sb.ToString();
        }

        public static string ReporteDeCompras(ReporteCompras_60MN reporte, string proveedor, string estado)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VetCare - " + T("msg.TituloReporteCompras", "REPORTE DE COMPRAS"));
            sb.AppendLine(new string('=', 110));
            sb.AppendLine($"{T("msg.Periodo", "Período")}: {reporte.Filtro.Desde:dd/MM/yyyy} - {reporte.Filtro.Hasta:dd/MM/yyyy}   " +
                          $"{T("msg.Proveedor", "Proveedor")}: {proveedor}   {T("msg.Estado", "Estado")}: {estado}");
            sb.AppendLine($"{T("msg.Generado", "Generado")}: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(new string('-', 110));
            sb.AppendLine($"{T("msg.Orden", "Orden"),-10}{T("msg.Fecha", "Fecha"),-12}{T("msg.Proveedor", "Proveedor"),-30}{T("msg.Estado", "Estado"),-20}" +
                          $"{"Total",13}{T("msg.Pagado", "Pagado"),13}{T("msg.Pend", "Pend."),6}  {T("msg.UltRecep", "Últ. recep."),-10}");
            sb.AppendLine(new string('-', 110));
            foreach (FilaReporteCompra_60MN f in reporte.Filas)
                sb.AppendLine($"{f.Numero,-10}{f.FechaHora,-12:dd/MM/yyyy}{Recortar(f.Proveedor, 29),-30}{Recortar(f.EstadoTexto, 19),-20}" +
                              $"{f.Total,13:N2}{f.ImportePagado,13:N2}{f.UnidadesPendientes,6}  {(f.UltimaRecepcion.HasValue ? f.UltimaRecepcion.Value.ToString("dd/MM/yyyy") : "-"),-10}");
            sb.AppendLine(new string('-', 110));
            sb.AppendLine(T("msg.TotalesPorProveedor", "Totales por proveedor"));
            foreach (TotalProveedorCompra_60MN t in reporte.PorProveedor)
                sb.AppendLine($"  {Recortar(t.Proveedor, 38),-40}{T("msg.Ordenes", "Órdenes")} {t.Ordenes,3}   {T("msg.Comprado", "Comprado")} {t.Comprado,13:N2}   " +
                              $"{T("msg.Pagado", "Pagado")} {t.Pagado,13:N2}   {T("msg.Pend", "Pend.")} {t.UnidadesPendientes,5}");
            sb.AppendLine(new string('=', 110));
            sb.AppendLine($"{T("msg.TotalComprado", "Total comprado (aprobado o posterior)")}: $ {reporte.TotalComprado:N2}");
            sb.AppendLine($"{T("msg.TotalPagado", "Total pagado")}: $ {reporte.TotalPagado:N2}");
            sb.AppendLine($"{T("msg.PendienteRecibir", "Pendiente de recibir")}: {reporte.UnidadesPendientes} {T("msg.Unidades", "unidad(es)")}");
            return sb.ToString();
        }
    }
}
