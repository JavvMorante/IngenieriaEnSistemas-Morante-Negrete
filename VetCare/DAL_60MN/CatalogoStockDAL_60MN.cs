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

        /// <summary>
        /// Proveedores para los combos de stock. Con soloActivos (alta y modificación de productos)
        /// se omiten los dados de baja, salvo ademasId: el proveedor actual del producto que se modifica.
        /// </summary>
        public List<Proveedor_60MN> ListarProveedores(bool soloActivos, int? ademasId = null)
        {
            return Conexion_60MN.EjecutarLista(@"
SELECT ProveedorID, RazonSocial, CUIT, Activo FROM Proveedor
 WHERE @todos = 1 OR Activo = 1 OR ProveedorID = @ademas
 ORDER BY RazonSocial",
                r => new Proveedor_60MN { Id = r.GetInt32(0), RazonSocial = r.GetString(1), Cuit = r.GetString(2), Activo = r.GetBoolean(3) },
                ("@todos", !soloActivos), ("@ademas", ademasId));
        }

        /// <summary>Margen general (%) que se propone en el alta de un producto si su categoría no tiene margen propio.</summary>
        public decimal ObtenerMargenGeneral()
        {
            object? valor = Conexion_60MN.EjecutarEscalar("SELECT Valor FROM Configuracion WHERE Clave = 'MargenGeneral'");
            return valor == null ? 30m : decimal.Parse((string)valor, CultureInfo.InvariantCulture);
        }

        /// <summary>Stock mínimo que se propone al registrar un producto (CUN09).</summary>
        public int ObtenerStockMinimoSugerido()
        {
            object? valor = Conexion_60MN.EjecutarEscalar("SELECT Valor FROM Configuracion WHERE Clave = 'StockMinimoSugerido'");
            return valor == null ? 5 : int.Parse((string)valor, CultureInfo.InvariantCulture);
        }

        /// <summary>CUN12: tope mensual de compras aprobadas, o null si la clave TopeCompraMensual no existe.</summary>
        public decimal? ObtenerTopeCompraMensual()
        {
            object? valor = Conexion_60MN.EjecutarEscalar("SELECT Valor FROM Configuracion WHERE Clave = 'TopeCompraMensual'");
            return valor == null ? null : decimal.Parse((string)valor, CultureInfo.InvariantCulture);
        }
    }
}
