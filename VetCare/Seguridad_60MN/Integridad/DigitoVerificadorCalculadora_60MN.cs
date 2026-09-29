using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Seguridad_60MN.Integridad
{
    /// <summary>
    /// Calcula el DVH (dígito verificador horizontal, por fila) y el DVV
    /// (dígito verificador vertical, por tabla) con HMAC-SHA256. A diferencia
    /// de una suma de bytes, para fabricar un dígito válido hace falta conocer
    /// la clave secreta (VETCARE_CLAVE_DV), que no está ni en el código ni en
    /// la base de datos.
    /// </summary>
    public sealed class DigitoVerificadorCalculadora_60MN
    {
        private readonly byte[] clave;

        public DigitoVerificadorCalculadora_60MN(byte[] clave)
        {
            if (clave == null || clave.Length != 32)
                throw new ArgumentException("Se requiere una clave de 32 bytes.", nameof(clave));
            this.clave = (byte[])clave.Clone();
        }

        /// <summary>
        /// DVH de una fila: HMAC de "tabla|campo1|campo2|...". Se incluye el
        /// nombre de la tabla para que un DVH válido en una tabla no sirva
        /// si se copia en otra.
        /// </summary>
        public string CalcularDVH(string tabla, IEnumerable<object> campos)
        {
            StringBuilder contenido = new StringBuilder(tabla);
            foreach (object campo in campos)
                contenido.Append('|').Append(ValorATexto(campo));
            return Firmar(contenido.ToString());
        }

        /// <summary>
        /// DVV de una tabla: HMAC de la concatenación de todos sus DVH en el
        /// orden de la clave primaria. Si se agrega, borra o altera una fila
        /// por fuera del sistema, el DVV deja de coincidir.
        /// </summary>
        public string CalcularDVV(string tabla, IEnumerable<string> digitosHorizontales)
        {
            StringBuilder contenido = new StringBuilder(tabla);
            foreach (string dvh in digitosHorizontales)
                contenido.Append('|').Append(dvh ?? string.Empty);
            return Firmar(contenido.ToString());
        }

        private string Firmar(string contenido)
        {
            using (HMACSHA256 hmac = new HMACSHA256(clave))
                return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(contenido)));
        }

        /// <summary>
        /// Representación canónica de cada valor, para que el DVH calculado
        /// desde un objeto en memoria sea igual al calculado leyendo la fila
        /// de la base (por ejemplo, 12.5m y el decimal 12.50 de SQL Server).
        /// </summary>
        private static string ValorATexto(object valor)
        {
            if (valor == null || valor is DBNull) return "\0NULL\0";
            if (valor is DateTime fecha) return fecha.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            if (valor is bool booleano) return booleano ? "1" : "0";
            if (valor is decimal numero) return numero.ToString("0.############################", CultureInfo.InvariantCulture);
            if (valor is string texto) return texto.TrimEnd();
            return Convert.ToString(valor, CultureInfo.InvariantCulture);
        }
    }
}
