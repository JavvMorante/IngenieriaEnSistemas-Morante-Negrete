using System.Net.Mail;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Criptografia;
using Seguridad_60MN.Emergencia;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>
    /// Gestión de usuarios y seguridad de acceso: CUS01, CUS02, CUS03, CUS05,
    /// CUS06, CUS07 y CUS08. DNI y Email se cifran con AES antes de llegar a
    /// la DAL; la contraseña se guarda con hash SHA-256 + sal.
    /// </summary>
    public class UsuarioBLL_60MN
    {
        public const int MaximoIntentos = 3;

        // Intentos fallidos del usuario de emergencia en este proceso (no tiene fila en la base donde contarlos).
        private static int intentosFallidosEmergencia;

        private readonly UsuarioDAL_60MN mapper = new UsuarioDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        private static SessionManager_60MN Sesion => SessionManager_60MN.Instancia;

        // =====================================================================
        // CUS05 - Iniciar sesión
        // =====================================================================

        public Usuario_60MN Login(string nombreUsuario, string clave)
        {
            if (Sesion.EstaLogueado)
                throw new LoginException_60MN(ResultadoLogin_60MN.SesionYaIniciada, "Ya existe una sesión iniciada.");

            nombreUsuario = (nombreUsuario ?? string.Empty).Trim();
            if (nombreUsuario.Length == 0 || string.IsNullOrEmpty(clave))
                throw new LoginException_60MN(ResultadoLogin_60MN.CamposVacios, "Debe ingresar usuario y contraseña.");

            UsuarioEmergencia_60MN? emergencia = CargarUsuarioEmergencia();
            if (emergencia != null && emergencia.EsUsuario(nombreUsuario))
                return LoginEmergencia(emergencia, clave);

            Usuario_60MN? usuario;
            try
            {
                usuario = mapper.ObtenerPorNombreUsuario(nombreUsuario);
            }
            catch (Exception ex)
            {
                throw new LoginException_60MN(ResultadoLogin_60MN.ErrorBaseDeDatos, "No se pudo acceder a la base de datos: " + ex.Message);
            }

            // 5.2: el usuario no existe.
            if (usuario == null)
            {
                bitacora.RegistrarSinSesion(null, nombreUsuario, EventoSistema_60MN.LoginFallido, "Intento de ingreso con un usuario inexistente.");
                throw new LoginException_60MN(ResultadoLogin_60MN.UsuarioInexistente,
                    "El usuario no existe o está bloqueado. Contáctese con un administrador.");
            }

            // El registro del usuario fue modificado por fuera del sistema (DVH inválido).
            if (!dv.VerificarFila(TablaDV_60MN.Usuario, usuario.Id))
            {
                bitacora.RegistrarSinSesion(usuario.Id, usuario.NombreUsuario, EventoSistema_60MN.RegistroAlterado,
                    "Se rechazó el ingreso: el registro del usuario no coincide con su dígito verificador.");
                throw new LoginException_60MN(ResultadoLogin_60MN.RegistroAlterado,
                    "Los datos de su usuario presentan inconsistencias de integridad. Contáctese con un administrador.");
            }

            // 5.3: usuario dado de baja. Se deniega sin revelar el estado del registro.
            if (!usuario.Activo)
            {
                bitacora.RegistrarSinSesion(usuario.Id, usuario.NombreUsuario, EventoSistema_60MN.LoginFallido, "Intento de ingreso de un usuario dado de baja.");
                throw new LoginException_60MN(ResultadoLogin_60MN.CredencialesInvalidas, "Usuario o contraseña incorrectos.");
            }

            // 5.2: usuario bloqueado.
            if (usuario.Bloqueado)
            {
                bitacora.RegistrarSinSesion(usuario.Id, usuario.NombreUsuario, EventoSistema_60MN.LoginFallido, "Intento de ingreso de un usuario bloqueado.");
                throw new LoginException_60MN(ResultadoLogin_60MN.UsuarioBloqueado,
                    "El usuario está bloqueado. Contáctese con un administrador.");
            }

            // Pasos 4 y 5: hash SHA-256 de la clave ingresada contra el almacenado. 5.1: credenciales incorrectas.
            if (!PasswordHasher_60MN.Verificar(clave, usuario.ClaveHash))
            {
                usuario.IntentosFallidos++;
                usuario.Bloqueado = usuario.IntentosFallidos >= MaximoIntentos;
                GuardarEstado(usuario);

                if (usuario.Bloqueado)
                {
                    bitacora.RegistrarSinSesion(usuario.Id, usuario.NombreUsuario, EventoSistema_60MN.UsuarioBloqueado,
                        $"Usuario bloqueado por superar {MaximoIntentos} intentos fallidos de ingreso.");
                    throw new LoginException_60MN(ResultadoLogin_60MN.UsuarioBloqueado,
                        $"Contraseña incorrecta. Se superaron los {MaximoIntentos} intentos y el usuario fue bloqueado. Contáctese con un administrador.");
                }

                bitacora.RegistrarSinSesion(usuario.Id, usuario.NombreUsuario, EventoSistema_60MN.LoginFallido,
                    $"Contraseña incorrecta (intento {usuario.IntentosFallidos} de {MaximoIntentos}).");
                throw new LoginException_60MN(ResultadoLogin_60MN.CredencialesInvalidas,
                    $"Usuario o contraseña incorrectos. Intentos restantes: {MaximoIntentos - usuario.IntentosFallidos}.");
            }

            // Carga de permisos (Composite): familias del usuario + patentes individuales.
            CargarPermisos(usuario, ModeloPermisos_60MN.Cargar());
            List<InconsistenciaDV_60MN> inconsistencias = new IntegridadBLL_60MN().VerificarInterno();
            bool integridadComprometida = inconsistencias.Count > 0;
            if (integridadComprometida && !usuario.Permisos.Any(p => p.TienePatente(CodigosPatente_60MN.Integridad)))
            {
                bitacora.RegistrarSinSesion(usuario.Id, usuario.NombreUsuario, EventoSistema_60MN.AccesoDenegadoIntegridad,
                    $"Ingreso rechazado: la base presenta {inconsistencias.Count} inconsistencia(s) de dígitos verificadores.");
                throw new LoginException_60MN(ResultadoLogin_60MN.IntegridadComprometida,
                    "La base de datos presenta inconsistencias de integridad. Contáctese con un administrador.");
            }

            // Paso 6: contador de intentos en cero.
            usuario.IntentosFallidos = 0;
            usuario.EnSesion = true;
            GuardarEstado(usuario);

            // Paso 7: carga de datos del usuario y registro de la sesión en el SessionManager.
            Descifrar(usuario);
            Sesion.Login(usuario);
            Sesion.IntegridadComprometida = integridadComprometida;

            bitacora.Registrar(EventoSistema_60MN.Login, "Inicio de sesión exitoso.");
            if (integridadComprometida)
                bitacora.Registrar(EventoSistema_60MN.IntegridadFallida,
                    $"Al iniciar sesión se detectaron {inconsistencias.Count} inconsistencia(s) de dígitos verificadores.");
            return usuario;
        }

        private Usuario_60MN LoginEmergencia(UsuarioEmergencia_60MN emergencia, string clave)
        {
            if (intentosFallidosEmergencia >= MaximoIntentos)
                throw new LoginException_60MN(ResultadoLogin_60MN.UsuarioBloqueado,
                    "El usuario de emergencia quedó bloqueado por intentos fallidos. Reinicie la aplicación.");

            bool hayAdministrador;
            try
            {
                hayAdministrador = ExisteAdministradorOperativo();
            }
            catch (Exception ex)
            {
                throw new LoginException_60MN(ResultadoLogin_60MN.ErrorBaseDeDatos, "No se pudo acceder a la base de datos: " + ex.Message);
            }

            if (hayAdministrador)
            {
                bitacora.RegistrarSinSesion(null, emergencia.Usuario, EventoSistema_60MN.LoginFallido,
                    "Intento de ingreso con el usuario de emergencia existiendo un administrador operativo.");
                throw new LoginException_60MN(ResultadoLogin_60MN.EmergenciaNoHabilitada,
                    "El usuario de emergencia solo puede utilizarse cuando no existe un administrador activo en el sistema.");
            }

            if (!emergencia.VerificarClave(clave))
            {
                intentosFallidosEmergencia++;
                bitacora.RegistrarSinSesion(null, emergencia.Usuario, EventoSistema_60MN.LoginFallido,
                    $"Contraseña incorrecta del usuario de emergencia (intento {intentosFallidosEmergencia} de {MaximoIntentos}).");
                throw new LoginException_60MN(ResultadoLogin_60MN.CredencialesInvalidas, "Usuario o contraseña incorrectos.");
            }

            intentosFallidosEmergencia = 0;

            // Familia en memoria con todas las patentes: no depende de las tablas de permisos (pueden estar dañadas).
            Familia_60MN familia = new Familia_60MN { Id = -1, Nombre = "Acceso de emergencia" };
            int idPatente = -100;
            foreach (string codigo in CodigosPatente_60MN.Todas)
                familia.AgregarHijo(new Patente_60MN { Id = idPatente--, Nombre = codigo, Codigo = codigo });

            Usuario_60MN usuario = new Usuario_60MN
            {
                Id = 0,
                NombreUsuario = emergencia.Usuario,
                Nombre = emergencia.Nombre,
                Apellido = emergencia.Apellido,
                Familias = familia.Nombre,
                PrimerIngreso = false
            };
            usuario.Permisos.Add(familia);

            Sesion.Login(usuario, esEmergencia: true);
            try
            {
                Sesion.IntegridadComprometida = new IntegridadBLL_60MN().VerificarInterno().Count > 0;
            }
            catch (Exception)
            {
                Sesion.IntegridadComprometida = true;
            }
            bitacora.Registrar(EventoSistema_60MN.LoginEmergencia,
                "Ingreso con el usuario de emergencia (archivo XML): no existía un administrador operativo en la base.");
            return usuario;
        }

        private UsuarioEmergencia_60MN? CargarUsuarioEmergencia()
        {
            try
            {
                return UsuarioEmergencia_60MN.Cargar();
            }
            catch (Exception ex)
            {
                bitacora.RegistrarSinSesion(null, "(sistema)", EventoSistema_60MN.RegistroAlterado,
                    "No se pudo cargar el usuario de emergencia: " + ex.Message);
                return null;
            }
        }

        // =====================================================================
        // CUS06 - Cerrar sesión
        // =====================================================================

        /// <summary>Cierra la sesión. Devuelve false si no se pudo registrar el evento (la sesión se cierra igual, flujo 6.1).</summary>
        public bool Logout()
        {
            if (!Sesion.EstaLogueado) return true;

            bool registrado = bitacora.Registrar(EventoSistema_60MN.Logout, "Cierre de sesión.");
            try
            {
                if (!Sesion.EsEmergencia)
                {
                    Usuario_60MN? usuario = mapper.ObtenerPorId(Sesion.Usuario!.Id);
                    if (usuario != null && usuario.EnSesion)
                    {
                        usuario.EnSesion = false;
                        GuardarEstado(usuario);
                    }
                }
            }
            catch (Exception)
            {
                registrado = false;
            }
            finally
            {
                Sesion.Logout();
            }
            return registrado;
        }

        // =====================================================================
        // Consultar usuarios (paso 2 de CUS01, CUS02, CUS03 y CUS08)
        // =====================================================================

        public List<Usuario_60MN> ListarUsuarios()
        {
            if (!new[] { CodigosPatente_60MN.UsuarioAlta, CodigosPatente_60MN.UsuarioModificar,
                         CodigosPatente_60MN.UsuarioBaja, CodigosPatente_60MN.UsuarioDesbloquear }.Any(Sesion.TienePermiso))
                throw new NegocioException_60MN("No tiene permiso para consultar usuarios.");

            List<Usuario_60MN> usuarios = mapper.ListarTodos();
            ModeloPermisos_60MN modelo = ModeloPermisos_60MN.Cargar();
            foreach (Usuario_60MN usuario in usuarios)
            {
                Descifrar(usuario);
                CargarPermisos(usuario, modelo);
            }
            return usuarios;
        }

        // =====================================================================
        // CUS01 - Crear usuario
        // =====================================================================

        /// <summary>
        /// Crea el usuario con su familia (tipo de usuario) y devuelve la contraseña
        /// inicial generada (se muestra una única vez). Las patentes individuales se
        /// agregan después desde "Permisos" (PermisoBLL_60MN.AsignarPermisosUsuario).
        /// </summary>
        public string CrearUsuario(Usuario_60MN nuevo, int familiaId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.UsuarioAlta);
            Normalizar(nuevo);
            ValidarDatos(nuevo, esAlta: true);
            ModeloPermisos_60MN modelo = ModeloPermisos_60MN.Cargar();
            if (!modelo.Familias.ContainsKey(familiaId))
                throw new NegocioException_60MN("Seleccione la familia (tipo de usuario).");

            if (mapper.ExisteNombreUsuario(nuevo.NombreUsuario))
                throw new NegocioException_60MN("Ya existe un usuario con ese nombre de usuario.");
            if (mapper.ListarTodos().Any(u => DescifrarCampo(u.Dni) == nuevo.Dni))
                throw new NegocioException_60MN("Ya existe un usuario con ese DNI.");

            UsuarioEmergencia_60MN? emergencia = CargarUsuarioEmergencia();
            if (emergencia != null && emergencia.EsUsuario(nuevo.NombreUsuario))
                throw new NegocioException_60MN("Ese nombre de usuario está reservado para el usuario de emergencia.");

            string claveInicial = PoliticaClave_60MN.Generar();
            Usuario_60MN aGuardar = Cifrado(nuevo);
            aGuardar.ClaveHash = PasswordHasher_60MN.Hashear(claveInicial);

            new PermisoBLL_60MN().ExigirTablasIntegras(TablaDV_60MN.UsuarioFamilia, TablaDV_60MN.UsuarioPatente);
            nuevo.Id = mapper.Insertar(aGuardar);
            dv.ActualizarFila(TablaDV_60MN.Usuario, nuevo.Id);
            new PermisoBLL_60MN().GuardarAsignacion(nuevo.Id, new[] { familiaId }, Array.Empty<int>());

            bitacora.Registrar(EventoSistema_60MN.UsuarioCreado,
                $"Alta del usuario \"{nuevo.NombreUsuario}\" ({nuevo.Apellido}, {nuevo.Nombre}) con la familia \"{modelo.Familias[familiaId].Nombre}\".");
            return claveInicial;
        }

        // =====================================================================
        // CUS02 - Modificar usuario
        // =====================================================================

        public void ModificarUsuario(Usuario_60MN modificado)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.UsuarioModificar);
            Normalizar(modificado);
            ValidarDatos(modificado, esAlta: false);

            Usuario_60MN actual = mapper.ObtenerPorId(modificado.Id)
                ?? throw new NegocioException_60MN("El usuario ya no existe.");

            // Las familias y patentes individuales se modifican con PermisoBLL_60MN.AsignarPermisosUsuario.
            actual.Nombre = modificado.Nombre;
            actual.Apellido = modificado.Apellido;
            actual.Email = CriptografiaHandler_60MN.Encriptar(modificado.Email);
            mapper.ModificarDatos(actual);
            dv.ActualizarFila(TablaDV_60MN.Usuario, actual.Id);

            bitacora.Registrar(EventoSistema_60MN.UsuarioModificado,
                $"Modificación de los datos del usuario \"{actual.NombreUsuario}\".");
        }

        // =====================================================================
        // CUS03 - Dar de baja usuario (baja lógica) y reactivación
        // =====================================================================

        public void DarDeBaja(int usuarioId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.UsuarioBaja);
            if (usuarioId == SesionActual_60MN.UsuarioId)
                throw new NegocioException_60MN("No puede darse de baja a sí mismo.");

            Usuario_60MN usuario = mapper.ObtenerPorId(usuarioId) ?? throw new NegocioException_60MN("El usuario ya no existe.");
            if (!usuario.Activo)
                throw new NegocioException_60MN("El usuario ya se encuentra dado de baja.");
            if (!ExisteAdministradorOperativo(simularCambio: u => { if (u.Id == usuarioId) u.Activo = false; }))
                throw new NegocioException_60MN("No se puede dar de baja al único administrador activo.");

            usuario.Activo = false;
            usuario.EnSesion = false;
            GuardarEstado(usuario);
            bitacora.Registrar(EventoSistema_60MN.UsuarioBaja, $"Baja lógica del usuario \"{usuario.NombreUsuario}\".");
        }

        public void Reactivar(int usuarioId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.UsuarioBaja);
            Usuario_60MN usuario = mapper.ObtenerPorId(usuarioId) ?? throw new NegocioException_60MN("El usuario ya no existe.");
            if (usuario.Activo)
                throw new NegocioException_60MN("El usuario ya se encuentra activo.");

            usuario.Activo = true;
            GuardarEstado(usuario);
            bitacora.Registrar(EventoSistema_60MN.UsuarioReactivado, $"Reactivación del usuario \"{usuario.NombreUsuario}\".");
        }

        // =====================================================================
        // CUS08 - Desbloquear usuario
        // =====================================================================

        public void Desbloquear(int usuarioId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.UsuarioDesbloquear);
            Usuario_60MN usuario = mapper.ObtenerPorId(usuarioId) ?? throw new NegocioException_60MN("El usuario ya no existe.");

            if (!usuario.Activo)
                throw new NegocioException_60MN("El usuario fue dado de baja: primero debe reactivarse el registro.");
            if (!usuario.Bloqueado)
                throw new NegocioException_60MN("El usuario seleccionado no se encuentra bloqueado.");

            usuario.Bloqueado = false;
            usuario.IntentosFallidos = 0;
            GuardarEstado(usuario);
            bitacora.Registrar(EventoSistema_60MN.UsuarioDesbloqueado, $"Desbloqueo del usuario \"{usuario.NombreUsuario}\".");
        }

        // =====================================================================
        // CUS07 - Cambiar contraseña / blanqueo por el administrador
        // =====================================================================

        public void CambiarClave(string claveActual, string claveNueva, string confirmacion)
        {
            if (!Sesion.EstaLogueado)
                throw new NegocioException_60MN("No hay una sesión iniciada.");
            if (Sesion.EsEmergencia)
                throw new NegocioException_60MN("La contraseña del usuario de emergencia no se administra desde el sistema.");

            Usuario_60MN usuario = mapper.ObtenerPorId(Sesion.Usuario!.Id)
                ?? throw new NegocioException_60MN("El usuario de la sesión ya no existe.");

            if (!PasswordHasher_60MN.Verificar(claveActual ?? string.Empty, usuario.ClaveHash))
                throw new NegocioException_60MN("La contraseña actual es incorrecta.");
            if (claveNueva != confirmacion)
                throw new NegocioException_60MN("La nueva contraseña y su confirmación no coinciden.");
            string? error = PoliticaClave_60MN.Validar(claveNueva);
            if (error != null)
                throw new NegocioException_60MN(error);
            if (PasswordHasher_60MN.Verificar(claveNueva, usuario.ClaveHash))
                throw new NegocioException_60MN("La nueva contraseña no puede ser igual a la anterior.");

            usuario.ClaveHash = PasswordHasher_60MN.Hashear(claveNueva);
            usuario.PrimerIngreso = false;
            GuardarEstado(usuario);
            if (Sesion.Usuario is Usuario_60MN enSesion)
                enSesion.PrimerIngreso = false;

            bitacora.Registrar(EventoSistema_60MN.ClaveCambiada, "Cambio de contraseña del usuario.");
        }

        /// <summary>Genera una nueva contraseña inicial (el usuario deberá cambiarla al ingresar).</summary>
        public string BlanquearClave(int usuarioId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.UsuarioModificar);
            Usuario_60MN usuario = mapper.ObtenerPorId(usuarioId) ?? throw new NegocioException_60MN("El usuario ya no existe.");

            string clave = PoliticaClave_60MN.Generar();
            usuario.ClaveHash = PasswordHasher_60MN.Hashear(clave);
            usuario.PrimerIngreso = true;
            usuario.IntentosFallidos = 0;
            GuardarEstado(usuario);
            bitacora.Registrar(EventoSistema_60MN.ClaveBlanqueada, $"Blanqueo de contraseña del usuario \"{usuario.NombreUsuario}\".");
            return clave;
        }

        // =====================================================================
        // Reglas comunes
        // =====================================================================

        /// <summary>
        /// Indica si existe al menos un administrador operativo: usuario activo,
        /// no bloqueado, con su registro íntegro (DVH válido) y cuyas familias o
        /// patentes individuales otorgan la patente de administración. Si no existe
        /// ninguno se habilita el usuario de emergencia. Recibe opcionalmente un
        /// modelo de permisos con cambios simulados, y/o una simulación sobre los
        /// datos del usuario, para validar un cambio antes de persistirlo.
        /// </summary>
        public bool ExisteAdministradorOperativo(ModeloPermisos_60MN? modelo = null, Action<Usuario_60MN>? simularCambio = null)
        {
            modelo ??= ModeloPermisos_60MN.Cargar();
            foreach (Usuario_60MN usuario in mapper.ListarTodos())
            {
                if (!dv.VerificarFila(TablaDV_60MN.Usuario, usuario.Id))
                    continue;
                simularCambio?.Invoke(usuario);
                if (usuario.Activo && !usuario.Bloqueado &&
                    modelo.PermisosDe(usuario.Id).Any(p => p.TienePatente(CodigosPatente_60MN.Administrador)))
                    return true;
            }
            return false;
        }

        /// <summary>Carga en el usuario sus familias y patentes individuales (y el texto para la grilla).</summary>
        private static void CargarPermisos(Usuario_60MN usuario, ModeloPermisos_60MN modelo)
        {
            usuario.Permisos.Clear();
            foreach (var permiso in modelo.PermisosDe(usuario.Id))
                usuario.Permisos.Add(permiso);
            usuario.Familias = modelo.NombresFamiliasDe(usuario.Id);
            usuario.PatentesIndividuales = modelo.CantidadPatentesIndividualesDe(usuario.Id);
        }

        private void GuardarEstado(Usuario_60MN usuario)
        {
            mapper.ActualizarEstado(usuario);
            dv.ActualizarFila(TablaDV_60MN.Usuario, usuario.Id);
        }

        private static void Normalizar(Usuario_60MN u)
        {
            u.NombreUsuario = (u.NombreUsuario ?? string.Empty).Trim();
            u.Nombre = (u.Nombre ?? string.Empty).Trim();
            u.Apellido = (u.Apellido ?? string.Empty).Trim();
            u.Dni = (u.Dni ?? string.Empty).Trim();
            u.Email = (u.Email ?? string.Empty).Trim();
        }

        private static void ValidarDatos(Usuario_60MN u, bool esAlta)
        {
            if (u.Apellido.Length == 0 || u.Nombre.Length == 0 || u.Email.Length == 0 ||
                (esAlta && (u.Dni.Length == 0 || u.NombreUsuario.Length == 0)))
                throw new NegocioException_60MN("Complete todos los campos obligatorios.");
            if (esAlta && (!u.Dni.All(char.IsDigit) || u.Dni.Length < 7 || u.Dni.Length > 8))
                throw new NegocioException_60MN("El DNI debe tener 7 u 8 dígitos numéricos.");
            if (esAlta && (u.NombreUsuario.Length < 4 || u.NombreUsuario.Contains(' ')))
                throw new NegocioException_60MN("El nombre de usuario debe tener al menos 4 caracteres y no contener espacios.");
            if (!EsEmailValido(u.Email))
                throw new NegocioException_60MN("El formato del mail es inválido.");
        }

        public static bool EsEmailValido(string email)
        {
            if (!MailAddress.TryCreate(email, out MailAddress? direccion)) return false;
            return direccion.Address == email && direccion.Host.Contains('.');
        }

        private static Usuario_60MN Cifrado(Usuario_60MN u) => new Usuario_60MN
        {
            Id = u.Id,
            NombreUsuario = u.NombreUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Dni = CriptografiaHandler_60MN.Encriptar(u.Dni),
            Email = CriptografiaHandler_60MN.Encriptar(u.Email)
        };

        private static void Descifrar(Usuario_60MN u)
        {
            u.Dni = DescifrarCampo(u.Dni);
            u.Email = DescifrarCampo(u.Email);
        }

        private static string DescifrarCampo(string valor)
        {
            try
            {
                return CriptografiaHandler_60MN.Desencriptar(valor);
            }
            catch (Exception)
            {
                return "(dato ilegible)";
            }
        }
    }
}
