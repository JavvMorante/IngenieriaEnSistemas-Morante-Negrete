using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Seguridad_60MN.Integridad
{
    /// <summary>
    /// Calcula el DVH (digito verificador horizontal, por fila) y el DVV
    /// (digito verificador vertical, por tabla) usando HMAC-SHA256.
    ///
    /// Reemplaza el algoritmo anterior de DAL_60MN.DigitosVerificadores_60MN
    /// (sumar cada byte de los campos multiplicado por su posicion). Ese
    /// algoritmo no es un mecanismo de seguridad real: cualquiera que edite
    /// una fila directamente en la base puede recalcular esa suma a mano y
    /// dejar el DVH "correcto". Con HMAC-SHA256 hace falta conocer la clave
    /// secreta (que no viaja en el codigo ni en la base, ver
    /// ConfiguracionSeguridad_60MN) para poder generar un digito valido.
    /// </summary>
    public sealed class DigitoVerificadorCalculadora_60MN
    {
        private readonly byte[] clave;

        /// <summary>
        /// Huella (SHA-256) de la clave usada, NO la clave en si. Sirve para
        /// detectar si la base fue firmada con una clave distinta a la
        /// configurada actualmente (por ejemplo, tras restaurar un backup
        /// de otro entorno) sin exponer la clave real en ningun lado.
        /// </summary>
        public string IdentificadorClave { get; }

        public DigitoVerificadorCalculadora_60MN(byte[] clave)
        {
            if (clave == null || clave.Length != 32)
                throw new ArgumentException("Se requiere una clave de 32 bytes.", nameof(clave));
            this.clave = (byte[])clave.Clone();
            using (SHA256 sha = SHA256.Create())
                IdentificadorClave = Convert.ToHexString(sha.ComputeHash(clave));
        }

        /// <summary>
        /// DVH de una fila: firma HMAC de la concatenacion de sus campos.
        /// Se incluye el nombre de tabla en el contenido firmado para que
        /// un DVH calculado para "Usuario" no sea valido si se lo pega en
        /// una fila de "Operacion".
        /// </summary>
        public string CalcularDVH(string tabla, params object[] campos)
        {
            string contenido = tabla + "|" + string.Join("|", Array.ConvertAll(campos, ValorATexto));
            return Firmar(contenido);
        }

        /// <summary>
        /// DVV de una tabla: firma HMAC de la concatenacion de todos los
        /// DVH de esa tabla, en el orden en que se leyeron. Si un solo DVH
        /// cambia, o se agrega/borra una fila sin recalcular, el DVV deja
        /// de coincidir con lo guardado.
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
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(contenido));
                return Convert.ToHexString(hash); // 64 caracteres hexadecimales
            }
        }

        private static string ValorATexto(object valor)
        {
            if (valor == null) return "\0NULL\0";
            if (valor is DateTime fecha) return fecha.ToString("O", CultureInfo.InvariantCulture); // ISO 8601, sin ambiguedad regional
            if (valor is bool booleano) return booleano ? "1" : "0";
            return Convert.ToString(valor, CultureInfo.InvariantCulture);
        }
    }
}
