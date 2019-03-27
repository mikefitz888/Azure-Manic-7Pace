#############################################################
## Captures Powershell Script Execution Policy and sets    ##
## it to Bypass to allow script to run and resets exection ## 
## policy back to Original state.                          ##
#############################################################

Start-Process powershell -ArgumentList '-file ./Install.ps1' -verb RunAs

$Policy = Get-ExecutionPolicy
$sourceDir = ".\manictime-client-plugin-example\installable-plugin\Release\Plugins\Packages\"
$targetDir = $Env:systemdrive + "\Users\" + $Env:username + "\AppData\Local\Finkit\ManicTime\Plugins\"

Function pressAnyKey {    
    Write-Host "Installed. Press any key to continue ..."
    $Key = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }

function softwareInstall ($Policy) {
    if ($Policy -like "Restricted") {
        $PolicyType = 'Restricted'
        Write-Host "Your policy is '$PolicyType'" -ForegroundColor Red
        Write-Host "Setting your policy to Bypass" -ForegroundColor Green
        Set-ExecutionPolicy -ExecutionPolicy Bypass -Force
        Copy-Item -Path $sourceDir -Recurse -Destination $targetDir -Container -Force
        Write-Host "Setting your policy back to '$PolicyType'"
        Set-ExecutionPolicy -ExecutionPolicy $PolicyType
    } elseif ($Policy -like 'AllSigned') {
        $PolicyType = 'AllSigned'
        Write-Host "Your policy is '$PolicyType'" -ForegroundColor Red
        Write-Host "Setting your policy to Bypass" -ForegroundColor Green
        Set-ExecutionPolicy -ExecutionPolicy Bypass -Force
        Copy-Item -Path $sourceDir -Recurse -Destination $targetDir -Container -Force
        Write-Host "Setting your policy back to '$PolicyType'"
        Set-ExecutionPolicy -ExecutionPolicy $PolicyType
    } elseif ($Policy -like 'RemoteSigned') {
        $PolicyType = 'RemoteSigned'
        Write-Host "Your policy is '$PolicyType'" -ForegroundColor Red
        Write-Host "Setting your policy to Bypass" -ForegroundColor Green
        Set-ExecutionPolicy -ExecutionPolicy Bypass -Force
        Copy-Item -Path $sourceDir -Recurse -Destination $targetDir -Container -Force
        Write-Host "Setting your policy back to '$PolicyType'"
        Set-ExecutionPolicy -ExecutionPolicy $PolicyType
    } elseif ($Policy -like 'Unrestricted') {
        $PolicyType = 'Unrestricted'
        Write-Host "Your policy is '$PolicyType' no changes are necessary to run this script" -ForegroundColor Red
        Copy-Item -Path $sourceDir -Recurse -Destination $targetDir -Container -Force
    } else {
        Write-Host "Script Failed due to undefined error"
    }
}

softwareInstall

pressAnyKey

Exit
