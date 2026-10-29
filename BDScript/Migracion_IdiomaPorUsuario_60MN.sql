/* =====================================================================
   VetCare - Migración de una base EXISTENTE: idioma por usuario.
   (Una instalación nueva no la necesita: VetCareBD_60MN.sql ya trae este esquema.)

   1. Agrega Usuario.IdiomaID (último idioma elegido por cada usuario, NULL = español).
   2. Elimina Idioma.Predeterminado (ya no hay un idioma predeterminado del sistema).
   3. Actualiza las traducciones de los textos que hablaban del "predeterminado".
   4. Crea el trigger que impide ciclos entre familias (TR_FamiliaFamilia_SinCiclos), si falta.
   5. Actualiza los textos en inglés de la pantalla de Backup (backup y restore en dos pasos).
   6. Agrega las traducciones al inglés de los textos que arma el código y del Reporte inteligente.
   7. Agrega Atencion.Indicaciones (indicaciones o tratamiento, opcional).
   8. Reactiva el trigger TR_Productos_BitacoraCambios (Gestión de Stock habilitada: guarda
      las versiones de cada producto en Productos_C; no forma parte de los dígitos verificadores).

   IMPORTANTE - dígitos verificadores: IdiomaID forma parte del DVH de Usuario e Indicaciones
   del de Atencion, así que después de agregarlas los DVH de esas tablas quedan desactualizados y hay que
   recalcularlos con la clave de la aplicación (SQL Server no la conoce):
     - ingresar a VetCare (si la integridad bloquea a todos los administradores, se
       habilita el usuario de emergencia) y usar ADMIN > Dígitos verificadores >
       Recalcular dígitos.
   Se puede ejecutar más de una vez: cada paso verifica si ya está aplicado.
   ===================================================================== */
USE VetCareBD_60MN;
GO

IF COL_LENGTH('Usuario', 'IdiomaID') IS NULL
    ALTER TABLE Usuario ADD IdiomaID INT NULL CONSTRAINT FK_Usuario_Idioma REFERENCES Idioma (IdiomaID);
GO

-- Atención: indicaciones o tratamiento, opcionales (la atención puede cerrarse sin medicación).
-- Forma parte del DVH de Atencion: después de agregarla hay que recalcular los dígitos.
IF COL_LENGTH('Atencion', 'Indicaciones') IS NULL
    ALTER TABLE Atencion ADD Indicaciones VARCHAR(1000) NULL;
GO

IF COL_LENGTH('Idioma', 'Predeterminado') IS NOT NULL
    ALTER TABLE Idioma DROP COLUMN Predeterminado;
GO

UPDATE Traduccion SET Texto = N'Language changed. It was saved as your language and will be applied every time you sign in.'
 WHERE IdiomaID = 2 AND Clave = 'msg.IdiomaGuardado';
UPDATE Traduccion SET Texto = N'The selected language is saved as yours: it will be applied every time you sign in.'
 WHERE IdiomaID = 2 AND Clave = 'FrmCambiarIdioma_60MN.lblNota';
GO

IF OBJECT_ID('TR_FamiliaFamilia_SinCiclos', 'TR') IS NULL
    EXEC (N'
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
        THROW 50001, N''La relación entre familias generaría un ciclo (una familia no puede contenerse a sí misma, ni directa ni indirectamente).'', 1;
    END
END');
GO

-- Textos en inglés de la pantalla de Backup (backup y restore en dos pasos, cualquier carpeta).
UPDATE Traduccion SET Texto = N'You can choose any folder where you have write permission (a USB drive too). SQL Server creates the backup in its own folder and VetCare copies it to the chosen one, verifying that the copy is identical.' WHERE IdiomaID = 2 AND Clave = 'lblNotaCarpeta';
UPDATE Traduccion SET Texto = N'Available backups (or choose a .bak file from any folder with Browse...)' WHERE IdiomaID = 2 AND Clave = 'lblRespaldos';
UPDATE Traduccion SET Texto = N'Restoring replaces the whole database: changes made after the backup (including the event log) are lost and the session is closed at the end. Only VetCareBD_60MN backups are accepted.' WHERE IdiomaID = 2 AND Clave = 'lblAdvertencia';
GO

-- Traducciones al inglés de los textos que arma el código y del Reporte inteligente (insertar o actualizar).
MERGE Traduccion AS t
USING (VALUES
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
    (2, 'msg.ResumenReporteInteligente', N'{0} unit(s) issued · {1} unit(s) delivered in consultations · $ {2}'),
    (2, 'mnuReporte2', N'Smart report'),
    (2, 'lblDiagnostico', N'Diagnosis *'),
    (2, 'lblIndicaciones', N'Instructions / treatment (optional: what to buy, dose, frequency)'),
    (2, 'msg.Indicaciones', N'Instructions'),
    (2, 'msg.SinIndicaciones', N'(no instructions)')
) AS s (IdiomaID, Clave, Texto)
   ON t.IdiomaID = s.IdiomaID AND t.Clave = s.Clave
WHEN MATCHED THEN UPDATE SET Texto = s.Texto
WHEN NOT MATCHED THEN INSERT (IdiomaID, Clave, Texto) VALUES (s.IdiomaID, s.Clave, s.Texto);
GO

-- Gestión de Stock habilitada: el trigger vuelve a registrar las versiones de los productos.
IF EXISTS (SELECT 1 FROM sys.triggers WHERE name = 'TR_Productos_BitacoraCambios' AND is_disabled = 1)
    ENABLE TRIGGER TR_Productos_BitacoraCambios ON Productos;
GO

PRINT 'Migración aplicada. Recalcule los dígitos verificadores desde ADMIN > Dígitos verificadores.';
GO
