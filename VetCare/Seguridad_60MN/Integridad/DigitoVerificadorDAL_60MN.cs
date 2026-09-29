using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Seguridad_60MN.Configuracion;

namespace Seguridad_60MN.Integridad
{
    /// <summary>Fila leída de una tabla protegida: valores de las columnas del DVH y el DVH guardado.</summary>
    public sealed class FilaDV_60MN
    {
        public object[] Valores { get; set; }

        public object[] ValoresClave { get; set; }

        public string DVHGuardado { get; set; }

        public string ClaveTexto => string.Join("-", ValoresClave.Select(v => Convert.ToString(v)));
    }

    /// <summary>
    /// Acceso a datos genérico para los dígitos verificadores. Los nombres de
    /// tabla y columna salen de TablaDV_60MN (constantes del sistema); todos
    /// los valores viajan como parámetros.
    /// </summary>
    public sealed class DigitoVerificadorDAL_60MN
    {
        public List<FilaDV_60MN> LeerFilas(TablaDV_60MN tabla)
        {
            string sql = $"SELECT {string.Join(", ", tabla.Columnas)}, DVH FROM {tabla.Nombre} ORDER BY {tabla.OrdenPorClave}";
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlCommand com = new SqlCommand(sql, con))
                return Leer(com, tabla);
        }

        public FilaDV_60MN LeerFila(TablaDV_60MN tabla, object[] clave)
        {
            string sql = $"SELECT {string.Join(", ", tabla.Columnas)}, DVH FROM {tabla.Nombre} WHERE {CondicionClave(tabla)}";
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                AgregarParametrosClave(com, tabla, clave);
                return Leer(com, tabla).FirstOrDefault();
            }
        }

        public void ActualizarDVH(TablaDV_60MN tabla, object[] clave, string dvh)
        {
            string sql = $"UPDATE {tabla.Nombre} SET DVH = @dvh WHERE {CondicionClave(tabla)}";
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                com.Parameters.AddWithValue("@dvh", dvh);
                AgregarParametrosClave(com, tabla, clave);
                com.ExecuteNonQuery();
            }
        }

        public List<string> LeerDVHs(TablaDV_60MN tabla)
        {
            string sql = $"SELECT DVH FROM {tabla.Nombre} ORDER BY {tabla.OrdenPorClave}";
            List<string> resultado = new List<string>();
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlCommand com = new SqlCommand(sql, con))
            using (SqlDataReader lector = com.ExecuteReader())
                while (lector.Read())
                    resultado.Add(lector.IsDBNull(0) ? null : lector.GetString(0));
            return resultado;
        }

        public void GuardarDVV(string tabla, string dvv)
        {
            const string sql = @"
MERGE DVV AS destino
USING (SELECT @tabla AS Tabla) AS origen ON destino.Tabla = origen.Tabla
WHEN MATCHED THEN UPDATE SET DVV = @dvv
WHEN NOT MATCHED THEN INSERT (Tabla, DVV) VALUES (@tabla, @dvv);";
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlCommand com = new SqlCommand(sql, con))
            {
                com.Parameters.AddWithValue("@tabla", tabla);
                com.Parameters.AddWithValue("@dvv", dvv);
                com.ExecuteNonQuery();
            }
        }

        public string LeerDVV(string tabla)
        {
            using (SqlConnection con = ConexionSeguridad_60MN.Abrir())
            using (SqlCommand com = new SqlCommand("SELECT DVV FROM DVV WHERE Tabla = @tabla", con))
            {
                com.Parameters.AddWithValue("@tabla", tabla);
                object valor = com.ExecuteScalar();
                return valor == null || valor is DBNull ? null : (string)valor;
            }
        }

        private static List<FilaDV_60MN> Leer(SqlCommand com, TablaDV_60MN tabla)
        {
            List<FilaDV_60MN> filas = new List<FilaDV_60MN>();
            int cantidadColumnas = tabla.Columnas.Length;
            using (SqlDataReader lector = com.ExecuteReader())
            {
                while (lector.Read())
                {
                    object[] valores = new object[cantidadColumnas];
                    for (int i = 0; i < cantidadColumnas; i++)
                        valores[i] = lector.IsDBNull(i) ? null : lector.GetValue(i);
                    filas.Add(new FilaDV_60MN
                    {
                        Valores = valores,
                        ValoresClave = valores.Take(tabla.ClavePrimaria.Length).ToArray(),
                        DVHGuardado = lector.IsDBNull(cantidadColumnas) ? null : lector.GetString(cantidadColumnas)
                    });
                }
            }
            return filas;
        }

        private static string CondicionClave(TablaDV_60MN tabla) =>
            string.Join(" AND ", tabla.ClavePrimaria.Select((c, i) => $"{c} = @k{i}"));

        private static void AgregarParametrosClave(SqlCommand com, TablaDV_60MN tabla, object[] clave)
        {
            if (clave.Length != tabla.ClavePrimaria.Length)
                throw new ArgumentException($"La tabla {tabla.Nombre} requiere {tabla.ClavePrimaria.Length} valor(es) de clave.");
            for (int i = 0; i < clave.Length; i++)
                com.Parameters.AddWithValue("@k" + i, clave[i]);
        }
    }
}
