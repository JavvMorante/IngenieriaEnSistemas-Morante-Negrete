namespace Servicios_60MN.Excepciones
{
    public enum ResultadoLogin_60MN
    {
        CamposVacios,
        SesionYaIniciada,
        UsuarioInexistente,
        CredencialesInvalidas,
        UsuarioBloqueado,
        RegistroAlterado,
        IntegridadComprometida,
        EmergenciaNoHabilitada,
        ErrorBaseDeDatos
    }

    /// <summary>Motivo por el que se rechazó un inicio de sesión (flujos alternativos de CUS05).</summary>
    public class LoginException_60MN : Exception
    {
        public ResultadoLogin_60MN Resultado { get; }

        public LoginException_60MN(ResultadoLogin_60MN resultado, string mensaje) : base(mensaje)
        {
            Resultado = resultado;
        }
    }
}
