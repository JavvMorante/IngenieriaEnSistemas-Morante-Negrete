using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Composite;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>
    /// Abrir la historia clínica (CUN03) o registrar una atención (CUN04) sobre una mascota.
    /// Lo usan la ficha del cliente (FrmClientes_60MN) y el menú PACIENTES, para que las dos
    /// entradas apliquen las mismas reglas.
    /// </summary>
    internal static class AccionesPaciente_60MN
    {
        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        /// <summary>Consulta la historia clínica; si la mascota no tiene, ofrece crearla.</summary>
        public static void AbrirHistoria(Form origen, Mascota_60MN mascota, Action? alCerrar = null)
        {
            if (!mascota.TieneHistoriaClinica &&
                !PuedeCrearHistoria(mascota, T("msg.SinHistoriaAbrir", "La mascota no tiene historia clínica. ¿Desea abrirla ahora?")))
                return;
            Historia(origen, mascota, alCerrar);
        }

        /// <summary>CUN04: la atención se registra sobre la historia clínica; sin historia, se deriva a crearla (1.1).</summary>
        public static void RegistrarAtencion(Form origen, Mascota_60MN mascota, Action? alCerrar = null)
        {
            if (!mascota.TieneHistoriaClinica)
            {
                if (PuedeCrearHistoria(mascota, T("msg.SinHistoriaAtencion",
                        "La mascota no posee historia clínica. Debe crearse antes de registrar la atención. ¿Abrirla ahora?")))
                    Historia(origen, mascota, alCerrar);
                return;
            }
            try
            {
                HistoriaClinica_60MN? hc = new HistoriaClinicaBLL_60MN().Consultar(mascota.Id);
                if (hc == null) return;
                VentanasMdi_60MN.Abrir(origen, () => new FrmAtencion_60MN(hc), $"atencion:{hc.Id}", alCerrar);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private static void Historia(Form origen, Mascota_60MN mascota, Action? alCerrar) =>
            VentanasMdi_60MN.Abrir(origen, () => new FrmHistoriaClinica_60MN(mascota.Id), $"historia:{mascota.Id}", alCerrar);

        private static bool PuedeCrearHistoria(Mascota_60MN mascota, string pregunta)
        {
            if (!SessionManager_60MN.Instancia.TienePermiso(CodigosPatente_60MN.HistoriaClinicaAlta))
            {
                Mensajes_60MN.Advertir("La mascota no tiene historia clínica y usted no tiene permiso para crearla.");
                return false;
            }
            if (!mascota.Activo)
            {
                Mensajes_60MN.Advertir("La mascota está dada de baja.");
                return false;
            }
            return Mensajes_60MN.Confirmar(pregunta);
        }
    }
}
