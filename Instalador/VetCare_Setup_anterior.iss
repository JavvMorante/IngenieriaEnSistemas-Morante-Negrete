; =====================================================================
;  VetCare - Instalador (T07.1 Instalador de software)
;  Inno Setup 7 - genera VetCare_Setup.exe
;
;  Qué hace:
;   1. Pide carpeta de instalación e instancia de SQL Server (requiere administrador).
;   2. Copia exe, DLL de los 7 proyectos, App.config (VetCare.exe.config),
;      UsuarioEmergencia_60MN.xml (la ayuda está integrada en el exe) y crea accesos directos.
;      Escribe la cadena de conexión elegida en VetCare.exe.config.
;   3. Consulta si existe la base VetCareBD_60MN.
;   4. Si existe: BACKUP DATABASE -> {app}\respaldos\VetCareBD_60MN_preinstalacion_aaaammdd_hhmm.bak
;      + RESTORE VERIFYONLY. Si falla, se cancela la instalación SIN tocar la base
;      (esto se hace antes de copiar archivos, en PrepareToInstall).
;   5. Base existente -> script de migración (solo agrega). Base nueva -> script inicial.
;   6. Los dígitos verificadores los verifica VetCare en el primer ingreso (no el instalador).
; =====================================================================

#define MyAppName "VetCare"
#define MyAppVersion "3.0"
#define MyAppPublisher "Javier Morante"
#define MyAppURL "https://github.com/JavvMorante/IngenieriaEnSistemas-Morante-Negrete/"
#define MyAppExeName "VetCare.exe"
#define BaseDir "C:\Users\jmora\Desktop\RepoFacultad\IngenieriaEnSistemas-Morante-Negrete"
#define ReleaseDir BaseDir + "\VetCare\VetCare\bin\Release\net48"
#define DoubleAmp(Value) StringChange(Value, "&", "&&")

[Setup]
AppId={{462232D8-DD00-4B30-94C2-0FE02C92A15D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
AllowNoIcons=yes
; Paso 1: el instalador se ejecuta con permisos de administrador
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir={#BaseDir}\Instalador
OutputBaseFilename=VetCare_Setup
SetupIconFile={#BaseDir}\VetCare\VetCare\Recursos\VetCare.ico
SolidCompression=yes
WizardStyle=modern dynamic

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Dirs]
; Carpeta donde queda el backup preventivo
Name: "{app}\respaldos"

[Files]
; Paso 2: ejecutable, DLL de los 7 proyectos, VetCare.exe.config y UsuarioEmergencia_60MN.xml
Source: "{#ReleaseDir}\*"; DestDir: "{app}"; Excludes: "*.pdb"; Flags: ignoreversion recursesubdirs createallsubdirs
; Scripts de base de datos (no se instalan: se extraen a {tmp} y se ejecutan)
Source: "{#BaseDir}\BDScript\VetCareBD_60MN.sql"; DestName: "VetCareBD_60MN_inicial.sql"; Flags: dontcopy
Source: "Scripts\VetCareBD_60MN_migracion.sql"; Flags: dontcopy

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#DoubleAmp(MyAppName)}}"; Flags: nowait postinstall skipifsilent

[Code]
const
  NombreBD = 'VetCareBD_60MN';

var
  PaginaSQL: TInputQueryWizardPage;
  BaseExistia: Boolean;
  RutaBackup: String;

{ ---------- Paso 1: página para elegir la instancia de SQL Server ---------- }
procedure InitializeWizard;
begin
  PaginaSQL := CreateInputQueryPage(wpSelectDir,
    'Servidor de base de datos',
    'Instancia de SQL Server donde se instalará la base ' + NombreBD + '.',
    'Ingrese la instancia de SQL Server (por ejemplo .\SQLEXPRESS01). ' +
    'Se usa autenticación de Windows. Si la base ya existe, antes de modificarla se generará un backup automático.');
  PaginaSQL.Add('Instancia de SQL Server:', False);
  PaginaSQL.Values[0] := '.\SQLEXPRESS01';
end;

function Instancia: String;
begin
  Result := Trim(PaginaSQL.Values[0]);
end;

function AbrirConexion(const BaseDatos: String): Variant;
begin
  Result := CreateOleObject('ADODB.Connection');
  Result.ConnectionTimeout := 15;
  Result.CommandTimeout := 0;
  Result.Open('Provider=SQLOLEDB;Data Source=' + Instancia + ';Initial Catalog=' + BaseDatos + ';Integrated Security=SSPI;');
end;

function ConsultarEscalar(Con: Variant; const Sql: String): Variant;
var
  Rs: Variant;
begin
  Rs := Con.Execute(Sql);
  Result := Rs.Fields.Item(0).Value;
  Rs.Close;
end;

{ Valida la conexión antes de dejar avanzar }
function NextButtonClick(CurPageID: Integer): Boolean;
var
  Con: Variant;
begin
  Result := True;
  if CurPageID = PaginaSQL.ID then
  begin
    if Instancia = '' then
    begin
      MsgBox('Ingrese la instancia de SQL Server.', mbError, MB_OK);
      Result := False;
      Exit;
    end;
    try
      Con := AbrirConexion('master');
      Con.Close;
    except
      MsgBox('No se pudo conectar a la instancia "' + Instancia + '".' + #13#10#13#10 + GetExceptionMessage, mbError, MB_OK);
      Result := False;
    end;
  end;
end;

{ ---------- Pasos 3 y 4: ¿existe la base? -> backup preventivo + RESTORE VERIFYONLY ----------
  Se ejecuta ANTES de copiar archivos: si algo falla, la instalación se cancela
  y la base no se toca. El .bak lo escribe el servicio de SQL Server en su propia
  carpeta de backups (único lugar donde tiene permiso) y después se copia a {app}
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  Con, Valor: Variant;
  CarpetaServidor, NombreArchivo, Intermedio: String;
begin
  Result := '';
  RutaBackup := '';
  try
    Con := AbrirConexion('master');
    try
      Valor := ConsultarEscalar(Con, 'SELECT DB_ID(N''' + NombreBD + ''')');
      BaseExistia := not VarIsNull(Valor);
      if BaseExistia then
      begin
        CarpetaServidor := ConsultarEscalar(Con, 'SELECT CAST(SERVERPROPERTY(''InstanceDefaultBackupPath'') AS NVARCHAR(4000))');
        NombreArchivo := NombreBD + '_preinstalacion_' + GetDateTimeString('yyyymmdd_hhnn', #0, #0) + '.bak';
        Intermedio := AddBackslash(CarpetaServidor) + NombreArchivo;
        RutaBackup := ExpandConstant('{app}\respaldos\') + NombreArchivo;

        WizardForm.StatusLabel.Caption := 'Generando backup preventivo de ' + NombreBD + '...';
        Con.Execute('BACKUP DATABASE [' + NombreBD + '] TO DISK = N''' + Intermedio + ''' ' +
                    'WITH FORMAT, INIT, CHECKSUM, NAME = N''' + NombreArchivo + ''', ' +
                    'DESCRIPTION = N''VetCare|' + RutaBackup + '''');

        WizardForm.StatusLabel.Caption := 'Verificando el backup (RESTORE VERIFYONLY)...';
        Con.Execute('RESTORE VERIFYONLY FROM DISK = N''' + Intermedio + ''' WITH CHECKSUM');

        if not ForceDirectories(ExtractFileDir(RutaBackup)) then
          RaiseException('No se pudo crear la carpeta ' + ExtractFileDir(RutaBackup));
        if not CopyFile(Intermedio, RutaBackup, False) then
          RaiseException('No se pudo copiar el backup a ' + RutaBackup);
        DeleteFile(Intermedio);
        Log('Backup preventivo generado y verificado: ' + RutaBackup);
      end
      else
        Log('La base ' + NombreBD + ' no existe: se creará con el script inicial.');
    finally
      Con.Close;
    end;
  except
    Result := 'No se pudo generar el backup preventivo de ' + NombreBD + '. ' +
              'La instalación se canceló sin modificar la base de datos.' + #13#10#13#10 + GetExceptionMessage;
  end;
end;

{ ---------- Paso 2: cadena de conexión en VetCare.exe.config (App.config) ---------- }
procedure ConfigurarConexion;
var
  Archivo, Cadena: String;
  Lineas: TArrayOfString;
  I: Integer;
begin
  Archivo := ExpandConstant('{app}\{#MyAppExeName}.config');
  Cadena := 'Data Source=' + Instancia + ';Initial Catalog=' + NombreBD +
            ';Integrated Security=True;TrustServerCertificate=True;';
  if not LoadStringsFromFile(Archivo, Lineas) then
    RaiseException('No se pudo leer ' + Archivo);
  for I := 0 to GetArrayLength(Lineas) - 1 do
    if Pos('key="conexionBD"', Lineas[I]) > 0 then
      Lineas[I] := #9#9 + '<add key="conexionBD" value="' + Cadena + '" />';
  if not SaveStringsToUTF8File(Archivo, Lineas, False) then
    RaiseException('No se pudo escribir ' + Archivo);
end;

{ ---------- Paso 5: ejecuta un script .sql separando los lotes por GO ---------- }
procedure EjecutarScript(const Archivo: String);
var
  Con: Variant;
  Lineas: TArrayOfString;
  Lote: String;
  I: Integer;
begin
  if not LoadStringsFromFile(Archivo, Lineas) then
    RaiseException('No se pudo leer el script ' + Archivo);
  Con := AbrirConexion('master');
  try
    Lote := '';
    for I := 0 to GetArrayLength(Lineas) - 1 do
    begin
      if CompareText(Trim(Lineas[I]), 'GO') = 0 then
      begin
        if Trim(Lote) <> '' then
          Con.Execute(Lote);
        Lote := '';
      end
      else
        Lote := Lote + Lineas[I] + #13#10;
    end;
    if Trim(Lote) <> '' then
      Con.Execute(Lote);
  finally
    Con.Close;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  Script: String;
begin
  if CurStep = ssPostInstall then
  begin
    try
      WizardForm.StatusLabel.Caption := 'Configurando la conexión a la base de datos...';
      ConfigurarConexion;

      if BaseExistia then
      begin
        WizardForm.StatusLabel.Caption := 'Actualizando la base existente (script de migración)...';
        ExtractTemporaryFile('VetCareBD_60MN_migracion.sql');
        Script := ExpandConstant('{tmp}\VetCareBD_60MN_migracion.sql');
      end
      else
      begin
        WizardForm.StatusLabel.Caption := 'Creando la base de datos (script inicial)...';
        ExtractTemporaryFile('VetCareBD_60MN_inicial.sql');
        Script := ExpandConstant('{tmp}\VetCareBD_60MN_inicial.sql');
      end;
      EjecutarScript(Script);
    except
      if BaseExistia then
        MsgBox('Error al actualizar la base de datos:' + #13#10 + GetExceptionMessage + #13#10#13#10 +
               'Sus datos están resguardados en:' + #13#10 + RutaBackup + #13#10 +
               'Puede restaurarlos desde VetCare (Backup y Restore).', mbCriticalError, MB_OK)
      else
        MsgBox('Error al crear la base de datos:' + #13#10 + GetExceptionMessage, mbCriticalError, MB_OK);
    end;
  end;
end;

{ Mensaje final con la ruta del backup, si se generó }
procedure CurPageChanged(CurPageID: Integer);
begin
  if (CurPageID = wpFinished) and (RutaBackup <> '') then
    WizardForm.FinishedLabel.Caption := WizardForm.FinishedLabel.Caption + #13#10#13#10 +
      'Backup preventivo de la base: ' + RutaBackup;
end;
