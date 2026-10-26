using System.Net.Mail;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>CUN01 5.1: ya existe un cliente con ese tipo y número de documento.</summary>
    public class ClienteDuplicadoException_60MN : NegocioException_60MN
    {
        public Cliente_60MN Existente { get; }

        public ClienteDuplicadoException_60MN(Cliente_60MN existente)
            : base($"Ya existe el cliente {existente.NombreCompleto} con {existente.Documento}.")
        {
            Existente = existente;
        }
    }

    /// <summary>Gestión de Pacientes: CUN01 Registrar, CUN02 Modificar, CUN03 Consultar Cliente y CUN04 Gestionar Mascota.</summary>
    public class ClienteBLL_60MN
    {
        public static readonly string[] TiposDocumento = { "DNI", "LE", "LC", "CI", "PAS" };

        private readonly ClienteDAL_60MN mapper = new ClienteDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        // =====================================================================
        // CUN03 - Consultar Cliente
        // =====================================================================

        public List<Cliente_60MN> Buscar(CriterioBusquedaCliente_60MN criterio, string valor, bool incluirBajas)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ClienteConsultar);
            valor = (valor ?? string.Empty).Trim();
            if (valor.Length == 0)
                throw new NegocioException_60MN("Ingrese el valor a buscar.");

            List<Cliente_60MN> clientes = mapper.Buscar(criterio, valor, incluirBajas);
            bitacora.Registrar(EventoSistema_60MN.ClientesConsultados,
                $"Búsqueda de clientes por {criterio}: \"{valor}\" ({clientes.Count} resultado(s)).");
            return clientes;
        }

        /// <summary>Paso 6: ficha del cliente con sus mascotas.</summary>
        public Cliente_60MN? ObtenerCliente(int clienteId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ClienteConsultar);
            return mapper.ObtenerPorId(clienteId);
        }

        // =====================================================================
        // CUN01 - Registrar Cliente («include» CUN04 para cada mascota)
        // =====================================================================

        public void RegistrarCliente(Cliente_60MN cliente, IList<Mascota_60MN> mascotas)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ClienteAlta);
            Normalizar(cliente);
            ValidarCliente(cliente, esAlta: true);

            // 5.1: documento duplicado.
            Cliente_60MN? existente = mapper.ObtenerPorDocumento(cliente.TipoDocumento, cliente.NroDocumento);
            if (existente != null)
                throw new ClienteDuplicadoException_60MN(existente);

            foreach (Mascota_60MN mascota in mascotas)
            {
                NormalizarMascota(mascota);
                ValidarMascota(mascota);
            }
            if (mascotas.GroupBy(m => m.Nombre, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1))
                throw new NegocioException_60MN("Hay dos mascotas con el mismo nombre en la carga.");

            mapper.Insertar(cliente, mascotas);
            dv.ActualizarFila(TablaDV_60MN.Cliente, cliente.Id);
            foreach (Mascota_60MN mascota in mascotas)
                dv.ActualizarFila(TablaDV_60MN.Mascota, mascota.Id);

            string detalle = mascotas.Count == 0 ? "sin mascotas" : "mascotas: " + string.Join(", ", mascotas.Select(m => m.Nombre));
            bitacora.Registrar(EventoSistema_60MN.ClienteRegistrado,
                $"Alta del cliente {cliente.NombreCompleto} ({cliente.Documento}), {detalle}.");
        }

        // =====================================================================
        // CUN02 - Modificar Cliente
        // =====================================================================

        public void ModificarCliente(Cliente_60MN modificado)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ClienteModificar);
            Normalizar(modificado);
            Cliente_60MN actual = mapper.ObtenerPorId(modificado.Id) ?? throw new NegocioException_60MN("El cliente ya no existe.");

            // 2.1: cliente dado de baja.
            if (!actual.Activo)
                throw new NegocioException_60MN("El cliente está dado de baja: reactívelo antes de modificarlo.");
            // 5.1: el documento es la clave identificatoria.
            if (actual.TipoDocumento != modificado.TipoDocumento || actual.NroDocumento != modificado.NroDocumento)
                throw new NegocioException_60MN("El tipo y número de documento no pueden modificarse.");
            ValidarCliente(modificado, esAlta: false);

            mapper.Modificar(modificado);
            dv.ActualizarFila(TablaDV_60MN.Cliente, modificado.Id);
            bitacora.Registrar(EventoSistema_60MN.ClienteModificado, $"Modificación del cliente {modificado.NombreCompleto} ({actual.Documento}).");
        }

        /// <summary>Baja lógica o reactivación del cliente (CUN02 2.1 / CUN03 5.1). Mascotas e historias quedan intactas.</summary>
        public void CambiarEstadoCliente(int clienteId, bool activo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.ClienteModificar);
            Cliente_60MN cliente = mapper.ObtenerPorId(clienteId) ?? throw new NegocioException_60MN("El cliente ya no existe.");
            if (cliente.Activo == activo)
                throw new NegocioException_60MN(activo ? "El cliente ya está activo." : "El cliente ya está dado de baja.");

            mapper.CambiarActivoCliente(clienteId, activo);
            dv.ActualizarFila(TablaDV_60MN.Cliente, clienteId);
            bitacora.Registrar(activo ? EventoSistema_60MN.ClienteReactivado : EventoSistema_60MN.ClienteBaja,
                $"{(activo ? "Reactivación" : "Baja lógica")} del cliente {cliente.NombreCompleto} ({cliente.Documento}).");
        }

        // =====================================================================
        // CUN04 - Gestionar Mascota
        // =====================================================================

        /// <summary>Flujo 5.1: la GUI lo consulta para pedir confirmación si el nombre ya existe para ese cliente.</summary>
        public bool ExisteMascotaConNombre(int clienteId, string nombre, int excluirId = 0) =>
            mapper.ExisteMascotaConNombre(clienteId, (nombre ?? string.Empty).Trim(), excluirId);

        public void RegistrarMascota(Mascota_60MN mascota)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Mascota);
            NormalizarMascota(mascota);
            ValidarMascota(mascota);
            Cliente_60MN cliente = ClienteActivo(mascota.ClienteId);

            mascota.Id = mapper.InsertarMascota(mascota);
            dv.ActualizarFila(TablaDV_60MN.Mascota, mascota.Id);
            bitacora.Registrar(EventoSistema_60MN.MascotaRegistrada,
                $"Alta de la mascota {mascota.Nombre} ({mascota.Especie}) del cliente {cliente.NombreCompleto}.");
        }

        public void ModificarMascota(Mascota_60MN mascota)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Mascota);
            NormalizarMascota(mascota);
            ValidarMascota(mascota);
            Mascota_60MN actual = mapper.ObtenerMascota(mascota.Id) ?? throw new NegocioException_60MN("La mascota ya no existe.");
            if (!actual.Activo)
                throw new NegocioException_60MN("La mascota está dada de baja: no se puede modificar.");
            ClienteActivo(actual.ClienteId);

            mapper.ModificarMascota(mascota);
            dv.ActualizarFila(TablaDV_60MN.Mascota, mascota.Id);
            bitacora.Registrar(EventoSistema_60MN.MascotaModificada, $"Modificación de la mascota {mascota.Nombre} (Id {mascota.Id}).");
        }

        /// <summary>Flujo 3.1: baja lógica; si tiene historia clínica, el historial de atenciones se conserva íntegro.</summary>
        public void CambiarEstadoMascota(int mascotaId, bool activo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Mascota);
            Mascota_60MN mascota = mapper.ObtenerMascota(mascotaId) ?? throw new NegocioException_60MN("La mascota ya no existe.");
            if (mascota.Activo == activo)
                throw new NegocioException_60MN(activo ? "La mascota ya está activa." : "La mascota ya está dada de baja.");
            if (activo)
                ClienteActivo(mascota.ClienteId);

            mapper.CambiarActivoMascota(mascotaId, activo);
            dv.ActualizarFila(TablaDV_60MN.Mascota, mascotaId);
            bitacora.Registrar(activo ? EventoSistema_60MN.MascotaModificada : EventoSistema_60MN.MascotaBaja,
                $"{(activo ? "Reactivación" : "Baja lógica")} de la mascota {mascota.Nombre}" +
                (mascota.TieneHistoriaClinica ? $" (conserva la historia clínica {mascota.NumeroHistoriaClinica})." : "."));
        }

        // =====================================================================
        // Reglas comunes
        // =====================================================================

        private Cliente_60MN ClienteActivo(int clienteId)
        {
            Cliente_60MN cliente = mapper.ObtenerPorId(clienteId) ?? throw new NegocioException_60MN("El cliente no existe.");
            if (!cliente.Activo)
                throw new NegocioException_60MN("El cliente está dado de baja: reactívelo antes de operar con sus mascotas.");
            return cliente;
        }

        // ---------------------------------------------------------------------
        // Validaciones (los largos máximos son los de las columnas de Cliente y Mascota)
        // ---------------------------------------------------------------------

        /// <summary>Edad máxima admitida para una mascota (contempla especies longevas como tortugas y loros).</summary>
        public const int EdadMaximaMascotaAnios = 100;

        private static void Normalizar(Cliente_60MN c)
        {
            c.TipoDocumento = (c.TipoDocumento ?? string.Empty).Trim().ToUpperInvariant();
            // El documento se acepta con puntos o espacios ("30.111.222") y se guarda solo con sus caracteres.
            c.NroDocumento = new string((c.NroDocumento ?? string.Empty).Where(ch => ch != '.' && !char.IsWhiteSpace(ch)).ToArray()).ToUpperInvariant();
            c.Nombre = Validaciones_60MN.Normalizar(c.Nombre);
            c.Apellido = Validaciones_60MN.Normalizar(c.Apellido);
            c.Direccion = Validaciones_60MN.Normalizar(c.Direccion);
            c.Telefono = Validaciones_60MN.Normalizar(c.Telefono);
            c.Email = string.IsNullOrWhiteSpace(c.Email) ? null : c.Email!.Trim();
        }

        private static void ValidarCliente(Cliente_60MN c, bool esAlta)
        {
            // CUN01 5.2 / CUN02 5.2: campos obligatorios.
            if (c.Nombre.Length == 0 || c.Apellido.Length == 0 || c.Direccion.Length == 0 || c.Telefono.Length == 0 ||
                (esAlta && c.NroDocumento.Length == 0))
                throw new NegocioException_60MN("Complete todos los campos obligatorios.");
            if (!TiposDocumento.Contains(c.TipoDocumento))
                throw new NegocioException_60MN("El tipo de documento no es válido.");
            if (c.TipoDocumento == "PAS"
                    ? !c.NroDocumento.All(char.IsLetterOrDigit) || c.NroDocumento.Length < 6 || c.NroDocumento.Length > 15
                    : !c.NroDocumento.All(char.IsDigit) || c.NroDocumento.Length < 7 || c.NroDocumento.Length > 8)
                throw new NegocioException_60MN("El número de documento no es válido (7 u 8 dígitos; pasaporte: 6 a 15 letras o números).");

            Validaciones_60MN.Largo(c.Nombre, 80, "El nombre");
            Validaciones_60MN.Largo(c.Apellido, 80, "El apellido");
            Validaciones_60MN.Largo(c.Direccion, 150, "La dirección");
            Validaciones_60MN.Largo(c.Telefono, 30, "El teléfono");
            Validaciones_60MN.Largo(c.Email, 150, "El mail");
            Validaciones_60MN.NombrePropio(c.Nombre, "El nombre");
            Validaciones_60MN.NombrePropio(c.Apellido, "El apellido");
            Validaciones_60MN.Telefono(c.Telefono, "El teléfono");
            if (c.Email != null && !Validaciones_60MN.EsEmailValido(c.Email))
                throw new NegocioException_60MN("El formato del mail es inválido (ejemplo: nombre@dominio.com).");
        }

        private static void NormalizarMascota(Mascota_60MN m)
        {
            m.Nombre = Validaciones_60MN.Normalizar(m.Nombre);
            m.Especie = Validaciones_60MN.Normalizar(m.Especie);
            m.Raza = Validaciones_60MN.Normalizar(m.Raza);
            m.Sexo = (m.Sexo ?? string.Empty).Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Reglas de la mascota (CUN04). También las usa la historia clínica (CUN07) cuando
        /// el administrador corrige especie, raza o fecha de nacimiento.
        /// </summary>
        internal static void ValidarMascota(Mascota_60MN m)
        {
            if (m.Nombre.Length == 0 || m.Especie.Length == 0 || m.Raza.Length == 0)
                throw new NegocioException_60MN("Complete nombre, especie y raza de la mascota.");
            if (m.Sexo != "M" && m.Sexo != "H")
                throw new NegocioException_60MN("Indique el sexo de la mascota.");

            Validaciones_60MN.Largo(m.Nombre, 60, "El nombre de la mascota");
            Validaciones_60MN.Largo(m.Especie, 40, "La especie");
            Validaciones_60MN.Largo(m.Raza, 60, "La raza");
            Validaciones_60MN.TextoSimple(m.Nombre, "El nombre de la mascota");
            Validaciones_60MN.NombrePropio(m.Especie, "La especie");
            Validaciones_60MN.TextoSimple(m.Raza, "La raza");

            // CUN04 5.2: fecha de nacimiento futura; y una fecha mínima razonable.
            if (m.FechaNacimiento.Date > DateTime.Today)
                throw new NegocioException_60MN("La fecha de nacimiento no puede ser posterior a la fecha actual.");
            if (m.FechaNacimiento.Date < DateTime.Today.AddYears(-EdadMaximaMascotaAnios))
                throw new NegocioException_60MN($"La fecha de nacimiento no es válida: la mascota tendría más de {EdadMaximaMascotaAnios} años.");
        }
    }
}
