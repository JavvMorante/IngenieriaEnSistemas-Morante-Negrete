using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Seguridad_60MN.Configuracion;
using Seguridad_60MN.Criptografia;
using Seguridad_60MN.Integridad;

namespace Seguridad_60MN.Auditoria
{
    /// <summary>Evento leído de la bitácora, con la descripción ya desencriptada.</summary>
    public sealed class RegistroBitacora_60MN
    {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        public int? UsuarioId { get; set; }

        public string Usuario { get; set; }

        public string Modulo { get; set; }

        public string Evento { get; set; }

        public int Criticidad { get; set; }

        public string Descripcion { get; set; }
    }

    /// <summary>Filtros de CUS09. Null = no filtra por ese criterio.</summary>
    public sealed class FiltroBitacora_60MN
    {
        public DateTime? Desde { get; set; }

        public DateTime? Hasta { get; set; }

        public ModuloSistema_60MN? Modulo { get; set; }

        public EventoSistema_60MN? Evento { get; set; }

        public int? Criticidad { get; set; }

        public int? UsuarioId { get; set; }
    }

    /// <summary>
    /// Registrar Evento (T06A) y consulta de la Bitácora de Eventos (CUS09).
    /// La descripción se guarda cifrada con AES y cada inserción actualiza
    /// el DVH de la fila y el DVV de la tabla Bitacora.
    /// </summary>
    public sealed class GestorBitacora_60MN
    {
        /// <summary>
        /// Registra un evento. Nunca lanza excepción: si falla la persistencia
        /// del log la operación original no se interrumpe (CUS06, flujo 6.1),
        /// y se devuelve false para que el llamador lo sepa.
        /// </summary>
        public bool Registrar(int? usuarioId, string usuario, EventoSistema_60MN evento, string descripcion)
        {
            DefinicionEvento_60MN definicion = CatalogoEventos_60MN.Obtener(evento);
            const string sql = @"INSERT INTO Bitacora (UsuarioID, Usuario, FechaHora, Modulo, Evento, Criticidad, Descripcion)
                                 VALUES (@usuarioId, @usuario, @fecha, @modulo, @evento, @criticidad, @descripcion);
                                 SELECT CAST(SCOPE_IDENTITY() AS int);";
            try
            {
                int id;
                using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    com.Parameters.AddWithValue("@usuarioId", (object)usuarioId ?? DBNull.Value);
                    com.Parameters.AddWithValue("@usuario", usuario ?? "(sin sesión)");
                    com.Parameters.AddWithValue("@fecha", DateTime.Now);
                    com.Parameters.AddWithValue("@modulo", definicion.Modulo.ToString());
                    com.Parameters.AddWithValue("@evento", evento.ToString());
                    com.Parameters.AddWithValue("@criticidad", definicion.Criticidad);
                    com.Parameters.AddWithValue("@descripcion", CriptografiaHandler_60MN.Encriptar(descripcion ?? string.Empty));
                    id = (int)com.ExecuteScalar();
                }
                new GestorDigitoVerificador_60MN().ActualizarFila(TablaDV_60MN.Bitacora, id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<RegistroBitacora_60MN> Consultar(FiltroBitacora_60MN filtro)
        {
            StringBuilder sql = new StringBuilder(
                "SELECT BitacoraID, FechaHora, UsuarioID, Usuario, Modulo, Evento, Criticidad, Descripcion FROM Bitacora WHERE 1 = 1");
            List<SqlParameter> parametros = new List<SqlParameter>();

            void Filtrar(string condicion, string nombre, object valor)
            {
                sql.Append(" AND ").Append(condicion);
                parametros.Add(new SqlParameter(nombre, valor));
            }

            if (filtro.Desde.HasValue) Filtrar("FechaHora >= @desde", "@desde", filtro.Desde.Value.Date);
            if (filtro.Hasta.HasValue) Filtrar("FechaHora < @hasta", "@hasta", filtro.Hasta.Value.Date.AddDays(1));
            if (filtro.Modulo.HasValue) Filtrar("Modulo = @modulo", "@modulo", filtro.Modulo.Value.ToString());
            if (filtro.Evento.HasValue) Filtrar("Evento = @evento", "@evento", filtro.Evento.Value.ToString());
            if (filtro.Criticidad.HasValue) Filtrar("Criticidad = @criticidad", "@criticidad", filtro.Criticidad.Value);
            if (filtro.UsuarioId.HasValue) Filtrar("UsuarioID = @usuarioId", "@usuarioId", filtro.UsuarioId.Value);
            sql.Append(" ORDER BY FechaHora DESC, BitacoraID DESC");

            List<RegistroBitacora_60MN> resultado = new List<RegistroBitacora_60MN>();
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlCommand com = new SqlCommand(sql.ToString(), con))
            {
                com.Parameters.AddRange(parametros.ToArray());
                using (SqlDataReader lector = com.ExecuteReader())
                    while (lector.Read())
                        resultado.Add(new RegistroBitacora_60MN
                        {
                            Id = lector.GetInt32(0),
                            FechaHora = lector.GetDateTime(1),
                            UsuarioId = lector.IsDBNull(2) ? null : lector.GetInt32(2),
                            Usuario = lector.GetString(3),
                            Modulo = lector.GetString(4),
                            Evento = lector.GetString(5),
                            Criticidad = lector.GetInt32(6),
                            Descripcion = DesencriptarSeguro(lector.GetString(7))
                        });
            }
            return resultado;
        }

        private static string DesencriptarSeguro(string valor)
        {
            try
            {
                return CriptografiaHandler_60MN.Desencriptar(valor);
            }
            catch (Exception)
            {
                return "(no se pudo desencriptar)";
            }
        }
    }
}
