namespace Servicios_60MN.Composite
{
    /// <summary>
    /// Códigos de las patentes del sistema. Son los mismos valores que la
    /// columna Patente.Codigo y que la propiedad Tag de cada ítem del menú
    /// principal y de cada botón (se ocultan si el usuario no tiene la patente).
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
        public const string BitacoraImportar = "SEG_BITACORA_IMPORT";  // CUS09: importar la bitácora desde XML
        public const string Backup = "SEG_BACKUP";                     // CUS12 (respaldar)
        public const string Restore = "SEG_RESTORE";                   // CUS12 (restaurar)
        public const string Integridad = "SEG_INTEGRIDAD";             // CUS13

        // Gestión de Pacientes (RFN1)
        public const string ClienteAlta = "PAC_CLIENTE_ALTA";          // CUN01
        public const string ClienteModificar = "PAC_CLIENTE_MODIF";    // CUN02
        public const string ClienteConsultar = "PAC_CLIENTE_CONSULTA"; // CUN03
        public const string Mascota = "PAC_MASCOTA";                   // CUN04
        public const string HistoriaClinicaAlta = "HC_ALTA";           // CUN05
        public const string HistoriaClinicaConsultar = "HC_CONSULTA";  // CUN06
        public const string HistoriaClinicaModificar = "HC_MODIF";     // CUN07
        public const string AtencionRegistrar = "ATN_REGISTRAR";       // CUN08
        public const string Carrito = "VEN_CARRITO";                   // CUN09
        public const string InformeAtencion = "ATN_INFORME";           // CUN10

        // Gestión de Stock (RFN2)
        public const string ProductoAlta = "STK_PROD_ALTA";            // CUN11
        public const string ProductoModificar = "STK_PROD_MODIF";      // CUN12
        public const string ConfiguracionStock = "STK_CONFIG";         // CUN13
        public const string MovimientoStock = "STK_MOVIMIENTO";        // CUN14
        public const string ReporteExistencias = "STK_REPORTE";        // CUN15

        // Opciones del menú fuera del alcance de esta versión ("TD"): tienen
        // patente propia para que su visibilidad también dependa del perfil.
        public const string Proveedores = "MAE_PROVEEDORES";
        public const string Facturar = "VEN_FACTURAR";
        public const string Despachar = "VEN_DESPACHAR";
        public const string Compras = "COM_COMPRAS";
        public const string Reportes = "REP_REPORTES";

        /// <summary>
        /// Patente que define a un "administrador": el sistema siempre debe
        /// conservar al menos un usuario operativo que la tenga.
        /// </summary>
        public const string Administrador = RolesPermisos;

        public static readonly string[] Todas =
        {
            UsuarioAlta, UsuarioModificar, UsuarioBaja, RolesPermisos, UsuarioDesbloquear,
            BitacoraEventos, BitacoraCambios, BitacoraExportar, BitacoraImportar, Backup, Restore, Integridad,
            ClienteAlta, ClienteModificar, ClienteConsultar, Mascota,
            HistoriaClinicaAlta, HistoriaClinicaConsultar, HistoriaClinicaModificar,
            AtencionRegistrar, Carrito, InformeAtencion,
            ProductoAlta, ProductoModificar, ConfiguracionStock, MovimientoStock, ReporteExistencias,
            Proveedores, Facturar, Despachar, Compras, Reportes
        };
    }
}
