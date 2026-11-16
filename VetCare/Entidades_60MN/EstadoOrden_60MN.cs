namespace Entidades_60MN
{
    /// <summary>Estados de una orden de compra (CUN11 a CUN14).</summary>
    public enum EstadoOrden_60MN
    {
        PendienteAprobacion,
        Aprobada,
        Rechazada,
        Enviada,
        Pagada,
        RecibidaParcial,
        Cerrada,
        Anulada
    }

    /// <summary>Estados de una solicitud de reposición (CUN10).</summary>
    public enum EstadoSolicitud_60MN
    {
        Pendiente,
        EnCompra,
        Cerrada,
        Anulada
    }

    /// <summary>
    /// Traduce los estados al texto de la columna Estado (PENDIENTE_APROBACION, EN_COMPRA, ...)
    /// y a una descripción legible para la pantalla.
    /// </summary>
    public static class EstadosCompra_60MN
    {
        private static readonly Dictionary<EstadoOrden_60MN, string> codigosOrden = new Dictionary<EstadoOrden_60MN, string>
        {
            [EstadoOrden_60MN.PendienteAprobacion] = "PENDIENTE_APROBACION",
            [EstadoOrden_60MN.Aprobada] = "APROBADA",
            [EstadoOrden_60MN.Rechazada] = "RECHAZADA",
            [EstadoOrden_60MN.Enviada] = "ENVIADA",
            [EstadoOrden_60MN.Pagada] = "PAGADA",
            [EstadoOrden_60MN.RecibidaParcial] = "RECIBIDA_PARCIAL",
            [EstadoOrden_60MN.Cerrada] = "CERRADA",
            [EstadoOrden_60MN.Anulada] = "ANULADA"
        };

        private static readonly Dictionary<EstadoSolicitud_60MN, string> codigosSolicitud = new Dictionary<EstadoSolicitud_60MN, string>
        {
            [EstadoSolicitud_60MN.Pendiente] = "PENDIENTE",
            [EstadoSolicitud_60MN.EnCompra] = "EN_COMPRA",
            [EstadoSolicitud_60MN.Cerrada] = "CERRADA",
            [EstadoSolicitud_60MN.Anulada] = "ANULADA"
        };

        public static string Codigo(this EstadoOrden_60MN estado) => codigosOrden[estado];

        public static string Codigo(this EstadoSolicitud_60MN estado) => codigosSolicitud[estado];

        public static EstadoOrden_60MN OrdenDesdeCodigo(string codigo) =>
            codigosOrden.First(par => par.Value == codigo).Key;

        public static EstadoSolicitud_60MN SolicitudDesdeCodigo(string codigo) =>
            codigosSolicitud.First(par => par.Value == codigo).Key;

        public static string Descripcion(this EstadoOrden_60MN estado) => estado switch
        {
            EstadoOrden_60MN.PendienteAprobacion => "Pendiente de aprobación",
            EstadoOrden_60MN.RecibidaParcial => "Recibida parcial",
            _ => estado.ToString()
        };

        public static string Descripcion(this EstadoSolicitud_60MN estado) =>
            estado == EstadoSolicitud_60MN.EnCompra ? "En compra" : estado.ToString();
    }
}
