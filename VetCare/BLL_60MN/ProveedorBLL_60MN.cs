using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// CUN11: uno o más datos del proveedor son inválidos. Errores va por campo
    /// (nombre de la propiedad de Proveedor_60MN → motivo) para marcarlos en la GUI.
    /// </summary>
    public class DatosProveedorInvalidosException_60MN : NegocioException_60MN
    {
        public IReadOnlyDictionary<string, string> Errores { get; }

        public DatosProveedorInvalidosException_60MN(IReadOnlyDictionary<string, string> errores)
            : base("Revise los datos marcados:\n" + string.Join("\n", errores.Values))
        {
            Errores = errores;
        }
    }

    /// <summary>CUN11 Gestionar Proveedores: búsqueda, alta, modificación y baja lógica.</summary>
    public class ProveedorBLL_60MN
    {
        // Largos máximos (la GUI los usa como MaxLength); menores o iguales que las columnas de Proveedor.
        public const int LargoRazonSocial = 120;
        public const int LargoDireccion = 150;
        public const int LargoContacto = 80;
        public const int LargoEmail = 150;
        public const int LargoTelefono = 20;
        public const int LargoMinimoTelefono = 6;

        /// <summary>Prefijos de CUIT: personas (20, 23, 24, 27) y personas jurídicas (30, 33, 34).</summary>
        private static readonly string[] PrefijosCuit = { "20", "23", "24", "27", "30", "33", "34" };

        private readonly ProveedorDAL_60MN mapper = new ProveedorDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        /// <summary>Con valor vacío lista todos los proveedores.</summary>
        public List<Proveedor_60MN> Buscar(CriterioBusquedaProveedor_60MN criterio, string? valor, bool incluirBajas)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Proveedores);
            valor = criterio == CriterioBusquedaProveedor_60MN.Cuit ? Validaciones_60MN.SinSeparadores(valor) : (valor ?? string.Empty).Trim();
            return mapper.Buscar(criterio, valor, incluirBajas);
        }

        public Proveedor_60MN? ObtenerPorId(int id)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Proveedores);
            return mapper.ObtenerPorId(id);
        }

        /// <summary>"Productos que provee" el proveedor seleccionado.</summary>
        public List<Producto_60MN> ListarProductos(int proveedorId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Proveedores);
            return mapper.ListarProductos(proveedorId);
        }

        public void Registrar(Proveedor_60MN proveedor)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Proveedores);
            Normalizar(proveedor);
            Validar(proveedor, esAlta: true);

            Proveedor_60MN? existente = mapper.ObtenerPorCuit(proveedor.Cuit);
            if (existente != null)
                throw new DatosProveedorInvalidosException_60MN(new Dictionary<string, string>
                {
                    [nameof(Proveedor_60MN.Cuit)] = $"El CUIT ya está registrado para {existente.RazonSocial}" +
                                                    (existente.Activo ? "." : " (dado de baja: puede reactivarlo).")
                });

            proveedor.Id = mapper.Insertar(proveedor);
            dv.ActualizarFila(TablaDV_60MN.Proveedor, proveedor.Id);
            bitacora.Registrar(EventoSistema_60MN.ProveedorRegistrado,
                $"Alta del proveedor {proveedor.RazonSocial} (CUIT {proveedor.CuitFormateado}).");
        }

        /// <summary>El CUIT no se modifica: se conserva el registrado.</summary>
        public void Modificar(Proveedor_60MN proveedor)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Proveedores);
            Proveedor_60MN actual = mapper.ObtenerPorId(proveedor.Id) ?? throw new NegocioException_60MN("El proveedor ya no existe.");
            if (!actual.Activo)
                throw new NegocioException_60MN("El proveedor está dado de baja: reactívelo antes de modificarlo.");
            proveedor.Cuit = actual.Cuit;
            Normalizar(proveedor);
            Validar(proveedor, esAlta: false);

            mapper.Modificar(proveedor);
            dv.ActualizarFila(TablaDV_60MN.Proveedor, proveedor.Id);
            bitacora.Registrar(EventoSistema_60MN.ProveedorModificado,
                $"Modificación del proveedor {proveedor.RazonSocial} (CUIT {actual.CuitFormateado}).");
        }

        /// <summary>
        /// Baja lógica o reactivación. Los productos conservan su proveedor; la GUI pide
        /// confirmación si tiene productos activos (ProductosActivos).
        /// </summary>
        public void CambiarEstado(int proveedorId, bool activo)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Proveedores);
            Proveedor_60MN actual = mapper.ObtenerPorId(proveedorId) ?? throw new NegocioException_60MN("El proveedor ya no existe.");
            if (actual.Activo == activo)
                throw new NegocioException_60MN(activo ? "El proveedor ya está activo." : "El proveedor ya está dado de baja.");

            mapper.CambiarActivo(proveedorId, activo);
            dv.ActualizarFila(TablaDV_60MN.Proveedor, proveedorId);
            bitacora.Registrar(activo ? EventoSistema_60MN.ProveedorModificado : EventoSistema_60MN.ProveedorBaja,
                $"{(activo ? "Reactivación" : "Baja lógica")} del proveedor {actual.RazonSocial} (CUIT {actual.CuitFormateado})" +
                (!activo && actual.ProductosActivos > 0 ? $"; {actual.ProductosActivos} producto(s) activo(s) conservan el proveedor." : "."));
        }

        /// <summary>
        /// Valida los datos sin guardarlos y devuelve los errores por campo (vacío si todo es
        /// correcto). La GUI lo usa para marcar cada campo; Registrar y Modificar aplican las mismas reglas.
        /// </summary>
        public IReadOnlyDictionary<string, string> ValidarDatos(Proveedor_60MN proveedor, bool esAlta)
        {
            Proveedor_60MN copia = new Proveedor_60MN
            {
                RazonSocial = proveedor.RazonSocial, Cuit = proveedor.Cuit, Telefono = proveedor.Telefono, Email = proveedor.Email,
                Direccion = proveedor.Direccion, Contacto = proveedor.Contacto
            };
            Normalizar(copia);
            return ErroresProveedor(copia, esAlta);
        }

        private static void Normalizar(Proveedor_60MN p)
        {
            p.RazonSocial = Validaciones_60MN.Normalizar(p.RazonSocial);
            // CUIT y teléfono se guardan solo con sus dígitos ("30-71234567-1" → "30712345671").
            p.Cuit = Validaciones_60MN.SinSeparadores(p.Cuit);
            p.Telefono = Validaciones_60MN.SinSeparadores(p.Telefono);
            p.Direccion = Validaciones_60MN.Normalizar(p.Direccion);
            p.Email = string.IsNullOrWhiteSpace(p.Email) ? null : p.Email!.Trim();
            p.Contacto = string.IsNullOrWhiteSpace(p.Contacto) ? null : Validaciones_60MN.Normalizar(p.Contacto);
        }

        private static void Validar(Proveedor_60MN p, bool esAlta)
        {
            Dictionary<string, string> errores = ErroresProveedor(p, esAlta);
            if (errores.Count > 0)
                throw new DatosProveedorInvalidosException_60MN(errores);
        }

        private static Dictionary<string, string> ErroresProveedor(Proveedor_60MN p, bool esAlta)
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

            // En una modificación el CUIT no se edita y no se vuelve a validar.
            if (esAlta)
                Revisar(nameof(Proveedor_60MN.Cuit), p.Cuit, true, () => ValidarCuit(p.Cuit));
            Revisar(nameof(Proveedor_60MN.RazonSocial), p.RazonSocial, true, () =>
            {
                Validaciones_60MN.Largo(p.RazonSocial, LargoRazonSocial, "La razón social");
                Validaciones_60MN.RazonSocial(p.RazonSocial, "La razón social");
            });
            Revisar(nameof(Proveedor_60MN.Telefono), p.Telefono, true, () =>
                Validaciones_60MN.TelefonoDigitos(p.Telefono, LargoMinimoTelefono, LargoTelefono, "El teléfono"));
            Revisar(nameof(Proveedor_60MN.Direccion), p.Direccion, true, () =>
            {
                Validaciones_60MN.Largo(p.Direccion, LargoDireccion, "La dirección");
                Validaciones_60MN.Direccion(p.Direccion, "La dirección");
            });
            Revisar(nameof(Proveedor_60MN.Email), p.Email ?? string.Empty, false, () =>
            {
                Validaciones_60MN.Largo(p.Email, LargoEmail, "El mail");
                if (!Validaciones_60MN.EsEmailValido(p.Email))
                    throw new NegocioException_60MN("El formato del mail es inválido (ejemplo: ventas@proveedor.com).");
            });
            Revisar(nameof(Proveedor_60MN.Contacto), p.Contacto ?? string.Empty, false, () =>
            {
                Validaciones_60MN.Largo(p.Contacto, LargoContacto, "El contacto");
                Validaciones_60MN.NombrePropio(p.Contacto!, "El contacto");
            });
            return errores;
        }

        private static void ValidarCuit(string cuit)
        {
            if (!cuit.All(char.IsDigit))
                throw new NegocioException_60MN("El CUIT solo puede contener números (con o sin guiones).");
            if (cuit.Length != 11)
                throw new NegocioException_60MN("El CUIT debe tener 11 dígitos (ej. 30-71234567-1).");
            if (!PrefijosCuit.Contains(cuit.Substring(0, 2)))
                throw new NegocioException_60MN("El CUIT debe empezar con 20, 23, 24, 27, 30, 33 o 34.");
            if (!Validaciones_60MN.CuitConDigitoValido(cuit))
                throw new NegocioException_60MN("El CUIT no es válido: el dígito verificador (el último) no corresponde.");
        }
    }
}
