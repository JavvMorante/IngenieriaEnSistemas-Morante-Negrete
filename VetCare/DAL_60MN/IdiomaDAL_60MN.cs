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
                "SELECT IdiomaID, Nombre, Codigo, EsBase FROM Idioma ORDER BY IdiomaID",
                r => new Idioma_60MN
                {
                    Id = r.GetInt32(0),
                    Nombre = r.GetString(1),
                    Codigo = r.GetString(2),
                    EsBase = r.GetBoolean(3)
                });
        }

        public Dictionary<string, string> ObtenerTraducciones(int idiomaId)
        {
            return Conexion_60MN.EjecutarLista(
                    "SELECT Clave, Texto FROM Traduccion WHERE IdiomaID = @id",
                    r => (Clave: r.GetString(0), Texto: r.GetString(1)), ("@id", idiomaId))
                .ToDictionary(t => t.Clave, t => t.Texto, StringComparer.OrdinalIgnoreCase);
        }
    }
}
