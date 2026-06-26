# Script para crear instalador de Rincon App
Write-Host "=== Creando Instalador de Rincon App ===" -ForegroundColor Green

# Verificar si Inno Setup esta instalado
$innoPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
if (-not (Test-Path $innoPath)) {
    Write-Host "ERROR: Inno Setup no encontrado en: $innoPath" -ForegroundColor Red
    Write-Host "Por favor instala Inno Setup 6 desde: https://jrsoftware.org/isdl.php" -ForegroundColor Yellow
    exit 1
}

# Paso 1: Compilar la aplicacion para Windows
Write-Host "Paso 1: Compilando aplicacion para Windows..." -ForegroundColor Yellow
Set-Location "Rincon"

$compileCommand = "dotnet publish -f net8.0-windows10.0.22621.0 -c Release -r win-x64 --self-contained false -o ..\PublishOutput"
Write-Host "Ejecutando: $compileCommand" -ForegroundColor Cyan
Invoke-Expression $compileCommand

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Fallo la compilacion" -ForegroundColor Red
    Set-Location ".."
    exit 1
}

Set-Location ".."

# Verificar que se creo el ejecutable
$exePath = "PublishOutput\Rincon.exe"
if (-not (Test-Path $exePath)) {
    Write-Host "ERROR: No se encontro el ejecutable en $exePath" -ForegroundColor Red
    exit 1
}

Write-Host "Compilacion exitosa!" -ForegroundColor Green

# Paso 2: Crear script ISS para Inno Setup
Write-Host "Paso 2: Creando script de instalacion..." -ForegroundColor Yellow

$issContent = @"
[Setup]
AppName=Rincon App
AppVersion=1.0.0
AppPublisher=Rincon Team
AppPublisherURL=
DefaultDirName={autopf}\RinconApp
DefaultGroupName=Rincon App
AllowNoIcons=yes
LicenseFile=
OutputDir=Output
OutputBaseFilename=RinconApp-Installer
SetupIconFile=
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "Crear icono en el escritorio"; GroupDescription: "Iconos adicionales:"

[Files]
Source: "PublishOutput\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Rincon App"; Filename: "{app}\Rincon.exe"
Name: "{group}\Desinstalar Rincon App"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Rincon App"; Filename: "{app}\Rincon.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Rincon.exe"; Description: "Ejecutar Rincon App"; Flags: nowait postinstall skipifsilent
"@

$issContent | Out-File -FilePath "instalador.iss" -Encoding UTF8
Write-Host "Script ISS creado exitosamente!" -ForegroundColor Green

# Paso 3: Crear directorio de salida
if (-not (Test-Path "Output")) {
    New-Item -ItemType Directory -Path "Output" | Out-Null
}

# Paso 4: Ejecutar Inno Setup
Write-Host "Paso 3: Generando instalador..." -ForegroundColor Yellow
$issPath = Join-Path (Get-Location) "instalador.iss"

Write-Host "Ejecutando Inno Setup..." -ForegroundColor Cyan
& $innoPath $issPath

if ($LASTEXITCODE -eq 0) {
    Write-Host "" -ForegroundColor Green
    Write-Host "=== INSTALADOR CREADO EXITOSAMENTE ===" -ForegroundColor Green
    Write-Host "Ubicacion: $(Join-Path (Get-Location) 'Output\RinconApp-Installer.exe')" -ForegroundColor Cyan
    Write-Host "" -ForegroundColor Green
} else {
    Write-Host "ERROR: Fallo al crear el instalador" -ForegroundColor Red
    exit 1
}