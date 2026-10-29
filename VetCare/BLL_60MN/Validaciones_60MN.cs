using System.Net.Mail;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// Reglas de formato compartidas por las BLL (cliente, mascota, usuario). Las
    /// validaciones de negocio viven en la BLL: la GUI solo filtra lo que se tipea y
    /// muestra los errores que devuelve la BLL; cualquier otro camino pasa por las mismas reglas.
    /// </summary>
    internal static class Validaciones_60MN
    {
        /// <summary>Recorta y deja un solo espacio entre palabras ("  Juan   Pablo " → "Juan Pablo").</summary>
        internal static string Normalizar(string? valor) =>
            string.Join(" ", (valor ?? string.Empty).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

        /// <summary>El valor no supera el largo de la columna en la base (evita el error "datos truncados" de SQL).</summary>
        internal static void Largo(string? valor, int maximo, string campo)
        {
            if (valor != null && valor.Length > maximo)
                throw new NegocioException_60MN($"{campo} no puede superar los {maximo} caracteres.");
        }

        /// <summary>Nombres y apellidos: letras (con acentos y ñ), espacios, apóstrofo, guion y punto.</summary>
        internal static void NombrePropio(string valor, string campo)
        {
            if (!valor.Any(char.IsLetter) || valor.Any(c => !char.IsLetter(c) && c != ' ' && c != '\'' && c != '-' && c != '.'))
                throw new NegocioException_60MN($"{campo} solo puede contener letras, espacios, apóstrofos y guiones.");
        }

        /// <summary>Texto descriptivo (especie, raza, nombre de mascota): letras, números, espacios y signos simples.</summary>
        internal static void TextoSimple(string valor, string campo)
        {
            if (!valor.Any(char.IsLetter) || valor.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && c != '\'' && c != '-' && c != '.'))
                throw new NegocioException_60MN($"{campo} solo puede contener letras, números, espacios, apóstrofos y guiones.");
        }

        /// <summary>Signos admitidos en nombres de productos ("Amoxicilina 250 mg x 10 comp.", "Pipeta 10-20 kg").</summary>
        internal const string SignosProducto = ".,-/%()+'";

        /// <summary>Nombre de producto: letras, números, espacios y los signos de SignosProducto; al menos una letra.</summary>
        internal static void NombreProducto(string valor, string campo)
        {
            if (valor.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && SignosProducto.IndexOf(c) < 0))
                throw new NegocioException_60MN($"{campo} solo puede contener letras, números, espacios y los signos {SignosProducto}");
            if (!valor.Any(char.IsLetter))
                throw new NegocioException_60MN($"{campo} debe incluir letras, no solo números o signos.");
        }

        /// <summary>Lote: letras sin acentos, números, guion y barra ("AMX-2401", "COL-M-01").</summary>
        internal static void Lote(string valor, string campo)
        {
            if (valor.Any(c => !(c >= 'A' && c <= 'Z') && !char.IsDigit(c) && c != '-' && c != '/'))
                throw new NegocioException_60MN($"{campo} solo puede contener letras (sin acentos), números, guiones y barras.");
            if (!valor.Any(char.IsLetterOrDigit))
                throw new NegocioException_60MN($"{campo} debe incluir letras o números.");
        }

        /// <summary>Texto libre corto (motivo de un movimiento): letras, números, espacios y signos de puntuación comunes.</summary>
        internal static void TextoLibre(string valor, string campo)
        {
            const string signos = ".,;:-/%()+'\"°º#¿?¡!";
            if (valor.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && signos.IndexOf(c) < 0))
                throw new NegocioException_60MN($"{campo} contiene caracteres no admitidos (use letras, números y signos de puntuación comunes).");
            if (!valor.Any(char.IsLetter))
                throw new NegocioException_60MN($"{campo} debe describirse con palabras.");
        }

        /// <summary>Dirección: letras, números, espacios y los signos de una dirección (. , - / ° º # '); al menos una letra.</summary>
        internal static void Direccion(string valor, string campo)
        {
            const string signos = ".,-/°º#'";
            if (valor.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && signos.IndexOf(c) < 0))
                throw new NegocioException_60MN($"{campo} solo puede contener letras, números, espacios y los signos {signos}");
            if (!valor.Any(char.IsLetter))
                throw new NegocioException_60MN($"{campo} debe incluir el nombre de la calle, no solo números.");
        }

        /// <summary>Razón social: letras, números, espacios y signos habituales ("Droguería del Sur S.A.", "Pet & Co S.R.L."); al menos una letra.</summary>
        internal static void RazonSocial(string valor, string campo)
        {
            const string signos = ".,-&'()/";
            if (valor.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && signos.IndexOf(c) < 0))
                throw new NegocioException_60MN($"{campo} solo puede contener letras, números, espacios y los signos {signos}");
            if (!valor.Any(char.IsLetter))
                throw new NegocioException_60MN($"{campo} debe incluir letras, no solo números o signos.");
        }

        /// <summary>Teléfono ya sin separadores: solo dígitos, entre minimo y maximo.</summary>
        internal static void TelefonoDigitos(string valor, int minimo, int maximo, string campo)
        {
            if (!valor.All(char.IsDigit))
                throw new NegocioException_60MN($"{campo} solo puede contener números.");
            if (valor.Length < minimo || valor.Length > maximo)
                throw new NegocioException_60MN($"{campo} debe tener entre {minimo} y {maximo} dígitos.");
        }

        /// <summary>Quita los separadores habituales de documentos y teléfonos ("30.111.222", "11 4244-1020", "(011)").</summary>
        internal static string SinSeparadores(string? valor) =>
            new string((valor ?? string.Empty).Where(ch => ch != '.' && ch != '-' && ch != '(' && ch != ')' && !char.IsWhiteSpace(ch)).ToArray());

        /// <summary>
        /// Dígito verificador de CUIL y CUIT (módulo 11): cada uno de los 10 primeros dígitos se
        /// multiplica por 5, 4, 3, 2, 7, 6, 5, 4, 3, 2; se suman y el dígito es 11 menos el
        /// resto de dividir por 11 (si da 11 es 0; si da 10, el número no es válido → -1).
        /// </summary>
        internal static int DigitoVerificadorCuil(string numero)
        {
            int[] pesos = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            int suma = 0;
            for (int i = 0; i < 10; i++)
                suma += (numero[i] - '0') * pesos[i];
            int digito = 11 - suma % 11;
            return digito == 11 ? 0 : digito == 10 ? -1 : digito;
        }

        /// <summary>11 dígitos y el último coincide con el dígito verificador.</summary>
        internal static bool CuitConDigitoValido(string numero) =>
            numero.Length == 11 && numero.All(char.IsDigit) && DigitoVerificadorCuil(numero) == numero[10] - '0';

        /// <summary>
        /// Mail: formato reconocido por MailAddress, sin nombre visible ("Juan &lt;j@x.com&gt;"),
        /// dominio con al menos un punto y sin puntos al principio, al final ni dobles
        /// en la parte local ni en el dominio (por ejemplo rechaza "j@x.com." y "j.@x.com").
        /// </summary>
        internal static bool EsEmailValido(string? email)
        {
            if (string.IsNullOrWhiteSpace(email) || email!.Any(char.IsWhiteSpace)) return false;
            MailAddress direccion;
            try
            {
                direccion = new MailAddress(email);
            }
            catch (FormatException)
            {
                return false;
            }
            if (direccion.Address != email) return false;

            string local = direccion.User;
            string dominio = direccion.Host;
            bool PuntosCorrectos(string parte) =>
                parte.Length > 0 && !parte.StartsWith(".") && !parte.EndsWith(".") && !parte.Contains("..");
            return PuntosCorrectos(local) && PuntosCorrectos(dominio) && dominio.Contains(".")
                   && dominio.Split('.').Last().Length >= 2;
        }
    }
}
