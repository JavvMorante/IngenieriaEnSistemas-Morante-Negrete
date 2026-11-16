# Patrones de diseño y trigger implementados en VetCare (13/10/2026)

Relevado del código real en `C:\Users\jmora\Desktop\RepoFacultad\IngenieriaEnSistemas-Morante-Negrete\VetCare`
(el repo sincronizado del proyecto está desactualizado: refleja la versión de julio, previa al rediseño).

## Patrones presentes

| Patrón | Clases | Archivo |
|---|---|---|
| Singleton | `SessionManager_60MN`, `GestorIdioma_60MN` | `Servicios_60MN/Sesion/`, `Servicios_60MN/Idioma/` |
| Composite | `Componente_60MN` (componente), `Familia_60MN` (compuesto), `Patente_60MN` (hoja), `IPermiso_60MN` | `Servicios_60MN/Composite/`, `Interfaces_60MN/` |
| Observer | `GestorIdioma_60MN` (sujeto), `FormBase_60MN` (observador), `IObservadorIdioma_60MN`, `TraductorFormularios_60MN` | `Servicios_60MN/Idioma/`, `VetCare/Utilidades/` |
| Template Method (hook) | `FormBase_60MN.AlCambiarIdioma()` | `VetCare/Utilidades/FormBase_60MN.cs` |
| Data Mapper + capas | `Conexion_60MN`, `*DAL_60MN`, `ICrud_60MN<T>`, `IEntity_60MN` | `DAL_60MN/`, `Interfaces_60MN/` |

Los tres primeros están declarados explícitamente en los comentarios XML del código, lo que facilita la defensa.

## Trigger (T06B / CUS10)

`TR_Productos_BitacoraCambios`, en `BDScript/VetCareBD_60MN.sql` (líneas 333-397) y en
`Herramientas_60MN/GeneradorScriptBD_60MN/Estructura.sql`.
`ON Productos AFTER INSERT, UPDATE, DELETE` → escribe en `Productos_C`, con `Act = 1` en la versión vigente.

Puntos destacables para la defensa:

- Usa `UPDATE(columna)` para **ignorar los recálculos de DVH**, que si no generarían versiones falsas en el historial.
- Clasifica la operación en ALTA / MODIFICACION / MOVIMIENTO / BAJA / REACTIVACION / ELIMINACION mediante un `CASE`
  y un `LEFT JOIN deleted` (si no hay fila previa, es un alta).
- Un cambio exclusivo de `StockActual` se registra como MOVIMIENTO (CUN14), no como MODIFICACION.
- Está escrito en operaciones de conjunto, no con variables escalares: el trigger se dispara **una vez por sentencia**.
- **Usuario que hizo el cambio**: la DAL escribe `Productos.UsuarioModificacion` (sale de `SessionManager_60MN`)
  y el trigger lo lee de `inserted`; `SESSION_CONTEXT(N'UsuarioID')` queda como plan B en un `COALESCE`.
  Consultado por `BitacoraCambiosDAL_60MN` (solo lectura).
- Hoy está **deshabilitado** (`DISABLE TRIGGER`, línea 396) junto con el módulo de stock.

## Observaciones

- `Legacy_60MN/` contiene el código anterior al rediseño (SQL concatenado, 3DES, Composite incompleto con
  `NotImplementedException`, `Session_60MN.IsInRole` comentado). Está excluido de la compilación. Conviene mencionarlo
  en la defensa antes de que el profesor lo encuentre.
- Todos los problemas de la revisión de código de agosto (inyección SQL, 3DES/MD5, shadowing en `Desconectar()`,
  ruta hardcodeada con clave en texto plano, formularios duplicados) fueron resueltos por el rediseño: la versión
  actual usa SQL parametrizado en toda la DAL, SHA-256 con sal para contraseñas, AES-256 para datos sensibles,
  HMAC-SHA256 para dígitos verificadores y `using` en cada conexión.
- Matiz a anticipar: el Template Method está implementado como un único hook opcional (`AlCambiarIdioma`), no con
  varios pasos abstractos obligatorios. Decirlo primero suma.

## Dónde está explicado en detalle

El documento **"VetCare - Resumen del proyecto.docx"**, secciones 16 a 22, explica cada patrón con
diagramas, referencias de archivo y línea, guion para explicarlo y preguntas probables.
Los diagramas UML están en **"VetCare-Patrones-Diagramas.docx"** y sus fuentes en `Diagramas-Mermaid/`.
