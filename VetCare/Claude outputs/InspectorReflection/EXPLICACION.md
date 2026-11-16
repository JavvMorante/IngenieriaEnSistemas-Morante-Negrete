# Inspector de miembros: Reflection + Chain of Responsibility

## Reflection

Reflection permite que un programa consulte, en tiempo de ejecución, la estructura de sus propios tipos a partir de los metadatos del ensamblado.

En `InspectorClase.Inspeccionar(Type tipo)` se llama a `tipo.GetMembers(...)`, que devuelve un arreglo de `MemberInfo` con todos los miembros de la clase. Los `BindingFlags` indican qué buscar: miembros de instancia, públicos y privados, declarados en la propia clase (sin los heredados de `Object`).

`MemberInfo` es la clase base; cada miembro llega en realidad como un objeto más específico:

| Miembro   | Tipo en Reflection | Datos que se leen                         |
|-----------|--------------------|-------------------------------------------|
| Atributo  | `FieldInfo`        | `Name`, `FieldType`                       |
| Propiedad | `PropertyInfo`     | `Name`, `PropertyType`                    |
| Método    | `MethodInfo`       | `Name`, `ReturnType`, `GetParameters()`   |

En ningún lugar del código se escriben los nombres `_precio`, `Nombre` o `CalcularPrecioFinal`, ni sus tipos: todo se obtiene de los metadatos. El único dato fijo es `typeof(Producto)`; si se agregan miembros a la clase, aparecen en la grilla sin tocar nada más.

## Chain of Responsibility

El patrón desacopla a quien envía una solicitud de quien la resuelve: la solicitud recorre una cadena de objetos hasta que alguno se hace cargo.

- **`ManejadorMiembro`** (clase base abstracta): guarda la referencia al siguiente eslabón y define `Procesar(MemberInfo)`. Si `PuedeProcesar` devuelve verdadero, llama a `ObtenerInformacion`; si no, delega en el siguiente. Si no hay siguiente, devuelve `null`.
- **`ManejadorAtributo`**, **`ManejadorPropiedad`**, **`ManejadorMetodo`** (eslabones concretos): cada uno sabe reconocer y describir un único tipo de miembro.
- **`InspectorClase`** (cliente): arma la cadena `Atributo -> Propiedad -> Método` y le entrega cada miembro al primer eslabón, sin saber cuál lo va a resolver.

Recorrido para `CalcularPrecioFinal`: `ManejadorAtributo` no lo reconoce y delega; `ManejadorPropiedad` tampoco y delega; `ManejadorMetodo` lo reconoce y devuelve nombre, tipo de retorno y cantidad de parámetros.

Los miembros que nadie procesa llegan al final de la cadena y se descartan: el constructor (`ConstructorInfo`) y los que genera el compilador para la propiedad automática (el campo de respaldo y los métodos `get_Nombre` / `set_Nombre`).

Para soportar un nuevo tipo de miembro (por ejemplo, eventos) alcanza con crear otro eslabón y agregarlo a la cadena; los existentes y el formulario no cambian.

## Separación de capas

- `Negocio/` — la clase `Producto`.
- `Inspeccion/` — Reflection y la cadena; devuelve una lista de `InfoMiembro`.
- `FormPrincipal` — solo pide la lista al inspector y la enlaza al `DataGridView`. No contiene condiciones sobre el tipo de miembro.

## Resultado esperado

| Nombre              | Categoría | Tipo de dato / retorno | Cantidad de parámetros |
|---------------------|-----------|------------------------|------------------------|
| _precio             | Atributo  | Decimal                |                        |
| Nombre              | Propiedad | String                 |                        |
| CalcularPrecioFinal | Método    | Decimal                | 1                      |

El orden de las filas es el que devuelve `GetMembers` y puede variar.
