#############################################################
## Captures Powershell Script Execution Policy and sets    ##
## it to Bypass to allow script to run and resets exection ## 
## policy back to iriginal state.                          ##
#############################################################

$Policy = Get-ExecutionPolicy | Out-Null

If ($Policy) {
    'Restrictied'
    Set-ExecutionPolicy -ExecutionPolicy Bypass -Force
    $PolicyType = Get-ExecutionPolicy
    }  
    elseif ($Policy) {
        'AllSigned '
        Set-ExecutionPolicy -ExecutionPolicy bypass -Force
        $PolicyType = Get-ExecutionPolicy
    } 
    elseif ($Policy) {
        'RemoteSigned'
        Set-ExecutionPolicy -ExecutionPolicy Bypass -Force
        $PolicyType = Get-ExecutionPolicy
    }
    elseif ($Policy){
        'Unrestricted'   
        Write-Host 'Your Powershell policy is already set to unrestricted'
        $PolicyType = Get-ExecutionPolicy
    }

$sourceDir = ".\manictime-client-plugin-example\installable-plugin\Release\Plugins\Packages\"
$targetDir = $Env:systemdrive + "\Users\" + $Env:username + "\AppData\Local\Finkit\ManicTime\Plugins\"
Copy-Item -Path $sourceDir -Recurse -Destination $targetDir -Container -Force


Set-ExecutionPolicy -ExecutionPolicy $PolicyType

Write-Host "Installed. Press any key to continue ..."
$Key = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
