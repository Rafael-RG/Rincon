[Setup]
AppId={{D2F5E4C3-8B1A-4F2E-9C5D-7E6A8B9C0D1E}
AppName=Rincon App
AppVersion=2.0.20260625
AppPublisher=Rincon Solutions
DefaultDirName={autopf}\Rincon App
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputDir=.\Instaladores
OutputBaseFilename=RinconApp-Setup-v20260625
Compression=lzma
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\Rincon.exe

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "Rincon\bin\Release\net8.0-windows10.0.22621.0\win10-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\Rincon App"; Filename: "{app}\Rincon.exe"
Name: "{autodesktop}\Rincon App"; Filename: "{app}\Rincon.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Rincon.exe"; Description: "{cm:LaunchProgram,Rincon App}"; Flags: nowait postinstall skipifsilent
