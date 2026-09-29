using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>Verificación y recálculo de los dígitos verificadores horizontales y verticales.</summary>
    public class IntegridadBLL_60MN
    {
        private readonly GestorDigitoVerificador_60MN gestor = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Verificación sin control de permisos ni registro, usada durante el login.</summary>
        internal List<InconsistenciaDV_60MN> VerificarInterno() => gestor.VerificarTodo();

        public List<InconsistenciaDV_60MN> Verificar()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Integridad);
            List<InconsistenciaDV_60MN> resultado = gestor.VerificarTodo();
            if (resultado.Count == 0)
                bitacora.Registrar(EventoSistema_60MN.IntegridadVerificada, "Verificación de dígitos verificadores: base íntegra.");
            else
                bitacora.Registrar(EventoSistema_60MN.IntegridadFallida,
                    $"Verificación de dígitos verificadores: {resultado.Count} inconsistencia(s) en " +
                    string.Join(", ", resultado.Select(r => r.Tabla).Distinct()) + ".");
            return resultado;
        }

        /// <summary>
        /// Recalcula todos los DVH y DVV aceptando el estado actual de la base
        /// como válido. Solo debe hacerse después de revisar las inconsistencias.
        /// </summary>
        public void RecalcularTodo()
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Integridad);
            gestor.RecalcularTodo();
            SessionManager_60MN.Instancia.IntegridadComprometida = false;
            bitacora.Registrar(EventoSistema_60MN.DigitosRecalculados, "Se recalcularon todos los dígitos verificadores (DVH y DVV).");
        }
    }
}
