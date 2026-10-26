# VetCare – guía técnica de la implementación

## Puesta en marcha
Plataforma: **.NET Framework 4.8** (WinForms, C# con `LangVersion latest`), proyectos con formato SDK. Se abre con
Visual Studio 2022 o se compila con `dotnet build VetCare.sln`.

1. Ejecutar `BDScript/VetCareBD_60MN.sql` en SQL Server (SSMS o `sqlcmd -S .\SQLEXPRESS01 -E -C -f 65001 -i BDScript\VetCareBD_60MN.sql`).
   **Borra y recrea** `VetCareBD_60MN`.
2. Ajustar `conexionBD` en `VetCare/App.config` si la instancia es otra.
3. Ejecutar el proyecto `VetCare`.

| Usuario | Contraseña | Permisos |
|---|---|---|
| admin | Admin1234 | Familia Administrador (acceso total, incluye todas las familias) |
| operario | Operario1234 | Familia Operario de stock (CUN11, CUN12, CUN14, CUN15) + patente individual `STK_CONFIG` (CUN13) |
| vendedor | Vendedor1234 | Familia Vendedor: clientes y mascotas (CUN01–CUN04) y carrito de medicación (CUN09) |
| veterinario | Veterinario1234 | Familia Veterinario: consulta de clientes, historia clínica, atención, carrito e informe (CUN05–CUN10) |
| emergencia | Emergencia1234 | Solo en `UsuarioEmergencia_60MN.xml`, no en la base |

## Deshabilitado por ahora: Gestión de Stock y Bitácora de cambios
Por ahora solo funcionan Gestión de Pacientes y Seguridad. Nada se borró; para reactivar:
1. `VetCare/VetCare.csproj`: quitar el `ItemGroup` marcado "DESHABILITADO POR AHORA" (saca de la compilación `Stock/**`,
   `FrmBitacoraCambios_60MN` y `FrmCompararVersiones_60MN`).
2. `VetCare/FrmMenuPrincipal_60MN.cs`: vaciar `OpcionesDeshabilitadas` y descomentar los `Abrir<...>()` de
   `mnuProductos`, `mnuMovimientosStock`, `mnuConfiguracionStock`, `mnuReporte1` y `mnuBitacoraCambios`.
3. `Herramientas_60MN/GeneradorScriptBD_60MN/Estructura.sql`: quitar el `DISABLE TRIGGER TR_Productos_BitacoraCambios`
   y regenerar el script.

La BLL/DAL de productos y movimientos sigue compilando porque el Carrito de medicación (CUN09) la usa: cada entrega
descuenta stock y genera su egreso. Las patentes y familias de stock siguen en la base sin cambios (Seguridad intacta);
el usuario `operario` por ahora solo ve USUARIO y AYUDA.

## Casos de uso implementados (Trabajo de Diploma V2.1)
- Seguridad: CUS01–CUS13 (usuarios, perfiles, login/logout, bitácoras, idioma, **CUS12 Gestión de Backup** y dígitos verificadores).
- Gestión de Pacientes (MAESTROS > Clientes y pacientes, VENTAS > Carrito): CUN01 Registrar Cliente, CUN02 Modificar Cliente,
  CUN03 Consultar Cliente, CUN04 Gestionar Mascota, CUN05 Crear Historia Clínica, CUN06 Consultar Historia Clínica,
  CUN07 Actualizar Historia Clínica, CUN08 Registrar Atención, CUN09 Registrar Medicación en Carrito, CUN10 Emitir Informe de Atención.
- Gestión de Stock (MAESTROS, REPORTES): CUN11 Registrar Producto, CUN12 Modificar Producto, CUN13 Configurar Margen y Alerta
  de Stock Mínimo, CUN14 Registrar Movimiento de Stock, CUN15 Generar Reporte de Existencias.
- Fuera del alcance (quedan en el menú como "en desarrollo"): Proveedores, Facturar, Despachar, Compras, Reporte 2 y 3, Ayuda en línea.

| Pantalla | Casos de uso |
|---|---|
| `Pacientes/FrmClientes_60MN` | CUN03 (búsqueda y ficha), acceso a CUN01, CUN02, CUN04, CUN05/06, CUN08 |
| `Pacientes/FrmCliente_60MN`, `FrmMascotas_60MN` | CUN01, CUN02, CUN04 |
| `Pacientes/FrmHistoriaClinica_60MN` | CUN05 (alta), CUN06 (consulta e impresión), CUN07 (modificación con historial en `HistoriaClinica_H`) |
| `Pacientes/FrmAtencion_60MN` | CUN08 (y desde ahí «extend» CUN09 y CUN10) |
| `Pacientes/FrmCarrito_60MN` | CUN09 («include» CUN14: cada entrega genera un EGRESO de stock) |
| `Pacientes/FrmInformeAtencion_60MN` | CUN10 (impresora o PDF con "Microsoft Print to PDF"; reimpresiones en bitácora) |
| `Stock/FrmConfiguracionStock_60MN` | CUN13 |
| `Stock/FrmReporteExistencias_60MN` | CUN15 (impresión/PDF y exportación CSV) |
| `Seguridad/FrmBackupRestore_60MN` | CUS12 |

## Bitácora de eventos: exportar / importar XML (serialización)
ADMIN > Bitácora de eventos → botones **Ver XML...**, **Exportar XML...** (`SEG_BITACORA_EXPORT`, familia Auditoría) e
**Importar XML...** (`SEG_BITACORA_IMPORT`, familia Administrador).
- Recorrido: `FrmBitacoraEventos_60MN` → `BitacoraBLL_60MN` (permiso, integridad, eventos) →
  `Seguridad_60MN/Auditoria/SerializadorBitacora_60MN` (XmlSerializer, firmas, validación, importación) → tabla `Bitacora` + DVH/DVV.
- El XML (`ArchivoBitacora_60MN` / `EventoBitacoraXml_60MN`) lleva la descripción **cifrada** (AES, igual que en la base),
  una firma HMAC-SHA256 por evento (como un DVH) y una firma del archivo (como un DVV).
- Importar: lectura sin DTD ni entidades externas (anti-XXE), máximo 20 MB / 20.000 eventos, validación de firmas, catálogo
  de eventos, fechas y longitudes; todo-o-nada, sin duplicados, y recalculando DVH/DVV. Se exige que el archivo sea el
  mismo que se previsualizó (SHA-256). Eventos: `BitacoraExportada`, `BitacoraImportada`, `BitacoraImportacionRechazada`.

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
  Bitacora, Productos, MovimientoStock, Cliente, Mascota, HistoriaClinica, Atencion, AtencionDetalle.
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
Horizontal: ADMIN, MAESTROS, USUARIO, VENTAS, COMPRAS, REPORTES, AYUDA (`FrmMenuPrincipal_60MN`, armado en el diseñador).
Cada ítem tiene en `Tag` la(s) patente(s) que lo habilitan; un menú sin ítems visibles se oculta.
Las opciones fuera del alcance (Proveedores, Facturar, Despachar, Compras, Reporte 2 y 3) tienen patente propia
(`MAE_PROVEEDORES`, `VEN_FACTURAR`, ...) y muestran "en desarrollo". El Administrador tiene todas porque su familia incluye
a las demás (Gestión de Usuarios, Auditoría, Operario de stock, Vendedor y Veterinario).
USUARIO > Re-Login cierra la sesión y vuelve al Login; USUARIO > Logout cierra la sesión y sale de la aplicación.

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
  - Base existente: aplicar `BDScript/Migracion_IdiomaPorUsuario_60MN.sql` y luego ADMIN > Dígitos verificadores > Recalcular.
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
