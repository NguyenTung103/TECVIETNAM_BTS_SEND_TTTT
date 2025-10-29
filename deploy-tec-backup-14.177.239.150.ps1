param(
    [string]$buildLocation,
    [string]$webRunFolder,
    [string]$appSettingJson,
    [string]$serviceName,
    [string]$applicationExe
)
. "$PSScriptRoot\shell-command.ps1"
$ErrorActionPreference = "Stop"
DeployServices $buildLocation $webRunFolder $appSettingJson $serviceName $applicationExe