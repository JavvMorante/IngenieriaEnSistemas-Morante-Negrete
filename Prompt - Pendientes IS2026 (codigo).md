# Prompt para Claude: completar en el código los puntos de "Necesario para aprobar IS2026"

Trabajá sobre `VetCare/VetCare.sln` (C#, WinForms, .NET Framework 4.8, `LangVersion latest`, `Nullable enable`, **sin paquetes NuGet nuevos**). El documento de referencia es `Javier Morante-Ingenieria de software-VetCare V2.docx` (raíz del repo): el código tiene que hacer exactamente lo que ese documento describe en T02.2, T04, T06A y T07. Leé antes `VetCare/README_VetCare_60MN.md`. Respetá las reglas del proyecto: sufijo `_60MN`, GUI → BLL → DAL (la GUI nunca accede a la DAL), permisos con `SesionActual_60MN.RequierePermiso`, eventos nuevos también en `CatalogoEventos_60MN`, `GestorDigitoVerificador_60MN.ActualizarFila` después de cada escritura, formularios que heredan de `FormBase_60MN` y textos nuevos traducidos en la tabla `Traduccion`. Hacé una fase por vez, mostrame los archivos tocados y esperá mi OK. No hay `dotnet` en la máquina: revisá con cuidado que compile.

## Fase 1 – Bloqueo al 4.º intento (T02.2.8)
- `BLL_60MN/UsuarioBLL_60MN.cs`: `MaximoIntentos = 4`. Intentos 1 y 2: "Usuario o contraseña incorrectos. Intentos restantes: N.". Intento 3: advertencia "le queda 1 intento antes de que la cuenta se bloquee". Intento 4: bloqueo, evento `UsuarioBloqueado` y mensaje de contactar a un administrador.
- Actualizar textos de bitácora, traducciones y cualquier comentario o pantalla que diga 3 intentos (incluido el usuario de emergencia).

## Fase 2 – Patrón State en el login (T04.5)
- `Interfaces_60MN/IEstadoUsuario_60MN.cs` (`Nombre`, `ValidarIngreso(contexto)`, `RegistrarFallo(contexto)`).
- `BLL_60MN/Estados/EstadoActivo_60MN`, `EstadoBloqueado_60MN`, `EstadoBaja_60MN` y `BLL_60MN/ContextoUsuario_60MN` (estado actual, `CambiarEstado`). El estado inicial sale de `Activo`/`Bloqueado` del usuario.
- `UsuarioBLL_60MN.Login` delega en el contexto: `ValidarIngreso()` reemplaza los `if` de baja y bloqueo, y `RegistrarFallo()` hace la transición Activo → Bloqueado al 4.º intento. El comportamiento visible tiene que quedar igual al de la Fase 1.

## Fase 3 – Pantalla de bitácora: paginación, PDF y LIMPIAR (T06A)
- **Paginación:** 50 registros por página consultando solo la página visible (`ORDER BY FechaHora DESC OFFSET @salto ROWS FETCH NEXT @tam ROWS ONLY` y un `COUNT(*)` con los mismos filtros) en `Seguridad_60MN/Auditoria/GestorBitacora_60MN`, expuesto por `BitacoraBLL_60MN`. En `FrmBitacoraEventos_60MN`: botones `|<`, `<`, `>`, `>|` y la leyenda "Página x de y"; al buscar vuelve a la página 1.
- **LIMPIAR:** hoy solo limpia los filtros. Tiene que restablecer todos los filtros a sus valores iniciales, volver a la página 1 y recargar la grilla con los datos de arranque (lo mismo que se ve al abrir la pantalla).
- **Exportar PDF:** botón "Exportar PDF" y casilla "Exportar solo lo filtrado". Tildada exporta el resultado de los filtros actuales (todas las páginas); destildada exporta la totalidad de la bitácora. Sin NuGet: generar el PDF con `PrintDocument` y la impresora "Microsoft Print to PDF" (como el reporte inteligente con `ImpresionTexto_60MN`), con título, filtros aplicados, fecha y tabla de eventos. Permiso `SEG_BITACORA_EXPORT` y evento `BitacoraExportada`.

## Fase 4 – Patrón Strategy en la exportación (T04.4)
- `Interfaces_60MN/IEstrategiaExportacion_60MN.cs` (`Formato`, `Extension`, `Exportar(registros, ruta)`).
- `Seguridad_60MN/Auditoria/ExportadorPdf_60MN`, `ExportadorCsv_60MN` y `ExportadorXml_60MN` (este último reutiliza `SerializadorBitacora_60MN`). Si el PDF necesita la GUI para imprimir, dejá la estrategia PDF en el proyecto VetCare y registrala en la BLL por la interfaz.
- `BitacoraBLL_60MN.Exportar(formato, registros, ruta)` elige la estrategia de un diccionario. `FrmBitacoraEventos_60MN` usa ese método para PDF, CSV y XML.

## Fase 5 – Ayuda por eventos del mouse (T07.2)
- En `FormBase_60MN`, al cargar, recorrer los controles y asignar un `ToolTip` (aparece al pasar el mouse) con el texto de ayuda del control. Los textos salen de `TemasAyuda_60MN` / tabla `Traduccion` con la clave `ayuda.<Formulario>.<Control>`, respetan el idioma y se actualizan al cambiarlo (en `ActualizarIdioma`).
- Cargar los textos de los botones y campos principales de Login, Gestión de usuarios, Bitácora de eventos, Clientes, Productos y Dígitos verificadores. F1 sigue funcionando como hoy.

## Fase 6 – Instalador con backup preventivo (T07.1)
- Script Inno Setup `Instalador/VetCare_60MN.iss` que genere `VetCare_Setup.exe`: copia los binarios de Release de los 7 proyectos, `App.config`, `UsuarioEmergencia_60MN.xml`, recursos de ayuda y crea accesos directos.
- Paso previo a tocar la base: `BDScript/BackupPreinstalacion_60MN.sql` que, si existe `VetCareBD_60MN`, hace `BACKUP DATABASE ... WITH INIT, CHECKSUM` a `respaldos\VetCareBD_60MN_preinstalacion_aaaammdd_hhmm.bak` y `RESTORE VERIFYONLY`. Si falla, la instalación se cancela sin modificar la base. El instalador lo ejecuta con `sqlcmd` contra la instancia elegida.
- Con la base existente ejecuta solo los scripts de migración; si no existe, ejecuta `VetCareBD_60MN.sql`.
- Dejá en el README cómo compilar el instalador y probar la reinstalación.

## Cierre
Actualizá `README_VetCare_60MN.md` y pasame la lista de archivos por fase, los scripts SQL a correr y una prueba manual de cada punto (4 intentos, paginación, LIMPIAR, PDF filtrado y total, tooltips, instalación sobre una base existente).
