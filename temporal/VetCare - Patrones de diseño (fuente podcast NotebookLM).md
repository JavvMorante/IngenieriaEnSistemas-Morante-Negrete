# VetCare: patrones de diseño implementados y en qué clases están

Este documento explica, para estudiar y defender el Trabajo de Diploma, qué patrones de diseño usa el sistema VetCare, qué problema resuelve cada uno, en qué clases concretas está implementado y cómo se recorre el código cuando el patrón entra en acción. Está pensado para escucharse como explicación, de lo general a lo particular.

## Contexto: qué es VetCare

VetCare es un sistema de escritorio para una veterinaria, hecho en C# con Windows Forms sobre .NET Framework 4.8 y una base de datos SQL Server llamada VetCareBD_60MN. Tiene dos procesos de negocio: Gestión de Pacientes (clientes, mascotas, historias clínicas, atenciones, carrito de medicación) y Gestión de Stock (productos, movimientos, proveedores, reportes). Además tiene un módulo de Seguridad completo: usuarios, perfiles con permisos, login, bitácora de eventos, bitácora de cambios, multi-idioma, backup y dígitos verificadores.

Todas las clases del proyecto terminan con el sufijo _60MN, que identifica al alumno. Cuando en este documento se nombra, por ejemplo, SessionManager_60MN, es una clase real del código.

Los patrones implementados son seis:

1. Arquitectura en capas (patrón de arquitectura).
2. Singleton.
3. Composite.
4. Observer.
5. Template Method.
6. Data Mapper.

Además hay un mecanismo de historial de versiones en la base de datos hecho con un trigger, que acompaña a la Bitácora de cambios.

## 1. Arquitectura en capas

### Qué problema resuelve
Separar responsabilidades: la pantalla no debe saber SQL, y las reglas de negocio no deben depender de cómo se dibuja un formulario. Así cada parte se puede cambiar o probar sin romper las demás.

### Cómo está en VetCare
La solución VetCare.sln tiene un proyecto por capa:

- VetCare: la interfaz gráfica. Son los formularios, todos con nombre Frm..._60MN, por ejemplo FrmClientes_60MN o FrmProductos_60MN.
- BLL_60MN: la lógica de negocio. Clases como ClienteBLL_60MN, ProductoBLL_60MN o BitacoraBLL_60MN. Acá se validan los datos, se controla que el usuario tenga permiso para cada operación y se registran los eventos en la bitácora.
- DAL_60MN: el acceso a datos. Clases como ClienteDAL_60MN o ProductoDAL_60MN, que son las únicas que escriben SQL.
- Entidades_60MN: los objetos del dominio, como Cliente_60MN, Mascota_60MN, Producto_60MN o Atencion_60MN. Son clases simples con propiedades.
- Interfaces_60MN: los contratos compartidos, como IEntity_60MN, IPermiso_60MN, IObservadorIdioma_60MN o IUsuario_60MN.
- Servicios_60MN: servicios transversales que usan varias capas. Acá viven el Composite de permisos, el Singleton de sesión y el Singleton de idioma que además es el sujeto del Observer.
- Seguridad_60MN: criptografía (hash de contraseñas, cifrado AES), dígitos verificadores, bitácora y serialización XML de la bitácora.

### Cómo se recorre
El camino siempre va en una sola dirección: el formulario llama a la BLL, la BLL llama a la DAL, y la DAL habla con SQL Server. Por ejemplo, al guardar un producto, FrmProducto_60MN arma un Producto_60MN con lo que escribió el usuario y llama a ProductoBLL_60MN. La BLL verifica el permiso, valida nombre, lote, precios y fecha de vencimiento, y recién entonces llama a ProductoDAL_60MN, que ejecuta el INSERT o el UPDATE. El formulario nunca toca la base.

Un detalle importante para la defensa: los permisos se controlan dos veces. La pantalla oculta lo que el usuario no puede usar, pero además cada método de la BLL vuelve a exigir la patente con SesionActual_60MN.RequierePermiso. Así, aunque alguien llegara a la operación por otro camino, la BLL la rechaza.

## 2. Singleton

### Qué problema resuelve
Garantizar que de una clase exista una sola instancia en todo el programa y que cualquier parte del sistema pueda llegar a ella. Sirve para información que tiene que ser única y compartida, como "quién está logueado ahora" o "qué idioma está activo".

### En qué clases está
Hay dos Singleton, los dos en el proyecto Servicios_60MN:

- SessionManager_60MN, en Servicios_60MN/Sesion. Guarda la sesión del usuario autenticado: el usuario, la hora de inicio, si entró con el usuario de emergencia y si se detectó un problema de integridad. Tiene los métodos Login, Logout y TienePermiso.
- GestorIdioma_60MN, en Servicios_60MN/Idioma. Guarda el idioma actual y el diccionario de traducciones. Como se verá más adelante, también es el sujeto del patrón Observer.

### Cómo está construido
Las dos clases siguen la misma receta, que es la forma clásica del patrón:

- La clase es sealed, es decir, no se puede heredar. Así nadie puede crear una subclase que rompa la unicidad.
- El constructor es privado. Desde afuera no se puede hacer new SessionManager_60MN().
- Hay un campo estático privado llamado instancia, que guarda el único objeto.
- Hay una propiedad estática pública llamada Instancia. La primera vez que alguien la pide, crea el objeto; las siguientes veces devuelve siempre el mismo.
- La creación está protegida con un lock sobre un objeto llamado candado. Esto hace que el Singleton sea seguro aunque dos hilos lo pidan al mismo tiempo: nunca se crean dos instancias.

### Quién lo usa
La BLL no llama directamente al SessionManager en todos lados. Hay una clase estática de ayuda, SesionActual_60MN, en BLL_60MN, que ofrece atajos como UsuarioId, NombreUsuario y RequierePermiso, y por dentro usa SessionManager_60MN.Instancia. El login, en FrmLogin_60MN y UsuarioBLL_60MN, es el que llama a Login. Del lado del idioma, FormBase_60MN y TraductorFormularios_60MN usan GestorIdioma_60MN.Instancia para traducir.

## 3. Composite

### Qué problema resuelve
Permite tratar de la misma manera a un objeto simple y a un grupo de objetos. En VetCare se usa para los permisos: un permiso puede ser una patente suelta (por ejemplo, "Registrar producto") o una familia, que es un rol que agrupa patentes y también otras familias. El Composite deja preguntar "¿este usuario tiene tal permiso?" sin importar si el permiso le llegó directo o a través de tres niveles de familias.

### En qué clases está
Está en Servicios_60MN/Composite, con su interfaz en Interfaces_60MN:

- IPermiso_60MN (Interfaces_60MN): es el componente del patrón. Define lo que tienen en común patentes y familias: Nombre, EsFamilia, AgregarHijo, QuitarHijo, ObtenerHijos y TienePatente.
- Componente_60MN: clase abstracta que implementa IPermiso_60MN. Agrega operaciones comunes, como ObtenerPatentes, que recorre todo el árbol y devuelve todas las patentes, y ContieneFamilia, que sirve para detectar ciclos.
- Patente_60MN: es la hoja. Representa un permiso atómico y tiene un Codigo, como SEG_BITACORA_EXPORT o STK_PROD_ALTA. Su TienePatente simplemente compara el código. Si alguien intenta agregarle un hijo, lanza una excepción, porque una hoja no puede contener nada.
- Familia_60MN: es el compuesto. Tiene una lista de hijos que pueden ser patentes o familias. Su TienePatente pregunta a cada hijo, y esa pregunta baja recursivamente por todo el árbol.
- CodigosPatente_60MN: no es parte del patrón en sí, pero centraliza como constantes todos los códigos de patente, para no escribirlos a mano en cada lugar.
- CicloFamiliasException_60MN: la excepción que se lanza si una familia intentara contenerse a sí misma, directa o indirectamente.

### Protección contra ciclos
Un Composite tiene que ser un árbol, sin ciclos: si la familia A contiene a B y B contiene a A, preguntar por un permiso no terminaría nunca. VetCare lo evita en dos lugares. En el código, Familia_60MN.AgregarHijo revisa con ContieneFamilia si el nuevo hijo ya contiene a la familia, y si es así lanza CicloFamiliasException_60MN. En la base, el trigger TR_FamiliaFamilia_SinCiclos rechaza cualquier inserción en la tabla FamiliaFamilia que forme un ciclo. Así la regla se cumple aunque alguien modifique la base a mano.

### Cómo se recorre
Las tablas son Patente, Familia, FamiliaPatente, FamiliaFamilia, UsuarioFamilia y UsuarioPatente. PermisoDAL_60MN lee esas tablas y reconstruye el árbol en memoria: crea los objetos Patente_60MN, crea los Familia_60MN y les agrega sus hijos. Cuando el usuario inicia sesión, UsuarioBLL_60MN carga sus familias y sus patentes individuales y se las asigna al usuario. Desde ese momento, SessionManager_60MN.TienePermiso recorre la lista de permisos del usuario y llama a TienePatente en cada uno: el Composite resuelve solo si la patente está en algún nivel.

En la pantalla, cada botón y cada opción de menú tiene en su propiedad Tag el código de la patente que lo habilita. El método Mensajes_60MN.AplicarPermisos recorre los controles del formulario y muestra u oculta cada uno según el usuario tenga o no esa patente. Por ejemplo, el botón Exportar XML de la Bitácora de eventos tiene el Tag SEG_BITACORA_EXPORT. Las familias se administran desde ADMIN, Perfiles, en el formulario FrmFamiliasPatentes_60MN, que además muestra una vista previa del árbol resultante.

## 4. Observer

### Qué problema resuelve
Permite que varios objetos se enteren automáticamente cuando otro cambia, sin que el que cambia tenga que conocerlos uno por uno. En VetCare se usa para el multi-idioma: cuando el usuario cambia de español a inglés, todas las pantallas abiertas se traducen en ese mismo momento, sin cerrarlas.

### En qué clases está
- Sujeto: GestorIdioma_60MN, en Servicios_60MN/Idioma. Es el mismo Singleton de idioma. Mantiene una lista de observadores y tiene los métodos Suscribir, Desuscribir, CambiarIdioma y Notificar.
- Interfaz del observador: IObservadorIdioma_60MN, en Interfaces_60MN. Tiene un solo método, ActualizarIdioma.
- Observador concreto: FormBase_60MN, en VetCare/Utilidades. Es el formulario base del que heredan todos los formularios del sistema, así que cada pantalla es automáticamente un observador.
- Piezas de apoyo: IdiomaBLL_60MN carga desde la base las traducciones del idioma elegido, usando IdiomaDAL_60MN, y se las entrega al gestor. TraductorFormularios_60MN es la clase que efectivamente recorre los controles de un formulario y les pone el texto traducido. FrmCambiarIdioma_60MN es la pantalla donde el usuario elige el idioma.

### Cómo se recorre
Cuando se abre cualquier formulario, en su evento de carga, FormBase_60MN se suscribe al GestorIdioma_60MN y se traduce. Cuando el usuario elige otro idioma en FrmCambiarIdioma_60MN, IdiomaBLL_60MN busca las traducciones en las tablas Idioma y Traduccion y llama a GestorIdioma_60MN.CambiarIdioma. El gestor guarda el nuevo diccionario y llama a Notificar, que recorre la lista de observadores y llama a ActualizarIdioma en cada formulario abierto. Cada formulario se vuelve a traducir. Al cerrarse, el formulario se desuscribe, para que el gestor no siga avisándole a una pantalla que ya no existe.

Dos detalles finos: Notificar recorre una copia de la lista, porque un formulario podría desuscribirse justo mientras se lo notifica. Y el gestor no accede a la base: solo recibe las traducciones. Eso respeta las capas, porque Servicios_60MN no depende de la DAL.

El idioma elegido queda guardado por usuario: la próxima vez que esa persona inicia sesión, el sistema le aplica su último idioma. La pantalla de login, en cambio, siempre arranca en español.

## 5. Template Method

### Qué problema resuelve
Define el esqueleto de un algoritmo en una clase base y deja que las subclases completen solo algunos pasos, sin cambiar el orden general.

### En qué clases está
En FormBase_60MN, en VetCare/Utilidades, junto con todos los formularios que heredan de ella.

- El método plantilla es ActualizarIdioma, que es el que llama el Observer. Su algoritmo es siempre el mismo: primero, si el formulario ya fue destruido, no hace nada; segundo, traduce todos los controles que vienen del diseñador con TraductorFormularios_60MN; tercero, llama a AlCambiarIdioma. El mismo esqueleto se aplica al cargar el formulario, en OnLoad.
- El paso variable, o "gancho", es AlCambiarIdioma: un método protegido y virtual que en FormBase_60MN está vacío.

### Cómo se recorre
Cada formulario decide si necesita completar el gancho. Los textos que vienen del diseñador se traducen solos, pero hay textos que arma el código en tiempo de ejecución, como el contador "104 evento(s)" o la línea "Usuario del evento" en FrmBitacoraEventos_60MN, o los datos del archivo en FrmDeserializarBitacora_60MN. Esos formularios sobrescriben AlCambiarIdioma para volver a armar esos textos en el idioma nuevo. Veinticinco formularios lo sobrescriben, entre ellos FrmMenuPrincipal_60MN, FrmClientes_60MN, FrmCliente_60MN y FrmProducto_60MN; otros, como FrmLogin_60MN o FrmIntegridad_60MN, no necesitan hacerlo. Ninguno puede alterar el orden del algoritmo: ese orden lo fija la clase base.

Por eso Observer y Template Method trabajan juntos: el Observer decide cuándo traducir y el Template Method decide cómo se traduce cada pantalla.

## 6. Data Mapper

### Qué problema resuelve
Separa los objetos del dominio de la base de datos. Las entidades no saben nada de SQL, y una clase aparte, el mapper, se encarga de convertir filas de la base en objetos y objetos en sentencias SQL.

### En qué clases está
En el proyecto DAL_60MN, una clase mapper por área: ClienteDAL_60MN (tablas Cliente y Mascota), ProductoDAL_60MN, AtencionDAL_60MN, HistoriaClinicaDAL_60MN, MovimientoStockDAL_60MN, ProveedorDAL_60MN, VentaDAL_60MN, UsuarioDAL_60MN, PermisoDAL_60MN e IdiomaDAL_60MN, entre otras. Las entidades están en Entidades_60MN e implementan la interfaz IEntity_60MN, que solo exige un Id.

### Cómo está construido
- Cada mapper tiene un método privado Mapear que recibe una fila leída de la base, un SqlDataReader, y devuelve la entidad armada. Por ejemplo, ProductoDAL_60MN.Mapear construye un Producto_60MN columna por columna.
- Toda consulta pasa por Conexion_60MN, una clase estática interna de la DAL. Tiene métodos como Abrir, EjecutarNonQuery, EjecutarEscalar y EjecutarLista. EjecutarLista recibe el SQL y la función de mapeo, así cada mapper solo escribe su consulta y su Mapear.
- Todos los valores viajan como parámetros SQL, nunca concatenados en el texto. Eso evita la inyección SQL.
- Cuando una operación toca varias tablas, como registrar una atención que también actualiza el peso en la historia clínica, el mapper la hace dentro de una transacción: o se guarda todo, o no se guarda nada.

### Cómo se recorre
Por ejemplo, al buscar productos, FrmProductos_60MN llama a ProductoBLL_60MN, que llama a ProductoDAL_60MN.Buscar. El mapper arma el SELECT con parámetros, lo ejecuta con Conexion_60MN.EjecutarLista y por cada fila usa Mapear para devolver una lista de Producto_60MN. La BLL y la pantalla trabajan con esa lista de objetos y nunca ven una fila de la base.

## Complemento: historial de versiones con trigger (Bitácora de cambios)

No es un patrón de diseño clásico de objetos, pero es una decisión de diseño importante y suele preguntarse. La tabla Productos tiene un trigger, TR_Productos_BitacoraCambios, que se dispara automáticamente después de cada INSERT, UPDATE o DELETE.

El trigger primero descarta lo que no es un cambio de negocio, como un UPDATE que solo recalcula el dígito verificador. Después marca como no actual la versión anterior del producto en la tabla Productos_C y guarda la versión nueva, clasificada como alta, baja, reactivación, movimiento de stock o modificación, con la fecha, la hora y el usuario. Como corre dentro de la misma transacción que el cambio, el historial nunca queda desfasado.

Con ese historial, la pantalla FrmBitacoraCambios_60MN muestra las versiones de cada producto, FrmCompararVersiones_60MN compara dos versiones y el sistema permite restaurar una versión anterior.

## Cómo se combinan los patrones en una sola operación

Un buen ejemplo para cerrar es lo que pasa cuando un usuario abre la Bitácora de eventos y exporta a XML:

1. Por la arquitectura en capas, FrmBitacoraEventos_60MN no accede a la base: llama a BitacoraBLL_60MN.
2. Al abrirse, la pantalla se suscribió al GestorIdioma_60MN (Observer) y se tradujo siguiendo el algoritmo de FormBase_60MN (Template Method). El contador "N evento(s)" se rearma en su AlCambiarIdioma.
3. El botón Exportar XML solo se ve porque Mensajes_60MN.AplicarPermisos le preguntó al usuario de la sesión, guardado en el Singleton SessionManager_60MN, si tiene la patente SEG_BITACORA_EXPORT; el Composite de familias y patentes resolvió la respuesta.
4. Al exportar, la BLL vuelve a exigir esa patente con SesionActual_60MN.RequierePermiso, y los datos se leen con los mappers de la DAL (Data Mapper).

Cada patrón resuelve una parte distinta del problema: las capas ordenan el sistema, el Singleton guarda el estado único, el Composite modela los permisos, el Observer y el Template Method resuelven el multi-idioma, y el Data Mapper aísla la base de datos.

## Resumen rápido: patrón, clases y ubicación

- Arquitectura en capas: proyectos VetCare (GUI), BLL_60MN, DAL_60MN, Entidades_60MN, Interfaces_60MN, Servicios_60MN y Seguridad_60MN.
- Singleton: SessionManager_60MN (Servicios_60MN/Sesion) y GestorIdioma_60MN (Servicios_60MN/Idioma). Atajo de uso: SesionActual_60MN (BLL_60MN).
- Composite: componente IPermiso_60MN (Interfaces_60MN) y Componente_60MN; hoja Patente_60MN; compuesto Familia_60MN (Servicios_60MN/Composite). Lo arma PermisoDAL_60MN y lo usan SessionManager_60MN.TienePermiso y Mensajes_60MN.AplicarPermisos. Ciclos: CicloFamiliasException_60MN y trigger TR_FamiliaFamilia_SinCiclos.
- Observer: sujeto GestorIdioma_60MN; observador IObservadorIdioma_60MN; observador concreto FormBase_60MN y, por herencia, todos los formularios. Apoyo: IdiomaBLL_60MN, IdiomaDAL_60MN, TraductorFormularios_60MN, FrmCambiarIdioma_60MN.
- Template Method: FormBase_60MN.ActualizarIdioma y OnLoad (método plantilla) y AlCambiarIdioma (gancho que sobrescriben los formularios).
- Data Mapper: clases ...DAL_60MN con su método Mapear, sobre Conexion_60MN; entidades de Entidades_60MN con IEntity_60MN.
- Historial de versiones: trigger TR_Productos_BitacoraCambios y tabla Productos_C, usados por FrmBitacoraCambios_60MN y FrmCompararVersiones_60MN.
