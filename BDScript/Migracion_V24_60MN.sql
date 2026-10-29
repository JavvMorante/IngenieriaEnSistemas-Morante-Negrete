/* =====================================================================
   VetCare - Migración de una base EXISTENTE a la versión V2.4 del Trabajo de Diploma.
   (Una instalación nueva no la necesita: VetCareBD_60MN.sql ya trae este esquema.)
   Correr antes BDScript/Migracion_IdiomaPorUsuario_60MN.sql si la base es anterior a esa migración.

   Fase 1 - Menú principal:
     1. Traducciones al inglés del menú nuevo (MAESTROS / PACIENTES / STOCK, Mascotas,
        Historias clínicas, Registrar atención, Ventas registradas, Reporte de atenciones)
        y del selector de paciente (FrmBuscarPaciente_60MN).
   Fase 2 - CUN02 Gestionar Mascotas:
     2. Traducciones al inglés de FrmGestionMascotas_60MN y del selector de cliente
        (FrmBuscarCliente_60MN). No hay cambios de esquema: usa las tablas Mascota y Cliente.
   Fase 3 - CUN11 Gestionar Proveedores:
     3. Agrega a Proveedor: Telefono, Email, Direccion, Contacto, Activo y DVH (tabla protegida).
     4. Deja el CUIT solo con sus 11 dígitos y corrige el dígito verificador de los dos CUIT de
        ejemplo que lo tenían mal; completa teléfono y dirección de los proveedores de ejemplo.
        Un proveedor propio sin teléfono o dirección queda con esos campos vacíos: complételos
        desde MAESTROS > Proveedores > Modificar.
     5. Traducciones al inglés de FrmProveedores_60MN y FrmProveedor_60MN.
     Después: ADMIN > Dígitos verificadores > Recalcular (crea el DVH de cada proveedor y su DVV).
   Fase 4 - CUN05 Carrito y Venta (sin facturación):
     6. Crea VentaEncabezado y VentaDetalle (tablas protegidas por DV).
     7. Pasa la medicación de AtencionDetalle a ventas (una venta por atención, a nombre del
        dueño de la mascota, con la fecha y el usuario del primer egreso) y elimina AtencionDetalle
        y su DVV. Todo en una transacción: si algo falla, no se cambia nada.
     8. Quita las patentes VEN_FACTURAR, VEN_DESPACHAR y COM_COMPRAS (y sus asignaciones) y los
        textos de esas opciones del menú.
     9. Traducciones al inglés del carrito y de VENTAS > Ventas registradas.
     Después: ADMIN > Dígitos verificadores > Recalcular (las ventas migradas no tienen DVH, y
     Patente, FamiliaPatente y UsuarioPatente cambian de filas).
   Fase 5 - CUN07 Generar Reporte de Atenciones:
    10. Traducciones al inglés de FrmReporteAtenciones_60MN. Sin cambios de esquema.
   Fase 6 - Ayuda en línea (F1):
    11. Textos en inglés de los temas de ayuda (claves ayuda.<tema>.titulo / .texto). Sin cambios de esquema.
   Fase 7 - Cierre:
    12. Familias: Operario de stock suma MAE_PROVEEDORES (CUN11) y Veterinario suma REP_REPORTES
        (CUN07 y CUN13). Vendedor ya tiene VEN_CARRITO (carrito y ventas registradas) y el
        Administrador ya tiene todas. Después: recalcular (FamiliaPatente cambia de filas).

   IMPORTANTE - dígitos verificadores: la tabla Traduccion no tiene DVH. Las fases
   siguientes sí cambian tablas protegidas; al terminar, recalcular desde
   ADMIN > Dígitos verificadores > Recalcular dígitos.
   Se puede ejecutar más de una vez: cada paso verifica si ya está aplicado.
   ===================================================================== */
USE VetCareBD_60MN;
GO

-- 1. Fase 1: textos en inglés del menú y del selector de paciente.
MERGE Traduccion AS t
USING (VALUES
    (2, 'mnuClientes', N'Customers'),
    (2, 'mnuMascotas', N'Pets'),
    (2, 'mnuPacientes', N'PATIENTS'),
    (2, 'mnuHistoriasClinicas', N'Medical records'),
    (2, 'mnuRegistrarAtencion', N'Register consultation'),
    (2, 'mnuStock', N'STOCK'),
    (2, 'mnuVentasRegistradas', N'Recorded sales'),
    (2, 'mnuReporteAtenciones', N'Consultations report'),
    (2, 'FrmBuscarPaciente_60MN', N'Select patient'),
    (2, 'FrmBuscarPaciente_60MN.btnAceptar', N'Select'),
    (2, 'colPaciente', N'Patient'),
    (2, 'colPropietario', N'Owner'),
    (2, 'msg.CriterioEspecie', N'Species'),
    (2, 'msg.ElegirPaciente', N'Search for and choose the patient.'),
    (2, 'msg.ElegirPacienteHistoria', N'Choose the patient to see their medical record.'),
    (2, 'msg.ElegirPacienteAtencion', N'Choose the patient to attend.'),
    (2, 'msg.SinPacientes', N'No patients were found with that criterion.'),
    (2, 'msg.SinHistoriaAbrir', N'The pet has no medical record. Do you want to open it now?'),
    (2, 'msg.SinHistoriaAtencion', N'The pet has no medical record. It must be created before registering the consultation. Open it now?')
) AS s (IdiomaID, Clave, Texto)
   ON t.IdiomaID = s.IdiomaID AND t.Clave = s.Clave
WHEN MATCHED THEN UPDATE SET Texto = s.Texto
WHEN NOT MATCHED THEN INSERT (IdiomaID, Clave, Texto) VALUES (s.IdiomaID, s.Clave, s.Texto);
GO

-- 2. Fase 2: textos en inglés de la gestión de mascotas y del selector de cliente.
MERGE Traduccion AS t
USING (VALUES
    (2, 'FrmGestionMascotas_60MN', N'Pet management'),
    (2, 'FrmGestionMascotas_60MN.lblTitulo', N'Pets'),
    (2, 'FrmGestionMascotas_60MN.colMascota', N'Pet'),
    (2, 'FrmGestionMascotas_60MN.btnBaja', N'Deactivate'),
    (2, 'msg.MascotasEncontradas', N'{0} pet(s) found.'),
    (2, 'msg.SinMascotasBusqueda', N'No pets were found. To register one, use "Add".'),
    (2, 'FrmBuscarCliente_60MN', N'Select customer'),
    (2, 'FrmBuscarCliente_60MN.lblTitulo', N'Choose the customer who owns the pet'),
    (2, 'FrmBuscarCliente_60MN.btnAceptar', N'Select'),
    (2, 'msg.SinClientesActivos', N'No active customers were found. Register the customer from MASTER DATA > Customers.')
) AS s (IdiomaID, Clave, Texto)
   ON t.IdiomaID = s.IdiomaID AND t.Clave = s.Clave
WHEN MATCHED THEN UPDATE SET Texto = s.Texto
WHEN NOT MATCHED THEN INSERT (IdiomaID, Clave, Texto) VALUES (s.IdiomaID, s.Clave, s.Texto);
GO

-- 3. Fase 3: columnas nuevas de Proveedor. Telefono y Direccion son obligatorias: las filas
--    existentes quedan con texto vacío hasta completarlas (paso 4 o desde la pantalla).
IF COL_LENGTH('Proveedor', 'Telefono') IS NULL
    ALTER TABLE Proveedor ADD Telefono VARCHAR(30) NOT NULL CONSTRAINT DF_Proveedor_Telefono DEFAULT ('');
IF COL_LENGTH('Proveedor', 'Email') IS NULL
    ALTER TABLE Proveedor ADD Email VARCHAR(150) NULL;
IF COL_LENGTH('Proveedor', 'Direccion') IS NULL
    ALTER TABLE Proveedor ADD Direccion VARCHAR(150) NOT NULL CONSTRAINT DF_Proveedor_Direccion DEFAULT ('');
IF COL_LENGTH('Proveedor', 'Contacto') IS NULL
    ALTER TABLE Proveedor ADD Contacto VARCHAR(80) NULL;
IF COL_LENGTH('Proveedor', 'Activo') IS NULL
    ALTER TABLE Proveedor ADD Activo BIT NOT NULL CONSTRAINT DF_Proveedor_Activo DEFAULT (1);
IF COL_LENGTH('Proveedor', 'DVH') IS NULL
    ALTER TABLE Proveedor ADD DVH CHAR(64) NULL;
GO

-- 4. CUIT solo con dígitos ("30-71234567-1" → "30712345671").
UPDATE Proveedor SET CUIT = REPLACE(REPLACE(CUIT, '-', ''), ' ', '') WHERE CUIT LIKE '%[- ]%';

-- Proveedores de ejemplo del script original: dos CUIT tenían mal el dígito verificador.
UPDATE Proveedor SET CUIT = '30709876542'
 WHERE CUIT = '30709876543' AND NOT EXISTS (SELECT 1 FROM Proveedor WHERE CUIT = '30709876542');
UPDATE Proveedor SET CUIT = '30698765433'
 WHERE CUIT = '30698765439' AND NOT EXISTS (SELECT 1 FROM Proveedor WHERE CUIT = '30698765433');

UPDATE p SET Telefono = s.Telefono, Direccion = s.Direccion, Email = COALESCE(p.Email, s.Email), Contacto = COALESCE(p.Contacto, s.Contacto)
  FROM Proveedor p
  JOIN (VALUES
    ('30712345671', '1142445500', N'Av. Hipólito Yrigoyen 10250, Lomas de Zamora', 'ventas@drogueriadelsur.com.ar', N'Marcela Ríos'),
    ('30709876542', '1148763300', N'Av. Belgrano 1840, CABA', 'pedidos@pethealth.com.ar', NULL),
    ('30698765433', '2214556677', N'Calle 48 1120, La Plata', NULL, N'Jorge Salinas')
  ) AS s (CUIT, Telefono, Direccion, Email, Contacto) ON s.CUIT = p.CUIT
 WHERE p.Telefono = '' OR p.Direccion = '';
GO

-- 5. Fase 3: textos en inglés de la gestión de proveedores.
MERGE Traduccion AS t
USING (VALUES
    (2, 'FrmProveedores_60MN', N'Suppliers'),
    (2, 'FrmProveedores_60MN.lblTitulo', N'Suppliers'),
    (2, 'FrmProveedores_60MN.btnNuevo', N'New supplier'),
    (2, 'colRazonSocial', N'Company name'),
    (2, 'colCuit', N'Tax ID (CUIT)'),
    (2, 'colContacto', N'Contact'),
    (2, 'colProductosActivos', N'Active products'),
    (2, 'colEstadoProducto', N'Status'),
    (2, 'msg.CriterioRazonSocial', N'Company name'),
    (2, 'msg.ProductosQueProvee', N'Products supplied'),
    (2, 'FrmProveedor_60MN', N'Supplier'),
    (2, 'FrmProveedor_60MN.TituloAlta', N'Register supplier'),
    (2, 'FrmProveedor_60MN.TituloModificacion', N'Modify supplier'),
    (2, 'lblRazonSocial', N'Company name *'),
    (2, 'lblCuit', N'Tax ID (CUIT) *'),
    (2, 'lblContacto', N'Contact'),
    (2, 'FrmProveedor_60MN.NotaAlta', N'The CUIT is entered with or without dashes and cannot be changed afterwards.'),
    (2, 'FrmProveedor_60MN.NotaModificacion', N'The CUIT cannot be changed.')
) AS s (IdiomaID, Clave, Texto)
   ON t.IdiomaID = s.IdiomaID AND t.Clave = s.Clave
WHEN MATCHED THEN UPDATE SET Texto = s.Texto
WHEN NOT MATCHED THEN INSERT (IdiomaID, Clave, Texto) VALUES (s.IdiomaID, s.Clave, s.Texto);
GO

-- 6. Fase 4: tablas de la venta.
IF OBJECT_ID('VentaEncabezado', 'U') IS NULL
BEGIN
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
END
GO
IF OBJECT_ID('VentaDetalle', 'U') IS NULL
BEGIN
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
END
GO

-- 7. Fase 4: AtencionDetalle → VentaEncabezado / VentaDetalle (una venta por atención).
IF OBJECT_ID('AtencionDetalle', 'U') IS NOT NULL
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;

    INSERT INTO VentaEncabezado (ClienteID, AtencionID, FechaHora, UsuarioID, Total)
    SELECT m.ClienteID, a.AtencionID,
           (SELECT MIN(mv.FechaHora) FROM AtencionDetalle d2 JOIN MovimientoStock mv ON mv.MovimientoID = d2.MovimientoID
             WHERE d2.AtencionID = a.AtencionID),
           (SELECT TOP (1) mv.UsuarioID FROM AtencionDetalle d2 JOIN MovimientoStock mv ON mv.MovimientoID = d2.MovimientoID
             WHERE d2.AtencionID = a.AtencionID ORDER BY d2.AtencionDetalleID),
           (SELECT SUM(d2.Cantidad * d2.PrecioUnitario) FROM AtencionDetalle d2 WHERE d2.AtencionID = a.AtencionID)
      FROM Atencion a
      JOIN HistoriaClinica h ON h.HistoriaClinicaID = a.HistoriaClinicaID
      JOIN Mascota m ON m.MascotaID = h.MascotaID
     WHERE EXISTS (SELECT 1 FROM AtencionDetalle d WHERE d.AtencionID = a.AtencionID)
       AND NOT EXISTS (SELECT 1 FROM VentaEncabezado v WHERE v.AtencionID = a.AtencionID);

    INSERT INTO VentaDetalle (VentaID, ProductoID, Cantidad, PrecioUnitario, Subtotal, MovimientoID)
    SELECT v.VentaID, d.ProductoID, d.Cantidad, d.PrecioUnitario, d.Cantidad * d.PrecioUnitario, d.MovimientoID
      FROM AtencionDetalle d
      JOIN VentaEncabezado v ON v.AtencionID = d.AtencionID
     WHERE NOT EXISTS (SELECT 1 FROM VentaDetalle vd WHERE vd.MovimientoID = d.MovimientoID)
     ORDER BY d.AtencionDetalleID;

    DROP TABLE AtencionDetalle;
    DELETE FROM DVV WHERE Tabla = 'AtencionDetalle';

    COMMIT TRANSACTION;
END
GO

-- 8. Fase 4: el sistema no factura. Se quitan las patentes de facturación, despacho y compras.
DELETE FROM FamiliaPatente WHERE PatenteID IN (SELECT PatenteID FROM Patente WHERE Codigo IN ('VEN_FACTURAR', 'VEN_DESPACHAR', 'COM_COMPRAS'));
DELETE FROM UsuarioPatente WHERE PatenteID IN (SELECT PatenteID FROM Patente WHERE Codigo IN ('VEN_FACTURAR', 'VEN_DESPACHAR', 'COM_COMPRAS'));
DELETE FROM Patente WHERE Codigo IN ('VEN_FACTURAR', 'VEN_DESPACHAR', 'COM_COMPRAS');
DELETE FROM Traduccion WHERE Clave IN ('mnuFacturar', 'mnuDespachar', 'mnuCompras', 'mnuGestionCompras', 'mnuReporte3');
UPDATE Patente SET Nombre = 'Gestionar proveedores' WHERE Codigo = 'MAE_PROVEEDORES';
UPDATE Patente SET Nombre = 'Reportes de atenciones e inteligente' WHERE Codigo = 'REP_REPORTES';
GO

-- 9. Fase 4: textos en inglés del carrito y de la consulta de ventas.
MERGE Traduccion AS t
USING (VALUES
    (2, 'FrmCarrito_60MN.btnConfirmar', N'Confirm sale'),
    (2, 'lblCliente', N'Customer'),
    (2, 'btnElegirCliente', N'Choose customer...'),
    (2, 'msg.ElegirClienteVenta', N'Choose the customer for the sale'),
    (2, 'msg.SinClienteVenta', N'(choose the customer for the sale)'),
    (2, 'msg.VentaSinAtencion', N'(sale without consultation)'),
    (2, 'FrmVentas_60MN', N'Recorded sales'),
    (2, 'FrmVentas_60MN.lblTitulo', N'Recorded sales'),
    (2, 'FrmVentas_60MN.btnElegirCliente', N'Choose...'),
    (2, 'btnQuitarCliente', N'All'),
    (2, 'colNumero', N'Sale'),
    (2, 'colCliente', N'Customer'),
    (2, 'colAtencion', N'Consultation'),
    (2, 'colUnidades', N'Units'),
    (2, 'colTotal', N'Total $'),
    (2, 'colProducto', N'Product'),
    (2, 'lblDetalle', N'Sale detail'),
    (2, 'msg.ElegirClienteConsulta', N'Choose the customer to look up'),
    (2, 'msg.TodosLosClientes', N'(all customers)'),
    (2, 'msg.ResumenVentas', N'{0} sale(s) · {1} unit(s) · Total $ {2}'),
    (2, 'msg.DetalleVenta', N'Sale detail')
) AS s (IdiomaID, Clave, Texto)
   ON t.IdiomaID = s.IdiomaID AND t.Clave = s.Clave
WHEN MATCHED THEN UPDATE SET Texto = s.Texto
WHEN NOT MATCHED THEN INSERT (IdiomaID, Clave, Texto) VALUES (s.IdiomaID, s.Clave, s.Texto);
UPDATE Traduccion SET Texto = N'Checks the horizontal (DVH, one per row) and vertical (DVV, one per table) check digits of Usuario, Familia, Patente, their relation tables, Bitacora, Productos, MovimientoStock, Cliente, Mascota, HistoriaClinica, Atencion, Proveedor, VentaEncabezado and VentaDetalle.'
 WHERE IdiomaID = 2 AND Clave = 'lblDescripcion' AND Texto LIKE N'Checks the horizontal%';
GO

-- 10. Fase 5: textos en inglés del reporte de atenciones.
MERGE Traduccion AS t
USING (VALUES
    (2, 'FrmReporteAtenciones_60MN', N'Consultations report'),
    (2, 'FrmReporteAtenciones_60MN.grpFiltros', N'Filters'),
    (2, 'FrmReporteAtenciones_60MN.lblProfesional', N'Veterinarian'),
    (2, 'FrmReporteAtenciones_60MN.lblResumen', N'Choose the period and press Generate.'),
    (2, 'lblPorProfesional', N'Consultations by professional'),
    (2, 'colProfesionalTotal', N'Professional'),
    (2, 'colCantidadAtenciones', N'Consultations'),
    (2, 'colPacientes', N'Patients'),
    (2, 'colPorcentaje', N'% of total'),
    (2, 'msg.SinAtencionesPeriodo', N'No consultations were registered in the period with the given filters.'),
    (2, 'msg.ResumenAtenciones', N'{0} consultation(s) · {1} patient(s) · {2} professional(s)')
) AS s (IdiomaID, Clave, Texto)
   ON t.IdiomaID = s.IdiomaID AND t.Clave = s.Clave
WHEN MATCHED THEN UPDATE SET Texto = s.Texto
WHEN NOT MATCHED THEN INSERT (IdiomaID, Clave, Texto) VALUES (s.IdiomaID, s.Clave, s.Texto);
GO

-- 11. Fase 6: ayuda en línea (F1), textos en inglés de los temas.
MERGE Traduccion AS t
USING (VALUES
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
    (2, 'ayuda.reporteInteligente.texto', N'REPORTS > Smart report: crosses the demand for supplies by month and category (stock) with the products most delivered in consultations (patients) to anticipate purchases.')
) AS s (IdiomaID, Clave, Texto)
   ON t.IdiomaID = s.IdiomaID AND t.Clave = s.Clave
WHEN MATCHED THEN UPDATE SET Texto = s.Texto
WHEN NOT MATCHED THEN INSERT (IdiomaID, Clave, Texto) VALUES (s.IdiomaID, s.Clave, s.Texto);
GO

-- 12. Fase 7: patentes nuevas de las familias (por nombre de familia y código de patente).
INSERT INTO FamiliaPatente (FamiliaID, PatenteID)
SELECT f.FamiliaID, p.PatenteID
  FROM (VALUES ('Operario de stock', 'MAE_PROVEEDORES'),
               ('Veterinario', 'REP_REPORTES'),
               ('Vendedor', 'VEN_CARRITO'),
               ('Administrador', 'REP_REPORTES'),
               ('Administrador', 'MAE_PROVEEDORES')) AS s (Familia, Codigo)
  JOIN Familia f ON f.Nombre = s.Familia
  JOIN Patente p ON p.Codigo = s.Codigo
 WHERE NOT EXISTS (SELECT 1 FROM FamiliaPatente fp WHERE fp.FamiliaID = f.FamiliaID AND fp.PatenteID = p.PatenteID);
GO

PRINT 'Migración V2.4 aplicada. Recalcule los dígitos verificadores desde ADMIN > Dígitos verificadores.';
GO
