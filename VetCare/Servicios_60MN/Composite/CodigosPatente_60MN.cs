namespace Servicios_60MN.Composite
{
    /// <summary>
    /// Códigos de las patentes del sistema. Son los mismos valores que la
    /// columna Permiso.Codigo y que la propiedad Tag de cada ítem del menú
    /// principal (el menú oculta los ítems cuya patente no tiene el usuario).
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
        public const string Integridad = "SEG_INTEGRIDAD";             // Dígitos verificadores

        // Stock
        public const string ProductoAlta = "STK_PROD_ALTA";            // CUN11
        public const string ProductoModificar = "STK_PROD_MODIF";      // CUN12
        public const string MovimientoStock = "STK_MOVIMIENTO";        // CUN14

        // Opciones del menú todavía no implementadas ("TD"): tienen patente
        // propia para que su visibilidad también dependa del rol del usuario.
        public const string Backup = "SEG_BACKUP";
        public const string Restore = "SEG_RESTORE";
        public const string Clientes = "MAE_CLIENTES";
        public const string Proveedores = "MAE_PROVEEDORES";
        public const string Carrito = "VEN_CARRITO";
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
            BitacoraEventos, BitacoraCambios, Integridad,
            ProductoAlta, ProductoModificar, MovimientoStock,
            Backup, Restore, Clientes, Proveedores, Carrito, Facturar, Despachar, Compras, Reportes
        };
    }
}
