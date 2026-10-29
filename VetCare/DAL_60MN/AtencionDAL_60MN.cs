using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Mapper de Atencion (CUN04 y CUN06); la medicación entregada se lee de las ventas (CUN05).</summary>
    public class AtencionDAL_60MN
    {
        private const string Select = @"
SELECT a.AtencionID, a.HistoriaClinicaID, a.FechaHora, a.UsuarioID, a.Profesional, a.Peso, a.Temperatura,
       a.FrecuenciaCardiaca, a.Motivo, a.Diagnostico, a.InformesEmitidos,
       h.Numero, m.Nombre AS Paciente, c.Apellido + ', ' + c.Nombre AS Propietario, a.Indicaciones, c.ClienteID
  FROM Atencion a
  JOIN HistoriaClinica h ON h.HistoriaClinicaID = a.HistoriaClinicaID
  JOIN Mascota m ON m.MascotaID = h.MascotaID
  JOIN Cliente c ON c.ClienteID = m.ClienteID";

        /// <summary>
        /// CUN08: registra la atención y actualiza el peso del legajo. Si el peso
        /// cambia, el valor anterior se guarda en HistoriaClinica_H (origen ATENCION).
        /// </summary>
        public int Insertar(Atencion_60MN a)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            int id;
            using (SqlCommand com = Conexion_60MN.Crear(@"
INSERT INTO Atencion (HistoriaClinicaID, FechaHora, UsuarioID, Profesional, Peso, Temperatura, FrecuenciaCardiaca, Motivo, Diagnostico, InformesEmitidos, Indicaciones)
VALUES (@hc, @fecha, @usuario, @profesional, @peso, @temperatura, @fc, @motivo, @diagnostico, 0, @indicaciones);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@hc", a.HistoriaClinicaId), ("@fecha", a.FechaHora), ("@usuario", a.UsuarioId), ("@profesional", a.Profesional),
                ("@peso", a.Peso), ("@temperatura", a.Temperatura), ("@fc", a.FrecuenciaCardiaca),
                ("@motivo", a.Motivo), ("@diagnostico", a.Diagnostico), ("@indicaciones", a.Indicaciones)))
                id = Convert.ToInt32(com.ExecuteScalar());

            decimal pesoAnterior;
            using (SqlCommand leer = Conexion_60MN.Crear("SELECT PesoActual FROM HistoriaClinica WHERE HistoriaClinicaID = @hc", con, tx, ("@hc", a.HistoriaClinicaId)))
                pesoAnterior = Convert.ToDecimal(leer.ExecuteScalar());
            if (pesoAnterior != a.Peso)
            {
                HistoriaClinicaDAL_60MN.GuardarValoresAnteriores(con, tx, a.HistoriaClinicaId, "ATENCION", a.UsuarioId);
                using SqlCommand peso = Conexion_60MN.Crear("UPDATE HistoriaClinica SET PesoActual = @peso WHERE HistoriaClinicaID = @hc",
                    con, tx, ("@peso", a.Peso), ("@hc", a.HistoriaClinicaId));
                peso.ExecuteNonQuery();
            }
            tx.Commit();
            return id;
        }

        public List<Atencion_60MN> ListarPorHistoria(int historiaClinicaId)
        {
            List<Atencion_60MN> atenciones = Conexion_60MN.EjecutarLista(
                Select + " WHERE a.HistoriaClinicaID = @hc ORDER BY a.FechaHora DESC, a.AtencionID DESC", Mapear, ("@hc", historiaClinicaId));
            CargarMedicacion(atenciones);
            return atenciones;
        }

        public Atencion_60MN? ObtenerPorId(int id)
        {
            List<Atencion_60MN> atenciones = Conexion_60MN.EjecutarLista(Select + " WHERE a.AtencionID = @id", Mapear, ("@id", id));
            CargarMedicacion(atenciones);
            return atenciones.FirstOrDefault();
        }

        /// <summary>
        /// Atenciones de los últimos días de las mascotas del cliente, para elegir a cuál
        /// asociar la medicación (CUN05 desde VENTAS > Carrito).
        /// </summary>
        public List<Atencion_60MN> ListarRecientes(int dias, int clienteId)
        {
            return Conexion_60MN.EjecutarLista(Select + " WHERE a.FechaHora >= @desde AND c.ClienteID = @cliente ORDER BY a.FechaHora DESC, a.AtencionID DESC",
                Mapear, ("@desde", DateTime.Today.AddDays(-dias)), ("@cliente", clienteId));
        }

        /// <summary>
        /// CUN07 Generar Reporte de Atenciones: atenciones del período [desde, hasta), opcionalmente
        /// de un profesional (usuario que la registró) y de una especie, en orden cronológico.
        /// </summary>
        public List<FilaReporteAtencion_60MN> ListarPorPeriodo(DateTime desde, DateTime hasta, int? usuarioId, string? especie)
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT a.AtencionID, a.FechaHora, h.Numero, m.Nombre, m.Especie, c.Apellido + ', ' + c.Nombre AS Propietario,
       a.UsuarioID, COALESCE(NULLIF(a.Profesional, ''), u.Usuario, '-') AS Profesional, a.Motivo
  FROM Atencion a
  JOIN HistoriaClinica h ON h.HistoriaClinicaID = a.HistoriaClinicaID
  JOIN Mascota m ON m.MascotaID = h.MascotaID
  JOIN Cliente c ON c.ClienteID = m.ClienteID
  LEFT JOIN Usuario u ON u.UsuarioID = a.UsuarioID
 WHERE a.FechaHora >= @desde AND a.FechaHora < @hasta
   AND (@usuario IS NULL OR a.UsuarioID = @usuario)
   AND (@especie IS NULL OR m.Especie = @especie)
 ORDER BY a.FechaHora, a.AtencionID", r => new FilaReporteAtencion_60MN
            {
                AtencionId = r.GetInt32(0),
                FechaHora = r.GetDateTime(1),
                NumeroHistoriaClinica = r.GetString(2),
                Paciente = r.GetString(3),
                Especie = r.GetString(4),
                Propietario = r.GetString(5),
                UsuarioId = r.IsDBNull(6) ? null : r.GetInt32(6),
                Profesional = r.GetString(7),
                Motivo = r.GetString(8)
            }, ("@desde", desde), ("@hasta", hasta), ("@usuario", usuarioId), ("@especie", especie));
        }

        /// <summary>Profesionales que registraron atenciones (filtro del reporte), con el nombre de su última atención.</summary>
        public List<ProfesionalAtencion_60MN> ListarProfesionales()
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT a.UsuarioID, MAX(a.Profesional) AS Profesional
  FROM Atencion a
 WHERE a.UsuarioID IS NOT NULL
 GROUP BY a.UsuarioID
 ORDER BY MAX(a.Profesional)", r => new ProfesionalAtencion_60MN { UsuarioId = r.GetInt32(0), Profesional = r.GetString(1) });
        }

        /// <summary>Especies de las mascotas registradas (filtro del reporte).</summary>
        public List<string> ListarEspecies()
        {
            return Conexion_60MN.EjecutarLista("SELECT DISTINCT Especie FROM Mascota ORDER BY Especie", r => r.GetString(0));
        }

        /// <summary>CUN10: cuenta cada emisión del informe (la primera y las reimpresiones).</summary>
        public void IncrementarInformes(int atencionId)
        {
            Conexion_60MN.EjecutarNonQuery("UPDATE Atencion SET InformesEmitidos = InformesEmitidos + 1 WHERE AtencionID = @id", ("@id", atencionId));
        }

        /// <summary>La medicación de cada atención: los ítems de las ventas asociadas a ella (CUN05).</summary>
        private static void CargarMedicacion(List<Atencion_60MN> atenciones)
        {
            if (atenciones.Count == 0) return;
            Dictionary<int, Atencion_60MN> porId = atenciones.ToDictionary(a => a.Id);
            string ids = string.Join(",", porId.Keys); // enteros propios, no datos del usuario
            List<(int AtencionId, ItemCarrito_60MN Item)> items = Conexion_60MN.EjecutarLista($@"
SELECT v.AtencionID, d.ProductoID, p.Codigo, p.Nombre, p.Lote, d.Cantidad, d.PrecioUnitario
  FROM VentaDetalle d
  JOIN VentaEncabezado v ON v.VentaID = d.VentaID
  JOIN Productos p ON p.ProductoID = d.ProductoID
 WHERE v.AtencionID IN ({ids}) ORDER BY d.VentaDetalleID",
                r => (r.GetInt32(0), new ItemCarrito_60MN
                {
                    ProductoId = r.GetInt32(1), Codigo = r.GetString(2), Producto = r.GetString(3), Lote = r.GetString(4),
                    Cantidad = r.GetInt32(5), PrecioUnitario = r.GetDecimal(6)
                }));
            foreach ((int atencionId, ItemCarrito_60MN item) in items)
                porId[atencionId].Medicacion.Add(item);
        }

        private static Atencion_60MN Mapear(SqlDataReader r) => new Atencion_60MN
        {
            Id = r.GetInt32(0),
            HistoriaClinicaId = r.GetInt32(1),
            FechaHora = r.GetDateTime(2),
            UsuarioId = r.IsDBNull(3) ? null : r.GetInt32(3),
            Profesional = r.GetString(4),
            Peso = r.GetDecimal(5),
            Temperatura = r.GetDecimal(6),
            FrecuenciaCardiaca = r.GetInt32(7),
            Motivo = r.GetString(8),
            Diagnostico = r.GetString(9),
            InformesEmitidos = r.GetInt32(10),
            NumeroHistoriaClinica = r.GetString(11),
            Paciente = r.GetString(12),
            Propietario = r.GetString(13),
            Indicaciones = r.IsDBNull(14) ? null : r.GetString(14),
            ClienteId = r.GetInt32(15)
        };
    }
}
