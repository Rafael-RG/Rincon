# Script para crear el instalador de Rincon App
# Este script copia todos los archivos necesarios a una carpeta de distribución

Write-Host "=== Creando Instalador de Rincon App ===" -ForegroundColor Green

# Rutas
$SourcePath = "C:\Repositorios\Rincon\Rincon\bin\Release\net8.0-windows10.0.22621.0\win10-x64"
$DistributionPath = "C:\Repositorios\Rincon\RinconApp-Installer"
$ReadmePath = "$DistributionPath\README.txt"

# Limpiar carpeta de distribución si existe
if (Test-Path $DistributionPath) {
    Write-Host "Limpiando carpeta anterior..." -ForegroundColor Yellow
    Remove-Item $DistributionPath -Recurse -Force
}

# Crear carpeta de distribución
Write-Host "Creando carpeta de distribución..." -ForegroundColor Cyan
New-Item -Path $DistributionPath -ItemType Directory -Force | Out-Null

# Copiar todos los archivos de la aplicación
Write-Host "Copiando archivos de la aplicación..." -ForegroundColor Cyan
Copy-Item "$SourcePath\*" -Destination $DistributionPath -Recurse -Force

# Crear README con instrucciones
$ReadmeContent = @"
=== RINCON APP - INSTALACION ===

Este paquete contiene la aplicación Rincon con el nuevo sistema de backup automático.

CARACTERÍSTICAS NUEVAS:
- Backup automático de base de datos dos veces al día (11:00 AM y 5:00 PM)
- Respaldos guardados en carpeta "DatabaseBackups" en Documents
- Mantiene automáticamente 14 backups (7 días)
- Compatible con Azure SQL Database

INSTALACIÓN:
1. Copia todo el contenido de esta carpeta a tu ubicación deseada (ej: C:\Program Files\RinconApp)
2. Ejecuta 'Rincon.exe' para iniciar la aplicación

REQUISITOS DEL SISTEMA:
- Windows 10 versión 22621.0 o superior
- .NET 8.0 Runtime (se descarga automáticamente si es necesario)
- Conexión a internet para Azure SQL Database

BACKUP AUTOMÁTICO:
- Los backups se crean automáticamente a las 11:00 AM y 5:00 PM
- Se almacenan en: %USERPROFILE%\Documents\RinconApp\DatabaseBackups\
- Formato: RinconDB_YYYYMMDD_HHMM.sql

SOPORTE:
Para soporte técnico, contacte al equipo de desarrollo.

Versión: $(Get-Date -Format 'yyyy.MM.dd')
"@

Set-Content -Path $ReadmePath -Value $ReadmeContent -Encoding UTF8

# Crear script de instalación simple
$InstallScript = @"
@echo off
echo === INSTALADOR DE RINCON APP ===
echo.

set "INSTALL_DIR=C:\Program Files\RinconApp"

echo Creando directorio de instalacion...
if not exist "%INSTALL_DIR%" mkdir "%INSTALL_DIR%"

echo Copiando archivos...
xcopy /E /I /H /Y *.* "%INSTALL_DIR%\"

echo.
echo === INSTALACION COMPLETADA ===
echo La aplicacion se ha instalado en: %INSTALL_DIR%
echo.
echo Para ejecutar la aplicacion:
echo "%INSTALL_DIR%\Rincon.exe"
echo.
pause
"@

Set-Content -Path "$DistributionPath\Instalar.bat" -Value $InstallScript -Encoding ASCII

# Crear script de desinstalación
$UninstallScript = @"
@echo off
echo === DESINSTALADOR DE RINCON APP ===
echo.

set "INSTALL_DIR=C:\Program Files\RinconApp"

echo ATENCION: Esto eliminara todos los archivos de la aplicacion.
echo Los backups en Documents NO seran eliminados.
echo.
set /p "confirm=¿Continuar con la desinstalacion? (S/N): "
if /i not "%confirm%"=="S" goto :cancel

echo Eliminando archivos...
if exist "%INSTALL_DIR%" (
    rmdir /S /Q "%INSTALL_DIR%"
    echo Aplicacion desinstalada correctamente.
) else (
    echo La aplicacion no se encuentra instalada.
)

goto :end

:cancel
echo Desinstalacion cancelada.

:end
echo.
pause
"@

Set-Content -Path "$DistributionPath\Desinstalar.bat" -Value $UninstallScript -Encoding ASCII

# Información del paquete creado
$TotalSize = (Get-ChildItem $DistributionPath -Recurse | Measure-Object -Property Length -Sum).Sum / 1MB
$FileCount = (Get-ChildItem $DistributionPath -Recurse -File).Count

Write-Host ""
Write-Host "=== INSTALADOR CREADO EXITOSAMENTE ===" -ForegroundColor Green
Write-Host "Ubicación: $DistributionPath" -ForegroundColor White
Write-Host "Tamaño total: $([math]::Round($TotalSize, 2)) MB" -ForegroundColor White
Write-Host "Archivos incluidos: $FileCount" -ForegroundColor White
Write-Host ""
Write-Host "ARCHIVOS INCLUIDOS:" -ForegroundColor Cyan
Write-Host "- Instalar.bat (Script de instalación automática)" -ForegroundColor Yellow
Write-Host "- Desinstalar.bat (Script de desinstalación)" -ForegroundColor Yellow
Write-Host "- README.txt (Instrucciones detalladas)" -ForegroundColor Yellow
Write-Host "- Rincon.exe (Aplicación principal)" -ForegroundColor Yellow
Write-Host "- Todas las dependencias y recursos necesarios" -ForegroundColor Yellow
Write-Host ""
Write-Host "Para distribuir: Comprime la carpeta '$DistributionPath' en un archivo ZIP" -ForegroundColor Green