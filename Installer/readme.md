# Compile Inno Installer

## History of Inno Installer

If you wish to compile the software yourself and do not have Visual Studio you can use Inno Setup.
Inno Setup is an open source utility that’s been in active development since 1997. It was created
partly in response to the at-the-time sub par InstallShield Express.

### Compile Install.exe

1. Download [Inno Setup](http://www.jrsoftware.org/isdl.php) and install
2. Load the `Inno_Setup_Compiler_Script.iss` into the Inno software.
3. Download the [Source Code](https://github.com/mikefitz888/Azure-Manic-7Pace/archive/master.zip) files to `C:\Users\%USERNAME%\Downloads
4. Extract the .Zip file to C:\Users\%USERNAME%\Downloads\Azure-Manic-7Pace-master
5. Replace the {USERNAME} fields in the `Inno_Setup_Compiler_Script.iss` script with your Windows Profile name found in `C:\Users`

        [Setup]
        OutputDir=C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master
        SetupIconFile=C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\Installer\icon.ico

        [Files]
        Source: "C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\MFitzpatrick.TagSource.Azure7PacePlugin\PluginIcon.png"; DestDir: "{app}"; Flags: ignoreversion
        Source: "C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\MFitzpatrick.TagSource.Azure7PacePlugin\PluginSpec.json"; DestDir: "{app}"; Flags: ignoreversion
        Source: "C:\Users\{USERNAME}\Downloads\Azure-Manic-7Pace-master\MFitzpatrick.TagSource.Azure7PacePlugin\Lib\*"; DestDir: "{localappdata}\Finkit\ManicTime\Plugins\Packages\MFitzpatrick.TagSource.Azure7PacePlugin\lib"; Flags: ignoreversion

Example:

        [Setup]
        OutputDir=C:\Users\noodlemctwoodle\Downloads\Azure-Manic-7Pace-master

6. Press CTRL+F9 to compile the installer, alternatively you can click Build in the File Menu, then Compile.
7. Locate Installer.exe in `C:\Users\%USERNAME%\Downloads\Azure-Manic-7Pace-master`