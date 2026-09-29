using Entidades_60MN;
using Microsoft.Data.SqlClient;

namespace DAL_60MN
{
    /// <summary>Mapper de las tablas Idioma y Traduccion.</summary>
    public class IdiomaDAL_60MN
    {
        public List<Idioma_60MN> Listar()
        {
            return Conexion_60MN.EjecutarLista(
                "SELECT IdiomaID, Nombre, Codigo, EsBase, Predeterminado FROM Idioma ORDER BY IdiomaID",
                r => new Idioma_60MN
                {
                    Id = r.GetInt32(0),
                    Nombre = r.GetString(1),
                    Codigo = r.GetString(2),
                    EsBase = r.GetBoolean(3),
                    Predeterminado = r.GetBoolean(4)
                });
        }

        public Dictionary<string, string> ObtenerTraducciones(int idiomaId)
        {
            return Conexion_60MN.EjecutarLista(
                    "SELECT Clave, Texto FROM Traduccion WHERE IdiomaID = @id",
                    r => (Clave: r.GetString(0), Texto: r.GetString(1)), ("@id", idiomaId))
                .ToDictionary(t => t.Clave, t => t.Texto, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>Marca un único idioma como predeterminado (el que se usa al abrir el login).</summary>
        public void EstablecerPredeterminado(int idiomaId)
        {
            using SqlConnection con = Conexion_60MN.Abrir();
            using SqlTransaction tx = con.BeginTransaction();
            using (SqlCommand com = Conexion_60MN.Crear(
                "UPDATE Idioma SET Predeterminado = CASE WHEN IdiomaID = @id THEN 1 ELSE 0 END", con, tx, ("@id", idiomaId)))
                com.ExecuteNonQuery();
            tx.Commit();
        }
    }
}
