[Setup]
AppName=Rincon App
AppVersion=1.0.0
DefaultDirName={pf}\Rincon App
DefaultGroupName=Rincon App
OutputDir=C:\Repositorios\Rincon
OutputBaseFilename=RinconApp-Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
Languages=spanish

[Languages]
Name: "spanish"; MessagesFile: "compiler:Spanish.isl"

[Files]
Source: "C:\Repositorios\Rincon\Rincon\bin\Debug\net8.0-windows10.0.22621.0\win10-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Rincon App"; Filename: "{app}\Rincon.exe"
Name: "{commondesktop}\Rincon App"; Filename: "{app}\Rincon.exe"

[Run]
Filename: "{app}\Rincon.exe"; Description: "Ejecutar Rincon App"; Flags: nowait postinstall skipifsilent
