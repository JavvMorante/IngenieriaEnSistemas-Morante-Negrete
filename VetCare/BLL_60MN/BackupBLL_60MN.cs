using DAL_60MN;
using Entidades_60MN;
using Microsoft.Data.SqlClient;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>Resultado de una restauración: archivo usado e integridad de la base restaurada.</summary>
    public class ResultadoRestore_60MN
    {
        public RespaldoBD_60MN Respaldo { get; set; } = new RespaldoBD_60MN();

        /// <summary>Inconsistencias de dígitos verificadores encontradas en la base restaurada.</summary>
        public List<InconsistenciaDV_60MN> Inconsistencias { get; set; } = new List<InconsistenciaDV_60MN>();
    }

    /// <summary>CUS12 Gestión de Backup: respaldar y restaurar la base de datos.</summary>
    public class BackupBLL_60MN
    {
        private readonly BackupDAL_60MN mapper = new BackupDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Carpeta sugerida: la de backups de la instancia (el servicio de SQL Server puede escribir en ella).</summary>
        public string CarpetaPredeterminada()
        {
            string? carpeta = mapper.CarpetaPredeterminada();
            return string.IsNullOrWhiteSpace(carpeta) ? Path.GetTempPath() : carpeta;
        }

        /// <summary>Nombre del archivo: VetCareBD_60MN_aaaaMMdd_HHmmss.bak.</summary>
        public string NombreArchivo(DateTime fecha) => $"{mapper.NombreBaseDatos}_{fecha:yyyyMMdd_HHmmss}.bak";

        public string PatronNombreArchivo => $"{mapper.NombreBaseDatos}_aaaaMMdd_HHmmss.bak";

        // =====================================================================
        // Respaldar (pasos 2 a 4)
        // =====================================================================

        public RespaldoBD_60MN Respaldar(string carpeta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Backup);
            if (string.IsNullOrWhiteSpace(carpeta))
                throw new NegocioException_60MN("Indique la carpeta de destino del backup.");

            // 3.1: no se respalda una base con inconsistencias de dígitos verificadores.
            List<InconsistenciaDV_60MN> inconsistencias = dv.VerificarTodo();
            if (inconsistencias.Count > 0)
            {
                SessionManager_60MN.Instancia.IntegridadComprometida = true;
                throw new NegocioException_60MN(
                    $"La base tiene {inconsistencias.Count} inconsistencia(s) de dígitos verificadores en " +
                    string.Join(", ", inconsistencias.Select(i => i.Tabla).Distinct()) +
                    ". Revíselas en ADMIN > Dígitos verificadores antes de respaldar.");
            }

            string ruta = Path.Combine(carpeta.Trim(), NombreArchivo(DateTime.Now));
            try
            {
                mapper.Respaldar(ruta, $"VetCare - backup completo generado por {SesionActual_60MN.NombreUsuario}");
            }
            catch (SqlException ex)
            {
                // 4.1: el servicio de SQL Server no puede escribir en la carpeta.
                throw new NegocioException_60MN(
                    $"No se pudo generar el backup en \"{carpeta}\". Verifique que la carpeta exista y que el " +
                    $"servicio de SQL Server tenga permiso de escritura.\n\nDetalle: {ex.Message}");
            }

            RespaldoBD_60MN respaldo = mapper.LeerArchivo(ruta) ?? new RespaldoBD_60MN { Ruta = ruta, Fecha = DateTime.Now };
            bitacora.Registrar(EventoSistema_60MN.BackupRealizado,
                $"Backup completo de {mapper.NombreBaseDatos} en {ruta} ({respaldo.Tamanio}" +
                (respaldo.Verificado ? ", verificado con checksum)." : ")."));
            return respaldo;
        }

        // =====================================================================
        // Restaurar (pasos 5 a 9)
        // =====================================================================

        /// <summary>Respaldos anteriores registrados por SQL Server, para elegir cuál restaurar.</summary>
        public List<RespaldoBD_60MN> ListarRespaldos()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Restore);
            return mapper.ListarRespaldos();
        }

        /// <summary>8.1: el archivo debe ser un backup completo, legible y verificado de esta base.</summary>
        public RespaldoBD_60MN ValidarArchivo(string ruta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Restore);
            if (string.IsNullOrWhiteSpace(ruta))
                throw new NegocioException_60MN("Seleccione el archivo de backup a restaurar.");

            RespaldoBD_60MN? respaldo = mapper.LeerArchivo(ruta.Trim());
            if (respaldo == null)
                throw new NegocioException_60MN("El archivo no existe, no es accesible para SQL Server o no es un backup válido.");
            if (!string.Equals(respaldo.BaseDatos, mapper.NombreBaseDatos, StringComparison.OrdinalIgnoreCase))
                throw new NegocioException_60MN($"El archivo es un backup de la base \"{respaldo.BaseDatos}\", no de {mapper.NombreBaseDatos}.");
            if (!respaldo.TipoCompleto)
                throw new NegocioException_60MN("El archivo no contiene un backup completo de la base de datos.");
            if (!respaldo.Verificado)
                throw new NegocioException_60MN("El archivo de backup está dañado: falló la verificación con checksum.");
            return respaldo;
        }

        /// <summary>
        /// Paso 8: registra el inicio, valida el archivo y reemplaza la base. Paso 9:
        /// verifica los dígitos verificadores de la base restaurada y registra el
        /// resultado. La GUI cierra la sesión a continuación.
        /// </summary>
        public ResultadoRestore_60MN Restaurar(string ruta)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Restore);
            RespaldoBD_60MN respaldo = ValidarArchivo(ruta);

            // Si el restore termina bien, este evento se pierde junto con el resto de la base
            // posterior al backup; si falla, queda como evidencia del intento.
            bitacora.Registrar(EventoSistema_60MN.RestoreIniciado,
                $"Inicio de la restauración de {mapper.NombreBaseDatos} desde {respaldo.Ruta} (backup del {respaldo.Fecha:dd/MM/yyyy HH:mm}).");
            try
            {
                mapper.Restaurar(respaldo.Ruta);
            }
            catch (SqlException ex)
            {
                throw new NegocioException_60MN($"No se pudo restaurar la base de datos. La base no fue modificada.\n\nDetalle: {ex.Message}");
            }

            List<InconsistenciaDV_60MN> inconsistencias = dv.VerificarTodo();
            SessionManager_60MN.Instancia.IntegridadComprometida = inconsistencias.Count > 0;
            bitacora.Registrar(EventoSistema_60MN.RestoreRealizado,
                $"Base {mapper.NombreBaseDatos} restaurada desde {respaldo.Archivo} (backup del {respaldo.Fecha:dd/MM/yyyy HH:mm}). " +
                (inconsistencias.Count == 0
                    ? "Dígitos verificadores: base íntegra."
                    : $"Dígitos verificadores: {inconsistencias.Count} inconsistencia(s)."));
            return new ResultadoRestore_60MN { Respaldo = respaldo, Inconsistencias = inconsistencias };
        }
    }
}
