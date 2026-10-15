param (
    [string]$Version = "1.3.0",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Resolve-Path "$scriptDir\.."
$srcDir = "$rootDir\src\ValheimGatherer"
$outDir = "$scriptDir\package"
$zipPath = "$rootDir\Gatherer-$Version.zip"
$thunderstoreZipPath = "$rootDir\Thunderstore-Gatherer-$Version.zip"
$readmePath = "$rootDir\README.md"
$readme = Get-Content $readmePath -Raw
$manualMatch = [regex]::Match($readme, '(?s)## Local-import manual\s+(.*?)\r?\n---')
if (!$manualMatch.Success) {
    throw "README.md is missing the Local-import manual section."
}
$manifest = Get-Content "$scriptDir\manifest.json" -Raw | ConvertFrom-Json
if ($manifest.version_number -ne $Version) {
    throw "Package version must match manifest.json."
}

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
Copy-Item $readmePath "$scriptDir\README.md" -Force
Copy-Item $readmePath "$outDir\" -Force

# 3. Create zip
if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Write-Host "Creating zip: $zipPath..."
# Preserve the short description for Thunderstore's metadata limits.
Compress-Archive -Path "$outDir\*" -DestinationPath $thunderstoreZipPath -Force

# Local imports display manifest.description, not the bundled Markdown README.
$manifest.description = ($manualMatch.Groups[1].Value.Trim() -replace '\s+', ' ')
$manifest | Add-Member -NotePropertyName author -NotePropertyValue "Imbrus_Chrapmajster"
$manifest | ConvertTo-Json -Depth 10 | Set-Content "$outDir\manifest.json" -Encoding UTF8
Compress-Archive -Path "$outDir\*" -DestinationPath $zipPath -Force

Write-Host "Package created successfully: $zipPath" -ForegroundColor Green
Write-Host "Thunderstore upload package: $thunderstoreZipPath"
Write-Host "Contents:"
$archive = [System.IO.Compression.ZipFile]::OpenRead($zipPath)
try {
    $archive.Entries | ForEach-Object {
        " - $($_.FullName) ($([math]::Round($_.Length / 1KB, 1)) KB)"
    }
} finally {
    $archive.Dispose()
}
