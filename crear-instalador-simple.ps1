# Script simplificado para crear instalador de Rincon App
Write-Host "=== Creando Instalador de Rincon App ===" -ForegroundColor Green

# Rutas - Usando los archivos de Release que acabamos de generar
$AppFiles = "Rincon\bin\Release\net8.0-windows10.0.22621.0\win10-x64"
$InstallerScript = "RinconInstaller.iss"

# Verificar que existen los archivos compilados
if (-not (Test-Path $AppFiles)) {
    Write-Host "ERROR: No se encontraron los archivos compilados en $AppFiles" -ForegroundColor Red
    Write-Host "Ejecuta primero: dotnet build -c Release" -ForegroundColor Yellow
    exit 1
}

Write-Host "Paso 1: Creando script de Inno Setup..." -ForegroundColor Yellow

# Crear el script .iss para Inno Setup
$issContent = @"
; Script de instalador para Rincon App con Backup Automático
[Setup]
AppId={{D2F5E4C3-8B1A-4F2E-9C5D-7E6A8B9C0D1E}
AppName=Rincon App
AppVersion=2.0.$(Get-Date -Format 'yyMMdd')
AppPublisher=Rincon Solutions
AppPublisherURL=https://github.com/rincon
AppSupportURL=https://github.com/rincon
AppUpdatesURL=https://github.com/rincon
DefaultDirName={autopf}\Rincon App
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputDir=.\Instaladores
OutputBaseFilename=RinconApp-Setup-v$(Get-Date -Format 'yyyyMMdd')
SetupIconFile=$AppFiles\appicon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
LanguageDetectionMethod=uilanguage
UninstallDisplayIcon={app}\Rincon.exe

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "$AppFiles\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\Rincon App"; Filename: "{app}\Rincon.exe"; Comment: "Aplicación Rincon con Backup Automático"
Name: "{autodesktop}\Rincon App"; Filename: "{app}\Rincon.exe"; Tasks: desktopicon; Comment: "Aplicación Rincon con Backup Automático"

[Run]
Filename: "{app}\Rincon.exe"; Description: "{cm:LaunchProgram,Rincon App}"; Flags: nowait postinstall skipifsilent

[Registry]
Root: HKCU; Subkey: "Software\RinconApp"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"
Root: HKCU; Subkey: "Software\RinconApp"; ValueType: string; ValueName: "Version"; ValueData: "2.0.$(Get-Date -Format 'yyMMdd')"
Root: HKCU; Subkey: "Software\RinconApp"; ValueType: string; ValueName: "BackupEnabled"; ValueData: "true"

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    MsgBox('Rincon App se ha instalado correctamente.' + #13#13 + 
           'NUEVA CARACTERÍSTICA:' + #13 +
           '• Backup automático de base de datos 2 veces al día (11:00 AM y 5:00 PM)' + #13 +
           '• Backups guardados en Documents\RinconApp\DatabaseBackups\' + #13 +
           '• Compatible con Azure SQL Database' + #13 + #13 +
           'La aplicación iniciará el servicio de backup automáticamente.', 
           mbInformation, MB_OK);
  end;
end;
"@

# Escribir el archivo .iss
$issContent | Out-File -FilePath $InstallerScript -Encoding UTF8

Write-Host "Paso 2: Ejecutando Inno Setup Compiler..." -ForegroundColor Yellow

# Buscar Inno Setup Compiler
$InnoSetupPath = @(
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles}\Inno Setup 6\ISCC.exe",
    "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
    "C:\Program Files\Inno Setup 6\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $InnoSetupPath) {
    Write-Host "ERROR: No se encontró Inno Setup Compiler" -ForegroundColor Red
    Write-Host "Verifica que Inno Setup esté instalado correctamente" -ForegroundColor Yellow
    exit 1
}

# Crear directorio de salida
if (-not (Test-Path "Instaladores")) {
    New-Item -ItemType Directory -Path "Instaladores"
}

# Compilar el instalador
try {
    Write-Host "Compilando instalador con: $InnoSetupPath" -ForegroundColor Cyan
    & $InnoSetupPath $InstallerScript
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "¡ÉXITO! Instalador creado correctamente" -ForegroundColor Green
        Write-Host "Archivo de instalador: .\Instaladores\RinconApp-Setup.exe" -ForegroundColor White
        
        # Verificar que el instalador se creó
        if (Test-Path ".\Instaladores\RinconApp-Setup.exe") {
            $installerSize = (Get-Item ".\Instaladores\RinconApp-Setup.exe").Length / 1MB
            Write-Host "Tamaño del instalador: $($installerSize.ToString('F2')) MB" -ForegroundColor Cyan
        }
    } else {
        Write-Host "ERROR: Falló la compilación del instalador (código: $LASTEXITCODE)" -ForegroundColor Red
    }
} catch {
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Proceso completado." -ForegroundColor Green