# VetCare – guía técnica de la implementación

## Puesta en marcha
1. Ejecutar `BDScript/VetCareBD_60MN.sql` en SQL Server (SSMS o `sqlcmd -S .\SQLEXPRESS01 -E -C -f 65001 -i BDScript\VetCareBD_60MN.sql`).
   **Borra y recrea** `VetCareBD_60MN`.
2. Ajustar `conexionBD` en `VetCare/App.config` si la instancia es otra.
3. Ejecutar el proyecto `VetCare`.

| Usuario | Contraseña | Permisos |
|---|---|---|
| admin | Admin1234 | Familia Administrador (acceso total) |
| operario | Operario1234 | Familia Operario de stock (CUN11, CUN12, CUN14) + patente individual Reportes |
| emergencia | Emergencia1234 | Solo en `UsuarioEmergencia_60MN.xml`, no en la base |

## Casos de uso implementados
- Seguridad: CUS01–CUS10 (+ Registrar Evento transversal, verificación de integridad).
- Negocio: CUN11 Registrar Producto, CUN12 Modificar Producto, CUN14 Registrar Movimiento de Stock.

## Seguridad (T03 / T06)
- **Contraseñas (T03.1)**: SHA-256 con sal aleatoria por usuario – `Seguridad_60MN/Criptografia/PasswordHasher_60MN`.
- **Datos sensibles (T03.2)**: AES-256 (DNI y Email del usuario, descripción de la bitácora) – `CriptografiaHandler_60MN`.
  Clave: `VETCARE_CLAVE_CRIPTO` del App.config.
- **Dígitos verificadores**: DVH por fila y DVV por tabla con HMAC-SHA256 (`VETCARE_CLAVE_DV`) –
  `Seguridad_60MN/Integridad`. Tablas: Usuario, Patente, Familia, FamiliaPatente, FamiliaFamilia, UsuarioFamilia, UsuarioPatente, Bitacora, Productos, MovimientoStock.
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
Las opciones aún no desarrolladas (Backup, Restore, Clientes, Proveedores, Ventas, Compras, Reportes) tienen patente
propia (`SEG_BACKUP`, `VEN_CARRITO`, ...) y muestran "en desarrollo". El Administrador tiene todas; la familia
"Ventas" agrupa Carrito/Facturar/Despachar y forma parte de la familia "Vendedor".
USUARIO > Re-Login cierra la sesión y vuelve al Login; USUARIO > Logout cierra la sesión y sale de la aplicación.

## Multi-idioma (patrón Observer)
| Pieza | Dónde | Rol |
|---|---|---|
| `IObservadorIdioma_60MN`, `IIdioma_60MN` | Interfaces_60MN | Contratos del Observer |
| `GestorIdioma_60MN` | Servicios_60MN/Idioma | **Sujeto** (Singleton): idioma actual + diccionario; `Suscribir`, `Desuscribir`, `Notificar` |
| `FormBase_60MN` | VetCare/Utilidades | **Observador concreto**: todos los formularios heredan de él; se suscribe al cargarse y se traduce |
| `TraductorFormularios_60MN` | VetCare/Utilidades | Recorre controles, columnas y menús y aplica la traducción |
| `IdiomaBLL_60MN` / `IdiomaDAL_60MN` | BLL / DAL | Leen `Idioma` y `Traduccion` y guardan el predeterminado |
| Tablas `Idioma`, `Traduccion` | BD | Traducciones por clave (`Formulario.Control` o `Control`) |

- El español es el idioma **base**: sus textos son los del diseñador, por eso solo el inglés tiene filas en `Traduccion`.
- Para agregar un idioma: una fila en `Idioma` y sus filas en `Traduccion` (sin tocar el código).
- El idioma se elige en el Login ("Cambio de idioma") o en USUARIO > Cambiar idioma; queda como `Predeterminado`
  y es el que se aplica al abrir el sistema la próxima vez (`Program.cs` → `AplicarPredeterminado`).
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
