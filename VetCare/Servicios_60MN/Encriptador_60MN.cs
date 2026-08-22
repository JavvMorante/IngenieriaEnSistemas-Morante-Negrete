using System.Security.Cryptography;
using System.Text;

namespace Servicios_60MN
{
    public class Encriptador_60MN
    {
        public static string Hash(string value)
        {
            using MD5 md5 = MD5.Create();

            byte[] md5data = md5.ComputeHash(
                Encoding.ASCII.GetBytes(value)
            );

            return Convert.ToHexString(md5data);
        }
    }
}