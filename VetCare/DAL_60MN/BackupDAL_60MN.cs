using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>
    /// CUS12 Gestión de Backup: respaldo completo y restauración de VetCareBD_60MN con
    /// los comandos BACKUP / RESTORE de SQL Server. El archivo lo escribe y lee el
    /// servicio de SQL Server, por eso la carpeta debe ser accesible para ese servicio.
    /// El nombre de la base no puede ir como parámetro en BACKUP/RESTORE: se arma con
    /// QUOTENAME dentro de sp_executesql y la ruta sí viaja como parámetro.
    /// </summary>
    public class BackupDAL_60MN
    {
        public string NombreBaseDatos => Conexion_60MN.NombreBaseDatos;

        /// <summary>Carpeta de backups predeterminada de la instancia de SQL Server.</summary>
        public string? CarpetaPredeterminada() =>
            Conexion_60MN.EjecutarEscalar("SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS NVARCHAR(4000))") as string;

        /// <summary>Backup completo, verificado con checksum, que reemplaza el archivo si existiera.</summary>
        public void Respaldar(string ruta, string descripcion)
        {
            const string sql = @"
DECLARE @sql NVARCHAR(MAX) = N'BACKUP DATABASE ' + QUOTENAME(@bd) +
    N' TO DISK = @ruta WITH FORMAT, INIT, CHECKSUM, NAME = @nombre, DESCRIPTION = @descripcion';
EXEC sp_executesql @sql, N'@ruta NVARCHAR(4000), @nombre NVARCHAR(128), @descripcion NVARCHAR(255)',
     @ruta = @ruta, @nombre = @nombre, @descripcion = @descripcion;";
            using SqlConnection con = Conexion_60MN.AbrirMaster();
            using SqlCommand com = Conexion_60MN.Crear(sql, con, null,
                ("@bd", NombreBaseDatos), ("@ruta", ruta), ("@nombre", Path.GetFileName(ruta)), ("@descripcion", descripcion));
            com.CommandTimeout = 0;
            com.ExecuteNonQuery();
        }

        /// <summary>Respaldos completos de la base registrados por SQL Server en msdb (sobreviven a un restore).</summary>
        public List<RespaldoBD_60MN> ListarRespaldos(int cantidad = 30)
        {
            const string sql = @"
SELECT TOP (@cantidad) m.physical_device_name AS Ruta, b.backup_finish_date AS Fecha,
       CAST(b.backup_size AS BIGINT) AS Tamanio, b.user_name AS Usuario, b.description AS Descripcion
FROM msdb.dbo.backupset b
JOIN msdb.dbo.backupmediafamily m ON m.media_set_id = b.media_set_id
WHERE b.database_name = @bd AND b.type = 'D'
ORDER BY b.backup_finish_date DESC";
            return Conexion_60MN.EjecutarLista(sql, l => new RespaldoBD_60MN
            {
                Ruta = l.GetString(l.GetOrdinal("Ruta")),
                Fecha = l.GetDateTime(l.GetOrdinal("Fecha")),
                TamanioBytes = l.GetInt64(l.GetOrdinal("Tamanio")),
                Usuario = l.TextoONulo("Usuario") ?? string.Empty,
                Descripcion = l.TextoONulo("Descripcion") ?? string.Empty
            }, ("@cantidad", cantidad), ("@bd", NombreBaseDatos));
        }

        /// <summary>
        /// Lee la cabecera del archivo (RESTORE HEADERONLY) y lo verifica con checksum
        /// (RESTORE VERIFYONLY). Devuelve null si el archivo no es un backup legible.
        /// </summary>
        public RespaldoBD_60MN? LeerArchivo(string ruta)
        {
            using SqlConnection con = Conexion_60MN.AbrirMaster();
            RespaldoBD_60MN? respaldo = null;
            try
            {
                using (SqlCommand cabecera = Conexion_60MN.Crear("RESTORE HEADERONLY FROM DISK = @ruta", con, null, ("@ruta", ruta)))
                using (SqlDataReader l = cabecera.ExecuteReader())
                {
                    if (l.Read())
                        respaldo = new RespaldoBD_60MN
                        {
                            Ruta = ruta,
                            BaseDatos = l["DatabaseName"] as string ?? string.Empty,
                            Fecha = l["BackupFinishDate"] is DateTime fecha ? fecha : DateTime.MinValue,
                            TamanioBytes = l["BackupSize"] is decimal tam ? (long)tam : Convert.ToInt64(l["BackupSize"]),
                            Usuario = l["UserName"] as string ?? string.Empty,
                            Descripcion = l["BackupDescription"] as string ?? string.Empty,
                            TipoCompleto = Convert.ToInt32(l["BackupType"]) == 1
                        };
                }
                if (respaldo == null) return null;

                using SqlCommand verificar = Conexion_60MN.Crear("RESTORE VERIFYONLY FROM DISK = @ruta WITH CHECKSUM", con, null, ("@ruta", ruta));
                verificar.CommandTimeout = 0;
                verificar.ExecuteNonQuery();
                respaldo.Verificado = true;
                return respaldo;
            }
            catch (SqlException)
            {
                // Archivo inexistente, ilegible, dañado o que no es un backup de SQL Server.
                return respaldo;
            }
        }

        /// <summary>
        /// Reemplaza la base con el contenido del archivo: la pasa a usuario único
        /// (desconectando las demás sesiones), restaura y la vuelve a multiusuario.
        /// </summary>
        public void Restaurar(string ruta)
        {
            const string sql = @"
DECLARE @nombre NVARCHAR(260) = QUOTENAME(@bd);
DECLARE @sql NVARCHAR(MAX) = N'ALTER DATABASE ' + @nombre + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE';
EXEC (@sql);
BEGIN TRY
    SET @sql = N'RESTORE DATABASE ' + @nombre + N' FROM DISK = @ruta WITH REPLACE, CHECKSUM';
    EXEC sp_executesql @sql, N'@ruta NVARCHAR(4000)', @ruta = @ruta;
    SET @sql = N'ALTER DATABASE ' + @nombre + N' SET MULTI_USER';
    EXEC (@sql);
END TRY
BEGIN CATCH
    SET @sql = N'ALTER DATABASE ' + @nombre + N' SET MULTI_USER';
    EXEC (@sql);
    THROW;
END CATCH";
            // Las conexiones del pool apuntan a la base que se va a reemplazar.
            SqlConnection.ClearAllPools();
            try
            {
                using SqlConnection con = Conexion_60MN.AbrirMaster();
                using SqlCommand com = Conexion_60MN.Crear(sql, con, null, ("@bd", NombreBaseDatos), ("@ruta", ruta));
                com.CommandTimeout = 0;
                com.ExecuteNonQuery();
            }
            finally
            {
                // Las conexiones que quedaron en el pool fueron cortadas por ROLLBACK IMMEDIATE.
                SqlConnection.ClearAllPools();
            }
        }
    }
}
