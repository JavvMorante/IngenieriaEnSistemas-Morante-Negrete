# Prompt para Claude: implementar el PN2 "Reposición de Medicamentos e Insumos" (V2.5)

Copiá todo lo que está debajo de la línea y pegalo en Claude (Claude Code / Cowork) con la carpeta `VetCare` abierta.

---

## Contexto

Trabajás sobre la solución **VetCare** (`VetCare/VetCare.sln`): C# WinForms .NET Framework 4.8, SQL Server, base `VetCareBD_60MN`. Las capas son:

- `VetCare` (GUI)
- `BLL_60MN`
- `DAL_60MN`
- `Entidades_60MN`
- `Interfaces_60MN`
- `Servicios_60MN`
- `Seguridad_60MN`
- `Herramientas_60MN/GeneradorScriptBD_60MN` (genera `BDScript/VetCareBD_60MN.sql` con los DVH)

Convenciones que hay que respetar:

- Todos los nombres de clases, formularios y archivos llevan el sufijo `_60MN`.
- Los formularios heredan de `FormBase_60MN`. Sus textos se traducen con `T("clave", "texto por defecto")` y la tabla `Traduccion` (es / en).
- Los mensajes se muestran con `Mensajes_60MN`. Los errores de negocio se lanzan con `NegocioException_60MN`.
- Cada alta o modificación de una tabla protegida llama a `GestorDigitoVerificador_60MN.ActualizarFila(TablaDV_60MN.X, id)`. Cada tabla nueva se agrega al catálogo de `Seguridad_60MN/Integridad/TablaDV_60MN.cs`, con sus columnas.
- Cada operación registra su evento con `BitacoraBLL_60MN.Registrar(EventoSistema_60MN.X, descripcion)`. Los eventos nuevos se agregan a `Seguridad_60MN/Auditoria/CatalogoEventos_60MN.cs`, con módulo y criticidad.
- Los permisos son patentes (patrón Composite). El código de la patente va en `Servicios_60MN/Composite/CodigosPatente_60MN.cs`, en la tabla `Patente` del script y en la propiedad `Tag` del ítem de menú o botón. El menú oculta lo que el usuario no tiene permitido.
- Las operaciones que tocan varias tablas van en una sola transacción SQL. Hay que seguir el patrón de `MovimientoStockDAL_60MN.Registrar` y `VentaBLL_60MN`, que usan concurrencia optimista sobre `StockActual`.

La documentación de referencia es `Javier Morante Trabajo de Diploma Vetcare V2.5.docx`, en la raíz del repo, sección "Proceso 2: Reposición de Medicamentos e Insumos". La numeración de CU de este prompt es la de ese documento.

## Qué cambia respecto del código actual

El viejo PN2 "Gestión de Stock" se reemplaza por un proceso de compras de 8 pasos:

1. El **encargado de depósito** genera una solicitud de reposición con los productos en stock mínimo.
2. El **analista de compras** pide cotización a los proveedores. Esto ocurre fuera del sistema.
3. El **proveedor** envía la cotización.
4. El analista compara las cotizaciones y registra la **orden de compra**.
5. El **gerente** la aprueba o la rechaza, con motivo.
6. El analista envía la orden y la paga por transferencia en el **banco**. En el sistema solo registra el comprobante.
7. El proveedor entrega la mercadería con el remito.
8. El encargado de depósito registra la **recepción**. El sistema suma el stock (movimiento INGRESO) y cierra la orden cuando se recibió todo.

Casos de uso:

| CU | Nombre | Actor | Patente |
|---|---|---|---|
| CUN08 | Gestionar Proveedores (ya existe) | Analista de compras | `MAE_PROVEEDORES` |
| CUN09 | Administrar Productos (ya existe, con margen por producto) | Encargado de depósito | `STK_PROD_ALTA`, `STK_PROD_MODIF` |
| CUN10 | Generar Solicitud de Reposición | Encargado de depósito | `CMP_SOLICITUD` (nueva) |
| CUN11 | Registrar Orden de Compra | Analista de compras | `CMP_ORDEN` (nueva) |
| CUN12 | Aprobar Orden de Compra | Gerente | `CMP_APROBAR` (nueva) |
| CUN13 | Registrar Pago al Proveedor | Analista de compras | `CMP_PAGO` (nueva) |
| CUN14 | Registrar Recepción de Mercadería | Encargado de depósito | `CMP_RECEPCION` (nueva) |
| CUN15 | Generar Reporte de Compras | Gerente / Analista | `REP_COMPRAS` (nueva) |
| CUN16 | Generar Reporte Inteligente (antes CUN13) | Gerente | `REP_REPORTES` |

**Se eliminan como casos de uso** "Registrar Movimiento de Stock" y "Configurar Margen y Alerta":

- Se quitan del menú los ítems `mnuMovimientosStock` (STOCK) y `mnuConfiguracionStock` (MAESTROS), y el menú STOCK completo si queda vacío.
- Se quitan las patentes `STK_MOVIMIENTO` y `STK_CONFIG`, con sus filas en `FamiliaPatente`.
- Se borran `FrmMovimientoStock_60MN` y `FrmConfiguracionStock_60MN`, con sus traducciones.
- **Se conserva** la tabla `MovimientoStock` y la lógica interna de `MovimientoStockBLL_60MN` / `MovimientoStockDAL_60MN`: ahí se siguen registrando los EGRESO de las ventas (CUN05) y ahora también los INGRESO de las recepciones (CUN14). Si hay métodos públicos que solo usaba el formulario eliminado, se pueden dejar `internal` o quitarlos.
- `FrmReporteExistencias_60MN` y la patente `STK_REPORTE` también se quitan. El reporte del proceso pasa a ser el de compras.

## Fase 1. Base de datos (`BDScript/VetCareBD_60MN.sql` y `Herramientas_60MN/GeneradorScriptBD_60MN/Estructura.sql`)

1. Agregar `Margen DECIMAL(6,2) NOT NULL` a `Productos`, entre `PrecioVenta` y `FechaVencimiento`. En los datos de prueba se carga con el margen de la categoría o, si es NULL, con `MargenGeneral` (30). `Categoria.Margen` y `Configuracion.MargenGeneral` quedan solo como valor por defecto al dar de alta un producto. Actualizar `TablaDV_60MN` (columnas de Productos), `Productos_C` y el trigger `TR_Productos_BitacoraCambios` si copian columnas.
2. Crear las tablas nuevas, todas con `DVH CHAR(64) NULL`:
   - `SolicitudReposicion` (SolicitudID PK identity, FechaHora, UsuarioID FK Usuario, Estado VARCHAR(20) CHECK IN ('PENDIENTE','EN_COMPRA','CERRADA','ANULADA'), Observaciones VARCHAR(400) NULL).
   - `SolicitudReposicionDetalle` (SolicitudDetalleID PK, SolicitudID FK, ProductoID FK, StockActual, StockMinimo, CantidadPedida INT CHECK > 0).
   - `OrdenCompra` (OrdenCompraID PK, Numero VARCHAR(20) calculado o generado 'OC-00001', SolicitudID FK, ProveedorID FK, FechaHora, NroCotizacion VARCHAR(30), PlazoEntregaDias INT, CondicionPago VARCHAR(60), MotivoEleccion VARCHAR(300) NULL, Total DECIMAL(12,2), Estado VARCHAR(20) CHECK IN ('PENDIENTE_APROBACION','APROBADA','RECHAZADA','ENVIADA','PAGADA','RECIBIDA_PARCIAL','CERRADA','ANULADA'), UsuarioID, AprobadaPor INT NULL FK Usuario, FechaAprobacion DATETIME2 NULL, MotivoRechazo VARCHAR(300) NULL).
   - `OrdenCompraDetalle` (OrdenDetalleID PK, OrdenCompraID FK, ProductoID FK, Cantidad, PrecioUnitario, Subtotal, CantidadRecibida INT DEFAULT 0).
   - `PagoProveedor` (PagoID PK, OrdenCompraID FK UNIQUE, Banco VARCHAR(60), NroComprobante VARCHAR(40), FechaPago DATE, Importe DECIMAL(12,2), UsuarioID). Índice único (Banco, NroComprobante).
   - `Recepcion` (RecepcionID PK, OrdenCompraID FK, NroRemito VARCHAR(30), FechaHora, UsuarioID, Observaciones NULL).
   - `RecepcionDetalle` (RecepcionDetalleID PK, RecepcionID FK, ProductoID FK, CantidadRecibida, Lote VARCHAR(40), FechaVencimiento DATE, MovimientoID INT NULL FK MovimientoStock). El remito no se puede repetir para el mismo proveedor: validarlo en la BLL, porque el proveedor está en la orden.
3. Agregar `Atendida BIT NOT NULL DEFAULT 0` a `AlertaReposicion`, para marcar las alertas incluidas en una solicitud.
4. Patentes nuevas (`CMP_SOLICITUD`, `CMP_ORDEN`, `CMP_APROBAR`, `CMP_PAGO`, `CMP_RECEPCION`, `REP_COMPRAS`). Quitar `STK_MOVIMIENTO`, `STK_CONFIG` y `STK_REPORTE`.
5. Familias:
   - Renombrar "Operario de stock" a **"Encargado de depósito"**: STK_PROD_ALTA, STK_PROD_MODIF, CMP_SOLICITUD, CMP_RECEPCION.
   - Crear **"Analista de compras"**: MAE_PROVEEDORES, CMP_ORDEN, CMP_PAGO, REP_COMPRAS.
   - Agregar CMP_APROBAR y REP_COMPRAS a **Administrador** (el gerente).
   - Agregar un usuario de prueba con la familia Analista de compras, igual que los demás usuarios de ejemplo.
6. Traducciones (es / en) de todos los formularios, columnas y mensajes nuevos, en la tabla `Traduccion`.
7. Volver a correr el generador para recalcular los DVH y DVV y regenerar `VetCareBD_60MN.sql`. Verificar que el script corre limpio en una base vacía y que "Dígitos verificadores" no informa errores.

## Fase 2. Entidades e interfaces (`Entidades_60MN`)

- `Producto_60MN`: agregar `Margen` (decimal).
- `EstadoOrden_60MN` (enum): PendienteAprobacion, Aprobada, Rechazada, Enviada, Pagada, RecibidaParcial, Cerrada, Anulada. Mapear contra los textos de la columna.
- `SolicitudReposicion_60MN` y `SolicitudDetalle_60MN`.
- `OrdenCompra_60MN` (con `List<OrdenCompraDetalle_60MN> Detalle`) y `OrdenCompraDetalle_60MN`.
- `Cotizacion_60MN`: solo en memoria, no se persiste aparte. Tiene ProveedorId, NroCotizacion, PlazoEntregaDias, CondicionPago, `Dictionary<int, decimal> Precios` y `Total`.
- `PagoProveedor_60MN`, `Recepcion_60MN` y `RecepcionDetalle_60MN`.
- `FiltroReporteCompra_60MN` y `FilaReporteCompra_60MN`.

## Fase 3. DAL y BLL

Crear las DAL siguiendo el estilo de `ProveedorDAL_60MN` / `MovimientoStockDAL_60MN`: SQL parametrizado y mapeo a entidades.

**CUN09 `ProductoBLL_60MN`**
- El alta propone `Margen` desde la categoría o el margen general.
- `CalcularPrecioVenta(costo, margen) = costo * (1 + margen / 100)`. El usuario puede editar el precio.
- El stock inicial de un alta es 0.
- `StockActual` no se edita desde el formulario: solo cambia por ventas y recepciones.
- Si el precio de venta queda menor que el de costo, se pide confirmación.

**CUN10 `SolicitudReposicionBLL_60MN`**
- `ListarProductosEnAlerta()`: productos activos con alerta activa y no atendida. La cantidad sugerida es `2 * StockMinimo - StockActual`, con mínimo 1.
- `Registrar(solicitud)`: valida al menos un ítem con cantidad > 0 y advierte si el producto ya está en una solicitud PENDIENTE o EN_COMPRA, o en una orden abierta. En una transacción inserta el encabezado y el detalle y marca las alertas como `Atendida = 1`. Después actualiza los DVH y registra el evento `SolicitudReposicionGenerada`.

**CUN11 `OrdenCompraBLL_60MN`**
- `ListarSolicitudesPendientes()` devuelve las solicitudes en PENDIENTE o EN_COMPRA, con lo que falta pedir.
- `Comparar(cotizaciones)` calcula el total de cada cotización y marca la de menor costo.
- `Registrar(orden)`:
  - Valida número de cotización obligatorio, precios > 0 y cantidades > 0.
  - Si se elige una cotización que no es la más barata, exige `MotivoEleccion`.
  - Si un precio supera en más del 20 % el `PrecioCosto` actual, guarda una advertencia que el gerente ve al aprobar. Puede ir en `MotivoEleccion` o en una columna extra.
  - Inserta la orden en PENDIENTE_APROBACION y pasa la solicitud a EN_COMPRA.
  - Registra el evento `OrdenCompraRegistrada`.
- `Anular(id, motivo)` y la reapertura de una orden RECHAZADA, que se corrige y vuelve a PENDIENTE_APROBACION.

**CUN12 (`OrdenCompraBLL_60MN`)**
- `Aprobar(id)` y `Rechazar(id, motivo)`:
  - Validan que el estado siga siendo PENDIENTE_APROBACION, con concurrencia: `UPDATE ... WHERE Estado = 'PENDIENTE_APROBACION'`, y si afecta 0 filas se informa que la orden cambió.
  - **Separación de funciones**: el usuario que aprueba no puede ser el que registró la orden.
  - Rechazar exige motivo.
  - Guardan AprobadaPor y FechaAprobacion.
  - Eventos `OrdenCompraAprobada` y `OrdenCompraRechazada`, con criticidad alta.
- Si existe la clave `Configuracion.TopeCompraMensual` y la suma de las órdenes aprobadas del mes más la actual lo supera, se advierte y se pide confirmación.

**CUN13 `PagoProveedorBLL_60MN`**
- `ListarOrdenesAPagar()` devuelve las órdenes APROBADA y ENVIADA.
- `MarcarEnviada(id)` pasa la orden a ENVIADA y devuelve el PDF de la orden. Reutilizar el exportador PDF del proyecto; si todavía no existe, usar el de la fase del Strategy de `Prompt - Pendientes IS2026 (codigo).md`.
- `Registrar(pago)`:
  - Valida que el comprobante no esté repetido para ese banco.
  - Valida que la fecha no sea futura ni anterior a la aprobación.
  - Si el importe es distinto del total, pide confirmación y motivo.
  - En una transacción inserta el pago y pasa la orden a PAGADA.
  - Registra el evento `PagoProveedorRegistrado`.

**CUN14 `RecepcionBLL_60MN`**
- `ObtenerOrdenParaRecibir(numero)` solo acepta órdenes PAGADA o RECIBIDA_PARCIAL y devuelve las cantidades pendientes.
- `Registrar(recepcion)`, en **una sola transacción**:
  1. Valida el remito no repetido para el proveedor.
  2. Valida que lo recibido no supere lo pendiente.
  3. Valida el vencimiento: si ya pasó, rechaza; si faltan menos de 90 días, advierte.
  4. Inserta la recepción y su detalle.
  5. Por cada producto con cantidad > 0, inserta un `MovimientoStock` INGRESO con el motivo "Recepción OC-xxxxx / remito yyy", actualiza `StockActual`, `Lote` y `FechaVencimiento` del producto con concurrencia optimista, y guarda el `MovimientoID` en el detalle.
  6. Suma `CantidadRecibida` en `OrdenCompraDetalle`.
  7. Si todo quedó recibido, pasa la orden a CERRADA y la solicitud a CERRADA; si no, a RECIBIDA_PARCIAL.
  8. Desactiva las alertas que dejaron de cumplirse.
  Si algo falla, hace rollback. Después actualiza los DVH de todas las tablas afectadas y registra el evento `MercaderiaRecibida`. El trigger de Productos alimenta `Productos_C` (CUS10).

**CUN15 `ReporteComprasBLL_60MN`**
- `Generar(filtro)`: desde ≤ hasta.
- Devuelve las órdenes del período con proveedor, estado, total, importe pagado y fecha de la última recepción.
- Calcula subtotales por proveedor y totales: comprado, pagado y pendiente de recibir en unidades.
- Registra el evento `ReporteComprasGenerado`.

**CUN16 Reporte Inteligente**
- Solo se renombra la referencia en comentarios y documentación. No cambia la lógica.

**Eventos nuevos en `CatalogoEventos_60MN`**
- Crear un módulo `Compras`.
- Los eventos de aprobación, rechazo y pago van con criticidad más alta que los de solicitud, orden y recepción.
- El reporte va con la más baja.
- Respetar el criterio que ya usa el catálogo.

## Fase 4. GUI (`VetCare/Compras/`)

Menú principal: **ADMIN, MAESTROS, PACIENTES, VENTAS, COMPRAS, REPORTES, USUARIO, AYUDA**. COMPRAS reemplaza a STOCK.

- **COMPRAS > Solicitud de reposición** (`FrmSolicitudReposicion_60MN`, Tag `CMP_SOLICITUD`).
  - Grilla de productos en alerta con columnas: check, Código, Producto, Proveedor habitual, Stock, Mínimo, Sugerido y "A pedir" (editable).
  - Buscador para agregar un producto que no está en alerta.
  - Campo de observación y botones "Enviar solicitud" y "Cancelar".
- **COMPRAS > Órdenes de compra** (`FrmOrdenCompra_60MN`, Tag `CMP_ORDEN`).
  - Combo de solicitudes pendientes.
  - Grilla de cotizaciones (Proveedor, N° cotización, Total, Plazo, Condición) con los botones "Cargar cotización" y "Quitar". "Cargar cotización" abre un diálogo con el proveedor y el precio de cada producto.
  - Detalle de la cotización elegida, campo "Motivo de elección" y botón "Generar orden".
  - Una pestaña o lista "Mis órdenes", con las rechazadas visibles para corregirlas o anularlas.
- **COMPRAS > Aprobar órdenes** (`FrmAprobarOrden_60MN`, Tag `CMP_APROBAR`).
  - Grilla de órdenes pendientes con advertencias.
  - Detalle de la orden y panel "Origen" con la solicitud y las cotizaciones.
  - Campo de motivo de rechazo y botones "Aprobar" y "Rechazar".
- **COMPRAS > Pagos a proveedores** (`FrmPagoProveedor_60MN`, Tag `CMP_PAGO`).
  - Grilla de órdenes aprobadas o enviadas y botón "Enviar orden", que guarda el PDF.
  - Grupo "Registrar pago" con Banco (combo editable), N° comprobante, Fecha e Importe, y una leyenda que aclare que el pago se hace en el home banking.
- **COMPRAS > Recepción de mercadería** (`FrmRecepcion_60MN`, Tag `CMP_RECEPCION`).
  - Campo N° de orden con botón "Buscar" y campo N° de remito.
  - Grilla con Producto, Pedido, Pendiente, Recibido, Lote, Vencimiento y Observación (las cuatro últimas editables).
  - Botón "Confirmar recepción" y una etiqueta con el estado resultante.
- **REPORTES > Reporte de compras** (`FrmReporteCompras_60MN`, Tag `REP_COMPRAS`).
  - Filtros Desde, Hasta, Proveedor y Estado, y botón "Generar".
  - Grilla de órdenes y grilla de totales por proveedor.
  - Botones "Exportar PDF" e "Imprimir".
- **MAESTROS > Productos**: agregar el campo Margen (%) en `FrmProducto_60MN`. El precio de venta se recalcula al cambiar el costo o el margen, con un botón "Usar sugerido". El campo de stock queda solo lectura, y en el alta muestra 0.
- **MAESTROS > Proveedores**: sin cambios funcionales.

En todos los formularios:

- Textos con `T(...)` y traducción en inglés.
- Validaciones con mensajes claros.
- Tooltips si ya se implementó la fase de tooltips.
- Ayuda F1 con un tema en `TemasAyuda_60MN` por cada formulario nuevo.

## Fase 5. Verificación

1. Compilar toda la solución sin advertencias nuevas.
2. Recrear la base con el script y entrar con el administrador, con un usuario Encargado de depósito y con un usuario Analista de compras. Cada uno ve solo su menú.
3. Recorrer el circuito completo:
   1. Bajar el stock de un producto con una venta, para que se genere la alerta.
   2. Generar la solicitud.
   3. Cargar dos cotizaciones y generar la orden.
   4. Intentar aprobarla con el mismo usuario que la cargó: tiene que fallar.
   5. Aprobarla como gerente.
   6. Enviarla y registrar el pago.
   7. Recibir una parte (la orden queda en RECIBIDA_PARCIAL) y después el resto (la orden queda en CERRADA).
   8. Verificar que el stock sube, que hay movimientos INGRESO, que hay una fila en `Productos_C` y que la alerta se desactiva.
4. Probar el rechazo con motivo y la corrección de la orden, el comprobante repetido y el remito repetido.
5. Correr "Dígitos verificadores": no tiene que haber diferencias.
6. Revisar la bitácora de eventos: están todos los eventos nuevos con su criticidad.
7. Generar el reporte de compras con distintos filtros y exportarlo a PDF.
8. Actualizar `README_VetCare_60MN.md` con el nuevo menú, las familias y la numeración CUN08 a CUN16.

Trabajá fase por fase y mostrame un resumen de los archivos tocados al final de cada una. Si encontrás algo del código actual que contradiga este prompt, avisame antes de decidir.
