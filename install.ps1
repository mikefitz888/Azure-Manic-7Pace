$sourceDir = ".\manictime-client-plugin-example\installable-plugin\Release\Plugins\Packages\"
$targetDir = $Env:systemdrive + "\Users\" + $Env:username + "\AppData\Local\Finkit\ManicTime\Plugins\"
Copy-Item -Path $sourceDir -Recurse -Destination $targetDir -Container -Force
Write-Host "Installed. Press any key to continue ..."
cmd /c pause | out-null