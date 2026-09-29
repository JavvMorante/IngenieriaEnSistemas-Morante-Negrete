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
