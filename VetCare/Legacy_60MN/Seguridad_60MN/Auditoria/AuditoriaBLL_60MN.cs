using System;
using System.Data;
using Seguridad_60MN.Integridad;

namespace Seguridad_60MN.Auditoria
{
    /// <summary>
    /// Punto unico de entrada para registrar y consultar la bitacora.
    /// Reemplaza a BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN /
    /// DAL_60MN.BitacoraDAL_60MN. La UI y la BLL de negocio deberian
    /// llamar solo a esta clase (Registrar / Consultar), no instanciar
    /// el mapper de bitacora ni el de encriptacion por separado como se
    /// hacia antes en cada formulario.
    /// </summary>
    public sealed class AuditoriaBLL_60MN
    {
        private readonly AuditoriaDAL_60MN mapper;
        private readonly DigitoVerificadorBLL_60MN digitoVerificador;

        public AuditoriaBLL_60MN(string cadenaConexion = null)
        {
            mapper = new AuditoriaDAL_60MN(cadenaConexion);
            digitoVerificador = new DigitoVerificadorBLL_60MN(cadenaConexion);
        }

        /// <summary>
        /// Registra un evento de bitacora. La descripcion se guarda en
        /// texto plano (a diferencia de antes, que la cifraba con
        /// EncriptacionBLL_60MN): no hace falta cifrar el texto de
        /// auditoria, solo la contrasenia. Si igual queres cifrarla,
        /// llama a CriptografiaHandler_60MN.Encriptar(descripcion) antes
        /// de pasarla.
        ///
        /// Nunca tira excepcion hacia arriba (igual que el BitacoraBLL_60MN
        /// actual): devuelve "OK" o el mensaje de error, para no cortar la
        /// operacion original si falla el logueo.
        /// </summary>
        public string Registrar(TipoEventoBitacora_60MN tipo, int usuarioId, string descripcion)
        {
            DefinicionEventoBitacora_60MN definicion = CatalogoAuditoria_60MN.Obtener(tipo);
            try
            {
                mapper.Insertar(tipo.ToString(), descripcion, usuarioId, definicion.Criticidad, DateTime.Now);
                digitoVerificador.RecalcularTabla("Bitacora");
                return "OK";
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }

        /// <summary>
        /// Consulta la bitacora, con permiso obligatorio: si no se pasa
        /// "tienePermiso" o devuelve false, se corta con
        /// UnauthorizedAccessException antes de tocar la base.
        /// </summary>
        /// <param name="tienePermiso">
        /// Chequeo de permisos que ya tenga el proyecto (por ejemplo,
        /// ManejadorPerfilUsuarioBLL_60MN.verificarPatentesEscenciales
        /// para la patente de "consultar bitacora"). Se recibe como
        /// delegado para no acoplar esta capa a la implementacion
        /// concreta de permisos de VetCare.
        /// </param>
        public DataTable Consultar(DateTime desde, DateTime hasta, int? criticidad, int? usuarioId, Func<bool> tienePermiso)
        {
            if (tienePermiso != null && !tienePermiso())
                throw new UnauthorizedAccessException("No tiene permiso para consultar la bitácora.");

            return mapper.Consultar(desde, hasta.Date.AddDays(1), criticidad, usuarioId);
        }
    }
}
