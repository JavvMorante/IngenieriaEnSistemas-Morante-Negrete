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
        Stock,
        Pacientes,
        Backup
    }

    /// <summary>
    /// Eventos tipificados que registra el caso de uso transversal Registrar Evento.
    /// Se guardan por nombre: no cambiar los nombres existentes (las bitácoras exportadas a XML los referencian).
    /// Numeración de casos de uso del Trabajo de Diploma V2.4.
    /// </summary>
    public enum EventoSistema_60MN
    {
        // Seguridad (CUS01 a CUS13)
        Login, LoginFallido, LoginEmergencia, Logout, UsuarioBloqueado, RegistroAlterado, AccesoDenegadoIntegridad,
        UsuarioCreado, UsuarioModificado, UsuarioBaja, UsuarioReactivado, UsuarioDesbloqueado, ClaveCambiada, ClaveBlanqueada, IdiomaCambiado,
        RolCreado, RolModificado, RolEliminado, PermisosUsuarioModificados,
        BitacoraEventosConsultada, BitacoraCambiosConsultada,
        BitacoraExportada, BitacoraDeserializada, BitacoraDeserializacionRechazada,
        // Ya no se generan (la importación se reemplazó por Deserializar XML): quedan para leer los eventos viejos.
        BitacoraImportada, BitacoraImportacionRechazada,
        IntegridadVerificada, IntegridadFallida, DigitosRecalculados,
        // PN2 - Gestión de Stock: CUN08 Administrar Existencias, CUN09 Movimiento de Stock, CUN10 Configurar Margen
        ProductoCreado, ProductoModificado, ProductoBaja, ProductoVersionRestaurada, MovimientoStock, AlertaReposicion,
        // CUN10, CUN12 Reporte de Existencias y CUN13 Reporte Inteligente
        ConfiguracionStockModificada, ReporteExistenciasGenerado, ReporteInteligenteGenerado,
        // CUN11 Gestionar Proveedores
        ProveedorRegistrado, ProveedorModificado, ProveedorBaja,
        // PN1 - Gestión de Pacientes: CUN01 Gestionar Clientes y CUN02 Gestionar Mascotas
        ClienteRegistrado, ClienteModificado, ClienteBaja, ClienteReactivado, ClientesConsultados,
        MascotaRegistrada, MascotaModificada, MascotaBaja,
        // CUN03 Gestionar Historias Clínicas
        HistoriaClinicaCreada, HistoriaClinicaConsultada, HistoriaClinicaModificada,
        // CUN04 Registrar Atención, CUN05 Medicación en Carrito (MedicacionEntregada: anterior a V2.4; hoy VentaRegistrada),
        // CUN06 Emitir Informe de Atención y CUN07 Reporte de Atenciones
        AtencionRegistrada, MedicacionEntregada, InformeAtencionEmitido, VentaRegistrada, VentasConsultadas, ReporteAtencionesGenerado,
        // CUS12 Gestión de Backup
        BackupRealizado, RestoreIniciado, RestoreRealizado
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
            // Exportar (serializar) y deserializar la bitácora en XML. Deserializar solo muestra el archivo en
            // pantalla; un archivo rechazado (firma inválida, alterado, etc.) es el caso más grave.
            Agregar(ModuloSistema_60MN.Bitacora, 1, EventoSistema_60MN.BitacoraDeserializacionRechazada, EventoSistema_60MN.BitacoraImportacionRechazada);
            Agregar(ModuloSistema_60MN.Bitacora, 2, EventoSistema_60MN.BitacoraImportada);
            Agregar(ModuloSistema_60MN.Bitacora, 4, EventoSistema_60MN.BitacoraExportada);
            Agregar(ModuloSistema_60MN.Bitacora, 5, EventoSistema_60MN.BitacoraDeserializada);

            Agregar(ModuloSistema_60MN.Integridad, 1, EventoSistema_60MN.IntegridadFallida, EventoSistema_60MN.DigitosRecalculados);
            Agregar(ModuloSistema_60MN.Integridad, 4, EventoSistema_60MN.IntegridadVerificada);

            // PN2: CUN08 Administrar Existencias, CUN09 Movimiento de Stock, CUN10 Configurar Margen y Alerta,
            // CUN11 Gestionar Proveedores, CUN12 Reporte de Existencias y CUN13 Reporte Inteligente.
            Agregar(ModuloSistema_60MN.Stock, 3, EventoSistema_60MN.ProductoBaja, EventoSistema_60MN.ProductoVersionRestaurada,
                EventoSistema_60MN.AlertaReposicion);
            Agregar(ModuloSistema_60MN.Stock, 4, EventoSistema_60MN.ProductoCreado, EventoSistema_60MN.ProductoModificado,
                EventoSistema_60MN.MovimientoStock);
            Agregar(ModuloSistema_60MN.Stock, 3, EventoSistema_60MN.ConfiguracionStockModificada);
            // CUN11 Gestionar Proveedores: la baja es más crítica porque deja de ofrecerse en el alta de productos.
            Agregar(ModuloSistema_60MN.Stock, 4, EventoSistema_60MN.ProveedorRegistrado, EventoSistema_60MN.ProveedorModificado);
            Agregar(ModuloSistema_60MN.Stock, 3, EventoSistema_60MN.ProveedorBaja);
            Agregar(ModuloSistema_60MN.Stock, 5, EventoSistema_60MN.ReporteExistenciasGenerado,
                EventoSistema_60MN.ReporteInteligenteGenerado);

            // PN1: CUN01 Clientes, CUN02 Mascotas, CUN03 Historias Clínicas, CUN04 Atención, CUN05 Carrito,
            // CUN06 Informe de Atención y CUN07 Reporte de Atenciones.
            Agregar(ModuloSistema_60MN.Pacientes, 3, EventoSistema_60MN.ClienteBaja, EventoSistema_60MN.MascotaBaja,
                EventoSistema_60MN.HistoriaClinicaModificada);
            Agregar(ModuloSistema_60MN.Pacientes, 4, EventoSistema_60MN.ClienteRegistrado, EventoSistema_60MN.ClienteModificado,
                EventoSistema_60MN.ClienteReactivado, EventoSistema_60MN.MascotaRegistrada, EventoSistema_60MN.MascotaModificada,
                EventoSistema_60MN.HistoriaClinicaCreada, EventoSistema_60MN.HistoriaClinicaConsultada,
                EventoSistema_60MN.AtencionRegistrada, EventoSistema_60MN.MedicacionEntregada, EventoSistema_60MN.InformeAtencionEmitido);
            Agregar(ModuloSistema_60MN.Pacientes, 5, EventoSistema_60MN.ClientesConsultados);
            // CUN05: la venta del carrito (con o sin atención). MedicacionEntregada queda para los
            // eventos registrados antes de V2.4 (bitácoras exportadas a XML que se vuelvan a importar).
            Agregar(ModuloSistema_60MN.Pacientes, 4, EventoSistema_60MN.VentaRegistrada);
            // Consulta de ventas (CUN05) y CUN07 Reporte de Atenciones: informativos.
            Agregar(ModuloSistema_60MN.Pacientes, 5, EventoSistema_60MN.VentasConsultadas, EventoSistema_60MN.ReporteAtencionesGenerado);

            Agregar(ModuloSistema_60MN.Backup, 1, EventoSistema_60MN.RestoreIniciado, EventoSistema_60MN.RestoreRealizado);
            Agregar(ModuloSistema_60MN.Backup, 2, EventoSistema_60MN.BackupRealizado);

            foreach (EventoSistema_60MN evento in Enum.GetValues(typeof(EventoSistema_60MN)))
                if (!resultado.ContainsKey(evento))
                    throw new InvalidOperationException("Evento sin definición en el catálogo: " + evento);

            return resultado;
        }
    }
}
