[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [PSCredential]
    $ServiceAccount,

    [Parameter(Mandatory)]
    [string]
    $PublishDestination
)

$DebugPreference = "Continue"

$ServiceName = "PCM"

Write-Debug "Checking publish destination"
if (-not (Test-Path $PublishDestination -PathType Container)) {
    throw "Invalid publish destination"
}

$ExistingService = Get-Service -Name $ServiceName
if ($null -ne $ExistingService) {
    Write-Debug "Removing existing service"

    Stop-Service -InputObject $ExistingService
    Remove-Service -InputObject $ExistingService
}

Write-Debug "Clearing publish destination"
Get-ChildItem $PublishDestination -Recurse | Remove-Item -Force -Recurse

Write-Debug "Publishing new service"
$WorkerSource = Join-Path $PSScriptRoot "..\src\Worker\Worker.csproj"
dotnet publish $WorkerSource --configuration Debug --output $PublishDestination
$PublishedWorkerBinary = Join-Path $PublishDestination "IV.PotentialComputingMachine.exe"

Write-Debug "Creating new service"
$NewService = New-Service `
    -Name $ServiceName `
    -BinaryPathName $PublishedWorkerBinary `
    -Credential $ServiceAccount `
    -StartupType Automatic

Write-Debug "Starting new service"
Start-Service -InputObject $NewService