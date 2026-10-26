using System;
using System.Security.Cryptography;

namespace Seguridad_60MN.Criptografia
{
    /// <summary>
    /// Primitivas criptográficas comunes de la capa de seguridad, escritas sobre las
    /// clases disponibles en .NET Framework 4.8 (RNGCryptoServiceProvider vía
    /// RandomNumberGenerator.Create, SHA256.Create). Reemplazan a los atajos que solo
    /// existen en .NET 5+ (RandomNumberGenerator.GetInt32/GetBytes(int), SHA256.HashData,
    /// Convert.ToHexString y CryptographicOperations.FixedTimeEquals) con el mismo resultado,
    /// por lo que los hashes y dígitos verificadores ya guardados siguen siendo válidos.
    /// </summary>
    public static class UtilCripto_60MN
    {
        /// <summary>Bytes aleatorios criptográficamente seguros (sales, IV de AES, claves).</summary>
        public static byte[] BytesAleatorios(int cantidad)
        {
            byte[] bytes = new byte[cantidad];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Entero aleatorio seguro en [0, maximoExclusivo). Descarta los valores del
        /// último tramo incompleto para que todos los resultados sean igual de probables
        /// (sin el sesgo que produciría un simple "% maximo").
        /// </summary>
        public static int EnteroAleatorio(int maximoExclusivo)
        {
            if (maximoExclusivo <= 0) throw new ArgumentOutOfRangeException(nameof(maximoExclusivo));
            uint rango = (uint)maximoExclusivo;
            uint limite = uint.MaxValue - (uint.MaxValue % rango);
            byte[] buffer = new byte[4];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                while (true)
                {
                    rng.GetBytes(buffer);
                    uint valor = BitConverter.ToUInt32(buffer, 0);
                    if (valor < limite)
                        return (int)(valor % rango);
                }
            }
        }

        public static byte[] Sha256(byte[] datos)
        {
            using (SHA256 sha = SHA256.Create())
                return sha.ComputeHash(datos);
        }

        /// <summary>Hexadecimal en mayúsculas, sin separadores (mismo formato que Convert.ToHexString).</summary>
        public static string AHexadecimal(byte[] datos) => BitConverter.ToString(datos).Replace("-", string.Empty);

        /// <summary>
        /// Compara dos arreglos en tiempo constante: recorre siempre todos los bytes, así el
        /// tiempo de respuesta no revela cuántos coincidieron (evita ataques de temporización
        /// al verificar contraseñas y firmas).
        /// </summary>
        public static bool IgualesTiempoConstante(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diferencia = 0;
            for (int i = 0; i < a.Length; i++)
                diferencia |= a[i] ^ b[i];
            return diferencia == 0;
        }
    }
}
