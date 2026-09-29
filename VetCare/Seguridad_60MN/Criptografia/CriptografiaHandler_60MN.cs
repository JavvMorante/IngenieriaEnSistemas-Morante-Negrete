using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Seguridad_60MN.Criptografia
{
    /// <summary>
    /// Encriptación REVERSIBLE con AES-256 (T03.2) para los datos sensibles
    /// de la base: DNI y Email de los usuarios y la descripción de la
    /// bitácora. Las contraseñas NO usan esta clase: se guardan con el hash
    /// irreversible de PasswordHasher_60MN (T03.1).
    ///
    /// La clave AES se deriva una única vez por proceso con PBKDF2 a partir
    /// de la clave maestra configurada (VETCARE_CLAVE_CRIPTO). Cada valor se
    /// cifra con un IV aleatorio, por lo que dos datos iguales producen
    /// textos cifrados distintos. Formato persistido: "iv.datos" en Base64.
    /// </summary>
    public static class CriptografiaHandler_60MN
    {
        private const int Iteraciones = 100_000;
        private const int TamanioClaveBytes = 32; // AES-256
        private const int TamanioIVBytes = 16;

        // Sal fija de derivación: no es secreta, solo separa este uso de la clave maestra de cualquier otro.
        private static readonly byte[] SalDerivacion = Encoding.UTF8.GetBytes("VetCare_60MN_AES_T03.2");

        private static readonly Lazy<byte[]> claveDerivada = new Lazy<byte[]>(() =>
            DerivarClave(Configuracion.ConfiguracionSeguridad_60MN.ObtenerClaveCriptografia()));

        public static string Encriptar(string textoPlano) => Encriptar(textoPlano, claveDerivada.Value);

        public static string Desencriptar(string textoEncriptado) => Desencriptar(textoEncriptado, claveDerivada.Value);

        public static string Encriptar(string textoPlano, byte[] clave)
        {
            if (textoPlano == null) throw new ArgumentNullException(nameof(textoPlano));

            byte[] iv = RandomNumberGenerator.GetBytes(TamanioIVBytes);
            byte[] datos = Encoding.UTF8.GetBytes(textoPlano);

            using (Aes aes = Aes.Create())
            using (ICryptoTransform encriptor = aes.CreateEncryptor(clave, iv))
            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream cripto = new CryptoStream(buffer, encriptor, CryptoStreamMode.Write))
                    cripto.Write(datos, 0, datos.Length);
                return Convert.ToBase64String(iv) + "." + Convert.ToBase64String(buffer.ToArray());
            }
        }

        public static string Desencriptar(string textoEncriptado, byte[] clave)
        {
            if (string.IsNullOrEmpty(textoEncriptado)) return textoEncriptado;

            string[] partes = textoEncriptado.Split('.');
            if (partes.Length != 2)
                throw new FormatException("El texto encriptado no tiene el formato esperado (iv.datos).");

            byte[] iv = Convert.FromBase64String(partes[0]);
            byte[] datos = Convert.FromBase64String(partes[1]);

            using (Aes aes = Aes.Create())
            using (ICryptoTransform desencriptor = aes.CreateDecryptor(clave, iv))
            using (MemoryStream buffer = new MemoryStream(datos))
            using (CryptoStream cripto = new CryptoStream(buffer, desencriptor, CryptoStreamMode.Read))
            using (StreamReader lector = new StreamReader(cripto, Encoding.UTF8))
                return lector.ReadToEnd();
        }

        public static byte[] DerivarClave(string claveMaestra)
        {
            using (Rfc2898DeriveBytes derivador = new Rfc2898DeriveBytes(claveMaestra, SalDerivacion, Iteraciones, HashAlgorithmName.SHA256))
                return derivador.GetBytes(TamanioClaveBytes);
        }
    }
}
