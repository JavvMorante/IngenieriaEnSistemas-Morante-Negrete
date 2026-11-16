# Diagramas de A03. Serialización (CUS14) — 16/10/2026

Se completaron los 7 marcadores "[ Insertar aquí … ]" que habían quedado en el capítulo
**A03. Serialización** de `Javier Morante Trabajo de Diploma Vetcare V2.1.docx`.
El documento pasó de 191 a 194 páginas (A03 ocupa ahora las páginas 187 a 193).

## Qué se insertó

| # | Sección | Archivo |
|---|---|---|
| 1 | Diagrama de Caso de Uso - CUS14 | `01-uc-cus14.png` |
| 2 | Diagrama de secuencia - CUS14 (Exportar) | `02-seq-cus14-exportar.png` |
| 3 | Diagrama de secuencia - CUS14 (Importar) | `03-seq-cus14-importar.png` |
| 4 | Diagrama de clases - CUS14 | `04-cls-cus14.png` |
| 5 | Diagrama Entidad Relación (DER) - CUS14 | `05-der-cus14.png` |
| 6 | GUI - FrmBitacoraEventos_60MN | `06-gui-cus14.png` |
| 7 | GUI - FrmVisorXmlBitacora_60MN | `07-gui-visor-cus14.png` |

Los originales (PNG + SVG editable) están en `Diagramas-CUS14-Serializacion/`.

## Criterios usados

- **Aspecto**: se replicó el estilo de Enterprise Architect que ya tienen el resto de los
  diagramas de la carpeta (marco con la pestaña `uc` / `sd` / `class DC` / `dm DER`,
  elipses celestes con sombra, cabeceras de línea de vida en degradé turquesa, barra de
  activación del actor en durazno, fragmento `opt` con pestaña, clases beige con atributos
  en rojo oscuro y operaciones con `+` verde, tablas DER en lavanda con `«column»` y `«PK»`).
  Los colores se tomaron muestreando píxeles de los diagramas existentes (CUS09 y CUS13).
- **Tipografía**: Carlito (métricamente igual a Calibri), la misma que usan los diagramas
  que ya estaban. El texto del documento no se tocó: `word/document.xml` sigue teniendo
  exactamente las mismas declaraciones de fuente que antes (21 Arial, 2 Cambria, 1 Calibri;
  el estilo Normal es Calibri 11 heredado de docDefaults).
- **Inserción**: cada párrafo marcador se vació y se le copió el `pPr` de un párrafo de
  imagen ya existente (centrado). Anchos entre 5,6" y 6,27" para que la densidad de texto
  quede pareja con los diagramas anteriores (~400 ppp).
- **Contenido**: tomado del código real (`SerializadorBitacora_60MN`, `BitacoraBLL_60MN`,
  `FrmBitacoraEventos_60MN`, `FrmVisorXmlBitacora_60MN`) y de la descomposición funcional
  que ya estaba escrita en A03.

## Faltantes conocidos de la carpeta (no tocados)

- **CUS12 - Gestión de Backup**: es el único caso de uso sin "Diagrama Entidad Relación
  (DER)"; el encabezado directamente no existe.
- **Diagramas de actividad**: la carpeta modelo tiene tres y la de VetCare no tiene ninguno.
- Encabezado huérfano "DER - Tablas involucradas en la bitácora de cambios" bajo T06b.
- N01 y N02 siguen unificados bajo "N01. Especificación funcional por proceso de negocio".
