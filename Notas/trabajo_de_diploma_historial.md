# Historial de versiones de la carpeta del Trabajo de Diploma

## Versión inicial (26/09/2026)

Salidas:
- `Javier Morante-Trabajo de diploma-VetCare.docx`: basado en el documento de Ingeniería,
  todo en Arial, con el mismo formato (estilos 1 / Estilo2 / Estilo21 y la tabla de
  especificación de CUS01 como plantilla).
- `VetCare.eapx`: copia del EAPX existente (VetCare-Ingenieria-Diagramas) + paquete
  "Trabajo de Diploma - Casos de Uso" con 24 diagramas de casos de uso y 24 de secuencia.

Decisiones: Seguridad T02.3 Logout, T02.4 Desbloquear, T02.5 Cambiar Contraseña,
T02.6 CUS02, T02.7 CUS03, T02.8 CUS04. Negocio: capítulos N01–N06 (PN1–PN6), cada caso
de uso con DF, diagrama de caso de uso, especificación y diagrama de secuencia.

Iteración 2 (26/09/2026) – Arquitectura base: T01.1 diagrama de capas rehecho según la
solución real (7 proyectos); T01.2 captura real del Explorador de soluciones.

## Versión V2 (29/09/2026)

Formato de la carpeta de referencia (Lautaro Biglieri): carátula de Javier, paleta verde,
G00–G05, N00 (PN1–PN6 + visión general), T01, T02, T03, T06.
`VetCare V2.eapx` (EA 15): EAPX original + paquete "Trabajo de Diploma V2 - VetCare".

## Versión V2.1 (06/10/2026)

`Javier Morante Trabajo de Diploma Vetcare V2.1.docx` (~181 páginas): sin G00; G01 con
5 párrafos; G02 con RFN1 Gestión de Pacientes y RFN2 Gestión de Stock.

Procesos de negocio reducidos a 2 (decisión de Javier: reducir alcance):
PN1 Gestión de Pacientes = CUN01–CUN10; PN2 Gestión de Stock = CUN11–CUN15.
Turnos (CUN16–18) y estadísticas (CUN19–20) fuera de alcance. Recepcionista eliminado
como actor.

Parte técnica en el orden de la referencia: T01 Arquitectura, T02 Log In/Log Out
(CUS01, 02, 03, 08, 05, 07, 06), T03 Encriptado, T04 Perfiles (CUS04), T05 Idiomas
(CUS11, patrón Observer), T06a Bitácora (CUS09), T06b (CUS10), T07 Backup (CUS12,
diseño propuesto), T08 Dígitos Verificadores (CUS13, HMAC-SHA256).

`VetCare - Casos de Uso V2.1.docx`: catálogo adaptado (13 CUS + 15 CUN = 28).
`VetCare V2.eapx`: se le agregó el paquete "Trabajo de Diploma V2.1 - VetCare".

Supuesto a confirmar: VetCare "con sede en Lomas de Zamora" (G01).

## Agregados del 16/10/2026

- Capítulo **A03. Serialización** con CUS14 (exportar/importar la bitácora en XML) y sus
  7 diagramas. El documento quedó en 194 páginas.
- Fuera de la carpeta: el **Reporte Inteligente** implementado en el código y el
  **Resumen del proyecto** ampliado con los patrones de diseño.

## Pendientes conocidos

- CUS12 - Gestión de Backup no tiene DER.
- La carpeta no tiene diagramas de actividad (el modelo tiene tres).
- N01 y N02 siguen unificados bajo un solo encabezado.
- Encabezado huérfano "DER - Tablas involucradas en la bitácora de cambios" bajo T06b.
- Capítulos Reporte 1 / Reporte 2 / Reporte 3 y D01 todavía sin documentar.
- Manual de instalación: decisión de Javier, no va en esta entrega.
- Recepcionista no figura en G04; proceso de cobranza sin especificar.
