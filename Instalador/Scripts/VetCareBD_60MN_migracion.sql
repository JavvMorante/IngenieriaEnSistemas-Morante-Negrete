/* =====================================================================
   VetCare - Script de MIGRACIÓN de VetCareBD_60MN  (V2.4 -> V3.0)
   ---------------------------------------------------------------------
   Lo ejecuta VetCare_Setup.exe cuando la base YA EXISTE, después del
   backup preventivo (VetCareBD_60MN_preinstalacion_aaaammdd_hhmm.bak).

   Regla: solo AGREGA lo nuevo. No borra tablas, columnas ni datos.
   Todo está protegido (IF OBJECT_ID / COL_LENGTH / IF NOT EXISTS), así
   que se puede ejecutar más de una vez sin errores.

   Cambios respecto de la versión 2.4:
     1. Productos.Margen y Productos_C.Margen (margen por producto).
     2. AlertaReposicion.Atendida.
     3. Trigger TR_Productos_BitacoraCambios con el campo Margen.
     4. Módulo de Compras (RFN2): SolicitudReposicion, SolicitudReposicionDetalle,
        OrdenCompra, OrdenCompraDetalle, PagoProveedor, Recepcion, RecepcionDetalle.
     5. Datos de seguridad nuevos: patentes CMP_* y REP_COMPRAS, familia
        "Analista de compras", usuario "compras" y configuración TopeCompraMensual.
     6. Registro de la versión aplicada (tabla VersionBD).

   Dígitos verificadores: las filas nuevas llevan su DVH calculado con las
   claves del App.config. Las filas existentes de Productos cambian (se
   agrega Margen), así que en el primer ingreso VetCare detectará
   inconsistencias de DVH/DVV: el administrador las resuelve con
   CU11 Reparar inconsistencia (Recalcular). Es el paso 6 del instalador.
   ===================================================================== */

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE VetCareBD_60MN;
GO

/* ---------------------------------------------------------------------
   1. Margen por producto
   Se agrega primero como NULL, se completa con el margen real de cada
   producto ((venta - costo) / costo) y después se vuelve obligatorio.
   --------------------------------------------------------------------- */
IF COL_LENGTH('dbo.Productos', 'Margen') IS NULL
    ALTER TABLE dbo.Productos ADD Margen DECIMAL(6,2) NULL;
GO
IF COL_LENGTH('dbo.Productos_C', 'Margen') IS NULL
    ALTER TABLE dbo.Productos_C ADD Margen DECIMAL(6,2) NULL;
GO

-- El trigger viejo no conoce Margen: se quita antes de completar los datos
-- (así la migración tampoco genera registros falsos en la bitácora de cambios).
IF OBJECT_ID(N'dbo.TR_Productos_BitacoraCambios', N'TR') IS NOT NULL
    DROP TRIGGER dbo.TR_Productos_BitacoraCambios;
GO

DECLARE @margenGeneral DECIMAL(6,2) =
    ISNULL((SELECT TRY_CAST(Valor AS DECIMAL(6,2)) FROM dbo.Configuracion WHERE Clave = 'MargenGeneral'), 0);

UPDATE p
   SET Margen = CASE
                    WHEN p.PrecioCosto > 0 AND p.PrecioVenta >= p.PrecioCosto
                        THEN CASE WHEN (p.PrecioVenta - p.PrecioCosto) * 100.0 / p.PrecioCosto > 9999.99 THEN 9999.99
                                  ELSE CAST((p.PrecioVenta - p.PrecioCosto) * 100.0 / p.PrecioCosto AS DECIMAL(6,2)) END
                    WHEN p.PrecioCosto > 0 THEN 0
                    ELSE ISNULL(c.Margen, @margenGeneral)
                END
  FROM dbo.Productos p
  LEFT JOIN dbo.Categoria c ON c.CategoriaID = p.CategoriaID
 WHERE p.Margen IS NULL;

UPDATE h
   SET Margen = CASE
                    WHEN h.PrecioCosto > 0 AND h.PrecioVenta >= h.PrecioCosto
                        THEN CASE WHEN (h.PrecioVenta - h.PrecioCosto) * 100.0 / h.PrecioCosto > 9999.99 THEN 9999.99
                                  ELSE CAST((h.PrecioVenta - h.PrecioCosto) * 100.0 / h.PrecioCosto AS DECIMAL(6,2)) END
                    ELSE 0
                END
  FROM dbo.Productos_C h
 WHERE h.Margen IS NULL;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Productos') AND name = 'Margen' AND is_nullable = 1)
    ALTER TABLE dbo.Productos ALTER COLUMN Margen DECIMAL(6,2) NOT NULL;
GO
IF OBJECT_ID(N'dbo.CK_Productos_Margen', N'C') IS NULL
    ALTER TABLE dbo.Productos ADD CONSTRAINT CK_Productos_Margen CHECK (Margen >= 0);
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Productos_C') AND name = 'Margen' AND is_nullable = 1)
    ALTER TABLE dbo.Productos_C ALTER COLUMN Margen DECIMAL(6,2) NOT NULL;
GO

/* ---------------------------------------------------------------------
   2. AlertaReposicion.Atendida (las alertas existentes quedan como no atendidas)
   --------------------------------------------------------------------- */
IF COL_LENGTH('dbo.AlertaReposicion', 'Atendida') IS NULL
    ALTER TABLE dbo.AlertaReposicion ADD Atendida BIT NOT NULL
        CONSTRAINT DF_AlertaReposicion_Atendida DEFAULT (0);
GO

/* ---------------------------------------------------------------------
   3. Trigger de la bitácora de cambios (versión 3.0, incluye Margen)
   --------------------------------------------------------------------- */

CREATE OR ALTER TRIGGER TR_Productos_BitacoraCambios
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
                OR UPDATE(StockMinimo) OR UPDATE(PrecioCosto) OR UPDATE(PrecioVenta) OR UPDATE(Margen)
                OR UPDATE(FechaVencimiento) OR UPDATE(Lote) OR UPDATE(Activo))
        RETURN;

    -- Cambio de existencias (venta CUN05, o recepción CUN14 que además actualiza lote y vencimiento): MOVIMIENTO.
    DECLARE @soloStock BIT = 0;
    IF UPDATE(StockActual)
       AND NOT (UPDATE(Nombre) OR UPDATE(CategoriaID) OR UPDATE(ProveedorID) OR UPDATE(StockMinimo)
                OR UPDATE(PrecioCosto) OR UPDATE(PrecioVenta) OR UPDATE(Margen))
        SET @soloStock = 1;

    UPDATE c
       SET Act = 0
      FROM Productos_C c
     WHERE c.Act = 1
       AND c.ProductoID IN (SELECT ProductoID FROM inserted UNION SELECT ProductoID FROM deleted);

    IF @hayInsertados = 1
        INSERT INTO Productos_C (ProductoID, Codigo, Nombre, CategoriaID, ProveedorID, StockActual, StockMinimo,
                                 PrecioCosto, PrecioVenta, Margen, FechaVencimiento, Lote, ActivoProducto,
                                 Operacion, FechaHora, UsuarioID, Act)
        SELECT i.ProductoID, i.Codigo, i.Nombre, i.CategoriaID, i.ProveedorID, i.StockActual, i.StockMinimo,
               i.PrecioCosto, i.PrecioVenta, i.Margen, i.FechaVencimiento, i.Lote, i.Activo,
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
                                 PrecioCosto, PrecioVenta, Margen, FechaVencimiento, Lote, ActivoProducto,
                                 Operacion, FechaHora, UsuarioID, Act)
        SELECT d.ProductoID, d.Codigo, d.Nombre, d.CategoriaID, d.ProveedorID, d.StockActual, d.StockMinimo,
               d.PrecioCosto, d.PrecioVenta, d.Margen, d.FechaVencimiento, d.Lote, d.Activo,
               'ELIMINACION', SYSDATETIME(), CAST(SESSION_CONTEXT(N'UsuarioID') AS INT), 1
          FROM deleted d;
END
GO

/* ---------------------------------------------------------------------
   4. Módulo de Compras (RFN2) - tablas nuevas
   --------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.SolicitudReposicion', N'U') IS NULL
BEGIN
    CREATE TABLE SolicitudReposicion (
        SolicitudID    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SolicitudReposicion PRIMARY KEY,
        Numero         AS ('SR-' + RIGHT('00000' + CAST(SolicitudID AS VARCHAR(10)), 5)) PERSISTED,
        FechaHora      DATETIME2(0)      NOT NULL,
        UsuarioID      INT               NOT NULL CONSTRAINT FK_SolicitudReposicion_Usuario REFERENCES Usuario (UsuarioID),
        Estado         VARCHAR(20)       NOT NULL CONSTRAINT CK_SolicitudReposicion_Estado
                                         CHECK (Estado IN ('PENDIENTE', 'EN_COMPRA', 'CERRADA', 'ANULADA')),
        Observaciones  VARCHAR(400)      NULL,
        DVH            CHAR(64)          NULL
    );
END
GO

IF OBJECT_ID(N'dbo.SolicitudReposicionDetalle', N'U') IS NULL
BEGIN
    CREATE TABLE SolicitudReposicionDetalle (
        SolicitudDetalleID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SolicitudReposicionDetalle PRIMARY KEY,
        SolicitudID         INT               NOT NULL CONSTRAINT FK_SolicitudDetalle_Solicitud REFERENCES SolicitudReposicion (SolicitudID),
        ProductoID          INT               NOT NULL CONSTRAINT FK_SolicitudDetalle_Producto REFERENCES Productos (ProductoID),
        StockActual         INT               NOT NULL,  -- foto del stock al generar la solicitud
        StockMinimo         INT               NOT NULL,
        CantidadPedida      INT               NOT NULL CONSTRAINT CK_SolicitudDetalle_Cantidad CHECK (CantidadPedida > 0),
        DVH                 CHAR(64)          NULL,
        CONSTRAINT UX_SolicitudDetalle_Producto UNIQUE (SolicitudID, ProductoID)
    );
END
GO

IF OBJECT_ID(N'dbo.OrdenCompra', N'U') IS NULL
BEGIN
    CREATE TABLE OrdenCompra (
        OrdenCompraID     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrdenCompra PRIMARY KEY,
        Numero            AS ('OC-' + RIGHT('00000' + CAST(OrdenCompraID AS VARCHAR(10)), 5)) PERSISTED,
        SolicitudID       INT               NOT NULL CONSTRAINT FK_OrdenCompra_Solicitud REFERENCES SolicitudReposicion (SolicitudID),
        ProveedorID       INT               NOT NULL CONSTRAINT FK_OrdenCompra_Proveedor REFERENCES Proveedor (ProveedorID),
        FechaHora         DATETIME2(0)      NOT NULL,
        NroCotizacion     VARCHAR(30)       NOT NULL,
        PlazoEntregaDias  INT               NOT NULL CONSTRAINT CK_OrdenCompra_Plazo CHECK (PlazoEntregaDias >= 0),
        CondicionPago     VARCHAR(60)       NOT NULL,
        MotivoEleccion    VARCHAR(300)      NULL,
        Advertencias      VARCHAR(400)      NULL,
        Total             DECIMAL(12,2)     NOT NULL CONSTRAINT CK_OrdenCompra_Total CHECK (Total > 0),
        Estado            VARCHAR(20)       NOT NULL CONSTRAINT CK_OrdenCompra_Estado
                                            CHECK (Estado IN ('PENDIENTE_APROBACION', 'APROBADA', 'RECHAZADA', 'ENVIADA',
                                                              'PAGADA', 'RECIBIDA_PARCIAL', 'CERRADA', 'ANULADA')),
        UsuarioID         INT               NOT NULL CONSTRAINT FK_OrdenCompra_Usuario REFERENCES Usuario (UsuarioID),
        AprobadaPor       INT               NULL     CONSTRAINT FK_OrdenCompra_Aprobador REFERENCES Usuario (UsuarioID),
        FechaAprobacion   DATETIME2(0)      NULL,
        MotivoRechazo     VARCHAR(300)      NULL,
        DVH               CHAR(64)          NULL
    );
    CREATE INDEX IX_OrdenCompra_Estado ON OrdenCompra (Estado, FechaHora);
    CREATE INDEX IX_OrdenCompra_Solicitud ON OrdenCompra (SolicitudID);
END
GO

IF OBJECT_ID(N'dbo.OrdenCompraDetalle', N'U') IS NULL
BEGIN
    CREATE TABLE OrdenCompraDetalle (
        OrdenDetalleID    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrdenCompraDetalle PRIMARY KEY,
        OrdenCompraID     INT               NOT NULL CONSTRAINT FK_OrdenDetalle_Orden REFERENCES OrdenCompra (OrdenCompraID),
        ProductoID        INT               NOT NULL CONSTRAINT FK_OrdenDetalle_Producto REFERENCES Productos (ProductoID),
        Cantidad          INT               NOT NULL CONSTRAINT CK_OrdenDetalle_Cantidad CHECK (Cantidad > 0),
        PrecioUnitario    DECIMAL(12,2)     NOT NULL CONSTRAINT CK_OrdenDetalle_Precio CHECK (PrecioUnitario > 0),
        Subtotal          DECIMAL(12,2)     NOT NULL,
        CantidadRecibida  INT               NOT NULL CONSTRAINT DF_OrdenDetalle_Recibida DEFAULT (0),
        DVH               CHAR(64)          NULL,
        CONSTRAINT UX_OrdenDetalle_Producto UNIQUE (OrdenCompraID, ProductoID),
        CONSTRAINT CK_OrdenDetalle_Recibida CHECK (CantidadRecibida BETWEEN 0 AND Cantidad)
    );
END
GO

IF OBJECT_ID(N'dbo.PagoProveedor', N'U') IS NULL
BEGIN
    CREATE TABLE PagoProveedor (
        PagoID          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PagoProveedor PRIMARY KEY,
        OrdenCompraID   INT               NOT NULL CONSTRAINT FK_PagoProveedor_Orden REFERENCES OrdenCompra (OrdenCompraID)
                                          CONSTRAINT UX_PagoProveedor_Orden UNIQUE,
        Banco           VARCHAR(60)       NOT NULL,
        NroComprobante  VARCHAR(40)       NOT NULL,
        FechaPago       DATE              NOT NULL,
        Importe         DECIMAL(12,2)     NOT NULL CONSTRAINT CK_PagoProveedor_Importe CHECK (Importe > 0),
        Observaciones   VARCHAR(300)      NULL,      -- motivo cuando el importe difiere del total de la orden
        UsuarioID       INT               NOT NULL CONSTRAINT FK_PagoProveedor_Usuario REFERENCES Usuario (UsuarioID),
        DVH             CHAR(64)          NULL,
        CONSTRAINT UX_PagoProveedor_Comprobante UNIQUE (Banco, NroComprobante)
    );
END
GO

IF OBJECT_ID(N'dbo.Recepcion', N'U') IS NULL
BEGIN
    CREATE TABLE Recepcion (
        RecepcionID     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Recepcion PRIMARY KEY,
        OrdenCompraID   INT               NOT NULL CONSTRAINT FK_Recepcion_Orden REFERENCES OrdenCompra (OrdenCompraID),
        NroRemito       VARCHAR(30)       NOT NULL,
        FechaHora       DATETIME2(0)      NOT NULL,
        UsuarioID       INT               NOT NULL CONSTRAINT FK_Recepcion_Usuario REFERENCES Usuario (UsuarioID),
        Observaciones   VARCHAR(400)      NULL,
        DVH             CHAR(64)          NULL
    );
    CREATE INDEX IX_Recepcion_Orden ON Recepcion (OrdenCompraID);
END
GO

IF OBJECT_ID(N'dbo.RecepcionDetalle', N'U') IS NULL
BEGIN
    CREATE TABLE RecepcionDetalle (
        RecepcionDetalleID  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_RecepcionDetalle PRIMARY KEY,
        RecepcionID         INT               NOT NULL CONSTRAINT FK_RecepcionDetalle_Recepcion REFERENCES Recepcion (RecepcionID),
        ProductoID          INT               NOT NULL CONSTRAINT FK_RecepcionDetalle_Producto REFERENCES Productos (ProductoID),
        CantidadRecibida    INT               NOT NULL CONSTRAINT CK_RecepcionDetalle_Cantidad CHECK (CantidadRecibida >= 0),
        Lote                VARCHAR(40)       NOT NULL,
        FechaVencimiento    DATE              NOT NULL,
        Observacion         VARCHAR(200)      NULL,
        MovimientoID        INT               NULL     CONSTRAINT FK_RecepcionDetalle_Movimiento REFERENCES MovimientoStock (MovimientoID),
        DVH                 CHAR(64)          NULL
    );
    CREATE INDEX IX_RecepcionDetalle_Recepcion ON RecepcionDetalle (RecepcionID);
END
GO

/* ---------------------------------------------------------------------
   5. Datos de seguridad y configuración nuevos
   (DVH tomados del script de la versión 3.0, generados con las mismas claves)
   --------------------------------------------------------------------- */

-- Nombres actualizados (solo si siguen con el nombre de la versión 2.4)
UPDATE dbo.Patente SET Nombre = N'Deserializar bitácora de eventos (XML)', DVH = '9EE8BD19304F40CA4332ADB9BA58A2BCC91C42555E3B3B4B0626CEBD5B0F406E'
 WHERE PatenteID = 32 AND Nombre = N'Importar bitácora de eventos (XML)';
UPDATE dbo.Familia SET Nombre = N'Encargado de depósito', DVH = '0FD251B2D42D62EF3F305FA295D1C098292EF7B2AFC81395E81BE66FBAF815D4'
 WHERE FamiliaID = 4 AND Nombre = N'Operario de stock';
GO

-- Patentes del módulo de Compras
SET IDENTITY_INSERT dbo.Patente ON;
IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE PatenteID = 33 OR Codigo = N'CMP_SOLICITUD')
    INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (33, N'Generar solicitud de reposición', N'CMP_SOLICITUD', '8478BD4CC35858FBB2C680C309398602678347AA1844C453459136BD7E9DA1CB');
IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE PatenteID = 34 OR Codigo = N'CMP_ORDEN')
    INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (34, N'Registrar orden de compra', N'CMP_ORDEN', '36E18164C461DC8E2B9850FE035AC60D23F07A78F1A24DF373F4BB10824C2D46');
IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE PatenteID = 35 OR Codigo = N'CMP_APROBAR')
    INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (35, N'Aprobar orden de compra', N'CMP_APROBAR', '2590B24E7F074E7A0A2465D267CF1FDFCE5C8CCC5CEEBA766579CDE32295547B');
IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE PatenteID = 36 OR Codigo = N'CMP_PAGO')
    INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (36, N'Registrar pago al proveedor', N'CMP_PAGO', '3B7E86FE77F705A0FD3611C1BA99A2E68A3ADA03746E22B5CD0BCF287958E8E6');
IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE PatenteID = 37 OR Codigo = N'CMP_RECEPCION')
    INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (37, N'Registrar recepción de mercadería', N'CMP_RECEPCION', 'BFF3EBB9DEF338CACE47BBB548DA8F7D6F4F78C4F96C97B7445AD97248612B6E');
IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE PatenteID = 38 OR Codigo = N'REP_COMPRAS')
    INSERT INTO Patente (PatenteID, Nombre, Codigo, DVH) VALUES (38, N'Generar reporte de compras', N'REP_COMPRAS', 'BB3B4576053DC418610552A190247411367710ED770C63FAE0C77619CC82477F');
SET IDENTITY_INSERT dbo.Patente OFF;
GO

-- Familia "Analista de compras"
SET IDENTITY_INSERT dbo.Familia ON;
IF NOT EXISTS (SELECT 1 FROM dbo.Familia WHERE FamiliaID = 7)
    INSERT INTO Familia (FamiliaID, Nombre, DVH) VALUES (7, N'Analista de compras', 'D914EFDE70B0A6667FE1DD081448A064A14B927C0B0948952EED12529ADBD2A7');
SET IDENTITY_INSERT dbo.Familia OFF;
GO

-- El Administrador incluye a la familia Analista de compras (Composite)
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaFamilia WHERE FamiliaPadreID = 1 AND FamiliaHijaID = 7)
    INSERT INTO FamiliaFamilia (FamiliaPadreID, FamiliaHijaID, DVH) VALUES (1, 7, '53DB2C7453F3EFE5A28BA4614F978D7F130BFBC888F24B2C31E30CE5DE8A0D12');
GO

-- Patentes nuevas asignadas a las familias
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 1 AND PatenteID = 35)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 35, 'EB8D8F4B49CE3DFC4873CBB6F4496D4A301E9E3EA0147A9BDA8D48DA901B7B4B');
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 1 AND PatenteID = 38)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (1, 38, 'B9D6EE9413B68662C514ABD8F1391C47BCDEFF5451C77FF7170D3175220027BF');
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 4 AND PatenteID = 33)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 33, 'BFD3D9BB01236F4942D4EE9F2CFBFF9035F996122D1FD0C31A84257FC2C26835');
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 4 AND PatenteID = 37)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (4, 37, '0371DEA29DD8E687A67B97520F56643C0125D5C4317E92852BAAB5E9DE443DD0');
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 7 AND PatenteID = 26)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (7, 26, 'D24476A469FD0B25BB8ECE4F76EAE4024488C03F85EF2A2A8C7DDE336798E871');
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 7 AND PatenteID = 34)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (7, 34, '9062A06643733192A22F0FFD72EDE9B772EA2F3F974EE3F47FA17DE36E1DB49B');
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 7 AND PatenteID = 36)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (7, 36, 'C8AEBE2D3E655435F535BEEB7B455156DEC6BB5BA6528002545730615632FA5A');
IF NOT EXISTS (SELECT 1 FROM dbo.FamiliaPatente WHERE FamiliaID = 7 AND PatenteID = 38)
    INSERT INTO FamiliaPatente (FamiliaID, PatenteID, DVH) VALUES (7, 38, '94F24FFD96085B050B388324F436211CF4BD2B58F5E1937B6605B3FA3CF2D237');
GO

-- Usuario inicial "compras" (clave Compras1234), solo si el ID 5 y el nombre están libres
SET IDENTITY_INSERT dbo.Usuario ON;
IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE UsuarioID = 5 OR Usuario = N'compras')
    INSERT INTO Usuario (UsuarioID, Usuario, Clave, Nombre, Apellido, DNI, Email, Activo, Bloqueado, IntentosFallidos, PrimerIngreso, EnSesion, DVH)
    VALUES (5, N'compras', N'3ktrpp5kV7dMOPg0xTuOuA==.fwSAmSf3beCu7S24XefmsYVXygkAXSgWiSrKwsws4Ic=', N'Martín', N'Herrera', N'T+GQ8Kn0f/JXQ5rmXaKHOw==.840NI9VzsNGpKO0TQfmSGw==', N'FkrMTjRb6W1Acl80dM5xhQ==.fKU/b2G2aD4MIgSmKeiW2buLTYhLblExg9uaONxQ6SI=', 1, 0, 0, 0, 0, 'D1B74B91D287B1FCACA727EAD1BE532E0C2C93F54C1F963284C6139E445234E3');
SET IDENTITY_INSERT dbo.Usuario OFF;
GO

IF EXISTS (SELECT 1 FROM dbo.Usuario WHERE UsuarioID = 5 AND Usuario = N'compras')
   AND NOT EXISTS (SELECT 1 FROM dbo.UsuarioFamilia WHERE UsuarioID = 5 AND FamiliaID = 7)
    INSERT INTO UsuarioFamilia (UsuarioID, FamiliaID, DVH) VALUES (5, 7, '2525F7C68BC2C6EE9F9896409085F2300AE13BE52C936CFAF39460E80B4E2129');
GO

-- Tope mensual de compras (CUN11)
IF NOT EXISTS (SELECT 1 FROM dbo.Configuracion WHERE Clave = 'TopeCompraMensual')
    INSERT INTO Configuracion (Clave, Valor) VALUES ('TopeCompraMensual', '2000000.00');
GO

-- Dígito verificador vertical de las tablas nuevas (vacías)
IF NOT EXISTS (SELECT 1 FROM dbo.DVV WHERE Tabla = 'SolicitudReposicion')
    INSERT INTO DVV (Tabla, DVV) VALUES ('SolicitudReposicion', '9304278EEC032DA32C46E704DBD92BB51E307000694BBE49883BD31048348ECB');
IF NOT EXISTS (SELECT 1 FROM dbo.DVV WHERE Tabla = 'SolicitudReposicionDetalle')
    INSERT INTO DVV (Tabla, DVV) VALUES ('SolicitudReposicionDetalle', '9623503542FF568C356D76C6821E3CAF2C3A2948821DD3C88C83D6FEAED0D53C');
IF NOT EXISTS (SELECT 1 FROM dbo.DVV WHERE Tabla = 'OrdenCompra')
    INSERT INTO DVV (Tabla, DVV) VALUES ('OrdenCompra', '94F1A5DC85E132259F69136855A31F4F2F91EC1A65C7BCE8A2FD3FB4AC81F981');
IF NOT EXISTS (SELECT 1 FROM dbo.DVV WHERE Tabla = 'OrdenCompraDetalle')
    INSERT INTO DVV (Tabla, DVV) VALUES ('OrdenCompraDetalle', '21FF4F7ADEB3260B4DC5E4A314B1D92247729FED858624D31657106B9827F841');
IF NOT EXISTS (SELECT 1 FROM dbo.DVV WHERE Tabla = 'PagoProveedor')
    INSERT INTO DVV (Tabla, DVV) VALUES ('PagoProveedor', '28377BFE1770C9FF3BE5EF24F2E92E9AECD5D724847FFC2A7E2E1D82B8291148');
IF NOT EXISTS (SELECT 1 FROM dbo.DVV WHERE Tabla = 'Recepcion')
    INSERT INTO DVV (Tabla, DVV) VALUES ('Recepcion', '84F9CEE824B33BEF39F1B80C9B55FA85EF2A3DDC77913F8B98DF0B887702AF6F');
IF NOT EXISTS (SELECT 1 FROM dbo.DVV WHERE Tabla = 'RecepcionDetalle')
    INSERT INTO DVV (Tabla, DVV) VALUES ('RecepcionDetalle', '1C97F249B4C59096DE8E932EBF989274EC9D71670C09FCD83673E12265BC9D79');
GO

/* ---------------------------------------------------------------------
   6. Registro de la versión aplicada
   --------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.VersionBD', N'U') IS NULL
    CREATE TABLE dbo.VersionBD (
        Version          VARCHAR(20) NOT NULL CONSTRAINT PK_VersionBD PRIMARY KEY,
        FechaAplicacion  DATETIME    NOT NULL CONSTRAINT DF_VersionBD_Fecha DEFAULT GETDATE()
    );
GO
IF NOT EXISTS (SELECT 1 FROM dbo.VersionBD WHERE Version = '3.0')
    INSERT INTO dbo.VersionBD (Version) VALUES ('3.0');
GO

-- Nota: las patentes de la versión 2.4 que ya no usa el sistema
-- (STK_CONFIG, STK_MOVIMIENTO, STK_REPORTE) y sus asignaciones NO se borran,
-- para respetar la regla de no eliminar datos. Si se quieren quitar, hacerlo
-- desde la gestión de perfiles de VetCare.
PRINT 'Migración de VetCareBD_60MN a la versión 3.0 aplicada.';
GO