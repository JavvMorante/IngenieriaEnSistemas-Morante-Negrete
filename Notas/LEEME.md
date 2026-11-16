# Notas de trabajo (generadas con Claude)

Notas del proyecto TD1-VetCare, copiadas acá para tenerlas junto al repositorio.
Son apuntes de trabajo: lo que vale como entrega son los .docx de la carpeta raíz.

| Archivo | De qué trata |
|---|---|
| `resumen_proyecto_vetcare.md` | Qué se agregó al "VetCare - Resumen del proyecto.docx" y cómo está armado |
| `patrones_y_trigger_vetcare.md` | Dónde está cada patrón de diseño y cómo funciona el trigger |
| `diagramas_a03_cus14.md` | Los 7 diagramas de A03/CUS14 y los criterios con que se hicieron |
| `reporte_inteligente_vetcare.md` | Implementación del reporte inteligente, archivo por archivo |
| `ideas_reportes_vetcare.md` | Reportes implementados y los que quedan pendientes |
| `trabajo_de_diploma_historial.md` | Historial de versiones de la carpeta del trabajo |

## Dos notas que no se copiaron acá

- **Catálogo de casos de uso candidatos**: el contenido completo ya está en
  `VetCare - Casos de Uso V2.1.docx` y en `VetCare-Casos-de-Uso-Candidatos_2.docx`,
  en la carpeta raíz. La nota era un resumen de esos documentos.
- **Revisión de código de agosto de 2026**: quedó desactualizada. Señalaba inyección
  SQL, 3DES/MD5, fuga de conexiones y formularios duplicados; todo eso lo resolvió el
  rediseño de septiembre (SQL parametrizado, SHA-256 con sal, AES-256, HMAC-SHA256 y
  `using` en cada conexión). El código viejo quedó en `Legacy_60MN/`, excluido de la
  compilación.

Las dos siguen disponibles en el proyecto TD1-VetCare de Claude si las llegás a necesitar.
