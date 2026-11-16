# Ideas de reportes para VetCare (16/10/2026)

Todas usan datos que ya existen en la base, sin tablas nuevas.

## Prioritarias — YA IMPLEMENTADAS en el Reporte Inteligente

### 1. Demanda estacional de insumos (RF2)
Egresos de `MovimientoStock` agrupados por mes y categoría. Barras apiladas: meses en el eje X,
una banda por categoría. Responde el objetivo declarado en G01: "proyectar la demanda en distintas
épocas del año". Es el único reporte que muestra algo que no se ve en una grilla.

### 2. Productos más entregados por período (RF1 + RF2)
Ranking desde `AtencionDetalle` (no desde `MovimientoStock`): cuenta lo entregado en atenciones,
o sea consumo clínico real y no reposición. Barras horizontales, top 10, filtro de período.
Cruza los dos requerimientos funcionales en un solo gráfico.

## Secundarias — PENDIENTES

| Reporte | RF | Datos | Gráfico |
|---|---|---|---|
| Motivos de consulta más frecuentes | RF1 | `Atencion.Motivo` agrupado | Torta |
| Atenciones por profesional y mes | RF1 | `Atencion` por `UsuarioID` | Barras agrupadas |
| Valorización del stock por categoría | RF2 | `StockActual × PrecioCosto` | Torta o barras |
| Productos próximos a vencer | RF2 | `FechaVencimiento` por tramos 30/60/90 días | Barras + tabla |

## Mapeo a los capítulos del modelo de carpeta

- **Reporte 1 – Informe de atención**: ya implementado en CUN10, solo falta documentarlo como reporte.
- **Reporte 2 – Reporte de existencias**: ya implementado en CUN15, mismo caso.
- **Reporte 3 – Reporte inteligente**: implementado el 16/10/2026 con los dos prioritarios en una sola
  pantalla con filtro de período. Falta documentar el capítulo en la carpeta.

## Advertencia

`Atencion.Motivo` es texto libre: agrupa mal si cada veterinario escribe distinto. Para el reporte de
motivos de consulta habría que pasarlo a lista desplegable primero, o postergarlo.
