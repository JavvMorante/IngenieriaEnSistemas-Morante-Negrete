using System.Globalization;
using Entidades_60MN;

namespace DAL_60MN
{
    /// <summary>Maestros auxiliares del módulo de stock: categorías, proveedores y parámetros.</summary>
    public class CatalogoStockDAL_60MN
    {
        public List<Categoria_60MN> ListarCategorias()
        {
            return Conexion_60MN.EjecutarLista("SELECT CategoriaID, Nombre, Margen FROM Categoria ORDER BY Nombre",
                r => new Categoria_60MN
                {
                    Id = r.GetInt32(0),
                    Nombre = r.GetString(1),
                    Margen = r.IsDBNull(2) ? null : r.GetDecimal(2)
                });
        }

        public List<Proveedor_60MN> ListarProveedores()
        {
            return Conexion_60MN.EjecutarLista("SELECT ProveedorID, RazonSocial, CUIT FROM Proveedor ORDER BY RazonSocial",
                r => new Proveedor_60MN { Id = r.GetInt32(0), RazonSocial = r.GetString(1), Cuit = r.GetString(2) });
        }

        /// <summary>Margen de ganancia general preconfigurado (PN4: por ejemplo 30%).</summary>
        public decimal ObtenerMargenGeneral()
        {
            object? valor = Conexion_60MN.EjecutarEscalar("SELECT Valor FROM Configuracion WHERE Clave = 'MargenGeneral'");
            return valor == null ? 30m : decimal.Parse((string)valor, CultureInfo.InvariantCulture);
        }
    }
}
