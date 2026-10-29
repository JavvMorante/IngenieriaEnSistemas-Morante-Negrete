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
     operario  / Operario1234   (familia Operario de stock + patente individual Configurar margen)
     vendedor  / Vendedor1234   (familia Vendedor: CUN01 a CUN04 y CUN09)
     veterinario / Veterinario1234 (familia Veterinario: CUN03, CUN05 a CUN10)
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
    -- Último idioma elegido por el usuario (se aplica al iniciar sesión). NULL = idioma base (español).
    -- La clave foránea a Idioma se agrega después de crear esa tabla. Entra en el DVH.
    IdiomaID          INT               NULL,
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

/* El CHECK solo impide que una familia se contenga a sí misma (A → A). Este trigger
   impide también los ciclos indirectos (A → B → A, A → B → C → A), aunque la fila se
   inserte por SQL directo: desde cada relación nueva Padre → Hija recorre hacia abajo
   las familias alcanzables desde la hija; si llega de nuevo al padre, hay un ciclo. */
CREATE TRIGGER TR_FamiliaFamilia_SinCiclos
ON FamiliaFamilia
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @hayCiclo BIT = 0;

    WITH Alcanzables AS (
        SELECT i.FamiliaPadreID AS Origen, i.FamiliaHijaID AS Actual, 1 AS Nivel
        FROM inserted i
        UNION ALL
        SELECT a.Origen, ff.FamiliaHijaID, a.Nivel + 1
        FROM Alcanzables a
        JOIN FamiliaFamilia ff ON ff.FamiliaPadreID = a.Actual
        WHERE a.Nivel < 50 AND a.Actual <> a.Origen
    )
    SELECT @hayCiclo = 1 FROM Alcanzables WHERE Actual = Origen;

    IF @hayCiclo = 1
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50001, N'La relación entre familias generaría un ciclo (una familia no puede contenerse a sí misma, ni directa ni indirectamente).', 1;
    END
END
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

-- CUN11 Gestionar Proveedores. El CUIT se guarda con sus 11 dígitos (sin guiones) y no se modifica;
-- la baja es lógica (Activo = 0) y los productos conservan su proveedor.
CREATE TABLE Proveedor (
    ProveedorID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Proveedor PRIMARY KEY,
    RazonSocial  VARCHAR(120)      NOT NULL,
    CUIT         VARCHAR(13)       NOT NULL CONSTRAINT UX_Proveedor_CUIT UNIQUE,
    Telefono     VARCHAR(30)       NOT NULL,
    Email        VARCHAR(150)      NULL,
    Direccion    VARCHAR(150)      NOT NULL,
    Contacto     VARCHAR(80)       NULL,
    Activo       BIT               NOT NULL CONSTRAINT DF_Proveedor_Activo DEFAULT (1),
    DVH          CHAR(64)          NULL
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
   GESTIÓN DE PACIENTES (RFN1: CUN01 a CUN10)
   --------------------------------------------------------------------- */

-- CUN01 / CUN02 / CUN03. Baja lógica (Activo = 0): se conservan mascotas e historias clínicas.
CREATE TABLE Cliente (
    ClienteID      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Cliente PRIMARY KEY,
    TipoDocumento  VARCHAR(5)        NOT NULL,
    NroDocumento   VARCHAR(15)       NOT NULL,
    Nombre         VARCHAR(80)       NOT NULL,
    Apellido       VARCHAR(80)       NOT NULL,
    Direccion      VARCHAR(150)      NOT NULL,
    Telefono       VARCHAR(30)       NOT NULL,
    Email          VARCHAR(150)      NULL,
    Activo         BIT               NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT (1),
    DVH            CHAR(64)          NULL,
    CONSTRAINT UX_Cliente_Documento UNIQUE (TipoDocumento, NroDocumento)
);
GO

-- CUN04. Un cliente tiene una o varias mascotas (uno a varios).
CREATE TABLE Mascota (
    MascotaID        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Mascota PRIMARY KEY,
    ClienteID        INT               NOT NULL CONSTRAINT FK_Mascota_Cliente REFERENCES Cliente (ClienteID),
    Nombre           VARCHAR(60)       NOT NULL,
    Especie          VARCHAR(40)       NOT NULL,
    Raza             VARCHAR(60)       NOT NULL,
    Sexo             CHAR(1)           NOT NULL CONSTRAINT CK_Mascota_Sexo CHECK (Sexo IN ('M', 'H')),
    FechaNacimiento  DATE              NOT NULL,
    Activo           BIT               NOT NULL CONSTRAINT DF_Mascota_Activo DEFAULT (1),
    DVH              CHAR(64)          NULL
);
CREATE INDEX IX_Mascota_Cliente ON Mascota (ClienteID);
GO

-- CUN05 / CUN06 / CUN07. Una historia clínica por mascota; el número se genera a partir del Id.
CREATE TABLE HistoriaClinica (
    HistoriaClinicaID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_HistoriaClinica PRIMARY KEY,
    Numero             AS ('HC-' + RIGHT('00000' + CAST(HistoriaClinicaID AS VARCHAR(10)), 5)) PERSISTED,
    MascotaID          INT               NOT NULL CONSTRAINT FK_HistoriaClinica_Mascota REFERENCES Mascota (MascotaID)
                                                  CONSTRAINT UX_HistoriaClinica_Mascota UNIQUE,
    FechaApertura      DATETIME2(0)      NOT NULL,
    PesoActual         DECIMAL(6,2)      NOT NULL,
    Alergias           VARCHAR(400)      NOT NULL,
    Observaciones      VARCHAR(1000)     NULL,
    UsuarioID          INT               NULL,
    DVH                CHAR(64)          NULL
);
GO

-- CUN07 (paso 6): valores anteriores de la historia clínica con su fecha, para el seguimiento evolutivo.
CREATE TABLE HistoriaClinica_H (
    HistoriaClinicaHID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_HistoriaClinica_H PRIMARY KEY,
    HistoriaClinicaID   INT               NOT NULL CONSTRAINT FK_HistoriaClinica_H_HC REFERENCES HistoriaClinica (HistoriaClinicaID),
    PesoActual          DECIMAL(6,2)      NOT NULL,
    Alergias            VARCHAR(400)      NOT NULL,
    Observaciones       VARCHAR(1000)     NULL,
    FechaHora           DATETIME2(0)      NOT NULL,
    UsuarioID           INT               NULL,
    Origen              VARCHAR(20)       NOT NULL  -- MODIFICACION (CUN07) o ATENCION (CUN08 actualiza el peso)
);
GO

-- CUN08 Registrar Atención. InformesEmitidos cuenta las emisiones del informe (CUN10, reimpresión 1.1).
CREATE TABLE Atencion (
    AtencionID          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Atencion PRIMARY KEY,
    HistoriaClinicaID   INT               NOT NULL CONSTRAINT FK_Atencion_HC REFERENCES HistoriaClinica (HistoriaClinicaID),
    FechaHora           DATETIME2(0)      NOT NULL,
    UsuarioID           INT               NULL,
    Profesional         VARCHAR(120)      NOT NULL,
    Peso                DECIMAL(6,2)      NOT NULL,
    Temperatura         DECIMAL(4,1)      NOT NULL,
    FrecuenciaCardiaca  INT               NOT NULL,
    Motivo              VARCHAR(300)      NOT NULL,
    Diagnostico         VARCHAR(1000)     NOT NULL,
    InformesEmitidos    INT               NOT NULL CONSTRAINT DF_Atencion_Informes DEFAULT (0),
    -- Indicaciones o tratamiento (por ejemplo, qué comprar y cómo administrarlo). Opcional: la atención
    -- puede cerrarse sin medicación; lo entregado en el carrito (CUN05) se registra aparte como venta.
    Indicaciones        VARCHAR(1000)     NULL,
    DVH                 CHAR(64)          NULL
);
CREATE INDEX IX_Atencion_HC ON Atencion (HistoriaClinicaID, FechaHora);
GO

-- CUN05 Registrar Medicación en Carrito: el carrito vive en memoria y al confirmarlo se graba una
-- venta (sin facturación). La atención es opcional: también se vende a un cliente sin atención.
CREATE TABLE VentaEncabezado (
    VentaID     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_VentaEncabezado PRIMARY KEY,
    ClienteID   INT               NOT NULL CONSTRAINT FK_VentaEncabezado_Cliente REFERENCES Cliente (ClienteID),
    AtencionID  INT               NULL     CONSTRAINT FK_VentaEncabezado_Atencion REFERENCES Atencion (AtencionID),
    FechaHora   DATETIME2(0)      NOT NULL,
    UsuarioID   INT               NULL,
    Total       DECIMAL(12,2)     NOT NULL,
    DVH         CHAR(64)          NULL
);
CREATE INDEX IX_VentaEncabezado_Fecha ON VentaEncabezado (FechaHora);
CREATE INDEX IX_VentaEncabezado_Atencion ON VentaEncabezado (AtencionID);
GO

-- Cada ítem vendido genera un egreso de stock (CUN09) en la misma transacción.
CREATE TABLE VentaDetalle (
    VentaDetalleID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_VentaDetalle PRIMARY KEY,
    VentaID         INT               NOT NULL CONSTRAINT FK_VentaDetalle_Venta REFERENCES VentaEncabezado (VentaID),
    ProductoID      INT               NOT NULL CONSTRAINT FK_VentaDetalle_Producto REFERENCES Productos (ProductoID),
    Cantidad        INT               NOT NULL CONSTRAINT CK_VentaDetalle_Cantidad CHECK (Cantidad > 0),
    PrecioUnitario  DECIMAL(12,2)     NOT NULL,
    Subtotal        DECIMAL(12,2)     NOT NULL,
    MovimientoID    INT               NOT NULL CONSTRAINT FK_VentaDetalle_Movimiento REFERENCES MovimientoStock (MovimientoID),
    DVH             CHAR(64)          NULL
);
CREATE INDEX IX_VentaDetalle_Venta ON VentaDetalle (VentaID);
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

-- Activo: registra las versiones de cada producto (postcondición de CUN11, CUN12 y CUN14)
-- aunque la pantalla de la Bitácora de cambios (CUS10) siga deshabilitada.

/* ---------------------------------------------------------------------
   MULTI-IDIOMA (patrón Observer en la GUI)
   - El español es el idioma BASE: sus textos son los del diseñador de
     Visual Studio, por eso no necesita filas en Traduccion.
   - Clave de traducción: "Formulario.Control" (específica) o "Control"
     (genérica, compartida por todos los formularios). Para textos que
     arma el código se usan claves "msg.*" / "estado.*".
   - El idioma es una preferencia POR USUARIO (Usuario.IdiomaID): el Login abre
     siempre en el idioma base y al ingresar se aplica el último idioma del usuario.
   --------------------------------------------------------------------- */
CREATE TABLE Idioma (
    IdiomaID        INT          NOT NULL CONSTRAINT PK_Idioma PRIMARY KEY,
    Nombre          NVARCHAR(50) NOT NULL,
    Codigo          VARCHAR(5)   NOT NULL CONSTRAINT UX_Idioma_Codigo UNIQUE,
    EsBase          BIT          NOT NULL
);
GO

ALTER TABLE Usuario ADD CONSTRAINT FK_Usuario_Idioma FOREIGN KEY (IdiomaID) REFERENCES Idioma (IdiomaID);
GO

CREATE TABLE Traduccion (
    IdiomaID  INT           NOT NULL CONSTRAINT FK_Traduccion_Idioma REFERENCES Idioma (IdiomaID),
    Clave     VARCHAR(120)  NOT NULL,
    Texto     NVARCHAR(400) NOT NULL,
    CONSTRAINT PK_Traduccion PRIMARY KEY (IdiomaID, Clave)
);
GO

INSERT INTO Idioma (IdiomaID, Nombre, Codigo, EsBase) VALUES
    (1, N'Español', 'es', 1),
    (2, N'English', 'en', 0);
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
(2, 'msg.IdiomaGuardado', N'Language changed. It was saved as your language and will be applied every time you sign in.'),
(2, 'estado.Usuario', N'User'), (2, 'estado.Familias', N'Families'), (2, 'estado.PatentesIndividuales', N'individual patent(s)'), (2, 'estado.Idioma', N'Language'),
(2, 'estado.Integridad', N'Integrity inconsistencies detected'), (2, 'estado.Emergencia', N'EMERGENCY USER'),
-- Menú principal
(2, 'FrmMenuPrincipal_60MN', N'VetCare - Veterinary management system'),
(2, 'mnuAdmin', N'ADMIN'), (2, 'mnuUsuarios', N'Users'), (2, 'mnuPerfiles', N'Profiles'), (2, 'mnuBackup', N'Backup'),
(2, 'mnuRestore', N'Restore'), (2, 'mnuBitacoraEventos', N'Event log'), (2, 'mnuDigitosVerificadores', N'Check digits'),
(2, 'mnuMaestros', N'MASTER DATA'), (2, 'mnuProductos', N'Products'), (2, 'mnuMovimientosStock', N'Stock movements'),
(2, 'mnuClientes', N'Customers'), (2, 'mnuMascotas', N'Pets'), (2, 'mnuProveedores', N'Suppliers'), (2, 'mnuBitacoraCambios', N'Change log'),
(2, 'mnuPacientes', N'PATIENTS'), (2, 'mnuHistoriasClinicas', N'Medical records'), (2, 'mnuRegistrarAtencion', N'Register consultation'),
(2, 'mnuStock', N'STOCK'), (2, 'mnuVentasRegistradas', N'Recorded sales'), (2, 'mnuReporteAtenciones', N'Consultations report'),
(2, 'mnuUsuario', N'USER'), (2, 'mnuReLogin', N'Re-Login'), (2, 'mnuCambiarClave', N'Change password'),
(2, 'mnuLogout', N'Logout'), (2, 'mnuCambiarIdioma', N'Change language'),
(2, 'mnuVentas', N'SALES'), (2, 'mnuCarrito', N'Cart'),
(2, 'mnuReportes', N'REPORTS'), (2, 'mnuReporte1', N'Stock report'), (2, 'mnuReporte2', N'Smart report'),
(2, 'mnuAyuda', N'HELP'), (2, 'mnuAyudaEnLinea', N'Online help (F1)'), (2, 'mnuAcercaDe', N'About VetCare'),
-- Selector de paciente (PACIENTES > Historias clínicas / Registrar atención)
(2, 'FrmBuscarPaciente_60MN', N'Select patient'), (2, 'FrmBuscarPaciente_60MN.btnAceptar', N'Select'),
(2, 'colPaciente', N'Patient'), (2, 'colPropietario', N'Owner'), (2, 'msg.CriterioEspecie', N'Species'),
(2, 'msg.ElegirPaciente', N'Search for and choose the patient.'),
(2, 'msg.ElegirPacienteHistoria', N'Choose the patient to see their medical record.'),
(2, 'msg.ElegirPacienteAtencion', N'Choose the patient to attend.'),
(2, 'msg.SinPacientes', N'No patients were found with that criterion.'),
(2, 'msg.SinHistoriaAbrir', N'The pet has no medical record. Do you want to open it now?'),
(2, 'msg.SinHistoriaAtencion', N'The pet has no medical record. It must be created before registering the consultation. Open it now?'),
-- CUN02 Gestionar Mascotas (MAESTROS > Mascotas) y selector de cliente
(2, 'FrmGestionMascotas_60MN', N'Pet management'), (2, 'FrmGestionMascotas_60MN.lblTitulo', N'Pets'),
(2, 'FrmGestionMascotas_60MN.colMascota', N'Pet'), (2, 'FrmGestionMascotas_60MN.btnBaja', N'Deactivate'),
(2, 'msg.MascotasEncontradas', N'{0} pet(s) found.'),
(2, 'msg.SinMascotasBusqueda', N'No pets were found. To register one, use "Add".'),
(2, 'FrmBuscarCliente_60MN', N'Select customer'), (2, 'FrmBuscarCliente_60MN.lblTitulo', N'Choose the customer who owns the pet'),
(2, 'FrmBuscarCliente_60MN.btnAceptar', N'Select'),
(2, 'msg.SinClientesActivos', N'No active customers were found. Register the customer from MASTER DATA > Customers.'),
-- CUN11 Gestionar Proveedores (MAESTROS > Proveedores)
(2, 'FrmProveedores_60MN', N'Suppliers'), (2, 'FrmProveedores_60MN.lblTitulo', N'Suppliers'),
(2, 'FrmProveedores_60MN.btnNuevo', N'New supplier'), (2, 'colRazonSocial', N'Company name'), (2, 'colCuit', N'Tax ID (CUIT)'),
(2, 'colContacto', N'Contact'), (2, 'colProductosActivos', N'Active products'), (2, 'colEstadoProducto', N'Status'),
(2, 'msg.CriterioRazonSocial', N'Company name'), (2, 'msg.ProductosQueProvee', N'Products supplied'),
(2, 'FrmProveedor_60MN', N'Supplier'), (2, 'FrmProveedor_60MN.TituloAlta', N'Register supplier'),
(2, 'FrmProveedor_60MN.TituloModificacion', N'Modify supplier'), (2, 'lblRazonSocial', N'Company name *'),
(2, 'lblCuit', N'Tax ID (CUIT) *'), (2, 'lblContacto', N'Contact'),
(2, 'FrmProveedor_60MN.NotaAlta', N'The CUIT is entered with or without dashes and cannot be changed afterwards.'),
(2, 'FrmProveedor_60MN.NotaModificacion', N'The CUIT cannot be changed.'),
-- CUN05 Carrito y venta (sin facturación) y VENTAS > Ventas registradas
(2, 'FrmCarrito_60MN.btnConfirmar', N'Confirm sale'), (2, 'lblCliente', N'Customer'), (2, 'btnElegirCliente', N'Choose customer...'),
(2, 'msg.ElegirClienteVenta', N'Choose the customer for the sale'), (2, 'msg.SinClienteVenta', N'(choose the customer for the sale)'),
(2, 'msg.VentaSinAtencion', N'(sale without consultation)'),
(2, 'FrmVentas_60MN', N'Recorded sales'), (2, 'FrmVentas_60MN.lblTitulo', N'Recorded sales'),
(2, 'FrmVentas_60MN.btnElegirCliente', N'Choose...'), (2, 'btnQuitarCliente', N'All'),
(2, 'colNumero', N'Sale'), (2, 'colCliente', N'Customer'), (2, 'colAtencion', N'Consultation'), (2, 'colUnidades', N'Units'),
(2, 'colTotal', N'Total $'), (2, 'colProducto', N'Product'), (2, 'lblDetalle', N'Sale detail'),
(2, 'msg.ElegirClienteConsulta', N'Choose the customer to look up'), (2, 'msg.TodosLosClientes', N'(all customers)'),
(2, 'msg.ResumenVentas', N'{0} sale(s) · {1} unit(s) · Total $ {2}'), (2, 'msg.DetalleVenta', N'Sale detail'),
-- CUN07 Generar Reporte de Atenciones (REPORTES > Reporte de atenciones)
(2, 'FrmReporteAtenciones_60MN', N'Consultations report'), (2, 'FrmReporteAtenciones_60MN.grpFiltros', N'Filters'),
(2, 'FrmReporteAtenciones_60MN.lblProfesional', N'Veterinarian'), (2, 'FrmReporteAtenciones_60MN.lblResumen', N'Choose the period and press Generate.'),
(2, 'lblPorProfesional', N'Consultations by professional'), (2, 'colProfesionalTotal', N'Professional'),
(2, 'colCantidadAtenciones', N'Consultations'), (2, 'colPacientes', N'Patients'), (2, 'colPorcentaje', N'% of total'),
(2, 'msg.SinAtencionesPeriodo', N'No consultations were registered in the period with the given filters.'),
(2, 'msg.ResumenAtenciones', N'{0} consultation(s) · {1} patient(s) · {2} professional(s)'),
-- Ayuda en línea (F1): temas en inglés; el español está en TemasAyuda_60MN
(2, 'FrmAyuda_60MN', N'Online help - VetCare'),
(2, 'lblIndice', N'Topic index'),
(2, 'ayuda.general.titulo', N'VetCare: how to use the system'),
(2, 'ayuda.general.texto', N'The main menu shows only the options your profile allows. MASTER DATA holds customers, pets, products and suppliers; PATIENTS, medical records and consultations; SALES, the cart; STOCK, the movements; REPORTS, the reports. Press F1 on any screen to see its help, or choose another topic in this index.'),
(2, 'ayuda.login.titulo', N'Signing in and changing the password'),
(2, 'ayuda.login.texto', N'Enter your user name and password. Three failed attempts lock the user: ask an administrator to unlock it. A new password must have at least 8 characters, with upper case, lower case and numbers.'),
(2, 'ayuda.usuarios.titulo', N'Users'),
(2, 'ayuda.usuarios.texto', N'ADMIN > Users: search, add, modify, deactivate, reactivate or unlock users. When adding one you choose its initial profile and the system generates a password that must be changed on first sign in. Permissions assigns families and individual patents.'),
(2, 'ayuda.perfiles.titulo', N'Profiles (families and patents)'),
(2, 'ayuda.perfiles.texto', N'ADMIN > Profiles: each family is a set of patents and may include other families. The system does not allow cycles between families nor a change that leaves it without an operational administrator.'),
(2, 'ayuda.bitacoraEventos.titulo', N'Event log'),
(2, 'ayuda.bitacoraEventos.texto', N'ADMIN > Event log: filter by dates, criticality, module, event and user. Export XML saves the signed events; Import XML validates them (signatures, catalog, duplicates) and adds all of them or none.'),
(2, 'ayuda.bitacoraCambios.titulo', N'Change log'),
(2, 'ayuda.bitacoraCambios.texto', N'ADMIN > Change log: version history of each product. You can compare two versions and restore a previous one.'),
(2, 'ayuda.integridad.titulo', N'Check digits'),
(2, 'ayuda.integridad.texto', N'ADMIN > Check digits: verifies the DVH (one per row) and DVV (one per table) of the protected tables and shows the inconsistencies. Recalculate accepts the current state of the database: use it only after a migration or after reviewing the changes.'),
(2, 'ayuda.backup.titulo', N'Backup and restore'),
(2, 'ayuda.backup.texto', N'ADMIN > Backup creates a copy of the database in the chosen folder. ADMIN > Restore replaces it with an earlier backup: later changes are lost and the session is closed. A database with check digit inconsistencies is not backed up.'),
(2, 'ayuda.idioma.titulo', N'Change language'),
(2, 'ayuda.idioma.texto', N'USER > Change language translates every open screen. The chosen language is saved as yours and applied every time you sign in.'),
(2, 'ayuda.clientes.titulo', N'CUN01 Manage Customers'),
(2, 'ayuda.clientes.texto', N'MASTER DATA > Customers: search by document, last name or pet name. From the record you can register, modify, deactivate or reactivate customers and manage their pets. The document is validated by type (DNI, CUIL with check digit, passport) and cannot be changed after registration.'),
(2, 'ayuda.mascotas.titulo', N'CUN02 Manage Pets'),
(2, 'ayuda.mascotas.texto', N'MASTER DATA > Pets: search by name, species or the owner''s document or last name. To add a pet, first choose its customer. Deactivation is logical: the medical record is kept.'),
(2, 'ayuda.historia.titulo', N'CUN03 Manage Medical Records'),
(2, 'ayuda.historia.texto', N'PATIENTS > Medical records: choose the patient. If it has no medical record you can open one. From the record you see the consultations, update the data (keeping the previous values) and print it.'),
(2, 'ayuda.atencion.titulo', N'CUN04 Register Consultation'),
(2, 'ayuda.atencion.texto', N'PATIENTS > Register consultation: choose the patient (it must have a medical record). Enter vital signs, reason and diagnosis; instructions are optional. Once registered, you can add medication (cart) and issue the report.'),
(2, 'ayuda.carrito.titulo', N'CUN05 Register Medication in Cart'),
(2, 'ayuda.carrito.texto', N'From the consultation (Add medication) or from SALES > Cart choosing the customer, with or without a consultation. Add products in stock; expired ones are not delivered. Confirming records the sale and the stock issue. SALES > Recorded sales lists the sales. The system does not invoice.'),
(2, 'ayuda.informe.titulo', N'CUN06 Issue Consultation Report'),
(2, 'ayuda.informe.texto', N'The report gathers patient, owner, professional, vital signs, reason, diagnosis, instructions and medication. Print it or choose "Microsoft Print to PDF" to save it; every issue is recorded.'),
(2, 'ayuda.reporteAtenciones.titulo', N'CUN07 Generate Consultations Report'),
(2, 'ayuda.reporteAtenciones.texto', N'REPORTS > Consultations report: choose the period and, optionally, the veterinarian and the species. It shows the consultations and the totals by professional; it can be printed (or PDF) and exported to CSV.'),
(2, 'ayuda.productos.titulo', N'CUN08 Manage Stock Items'),
(2, 'ayuda.productos.texto', N'MASTER DATA > Products: search, register and modify products. The sale price is proposed with the category margin. Stock is not edited directly: it is adjusted with a stock movement.'),
(2, 'ayuda.movimientos.titulo', N'CUN09 Register Stock Movement'),
(2, 'ayuda.movimientos.texto', N'STOCK > Stock movements: register receipts, issues, adjustments or the write-off of an expired batch, always with a reason. If stock reaches the minimum or falls below it, a replenishment alert is raised.'),
(2, 'ayuda.configuracion.titulo', N'CUN10 Configure Margin and Minimum Stock Alert'),
(2, 'ayuda.configuracion.texto', N'ADMIN > Stock settings: general profit margin, specific margin for each category and suggested minimum stock for new products.'),
(2, 'ayuda.proveedores.titulo', N'CUN11 Manage Suppliers'),
(2, 'ayuda.proveedores.texto', N'MASTER DATA > Suppliers: search by company name or CUIT and see the products each one supplies. The CUIT is validated with its check digit and cannot be changed. A deactivated supplier is not offered when registering products.'),
(2, 'ayuda.reporteExistencias.titulo', N'CUN12 Generate Stock Report'),
(2, 'ayuda.reporteExistencias.texto', N'REPORTS > Stock report: current stock, products below the minimum or about to expire, filtered by category and supplier. It can be printed (or PDF) and exported to CSV.'),
(2, 'ayuda.reporteInteligente.titulo', N'CUN13 Generate Smart Report'),
(2, 'ayuda.reporteInteligente.texto', N'REPORTS > Smart report: crosses the demand for supplies by month and category (stock) with the products most delivered in consultations (patients) to anticipate purchases.'),
-- Login
(2, 'FrmLogin_60MN', N'VetCare - Sign in'), (2, 'FrmLogin_60MN.lblIdioma', N'Change language'),
(2, 'FrmLogin_60MN.lblSubtitulo', N'Sign in'), (2, 'FrmLogin_60MN.lblUsuario', N'Login'), (2, 'FrmLogin_60MN.lblClave', N'Password'),
(2, 'FrmLogin_60MN.btnAceptar', N'Enter'), (2, 'FrmLogin_60MN.btnCancelar', N'Exit'),
-- Cambiar idioma
(2, 'FrmCambiarIdioma_60MN', N'Change language'), (2, 'FrmCambiarIdioma_60MN.lblIdioma', N'Language'),
(2, 'FrmCambiarIdioma_60MN.lblNota', N'The selected language is saved as yours: it will be applied every time you sign in.'),
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
(2, 'lblDescripcion', N'Checks the horizontal (DVH, one per row) and vertical (DVV, one per table) check digits of Usuario, Familia, Patente, their relation tables, Bitacora, Productos, MovimientoStock, Cliente, Mascota, HistoriaClinica, Atencion, Proveedor, VentaEncabezado and VentaDetalle.'),
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
(2, 'colCantidad', N'Quantity'), (2, 'colAnterior', N'Previous stock'), (2, 'colResultante', N'Resulting stock'), (2, 'colMotivo', N'Reason'),
-- Menú: Gestión de Pacientes y Stock
(2, 'mnuConfiguracionStock', N'Stock settings'), 
-- Clientes y mascotas (CUN01 a CUN04)
(2, 'FrmClientes_60MN', N'Customers and pets'), (2, 'FrmClientes_60MN.lblTitulo', N'Customers and pets'),
(2, 'lblCriterio', N'Search by'), (2, 'chkIncluirBajas', N'Include deactivated'), (2, 'colDocumento', N'Document'),
(2, 'colTelefono', N'Phone'), (2, 'grpFicha', N'Customer record'), (2, 'btnNuevoCliente', N'New customer'),
(2, 'btnModificarCliente', N'Modify'), (2, 'btnBajaCliente', N'Deactivate'), (2, 'btnReactivarCliente', N'Reactivate'),
(2, 'lblMascotas', N'Customer pets'), (2, 'colMascota', N'Name'), (2, 'colEspecie', N'Species'), (2, 'colRaza', N'Breed'),
(2, 'colSexo', N'Sex'), (2, 'colNacimiento', N'Birth date'), (2, 'colHistoria', N'Medical record'), (2, 'colEstadoMascota', N'Status'),
(2, 'btnAnadirMascota', N'Add pet'), (2, 'btnModificarMascota', N'Modify'), (2, 'btnBajaMascota', N'Deactivate'),
(2, 'btnHistoriaClinica', N'Medical record'), (2, 'btnNuevaAtencion', N'New consultation'),
(2, 'msg.CriterioDocumento', N'Document'), (2, 'msg.CriterioApellido', N'Last name'), (2, 'msg.CriterioMascota', N'Pet name'),
(2, 'msg.SeleccioneCliente', N'Search and select a customer to see their record.'), (2, 'msg.Direccion', N'Address'),
(2, 'msg.Telefono', N'Phone'), (2, 'msg.ClienteDeBaja', N'DEACTIVATED CUSTOMER: it can be reactivated.'),
(2, 'FrmCliente_60MN', N'Customer'), (2, 'FrmCliente_60MN.TituloAlta', N'Register customer'), (2, 'FrmCliente_60MN.TituloModificacion', N'Modify customer'),
(2, 'lblTipoDocumento', N'Document type *'), (2, 'lblNroDocumento', N'Document number *'), (2, 'FrmCliente_60MN.lblNombre', N'Name *'),
(2, 'lblApellido', N'Last name *'), (2, 'lblDireccion', N'Address *'), (2, 'lblTelefono', N'Phone *'), (2, 'lblEmail', N'Email'),
(2, 'grpMascotas', N'Pets to register'), (2, 'btnAgregarMascota', N'Add pet'), (2, 'btnQuitarMascota', N'Remove'),
(2, 'FrmCliente_60MN.lblNota', N'If the customer does not bring a pet, they can be registered now and the pet added later from their record.'),
(2, 'FrmMascotas_60MN', N'Pet'), (2, 'FrmMascotas_60MN.TituloAlta', N'Add pet'), (2, 'FrmMascotas_60MN.TituloModificacion', N'Modify pet'),
(2, 'FrmMascotas_60MN.lblNombre', N'Name *'), (2, 'FrmMascotas_60MN.lblEspecie', N'Species *'), (2, 'FrmMascotas_60MN.lblRaza', N'Breed *'),
(2, 'lblSexo', N'Sex *'), (2, 'rdoMacho', N'Male'), (2, 'rdoHembra', N'Female'), (2, 'FrmMascotas_60MN.lblNacimiento', N'Birth date *'),
-- Historia clínica (CUN05 a CUN07)
(2, 'FrmHistoriaClinica_60MN', N'Medical record'), (2, 'FrmHistoriaClinica_60MN.TituloAlta', N'Open medical record'),
(2, 'FrmHistoriaClinica_60MN.btnAbrir', N'Open medical record'), (2, 'FrmHistoriaClinica_60MN.btnGuardar', N'Save'),
(2, 'grpPaciente', N'Patient'), (2, 'lblEspecie', N'Species'), (2, 'lblRaza', N'Breed'), (2, 'lblNacimiento', N'Birth date'),
(2, 'grpDatos', N'Clinical data'), (2, 'FrmHistoriaClinica_60MN.lblPeso', N'Current weight (kg) *'), (2, 'chkSinAlergias', N'No known allergies'),
(2, 'lblAlergias', N'Allergies *'), (2, 'lblObservaciones', N'Remarks'), (2, 'btnCancelarEdicion', N'Cancel'),
(2, 'FrmHistoriaClinica_60MN.btnImprimir', N'Print record'), (2, 'tpAtenciones', N'Consultations'),
(2, 'colProfesional', N'Professional'), (2, 'colPeso', N'Weight (kg)'), (2, 'btnVerInforme', N'View / print report'),
(2, 'btnMedicacion', N'Add medication'), (2, 'tpEvolucion', N'History (previous values)'), (2, 'colFechaEvolucion', N'Date and time'),
(2, 'colOrigen', N'Origin'), (2, 'colPesoEvolucion', N'Weight (kg)'), (2, 'colAlergiasEvolucion', N'Allergies'),
(2, 'colObservacionesEvolucion', N'Remarks'), (2, 'colUsuarioEvolucion', N'User'),
(2, 'msg.HistoriaNueva', N'New medical record'), (2, 'msg.Abierta', N'opened on'), (2, 'msg.Alergias', N'ALLERGIES'),
(2, 'msg.Propietario', N'Owner'), (2, 'msg.SinAtenciones', N'No consultations registered.'),
-- Atención, carrito e informe (CUN08 a CUN10)
(2, 'FrmAtencion_60MN', N'Register consultation'), (2, 'lblProfesional', N'Professional'), (2, 'grpSignos', N'Vital signs'),
(2, 'lblPeso', N'Weight (kg)'), (2, 'lblTemperatura', N'Temperature (°C)'), (2, 'lblFrecuencia', N'Heart rate (bpm)'),
(2, 'FrmAtencion_60MN.lblMotivo', N'Reason for consultation *'), (2, 'lblDiagnostico', N'Diagnosis *'),
(2, 'btnRegistrar', N'Register consultation'), (2, 'btnInforme', N'Issue report'),
(2, 'FrmCarrito_60MN', N'Medication cart'), (2, 'lblAtencion', N'Consultation'), (2, 'FrmCarrito_60MN.lblBuscar', N'Product'),
(2, 'colPrecio', N'Price'), (2, 'colVence', N'Expiration'), (2, 'FrmCarrito_60MN.lblCantidad', N'Quantity'),
(2, 'btnAgregar', N'Add to cart'), (2, 'lblCarrito', N'Cart'), (2, 'colProductoCarrito', N'Product'), (2, 'colLoteCarrito', N'Batch'),
(2, 'colCantidadCarrito', N'Quantity'), (2, 'colPrecioCarrito', N'Unit price'), (2, 'colSubtotal', N'Subtotal'),
(2, 'btnQuitar', N'Remove item'), (2, 'btnConfirmar', N'Confirm delivery'),
(2, 'FrmInformeAtencion_60MN', N'Consultation report'), (2, 'btnImprimir', N'Print / PDF'),
(2, 'FrmInformeAtencion_60MN.btnGuardar', N'Save as text'),
-- Configuración de stock (CUN13)
(2, 'FrmConfiguracionStock_60MN', N'Stock settings'), (2, 'grpGeneral', N'General parameters'), (2, 'lblMargen', N'Profit margin (%)'),
(2, 'FrmConfiguracionStock_60MN.lblStockMinimo', N'Suggested minimum stock (units)'), (2, 'lblCategorias', N'Margin by category (%)'),
(2, 'colMargenCategoria', N'Margin (%)'), (2, 'FrmConfiguracionStock_60MN.lblAyuda', N'Empty = the category uses the general margin.'),
(2, 'chkRecalcular', N'Recalculate the sale price of active products'),
(2, 'chkAplicarMinimo', N'Apply the suggested minimum stock to active products'), (2, 'FrmConfiguracionStock_60MN.btnGuardar', N'Save'),
-- Reporte de existencias (CUN15)
(2, 'FrmReporteExistencias_60MN', N'Stock report'), (2, 'grpFiltros', N'Criteria'), (2, 'lblTipo', N'Type'),
(2, 'FrmReporteExistencias_60MN.lblProveedor', N'Supplier'), (2, 'FrmReporteExistencias_60MN.chkVencimiento', N'Expiration from'),
(2, 'btnGenerar', N'Generate'), (2, 'colMinimo', N'Minimum'), (2, 'colValorizacion', N'Valuation'), (2, 'btnExportar', N'Export CSV'),
(2, 'msg.ExistenciasGenerales', N'General stock'), (2, 'msg.BajoStockMinimo', N'Below minimum stock'),
(2, 'msg.ProximosAVencer', N'About to expire'), (2, 'msg.SinResultados', N'No products match the criteria.'),
(2, 'msg.Total', N'Total'),
-- Backup y restore (CUS12)
(2, 'FrmBackupRestore_60MN', N'Backup / Restore'), (2, 'FrmBackupRestore_60MN.lblTitulo', N'Database backup and restore'),
(2, 'grpRespaldo', N'Back up'), (2, 'lblCarpeta', N'Destination folder *'), (2, 'btnExaminarCarpeta', N'Browse...'),
(2, 'lblNotaCarpeta', N'You can choose any folder where you have write permission (a USB drive too). SQL Server creates the backup in its own folder and VetCare copies it to the chosen one, verifying that the copy is identical.'),
(2, 'btnRespaldar', N'Back up database'), (2, 'grpRestaurar', N'Restore'), (2, 'lblRespaldos', N'Available backups (or choose a .bak file from any folder with Browse...)'),
(2, 'colArchivo', N'File'), (2, 'colTamanio', N'Size'), (2, 'colRuta', N'Path'), (2, 'lblArchivo', N'Backup file *'),
(2, 'btnExaminarArchivo', N'Browse...'),
(2, 'lblAdvertencia', N'Restoring replaces the whole database: changes made after the backup (including the event log) are lost and the session is closed at the end. Only VetCareBD_60MN backups are accepted.'),
(2, 'FrmBackupRestore_60MN.btnRestaurar', N'Restore database'), (2, 'msg.ArchivoAGenerar', N'File to generate'),
(2, 'msg.ElegirCarpetaBackup', N'Backup destination folder'),
-- Bitácora de eventos: exportar / importar XML
(2, 'btnExportarXml', N'Export XML...'), (2, 'btnImportarXml', N'Import XML...'), (2, 'btnVerXml', N'View XML...'),
(2, 'FrmVisorXmlBitacora_60MN', N'Event log in XML'), (2, 'tpXml', N'XML'), (2, 'tpEventos', N'File events'),
(2, 'FrmVisorXmlBitacora_60MN.btnImportar', N'Import events'),
(2, 'msg.XmlValido', N'Valid file: HMAC-SHA256 signatures are correct and the content was verified.'),
(2, 'msg.XmlRechazado', N'Rejected file: it cannot be imported.'), (2, 'msg.XmlExportadoPor', N'Exported by'),
(2, 'msg.XmlVersion', N'Version'), (2, 'msg.XmlCifrado', N'Encrypted descriptions'), (2, 'msg.Si', N'Yes'), (2, 'msg.No', N'No'),
(2, 'msg.XmlEventos', N'Events'), (2, 'msg.XmlRango', N'Period'), (2, 'msg.XmlNuevos', N'New'), (2, 'msg.XmlDuplicados', N'Already present'),
-- Textos que arma el código (bitácora, historia clínica, login) y Reporte inteligente
(2, 'msg.Criticidad1', N'1 - Very high'),
(2, 'msg.Criticidad2', N'2 - High'),
(2, 'msg.Criticidad3', N'3 - Medium'),
(2, 'msg.Criticidad4', N'4 - Low'),
(2, 'msg.Criticidad5', N'5 - Informational'),
(2, 'msg.EventosEncontrados', N'{0} event(s)'),
(2, 'msg.UsuarioDelEvento', N'Event user'),
(2, 'msg.SinUsuarioAsociado', N'(no associated user record)'),
(2, 'msg.Peso', N'Weight'),
(2, 'msg.Temperatura', N'Temp.'),
(2, 'msg.FrecuenciaCardiaca', N'HR'),
(2, 'msg.Motivo', N'Reason'),
(2, 'msg.Diagnostico', N'Diagnosis'),
(2, 'msg.Medicacion', N'Medication'),
(2, 'msg.SinMedicacion', N'(no medication)'),
(2, 'lblIndicaciones', N'Instructions / treatment (optional: what to buy, dose, frequency)'), (2, 'msg.Indicaciones', N'Instructions'), (2, 'msg.SinIndicaciones', N'(no instructions)'),
(2, 'msg.Lote', N'batch'),
(2, 'msg.SesionYaIniciada', N'A session is already open.'),
(2, 'msg.DebeCambiarClave', N'You must change your password to sign in.'),
(2, 'FrmReporteInteligente_60MN', N'Smart report'),
(2, 'FrmReporteInteligente_60MN.grpFiltros', N'Period'),
(2, 'lblTop', N'Products in the ranking'),
(2, 'msg.DemandaPorCategoria', N'Supply demand by month and category (units issued)'),
(2, 'msg.TopProductosEntregados', N'Products most delivered in consultations'),
(2, 'msg.SinMovimientos', N'No movements were found in the selected period.'),
(2, 'msg.GenerarParaVer', N'Choose a period and press Generate.'),
(2, 'msg.ResumenReporteInteligente', N'{0} unit(s) issued · {1} unit(s) delivered in consultations · $ {2}');
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
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (9, N'Respaldar base de datos (Backup)', N'SEG_BACKUP', '108C394E59DAE80DD69BDF70FCEC56507977326C6646EEEFCD2BD778D3AABA94');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (10, N'Restaurar base de datos (Restore)', N'SEG_RESTORE', 'A036A747AC3F329BEE1F212D09A72894F040EBCAFD3B9770B368398D12DFC54D');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (11, N'Registrar cliente', N'PAC_CLIENTE_ALTA', '967E3B9F353B82A1F4FE397CED29D999E986914A710828DB199588EC2537FB92');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (12, N'Modificar cliente', N'PAC_CLIENTE_MODIF', '28165CC7CE60294E164E3E9A0A6FD65A288FA0B27A734BAD250DA9AC5E1CEE40');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (13, N'Consultar cliente', N'PAC_CLIENTE_CONSULTA', '6986529ABC3568768099FE52374869639FACFC84E3DDF2E76E919A12F33C0F8F');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (14, N'Gestionar mascota', N'PAC_MASCOTA', '8E5E9070B584BB9043B06F9FDD710B79A971451D5A2EA9C32C4A7261CB156952');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (15, N'Crear historia clínica', N'HC_ALTA', '0767CA235CB0800E83687C84EBA0DE80B8046E40CC2DB1291BBD74645CEC889E');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (16, N'Consultar historia clínica', N'HC_CONSULTA', '084DCB2377F0D657733A405480ED008F9B55232F76FA47535AFC294548B4FA27');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (17, N'Actualizar historia clínica', N'HC_MODIF', '62CFCA2352A9DAFFB50BFAC88FE972D636AD9A48E699E6B6443D6031260F24CD');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (18, N'Registrar atención', N'ATN_REGISTRAR', '896A70B56E01F04089D749713E0EE210C9DABA46CBEDD41B5315EDEDCAC96417');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (19, N'Registrar medicación en carrito', N'VEN_CARRITO', '56F4D6E55B65FEDFAD3321693EA658CF7BDD2FE96EE862F3F3964B8CFE4C25A7');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (20, N'Emitir informe de atención', N'ATN_INFORME', '1C5D04188A41A72C522619942A513367D3C336ED360D5AD580601F38450F4DCD');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (21, N'Registrar producto', N'STK_PROD_ALTA', '385D7DF7E8506884F563853DF9AD7CEDF9AB60B41D55C84949A5540ADA378037');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (22, N'Modificar producto', N'STK_PROD_MODIF', 'D47501FFE4AE5F1136DF10435049D515DE696E617A02537E6D833FA8C350620D');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (23, N'Configurar margen y alerta de stock mínimo', N'STK_CONFIG', 'CB3B6493ED2D414EB871D1C4DE4BE8A73DD33B47A1771A9179EE1963B65CD9DA');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (24, N'Registrar movimiento de stock', N'STK_MOVIMIENTO', '59E5B6C1F5580E92639017AB081508FD9ACE6EE402C957BF4EA53A97E5C5327E');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (25, N'Generar reporte de existencias', N'STK_REPORTE', 'D531941AEB488BAD5AB0E08EE97825CCBC3759695A81668D3FBDC4198B3A4B7D');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (26, N'Gestionar proveedores', N'MAE_PROVEEDORES', '428291CB9C34F15895B3002A104EEB0A3647A2AFB61281D3FB2D8D19A2E98789');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (30, N'Reportes de atenciones e inteligente', N'REP_REPORTES', '0513DDA1C55050DE3FE43F14698BF127B3A85F363E63E0D36B14DD01FE6FC463');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (31, N'Exportar bitácora de eventos (XML)', N'SEG_BITACORA_EXPORT', '96BF17B39F493E5F059B1939E391E6AC7C20444970C3EA39289E696180ACF6A2');
INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (32, N'Importar bitácora de eventos (XML)', N'SEG_BITACORA_IMPORT', '2BA370E416600577DC7A90005A5A06D5ED06106AFEFEC9FA7DEFC5703A9333F7');
SET IDENTITY_INSERT Patente OFF;

SET IDENTITY_INSERT Familia ON;
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (1, N'Administrador', '4236F25C0643A0CE7457BBF3E34E6A16AB3931FC03C6FFC9C496C94A3DE13564');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (2, N'Gestión de Usuarios', '89E2A52D6A39A527603F46F8905D18F1958A9D56E14660B471B3AE3A30D79F7C');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (3, N'Auditoría', '31494F81786CDEBC84406735CFD1F49DD40FBACC2F547A68CF2451A76F20635F');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (4, N'Operario de stock', '90E76A6FC4994643F01C76E6FC905FF846D0119EE9204EC151D8D05DC8455AD5');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (5, N'Vendedor', '0650DDCB4AA970B6D55EEE018500A880768B2D715ECBF9F2189906168D5BC186');
INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (6, N'Veterinario', 'CA5E346E26E9EAE4563416091AC7832E4024C11BEBFA28F8632E08180D8274A6');
SET IDENTITY_INSERT Familia OFF;

INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 4, 'EFA56CA206719F3AD5536742F050CA48D5A9D889D76284D2D79ED572010E61C8');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 9, '9C193D36F4223E11A575D9A483B1549729D16B18D81154BEA0EBD79A1AEA839E');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 10, '49783CD3927D10F44BA87B752698C152B6C9862360E82B5831163AA3D97CC8D7');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 23, 'F605A2FD23E0389E0031B6A017FBBEAEFB4889EFF3F2EE20B8DDC149616C8CB7');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 26, '465CF84D5339D0688052D96AA13EE6CF4255CE0A34852C6791328274ACE2AE26');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 30, '7F0BA9B7239D60193E34E6E313648889D98D4BA69A982B5B0A2B2C7529BC4F1C');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 32, '89E42F53FD9071C68B727EDC6E7CA2812B5143BAB5152242B0A260CD269646A6');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 1, '75552B08D0E0493C69BB75B221AF305A7D6D7000DEFAB6C73DA0DB6F3740CA1E');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 2, 'E9743ABF50693F514F2138D468B0F043875F88A48B887A57BCCF36555C0034F1');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 3, 'F208A336557A8A4DDABF10C983EB2357068975E845D1BF9C17E47EE81BF109D1');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (2, 5, 'E6D25A07E94E832CB0A795237323E7FF3257F121414DECA5A6EF1EC2DB0E4BA6');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (3, 6, '454217652E98A6BEA4B68B7E0916D22E18AEDACE87DE9251B327CDB9F3BD2229');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (3, 7, '9EEBDF5E3F7E97113E09C686102E3072C657B9FF8EB99757A8E655688B62FA6B');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (3, 8, 'C8913C97B1F67FA8728FF8BEA2757354785D47DB7F016441B02E43F99B33E465');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (3, 31, '7D17D66E423087081E05B98D41137DC6BC5F08F06B642F6BBA2BCA0523148E2D');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 21, '8EB6997128829D9843838925484D25BC204A623978D8883C9DB2CF1E20EC995D');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 22, '4A85AFC9633FA5D1BDD4B7BF03F191D9D6FADB9BE46B6A7F4D02546784710A64');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 24, 'BB20BEE58A5B1EC87FFEAAB63AC1153050DA98637D224D68E25F38EE9C19C87B');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 25, '083A495B2D085EF00546DC7CE53426CC72249F9917C509A45C7F81D817B761EF');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 26, '4881B37C480D3E584DA2A42D008C704D0193F459A3AA6453620B8255E2718B58');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 11, 'AB36D4B952B558F264EB703BCA5FC8DE4BC45F46FC0FD9CDC3E0F16A98AC3E07');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 12, 'B109DF5AAB31A4617A5F80BC43D2E494322EF5A6CD4191B5002DA2EF5760FE4C');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 13, '66AFEB1DC6C9006D6344D812EB131FFE89D447DECE83370F6968AA3C347263BC');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 14, 'C1E87394B289C9B163A9AC9C7AD100D7DDA732CFB8FF637B616912E1586FBAE4');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (5, 19, '58E31CEB3D73936C2CD3E785957D5F0EEBA4D78A15806C4F888E0C0028D96DD9');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 13, 'AA9A21679C341A8B9E90E48AD1EFDF685EAECD973C67E9892536244DB25C6616');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 15, 'E45FFC6DDF933F33799A09790301A8A7CF8652C45C67851112708B8843037FBC');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 16, 'DCC45D8DA12F0CE946B7010D49D053FD51140124C0C85770F4FD06A60EEF61DC');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 17, 'FAE6EB3C9604E561E0EBB164EC89313E849BFF9A120B7AC6343E2D8372DB5E3A');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 18, 'A2A5F52B9B214E0197D0385C8CE60AD9C6F68D13782127A598672333FD5BC879');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 19, '700F6DA1C589F31A0079BE0C533F8FAE251EDC8E2A3FDAA22AA10DD378880CBD');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 20, 'F03107CF26DECB55348C6C1088B5953DEA0BA46A658946463847365A7DE03CEB');
INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (6, 30, 'E346E821BD8FAC341A642710F2E0C61637A5AF8CC45AB63D393013966E1151E7');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 2, 'CB4235A38D71766EE931064D369A34262391A1AB19175AECDF86C33ACFDEDC0B');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 3, 'DFBFD56D72390C4C5449D60ABBA865CF536212E8BF1549D229C0EDBE7E64A8D3');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 4, 'A89F60F5A85FEC6AF3111681D0A1EB2AF8265992322DADCBA7726AD1DD4C9A1E');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 5, 'E8830CD3829711FF83A0B6089E43FDD6EE52B35D17222578436988885DA74FAF');
INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 6, '2D4FE081D1F545E217182455E99B93758D46B368F6019D75E34D2DE189AA8711');
GO

SET IDENTITY_INSERT Usuario ON;
INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)
     VALUES (1, N'admin', N'ma7/zo5+YW0k+MS0PPewyg==.Lv96YbrruRxT4JksRe2TQI1xbYqjJg4BCjkQjIf+9SA=', N'Mariano', N'Núñez', N'MYYMYQO8+CE/GjYcDPpJ9w==.2QmRejKUN+hylboe7oshrg==', N's6OA15E0WToM3SGrzfQBuQ==.YyD9rhynGHGQO5DsqoOHCH69fwFNGzd7v6enG2gaE2Q=', 1, 0, 0, 0, 0, '8003327C20BC4F7BD074DF392D23A335BD4F2D815DF31ECFCA7DA74BE42B371A');
INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)
     VALUES (2, N'operario', N'g9hcAlyd4r3n7ztPy/kskw==.MyZRAAzcW0YaA9KNOy2K0DlvC+m0cOiem9d5hPRL9ao=', N'Sofía', N'Ramírez', N'GEx5pFrEMoCTIoKlRlNaXQ==.bcl7UtY+rkNXlNzdBdXNWw==', N'PQoNS5itwB+allPfzbOlhA==.lyLWs9qbfypF4rKLFCQW2l6kv+QFLUgvqH8HfN/O8+8=', 1, 0, 0, 0, 0, 'B00A5A05511CE6289F97FB3B3EE73AEFA107CAC84570FB9E0B1093CC18DF7631');
INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)
     VALUES (3, N'vendedor', N'CX6z5aqtyKx5kIHOLJT0xQ==.fH+ldvaZuAnvHysbvUDE01I/D7SCH+NhssBuYj7Dw8s=', N'Lucas', N'González', N'zPO/0hfnMXAMCf6ecn69jw==.BIA1wJGaC3FtD6J2n2RTGw==', N'BiGiSM+8HwNy9LuKdAhTtw==.l4dBI2fhF8wplh2gb1aWjTt/V7h6hlvxk3zgNyRkVWc=', 1, 0, 0, 0, 0, 'EC5B83C2DB3B7F5AAFECDD9713DB5D0B932E574223DAEA61407DC82DAD2C6856');
INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)
     VALUES (4, N'veterinario', N'9ZSoxHabfmHKulPo+TtRqA==.SvuKYQPeu/KZ2dQMLxZMJ2IyY4hgt7qj1sa6n/kd5Fk=', N'Carla', N'Domínguez', N'137rx/BSGWI1gskai8Mv5w==.qG3iFHv83NQ1dJy3cd09tA==', N'sITiRO5bjCKOIWJpiixEwA==.dTb0HOW6mfrdsDQxp2Nln3O91OIVflKCsGFBaixhBao=', 1, 0, 0, 0, 0, 'AC29DC0FD0721DD05D173B2174FD66D413B8F155888B8F665C1F804C55027B96');
SET IDENTITY_INSERT Usuario OFF;

INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID, DVH) VALUES (1, 1, 'FD8C07E3E37AF4D8DAC7026E65AD9E80CAB7897C5A534C9B66593B188AB3D40D');
INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID, DVH) VALUES (2, 4, '44046515A543E6A16DA7D5F618C735744C8FD2E8563D4DA78D9988513077233D');
INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID, DVH) VALUES (3, 5, 'FF99F9393125045555391EE5BD1D2A3AFC032178A3E33CD46C7B81D98C6C4AD7');
INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID, DVH) VALUES (4, 6, '886C98E99EE2225A09A3BF4B83ACE69EAED025A71CAC8561E003867F2F5F8EFB');
INSERT INTO UsuarioPatente (UsuarioID, PatenteID, DVH) VALUES (2, 23, 'EFC7544E21CDAF4E30A5DB95A9842B9ADBEAA26CFC23703C185E138C1D935B29');
GO

SET IDENTITY_INSERT Categoria ON;
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (1, N'Medicamentos', NULL);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (2, N'Vacunas', 35.00);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (3, N'Antiparasitarios', NULL);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (4, N'Alimentos', 20.00);
INSERT INTO Categoria (CategoriaID, Nombre, Margen) VALUES (5, N'Accesorios', 40.00);
SET IDENTITY_INSERT Categoria OFF;

SET IDENTITY_INSERT Proveedor ON;
INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT, Telefono, Email, Direccion, Contacto, Activo, DVH)
     VALUES (1, N'Droguería Veterinaria del Sur S.A.', '30712345671', '1142445500', N'ventas@drogueriadelsur.com.ar', N'Av. Hipólito Yrigoyen 10250, Lomas de Zamora', N'Marcela Ríos', 1, 'F41D6ACB248B218854D56A17C73B3020F1EF5EAA02664D18AC30C13176C24ACD');
INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT, Telefono, Email, Direccion, Contacto, Activo, DVH)
     VALUES (2, N'Distribuidora Pet Health S.R.L.', '30709876542', '1148763300', N'pedidos@pethealth.com.ar', N'Av. Belgrano 1840, CABA', NULL, 1, '418AA95A9AAEF77D7CC22E70ACBAA8546B80934D65319EE6F33A7A9716D2F7E9');
INSERT INTO Proveedor (ProveedorID, RazonSocial, CUIT, Telefono, Email, Direccion, Contacto, Activo, DVH)
     VALUES (3, N'Laboratorios BioVet S.A.', '30698765433', '2214556677', NULL, N'Calle 48 1120, La Plata', N'Jorge Salinas', 1, '0E34F4BCCC32D2EAF4EC6A77A590BBFC26B0CBFB213E43C1B9ED71951F9CADA2');
SET IDENTITY_INSERT Proveedor OFF;

INSERT INTO Configuracion (Clave, Valor) VALUES ('MargenGeneral', '30.00');
INSERT INTO Configuracion (Clave, Valor) VALUES ('StockMinimoSugerido', '5');
GO

-- Productos usados por el carrito de medicación (CUN09). El trigger de la bitácora de cambios está deshabilitado por ahora.
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

SET IDENTITY_INSERT Cliente ON;
INSERT INTO Cliente (ClienteID, TipoDocumento, NroDocumento, Nombre, Apellido, Direccion, Telefono, Email, Activo, DVH)
     VALUES (1, N'DNI', N'28555111', N'Laura', N'Pereyra', N'Av. Hipólito Yrigoyen 8900, Lomas de Zamora', N'11 4244-1020', N'lpereyra@mail.com', 1, '3D78F815F828BABDD06734A8717D58CCFE601520B64BB3F5A0305D712C4C1B8C');
INSERT INTO Cliente (ClienteID, TipoDocumento, NroDocumento, Nombre, Apellido, Direccion, Telefono, Email, Activo, DVH)
     VALUES (2, N'DNI', N'31777222', N'Diego', N'Acosta', N'Laprida 450, Lomas de Zamora', N'11 5566-7788', NULL, 1, '6B41FAFAAAAAC51FE8444AB35C31EB1EA2A8B7072A9F58E8682A1DEABF5971A2');
SET IDENTITY_INSERT Cliente OFF;

SET IDENTITY_INSERT Mascota ON;
INSERT INTO Mascota (MascotaID, ClienteID, Nombre, Especie, Raza, Sexo, FechaNacimiento, Activo, DVH)
     VALUES (1, 1, N'Toby', N'Perro', N'Caniche', 'M', '2019-04-12', 1, '7BE945BE26F597B6C6BA99F67CC300BCD2122C02922433A22CCEFD3E7E249C9A');
INSERT INTO Mascota (MascotaID, ClienteID, Nombre, Especie, Raza, Sexo, FechaNacimiento, Activo, DVH)
     VALUES (2, 1, N'Mora', N'Gato', N'Europeo común', 'H', '2021-09-03', 1, 'DD98C8C9274E2A105DA6DBE3869732EA44EE2859C87D223162C6B6DDA853639C');
INSERT INTO Mascota (MascotaID, ClienteID, Nombre, Especie, Raza, Sexo, FechaNacimiento, Activo, DVH)
     VALUES (3, 2, N'Rocco', N'Perro', N'Labrador', 'M', '2022-01-20', 1, '1A28C6F0C8E3533C21839CB98AB9E10207F7827E15119F40BEE404FCBA363BDC');
SET IDENTITY_INSERT Mascota OFF;

SET IDENTITY_INSERT HistoriaClinica ON;
INSERT INTO HistoriaClinica (HistoriaClinicaID, MascotaID, FechaApertura, PesoActual, Alergias, Observaciones, UsuarioID, DVH)
     VALUES (1, 1, '2026-09-01T10:30:00', 7.40, N'Sin alergias registradas', NULL, 4, 'DF5231ED5D4423A0D10EE5EE021F8AACB30FE05F191B888F75EAFDDD818D9B13');
SET IDENTITY_INSERT HistoriaClinica OFF;
GO

INSERT INTO DVV (Tabla, DVV) VALUES ('Usuario', '9009845F0ADF3195CA062E656ACEDEE06C95698F2AB616A89EFDCE8F405F5170');
INSERT INTO DVV (Tabla, DVV) VALUES ('Patente', '803B02DF9E6C09B84DED9D05BA30EBED85835DD122F95D23B90F7F45AC7AE334');
INSERT INTO DVV (Tabla, DVV) VALUES ('Familia', '6491A6587F7A15AE80FF44616D89F38DCCF327F2E30DF66F408CFEC910F0ECC3');
INSERT INTO DVV (Tabla, DVV) VALUES ('FamiliaPatente', 'FAF84C2898CF76C3CA257325891695D933EFB03DBD80A0BB584291ADCE46EB6D');
INSERT INTO DVV (Tabla, DVV) VALUES ('FamiliaFamilia', '37CECA68568F37F4CFD3BD5530E1592273BBA7FBB87BAFFF31306053CDC43E43');
INSERT INTO DVV (Tabla, DVV) VALUES ('UsuarioFamilia', 'EE144C71651E31BDB650EB9B893391259AD27055DD8EF3D06D228142BB946915');
INSERT INTO DVV (Tabla, DVV) VALUES ('UsuarioPatente', '81664C382F035D276B96BC8DED42A6C602B2EFBC32EE55B107A1E4B4572EED07');
INSERT INTO DVV (Tabla, DVV) VALUES ('Bitacora', '1E1CEE03C9F3047366842C37A5B35E236FB6CE40AAEF9133ECA685921AC5145E');
INSERT INTO DVV (Tabla, DVV) VALUES ('Productos', '91460DB02B8CB33F139A92011C09C686668DBAA7460A2B92448250E540DA1E09');
INSERT INTO DVV (Tabla, DVV) VALUES ('MovimientoStock', '83BDDF37682E8B90799B49FB59A561480FB233BCEA73658518FC05E9627442F7');
INSERT INTO DVV (Tabla, DVV) VALUES ('Cliente', '12B1D957863F9666323223B99638491A74B4ED603EF3287C68072D0EE4F56991');
INSERT INTO DVV (Tabla, DVV) VALUES ('Mascota', '08381F714CDD809D75633C3A59C34D8C406B2BBA60180A706D7F311BE1FEC956');
INSERT INTO DVV (Tabla, DVV) VALUES ('HistoriaClinica', 'CAD7E6E6CB7029A563513A84AA6E80E1AA80997F70338B494D7EE8FA59329B9F');
INSERT INTO DVV (Tabla, DVV) VALUES ('Atencion', '159BD053D5A6DA4FEE4E349E3DFAE3A6A79A0F056D6AE27C506C94739D22D370');
INSERT INTO DVV (Tabla, DVV) VALUES ('Proveedor', '562BD6B48753BCA4D19E7D1C19B88C1EFDCA923264F363B10EC02AC5F5E413DD');
INSERT INTO DVV (Tabla, DVV) VALUES ('VentaEncabezado', '3D8A1546A48B4082256EECE07FE96F8CB38FBF07AB902CF839ED5F46E8F6FB2F');
INSERT INTO DVV (Tabla, DVV) VALUES ('VentaDetalle', '61213FD3894AEEDF3EA4A9F07B96E522E44339290A362428E3A67E2A55783E11');
GO

PRINT 'VetCareBD_60MN creada correctamente.';
GO
