using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>CUN05 Crear, CUN06 Consultar y CUN07 Actualizar Historia Clínica.</summary>
    public class HistoriaClinicaBLL_60MN
    {
        private readonly HistoriaClinicaDAL_60MN mapper = new HistoriaClinicaDAL_60MN();
        private readonly ClienteDAL_60MN clientes = new ClienteDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>
        /// CUN06: legajo de la mascota con su cabecera (paciente y propietario), o null si
        /// todavía no tiene historia (flujo 2.1). El acceso queda en la bitácora por ser
        /// información sensible.
        /// </summary>
        public HistoriaClinica_60MN? Consultar(int mascotaId)
        {
            // 1.1: sin permisos sobre datos clínicos se deniega el acceso.
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.HistoriaClinicaConsultar);
            HistoriaClinica_60MN? hc = ObtenerConCabecera(mascotaId);
            if (hc != null)
                bitacora.Registrar(EventoSistema_60MN.HistoriaClinicaConsultada,
                    $"Consulta de la historia clínica {hc.Numero} ({hc.Mascota.Nombre}, propietario {hc.Propietario.NombreCompleto}).");
            return hc;
        }

        /// <summary>Cabecera del paciente para el alta (CUN05 paso 4): mascota y propietario.</summary>
        public (Mascota_60MN Mascota, Cliente_60MN Propietario) DatosParaAlta(int mascotaId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.HistoriaClinicaAlta);
            return MascotaYPropietario(mascotaId);
        }

        public bool TieneHistoriaClinica(int mascotaId) => mapper.ObtenerPorMascota(mascotaId) != null;

        /// <summary>CUN05: abre el legajo de la mascota y genera su número.</summary>
        public HistoriaClinica_60MN Crear(int mascotaId, decimal pesoActual, string alergias, string? observaciones)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.HistoriaClinicaAlta);
            var (mascota, propietario) = MascotaYPropietario(mascotaId);
            if (!propietario.Activo)
                throw new NegocioException_60MN("El cliente está dado de baja: reactívelo antes de abrir la historia clínica.");
            if (!mascota.Activo)
                throw new NegocioException_60MN("La mascota está dada de baja.");
            // 2.1: la mascota ya posee historia clínica.
            if (mapper.ObtenerPorMascota(mascotaId) != null)
                throw new NegocioException_60MN($"{mascota.Nombre} ya tiene historia clínica: se abrirá para consulta.");

            HistoriaClinica_60MN hc = new HistoriaClinica_60MN
            {
                MascotaId = mascotaId,
                FechaApertura = DateTime.Now,
                PesoActual = pesoActual,
                Alergias = NormalizarAlergias(alergias),
                Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim()
            };
            Validar(hc);

            hc.Id = mapper.Insertar(hc, SesionActual_60MN.UsuarioId);
            dv.ActualizarFila(TablaDV_60MN.HistoriaClinica, hc.Id);
            HistoriaClinica_60MN creada = ObtenerConCabecera(mascotaId)!;
            bitacora.Registrar(EventoSistema_60MN.HistoriaClinicaCreada,
                $"Apertura de la historia clínica {creada.Numero} de {mascota.Nombre} (propietario {propietario.NombreCompleto}).");
            return creada;
        }

        /// <summary>
        /// CUN07: actualiza peso, alergias y observaciones; los valores anteriores quedan
        /// en el historial con su fecha. Especie, raza y fecha de nacimiento solo se
        /// cambian con permiso de administrador (flujo 3.1).
        /// </summary>
        public void Actualizar(HistoriaClinica_60MN modificada, Mascota_60MN? datosPaciente)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.HistoriaClinicaModificar);
            HistoriaClinica_60MN actual = mapper.ObtenerPorId(modificada.Id) ?? throw new NegocioException_60MN("La historia clínica ya no existe.");

            modificada.Alergias = NormalizarAlergias(modificada.Alergias);
            modificada.Observaciones = string.IsNullOrWhiteSpace(modificada.Observaciones) ? null : modificada.Observaciones.Trim();
            Validar(modificada);

            List<string> cambios = new List<string>();
            if (actual.PesoActual != modificada.PesoActual) cambios.Add($"peso {actual.PesoActual:0.##} → {modificada.PesoActual:0.##} kg");
            if (actual.Alergias != modificada.Alergias) cambios.Add($"alergias: {modificada.Alergias}");
            if (actual.Observaciones != modificada.Observaciones) cambios.Add("observaciones");

            Mascota_60MN mascota = clientes.ObtenerMascota(actual.MascotaId)!;
            string especie = Validaciones_60MN.Normalizar(datosPaciente?.Especie);
            string raza = Validaciones_60MN.Normalizar(datosPaciente?.Raza);
            bool cambiaPaciente = datosPaciente != null &&
                (especie != mascota.Especie || raza != mascota.Raza ||
                 datosPaciente.FechaNacimiento.Date != mascota.FechaNacimiento.Date);
            if (cambiaPaciente)
            {
                if (!SessionManager_60MN.Instancia.TienePermiso(CodigosPatente_60MN.Administrador))
                    throw new NegocioException_60MN("La especie, la raza y la fecha de nacimiento solo pueden modificarse con permiso de administrador.");
                // Mismas reglas que el alta y la modificación de la mascota (CUN04): obligatorios, largos, caracteres y fechas.
                ClienteBLL_60MN.ValidarMascota(new Mascota_60MN
                {
                    Nombre = mascota.Nombre,
                    Sexo = mascota.Sexo,
                    Especie = especie,
                    Raza = raza,
                    FechaNacimiento = datosPaciente!.FechaNacimiento.Date
                });
                cambios.Add("datos del paciente");
            }
            if (cambios.Count == 0)
                throw new NegocioException_60MN("No hay cambios para guardar.");

            mapper.Actualizar(modificada, "MODIFICACION", SesionActual_60MN.UsuarioId);
            dv.ActualizarFila(TablaDV_60MN.HistoriaClinica, modificada.Id);
            if (cambiaPaciente)
            {
                mascota.Especie = especie;
                mascota.Raza = raza;
                mascota.FechaNacimiento = datosPaciente!.FechaNacimiento.Date;
                clientes.ModificarMascota(mascota);
                dv.ActualizarFila(TablaDV_60MN.Mascota, mascota.Id);
            }
            bitacora.Registrar(EventoSistema_60MN.HistoriaClinicaModificada,
                $"Actualización de la historia clínica {actual.Numero}: {string.Join("; ", cambios)}.");
        }

        public List<HistoriaClinicaHistorial_60MN> ListarHistorial(int historiaClinicaId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.HistoriaClinicaConsultar);
            return mapper.ListarHistorial(historiaClinicaId);
        }

        /// <summary>CUN06 paso 4: atenciones en orden cronológico (la más reciente primero), con su medicación.</summary>
        public List<Atencion_60MN> ListarAtenciones(int historiaClinicaId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.HistoriaClinicaConsultar);
            return new AtencionDAL_60MN().ListarPorHistoria(historiaClinicaId);
        }

        internal HistoriaClinica_60MN? ObtenerConCabecera(int mascotaId)
        {
            HistoriaClinica_60MN? hc = mapper.ObtenerPorMascota(mascotaId);
            if (hc == null) return null;
            var (mascota, propietario) = MascotaYPropietario(mascotaId);
            hc.Mascota = mascota;
            hc.Propietario = propietario;
            return hc;
        }

        private (Mascota_60MN, Cliente_60MN) MascotaYPropietario(int mascotaId)
        {
            Mascota_60MN mascota = clientes.ObtenerMascota(mascotaId) ?? throw new NegocioException_60MN("La mascota no existe.");
            // CUN05 1.1: el cliente debe estar registrado.
            Cliente_60MN propietario = clientes.ObtenerPorId(mascota.ClienteId) ?? throw new NegocioException_60MN("El cliente no está registrado.");
            return (mascota, propietario);
        }

        private static string NormalizarAlergias(string alergias) =>
            string.IsNullOrWhiteSpace(alergias) ? string.Empty : alergias.Trim();

        private static void Validar(HistoriaClinica_60MN hc)
        {
            if (hc.PesoActual <= 0 || hc.PesoActual > 200)
                throw new NegocioException_60MN("El peso debe ser mayor a 0 y menor a 200 kg.");
            // CUN05 5.2: si no se conocen alergias se elige "Sin alergias registradas".
            if (hc.Alergias.Length == 0)
                throw new NegocioException_60MN($"Indique las alergias o marque \"{HistoriaClinica_60MN.SinAlergias}\".");
        }
    }
}
