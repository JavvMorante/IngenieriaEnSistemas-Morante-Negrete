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

    /// <summary>
    /// CUN01 5.2 / CUN02 5.2: uno o más datos del cliente son inválidos. Errores va por
    /// campo (nombre de la propiedad de Cliente_60MN → motivo) para marcarlos en la GUI.
    /// </summary>
    public class DatosClienteInvalidosException_60MN : NegocioException_60MN
    {
        public IReadOnlyDictionary<string, string> Errores { get; }

        public DatosClienteInvalidosException_60MN(IReadOnlyDictionary<string, string> errores)
            : base("Revise los datos marcados:\n" + string.Join("\n", errores.Values))
        {
            Errores = errores;
        }
    }

    /// <summary>
    /// Regla de formato del número según el tipo de documento. La GUI la usa para el
    /// filtro de teclado y el largo del campo; la BLL, para validar antes de guardar.
    /// </summary>
    public sealed class ReglaDocumento_60MN
    {
        public string Tipo { get; }
        public int LargoMinimo { get; }
        public int LargoMaximo { get; }
        /// <summary>Pasaporte: admite letras además de números.</summary>
        public bool AdmiteLetras { get; }
        /// <summary>CUIL: se puede tipear con guiones (20-30111222-0); se guardan solo los dígitos.</summary>
        public bool AdmiteGuiones { get; }
        /// <summary>Largo del campo de la pantalla, contando los guiones del CUIL.</summary>
        public int LargoTipeo => AdmiteGuiones ? LargoMaximo + 2 : LargoMaximo;
        public string Ayuda { get; }

        internal ReglaDocumento_60MN(string tipo, int minimo, int maximo, bool letras, bool guiones, string ayuda)
        {
            Tipo = tipo; LargoMinimo = minimo; LargoMaximo = maximo; AdmiteLetras = letras; AdmiteGuiones = guiones; Ayuda = ayuda;
        }
    }

    /// <summary>Gestión de Pacientes: CUN01 Registrar, CUN02 Modificar, CUN03 Consultar Cliente y CUN04 Gestionar Mascota.</summary>
    public class ClienteBLL_60MN
    {
        public static readonly string[] TiposDocumento = { "DNI", "CUIL", "LE", "LC", "CI", "PAS" };

        /// <summary>
        /// Formato del número por tipo: DNI, LE y LC son de 7 u 8 dígitos; el CUIL tiene
        /// 11 dígitos con dígito verificador; la cédula, de 6 a 12 dígitos; el pasaporte
        /// combina letras y números (el argentino es AAA123456).
        /// </summary>
        private static readonly Dictionary<string, ReglaDocumento_60MN> ReglasDocumento = new[]
        {
            new ReglaDocumento_60MN("DNI", 7, 8, false, false, "7 u 8 dígitos, sin puntos"),
            new ReglaDocumento_60MN("CUIL", 11, 11, false, true, "11 dígitos, con o sin guiones (20-30111222-0)"),
            new ReglaDocumento_60MN("LE", 7, 8, false, false, "7 u 8 dígitos"),
            new ReglaDocumento_60MN("LC", 7, 8, false, false, "7 u 8 dígitos"),
            new ReglaDocumento_60MN("CI", 6, 12, false, false, "6 a 12 dígitos"),
            new ReglaDocumento_60MN("PAS", 6, 12, true, false, "6 a 12 letras y números (ej. AAA123456)")
        }.ToDictionary(r => r.Tipo);

        public static ReglaDocumento_60MN ReglaDocumento(string tipo) =>
            ReglasDocumento.TryGetValue((tipo ?? string.Empty).Trim().ToUpperInvariant(), out ReglaDocumento_60MN? regla)
                ? regla : ReglasDocumento["DNI"];

        // Largos máximos de los datos del cliente (la GUI los usa como MaxLength). Son
        // menores o iguales que las columnas de la tabla Cliente.
        public const int LargoNombre = 40;
        public const int LargoApellido = 40;
        public const int LargoDireccion = 60;
        public const int LargoTelefono = 20;
        public const int LargoMinimoTelefono = 6;
        public const int LargoEmail = 150;

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

        /// <summary>
        /// Búsqueda de mascotas con su dueño: la usan el selector de paciente (PACIENTES >
        /// Historias clínicas / Registrar atención) y la gestión de mascotas. El documento se
        /// acepta con puntos o guiones, como en el alta del cliente.
        /// </summary>
        public List<Mascota_60MN> BuscarMascotas(CriterioBusquedaMascota_60MN criterio, string valor, bool incluirBajas)
        {
            SesionActual_60MN.RequiereAlgunPermiso(CodigosPatente_60MN.ClienteConsultar, CodigosPatente_60MN.Mascota);
            valor = criterio == CriterioBusquedaMascota_60MN.Documento ? SinSeparadores(valor) : (valor ?? string.Empty).Trim();
            if (valor.Length == 0)
                throw new NegocioException_60MN("Ingrese el valor a buscar.");

            List<Mascota_60MN> mascotas = mapper.BuscarMascotas(criterio, valor, incluirBajas);
            bitacora.Registrar(EventoSistema_60MN.ClientesConsultados,
                $"Búsqueda de mascotas por {criterio}: \"{valor}\" ({mascotas.Count} resultado(s)).");
            return mascotas;
        }

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
            // Documento y teléfono se guardan solo con sus dígitos: se quitan los separadores
            // habituales ("30.111.222", "11 4244-1020"); cualquier otro carácter queda y se rechaza.
            c.NroDocumento = SinSeparadores(c.NroDocumento).ToUpperInvariant();
            c.Nombre = Validaciones_60MN.Normalizar(c.Nombre);
            c.Apellido = Validaciones_60MN.Normalizar(c.Apellido);
            c.Direccion = Validaciones_60MN.Normalizar(c.Direccion);
            c.Telefono = SinSeparadores(c.Telefono);
            c.Email = string.IsNullOrWhiteSpace(c.Email) ? null : c.Email!.Trim();
        }

        private static string SinSeparadores(string? valor) => Validaciones_60MN.SinSeparadores(valor);

        /// <summary>
        /// CUN01 5.2 / CUN02 5.2: valida los datos del cliente sin guardarlos y devuelve los
        /// errores por campo (vacío si todo es correcto). La GUI lo usa para marcar cada
        /// campo al salir de él; Registrar y Modificar aplican las mismas reglas.
        /// </summary>
        public IReadOnlyDictionary<string, string> ValidarDatos(Cliente_60MN cliente, bool esAlta)
        {
            Cliente_60MN copia = new Cliente_60MN
            {
                TipoDocumento = cliente.TipoDocumento, NroDocumento = cliente.NroDocumento, Nombre = cliente.Nombre,
                Apellido = cliente.Apellido, Direccion = cliente.Direccion, Telefono = cliente.Telefono, Email = cliente.Email
            };
            Normalizar(copia);
            return ErroresCliente(copia, esAlta);
        }

        private static void ValidarCliente(Cliente_60MN c, bool esAlta)
        {
            Dictionary<string, string> errores = ErroresCliente(c, esAlta);
            if (errores.Count > 0)
                throw new DatosClienteInvalidosException_60MN(errores);
        }

        private static Dictionary<string, string> ErroresCliente(Cliente_60MN c, bool esAlta)
        {
            Dictionary<string, string> errores = new Dictionary<string, string>();
            void Revisar(string campo, string valor, bool obligatorio, Action regla)
            {
                if (valor.Length == 0)
                {
                    if (obligatorio) errores[campo] = "Campo obligatorio.";
                    return;
                }
                try { regla(); }
                catch (NegocioException_60MN ex) { errores[campo] = ex.Message; }
            }

            if (!TiposDocumento.Contains(c.TipoDocumento))
                errores[nameof(Cliente_60MN.TipoDocumento)] = "El tipo de documento no es válido.";

            // En una modificación el documento no se edita (CUN02 5.1) y no se vuelve a validar.
            if (esAlta)
                Revisar(nameof(Cliente_60MN.NroDocumento), c.NroDocumento, true, () => ValidarNroDocumento(c.TipoDocumento, c.NroDocumento));

            Revisar(nameof(Cliente_60MN.Nombre), c.Nombre, true, () =>
            {
                Validaciones_60MN.Largo(c.Nombre, LargoNombre, "El nombre");
                Validaciones_60MN.NombrePropio(c.Nombre, "El nombre");
            });
            Revisar(nameof(Cliente_60MN.Apellido), c.Apellido, true, () =>
            {
                Validaciones_60MN.Largo(c.Apellido, LargoApellido, "El apellido");
                Validaciones_60MN.NombrePropio(c.Apellido, "El apellido");
            });
            Revisar(nameof(Cliente_60MN.Direccion), c.Direccion, true, () =>
            {
                Validaciones_60MN.Largo(c.Direccion, LargoDireccion, "La dirección");
                Validaciones_60MN.Direccion(c.Direccion, "La dirección");
            });
            Revisar(nameof(Cliente_60MN.Telefono), c.Telefono, true, () =>
                Validaciones_60MN.TelefonoDigitos(c.Telefono, LargoMinimoTelefono, LargoTelefono, "El teléfono"));
            Revisar(nameof(Cliente_60MN.Email), c.Email ?? string.Empty, false, () =>
            {
                Validaciones_60MN.Largo(c.Email, LargoEmail, "El mail");
                if (!Validaciones_60MN.EsEmailValido(c.Email))
                    throw new NegocioException_60MN("El formato del mail es inválido (ejemplo: nombre@dominio.com).");
            });
            return errores;
        }

        /// <summary>Número de documento según su tipo (ver ReglasDocumento); el CUIL además verifica su último dígito.</summary>
        private static void ValidarNroDocumento(string tipo, string numero)
        {
            ReglaDocumento_60MN regla = ReglaDocumento(tipo);
            string nombre = tipo == "PAS" ? "El pasaporte" : tipo == "CUIL" ? "El CUIL" : $"El {tipo}";
            if (regla.AdmiteLetras)
            {
                if (!numero.All(ch => (ch >= 'A' && ch <= 'Z') || char.IsDigit(ch)))
                    throw new NegocioException_60MN($"{nombre} solo puede contener letras (sin ñ ni acentos) y números.");
                if (!numero.Any(char.IsDigit))
                    throw new NegocioException_60MN($"{nombre} debe incluir números.");
            }
            else if (!numero.All(char.IsDigit))
                throw new NegocioException_60MN($"{nombre} solo puede contener números.");

            if (numero.Length < regla.LargoMinimo || numero.Length > regla.LargoMaximo)
                throw new NegocioException_60MN($"{nombre} no tiene un largo válido: {regla.Ayuda}.");

            if (tipo == "CUIL")
            {
                if (!PrefijosCuil.Contains(numero.Substring(0, 2)))
                    throw new NegocioException_60MN("El CUIL debe empezar con 20, 23, 24 o 27.");
                if (!Validaciones_60MN.CuitConDigitoValido(numero))
                    throw new NegocioException_60MN("El CUIL no es válido: el dígito verificador (el último) no corresponde.");
            }
        }

        /// <summary>Prefijos del CUIL de personas: 20 varón, 27 mujer, 23 y 24 los asigna ANSES cuando el número se repite.</summary>
        private static readonly string[] PrefijosCuil = { "20", "23", "24", "27" };

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
