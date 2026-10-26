using System.Net.Mail;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// Reglas de formato compartidas por las BLL (cliente, mascota, usuario). Las
    /// validaciones de negocio viven en la BLL: la GUI solo marca los campos vacíos,
    /// y cualquier otro camino que llegue a la BLL pasa por las mismas reglas.
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

        /// <summary>
        /// Teléfono: dígitos, espacios, guiones y paréntesis, con un "+" opcional solo al
        /// principio, y entre 6 y 15 dígitos (15 es el máximo internacional, E.164).
        /// </summary>
        internal static void Telefono(string valor, string campo)
        {
            int digitos = valor.Count(char.IsDigit);
            bool caracteresValidos = valor.Select((c, i) => char.IsDigit(c) || c == ' ' || c == '-' || c == '(' || c == ')' || (c == '+' && i == 0)).All(ok => ok);
            if (!caracteresValidos || digitos < 6 || digitos > 15)
                throw new NegocioException_60MN($"{campo} no es válido: use solo números (6 a 15 dígitos), espacios, guiones, paréntesis o un + inicial.");
        }

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
