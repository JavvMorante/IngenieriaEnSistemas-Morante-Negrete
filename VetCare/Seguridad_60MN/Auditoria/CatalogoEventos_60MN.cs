using System;
using System.Collections.Generic;
using System.Linq;

namespace Seguridad_60MN.Auditoria
{
    /// <summary>Módulos del sistema tipificados para la bitácora (CUS09, paso 2).</summary>
    public enum ModuloSistema_60MN
    {
        Seguridad,
        Usuarios,
        Roles,
        Bitacora,
        Integridad,
        Stock
    }

    /// <summary>Eventos tipificados que registra el caso de uso transversal Registrar Evento.</summary>
    public enum EventoSistema_60MN
    {
        Login, LoginFallido, LoginEmergencia, Logout, UsuarioBloqueado, RegistroAlterado, AccesoDenegadoIntegridad,
        UsuarioCreado, UsuarioModificado, UsuarioBaja, UsuarioReactivado, UsuarioDesbloqueado, ClaveCambiada, ClaveBlanqueada, IdiomaCambiado,
        RolCreado, RolModificado, RolEliminado, PermisosUsuarioModificados,
        BitacoraEventosConsultada, BitacoraCambiosConsultada,
        IntegridadVerificada, IntegridadFallida, DigitosRecalculados,
        ProductoCreado, ProductoModificado, ProductoBaja, ProductoVersionRestaurada, MovimientoStock, AlertaReposicion
    }

    public sealed class DefinicionEvento_60MN
    {
        public EventoSistema_60MN Evento { get; }

        public ModuloSistema_60MN Modulo { get; }

        /// <summary>Escala de 1 a 5: 1 = más crítico, 5 = informativo.</summary>
        public int Criticidad { get; }

        internal DefinicionEvento_60MN(EventoSistema_60MN evento, ModuloSistema_60MN modulo, int criticidad)
        {
            Evento = evento;
            Modulo = modulo;
            Criticidad = criticidad;
        }
    }

    /// <summary>Define una sola vez el módulo y la criticidad de cada evento del sistema.</summary>
    public static class CatalogoEventos_60MN
    {
        private static readonly Dictionary<EventoSistema_60MN, DefinicionEvento_60MN> definiciones = Construir();

        public static DefinicionEvento_60MN Obtener(EventoSistema_60MN evento) => definiciones[evento];

        public static IEnumerable<EventoSistema_60MN> EventosDe(ModuloSistema_60MN modulo) =>
            definiciones.Values.Where(d => d.Modulo == modulo).Select(d => d.Evento);

        private static Dictionary<EventoSistema_60MN, DefinicionEvento_60MN> Construir()
        {
            Dictionary<EventoSistema_60MN, DefinicionEvento_60MN> resultado = new Dictionary<EventoSistema_60MN, DefinicionEvento_60MN>();

            void Agregar(ModuloSistema_60MN modulo, int criticidad, params EventoSistema_60MN[] eventos)
            {
                foreach (EventoSistema_60MN evento in eventos)
                    resultado.Add(evento, new DefinicionEvento_60MN(evento, modulo, criticidad));
            }

            Agregar(ModuloSistema_60MN.Seguridad, 1, EventoSistema_60MN.LoginEmergencia, EventoSistema_60MN.RegistroAlterado,
                EventoSistema_60MN.AccesoDenegadoIntegridad);
            Agregar(ModuloSistema_60MN.Seguridad, 2, EventoSistema_60MN.UsuarioBloqueado);
            Agregar(ModuloSistema_60MN.Seguridad, 3, EventoSistema_60MN.LoginFallido, EventoSistema_60MN.ClaveCambiada);
            Agregar(ModuloSistema_60MN.Seguridad, 4, EventoSistema_60MN.Login, EventoSistema_60MN.Logout);

            Agregar(ModuloSistema_60MN.Usuarios, 2, EventoSistema_60MN.UsuarioCreado, EventoSistema_60MN.UsuarioBaja,
                EventoSistema_60MN.UsuarioReactivado, EventoSistema_60MN.UsuarioDesbloqueado, EventoSistema_60MN.ClaveBlanqueada);
            Agregar(ModuloSistema_60MN.Usuarios, 3, EventoSistema_60MN.UsuarioModificado);
            Agregar(ModuloSistema_60MN.Usuarios, 5, EventoSistema_60MN.IdiomaCambiado);

            Agregar(ModuloSistema_60MN.Roles, 2, EventoSistema_60MN.RolCreado, EventoSistema_60MN.RolModificado, EventoSistema_60MN.RolEliminado,
                EventoSistema_60MN.PermisosUsuarioModificados);

            Agregar(ModuloSistema_60MN.Bitacora, 5, EventoSistema_60MN.BitacoraEventosConsultada, EventoSistema_60MN.BitacoraCambiosConsultada);

            Agregar(ModuloSistema_60MN.Integridad, 1, EventoSistema_60MN.IntegridadFallida, EventoSistema_60MN.DigitosRecalculados);
            Agregar(ModuloSistema_60MN.Integridad, 4, EventoSistema_60MN.IntegridadVerificada);

            Agregar(ModuloSistema_60MN.Stock, 3, EventoSistema_60MN.ProductoBaja, EventoSistema_60MN.ProductoVersionRestaurada,
                EventoSistema_60MN.AlertaReposicion);
            Agregar(ModuloSistema_60MN.Stock, 4, EventoSistema_60MN.ProductoCreado, EventoSistema_60MN.ProductoModificado,
                EventoSistema_60MN.MovimientoStock);

            foreach (EventoSistema_60MN evento in Enum.GetValues(typeof(EventoSistema_60MN)))
                if (!resultado.ContainsKey(evento))
                    throw new InvalidOperationException("Evento sin definición en el catálogo: " + evento);

            return resultado;
        }
    }
}
