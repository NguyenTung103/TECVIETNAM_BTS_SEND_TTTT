. "$PSScriptRoot\shell-command.ps1"
param([string]$buildLocation, [string]$webRunFolder, [string]$appSettingJson, [string]$serviceName, [string]$applicationExe)
$ErrorActionPreference = "Stop"
DeployServices $buildLocation $webRunFolder $appSettingJson $serviceName $applicationExe