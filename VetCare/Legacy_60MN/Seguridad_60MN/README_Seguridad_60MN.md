# Capa Seguridad_60MN — guía de integración

Este paquete arma tu "capa de seguridad" como proyecto propio dentro de la solución VetCare, inspirado en el ejemplo que compartiste (Security.csproj con Audit/Integrity/Session/Cryptography), pero con tu nomenclatura `Clase_60MN` y adaptado a las tablas reales de VetCare que vimos en la revisión de código.

Decisiones tomadas (confirmadas con vos):

- Proyecto nuevo `Seguridad_60MN`, separado de `BLL_60MN` — no una carpeta más adentro de BLL.
- Dígitos verificadores (DVH/DVV): migrados a **HMAC-SHA256**, en vez de la suma simple de bytes que había antes.
- Contraseñas: **ahora son irreversibles**. Se reemplazó el 3DES-ECB reversible de `EncriptacionBLL_60MN` por `PasswordHasher_60MN` (SHA-256 + sal aleatoria por usuario, formato `sal.hash` en Base64). Ya no existe forma de recuperar la contraseña de un usuario, solo de verificar un intento de login contra el hash guardado. Las claves ya guardadas con el esquema anterior no se pueden convertir (no hay forma de derivar un hash sin la contraseña en texto plano): hay que resetearlas una vez con `UsuarioBLL_60MN.ResetearTodasLasClaves()` (ver `migracion_seguridad_60mn.sql`). `CriptografiaHandler_60MN` (AES-CBC + PBKDF2) sigue existiendo para datos que sí necesitan ser reversibles, como el texto de la bitácora.

## Estructura de archivos entregados

```
Seguridad_60MN/
  Seguridad_60MN.csproj
  Configuracion/ConfiguracionSeguridad_60MN.cs
  Criptografia/CriptografiaHandler_60MN.cs
  Integridad/DigitoVerificadorCalculadora_60MN.cs
  Integridad/DigitoVerificadorDAL_60MN.cs
  Integridad/DigitoVerificadorBLL_60MN.cs
  Auditoria/CatalogoAuditoria_60MN.cs
  Auditoria/AuditoriaDAL_60MN.cs
  Auditoria/AuditoriaBLL_60MN.cs
migracion_seguridad_60mn.sql
README_Seguridad_60MN.md   (este archivo)
```

## Paso 1 — Agregar el proyecto a la solución

En Visual Studio: clic derecho en la solución → **Agregar → Proyecto existente...** → seleccionar `Seguridad_60MN.csproj`. Después, desde `BLL_60MN` (y desde `VetCare` si vas a llamar algo directo desde la UI), agregar una referencia de proyecto a `Seguridad_60MN`.

Si tus otros proyectos (`DAL_60MN`, `BLL_60MN`) NO son de estilo SDK (`.csproj` viejo con `<Project ToolsVersion=...>` en vez de `<Project Sdk="Microsoft.NET.Sdk">`), avisame y te adapto el `.csproj` al formato clásico — lo armé como SDK-style asumiendo que todo el resto de la solución también lo es (igual que `VetCare.csproj`, que vimos apunta a `net8.0-windows`).

## Paso 2 — Verificar nombres de columnas

Reconstruí las consultas de `DigitoVerificadorDAL_60MN` y `AuditoriaDAL_60MN` leyendo el código que ya habíamos revisado (`DigitosVerificadores_60MN.cs`, `BitacoraDAL_60MN.cs`), no un script de creación real. Antes de compilar, confirmá que existen tal cual en tu base:

- `Usuario(UsuarioId, Usuario, Clave, DVH)`
- `UsuarioOperacion(UsuarioId, OperacionID, Habilitado, DVH)`
- `Bitacora(BitacoraID, UsuarioID, FechayHora, DVH)`
- `PerfilUsuario(PerfilUsuarioID, NombrePerfil, DescPerfil, DVH)`
- `Operacion(OperacionID, Descripcion, PatenteEscencial, DVH)`

Si algún nombre difiere, es un cambio de una línea en el `.cs` correspondiente (el SQL está todo en un solo lugar por tabla).

## Paso 3 — Configurar las dos claves nuevas

Ninguna clave va en el código. Se configuran en el `App.config` del proyecto de inicio (`VetCare`), dentro de `<appSettings>`, junto a `conexionBD`:

```xml
<appSettings>
  <add key="conexionBD" value="...la que ya tenés..." />
  <add key="VETCARE_CLAVE_CRIPTO" value="PEGAR_AQUI_UNA_FRASE_LARGA_Y_UNICA" />
  <add key="VETCARE_CLAVE_DV" value="PEGAR_AQUI_LO_QUE_GENERE_EL_PASO_SIGUIENTE" />
</appSettings>
```

Para generar `VETCARE_CLAVE_DV` (tiene que ser Base64 de 32 bytes exactos), corré una sola vez, en un programa de prueba o en el Immediate Window de Visual Studio:

```csharp
Console.WriteLine(Seguridad_60MN.Configuracion.ConfiguracionSeguridad_60MN.GenerarClaveAleatoria());
```

Copiá el resultado a `VETCARE_CLAVE_DV`. Para `VETCARE_CLAVE_CRIPTO` alcanza con una frase larga inventada (no hace falta que sea Base64). **Importante:** una vez que hay datos reales cifrados/firmados con estas claves, no las cambies — si cambiás `VETCARE_CLAVE_DV` todos los DVH/DVV van a dar "incorrectos", y si cambiás `VETCARE_CLAVE_CRIPTO` las contraseñas ya guardadas no se van a poder desencriptar más.

## Paso 4 — Correr la migración de base

Ejecutar `migracion_seguridad_60mn.sql` contra tu base (con backup previo). Cambia el tipo de columna de `DVH`/`DVV` de `INT` a `CHAR(64)` en las 5 tablas protegidas, y crea/ajusta la tabla `DVV`.

## Paso 5 — Recalcular los dígitos por primera vez

Después de migrar, todos los `DVH`/`DVV` quedan en `NULL`. Hay que recalcularlos una vez con la clave nueva:

```csharp
new Seguridad_60MN.Integridad.DigitoVerificadorBLL_60MN().RecalcularTodo();
```

Podés llamarlo desde un botón de prueba, desde `Program.cs` una sola vez, o desde el mismo lugar donde hoy tenés el botón de "Recalcular dígitos verificadores".

## Paso 6 — Reemplazar los usos actuales

Ejemplo concreto en `Login.cs`. Antes:

```csharp
var1 = crypt.Encriptar(txtClave.Text);
...
log.NombreOperacion = crypt.Encriptar("Login");
log.Descripcion = crypt.Encriptar("Login Exitoso: " + txtUsuario.Text + " ");
log.Criticidad = 5;
log.Usuarioid = USU1.UsuarioID;
string rta = log.IngresarDatoBitacora(log.NombreOperacion, log.Descripcion, log.Criticidad, log.Usuarioid);
```

Después:

```csharp
var1 = Seguridad_60MN.Criptografia.CriptografiaHandler_60MN.Encriptar(txtClave.Text);
...
var auditoria = new Seguridad_60MN.Auditoria.AuditoriaBLL_60MN();
auditoria.Registrar(Seguridad_60MN.Auditoria.TipoEventoBitacora_60MN.Login,
                     USU1.UsuarioID, "Login exitoso: " + txtUsuario.Text);
```

Fijate que ya no hace falta decidir a mano la `Criticidad` (1, 2, 4, 5): la define `CatalogoAuditoria_60MN` una sola vez para todo el proyecto según el tipo de evento.

Para consultar la bitácora (`ConsultarBitacora.cs`), en vez de armar el SQL con los filtros pegoteados:

```csharp
DataTable dt = new AuditoriaBLL_60MN().Consultar(
    fechaDesde, fechaHasta, criticidadSeleccionada, usuarioSeleccionado,
    tienePermiso: () => usu.verificarPatentesEscenciales(mp.Usuarioid) == "True");
```

Repetí este reemplazo en cada lugar donde hoy se instancia `BLL_60MN.Seguridad_MN60.EncriptacionBLL_60MN` o `BLL_60MN.Seguridad_MN60.BitacoraBLL_60MN` directamente (los vimos en `Login.cs`, `LogOut.cs`, `ModificarUsuario.cs`, `ModificarUsuarios.cs`, `AltaFamilia.cs`, `AsignacionDePatentes.cs`, `ABMUsuarios.cs`, `DigitosVerificadores.cs`, entre otros). No hace falta borrar las clases viejas de una — podés migrar formulario por formulario y dejar ambas convivan mientras terminás, siempre que no mezcles un DVH viejo (INT) con uno nuevo (texto) en la misma tabla al mismo tiempo.

## Qué es igual, qué queda pendiente

La bitácora sigue siendo la misma tabla `Bitacora` y su texto sigue siendo reversible. El flujo de login no cambia para el usuario final, aunque por dentro ya no compara la contraseña con un `WHERE` en SQL: trae el usuario por nombre y verifica el hash en código (necesario porque la sal es distinta por usuario). La contraseña ya **no** se puede desencriptar.

Pendiente si en algún momento querés seguir mejorando (no bloquea esta entrega): aplicar el mismo patrón de `SqlParameter` que usa `AuditoriaDAL_60MN`/`DigitoVerificadorDAL_60MN` al resto de `DAL_60MN` (`UsuarioDAL_60MN`, `ManejadorPerfilUsuarioDAL_60MN`, etc.), que señalamos en la primera revisión que arman el SQL concatenando texto.
