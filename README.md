# JujiTABS

## Single-EXE Launcher

Dieses Repository baut über GitHub Actions eine einzelne Windows-Datei `JujiTABS.exe`.

Die EXE enthält `JujiTABS.dll` eingebettet. Beim Start wird die DLL in `BepInEx/plugins` kopiert und TABS gestartet.

### Build

1. Stelle `JujiTABS/bin/JujiTABS.dll` in diesem Repository bereit.
2. Push auf `main` oder starte den Workflow `Build JujiTABS EXE` manuell.
3. GitHub Actions erstellt das Artifact `JujiTABS-Windows`.
4. Im Artifact liegt `JujiTABS.exe`.

### Nutzung

Lege `JujiTABS.exe` direkt in deinen TABS-Ordner und starte sie dort. BepInEx muss bereits im TABS-Ordner installiert sein.

Die EXE selbst ist als self-contained `win-x64` Single-File veröffentlicht; .NET muss auf dem Ziel-PC nicht separat installiert werden.

### Hinweis

Eine einzelne EXE kann technisch nicht garantiert „uncrackbar“ gemacht werden. Der Single-File-Aufbau erschwert aber die einfache Weitergabe einzelner Komponenten.
