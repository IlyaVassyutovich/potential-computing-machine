[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [string]
    $PublishDestination
)

$DebugPreference = "Continue"

Write-Debug "Checking publish destination"
if (-not (Test-Path $PublishDestination -PathType Container)) {
    throw "Invalid publish destination"
}
$ServiceName = "PCM"

$ExistingService = Get-Service -Name $ServiceName
if ($null -ne $ExistingService) {
    Write-Debug "Removing existing service"

    Stop-Service -InputObject $ExistingService
    Remove-Service -InputObject $ExistingService
}

Write-Debug "Clearing publish destination"
Get-ChildItem $PublishDestination -Recurse | Remove-Item -Force -Recurse