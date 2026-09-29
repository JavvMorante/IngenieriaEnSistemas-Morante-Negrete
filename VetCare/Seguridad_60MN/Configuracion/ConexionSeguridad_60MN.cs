using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Seguridad_60MN.Configuracion
{
    /// <summary>Conexión propia de la capa de seguridad a VetCareBD_60MN (misma cadena "conexionBD" del App.config).</summary>
    internal static class ConexionSeguridad_60MN
    {
        internal static SqlConnection Abrir()
        {
            string cadena = ConfigurationManager.AppSettings["conexionBD"];
            if (string.IsNullOrWhiteSpace(cadena))
                throw new InvalidOperationException("Falta la cadena de conexión 'conexionBD' en App.config.");
            SqlConnection conexion = new SqlConnection(cadena);
            conexion.Open();
            return conexion;
        }
    }
}
