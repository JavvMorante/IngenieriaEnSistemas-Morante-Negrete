# Reporte Inteligente (Reporte 2) — implementación

Implementado el 16/10/2026. Cruza los dos requerimientos funcionales del trabajo
en una sola pantalla, con gráficos dibujados a mano con GDI+ (sin paquetes NuGet).

## Qué muestra

1. **Demanda de insumos por mes y categoría** (RF2 Gestión de Stock) — barras apiladas.
   Sale de `MovimientoStock` con `Tipo = 'EGRESO'` (la `Cantidad` se guarda con signo
   negativo, se devuelve en positivo), agrupado por año/mes/categoría.
2. **Productos más entregados en atenciones** (RF1 Gestión de Pacientes) — barras
   horizontales ordenadas de mayor a menor. Sale de `AtencionDetalle` unido a `Atencion`,
   no de `MovimientoStock`, porque acá interesa el consumo clínico real y no los ingresos
   ni los ajustes.

Filtros: período (desde / hasta) y cantidad de productos del ranking (1 a 20, por defecto 10).
Salidas: pantalla, impresión / PDF (se imprimen los datos en texto, no la imagen) y CSV
(los dos bloques en un archivo, identificados por la columna `Bloque`).

## Archivos

| Capa | Archivo |
|---|---|
| Entidades | `Entidades_60MN/ReporteInteligente_60MN.cs` — `DemandaMensual_60MN`, `ProductoEntregado_60MN` |
| DAL | `DAL_60MN/ReporteInteligenteDAL_60MN.cs` — `DemandaPorMesYCategoria`, `TopProductosEntregados` (solo lectura) |
| BLL | `BLL_60MN/ReporteInteligenteBLL_60MN.cs` — `ResultadoReporteInteligente_60MN` + `Generar(desde, hasta, top)` |
| GUI | `VetCare/Reportes/FrmReporteInteligente_60MN.cs` / `.Designer.cs` / `.resx` |
| GUI | `VetCare/Utilidades/Graficos_60MN.cs` — `BarrasApiladas`, `BarrasHorizontales`, `Vacio` |
| GUI | `VetCare/Utilidades/PanelGrafico_60MN.cs` — `Panel` con `DoubleBuffered` para que el dibujo no parpadee |

## Cambios en archivos existentes

- `Seguridad_60MN/Auditoria/CatalogoEventos_60MN.cs`: nuevo valor
  `EventoSistema_60MN.ReporteInteligenteGenerado`, registrado en
  `ModuloSistema_60MN.Stock` con criticidad 5. El catálogo valida en tiempo de ejecución
  que todo valor del enum esté registrado, por eso los dos cambios van juntos.
- `VetCare/FrmMenuPrincipal_60MN.Designer.cs`: `mnuReporte2` pasa de "Reporte 2" /
  `mnuPendiente_Click` a "Reporte inteligente" / `mnuReporte2_Click`.
- `VetCare/FrmMenuPrincipal_60MN.cs`: nuevo `mnuReporte2_Click` → `Abrir<FrmReporteInteligente_60MN>()`.
- `BDScript/VetCareBD_60MN.sql`: bloque de traducciones al inglés del nuevo formulario.
- `BDScript/Traducciones_ReporteInteligente_60MN.sql` (nuevo): el mismo bloque como
  script de actualización para una base ya creada.

## Decisiones de diseño

- **Seguridad**: `Generar` empieza con `SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Reportes)`
  (patente `REP_REPORTES`, PatenteID 30, ya asignada a la familia 1). El menú además oculta
  el ítem por `Tag`, así que la autorización queda en los dos niveles de siempre.
- **Bitácora**: la BLL registra `ReporteInteligenteGenerado` con el período y los totales.
- **Dígitos verificadores**: el reporte no escribe ninguna tabla, por lo tanto no interviene
  en DVH/DVV.
- **Validaciones de la BLL**: desde > hasta, inicio en fecha futura y período de más de
  36 meses lanzan `NegocioException_60MN`. El límite superior de las consultas es exclusivo
  (`hasta.AddDays(1)`) para incluir todas las horas del último día.
- **Multi-idioma**: el formulario hereda de `FormBase_60MN` (Observer). Los títulos de los
  gráficos viven dentro del dibujo, así que `AlCambiarIdioma()` invalida los dos paneles.
- **Sin dependencias nuevas**: nada de NuGet; los gráficos se trazan en el evento `Paint`.

## Por qué hay datos aunque el módulo de Stock esté deshabilitado

`VetCare.csproj` excluye de la compilación `Stock\**` (CUN11–CUN15) y los dos formularios
de bitácora de cambios. El Reporte Inteligente vive en `Reportes\`, que sí compila, y los
datos que consume los escribe el carrito de medicación (CUN09), que está habilitado: al
confirmar la entrega inserta en `AtencionDetalle` y genera los movimientos `EGRESO` en
`MovimientoStock`.

## Pendiente

- Compilar en Visual Studio.
- Correr `BDScript/Traducciones_ReporteInteligente_60MN.sql` para que el formulario se vea
  traducido en inglés.
- Documentar el capítulo "Reporte 2" en la carpeta del trabajo de diploma.
