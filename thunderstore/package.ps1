param (
    [string]$Version = "1.0.0",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Resolve-Path "$scriptDir\.."
$srcDir = "$rootDir\src\ValheimGatherer"
$outDir = "$scriptDir\package"
$zipPath = "$rootDir\Gatherer-$Version.zip"

Write-Host "=== Packaging Gatherer v$Version for Thunderstore / r2modman ===" -ForegroundColor Cyan

# 1. Build
Write-Host "Building project ($Configuration)..."
$dotnetPath = if (Test-Path "C:\Users\amiar\.dotnet\dotnet.exe") { "C:\Users\amiar\.dotnet\dotnet.exe" } else { "dotnet" }
& $dotnetPath build "$srcDir\ValheimGatherer.csproj" -c $Configuration
if ($LASTEXITCODE -ne 0) {
    throw "Build failed with exit code $LASTEXITCODE"
}

# 2. Prepare package directory
if (Test-Path $outDir) {
    Remove-Item -Path $outDir -Recurse -Force
}
New-Item -ItemType Directory -Path $outDir -Force | Out-Null

$binDir = "$srcDir\bin\$Configuration"

# Copy binary files
Copy-Item "$binDir\ValheimGatherer.dll" "$outDir\" -Force
if (Test-Path "$binDir\ValheimGatherer.pdb") {
    Copy-Item "$binDir\ValheimGatherer.pdb" "$outDir\" -Force
}

# Copy package metadata
Copy-Item "$scriptDir\manifest.json" "$outDir\" -Force
Copy-Item "$scriptDir\icon.png" "$outDir\" -Force
Copy-Item "$scriptDir\README.md" "$outDir\" -Force

# 3. Create zip
if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Write-Host "Creating zip: $zipPath..."
Compress-Archive -Path "$outDir\*" -DestinationPath $zipPath -Force

Write-Host "Package created successfully: $zipPath" -ForegroundColor Green
Write-Host "Contents:"
[System.IO.Compression.ZipFile]::OpenRead($zipPath).Entries | ForEach-Object {
    " - $($_.FullName) ($([math]::Round($_.Length / 1KB, 1)) KB)"
}
