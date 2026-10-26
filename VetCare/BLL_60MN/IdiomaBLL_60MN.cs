using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Idioma;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>
    /// Multi-idioma: carga las traducciones del idioma elegido desde la base y
    /// se las entrega al GestorIdioma_60MN (sujeto del Observer), que notifica
    /// a los formularios abiertos.
    ///
    /// El idioma es una preferencia POR USUARIO (Usuario.IdiomaID): cada usuario
    /// tiene guardado su último idioma elegido y se le aplica al iniciar sesión.
    /// La pantalla de Login siempre abre en el idioma base (español) y se vuelve
    /// a él al cerrar la sesión.
    /// </summary>
    public class IdiomaBLL_60MN
    {
        private readonly IdiomaDAL_60MN mapper = new IdiomaDAL_60MN();
        private readonly UsuarioDAL_60MN usuarios = new UsuarioDAL_60MN();

        public List<Idioma_60MN> Listar() => mapper.Listar();

        /// <summary>Cambia el idioma en pantalla (todos los formularios abiertos se traducen).</summary>
        public void Aplicar(Idioma_60MN idioma)
        {
            // El idioma base usa los textos originales del diseñador: no necesita traducciones.
            Dictionary<string, string> traducciones = idioma.EsBase
                ? new Dictionary<string, string>()
                : mapper.ObtenerTraducciones(idioma.Id);
            GestorIdioma_60MN.Instancia.CambiarIdioma(idioma, traducciones);
        }

        /// <summary>Idioma base del sistema (español): el del Login y el de quien no eligió ninguno.</summary>
        public void AplicarBase()
        {
            List<Idioma_60MN> idiomas = mapper.Listar();
            Idioma_60MN? idiomaBase = idiomas.FirstOrDefault(i => i.EsBase) ?? idiomas.FirstOrDefault();
            if (idiomaBase != null)
                Aplicar(idiomaBase);
        }

        /// <summary>
        /// Idioma con el que queda la sesión recién iniciada:
        /// - usuario de emergencia (no tiene fila en la base): siempre el idioma base;
        /// - si eligió un idioma en el combo del Login: ese, y se guarda como su idioma;
        /// - si no: su último idioma guardado (o el base si nunca eligió uno).
        /// Aplicarlo notifica a todas las pantallas abiertas (Observer).
        /// </summary>
        public void AplicarIdiomaAlIngresar(Idioma_60MN? elegidoEnLogin)
        {
            SessionManager_60MN sesion = SessionManager_60MN.Instancia;
            if (!sesion.EstaLogueado || sesion.EsEmergencia)
            {
                AplicarBase();
                return;
            }

            if (elegidoEnLogin != null)
            {
                CambiarIdiomaUsuario(elegidoEnLogin);
                return;
            }

            // Cargar la preferencia guardada no es un cambio: no se registra en la bitácora.
            int? idiomaUsuario = sesion.Usuario!.IdiomaId;
            Idioma_60MN? idioma = idiomaUsuario.HasValue ? mapper.Listar().FirstOrDefault(i => i.Id == idiomaUsuario.Value) : null;
            if (idioma != null)
                Aplicar(idioma);
            else
                AplicarBase();
        }

        /// <summary>
        /// Cambio de idioma elegido por el usuario (combo del Login o USUARIO > Cambiar idioma):
        /// se aplica, se guarda contra SU usuario (actualizando DVH y DVV, porque IdiomaID está
        /// protegido por el dígito verificador) y queda en la bitácora.
        /// </summary>
        public void CambiarIdiomaUsuario(Idioma_60MN idioma)
        {
            SessionManager_60MN sesion = SessionManager_60MN.Instancia;
            List<Idioma_60MN> idiomas = mapper.Listar();

            // "Anterior" es el idioma guardado del usuario (no el de la pantalla: en el Login el combo ya lo cambió).
            int? idAnterior = sesion.EstaLogueado && !sesion.EsEmergencia
                ? sesion.Usuario!.IdiomaId ?? idiomas.FirstOrDefault(i => i.EsBase)?.Id
                : GestorIdioma_60MN.Instancia.IdiomaActual?.Id;
            string anterior = idiomas.FirstOrDefault(i => i.Id == idAnterior)?.Nombre ?? "-";

            Aplicar(idioma);
            if (!sesion.EstaLogueado || idAnterior == idioma.Id)
                return; // sin sesión, o eligió el mismo idioma que ya tenía: no hay nada que guardar ni registrar

            // El usuario de emergencia no tiene fila en la base: el cambio vale solo para esta sesión.
            if (!sesion.EsEmergencia)
            {
                int usuarioId = sesion.Usuario!.Id;
                usuarios.ActualizarIdioma(usuarioId, idioma.Id);
                new GestorDigitoVerificador_60MN().ActualizarFila(TablaDV_60MN.Usuario, usuarioId);
                sesion.Usuario.IdiomaId = idioma.Id;
            }
            new BitacoraBLL_60MN().Registrar(EventoSistema_60MN.IdiomaCambiado, $"Cambio de idioma: {anterior} → {idioma.Nombre}.");
        }
    }
}
