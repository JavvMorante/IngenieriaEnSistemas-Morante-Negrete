using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Mapper de PagoProveedor (CUN13).</summary>
    public class PagoProveedorDAL_60MN
    {
        public bool ExisteComprobante(string banco, string nroComprobante)
        {
            return Convert.ToInt32(Conexion_60MN.EjecutarEscalar(
                "SELECT COUNT(*) FROM PagoProveedor WHERE Banco = @banco AND NroComprobante = @nro",
                ("@banco", banco), ("@nro", nroComprobante))) > 0;
        }

        /// <summary>Bancos ya usados en pagos anteriores (para el combo editable).</summary>
        public List<string> ListarBancosUsados()
        {
            return Conexion_60MN.EjecutarLista("SELECT DISTINCT Banco FROM PagoProveedor ORDER BY Banco", r => r.GetString(0));
        }

        /// <summary>
        /// Graba el pago y pasa la orden a PAGADA en una sola transacción, solo si la orden sigue
        /// APROBADA o ENVIADA. Devuelve el Id del pago, o 0 si la orden cambió de estado.
        /// </summary>
        public int Registrar(PagoProveedor_60MN pago)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();

            using (SqlCommand orden = Conexion_60MN.Crear(
                "UPDATE OrdenCompra SET Estado = 'PAGADA' WHERE OrdenCompraID = @orden AND Estado IN ('APROBADA', 'ENVIADA')",
                con, tx, ("@orden", pago.OrdenCompraId)))
            {
                if (orden.ExecuteNonQuery() == 0)
                {
                    tx.Rollback();
                    return 0;
                }
            }

            int id;
            using (SqlCommand insertar = Conexion_60MN.Crear(@"
INSERT INTO PagoProveedor (OrdenCompraID, Banco, NroComprobante, FechaPago, Importe, Observaciones, UsuarioID)
VALUES (@orden, @banco, @nro, @fecha, @importe, @observaciones, @usuario);
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx,
                ("@orden", pago.OrdenCompraId), ("@banco", pago.Banco), ("@nro", pago.NroComprobante),
                ("@fecha", pago.FechaPago.Date), ("@importe", pago.Importe), ("@observaciones", pago.Observaciones),
                ("@usuario", pago.UsuarioId)))
                id = Convert.ToInt32(insertar.ExecuteScalar());

            tx.Commit();
            return id;
        }
    }
}
