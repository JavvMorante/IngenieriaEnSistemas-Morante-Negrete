using System;
using System.Linq;
using System.Security.Cryptography;

namespace Seguridad_60MN.Criptografia
{
    /// <summary>Política de complejidad mínima de contraseñas (CUS07, flujo 5.2) y generación de claves iniciales (CUS01).</summary>
    public static class PoliticaClave_60MN
    {
        public const int LongitudMinima = 8;

        public const string Descripcion =
            "La contraseña debe tener al menos 8 caracteres e incluir mayúsculas, minúsculas y números.";

        /// <summary>Devuelve null si la clave cumple la política, o el motivo por el que no la cumple.</summary>
        public static string Validar(string clave)
        {
            if (string.IsNullOrEmpty(clave) || clave.Length < LongitudMinima)
                return $"La contraseña debe tener al menos {LongitudMinima} caracteres.";
            if (!clave.Any(char.IsUpper)) return "La contraseña debe incluir al menos una letra mayúscula.";
            if (!clave.Any(char.IsLower)) return "La contraseña debe incluir al menos una letra minúscula.";
            if (!clave.Any(char.IsDigit)) return "La contraseña debe incluir al menos un número.";
            return null;
        }

        /// <summary>Genera una contraseña aleatoria que cumple la política (sin caracteres ambiguos como 0/O o 1/l).</summary>
        public static string Generar(int longitud = 10)
        {
            const string mayusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string minusculas = "abcdefghijkmnpqrstuvwxyz";
            const string digitos = "23456789";
            const string todos = mayusculas + minusculas + digitos;

            char[] clave = new char[Math.Max(longitud, LongitudMinima)];
            clave[0] = mayusculas[RandomNumberGenerator.GetInt32(mayusculas.Length)];
            clave[1] = minusculas[RandomNumberGenerator.GetInt32(minusculas.Length)];
            clave[2] = digitos[RandomNumberGenerator.GetInt32(digitos.Length)];
            for (int i = 3; i < clave.Length; i++)
                clave[i] = todos[RandomNumberGenerator.GetInt32(todos.Length)];

            // Mezcla (Fisher-Yates) para que los tipos obligatorios no queden siempre al principio.
            for (int i = clave.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (clave[i], clave[j]) = (clave[j], clave[i]);
            }
            return new string(clave);
        }
    }
}
