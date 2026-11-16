# Prompt para Claude: acomodar el menú y completar VetCare según el Trabajo de Diploma V2.4

Trabajá sobre la solución `VetCare/VetCare.sln` de este repositorio (C#, WinForms, **.NET Framework 4.8**, `LangVersion latest`, `Nullable enable`, sin paquetes NuGet nuevos). El documento de referencia es `Javier Morante Trabajo de Diploma Vetcare V2.4.docx` (carpeta raíz del repo): el código tiene que quedar alineado con sus casos de uso. Antes de cambiar nada, leé `VetCare/README_VetCare_60MN.md`, `Usuarios_y_Permisos_VetCare_60MN.md` y los archivos que se mencionan abajo, y armá un plan por fases. Hacé una fase por vez, mostrame qué archivos tocaste y esperá mi OK antes de pasar a la siguiente.

## Reglas del proyecto (respetarlas en todo lo nuevo)

- Todo lo nuevo lleva el sufijo `_60MN` (clases, formularios, archivos).
- Arquitectura en capas: `VetCare` (GUI) → `BLL_60MN` → `DAL_60MN`, con `Entidades_60MN`, `Servicios_60MN`, `Seguridad_60MN` e `Interfaces_60MN`. La GUI nunca accede a la DAL.
- SQL siempre parametrizado a través de `Conexion_60MN` (mismo estilo que `ClienteDAL_60MN` y `ProductoDAL_60MN`).
- Permisos en dos niveles: `SesionActual_60MN.RequierePermiso(...)` al inicio de cada método público de la BLL, y el código de patente en el `Tag` del ítem de menú. Los códigos van en `Servicios_60MN/Composite/CodigosPatente_60MN.cs`.
- Bitácora: cada operación registra un evento con `BitacoraBLL_60MN.Registrar(EventoSistema_60MN.X, descripcion)`. Todo valor nuevo del enum `EventoSistema_60MN` se agrega también en `Seguridad_60MN/Auditoria/CatalogoEventos_60MN.cs` con módulo y criticidad (1 muy alta … 5 informativa); si falta, el sistema no arranca.
- Dígitos verificadores: toda tabla nueva con datos sensibles tiene columna `DVH CHAR(64) NULL`, se registra en `Seguridad_60MN/Integridad/TablaDV_60MN.cs` y después de cada INSERT/UPDATE se llama `GestorDigitoVerificador_60MN.ActualizarFila(...)`.
- Formularios: heredan de `FormBase_60MN` (patrón Observer del idioma), se abren con `Abrir<T>()` en el menú y usan la misma estética que los existentes (verde azulado 0,102,102, botones y grillas como `FrmClientes_60MN`). Los textos nuevos se agregan en inglés a la tabla `Traduccion`.
- Validaciones en la BLL con errores por campo, como `ClienteBLL_60MN.ValidarDatos` / `ProductoBLL_60MN.ValidarDatos`, y normalización de datos antes de guardar.
- Base de datos: el script `BDScript/VetCareBD_60MN.sql` se **genera** con `Herramientas_60MN/GeneradorScriptBD_60MN` (estructura en `Estructura.sql`, datos iniciales con DVH en `Program.cs`). Modificá esos dos archivos y regenerá el script; no escribas DVH a mano. Además, dejá un script de migración aparte `BDScript/Migracion_V24_60MN.sql` para actualizar una base ya creada sin borrarla (y recordame recalcular los DV desde ADMIN > Dígitos verificadores después de correrlo).
- No hay `dotnet` en la máquina donde trabajás: revisá con cuidado que compile (usings, namespaces, `.Designer.cs` y `.resx` coherentes) y al final pasame la lista de archivos tocados y de scripts a ejecutar.

## Fase 1 – Acomodar el menú principal

Archivos: `VetCare/FrmMenuPrincipal_60MN.Designer.cs` y `VetCare/FrmMenuPrincipal_60MN.cs`.

MAESTROS tiene que tener solo los maestros del sistema: **Clientes, Mascotas, Productos y Proveedores**. Hoy tiene además "Movimientos de stock", "Configuración de stock" y "Bitácora de cambios", que no son maestros, y le falta "Mascotas". Estructura final:

| Menú | Ítems (Tag = patentes) |
|---|---|
| ADMIN | Usuarios, Perfiles, Backup, Restore, Bitácora de eventos, **Bitácora de cambios** (`SEG_BITACORA_CAMBIOS`), Dígitos verificadores, **Configuración de stock** (`STK_CONFIG`) |
| MAESTROS | **Clientes** (`PAC_CLIENTE_CONSULTA\|PAC_CLIENTE_ALTA\|PAC_CLIENTE_MODIF`), **Mascotas** (`PAC_MASCOTA`), **Productos** (`STK_PROD_ALTA\|STK_PROD_MODIF`), **Proveedores** (`MAE_PROVEEDORES`) |
| PACIENTES | **Historias clínicas** (`HC_CONSULTA\|HC_ALTA\|HC_MODIF`), **Registrar atención** (`ATN_REGISTRAR`) |
| VENTAS | **Carrito** (`VEN_CARRITO`), **Ventas registradas** (`VEN_CARRITO`) |
| STOCK | **Movimientos de stock** (`STK_MOVIMIENTO`) |
| REPORTES | **Reporte de atenciones** (`REP_REPORTES`), **Reporte de existencias** (`STK_REPORTE`), **Reporte inteligente** (`REP_REPORTES`) |
| USUARIO | Re-Login, Cambiar clave, Logout, Cambiar idioma (sin cambios) |
| AYUDA | Ayuda en línea (F1), Acerca de VetCare |

- "Historias clínicas" y "Registrar atención" abren primero un selector de paciente reutilizable (`FrmBuscarPaciente_60MN`: busca por documento/apellido del dueño o nombre de la mascota y devuelve la mascota elegida) y después `FrmHistoriaClinica_60MN` o `FrmAtencion_60MN`.
- Mantené el mecanismo actual de visibilidad por `Tag` (`AplicarPermisosMenu`) y el renderizado de `RenderizadorMenu_60MN`.
- Agregá los nombres nuevos de menú a las traducciones.

## Fase 2 – Gestionar Mascotas como maestro (CUN02, «extend» de CUN01)

- Nuevo `VetCare/Pacientes/FrmGestionMascotas_60MN`: buscador (nombre de mascota, especie, documento o apellido del dueño, "Incluir dados de baja"), grilla con mascota + dueño + si tiene historia clínica, y botones Añadir, Modificar, Dar de baja, Reactivar.
- Para **Añadir** es obligatorio elegir el cliente (una mascota no puede existir sin cliente): reutilizá la búsqueda de clientes y después `FrmMascotas_60MN`.
- Reutilizá `ClienteBLL_60MN.RegistrarMascota / ModificarMascota / CambiarEstadoMascota / ExisteMascotaConNombre`. Agregá en BLL y DAL solo lo que falte, por ejemplo `BuscarMascotas(criterio, valor, incluirBajas)`.
- Desde `FrmClientes_60MN` se tiene que seguir pudiendo gestionar las mascotas de la ficha del cliente, como hoy.

## Fase 3 – Gestionar Proveedores (CUN11)

Hoy el ítem existe pero abre `mnuPendiente_Click`, y la tabla `Proveedor` solo tiene `RazonSocial` y `CUIT`.

- Tabla `Proveedor`: agregar `Telefono VARCHAR(30) NOT NULL`, `Email VARCHAR(150) NULL`, `Direccion VARCHAR(150) NOT NULL`, `Contacto VARCHAR(80) NULL`, `Activo BIT NOT NULL DEFAULT 1`, `DVH CHAR(64) NULL`. Registrarla en `TablaDV_60MN`.
- `Entidades_60MN/Proveedor_60MN.cs` con las propiedades nuevas.
- Nuevos `DAL_60MN/ProveedorDAL_60MN.cs` (`Buscar`, `ObtenerPorId`, `ObtenerPorCuit`, `Insertar`, `Modificar`, `CambiarActivo`, `ListarProductos(proveedorId)`) y `BLL_60MN/ProveedorBLL_60MN.cs` (`Buscar`, `Registrar`, `Modificar`, `CambiarEstado`, `ListarProductos`, `ValidarDatos`).
- Validaciones: CUIT de 11 dígitos con dígito verificador válido y único (podés reutilizar `DigitoVerificadorCuil` de `ClienteBLL_60MN` moviéndolo a `Validaciones_60MN`), razón social, teléfono y dirección obligatorios, email con formato válido si se carga. El CUIT no se puede modificar.
- Baja lógica: si el proveedor tiene productos activos, avisar y pedir confirmación; los productos conservan su proveedor.
- `ProductoBLL_60MN.ListarProveedores()` / `CatalogoStockDAL_60MN.ListarProveedores()` tienen que devolver solo los proveedores **activos** para el alta de productos.
- Nuevo `VetCare/Stock/FrmProveedores_60MN` (lista + "Productos que provee") y `FrmProveedor_60MN` (alta/modificación), con la misma estética que `FrmProductos_60MN`.
- Eventos nuevos: `ProveedorRegistrado`, `ProveedorModificado` (módulo Stock, criticidad 4) y `ProveedorBaja` (criticidad 3).

## Fase 4 – Carrito y Venta (CUN05), sin facturación

Hoy la medicación se guarda en `AtencionDetalle`. Según la V2.4, el carrito vive en memoria y al confirmarlo se graba una **venta**:

- Tablas nuevas `VentaEncabezado` (`VentaID`, `ClienteID` FK, `AtencionID` FK NULL, `FechaHora`, `UsuarioID`, `Total DECIMAL(12,2)`, `DVH`) y `VentaDetalle` (`VentaDetalleID`, `VentaID` FK, `ProductoID` FK, `Cantidad > 0`, `PrecioUnitario`, `Subtotal`, `MovimientoID` FK a `MovimientoStock`, `DVH`). Registrarlas en `TablaDV_60MN`.
- La migración pasa los datos de `AtencionDetalle` a `VentaEncabezado`/`VentaDetalle` (una venta por atención, con el cliente dueño de la mascota) y después elimina `AtencionDetalle` (sacarla también de `TablaDV_60MN`).
- Entidades: `Carrito_60MN` (en memoria: `ClienteId`, `AtencionId?`, `Items: List<ItemCarrito_60MN>`, `Total`, `Agregar`, `Quitar`), `VentaEncabezado_60MN` y `VentaDetalle_60MN`.
- `VentaBLL_60MN` y `VentaDAL_60MN`: mover ahí lo que hoy hace `AtencionBLL_60MN.RegistrarMedicacion` / `AtencionDAL_60MN.RegistrarCarrito`. Todo en **una sola transacción**: encabezado, detalle y movimiento de egreso de stock por ítem, con las mismas validaciones (stock, vencidos, dados de baja, cantidades) y la evaluación de alertas de reposición. Agregar `ObtenerPorAtencion(atencionId)` y `ListarVentas(desde, hasta, clienteId?)`.
- El carrito se puede abrir desde una atención (como hoy) o desde VENTAS > Carrito eligiendo un cliente, con o sin atención.
- Nuevo `FrmVentas_60MN` (VENTAS > Ventas registradas): consulta de ventas por período y cliente, con su detalle. Solo consulta.
- Actualizar a la tabla nueva: `AtencionBLL_60MN.ComponerInforme` (la medicación sale de la venta de la atención) y `ReporteInteligenteDAL_60MN.TopProductosEntregados` (de `VentaDetalle` unido a `VentaEncabezado` con `AtencionID` no nulo).
- **El sistema no factura.** Quitar las patentes `VEN_FACTURAR`, `VEN_DESPACHAR` y `COM_COMPRAS` del generador del script, de `CodigosPatente_60MN` y de las familias, y dejar en la migración el DELETE correspondiente (después hay que recalcular DV).

## Fase 5 – Reporte de Atenciones (CUN07)

- `DAL_60MN/AtencionDAL_60MN.ListarPorPeriodo(desde, hasta, usuarioId?, especie?)` que devuelva `FilaReporteAtencion_60MN` (fecha, número de HC, paciente, especie, propietario, profesional, motivo), uniendo `Atencion`, `HistoriaClinica`, `Mascota`, `Cliente` y `Usuario`.
- `BLL_60MN/ReporteAtencionesBLL_60MN.Generar(desde, hasta, usuarioId?, especie?)`: permiso `REP_REPORTES`; valida desde > hasta y fecha futura, límite superior exclusivo (`hasta.AddDays(1)`), totales por profesional, y registra el evento nuevo `ReporteAtencionesGenerado` (módulo Pacientes, criticidad 5).
- `VetCare/Reportes/FrmReporteAtenciones_60MN`: filtros (desde, hasta, veterinario, especie), grilla de atenciones, grilla "Atenciones por profesional", Imprimir / PDF (reutilizá `ImpresionTexto_60MN` como en el reporte inteligente) y Exportar CSV.

## Fase 6 – Ayuda en línea (F1)

Hoy "Ayuda en línea" está pendiente y la G05 del documento la pide. Implementá una ayuda simple: F1 en cualquier formulario (desde `FormBase_60MN`) y AYUDA > Ayuda en línea abren una ventana `FrmAyuda_60MN` con el tema del formulario activo (textos en un recurso o en la tabla `Traduccion`, español e inglés) y un índice de temas.

## Fase 7 – Cierre

- Actualizar los comentarios de `CodigosPatente_60MN.cs` y del catálogo de eventos con la numeración nueva (CUN01 Gestionar Clientes … CUN13 Reporte Inteligente).
- Patentes y familias: la familia Vendedor suma `VEN_CARRITO` para "Ventas registradas"; Veterinario y Gerente/Administrador, `REP_REPORTES`; Operario de stock, `MAE_PROVEEDORES`.
- Actualizar `README_VetCare_60MN.md` y `Usuarios_y_Permisos_VetCare_60MN.md` (menú, casos de uso con la numeración nueva, tablas con DV, scripts a ejecutar).
- Al terminar, dame: la lista de archivos nuevos y modificados por fase, los scripts SQL a correr y en qué orden, y una prueba manual por caso de uso (pasos y resultado esperado).

Numeración de casos de uso de la V2.4, para comentarios y textos:

| PN1 – Gestión de Pacientes | PN2 – Gestión de Stock |
|---|---|
| CUN01 Gestionar Clientes | CUN08 Administrar Existencias |
| CUN02 Gestionar Mascotas | CUN09 Registrar Movimiento de Stock |
| CUN03 Gestionar Historias Clínicas | CUN10 Configurar Margen y Alerta de Stock Mínimo |
| CUN04 Registrar Atención | CUN11 Gestionar Proveedores |
| CUN05 Registrar Medicación en Carrito | CUN12 Generar Reporte de Existencias |
| CUN06 Emitir Informe de Atención | CUN13 Generar Reporte Inteligente (cruza PN1 y PN2) |
| CUN07 Generar Reporte de Atenciones | |
