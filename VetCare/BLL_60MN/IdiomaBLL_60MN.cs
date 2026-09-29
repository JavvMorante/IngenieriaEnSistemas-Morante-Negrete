using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Servicios_60MN.Idioma;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>
    /// Multi-idioma: carga las traducciones del idioma elegido desde la base y
    /// se las entrega al GestorIdioma_60MN (sujeto del Observer), que notifica
    /// a los formularios abiertos. El idioma elegido queda como predeterminado
    /// para el próximo inicio de sesión.
    /// </summary>
    public class IdiomaBLL_60MN
    {
        private readonly IdiomaDAL_60MN mapper = new IdiomaDAL_60MN();

        public List<Idioma_60MN> Listar() => mapper.Listar();

        /// <summary>Idioma marcado como predeterminado (o el base si no hay ninguno marcado).</summary>
        public Idioma_60MN? ObtenerPredeterminado()
        {
            List<Idioma_60MN> idiomas = mapper.Listar();
            return idiomas.FirstOrDefault(i => i.Predeterminado) ?? idiomas.FirstOrDefault(i => i.EsBase) ?? idiomas.FirstOrDefault();
        }

        /// <summary>Aplica el idioma predeterminado al iniciar la aplicación.</summary>
        public void AplicarPredeterminado()
        {
            Idioma_60MN? idioma = ObtenerPredeterminado();
            if (idioma != null)
                Aplicar(idioma);
        }

        /// <summary>Cambia el idioma en pantalla (todos los formularios abiertos se traducen).</summary>
        public void Aplicar(Idioma_60MN idioma)
        {
            // El idioma base usa los textos originales del diseñador: no necesita traducciones.
            Dictionary<string, string> traducciones = idioma.EsBase
                ? new Dictionary<string, string>()
                : mapper.ObtenerTraducciones(idioma.Id);
            GestorIdioma_60MN.Instancia.CambiarIdioma(idioma, traducciones);
        }

        /// <summary>Aplica el idioma y lo guarda como predeterminado para el próximo inicio de sesión.</summary>
        public void CambiarYGuardar(Idioma_60MN idioma)
        {
            string anterior = GestorIdioma_60MN.Instancia.IdiomaActual?.Nombre ?? "-";
            Aplicar(idioma);
            mapper.EstablecerPredeterminado(idioma.Id);
            if (SessionManager_60MN.Instancia.EstaLogueado)
                new BitacoraBLL_60MN().Registrar(EventoSistema_60MN.IdiomaCambiado, $"Cambio de idioma: {anterior} → {idioma.Nombre}.");
        }
    }
}
