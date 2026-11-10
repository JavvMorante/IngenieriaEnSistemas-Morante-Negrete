# VetCare – guía técnica de la implementación

## Puesta en marcha
Plataforma: **.NET Framework 4.8** (WinForms, C# con `LangVersion latest`), proyectos con formato SDK. Se abre con
Visual Studio 2022 o se compila con `dotnet build VetCare.sln`.

**Instalación** (único script de base de datos)
1. Ejecutar `BDScript/VetCareBD_60MN.sql` en SQL Server (SSMS o `sqlcmd -S .\SQLEXPRESS01 -E -C -f 65001 -i BDScript\VetCareBD_60MN.sql`).
   **Borra y recrea** `VetCareBD_60MN` con el esquema V2.4 y los datos de prueba (con sus DVH y DVV).
2. Ajustar `conexionBD` en `VetCare/App.config` si la instancia es otra.
3. Ejecutar el proyecto `VetCare`.

No hay scripts de migración: una base de una versión anterior se reinstala con este script (pierde sus datos; hacer
antes un backup si se los quiere conservar).

| Usuario | Contraseña | Permisos |
|---|---|---|
| admin | Admin1234 | Familia Administrador (acceso total, incluye todas las familias) |
| operario | Operario1234 | Familia Operario de stock (CUN08, CUN09, CUN11, CUN12) + patente individual `STK_CONFIG` (CUN10) |
| vendedor | Vendedor1234 | Familia Vendedor: clientes (CUN01), mascotas (CUN02), carrito y ventas registradas (CUN05) |
| veterinario | Veterinario1234 | Familia Veterinario: consulta de clientes, CUN03 a CUN07 y reporte inteligente (CUN13) |
| emergencia | Emergencia1234 | Solo en `UsuarioEmergencia_60MN.xml`, no en la base |

El detalle de perfiles, patentes y menú de cada usuario está en `Usuarios_y_Permisos_VetCare_60MN.md` (raíz del repositorio).

## Estado
Implementados los casos de uso del Trabajo de Diploma **V2.4**: los dos procesos de negocio, la seguridad, la bitácora de
cambios y la ayuda en línea (F1). **El sistema no factura**: el carrito registra ventas, sin comprobantes ni despacho.

El trigger `TR_Productos_BitacoraCambios` está activo: guarda las versiones de cada producto en `Productos_C`
(postcondición de CUN08 y CUN09). Solo tienen historial los productos creados o modificados desde que el trigger está activo.

Validaciones en la BLL con error por campo (la pantalla filtra lo que se tipea y marca el campo):
- Cliente: documento según su tipo (DNI/LE/LC 7–8 dígitos, CUIL con dígito verificador, CI, pasaporte), nombre, apellido,
  dirección, teléfono (solo dígitos) y mail.
- Proveedor: CUIT de 11 dígitos con dígito verificador, único y no modificable; razón social, teléfono y dirección obligatorios.
- Producto: nombre (letras, números y `. , - / % ( ) + '`, hasta 80), lote (A–Z, números, `-` y `/`, hasta 20); motivo de
  movimiento (texto con palabras, hasta 150).

Manual de usuario de los dos procesos: https://claude.ai/artifact/4M31Q78g2iPzFQyTQN1LbH

## Casos de uso implementados (Trabajo de Diploma V2.4)
- Seguridad: CUS01–CUS13 (usuarios, perfiles, login/logout, bitácoras, idioma, CUS12 Gestión de Backup y dígitos verificadores).

| PN1 – Gestión de Pacientes | PN2 – Gestión de Stock |
|---|---|
| CUN01 Gestionar Clientes | CUN08 Administrar Existencias |
| CUN02 Gestionar Mascotas | CUN09 Registrar Movimiento de Stock |
| CUN03 Gestionar Historias Clínicas | CUN10 Configurar Margen y Alerta de Stock Mínimo |
| CUN04 Registrar Atención | CUN11 Gestionar Proveedores |
| CUN05 Registrar Medicación en Carrito | CUN12 Generar Reporte de Existencias |
| CUN06 Emitir Informe de Atención | CUN13 Generar Reporte Inteligente (cruza PN1 y PN2) |
| CUN07 Generar Reporte de Atenciones | |

| Pantalla | Casos de uso |
|---|---|
| `Pacientes/FrmClientes_60MN`, `FrmCliente_60MN` | CUN01 (búsqueda, ficha, alta, modificación, baja); desde la ficha, mascotas, historia clínica y atención |
| `Pacientes/FrmGestionMascotas_60MN`, `FrmMascotas_60MN`, `FrmBuscarCliente_60MN` | CUN02 (para añadir se elige primero el cliente) |
| `Pacientes/FrmBuscarPaciente_60MN` | Selector de paciente de PACIENTES > Historias clínicas y Registrar atención |
| `Pacientes/FrmHistoriaClinica_60MN` | CUN03 (alta, consulta e impresión, actualización con historial en `HistoriaClinica_H`) |
| `Pacientes/FrmAtencion_60MN` | CUN04: motivo y diagnóstico obligatorios, indicaciones opcionales; desde ahí «extend» CUN05 y CUN06 |
| `Pacientes/FrmCarrito_60MN` | CUN05: carrito en memoria (`Carrito_60MN`); al confirmar, venta + egreso de stock por ítem («include» CUN09) en una transacción. Desde la atención o desde VENTAS > Carrito eligiendo cliente, con o sin atención |
| `Pacientes/FrmVentas_60MN` | VENTAS > Ventas registradas: consulta por período y cliente, con el detalle (solo consulta) |
| `Pacientes/FrmInformeAtencion_60MN` | CUN06 (impresora o PDF con "Microsoft Print to PDF"; la medicación sale de la venta de la atención) |
| `Reportes/FrmReporteAtenciones_60MN` | CUN07 (filtros por veterinario y especie, totales por profesional, impresión/PDF y CSV) |
| `Stock/FrmProductos_60MN`, `FrmProducto_60MN` | CUN08 (precio sugerido por margen, lote duplicado → ingreso, stock bloqueado: se ajusta con CUN09) |
| `Stock/FrmMovimientoStock_60MN` | CUN09 (ingreso, egreso, ajuste, baja por vencimiento, alerta de reposición) |
| `Stock/FrmConfiguracionStock_60MN` | CUN10 |
| `Stock/FrmProveedores_60MN`, `FrmProveedor_60MN` | CUN11 (productos que provee, baja lógica con aviso si tiene productos activos) |
| `Stock/FrmReporteExistencias_60MN` | CUN12 (impresión/PDF y exportación CSV) |
| `Reportes/FrmReporteInteligente_60MN` | CUN13 |
| `Utilidades/FrmAyuda_60MN` | Ayuda en línea (F1 en cualquier pantalla o AYUDA > Ayuda en línea; temas en `TemasAyuda_60MN`) |
| `Seguridad/FrmBackupRestore_60MN` | CUS12 |

## Bitácora de eventos: serializar / deserializar XML
ADMIN > Bitácora de eventos → botones **Exportar XML...** (serializa; `SEG_BITACORA_EXPORT`, familia Auditoría) y
**Deserializar XML...** (`SEG_BITACORA_IMPORT`, familia Administrador).
- Recorrido: `FrmBitacoraEventos_60MN` → `BitacoraBLL_60MN` (permiso, integridad, eventos) →
  `Seguridad_60MN/Auditoria/SerializadorBitacora_60MN` (XmlSerializer, firmas, validación).
- El XML (`ArchivoBitacora_60MN` / `EventoBitacoraXml_60MN`) lleva la descripción **cifrada** (AES, igual que en la base),
  una firma HMAC-SHA256 por evento (como un DVH) y una firma del archivo (como un DVV).
- Deserializar: se elige un XML exportado antes y sus eventos se muestran en `FrmDeserializarBitacora_60MN` (grilla y Salir,
  ventana del menú principal). No modifica la base. Lectura sin DTD ni entidades externas (anti-XXE), máximo 20 MB / 20.000
  eventos, validación de firmas, catálogo de eventos, fechas y longitudes: un archivo alterado se rechaza y no se muestra.
  Eventos: `BitacoraExportada`, `BitacoraDeserializada`, `BitacoraDeserializacionRechazada` (los viejos `BitacoraImportada` y
  `BitacoraImportacionRechazada` quedan en el catálogo solo para leer registros anteriores).

## Backup y restore (CUS12)
ADMIN > Backup o ADMIN > Restore abren `FrmBackupRestore_60MN`; cada sección se ve según la patente (`SEG_BACKUP` / `SEG_RESTORE`).
- `BackupBLL_60MN` → `BackupDAL_60MN`: `BACKUP DATABASE ... WITH CHECKSUM` a `VetCareBD_60MN_aaaaMMdd_HHmmss.bak` y
  `RESTORE DATABASE ... WITH REPLACE` (usuario único → restaurar → multiusuario), conectando a `master`. La ruta viaja como
  parámetro y el nombre de la base se arma con `QUOTENAME`.
- No se respalda una base con inconsistencias de DV (3.1). Antes de restaurar se valida el archivo con `RESTORE HEADERONLY`
  (que sea de esta base y completo) y `RESTORE VERIFYONLY` (8.1).
- Al restaurar se verifican los DV de la base restaurada, se registra `RestoreRealizado` y se cierra la sesión.
  Se pierden los cambios posteriores al backup, incluida la bitácora (`RestoreIniciado` solo queda si la restauración falla).
- **Cualquier carpeta, en dos pasos.** El .bak lo escribe y lo lee el **servicio de SQL Server**, que solo tiene permiso
  sobre su carpeta de backups. Por eso: al respaldar, SQL Server lo genera en su carpeta y VetCare lo copia a la carpeta
  elegida (verificando la copia con SHA-256) y borra el intermedio; al restaurar, VetCare copia el archivo elegido a la
  carpeta de SQL Server, lo valida, restaura y borra la copia. Requiere SQL Server en el mismo equipo y que el usuario de
  Windows pueda escribir en la carpeta de backups de la instancia. Carpeta sugerida: `Documentos\Backups VetCare`.
- El historial de backups se lee de `msdb` (sobrevive a la restauración) y muestra la carpeta final elegida.
- Un backup anterior a cambios del esquema (por ejemplo, de antes de agregar las tablas de pacientes) deja la base con el
  esquema viejo: restaurar solo backups generados con esta versión.

## Seguridad (T03 / T06)
- **Contraseñas (T03.1)**: SHA-256 con sal aleatoria por usuario – `Seguridad_60MN/Criptografia/PasswordHasher_60MN`.
- **Datos sensibles (T03.2)**: AES-256 (DNI y Email del usuario, descripción de la bitácora) – `CriptografiaHandler_60MN`.
  Clave: `VETCARE_CLAVE_CRIPTO` del App.config.
- **Dígitos verificadores**: DVH por fila y DVV por tabla con HMAC-SHA256 (`VETCARE_CLAVE_DV`) –
  `Seguridad_60MN/Integridad`. Tablas: Usuario, Patente, Familia, FamiliaPatente, FamiliaFamilia, UsuarioFamilia, UsuarioPatente,
  Bitacora, Productos, MovimientoStock, Cliente, Mascota, HistoriaClinica, Atencion, Proveedor, VentaEncabezado y VentaDetalle
  (registradas en `TablaDV_60MN`; `AtencionDetalle` se reemplazó por las tablas de venta en V2.4).
  Se verifican al iniciar sesión: si hay inconsistencias solo ingresan usuarios con la patente `SEG_INTEGRIDAD`.
  Un registro de usuario alterado no puede iniciar sesión.
- **Permisos**: modelo Usuario - Familia - Patente con patrón Composite (ver sección "Modelo de permisos").
  Cada ítem de menú y botón tiene en `Tag` el código de patente que lo habilita (editable desde el diseñador).
- **Login**: 3 intentos fallidos bloquean al usuario; primer ingreso obliga a cambiar la contraseña (CUS05 «extend» CUS07).
- **Usuario de emergencia**: `VetCare/UsuarioEmergencia_60MN.xml` (hash + firma HMAC; si se edita a mano queda inválido).
  Solo se acepta cuando no existe un administrador operativo (activo, no bloqueado, con DVH válido y patente `SEG_ROLES`),
  por ejemplo si el admin fue borrado de la base. Entra con todas las patentes para crear un nuevo administrador.
- **Bitácora de cambios (T06B)**: trigger `TR_Productos_BitacoraCambios` → `Productos_C` con `Act = 1` en la versión vigente.

## Modelo de permisos: Usuario - Familia - Patente
| Tabla | Contenido |
|---|---|
| `Patente` | Permiso atómico sobre una operación (código `SEG_USU_ALTA`, `STK_MOVIMIENTO`, ...) — hoja del Composite |
| `Familia` | Conjunto de patentes = tipo de usuario (Administrador, Operario de stock, Vendedor, Veterinario...) — compuesto |
| `FamiliaPatente` | Patentes que componen cada familia |
| `FamiliaFamilia` | Familias incluidas dentro de otra (p. ej. Vendedor incluye Ventas; Administrador incluye todas) |
| `UsuarioFamilia` | Familia(s) asignadas al usuario |
| `UsuarioPatente` | Patentes individuales del usuario, además de las de su familia |

- Código: `Patente_60MN` / `Familia_60MN` / `Componente_60MN` (Servicios_60MN/Composite), `PermisoDAL_60MN`,
  `ModeloPermisos_60MN` y `PermisoBLL_60MN` (BLL). El usuario en sesión tiene `Permisos` = sus familias + sus patentes
  individuales; `SessionManager_60MN.TienePermiso` recorre ese árbol.
- GUI: ADMIN > Perfiles (`FrmFamiliasPatentes_60MN`) arma cada familia; ADMIN > Usuarios > **Permisos**
  (`FrmPermisosUsuario_60MN`) asigna al usuario sus familias y patentes individuales. En el alta se elige la familia inicial.
- Reglas: todo usuario tiene al menos una familia; nadie modifica sus propios permisos; no se permiten ciclos entre
  familias; ningún cambio puede dejar al sistema sin un administrador operativo (se simula antes de guardar).

## Menú principal
Horizontal (`FrmMenuPrincipal_60MN`, armado en el diseñador). Cada ítem tiene en `Tag` la(s) patente(s) que lo
habilitan; un menú sin ítems visibles se oculta.

| Menú | Ítems (Tag = patentes) |
|---|---|
| ADMIN | Usuarios, Perfiles, Backup, Restore, Bitácora de eventos, Bitácora de cambios (`SEG_BITACORA_CAMBIOS`), Dígitos verificadores, Configuración de stock (`STK_CONFIG`) |
| MAESTROS | Clientes (`PAC_CLIENTE_CONSULTA\|PAC_CLIENTE_ALTA\|PAC_CLIENTE_MODIF`), Mascotas (`PAC_MASCOTA`), Productos (`STK_PROD_ALTA\|STK_PROD_MODIF`), Proveedores (`MAE_PROVEEDORES`) |
| PACIENTES | Historias clínicas (`HC_CONSULTA\|HC_ALTA\|HC_MODIF`), Registrar atención (`ATN_REGISTRAR`): ambos abren primero el selector de paciente |
| VENTAS | Carrito, Ventas registradas (`VEN_CARRITO`) |
| STOCK | Movimientos de stock (`STK_MOVIMIENTO`) |
| REPORTES | Reporte de atenciones (`REP_REPORTES`), Reporte de existencias (`STK_REPORTE`), Reporte inteligente (`REP_REPORTES`) |
| USUARIO | Re-Login, Cambiar clave, Logout, Cambiar idioma (sin patente) |
| AYUDA | Ayuda en línea (F1), Acerca de VetCare |

El Administrador ve todo porque su familia incluye a las demás (Gestión de Usuarios, Auditoría, Operario de stock,
Vendedor y Veterinario). USUARIO > Re-Login cierra la sesión y vuelve al Login; USUARIO > Logout cierra la sesión y sale.

## Ayuda en línea (F1)
`FormBase_60MN.ProcessCmdKey` atrapa F1 en cualquier pantalla y abre `FrmAyuda_60MN` con el tema de la ventana activa
(en el menú principal, el de la ventana hija activa); AYUDA > Ayuda en línea hace lo mismo. Los temas (uno por caso de uso
y por pantalla de seguridad) están en `Utilidades/TemasAyuda_60MN.cs`: el español es el texto base y el inglés está en
`Traduccion` (claves `ayuda.<tema>.titulo` y `ayuda.<tema>.texto`, hasta 400 caracteres).

## Multi-idioma (patrón Observer)
| Pieza | Dónde | Rol |
|---|---|---|
| `IObservadorIdioma_60MN`, `IIdioma_60MN` | Interfaces_60MN | Contratos del Observer |
| `GestorIdioma_60MN` | Servicios_60MN/Idioma | **Sujeto** (Singleton): idioma actual + diccionario; `Suscribir`, `Desuscribir`, `Notificar` |
| `FormBase_60MN` | VetCare/Utilidades | **Observador concreto**: todos los formularios heredan de él; se suscribe al cargarse y se traduce |
| `TraductorFormularios_60MN` | VetCare/Utilidades | Recorre controles, columnas y menús y aplica la traducción |
| `IdiomaBLL_60MN` / `IdiomaDAL_60MN` | BLL / DAL | Leen `Idioma` y `Traduccion`; guardan el idioma de cada usuario (`Usuario.IdiomaID`) |
| Tablas `Idioma`, `Traduccion` | BD | Traducciones por clave (`Formulario.Control` o `Control`) |

- El español es el idioma **base**: sus textos son los del diseñador, por eso solo el inglés tiene filas en `Traduccion`.
- Para agregar un idioma: una fila en `Idioma` y sus filas en `Traduccion` (sin tocar el código).
- **El idioma es por usuario** (`Usuario.IdiomaID`, protegido por el DVH): cada usuario tiene guardado su último idioma.
  - El Login abre siempre en español (`Program.cs` → `AplicarBase`) y vuelve a español al cerrar la sesión.
  - Al ingresar (`FrmLogin_60MN` → `IdiomaBLL_60MN.AplicarIdiomaAlIngresar`): si eligió un idioma en el combo del Login,
    se guarda como el suyo; si no, se le aplica su último idioma. El usuario de emergencia usa siempre español.
  - USUARIO > Cambiar idioma (`CambiarIdiomaUsuario`) guarda la elección contra el usuario y registra `IdiomaCambiado`.
- Limitación: los mensajes de las reglas de negocio (excepciones de la BLL) y las etiquetas que arman datos en tiempo
  de ejecución siguen en español; los textos fijos de pantallas, menú, grillas y mensajes generales sí se traducen.

## Flujo de llamadas (para los diagramas de secuencia)
`Frm*_60MN` (GUI) → `*BLL_60MN` (BLL) → `*DAL_60MN` (DAL, SQL parametrizado) → BD
- Cada BLL, después de escribir, llama a `GestorDigitoVerificador_60MN.ActualizarFila(tabla, id)` (Seguridad).
- Registrar Evento: `BitacoraBLL_60MN.Registrar` → toma el usuario de `SessionManager_60MN` → `GestorBitacora_60MN.Registrar`
  (cifra con AES, inserta y actualiza DVH/DVV).
- Autorización: el menú oculta opciones y además cada método de la BLL verifica la patente (`SesionActual_60MN.RequierePermiso`).

## Regenerar el script
Si se cambian las claves del App.config, los hashes/DV del script dejan de coincidir. Regenerar con:
```
dotnet run --project Herramientas_60MN/GeneradorScriptBD_60MN
```
(ejecutar desde la carpeta de la solución; reescribe `BDScript/VetCareBD_60MN.sql` y `VetCare/UsuarioEmergencia_60MN.xml`).

## Código anterior
Los formularios y clases previos (SQL concatenado, 3DES, esquema viejo) se movieron a `Legacy_60MN/` (no se compila).
El backup de la base anterior quedó en la carpeta de backups de SQL Server:
`VetCareBD_60MN_previo_rediseno_20260926.bak`.
