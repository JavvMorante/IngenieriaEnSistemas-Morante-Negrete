-- ============================================================
-- Migracion para la nueva capa Seguridad_60MN.
-- HACER BACKUP DE LA BASE ANTES DE CORRER ESTO.
--
-- Pasa los digitos verificadores de "suma simple" (INT) al
-- esquema nuevo con HMAC-SHA256 (texto hexadecimal de 64
-- caracteres). Se pierden los DVH/DVV calculados hasta ahora
-- (no hay forma de migrarlos: son algoritmos distintos), pero
-- eso no es un problema: DigitoVerificadorBLL_60MN.RecalcularTodo()
-- los vuelve a calcular con la clave nueva la primera vez que
-- se ejecuta (ver README_Seguridad_60MN.md, paso 5).
-- ============================================================

ALTER TABLE Usuario          ALTER COLUMN DVH CHAR(64) NULL;
ALTER TABLE UsuarioOperacion ALTER COLUMN DVH CHAR(64) NULL;
ALTER TABLE Bitacora         ALTER COLUMN DVH CHAR(64) NULL;
ALTER TABLE PerfilUsuario    ALTER COLUMN DVH CHAR(64) NULL;
ALTER TABLE Operacion        ALTER COLUMN DVH CHAR(64) NULL;
GO

-- La tabla DVV pasa a tener el digito vertical como texto (antes
-- era INT), mas una huella (ClaveId) de la clave de firma usada,
-- para poder detectar si alguna vez se firma con una clave
-- distinta a la configurada (por ejemplo, tras restaurar un
-- backup de otro entorno) sin guardar la clave real en la base.
IF OBJECT_ID('DVV', 'U') IS NULL
BEGIN
    CREATE TABLE DVV (
        Tabla               VARCHAR(50) PRIMARY KEY,
        DVV                 CHAR(64)    NOT NULL,
        ClaveId             CHAR(64)    NOT NULL,
        FechaActualizacion  DATETIME    NOT NULL DEFAULT GETDATE()
    );
END
ELSE
BEGIN
    ALTER TABLE DVV ALTER COLUMN DVV CHAR(64) NOT NULL;
    IF COL_LENGTH('DVV', 'ClaveId') IS NULL
        ALTER TABLE DVV ADD ClaveId CHAR(64) NOT NULL DEFAULT '';
    IF COL_LENGTH('DVV', 'FechaActualizacion') IS NULL
        ALTER TABLE DVV ADD FechaActualizacion DATETIME NOT NULL DEFAULT GETDATE();
END
GO

-- Las contraseñas de usuario ahora se guardan hasheadas de forma
-- IRREVERSIBLE (SHA-256 + sal por usuario, ver PasswordHasher_60MN),
-- en vez de cifradas con el 3DES reversible de EncriptacionBLL_60MN.
-- El formato persistido es "sal.hash" en Base64 (~69 caracteres fijos).
-- Si la columna Clave es mas chica que eso, agrandarla:
-- ALTER TABLE Usuario ALTER COLUMN Clave VARCHAR(100) NOT NULL;
-- (Descomentar solo si hace falta -- depende del tamaño actual de la columna.)
--
-- Las claves ya guardadas con el 3DES anterior NO se pueden convertir
-- a hash sin conocer la contraseña en texto plano (por eso era
-- reversible y esto no). Correr una sola vez, después de migrar el
-- esquema, para resetear la clave de cada usuario existente:
--   new BLL_60MN.UsuarioBLL_60MN().ResetearTodasLasClaves();
-- Devuelve la lista (usuario, clave nueva en texto plano) para poder
-- comunicársela a cada uno -- no queda guardada en ningún lado.
