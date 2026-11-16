namespace Servicios_60MN.Composite
{
    /// <summary>
    /// Códigos de las patentes del sistema. Son los mismos valores que la
    /// columna Patente.Codigo y que la propiedad Tag de cada ítem del menú
    /// principal y de cada botón (se ocultan si el usuario no tiene la patente).
    /// Numeración de casos de uso del Trabajo de Diploma V2.5.
    /// </summary>
    public static class CodigosPatente_60MN
    {
        // Seguridad
        public const string UsuarioAlta = "SEG_USU_ALTA";              // CUS01
        public const string UsuarioModificar = "SEG_USU_MODIF";        // CUS02
        public const string UsuarioBaja = "SEG_USU_BAJA";              // CUS03
        public const string RolesPermisos = "SEG_ROLES";               // CUS04
        public const string UsuarioDesbloquear = "SEG_USU_DESBLOQ";    // CUS08
        public const string BitacoraEventos = "SEG_BITACORA_EVENTOS";  // CUS09
        public const string BitacoraCambios = "SEG_BITACORA_CAMBIOS";  // CUS10
        public const string BitacoraExportar = "SEG_BITACORA_EXPORT";  // CUS09: exportar la bitácora a XML
        public const string BitacoraDeserializar = "SEG_BITACORA_IMPORT";  // CUS09: deserializar (ver en pantalla) un XML de bitácora
        public const string Backup = "SEG_BACKUP";                     // CUS12 (respaldar)
        public const string Restore = "SEG_RESTORE";                   // CUS12 (restaurar)
        public const string Integridad = "SEG_INTEGRIDAD";             // CUS13

        // PN1 - Gestión de Pacientes
        public const string ClienteAlta = "PAC_CLIENTE_ALTA";          // CUN01 Gestionar Clientes (alta)
        public const string ClienteModificar = "PAC_CLIENTE_MODIF";    // CUN01 Gestionar Clientes (modificación y baja)
        public const string ClienteConsultar = "PAC_CLIENTE_CONSULTA"; // CUN01 Gestionar Clientes (consulta)
        public const string Mascota = "PAC_MASCOTA";                   // CUN02 Gestionar Mascotas
        public const string HistoriaClinicaAlta = "HC_ALTA";           // CUN03 Gestionar Historias Clínicas (alta)
        public const string HistoriaClinicaConsultar = "HC_CONSULTA";  // CUN03 Gestionar Historias Clínicas (consulta)
        public const string HistoriaClinicaModificar = "HC_MODIF";     // CUN03 Gestionar Historias Clínicas (actualización)
        public const string AtencionRegistrar = "ATN_REGISTRAR";       // CUN04 Registrar Atención
        public const string Carrito = "VEN_CARRITO";                   // CUN05 Registrar Medicación en Carrito (y ventas registradas)
        public const string InformeAtencion = "ATN_INFORME";           // CUN06 Emitir Informe de Atención

        // PN2 - Reposición de Medicamentos e Insumos (Trabajo de Diploma V2.5)
        public const string Proveedores = "MAE_PROVEEDORES";           // CUN08 Gestionar Proveedores
        public const string ProductoAlta = "STK_PROD_ALTA";            // CUN09 Administrar Productos (alta)
        public const string ProductoModificar = "STK_PROD_MODIF";      // CUN09 Administrar Productos (modificación, baja y reactivación)
        public const string SolicitudReposicion = "CMP_SOLICITUD";     // CUN10 Generar Solicitud de Reposición
        public const string OrdenCompra = "CMP_ORDEN";                 // CUN11 Registrar Orden de Compra
        public const string AprobarOrden = "CMP_APROBAR";              // CUN12 Aprobar Orden de Compra
        public const string PagoProveedor = "CMP_PAGO";                // CUN13 Registrar Pago al Proveedor
        public const string Recepcion = "CMP_RECEPCION";               // CUN14 Registrar Recepción de Mercadería
        public const string ReporteCompras = "REP_COMPRAS";            // CUN15 Generar Reporte de Compras

        // Reportes que cruzan los dos procesos: CUN07 Reporte de Atenciones y CUN16 Reporte Inteligente.
        // El sistema no factura: no hay patentes de facturación ni despacho.
        public const string Reportes = "REP_REPORTES";

        /// <summary>
        /// Patente que define a un "administrador": el sistema siempre debe
        /// conservar al menos un usuario operativo que la tenga.
        /// </summary>
        public const string Administrador = RolesPermisos;

        public static readonly string[] Todas =
        {
            UsuarioAlta, UsuarioModificar, UsuarioBaja, RolesPermisos, UsuarioDesbloquear,
            BitacoraEventos, BitacoraCambios, BitacoraExportar, BitacoraDeserializar, Backup, Restore, Integridad,
            ClienteAlta, ClienteModificar, ClienteConsultar, Mascota,
            HistoriaClinicaAlta, HistoriaClinicaConsultar, HistoriaClinicaModificar,
            AtencionRegistrar, Carrito, InformeAtencion,
            ProductoAlta, ProductoModificar, Proveedores, Reportes,
            SolicitudReposicion, OrdenCompra, AprobarOrden, PagoProveedor, Recepcion, ReporteCompras
        };
    }
}
