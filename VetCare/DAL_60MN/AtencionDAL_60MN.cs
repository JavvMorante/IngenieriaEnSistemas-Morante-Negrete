using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Resultado de entregar un ítem del carrito: el egreso de stock y el detalle generados.</summary>
    public class EntregaCarrito_60MN
    {
        public int ProductoId { get; set; }

        public int MovimientoId { get; set; }

        public int DetalleId { get; set; }

        public int StockResultante { get; set; }
    }

    /// <summary>Mapper de Atencion y AtencionDetalle (CUN08 a CUN10).</summary>
    public class AtencionDAL_60MN
    {
        private const string Select = @"
SELECT a.AtencionID, a.HistoriaClinicaID, a.FechaHora, a.UsuarioID, a.Profesional, a.Peso, a.Temperatura,
       a.FrecuenciaCardiaca, a.Motivo, a.Diagnostico, a.InformesEmitidos,
       h.Numero, m.Nombre AS Paciente, c.Apellido + ', ' + c.Nombre AS Propietario
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
INSERT INTO Atencion (HistoriaClinicaID, FechaHora, UsuarioID, Profesional, Peso, Temperatura, FrecuenciaCardiaca, Motivo, Diagnostico, InformesEmitidos)
VALUES (@hc, @fecha, @usuario, @profesional, @peso, @temperatura, @fc, @motivo, @diagnostico, 0);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@hc", a.HistoriaClinicaId), ("@fecha", a.FechaHora), ("@usuario", a.UsuarioId), ("@profesional", a.Profesional),
                ("@peso", a.Peso), ("@temperatura", a.Temperatura), ("@fc", a.FrecuenciaCardiaca),
                ("@motivo", a.Motivo), ("@diagnostico", a.Diagnostico)))
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

        /// <summary>Atenciones de los últimos días, para elegir a cuál asociar la medicación (CUN09 desde VENTAS > Carrito).</summary>
        public List<Atencion_60MN> ListarRecientes(int dias)
        {
            return Conexion_60MN.EjecutarLista(Select + " WHERE a.FechaHora >= @desde ORDER BY a.FechaHora DESC, a.AtencionID DESC",
                Mapear, ("@desde", DateTime.Today.AddDays(-dias)));
        }

        /// <summary>
        /// CUN09 «include» CUN14: en una sola transacción, por cada ítem descuenta el
        /// stock (bloqueando la fila del producto), registra el egreso en
        /// MovimientoStock y el detalle en AtencionDetalle. Si algún producto no
        /// alcanza, no se registra nada.
        /// </summary>
        public List<EntregaCarrito_60MN> RegistrarCarrito(int atencionId, IEnumerable<ItemCarrito_60MN> items, int? usuarioId, string motivo)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            List<EntregaCarrito_60MN> entregas = new List<EntregaCarrito_60MN>();
            DateTime ahora = DateTime.Now;

            foreach (ItemCarrito_60MN item in items)
            {
                int stockAnterior;
                using (SqlCommand leer = Conexion_60MN.Crear(
                    "SELECT StockActual FROM Productos WITH (UPDLOCK, ROWLOCK) WHERE ProductoID = @id AND Activo = 1", con, tx, ("@id", item.ProductoId)))
                {
                    object? valor = leer.ExecuteScalar();
                    if (valor == null || valor is DBNull)
                        throw new InvalidOperationException($"El producto {item.Codigo} ya no está disponible.");
                    stockAnterior = Convert.ToInt32(valor);
                }
                if (item.Cantidad > stockAnterior)
                    throw new InvalidOperationException($"Stock insuficiente de {item.Producto}: disponible {stockAnterior}, solicitado {item.Cantidad}.");

                int stockResultante = stockAnterior - item.Cantidad;
                using (SqlCommand stock = Conexion_60MN.Crear(
                    "UPDATE Productos SET StockActual = @nuevo, UsuarioModificacion = @usuario WHERE ProductoID = @id", con, tx,
                    ("@nuevo", stockResultante), ("@usuario", usuarioId ?? 0), ("@id", item.ProductoId)))
                    stock.ExecuteNonQuery();

                int movimientoId;
                using (SqlCommand mov = Conexion_60MN.Crear(@"
INSERT INTO MovimientoStock (ProductoID, Tipo, Cantidad, StockAnterior, StockResultante, Motivo, FechaHora, UsuarioID)
VALUES (@producto, 'EGRESO', @cantidad, @anterior, @resultante, @motivo, @fecha, @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                    ("@producto", item.ProductoId), ("@cantidad", -item.Cantidad), ("@anterior", stockAnterior),
                    ("@resultante", stockResultante), ("@motivo", motivo), ("@fecha", ahora), ("@usuario", usuarioId)))
                    movimientoId = Convert.ToInt32(mov.ExecuteScalar());

                int detalleId;
                using (SqlCommand det = Conexion_60MN.Crear(@"
INSERT INTO AtencionDetalle (AtencionID, ProductoID, Cantidad, PrecioUnitario, MovimientoID)
VALUES (@atencion, @producto, @cantidad, @precio, @movimiento);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                    ("@atencion", atencionId), ("@producto", item.ProductoId), ("@cantidad", item.Cantidad),
                    ("@precio", item.PrecioUnitario), ("@movimiento", movimientoId)))
                    detalleId = Convert.ToInt32(det.ExecuteScalar());

                entregas.Add(new EntregaCarrito_60MN
                {
                    ProductoId = item.ProductoId, MovimientoId = movimientoId, DetalleId = detalleId, StockResultante = stockResultante
                });
            }
            tx.Commit();
            return entregas;
        }

        /// <summary>CUN10: cuenta cada emisión del informe (la primera y las reimpresiones).</summary>
        public void IncrementarInformes(int atencionId)
        {
            Conexion_60MN.EjecutarNonQuery("UPDATE Atencion SET InformesEmitidos = InformesEmitidos + 1 WHERE AtencionID = @id", ("@id", atencionId));
        }

        private static void CargarMedicacion(List<Atencion_60MN> atenciones)
        {
            if (atenciones.Count == 0) return;
            Dictionary<int, Atencion_60MN> porId = atenciones.ToDictionary(a => a.Id);
            string ids = string.Join(",", porId.Keys); // enteros propios, no datos del usuario
            List<(int AtencionId, ItemCarrito_60MN Item)> items = Conexion_60MN.EjecutarLista($@"
SELECT d.AtencionID, d.ProductoID, p.Codigo, p.Nombre, p.Lote, d.Cantidad, d.PrecioUnitario
  FROM AtencionDetalle d JOIN Productos p ON p.ProductoID = d.ProductoID
 WHERE d.AtencionID IN ({ids}) ORDER BY d.AtencionDetalleID",
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
            Propietario = r.GetString(13)
        };
    }
}
