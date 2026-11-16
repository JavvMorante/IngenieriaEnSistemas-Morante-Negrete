namespace VetCare
{
    /// <summary>Tema de la ayuda en línea: título, texto y formularios que lo abren con F1.</summary>
    internal sealed class TemaAyuda_60MN
    {
        public string Id { get; }

        public string[] Formularios { get; }

        private readonly string titulo;
        private readonly string texto;

        public TemaAyuda_60MN(string id, string titulo, string texto, params string[] formularios)
        {
            Id = id;
            this.titulo = titulo;
            this.texto = texto;
            Formularios = formularios;
        }

        /// <summary>Título en el idioma actual (clave ayuda.&lt;id&gt;.titulo; el español es el texto base).</summary>
        public string Titulo => TraductorFormularios_60MN.T($"ayuda.{Id}.titulo", titulo);

        /// <summary>Texto en el idioma actual (clave ayuda.&lt;id&gt;.texto).</summary>
        public string Texto => TraductorFormularios_60MN.T($"ayuda.{Id}.texto", texto);

        public override string ToString() => Titulo;
    }

    /// <summary>
    /// Ayuda en línea (G05): un tema por caso de uso o pantalla. Como el resto de la interfaz,
    /// el español es el idioma base y está en el código; el inglés está en la tabla Traduccion
    /// (claves ayuda.&lt;id&gt;.titulo y ayuda.&lt;id&gt;.texto, de hasta 400 caracteres).
    /// </summary>
    internal static class TemasAyuda_60MN
    {
        public const string General = "general";

        public static readonly IReadOnlyList<TemaAyuda_60MN> Todos = new List<TemaAyuda_60MN>
        {
            new TemaAyuda_60MN(General, "VetCare: cómo usar el sistema",
                "El menú principal muestra solo las opciones que permite su perfil. MAESTROS reúne clientes, mascotas, productos y proveedores; " +
                "PACIENTES, las historias clínicas y las atenciones; VENTAS, el carrito; COMPRAS, la reposición de medicamentos e insumos; REPORTES, los reportes. " +
                "Presione F1 en cualquier pantalla para ver su ayuda, o elija otro tema en este índice.",
                "FrmMenuPrincipal_60MN"),
            new TemaAyuda_60MN("login", "Iniciar sesión y cambiar la contraseña",
                "Ingrese su usuario y contraseña. Tres intentos fallidos bloquean al usuario: pida a un administrador que lo desbloquee. " +
                "La contraseña nueva debe tener al menos 8 caracteres, con mayúsculas, minúsculas y números.",
                "FrmLogin_60MN", "FrmCambiarClave_60MN"),
            new TemaAyuda_60MN("usuarios", "Usuarios",
                "ADMIN > Usuarios: busque, añada, modifique, dé de baja, reactive o desbloquee usuarios. Al añadir uno se elige su perfil " +
                "inicial y el sistema genera una contraseña que debe cambiarse en el primer ingreso. Permisos asigna familias y patentes individuales.",
                "FrmGestionUsuarios_60MN", "FrmUsuario_60MN", "FrmPermisosUsuario_60MN"),
            new TemaAyuda_60MN("perfiles", "Perfiles (familias y patentes)",
                "ADMIN > Perfiles: cada familia es un conjunto de patentes y puede incluir otras familias. El sistema no permite ciclos entre " +
                "familias ni un cambio que deje al sistema sin un administrador operativo.",
                "FrmFamiliasPatentes_60MN"),
            new TemaAyuda_60MN("bitacoraEventos", "Bitácora de eventos",
                "ADMIN > Bitácora de eventos: filtre por fechas, criticidad, módulo, evento y usuario. Exportar XML guarda los eventos firmados; Visualizar " +
                "abre un XML exportado y muestra sus eventos en una grilla (si el archivo fue alterado, lo rechaza).",
                "FrmBitacoraEventos_60MN", "FrmDeserializarBitacora_60MN"),
            new TemaAyuda_60MN("bitacoraCambios", "Bitácora de cambios",
                "ADMIN > Bitácora de cambios: historial de versiones de cada producto (altas, modificaciones y movimientos por ventas y recepciones). Puede comparar dos versiones y restaurar una anterior.",
                "FrmBitacoraCambios_60MN", "FrmCompararVersiones_60MN"),
            new TemaAyuda_60MN("integridad", "Dígitos verificadores",
                "ADMIN > Dígitos verificadores: verifica los DVH (uno por fila) y DVV (uno por tabla) de las tablas protegidas y muestra las " +
                "inconsistencias. Recalcular acepta el estado actual de la base: úselo solo después de una migración o de revisar los cambios.",
                "FrmIntegridad_60MN"),
            new TemaAyuda_60MN("backup", "Backup y restore",
                "ADMIN > Backup genera una copia de la base en la carpeta elegida. ADMIN > Restore la reemplaza por un backup anterior: se " +
                "pierden los cambios posteriores y la sesión se cierra. No se respalda una base con inconsistencias de dígitos verificadores.",
                "FrmBackupRestore_60MN"),
            new TemaAyuda_60MN("idioma", "Cambiar idioma",
                "USUARIO > Cambiar idioma traduce todas las pantallas abiertas. El idioma elegido queda guardado como el suyo y se aplica " +
                "cada vez que inicia sesión.",
                "FrmCambiarIdioma_60MN"),
            new TemaAyuda_60MN("clientes", "CUN01 Gestionar Clientes",
                "MAESTROS > Clientes: busque por documento, apellido o nombre de mascota. Desde la ficha puede registrar, modificar, dar de baja " +
                "o reactivar clientes y gestionar sus mascotas. El documento se valida según su tipo (DNI, CUIL con dígito verificador, " +
                "pasaporte) y no se modifica después del alta.",
                "FrmClientes_60MN", "FrmCliente_60MN", "FrmBuscarCliente_60MN"),
            new TemaAyuda_60MN("mascotas", "CUN02 Gestionar Mascotas",
                "MAESTROS > Mascotas: busque por nombre, especie o por el documento o apellido del dueño. Para añadir una mascota primero " +
                "elija su cliente. La baja es lógica: la historia clínica se conserva.",
                "FrmGestionMascotas_60MN", "FrmMascotas_60MN"),
            new TemaAyuda_60MN("historia", "CUN03 Gestionar Historias Clínicas",
                "PACIENTES > Historias clínicas: elija el paciente. Si no tiene historia clínica puede abrirla. Desde la historia se consultan " +
                "las atenciones, se actualizan los datos (con historial de valores anteriores) y se imprime.",
                "FrmHistoriaClinica_60MN", "FrmBuscarPaciente_60MN"),
            new TemaAyuda_60MN("atencion", "CUN04 Registrar Atención",
                "PACIENTES > Registrar atención: elija el paciente (debe tener historia clínica). Cargue signos vitales, motivo y diagnóstico; " +
                "las indicaciones son opcionales. Registrada la atención, puede agregar medicación (carrito) y emitir el informe.",
                "FrmAtencion_60MN"),
            new TemaAyuda_60MN("carrito", "CUN05 Registrar Medicación en Carrito",
                "Desde la atención (Agregar medicación) o desde VENTAS > Carrito eligiendo el cliente, con o sin atención. Agregue productos " +
                "con stock; no se entregan vencidos. Al confirmar se registra la venta y el egreso de stock. VENTAS > Ventas registradas " +
                "consulta las ventas. El sistema no factura.",
                "FrmCarrito_60MN", "FrmVentas_60MN"),
            new TemaAyuda_60MN("informe", "CUN06 Emitir Informe de Atención",
                "El informe reúne paciente, propietario, profesional, signos vitales, motivo, diagnóstico, indicaciones y medicación. " +
                "Imprímalo o elija \"Microsoft Print to PDF\" para guardarlo; cada emisión queda registrada.",
                "FrmInformeAtencion_60MN"),
            new TemaAyuda_60MN("reporteAtenciones", "CUN07 Generar Reporte de Atenciones",
                "REPORTES > Reporte de atenciones: elija el período y, si quiere, el veterinario y la especie. Muestra las atenciones y los " +
                "totales por profesional; se imprime (o PDF) y se exporta a CSV.",
                "FrmReporteAtenciones_60MN"),
            new TemaAyuda_60MN("proveedores", "CUN08 Gestionar Proveedores",
                "MAESTROS > Proveedores: busque por razón social o CUIT y vea los productos que provee cada uno. El CUIT se valida con su " +
                "dígito verificador y no se modifica. Un proveedor dado de baja no se ofrece en productos ni en cotizaciones.",
                "FrmProveedores_60MN", "FrmProveedor_60MN"),
            new TemaAyuda_60MN("productos", "CUN09 Administrar Productos",
                "MAESTROS > Productos: busque, registre y modifique productos. El margen es de cada producto (en el alta lo propone la " +
                "categoría) y el precio de venta se calcula con él; Usar sugerido lo vuelve a aplicar. El stock no se edita: arranca en 0 " +
                "y cambia con las ventas y la recepción de mercadería. Historial de precios muestra las versiones del producto.",
                "FrmProductos_60MN", "FrmProducto_60MN"),
            new TemaAyuda_60MN("solicitud", "CUN10 Generar Solicitud de Reposición",
                "COMPRAS > Solicitud de reposición: muestra los productos en alerta de stock mínimo con una cantidad sugerida (el doble del " +
                "mínimo menos el stock). Marque los que quiere pedir, ajuste A pedir, agregue otros con el buscador y envíe la solicitud.",
                "FrmSolicitudReposicion_60MN"),
            new TemaAyuda_60MN("ordenCompra", "CUN11 Registrar Orden de Compra",
                "COMPRAS > Órdenes de compra: elija la solicitud, cargue la cotización de cada proveedor y compárelas (se marca la más " +
                "barata). Si elige otra, indique el motivo. La orden queda pendiente de aprobación. En Mis órdenes corrija las rechazadas o anule una orden no pagada.",
                "FrmOrdenCompra_60MN", "FrmCotizacion_60MN"),
            new TemaAyuda_60MN("aprobarOrden", "CUN12 Aprobar Orden de Compra",
                "COMPRAS > Aprobar órdenes: revise el detalle, el origen (solicitud y cotización) y las advertencias de precio. Apruebe o " +
                "rechace con motivo. Quien registró la orden no puede aprobarla. Si se supera el tope mensual de compras, se pide confirmación.",
                "FrmAprobarOrden_60MN"),
            new TemaAyuda_60MN("pagoProveedor", "CUN13 Registrar Pago al Proveedor",
                "COMPRAS > Pagos a proveedores: Enviar orden la imprime o guarda en PDF para mandarla al proveedor. El pago se hace en el " +
                "home banking; aquí se registra banco, comprobante, fecha e importe. Un comprobante no se repite para el mismo banco.",
                "FrmPagoProveedor_60MN"),
            new TemaAyuda_60MN("recepcion", "CUN14 Registrar Recepción de Mercadería",
                "COMPRAS > Recepción de mercadería: busque la orden pagada, ingrese el remito y lo recibido de cada producto con su lote y " +
                "vencimiento. El stock sube con un movimiento de ingreso. Si falta mercadería la orden queda recibida parcial; al completarla se cierra.",
                "FrmRecepcion_60MN"),
            new TemaAyuda_60MN("reporteCompras", "CUN15 Generar Reporte de Compras",
                "REPORTES > Reporte de compras: elija el período y, si quiere, el proveedor y el estado. Muestra cada orden con lo pagado y " +
                "lo recibido, los totales por proveedor y los generales. Se imprime o se exporta a PDF.",
                "FrmReporteCompras_60MN"),
            new TemaAyuda_60MN("reporteInteligente", "CUN16 Generar Reporte Inteligente",
                "REPORTES > Reporte inteligente: cruza la demanda de insumos por mes y categoría (stock) con los productos más entregados " +
                "en atenciones (pacientes) para anticipar compras.",
                "FrmReporteInteligente_60MN"),
        };

        /// <summary>Tema de un formulario (por su nombre); el tema general si no tiene uno propio.</summary>
        public static TemaAyuda_60MN DeFormulario(string? formulario) =>
            Todos.FirstOrDefault(t => t.Formularios.Contains(formulario)) ?? Todos[0];
    }
}
