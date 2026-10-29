# VetCare – Usuarios, contraseñas, perfiles y permisos

Usuarios de prueba que crea el script `BDScript/VetCareBD_60MN.sql` (más el usuario de emergencia del XML).
Modelo de permisos: **Usuario – Familia – Patente**. La *familia* es el perfil o rol (un conjunto de patentes) y la
*patente* es un permiso atómico. Además de su familia, un usuario puede tener patentes individuales.
Numeración de casos de uso: Trabajo de Diploma **V2.4**.

> Las contraseñas son de prueba. Ningún usuario está obligado a cambiar la contraseña en el primer ingreso
> (`PrimerIngreso = 0`). Tres intentos fallidos bloquean al usuario.
> Una contraseña nueva debe tener al menos 8 caracteres, con mayúsculas, minúsculas y números.

## 1. Usuarios del sistema

| Usuario | Contraseña | Nombre | Perfil (familia) | Patentes individuales | Rol en el negocio |
|---|---|---|---|---|---|
| `admin` | `Admin1234` | Mariano Núñez | **Administrador** (incluye todas las demás familias) | — | Gerente / administrador del sistema: acceso total |
| `operario` | `Operario1234` | Sofía Ramírez | **Operario de stock** | `STK_CONFIG` (CUN10 Configurar margen y alerta de stock mínimo) | Encargado de stock |
| `vendedor` | `Vendedor1234` | Lucas González | **Vendedor** | — | Atención al público: clientes, mascotas y venta de medicación |
| `veterinario` | `Veterinario1234` | Carla Domínguez | **Veterinario** | — | Profesional: historias clínicas, atenciones, informes y reportes |
| `emergencia` | `Emergencia1234` | Usuario Emergencia | Todas las patentes (no está en la base) | — | Solo para recuperar el sistema (ver punto 4) |

## 2. Qué ve cada usuario en el menú

Menú horizontal: ADMIN, MAESTROS, PACIENTES, VENTAS, STOCK, REPORTES, USUARIO, AYUDA. Un menú sin opciones permitidas se oculta.

| Usuario | Menú visible |
|---|---|
| `admin` | **ADMIN:** Usuarios, Perfiles, Backup, Restore, Bitácora de eventos (Ver / Exportar / Importar XML), Bitácora de cambios, Dígitos verificadores, Configuración de stock<br>**MAESTROS:** Clientes, Mascotas, Productos, Proveedores<br>**PACIENTES:** Historias clínicas, Registrar atención<br>**VENTAS:** Carrito, Ventas registradas<br>**STOCK:** Movimientos de stock<br>**REPORTES:** Reporte de atenciones, Reporte de existencias, Reporte inteligente<br>**USUARIO** y **AYUDA** |
| `operario` | **ADMIN:** Configuración de stock (por su patente individual `STK_CONFIG`)<br>**MAESTROS:** Productos, Proveedores<br>**STOCK:** Movimientos de stock<br>**REPORTES:** Reporte de existencias<br>**USUARIO** y **AYUDA** |
| `vendedor` | **MAESTROS:** Clientes (registra, modifica y da de baja), Mascotas<br>**VENTAS:** Carrito, Ventas registradas<br>**USUARIO** y **AYUDA** |
| `veterinario` | **MAESTROS:** Clientes (solo consulta; no ve los botones de alta y modificación)<br>**PACIENTES:** Historias clínicas, Registrar atención<br>**VENTAS:** Carrito, Ventas registradas<br>**REPORTES:** Reporte de atenciones, Reporte inteligente<br>**USUARIO** y **AYUDA** |

**USUARIO** (todos): Re-Login, Cambiar clave, Logout, Cambiar idioma. **AYUDA** (todos): Ayuda en línea (F1), Acerca de VetCare.

## 3. Perfiles (familias) y sus patentes

### Vendedor
| Patente | Descripción | Caso de uso |
|---|---|---|
| `PAC_CLIENTE_ALTA` | Registrar cliente | CUN01 Gestionar Clientes |
| `PAC_CLIENTE_MODIF` | Modificar / dar de baja cliente | CUN01 Gestionar Clientes |
| `PAC_CLIENTE_CONSULTA` | Consultar cliente | CUN01 Gestionar Clientes |
| `PAC_MASCOTA` | Gestionar mascotas | CUN02 Gestionar Mascotas |
| `VEN_CARRITO` | Medicación en carrito y ventas registradas | CUN05 Registrar Medicación en Carrito |

### Veterinario
| Patente | Descripción | Caso de uso |
|---|---|---|
| `PAC_CLIENTE_CONSULTA` | Consultar cliente | CUN01 Gestionar Clientes |
| `HC_ALTA` | Crear historia clínica | CUN03 Gestionar Historias Clínicas |
| `HC_CONSULTA` | Consultar historia clínica | CUN03 Gestionar Historias Clínicas |
| `HC_MODIF` | Actualizar historia clínica | CUN03 Gestionar Historias Clínicas |
| `ATN_REGISTRAR` | Registrar atención | CUN04 Registrar Atención |
| `VEN_CARRITO` | Medicación en carrito y ventas registradas | CUN05 Registrar Medicación en Carrito |
| `ATN_INFORME` | Emitir informe de atención | CUN06 Emitir Informe de Atención |
| `REP_REPORTES` | Reporte de atenciones y reporte inteligente | CUN07 y CUN13 |

### Operario de stock
| Patente | Descripción | Caso de uso |
|---|---|---|
| `STK_PROD_ALTA` | Registrar producto | CUN08 Administrar Existencias |
| `STK_PROD_MODIF` | Modificar producto | CUN08 Administrar Existencias |
| `STK_MOVIMIENTO` | Registrar movimiento de stock | CUN09 Registrar Movimiento de Stock |
| `MAE_PROVEEDORES` | Gestionar proveedores | CUN11 Gestionar Proveedores |
| `STK_REPORTE` | Generar reporte de existencias | CUN12 Generar Reporte de Existencias |

### Gestión de Usuarios
| Patente | Descripción | Caso de uso |
|---|---|---|
| `SEG_USU_ALTA` | Crear usuario | CUS01 |
| `SEG_USU_MODIF` | Modificar usuario (y sus permisos) | CUS02 |
| `SEG_USU_BAJA` | Dar de baja / reactivar usuario | CUS03 |
| `SEG_USU_DESBLOQ` | Desbloquear usuario | CUS08 |

### Auditoría
| Patente | Descripción | Caso de uso |
|---|---|---|
| `SEG_BITACORA_EVENTOS` | Consultar bitácora de eventos | CUS09 |
| `SEG_BITACORA_CAMBIOS` | Consultar bitácora de cambios | CUS10 |
| `SEG_INTEGRIDAD` | Verificar integridad (dígitos verificadores) | CUS13 |
| `SEG_BITACORA_EXPORT` | Exportar bitácora de eventos (XML) | CUS09 |

### Administrador
Patentes propias de la familia:

| Patente | Descripción | Caso de uso |
|---|---|---|
| `SEG_ROLES` | Gestionar familias y permisos (define a un "administrador") | CUS04 |
| `SEG_BACKUP` | Respaldar base de datos | CUS12 |
| `SEG_RESTORE` | Restaurar base de datos | CUS12 |
| `SEG_BITACORA_IMPORT` | Importar bitácora de eventos (XML) | CUS09 |
| `STK_CONFIG` | Configurar margen y alerta de stock mínimo | CUN10 |
| `MAE_PROVEEDORES` | Gestionar proveedores | CUN11 |
| `REP_REPORTES` | Reporte de atenciones y reporte inteligente | CUN07 y CUN13 |

Además **incluye** las familias Gestión de Usuarios, Auditoría, Operario de stock, Vendedor y Veterinario
(relación `FamiliaFamilia`, patrón Composite), por eso el administrador tiene todas las patentes.

> El sistema **no factura**: las patentes `VEN_FACTURAR`, `VEN_DESPACHAR` y `COM_COMPRAS` se eliminaron
> (en una base existente las quita `BDScript/Migracion_V24_60MN.sql`).

## 4. Usuario de emergencia

- **Usuario / contraseña:** `emergencia` / `Emergencia1234`.
- **Dónde está:** solo en `VetCare/UsuarioEmergencia_60MN.xml`, no en la base. El archivo guarda la contraseña con hash SHA-256 y está firmado con HMAC: si se edita a mano, queda inválido.
- **Cuándo funciona:** únicamente cuando no existe ningún administrador operativo, es decir, un usuario activo, no bloqueado, con DVH válido y con la patente `SEG_ROLES`. Por ejemplo, si el `admin` fue borrado de la base.
- **Qué permite:** entra con todas las patentes, para crear o reparar un administrador.

## 5. Cómo crear más usuarios con estos perfiles

1. Entrar como `admin` → **ADMIN > Usuarios > Añadir**.
2. Completar los datos y elegir la familia inicial (por ejemplo, *Vendedor* o *Veterinario*).
3. El sistema genera una contraseña inicial, que el usuario debe cambiar en su primer ingreso.
4. Para sumar familias o patentes individuales: **ADMIN > Usuarios > Permisos**.
5. Para cambiar qué incluye cada perfil: **ADMIN > Perfiles**.
