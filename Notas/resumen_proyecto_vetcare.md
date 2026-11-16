# VetCare - Resumen del proyecto.docx — versión ampliada (16/10/2026)

Documento de estudio para poder explicar el trabajo. Pasó de 17 a 26 secciones
y de ~20 a 42 páginas. **No se quitó nada de lo que ya estaba**: las 17 secciones
originales, sus 15 mapas conceptuales y las 2 tablas finales siguen intactas; lo
nuevo se insertó entre la sección 15 y las dos finales.

## Lo que se agregó

| Sección | Contenido |
|---|---|
| 16 | Los patrones de diseño de un vistazo: qué es un patrón, tabla resumen y diagrama de capas |
| 17 | Singleton — SessionManager_60MN y GestorIdioma_60MN |
| 18 | Composite — los permisos como un árbol |
| 19 | Observer — el cambio de idioma en caliente |
| 20 | Template Method — el esqueleto en FormBase_60MN |
| 21 | Data Mapper — cómo se habla con la base |
| 22 | El trigger de la bitácora de cambios |
| 23 | Serialización XML de la bitácora (A03 – CUS14) |
| 24 | Reporte inteligente |

Las dos secciones finales se renumeraron a 25 (Preguntas) y 26 (Glosario) y se
les agregaron 8 filas a cada tabla. El subtítulo de la portada pasó a
"Resumen del proyecto, mapas conceptuales y patrones de diseño", y se sumó un
párrafo en "Cómo usar este resumen" explicando la estructura nueva.

## Estructura fija de cada sección de patrón

1. Qué problema resuelve, con una analogía.
2. Diagrama de estructura genérica + diagrama de la implementación en VetCare
   (son los 15 diagramas Mermaid de `VetCare-Patrones-Diagramas.docx`, reutilizados
   desde `Diagramas-Mermaid/`).
3. **Dónde verlo en el código**: tabla archivo / líneas / qué mirar, con números
   de línea reales tomados del repo.
4. Fragmento de código comentado, en Consolas sobre fondo gris.
5. **Cómo explicarlo en treinta segundos**: cuatro frases para decir de memoria.
6. **Qué te pueden preguntar**: tabla de preguntas probables con su respuesta.

## Formato

El documento usa **Arial 11** (viene de `docDefaults`, no de estilos por párrafo).
Los encabezados son el estilo Heading 1 del documento; las listas reusan `numId=2`;
las tablas replican el formato existente (cabecera verde 385723 con texto blanco,
primera columna sombreada E2EFD9, bordes A8D08D, ancho total 9026 twips).

## Cuidado al reeditar

`d.tables[n]` ya no sirve para encontrar las dos tablas originales: ahora hay 18
tablas y las originales quedaron al final. Hay que buscarlas por el texto de su
encabezado ("Pregunta" / "Término") recorriendo la lista **en reversa**.
