# $cert = Get-ChildItem -Path Cert:\LocalMachine\Root | Where-Object { $_.FriendlyName -like "*Cosmos*" }
# if ($cert) {
#     Write-Information "✅ Cosmos DB Emulator certificate already imported."
#     exit 0
# }

# Check for admin rights
if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Warning "This script needs to be run as Administrator."

    # Relaunch the script with elevated privileges
    $script = $MyInvocation.MyCommand.Definition
    Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$script`"" -Verb RunAs
    Wait-Process -Name powershell -ErrorAction SilentlyContinue
    exit
}


# # Ensure Docker is running
# if (-not (Get-Service -Name docker -ErrorAction SilentlyContinue)) {
#     Write-Error "Docker service is not running. Please start Docker and try again."
#     exit 1
# }
# Import-Certificate.ps1
# This script imports the Cosmos DB Emulator certificate into the Trusted Root Certification Authorities store.

# Set your container name or ID
$containerName = "cosmosdb-emulator"

# Export certificates from the container
docker cp "$($containerName):/scripts/certs/rootCA.crt" "$env:TEMP\rootCA.crt"

# Import the certificate into Trusted Root Certification Authorities
Import-Certificate -FilePath "$env:TEMP\rootCA.crt" -CertStoreLocation "Cert:\LocalMachine\Root"

# Clean up the temporary certificate file
Remove-Item -Path "$env:TEMP\rootCA.crt" -Force
# Verify the certificate import
# $cert = Get-ChildItem -Path Cert:\LocalMachine\Root | Where-Object { $_.FriendlyName -like "*Cosmos*" }
# if (-not $cert) {
#     Write-Host "❌ Cosmos DB Emulator certificate import failed."
#     exit 1
# } else {
#     Write-Host "✅ Cosmos DB Emulator certificate imported successfully."
# }