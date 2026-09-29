using System;
using System.Security.Cryptography;
using System.Text;

namespace Seguridad_60MN.Criptografia
{
    /// <summary>
    /// Hashing IRREVERSIBLE de contrasenias de usuario (SHA-256 + sal aleatoria
    /// por usuario). A diferencia de CriptografiaHandler_60MN (cifrado
    /// reversible, pensado para datos que la aplicacion necesita volver a leer,
    /// como el texto de la bitacora), esta clase no tiene metodo de "deshash":
    /// una vez guardado el resultado de Hashear(), la contrasenia original no
    /// se puede recuperar. Solo se puede volver a verificar un intento de
    /// login contra el hash guardado (Verificar()).
    ///
    /// La sal evita que dos usuarios con la misma contrasenia tengan el mismo
    /// hash guardado (y evita ataques de tablas precalculadas tipo rainbow
    /// table). No es secreta: se guarda junto con el hash para poder
    /// recalcularlo despues.
    /// </summary>
    public static class PasswordHasher_60MN
    {
        private const int TamanioSalBytes = 16;

        /// <summary>Genera el valor a persistir en la columna Clave: "sal.hash" (Base64).</summary>
        public static string Hashear(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            byte[] sal = new byte[TamanioSalBytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) rng.GetBytes(sal);

            byte[] hash = CalcularHash(password, sal);

            return string.Join(".", Convert.ToBase64String(sal), Convert.ToBase64String(hash));
        }

        /// <summary>Compara una contrasenia ingresada contra un valor generado por Hashear().</summary>
        public static bool Verificar(string password, string hashAlmacenado)
        {
            if (password == null || string.IsNullOrEmpty(hashAlmacenado)) return false;

            string[] partes = hashAlmacenado.Split('.');
            if (partes.Length != 2) return false;

            try
            {
                byte[] sal = Convert.FromBase64String(partes[0]);
                byte[] hashEsperado = Convert.FromBase64String(partes[1]);
                byte[] hashIngresado = CalcularHash(password, sal);

                return CryptographicOperations.FixedTimeEquals(hashEsperado, hashIngresado);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] CalcularHash(string password, byte[] sal)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinado = new byte[sal.Length + passwordBytes.Length];
            Buffer.BlockCopy(sal, 0, combinado, 0, sal.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinado, sal.Length, passwordBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
                return sha256.ComputeHash(combinado);
        }
    }
}
