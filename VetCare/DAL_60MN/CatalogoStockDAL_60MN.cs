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

        /// <summary>Stock mínimo que se propone al registrar un producto (CUN13).</summary>
        public int ObtenerStockMinimoSugerido()
        {
            object? valor = Conexion_60MN.EjecutarEscalar("SELECT Valor FROM Configuracion WHERE Clave = 'StockMinimoSugerido'");
            return valor == null ? 5 : int.Parse((string)valor, CultureInfo.InvariantCulture);
        }

        /// <summary>CUN13: guarda el margen general, el stock mínimo sugerido y los márgenes por categoría en una transacción.</summary>
        public void GuardarConfiguracion(decimal margenGeneral, int stockMinimoSugerido, IEnumerable<Categoria_60MN> categorias)
        {
            using Microsoft.Data.SqlClient.SqlConnection con = Conexion_60MN.Abrir();
            using Microsoft.Data.SqlClient.SqlTransaction tx = con.BeginTransaction();
            void Parametro(string clave, string valor)
            {
                using var com = Conexion_60MN.Crear(@"
MERGE Configuracion AS d USING (SELECT @clave AS Clave) AS o ON d.Clave = o.Clave
WHEN MATCHED THEN UPDATE SET Valor = @valor
WHEN NOT MATCHED THEN INSERT (Clave, Valor) VALUES (@clave, @valor);", con, tx, ("@clave", clave), ("@valor", valor));
                com.ExecuteNonQuery();
            }
            Parametro("MargenGeneral", margenGeneral.ToString("0.##", CultureInfo.InvariantCulture));
            Parametro("StockMinimoSugerido", stockMinimoSugerido.ToString(CultureInfo.InvariantCulture));
            foreach (Categoria_60MN c in categorias)
            {
                using var com = Conexion_60MN.Crear("UPDATE Categoria SET Margen = @margen WHERE CategoriaID = @id", con, tx,
                    ("@margen", c.Margen), ("@id", c.Id));
                com.ExecuteNonQuery();
            }
            tx.Commit();
        }
    }
}
