/* =====================================================================
   VetCare - Script de creación de la base de datos VetCareBD_60MN
   ---------------------------------------------------------------------
   ESTE ARCHIVO SE GENERA con Herramientas_60MN/GeneradorScriptBD_60MN
   (la sección de datos iniciales contiene hashes, datos cifrados con
   AES y dígitos verificadores calculados con las claves del App.config).
   Si se cambian VETCARE_CLAVE_CRIPTO o VETCARE_CLAVE_DV hay que volver
   a generarlo.

   ATENCIÓN: si la base ya existe, se ELIMINA y se vuelve a crear.

   Usuarios iniciales:
     admin     / Admin1234      (familia Administrador)
     operario  / Operario1234   (familia Operario de stock + patente individual Reportes)
   Usuario de emergencia (archivo UsuarioEmergencia_60MN.xml, no está en la base):
     emergencia / Emergencia1234
   ===================================================================== */

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE master;
GO

IF DB_ID(N'VetCareBD_60MN') IS NOT NULL
BEGIN
    ALTER DATABASE VetCareBD_60MN SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VetCareBD_60MN;
END
GO

CREATE DATABASE VetCareBD_60MN;
GO

USE VetCareBD_60MN;
GO

/* ---------------------------------------------------------------------
   SEGURIDAD
   --------------------------------------------------------------------- */

/* Modelo de permisos USUARIO - FAMILIA - PATENTE (patrón Composite):
     Patente         permiso atómico sobre una operación del sistema (hoja).
     Familia         conjunto de patentes = tipo de usuario (Vendedor, Veterinario...) (compuesto).
     FamiliaPatente  patentes que componen cada familia.
     FamiliaFamilia  familias incluidas dentro de otra familia (anidamiento del Composite).
     UsuarioFamilia  familia(s) asignadas al usuario.
     UsuarioPatente  patentes individuales asignadas al usuario, además de las de su familia. */

-- Clave: hash SHA-256 con sal (irreversible, T03.1). DNI y Email: AES (reversible, T03.2).
CREATE TABLE Usuario (
    UsuarioID         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuario PRIMARY KEY,
    Usuario           VARCHAR(50)       NOT NULL CONSTRAINT UX_Usuario_Usuario UNIQUE,
    Clave             VARCHAR(100)      NOT NULL,
    Nombre            VARCHAR(80)       NOT NULL,
    Apellido          VARCHAR(80)       NOT NULL,
    DNI               VARCHAR(200)      NOT NULL,
    Email             VARCHAR(400)      NOT NULL,
    Activo            BIT               NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT (1),
    Bloqueado         BIT               NOT NULL CONSTRAINT DF_Usuario_Bloqueado DEFAULT (0),
    IntentosFallidos  INT               NOT NULL CONSTRAINT DF_Usuario_Intentos DEFAULT (0),
    PrimerIngreso     BIT               NOT NULL CONSTRAINT DF_Usuario_PrimerIngreso DEFAULT (1),
    EnSesion          BIT               NOT NULL CONSTRAINT DF_Usuario_EnSesion DEFAULT (0),
    DVH               CHAR(64)          NULL
);
GO

CREATE TABLE Patente (
    PatenteID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Patente PRIMARY KEY,
    Nombre     VARCHAR(100)      NOT NULL CONSTRAINT UX_Patente_Nombre UNIQUE,
    Codigo     VARCHAR(50)       NOT NULL CONSTRAINT UX_Patente_Codigo UNIQUE,
    DVH        CHAR(64)          NULL
);
GO

CREATE TABLE Familia (
    FamiliaID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Familia PRIMARY KEY,
    Nombre     VARCHAR(100)      NOT NULL CONSTRAINT UX_Familia_Nombre UNIQUE,
    DVH        CHAR(64)          NULL
);
GO

CREATE TABLE FamiliaPatente (
    FamiliaID  INT      NOT NULL CONSTRAINT FK_FamiliaPatente_Familia REFERENCES Familia (FamiliaID),
    PatenteID  INT      NOT NULL CONSTRAINT FK_FamiliaPatente_Patente REFERENCES Patente (PatenteID),
    DVH        CHAR(64) NULL,
    CONSTRAINT PK_FamiliaPatente PRIMARY KEY (FamiliaID, PatenteID)
);
GO

CREATE TABLE FamiliaFamilia (
    FamiliaPadreID  INT      NOT NULL CONSTRAINT FK_FamiliaFamilia_Padre REFERENCES Familia (FamiliaID),
    FamiliaHijaID   INT      NOT NULL CONSTRAINT FK_FamiliaFamilia_Hija  REFERENCES Familia (FamiliaID),
    DVH             CHAR(64) NULL,
    CONSTRAINT PK_FamiliaFamilia PRIMARY KEY (FamiliaPadreID, FamiliaHijaID),
    CONSTRAINT CK_FamiliaFamilia_Ciclo CHECK (FamiliaPadreID <> FamiliaHijaID)
);
GO

-- ON DELETE CASCADE hacia Usuario: si se borra un usuario directamente en la base,
-- desaparecen también sus asignaciones y los DVV de estas tablas lo delatan.
CREATE TABLE UsuarioFamilia (
    UsuarioID  INT      NOT NULL CONSTRAINT FK_UsuarioFamilia_Usuario REFERENCES Usuario (UsuarioID) ON DELETE CASCADE,
    FamiliaID  INT      NOT NULL CONSTRAINT FK_UsuarioFamilia_Familia REFERENCES Familia (FamiliaID),
    DVH        CHAR(64) NULL,
    CONSTRAINT PK_UsuarioFamilia PRIMARY KEY (UsuarioID, FamiliaID)
);
GO

CREATE TABLE UsuarioPatente (
    UsuarioID  INT      NOT NULL CONSTRAINT FK_UsuarioPatente_Usuario REFERENCES Usuario (UsuarioID) ON DELETE CASCADE,
    PatenteID  INT      NOT NULL CONSTRAINT FK_UsuarioPatente_Patente REFERENCES Patente (PatenteID),
    DVH        CHAR(64) NULL,
    CONSTRAINT PK_UsuarioPatente PRIMARY KEY (UsuarioID, PatenteID)
);
GO

-- Bitácora de Eventos (T06A). Sin FK a Usuario a propósito: el historial
-- debe conservarse aunque el registro del usuario se elimine de la base.
CREATE TABLE Bitacora (
    BitacoraID   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Bitacora PRIMARY KEY,
    UsuarioID    INT               NULL,
    Usuario      VARCHAR(50)       NOT NULL,
    FechaHora    DATETIME2(0)      NOT NULL,
    Modulo       VARCHAR(30)       NOT NULL,
    Evento       VARCHAR(50)       NOT NULL,
    Criticidad   INT               NOT NULL CONSTRAINT CK_Bitacora_Criticidad CHECK (Criticidad BETWEEN 1 AND 5),
    Descripcion  VARCHAR(1500)     NOT NULL,
    DVH          CHAR(64)          NULL
);
CREATE INDEX IX_Bitacora_FechaHora ON Bitacora (FechaHora);
GO

-- Dígito verificador vertical: uno por tabla protegida.
CREATE TABLE DVV (
    Tabla  VARCHAR(50) NOT NULL CONSTRAINT PK_DVV PRIMARY KEY,
    DVV    CHAR(64)    NOT NULL
);
GO

/* ---------------------------------------------------------------------
   STOCK E INSUMOS (PN4)
   --------------------------------------------------------------------- */

CREATE TABLE Categoria (
    CategoriaID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categoria PRIMARY KEY,
    Nombre       VARCHAR(80)       NOT NULL CONSTRAINT UX_Categoria_Nombre UNIQUE,
    Margen       DECIMAL(6,2)      NULL      -- margen particular (%); NULL = usa el margen general
);
GO

CREATE TABLE Proveedor (
    ProveedorID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Proveedor PRIMARY KEY,
    RazonSocial  VARCHAR(120)      NOT NULL,
    CUIT         VARCHAR(13)       NOT NULL CONSTRAINT UX_Proveedor_CUIT UNIQUE
);
GO

CREATE TABLE Configuracion (
    Clave  VARCHAR(50)  NOT NULL CONSTRAINT PK_Configuracion PRIMARY KEY,
    Valor  VARCHAR(100) NOT NULL
);
GO

CREATE TABLE Productos (
    ProductoID           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Productos PRIMARY KEY,
    Codigo               AS ('PRD-' + RIGHT('00000' + CAST(ProductoID AS VARCHAR(10)), 5)) PERSISTED,
    Nombre               VARCHAR(120)      NOT NULL,
    CategoriaID          INT               NOT NULL CONSTRAINT FK_Productos_Categoria REFERENCES Categoria (CategoriaID),
    ProveedorID          INT               NOT NULL CONSTRAINT FK_Productos_Proveedor REFERENCES Proveedor (ProveedorID),
    StockActual          INT               NOT NULL CONSTRAINT CK_Productos_Stock CHECK (StockActual >= 0),
    StockMinimo          INT               NOT NULL CONSTRAINT CK_Productos_StockMinimo CHECK (StockMinimo >= 0),
    PrecioCosto          DECIMAL(12,2)     NOT NULL CONSTRAINT CK_Productos_Costo CHECK (PrecioCosto >= 0),
    PrecioVenta          DECIMAL(12,2)     NOT NULL CONSTRAINT CK_Productos_Venta CHECK (PrecioVenta >= 0),
    FechaVencimiento     DATE              NOT NULL,
    Lote                 VARCHAR(40)       NOT NULL,
    Activo               BIT               NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT (1),
    UsuarioModificacion  INT               NULL,  -- usuario de la sesión que hizo el último cambio (lo lee el trigger)
    DVH                  CHAR(64)          NULL,
    CONSTRAINT UX_Productos_NombreLote UNIQUE (Nombre, Lote)
);
GO

-- Bitácora de Cambios (T06B): historial completo de Productos. Act = 1 marca la versión vigente.
CREATE TABLE Productos_C (
    ProductoCID       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Productos_C PRIMARY KEY,
    ProductoID        INT               NOT NULL,
    Codigo            VARCHAR(20)       NOT NULL,
    Nombre            VARCHAR(120)      NOT NULL,
    CategoriaID       INT               NOT NULL,
    ProveedorID       INT               NOT NULL,
    StockActual       INT               NOT NULL,
    StockMinimo       INT               NOT NULL,
    PrecioCosto       DECIMAL(12,2)     NOT NULL,
    PrecioVenta       DECIMAL(12,2)     NOT NULL,
    FechaVencimiento  DATE              NOT NULL,
    Lote              VARCHAR(40)       NOT NULL,
    ActivoProducto    BIT               NOT NULL,
    Operacion         VARCHAR(20)       NOT NULL,  -- ALTA, MODIFICACION, MOVIMIENTO, BAJA, REACTIVACION, ELIMINACION
    FechaHora         DATETIME2(0)      NOT NULL CONSTRAINT DF_Productos_C_FechaHora DEFAULT (SYSDATETIME()),
    UsuarioID         INT               NULL,
    Act               BIT               NOT NULL
);
CREATE INDEX IX_Productos_C_Producto ON Productos_C (ProductoID, Act);
GO

CREATE TABLE MovimientoStock (
    MovimientoID     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_MovimientoStock PRIMARY KEY,
    ProductoID       INT               NOT NULL CONSTRAINT FK_MovimientoStock_Producto REFERENCES Productos (ProductoID),
    Tipo             VARCHAR(10)       NOT NULL CONSTRAINT CK_MovimientoStock_Tipo CHECK (Tipo IN ('INGRESO', 'EGRESO', 'AJUSTE')),
    Cantidad         INT               NOT NULL,  -- con signo: + suma stock, - resta stock
    StockAnterior    INT               NOT NULL,
    StockResultante  INT               NOT NULL,
    Motivo           VARCHAR(200)      NOT NULL,
    FechaHora        DATETIME2(0)      NOT NULL,
    UsuarioID        INT               NULL,
    DVH              CHAR(64)          NULL
);
CREATE INDEX IX_MovimientoStock_Producto ON MovimientoStock (ProductoID, FechaHora);
GO

CREATE TABLE AlertaReposicion (
    AlertaID    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AlertaReposicion PRIMARY KEY,
    ProductoID  INT               NOT NULL CONSTRAINT FK_AlertaReposicion_Producto REFERENCES Productos (ProductoID),
    FechaHora   DATETIME2(0)      NOT NULL CONSTRAINT DF_AlertaReposicion_FechaHora DEFAULT (SYSDATETIME()),
    Activa      BIT               NOT NULL CONSTRAINT DF_AlertaReposicion_Activa DEFAULT (1)
);
GO

/* ---------------------------------------------------------------------
   TRIGGER DE LA BITÁCORA DE CAMBIOS (T06B)
   Ante cada alta, modificación o eliminación en Productos:
     - pone Act = 0 en las versiones anteriores del producto en Productos_C
     - inserta el nuevo estado con Act = 1, la operación, fecha/hora y usuario
   Las actualizaciones que solo recalculan el DVH no generan historial.
   --------------------------------------------------------------------- */
CREATE TRIGGER TR_Productos_BitacoraCambios
ON Productos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @hayInsertados BIT = CASE WHEN EXISTS (SELECT 1 FROM inserted) THEN 1 ELSE 0 END;
    DECLARE @hayEliminados BIT = CASE WHEN EXISTS (SELECT 1 FROM deleted) THEN 1 ELSE 0 END;

    IF @hayInsertados = 0 AND @hayEliminados = 0
        RETURN;

    IF @hayInsertados = 1 AND @hayEliminados = 1
       AND NOT (UPDATE(Nombre) OR UPDATE(CategoriaID) OR UPDATE(ProveedorID) OR UPDATE(StockActual)
                OR UPDATE(StockMinimo) OR UPDATE(PrecioCosto) OR UPDATE(PrecioVenta)
                OR UPDATE(FechaVencimiento) OR UPDATE(Lote) OR UPDATE(Activo))
        RETURN;

    -- Cambio exclusivo de existencias (CUN14): se registra como MOVIMIENTO.
    DECLARE @soloStock BIT = 0;
    IF UPDATE(StockActual)
       AND NOT (UPDATE(Nombre) OR UPDATE(CategoriaID) OR UPDATE(ProveedorID) OR UPDATE(StockMinimo)
                OR UPDATE(PrecioCosto) OR UPDATE(PrecioVenta) OR UPDATE(FechaVencimiento) OR UPDATE(Lote))
        SET @soloStock = 1;

    UPDATE c
       SET Act = 0
      FROM Productos_C c
     WHERE c.Act = 1
       AND c.ProductoID IN (SELECT ProductoID FROM inserted UNION SELECT ProductoID FROM deleted);

    IF @hayInsertados = 1
        INSERT INTO Productos_C (ProductoID, Codigo, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo,
                                 PrecioCosto, PrecioVenta, FechaVencimiento, Lote, ActivoProducto,
                                 Operacion, FechaHora, UsuarioID, Act)
        SELECT i.ProductoID, i.Codigo, i.Nombre, i.CategoriaID, i.ProveedorID, i.StockActual, i.StockMinimo,
               i.PrecioCosto, i.PrecioVenta, i.FechaVencimiento, i.Lote, i.Activo,
               CASE
                   WHEN d.ProductoID IS NULL THEN 'ALTA'
                   WHEN d.Activo = 1 AND i.Activo = 0 THEN 'BAJA'
                   WHEN d.Activo = 0 AND i.Activo = 1 THEN 'REACTIVACION'
                   WHEN @soloStock = 1 THEN 'MOVIMIENTO'
                   ELSE 'MODIFICACION'
               END,
               SYSDATETIME(),
               COALESCE(i.UsuarioModificacion, CAST(SESSION_CONTEXT(N'UsuarioID') AS INT)),
               1
          FROM inserted i
          LEFT JOIN deleted d ON d.ProductoID = i.ProductoID;
    ELSE
        INSERT INTO Productos_C (ProductoID, Codigo, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo,
                                 PrecioCosto, PrecioVenta, FechaVencimiento, Lote, ActivoProducto,
                                 Operacion, FechaHora, UsuarioID, Act)
        SELECT d.ProductoID, d.Codigo, d.Nombre, d.CategoriaID, d.ProveedorID, d.StockActual, d.StockMinimo,
               d.PrecioCosto, d.PrecioVenta, d.FechaVencimiento, d.Lote, d.Activo,
               'ELIMINACION', SYSDATETIME(), CAST(SESSION_CONTEXT(N'UsuarioID') AS INT), 1
          FROM deleted d;
END
GO

/* ---------------------------------------------------------------------
   MULTI-IDIOMA (patrón Observer en la GUI)
   - El español es el idioma BASE: sus textos son los del diseñador de
     Visual Studio, por eso no necesita filas en Traduccion.
   - Clave de traducción: "Formulario.Control" (específica) o "Control"
     (genérica, compartida por todos los formularios). Para textos que
     arma el código se usan claves "msg.*" / "estado.*".
   - Predeterminado = 1 marca el idioma con el que abre el próximo login.
   --------------------------------------------------------------------- */
CREATE TABLE Idioma (
    IdiomaID        INT          NOT NULL CONSTRAINT PK_Idioma PRIMARY KEY,
    Nombre          NVARCHAR(50) NOT NULL,
    Codigo          VARCHAR(5)   NOT NULL CONSTRAINT UX_Idioma_Codigo UNIQUE,
    EsBase          BIT          NOT NULL,
    Predeterminado  BIT          NOT NULL
);
GO

CREATE TABLE Traduccion (
    IdiomaID  INT           NOT NULL CONSTRAINT FK_Traduccion_Idioma REFERENCES Idioma (IdiomaID),
    Clave     VARCHAR(120)  NOT NULL,
    Texto     NVARCHAR(400) NOT NULL,
    CONSTRAINT PK_Traduccion PRIMARY KEY (IdiomaID, Clave)
);
GO

INSERT INTO Idioma (IdiomaID, Nombre, Codigo, EsBase, Predeterminado) VALUES
    (1, N'Español', 'es', 1, 1),
    (2, N'English', 'en', 0, 0);
GO

INSERT INTO Traduccion (IdiomaID, Clave, Texto) VALUES
-- Genéricas (cualquier formulario)
(2, 'btnAceptar', N'Accept'), (2, 'btnCancelar', N'Cancel'), (2, 'btnBuscar', N'Search'), (2, 'btnCerrar', N'Close'),
(2, 'btnModificar', N'Modify'), (2, 'btnDarBaja', N'Deactivate'), (2, 'btnReactivar', N'Reactivate'),
(2, 'lblDesde', N'From'), (2, 'lblHasta', N'To'), (2, 'lblCodigo', N'Code'), (2, 'lblNombre', N'Name'),
(2, 'lblUsuario', N'User'), (2, 'lblCategoria', N'Category'),
(2, 'colUsuario', N'User'), (2, 'colNombre', N'Name'), (2, 'colApellido', N'Last name'), (2, 'colCodigo', N'Code'),
(2, 'colFecha', N'Date and time'), (2, 'colFechaHora', N'Date and time'), (2, 'colCategoria', N'Category'),
(2, 'colProveedor', N'Supplier'), (2, 'colStock', N'Stock'), (2, 'colStockMinimo', N'Minimum'), (2, 'colCosto', N'Cost'),
(2, 'colVenta', N'Price'), (2, 'colVencimiento', N'Expiration'), (2, 'colLote', N'Batch'), (2, 'colEstado', N'Status'),
-- Mensajes y textos armados por el código
(2, 'msg.Todos', N'(All)'), (2, 'msg.EnDesarrollo', N'Feature under development (TD).'),
(2, 'msg.ErrorInesperado', N'An unexpected error occurred:'),
(2, 'msg.ConfirmarReLogin', N'Do you want to close the session and sign in again?'),
(2, 'msg.ConfirmarLogout', N'Do you want to close the session and exit VetCare?'),
(2, 'msg.ConfirmarSalir', N'Do you want to exit VetCare?'),
(2, 'msg.VentanasAbiertas', N'There are {0} open window(s). Unsaved operations will be lost. Continue?'),
(2, 'msg.IdiomaGuardado', N'Language changed. It will be the default language the next time you sign in.'),
(2, 'estado.Usuario', N'User'), (2, 'estado.Familias', N'Families'), (2, 'estado.PatentesIndividuales', N'individual patent(s)'), (2, 'estado.Idioma', N'Language'),
(2, 'estado.Integridad', N'Integrity inconsistencies detected'), (2, 'estado.Emergencia', N'EMERGENCY USER'),
-- Menú principal
(2, 'FrmMenuPrincipal_60MN', N'VetCare - Veterinary management system'),
(2, 'mnuAdmin', N'ADMIN'), (2, 'mnuUsuarios', N'Users'), (2, 'mnuPerfiles', N'Profiles'), (2, 'mnuBackup', N'Backup'),
(2, 'mnuRestore', N'Restore'), (2, 'mnuBitacoraEventos', N'Event log'), (2, 'mnuDigitosVerificadores', N'Check digits'),
(2, 'mnuMaestros', N'MASTER DATA'), (2, 'mnuProductos', N'Products'), (2, 'mnuMovimientosStock', N'Stock movements'),
(2, 'mnuClientes', N'Customers'), (2, 'mnuProveedores', N'Suppliers'), (2, 'mnuBitacoraCambios', N'Change log'),
(2, 'mnuUsuario', N'USER'), (2, 'mnuReLogin', N'Re-Login'), (2, 'mnuCambiarClave', N'Change password'),
(2, 'mnuLogout', N'Logout'), (2, 'mnuCambiarIdioma', N'Change language'),
(2, 'mnuVentas', N'SALES'), (2, 'mnuCarrito', N'Cart'), (2, 'mnuFacturar', N'Invoice'), (2, 'mnuDespachar', N'Dispatch'),
(2, 'mnuCompras', N'PURCHASES'), (2, 'mnuGestionCompras', N'Purchases'),
(2, 'mnuReportes', N'REPORTS'), (2, 'mnuReporte1', N'Report 1'), (2, 'mnuReporte2', N'Report 2'), (2, 'mnuReporte3', N'Report 3'),
(2, 'mnuAyuda', N'HELP'), (2, 'mnuAyudaEnLinea', N'Online help (F1)'), (2, 'mnuAcercaDe', N'About VetCare'),
-- Login
(2, 'FrmLogin_60MN', N'VetCare - Sign in'), (2, 'FrmLogin_60MN.lblIdioma', N'Change language'),
(2, 'FrmLogin_60MN.lblSubtitulo', N'Sign in'), (2, 'FrmLogin_60MN.lblUsuario', N'Login'), (2, 'FrmLogin_60MN.lblClave', N'Password'),
(2, 'FrmLogin_60MN.btnAceptar', N'Enter'), (2, 'FrmLogin_60MN.btnCancelar', N'Exit'),
-- Cambiar idioma
(2, 'FrmCambiarIdioma_60MN', N'Change language'), (2, 'FrmCambiarIdioma_60MN.lblIdioma', N'Language'),
(2, 'FrmCambiarIdioma_60MN.lblNota', N'The selected language will be the default the next time you sign in.'),
-- Cambiar contraseña
(2, 'FrmCambiarClave_60MN', N'Change password'), (2, 'FrmCambiarClave_60MN.lblInfo', N'Enter your current password and the new password.'),
(2, 'FrmCambiarClave_60MN.lblActual', N'Current password'), (2, 'FrmCambiarClave_60MN.lblNueva', N'New password'),
(2, 'FrmCambiarClave_60MN.lblConfirmacion', N'Confirm password'), (2, 'FrmCambiarClave_60MN.lblInfoPrimerIngreso', N'First sign in: replace the initial password.'),
(2, 'FrmCambiarClave_60MN.lblPolitica', N'The password must have at least 8 characters and include uppercase, lowercase and numbers.'),
-- Gestión de usuarios
(2, 'FrmGestionUsuarios_60MN', N'User management'), (2, 'FrmGestionUsuarios_60MN.lblTitulo', N'User management'),
(2, 'chkMostrarBajas', N'Show deactivated users'), (2, 'colDni', N'ID number'), (2, 'colEmail', N'Email'), (2, 'colFamilias', N'Families'), (2, 'colPatentesIndividuales', N'Individual patents'),
(2, 'colIntentos', N'Failed attempts'), (2, 'btnAnadir', N'Add'), (2, 'btnDesbloquear', N'Unlock'),
(2, 'btnBlanquear', N'Reset password'), (2, 'btnActualizar', N'Refresh'), (2, 'btnPermisos', N'Permissions'),
-- Usuario (alta / modificación)
(2, 'FrmUsuario_60MN', N'User'), (2, 'FrmUsuario_60MN.lblApellido', N'Last name *'), (2, 'FrmUsuario_60MN.lblNombre', N'Name *'),
(2, 'FrmUsuario_60MN.lblDni', N'ID number *'), (2, 'FrmUsuario_60MN.lblEmail', N'Email *'),
(2, 'FrmUsuario_60MN.lblUsuario', N'User name *'), (2, 'FrmUsuario_60MN.lblFamilia', N'Family *'),
(2, 'FrmUsuario_60MN.lblNota', N'On accept, the system generates an initial password that the user must change on first sign in.'),
(2, 'FrmUsuario_60MN.TituloAlta', N'Add user'), (2, 'FrmUsuario_60MN.TituloModificacion', N'Modify user'),
(2, 'FrmUsuario_60MN.NotaModificacion', N'The ID number and the user name cannot be modified. Families and individual patents are assigned with the Permissions button.'),
-- Familias y patentes
(2, 'FrmFamiliasPatentes_60MN', N'Families and patents'), (2, 'FrmFamiliasPatentes_60MN.lblTitulo', N'Families and patents'),
(2, 'lblListaFamilias', N'Families (user types)'), (2, 'btnNuevaFamilia', N'New family'), (2, 'btnEliminarFamilia', N'Delete family'),
(2, 'lblNombreFamilia', N'Family name'), (2, 'lblPatentes', N'Family patents'), (2, 'lblSubfamilias', N'Included families'),
(2, 'lblArbol', N'Family permission tree'), (2, 'btnGuardar', N'Accept'), (2, 'btnDescartar', N'Cancel'),
(2, 'msg.UsuariosConFamilia', N'Active users with this family: {0}'), (2, 'msg.FamiliaNueva', N'New family'),
-- Permisos del usuario (familias + patentes individuales)
(2, 'FrmPermisosUsuario_60MN', N'User permissions'), (2, 'FrmPermisosUsuario_60MN.lblTitulo', N'User permissions'),
(2, 'lblFamiliasUsuario', N'Families (user type)'), (2, 'lblPatentesUsuario', N'Individual patents (in addition to those of the family)'),
(2, 'lblPermisosEfectivos', N'Effective permissions of the user'), (2, 'msg.NodoFamilias', N'Families (UsuarioFamilia)'),
(2, 'msg.NodoPatentesIndividuales', N'Individual patents (UsuarioPatente)'), (2, 'msg.PatentesEfectivas', N'Effective patents: {0}'),
-- Bitácora de eventos
(2, 'FrmBitacoraEventos_60MN', N'Event log'), (2, 'FrmBitacoraEventos_60MN.lblTitulo', N'Event log'),
(2, 'lblCriticidad', N'Criticality'), (2, 'lblModulo', N'Module'), (2, 'lblEvento', N'Event'), (2, 'btnLimpiar', N'Clear filters'),
(2, 'colModulo', N'Module'), (2, 'colEvento', N'Event'), (2, 'colCriticidad', N'Criticality'), (2, 'colDescripcion', N'Description'),
-- Bitácora de cambios
(2, 'FrmBitacoraCambios_60MN', N'Change log'), (2, 'FrmBitacoraCambios_60MN.lblTitulo', N'Change log - Products'),
(2, 'colAct', N'Current'), (2, 'colOperacion', N'Operation'),
(2, 'lblAyuda', N'Current version (Act = 1) in green. Select two versions with Ctrl to compare them.'),
(2, 'btnComparar', N'Compare versions'), (2, 'btnRestaurar', N'Restore version'),
(2, 'FrmCompararVersiones_60MN', N'Compare versions'), (2, 'colCampo', N'Field'), (2, 'colVersionA', N'Previous version'),
(2, 'colVersionB', N'Later version'), (2, 'lblReferencia', N'Highlighted fields changed between versions.'),
-- Integridad
(2, 'FrmIntegridad_60MN', N'Data integrity'), (2, 'FrmIntegridad_60MN.lblTitulo', N'Data integrity'),
(2, 'lblDescripcion', N'Checks the horizontal (DVH, one per row) and vertical (DVV, one per table) check digits of Usuario, Familia, Patente, their relation tables, Bitacora, Productos and MovimientoStock.'),
(2, 'colTabla', N'Table'), (2, 'colTipo', N'Type'), (2, 'colRegistro', N'Record (key)'), (2, 'colDetalle', N'Detail'),
(2, 'btnVerificar', N'Verify'), (2, 'btnRecalcular', N'Recalculate digits'),
-- Productos
(2, 'FrmProductos_60MN', N'Products'), (2, 'FrmProductos_60MN.lblTitulo', N'Products and supplies (Stock)'),
(2, 'lblBuscar', N'Search'), (2, 'chkInactivos', N'Include deactivated'), (2, 'btnNuevo', N'New product'),
(2, 'btnMovimiento', N'Movement'),
(2, 'FrmProducto_60MN', N'Product'), (2, 'FrmProducto_60MN.lblNombre', N'Name *'), (2, 'FrmProducto_60MN.lblCategoria', N'Category *'),
(2, 'lblProveedor', N'Supplier *'), (2, 'lblStockActual', N'Current stock *'), (2, 'lblStockMinimo', N'Minimum stock *'),
(2, 'lblPrecioCosto', N'Cost price *'), (2, 'lblPrecioVenta', N'Sale price *'), (2, 'btnUsarSugerido', N'Use suggested'),
(2, 'lblVencimiento', N'Expiration date *'), (2, 'lblLote', N'Batch *'),
(2, 'FrmProducto_60MN.TituloAlta', N'Register product'), (2, 'FrmProducto_60MN.TituloModificacion', N'Modify product'),
(2, 'FrmProducto_60MN.NotaAlta', N'The sale price is suggested by applying the profit margin of the category or the general one.'),
(2, 'FrmProducto_60MN.NotaModificacion', N'Current stock cannot be modified here: register a stock movement so the adjustment is justified and traced.'),
-- Movimientos de stock
(2, 'FrmMovimientoStock_60MN', N'Stock movements'), (2, 'FrmMovimientoStock_60MN.lblTitulo', N'Stock movements'),
(2, 'FrmMovimientoStock_60MN.lblProducto', N'Product *'), (2, 'FrmMovimientoStock_60MN.btnCancelar', N'Close'),
(2, 'grpTipo', N'Movement type'), (2, 'rdoIngreso', N'Receipt'), (2, 'rdoEgreso', N'Issue'), (2, 'rdoAjuste', N'Adjustment'),
(2, 'grpAjuste', N'Adjustment'), (2, 'rdoSumar', N'Add (inventory difference)'), (2, 'rdoRestar', N'Subtract (breakage, loss...)'),
(2, 'chkVencimiento', N'Expiration adjustment: write off the whole batch (stock to 0)'),
(2, 'lblCantidad', N'Quantity *'), (2, 'lblMotivo', N'Reason *'), (2, 'lblHistorial', N'Latest movements of the product'),
(2, 'colCantidad', N'Quantity'), (2, 'colAnterior', N'Previous stock'), (2, 'colResultante', N'Resulting stock'), (2, 'colMotivo', N'Reason');
GO

/* ---------------------------------------------------------------------
   DATOS INICIALES (generados: hashes, AES y dígitos verificadores)
   --------------------------------------------------------------------- */

SET IDENTITY_INSERT Patente ON;
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (1, N'Crear usuario', N'SEG_USU_ALTA', '713DB77375CEE44E8E8E41C0A4B3D322DEB0E54DF52A09003CED85931B9CDA58');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (2, N'Modificar usuario', N'SEG_USU_MODIF', 'AA23AFEB1ABD08016252F876B982C8E935B506E9D1DA667AA0267F0CD80634EE');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (3, N'Dar de baja / reactivar usuario', N'SEG_USU_BAJA', '67BCD6A8AB0264FA76880F6BC87E28CCD349A53A09650B1144F6F00AF2764A0C');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (4, N'Gestionar familias y permisos', N'SEG_ROLES', '2B430D067D27EDBF01F1A57E9C1E9150C68BCA04429071A5850E9BF2568B073B');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (5, N'Desbloquear usuario', N'SEG_USU_DESBLOQ', '6AB208E5D109462EBC1306ED559231BAE2E1FD8AB6A6CE4DCBF169B953A5416B');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (6, N'Consultar bitácora de eventos', N'SEG_BITACORA_EVENTOS', '9F6E22847570B3DAB6CF1EFE5C723F0EC1A529BCB8867F5A216CB9242BF6AD40');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (7, N'Consultar bitácora de cambios', N'SEG_BITACORA_CAMBIOS', 'A8D62552CCF198389C2194EA70106360E23CCBE8198DC03B3186EF663EC2E8E3');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (8, N'Verificar integridad (dígitos verificadores)', N'SEG_INTEGRIDAD', 'CB9758C9318562D7FC81696CA9CC457BBDA5D2D34EAD40F7F906DF79D59AF218');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (9, N'Registrar producto', N'STK_PROD_ALTA', 'E7328E20EFC5D0CBDEF7E783E98381EC9343C6C9AAC4AD7024505D2C9F6C663D');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (10, N'Modificar producto', N'STK_PROD_MODIF', '85CB64B7D35512E2AB5B49A8CA54AAAAAF8624E20F266CCE29491224BB3429F6');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (11, N'Registrar movimiento de stock', N'STK_MOVIMIENTO', '271CDA998BBA067C68C87A86F80DB0ABC585DD5F201B369359F837784B1CC954');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (12, N'Backup', N'SEG_BACKUP', 'B861E8E290ECFA5B0A55649C8EE42170BEA70543BE8DBEA834419D3FA7E4F098');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (13, N'Restore', N'SEG_RESTORE', '374D235D5A98A2411337322990E03EF5901A17059FB02A20BB3C2EDD9BD68BB1');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (14, N'Clientes', N'MAE_CLIENTES', '40E0DF5E8560F1339A1991D96AFE3497A2DAB0A6B1243059100B9945019FF2B5');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (15, N'Proveedores', N'MAE_PROVEEDORES', 'AD0549B86DA5BCE018979C3AA0B758D57AEBA4B8BDD3BBD53562BCB9E88C414F');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (16, N'Carrito', N'VEN_CARRITO', '64012B0BA247EA97A0021EC3DEFE2D03443F8043B7C5FCAC6BF414BC29589594');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (17, N'Facturar', N'VEN_FACTURAR', '312181795D84058D190034A855D49C63BFB9300DFA352C887C701F836285010A');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (18, N'Despachar', N'VEN_DESPACHAR', '8C9EA69BF7F4B236846DCFD27B29BA1C9460FFDDEE363DE1674702D4475F609B');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (19, N'Compras', N'COM_COMPRAS', '9BF1E68A3D5CAA5132F7BE44A93F348D0B1A7F0E33398D108E731B2B952AA455');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (20, N'Reportes', N'REP_REPORTES', 'AF5821064E60B71A69C1CF7D737E7DDD3D02A1DF5575EC2B24C0F85452A7D470');
SET IDENTITY_INSERT Patente OFF;

SET IDENTITY_INSERT Familia ON;
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (1, N'Administrador', '4236F25C0643A0CE7457BBF3E34E6A16AB3931FC03C6FFC9C496C94A3DE13564');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (2, N'Gestión de Usuarios', '89E2A52D6A39A527603F46F8905D18F1958A9D56E14660B471B3AE3A30D79F7C');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (3, N'Auditoría', '31494F81786CDEBC84406735CFD1F49DD40FBACC2F547A68CF2451A76F20635F');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (4, N'Operario de stock', '90E76A6FC4994643F01C76E6FC905FF846D0119EE9204EC151D8D05DC8455AD5');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (5, N'Ventas', 'F838200EC6CC864D3A1670C3F24280FE678094CE1A954267791DB85E625114BF');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (6, N'Vendedor', '84711E5991E139AB9057BE4FFC94E3DCBB4EF94E07CC4CE22BEE0DA9026D3811');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (7, N'Veterinario', '7B9FEFB6DA33007AAC819F5D9509060C3F12E54FDC3C5F69A6E5F44765AF92BF');
SET IDENTITY_INSERT Familia OFF;

INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 4, 'EFA56CA206719F3AD5536742F050CA48D5A9D889D76284D2D79ED572010E61C8');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 12, 'F9195FF5FB849D30BC91768A3CC39388D16B35ACB20C4811FD82AB79B1CAE5D9');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 13, '2768C1579E83E443F1AA76170AB50B0A4EA4AB4ADE255592E2AC84E7804BFAAB');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 14, 'AF1E3AAE8888D4B5FBEA6522A7C0305344B64372DF9E6105BE4CBC1C2918BE58');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 15, 'C0A3438FE5589D050A1938B32F434B3083E66AAB9D72DF9207151133437FA173');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 19, '8F6975E3F8C05B69A2A96EA58D1A2C39F4794868CEAD6168206BECD4D9A54271');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 20, '7EDEC7C3D7A5CBE309DFCBA27A4ACE31D448B9CB58C545B3F3D3BA403911FFB0');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 1, '75552B08D0E0493C69BB75B221AF305A7D6D7000DEFAB6C73DA0DB6F3740CA1E');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 2, 'E9743ABF50693F514F2138D468B0F043875F88A48B887A57BCCF36555C0034F1');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 3, 'F208A336557A8A4DDABF10C983EB2357068975E845D1BF9C17E47EE81BF109D1');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 5, 'E6D25A07E94E832CB0A795237323E7FF3257F121414DECA5A6EF1EC2DB0E4BA6');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (3, 6, '454217652E98A6BEA4B68B7E0916D22E18AEDACE87DE9251B327CDB9F3BD2229');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (3, 7, '9EEBDF5E3F7E97113E09C686102E3072C657B9FF8EB99757A8E655688B62FA6B');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (3, 8, 'C8913C97B1F67FA8728FF8BEA2757354785D47DB7F016441B02E43F99B33E465');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 9, 'D2BD19E76BEF5275AD797024CC242C22437068D2A9F5F6716C6EB7782F782EF1');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 10, 'FEAC9A3AAE85A17F5EA5B4A1E51930646B9239C63F29DE2E7DBED2261D236B5D');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 11, 'A55CDDE32AE737DA87B844DF207872DD8437E3639071D433777F2976BD37E7A7');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 16, '65F2970C5324907ECD94EA0EC49560DD1189F48FE3D9E2C1189AD9F7A7D51410');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 17, '62868254DFE868815371618424BD9F2794CA27EA23EB78EBDB6A8E2BDBB1B506');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 18, '1D0C157230A3555315B37A3712BB0FF5684092DD5369DFB7FA9FA748815669F3');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 14, 'BD482263F068E0469F13DC7A3848EDA6EAB4EAC3F474236CBE8EF13124BEC8C2');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (7, 14, 'AA799687E8F0D73644EA2639B8E10F1390490A109EE22EEF05142DFD2AACFDCA');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (7, 20, '888F448DA30F5F5F13A1EB35D635726400A0640F473B00F8F4DCFF32D6B9DBF7');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 2, 'CB4235A38D71766EE931064D369A34262391A1AB19175AECDF86C33ACFDEDC0B');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 3, 'DFBFD56D72390C4C5449D60ABBA865CF536212E8BF1549D229C0EDBE7E64A8D3');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 4, 'A89F60F5A85FEC6AF3111681D0A1EB2AF8265992322DADCBA7726AD1DD4C9A1E');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 5, 'E8830CD3829711FF83A0B6089E43FDD6EE52B35D17222578436988885DA74FAF');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (6, 5, 'C1D0AC5106DA80212F4353D3214498D67F0780C8DFA105D60145CB0EE961DB6B');
GO

SET IDENTITY_INSERT Usuario ON;
INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)
     VALUES (1, N'admin', N'yLBWp/h8Yhxs1ZWUmDqfHg==.JrQI0by14BDr6YHhEUM/k3BzGLRY1qeXT04142gYFcY=', N'Mariano', N'Núñez', N'4kvq0X4jP0bXPdtLrMS2kw==.z1ChpqYWRCKxJzlwuWlbEQ==', N'NK1Z+U59uVGo1sLOoN/BJQ==.XLpow11P/hEW8kMkqoNVXWf66Fl6ooySUBuwV68sNF4=', 1, 0, 0, 0, 0, '07336D471089A2D98F6D531A0B6DA43E0BB70FDFF7EB1EA3D7503A5225BEFD1B');
INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)
     VALUES (2, N'operario', N'xpfh9MDm2VsPMtNMOPKqRA==.sDZwLvdW4yIP9pgFMReEUP5yTT+rru8tTL2+xoBRQcU=', N'Sofía', N'Ramírez', N'2GVKzPtyULNJbcGpIflFkg==.2drKPxLnMqPCdpvF8znI5g==', N't7gKSB8jpbK0yvV52t+EpQ==.30ESn7bkeC7ElrnK/zj1xnnZiO+Y0LEUhSSulZI6+Qg=', 1, 0, 0, 0, 0, 'B8B3B816BF669CBEB27B3144A6FD669E7A6AE2F8232A461A9E00D9362A2B4372');
SET IDENTITY_INSERT Usuario OFF;

INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID, DVH) VALUES (1, 1, 'FD8C07E3E37AF4D8DAC7026E65AD9E80CAB7897C5A534C9B66593B188AB3D40D');
INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID, DVH) VALUES (2, 4, '44046515A543E6A16DA7D5F618C735744C8FD2E8563D4DA78D9988513077233D');
INSERT INTO UsuarioPatente (UsuarioID, PatenteID, DVH) VALUES (2, 20, 'CB32FD7978BF2496B6AA7781E5E99773B5DB6414AB649D77B3173CEDCFB7A6BE');
GO

SET IDENTITY_INSERT Categoria ON;
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (1, N'Medicamentos', NULL);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (2, N'Vacunas', 35.00);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (3, N'Antiparasitarios', NULL);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (4, N'Alimentos', 20.00);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (5, N'Accesorios', 40.00);
SET IDENTITY_INSERT Categoria OFF;

SET IDENTITY_INSERT Proveedor ON;
INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT) VALUES (1, N'Droguería Veterinaria del Sur S.A.', '30-71234567-1');
INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT) VALUES (2, N'Distribuidora Pet Health S.R.L.', '30-70987654-3');
INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT) VALUES (3, N'Laboratorios BioVet S.A.', '30-69876543-9');
SET IDENTITY_INSERT Proveedor OFF;

INSERT INTO Configuracion (Clave, Valor) VALUES ('MargenGeneral', '30.00');
GO

-- El trigger TR_Productos_BitacoraCambios registra el ALTA de cada producto en Productos_C.
SET IDENTITY_INSERT Productos ON;
INSERT INTO Productos (ProductoID, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo, PrecioCosto, PrecioVenta, FechaVencimiento, Lote, Activo, UsuarioModificacion, DVH)
     VALUES (1, N'Amoxicilina 250 mg x 10 comp.', 1, 1, 40, 10, 1800.00, 2340.00, '2027-06-30', N'AMX-2401', 1, 1, '1B3238A9221411581B1BC516C4969703D0AF2F70EA44266CD9E379C9ACDA2DF7');
INSERT INTO Productos (ProductoID, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo, PrecioCosto, PrecioVenta, FechaVencimiento, Lote, Activo, UsuarioModificacion, DVH)
     VALUES (2, N'Vacuna Séxtuple canina', 2, 3, 25, 8, 5200.00, 7020.00, '2027-03-31', N'VSX-118', 1, 1, '4456AA608B2D1490873B4BE1B8D8E2A416132E09ADE4997940695359173E78AD');
INSERT INTO Productos (ProductoID, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo, PrecioCosto, PrecioVenta, FechaVencimiento, Lote, Activo, UsuarioModificacion, DVH)
     VALUES (3, N'Pipeta antipulgas perro 10-20 kg', 3, 2, 6, 10, 3100.00, 4030.00, '2028-01-31', N'PIP-7755', 1, 1, '72361458FB79327F6C5023D4816490C32118D51FC301E8D8EA719676FB1615B5');
INSERT INTO Productos (ProductoID, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo, PrecioCosto, PrecioVenta, FechaVencimiento, Lote, Activo, UsuarioModificacion, DVH)
     VALUES (4, N'Alimento balanceado gato adulto 3 kg', 4, 2, 15, 5, 9800.00, 11760.00, '2027-11-30', N'ALG-3021', 1, 1, '893707FA49CC3415874FEB34EC4CE5049AE0F06D2570F5DD480B79937D55D788');
INSERT INTO Productos (ProductoID, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo, PrecioCosto, PrecioVenta, FechaVencimiento, Lote, Activo, UsuarioModificacion, DVH)
     VALUES (5, N'Collar isabelino talle M', 5, 2, 12, 4, 2500.00, 3500.00, '2030-12-31', N'COL-M-01', 1, 1, 'E1956C0DDA91F0DCE3EAC70C05B020822C348ECC6F2C5CB790075F9BEF1FE1B2');
SET IDENTITY_INSERT Productos OFF;

-- La pipeta arranca por debajo del stock mínimo: alerta de reposición activa.
INSERT INTO AlertaReposicion (ProductoID, Activa) VALUES (3, 1);
GO

INSERT INTO DVV (Tabla, DVV) VALUES ('Usuario', '8E863166BEFC07D4E6589F5B1E74F2814C04321A7CA341A279A4BBBC24CA0721');
INSERT INTO DVV (Tabla, DVV) VALUES ('Patente', '6F4D212609C18ADD0033089E8E43FA074341083443FF94A0AD5D1927FE9F390C');
INSERT INTO DVV (Tabla, DVV) VALUES ('Familia', '9F7ADBB1F68E1A73F1332366D970A7BF0C0F30FF7681C32CBD3C2268938B4DE8');
INSERT INTO DVV (Tabla, DVV) VALUES ('FamiliaPatente', 'D5F9F71820D9BE699E2E4658BDC90AEC15CC3273EEAC44CC2D0DD3459285497F');
INSERT INTO DVV (Tabla, DVV) VALUES ('FamiliaFamilia', 'C52D68AC7B472518DBEFAAE30BCD341DF458ED2ADC15AD1D1DB498AF57237A8C');
INSERT INTO DVV (Tabla, DVV) VALUES ('UsuarioFamilia', 'D35361E02C216FA16C37BB44D39735EC49E4D7181A7676C97DAFE8AC9907CC67');
INSERT INTO DVV (Tabla, DVV) VALUES ('UsuarioPatente', '41141D044AB1800F124475D1E09A62D35BDEE856C06808D4850855624C5F852B');
INSERT INTO DVV (Tabla, DVV) VALUES ('Bitacora', '1E1CEE03C9F3047366842C37A5B35E236FB6CE40AAEF9133ECA685921AC5145E');
INSERT INTO DVV (Tabla, DVV) VALUES ('Productos', '91460DB02B8CB33F139A92011C09C686668DBAA7460A2B92448250E540DA1E09');
INSERT INTO DVV (Tabla, DVV) VALUES ('MovimientoStock', '83BDDF37682E8B90799B49FB59A561480FB233BCEA73658518FC05E9627442F7');
GO

PRINT 'VetCareBD_60MN creada correctamente.';
GO
