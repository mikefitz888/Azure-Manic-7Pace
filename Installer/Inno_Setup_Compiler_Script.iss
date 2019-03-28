#define MyAppName "Azure DevOps Plugin for ManicTime"
#define MyAppVersion "0.1"
#define MyAppPublisher "mikefitz888"
#define MyAppURL "https://github.com/mikefitz888/Azure-Manic-7Pace"

[Setup]
AppId={{021CA80F-E00A-4D59-8214-0D6B295D5BD8}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={localappdata}\Finkit\ManicTime\Plugins\Packages\MFitzpatrick.TagSource.Azure7PacePlugin
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master
OutputBaseFilename=Install
SetupIconFile=C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\icon.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\MFitzpatrick.TagSource.Azure7PacePlugin\PluginIcon.png"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\MFitzpatrick.TagSource.Azure7PacePlugin\PluginSpec.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\MFitzpatrick.TagSource.Azure7PacePlugin\Lib\*"; DestDir: "{localappdata}\Finkit\ManicTime\Plugins\Packages\MFitzpatrick.TagSource.Azure7PacePlugin\lib"; Flags: ignoreversion