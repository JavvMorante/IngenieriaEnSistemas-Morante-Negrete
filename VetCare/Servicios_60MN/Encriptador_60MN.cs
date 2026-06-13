using System.Security.Cryptography;
using System.Text;

namespace Servicios_60MN
{
    public class Encriptador_60MN
    {

        public static string Hash(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(value));
            return (new ASCIIEncoding()).GetString(md5data);
        }

    }
}
