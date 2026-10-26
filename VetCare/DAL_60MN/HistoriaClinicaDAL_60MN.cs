using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Mapper de HistoriaClinica e HistoriaClinica_H (CUN05 a CUN07).</summary>
    public class HistoriaClinicaDAL_60MN
    {
        private const string Select = @"
SELECT h.HistoriaClinicaID, h.Numero, h.MascotaID, h.FechaApertura, h.PesoActual, h.Alergias, h.Observaciones
  FROM HistoriaClinica h";

        public HistoriaClinica_60MN? ObtenerPorMascota(int mascotaId) =>
            Conexion_60MN.EjecutarLista(Select + " WHERE h.MascotaID = @id", Mapear, ("@id", mascotaId)).FirstOrDefault();

        public HistoriaClinica_60MN? ObtenerPorId(int id) =>
            Conexion_60MN.EjecutarLista(Select + " WHERE h.HistoriaClinicaID = @id", Mapear, ("@id", id)).FirstOrDefault();

        /// <summary>CUN05: abre la historia clínica. Devuelve su Id (el número HC-xxxxx lo genera la base).</summary>
        public int Insertar(HistoriaClinica_60MN hc, int? usuarioId)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(@"
INSERT INTO HistoriaClinica (MascotaID, FechaApertura, PesoActual, Alergias, Observaciones, UsuarioID)
VALUES (@mascota, @fecha, @peso, @alergias, @observaciones, @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                ("@mascota", hc.MascotaId), ("@fecha", hc.FechaApertura), ("@peso", hc.PesoActual),
                ("@alergias", hc.Alergias), ("@observaciones", hc.Observaciones), ("@usuario", usuarioId)));
        }

        /// <summary>
        /// CUN07: guarda los valores ANTERIORES en HistoriaClinica_H y actualiza la
        /// historia, en una sola transacción (trazabilidad del paso 6).
        /// </summary>
        public void Actualizar(HistoriaClinica_60MN hc, string origen, int? usuarioId)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            GuardarValoresAnteriores(con, tx, hc.Id, origen, usuarioId);
            using (SqlCommand com = Conexion_60MN.Crear(
                "UPDATE HistoriaClinica SET PesoActual = @peso, Alergias = @alergias, Observaciones = @observaciones WHERE HistoriaClinicaID = @id",
                con, tx, ("@peso", hc.PesoActual), ("@alergias", hc.Alergias), ("@observaciones", hc.Observaciones), ("@id", hc.Id)))
                com.ExecuteNonQuery();
            tx.Commit();
        }

        public List<HistoriaClinicaHistorial_60MN> ListarHistorial(int historiaClinicaId)
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT h.FechaHora, h.PesoActual, h.Alergias, h.Observaciones, h.Origen, COALESCE(u.Usuario, '(emergencia)')
  FROM HistoriaClinica_H h
  LEFT JOIN Usuario u ON u.UsuarioID = h.UsuarioID
 WHERE h.HistoriaClinicaID = @id
 ORDER BY h.FechaHora DESC, h.HistoriaClinicaHID DESC",
                r => new HistoriaClinicaHistorial_60MN
                {
                    FechaHora = r.GetDateTime(0),
                    PesoActual = r.GetDecimal(1),
                    Alergias = r.GetString(2),
                    Observaciones = r.IsDBNull(3) ? null : r.GetString(3),
                    Origen = r.GetString(4),
                    Usuario = r.GetString(5)
                }, ("@id", historiaClinicaId));
        }

        /// <summary>Copia el estado actual de la historia a HistoriaClinica_H antes de modificarla.</summary>
        internal static void GuardarValoresAnteriores(SqlConnection con, SqlTransaction tx, int historiaClinicaId, string origen, int? usuarioId)
        {
            using SqlCommand com = Conexion_60MN.Crear(@"
INSERT INTO HistoriaClinica_H (HistoriaClinicaID, PesoActual, Alergias, Observaciones, FechaHora, UsuarioID, Origen)
SELECT HistoriaClinicaID, PesoActual, Alergias, Observaciones, @fecha, @usuario, @origen
  FROM HistoriaClinica WHERE HistoriaClinicaID = @id", con, tx,
                ("@fecha", DateTime.Now), ("@usuario", usuarioId), ("@origen", origen), ("@id", historiaClinicaId));
            com.ExecuteNonQuery();
        }

        private static HistoriaClinica_60MN Mapear(SqlDataReader r) => new HistoriaClinica_60MN
        {
            Id = r.GetInt32(0),
            Numero = r.GetString(1),
            MascotaId = r.GetInt32(2),
            FechaApertura = r.GetDateTime(3),
            PesoActual = r.GetDecimal(4),
            Alergias = r.GetString(5),
            Observaciones = r.IsDBNull(6) ? null : r.GetString(6)
        };
    }
}
