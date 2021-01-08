[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [pscredential]
    $Credential
)

$DebugPreference = "Continue"

$ServiceName = "PCM"

$WorkerSource = Join-Path $PSScriptRoot "src\Worker\Worker.csproj"
$PublishedWorkerRoot = Join-Path $PSScriptRoot "src\Worker\bin\Debug\net5.0\publish\"
$PublishedWorkerBinary = Join-Path $PublishedWorkerRoot "IV.PotentialComputingMachine.exe"
Write-Debug "Worker root: $PublishedWorkerRoot"


$ExistingService = Get-Service -Name $ServiceName
if ($null -ne $ExistingService) {
    Write-Debug "Removing existing service"
    $ExistingService

    Stop-Service -InputObject $ExistingService
    Remove-Service -InputObject $ExistingService
}

Write-Debug "Publishing new service"
dotnet publish $WorkerSource -c Debug

Write-Debug "Creating new service"
$NewService = New-Service `
    -Name $ServiceName `
    -BinaryPathName $PublishedWorkerBinary `
    -Credential $Credential `
    -StartupType Automatic

Write-Debug "Starting new service"
Start-Service -InputObject $NewService