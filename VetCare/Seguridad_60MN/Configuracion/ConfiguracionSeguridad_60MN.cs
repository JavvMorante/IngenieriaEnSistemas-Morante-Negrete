using System;
using System.Configuration;
using System.Security.Cryptography;

namespace Seguridad_60MN.Configuracion
{
    /// <summary>
    /// Punto unico donde se leen los secretos de toda la capa de seguridad
    /// (clave de cifrado reversible, clave de firma de los digitos
    /// verificadores). Ninguna clave sensible va hardcodeada en el codigo
    /// fuente ni se sube al repositorio -- a diferencia de la version
    /// anterior, donde EncriptacionBLL_60MN tenia "MasterKey" escrita en
    /// el propio .cs. Se configuran por variable de entorno o, si no esta
    /// definida, en el App.config (appSettings) del proyecto de inicio
    /// (VetCare), igual que ya se hace con "conexionBD".
    /// </summary>
    public static class ConfiguracionSeguridad_60MN
    {
        public const string ClaveCriptografiaSetting = "VETCARE_CLAVE_CRIPTO";
        public const string ClaveDigitoVerificadorSetting = "VETCARE_CLAVE_DV";

        /// <summary>Clave maestra para CriptografiaHandler_60MN (texto libre, cuanto mas larga y aleatoria, mejor).</summary>
        public static string ObtenerClaveCriptografia()
        {
            return ObtenerTexto(ClaveCriptografiaSetting);
        }

        /// <summary>Clave de 32 bytes (Base64) para firmar los digitos verificadores con HMAC-SHA256.</summary>
        public static byte[] ObtenerClaveDigitoVerificador()
        {
            string valor = ObtenerTexto(ClaveDigitoVerificadorSetting);
            byte[] clave;
            try
            {
                clave = Convert.FromBase64String(valor);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException(
                    $"'{ClaveDigitoVerificadorSetting}' debe ser una clave de 32 bytes codificada en Base64. " +
                    "Generar una nueva con ConfiguracionSeguridad_60MN.GenerarClaveAleatoria().");
            }
            if (clave.Length != 32)
                throw new InvalidOperationException(
                    $"'{ClaveDigitoVerificadorSetting}' debe tener exactamente 32 bytes (tiene {clave.Length}).");
            return clave;
        }

        /// <summary>
        /// Genera un valor Base64 de 32 bytes al azar, listo para pegar en
        /// App.config como VETCARE_CLAVE_DV. Usar UNA sola vez por entorno
        /// (dev, produccion) y no volver a cambiarlo despues, porque todos
        /// los digitos verificadores calculados con una clave anterior
        /// dejan de validar si la clave cambia.
        /// </summary>
        public static string GenerarClaveAleatoria()
        {
            byte[] bytes = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string ObtenerTexto(string nombreSetting)
        {
            string valor = Environment.GetEnvironmentVariable(nombreSetting);
            if (string.IsNullOrWhiteSpace(valor))
                valor = ConfigurationManager.AppSettings[nombreSetting];
            if (string.IsNullOrWhiteSpace(valor))
                throw new InvalidOperationException(
                    $"Falta configurar '{nombreSetting}' (variable de entorno o <appSettings> de App.config). " +
                    "Ver README_Seguridad_60MN.md, seccion 'Configuracion inicial'.");
            return valor;
        }
    }
}
