<#
.SYNOPSIS
  Create a source archive, publish binaries and tag the repository for a release.

.DESCRIPTION
  Usage examples:
    # dry run: just create tag and source zip, do not push or create GitHub release
    .\scripts\release.ps1 -Version "v1.0.0"

    # commit uncommitted changes automatically, push and create GitHub release (requires gh)
    .\scripts\release.ps1 -Version "1.0.0" -AutoCommit -Push -CreateGithubRelease

.PARAMETER Version
  Release version (required). If it doesn't start with 'v' the script will prefix with 'v'.

.PARAMETER AutoCommit
  If set, script will `git add -A` and commit uncommitted changes with a release message.

.PARAMETER Push
  If set, pushes current branch and the created tag to 'origin'.

.PARAMETER IncludeBinaries
  If set (default), publishes non-test csproj projects and includes binaries in the archive.

.PARAMETER Runtime
  Optional dotnet runtime identifier (e.g. win-x64) to pass to `dotnet publish`. If empty, framework-dependent publish is used.

.PARAMETER CreateGithubRelease
  If set, uses `gh` CLI to create a GitHub Release and attach artifacts (requires authenticated gh).

#>

param(
    [Parameter(Mandatory=$true)][string]$Version,
    [switch]$AutoCommit,
    [switch]$Push,
    [switch]$IncludeBinaries = $true,
    [string]$Runtime = "",
    [switch]$CreateGithubRelease
)

function Write-Info ($msg) { Write-Host "[INFO] $msg" -ForegroundColor Cyan }
function Write-ErrorAndExit ($msg) { Write-Host "[ERROR] $msg" -ForegroundColor Red; exit 1 }

# Normalize version/tag
if (-not $Version.StartsWith("v")) { $Tag = "v$Version" } else { $Tag = $Version }

# Ensure git available
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-ErrorAndExit "git is not available in PATH."
}

# Ensure dotnet available
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-ErrorAndExit "dotnet CLI is not available in PATH."
}

$RepoRoot = (git rev-parse --show-toplevel) 2>$null
if (-not $RepoRoot) { Write-ErrorAndExit "Not inside a git repository." }
Set-Location $RepoRoot

$ArtifactsDir = Join-Path $RepoRoot "artifacts"
$PublishDir = Join-Path $ArtifactsDir "publish"
New-Item -ItemType Directory -Force -Path $PublishDir | Out-Null

# Check working tree status
$status = git status --porcelain
if ($status) {
    if ($AutoCommit) {
        Write-Info "Uncommitted changes detected; auto-committing."
        git add -A
        git commit -m "chore(release): prepare $Tag" || Write-ErrorAndExit "git commit failed."
    } else {
        Write-ErrorAndExit "Uncommitted changes detected. Commit them or rerun with -AutoCommit."
    }
} else {
    Write-Info "Working tree clean."
}

# Ensure tag does not already exist
if (git tag -l $Tag) {
    Write-ErrorAndExit "Tag '$Tag' already exists. Aborting."
}

# Create annotated tag
Write-Info "Creating annotated tag $Tag"
git tag -a $Tag -m "Release $Tag" || Write-ErrorAndExit "Failed to create git tag."

# Optionally push branch and tag
if ($Push) {
    Write-Info "Pushing current branch and tag to origin"
    git push origin HEAD || Write-ErrorAndExit "Failed to push branch."
    git push origin $Tag || Write-ErrorAndExit "Failed to push tag."
}

# Create source archive from tag
$RepoName = Split-Path $RepoRoot -Leaf
$SourceZip = Join-Path $ArtifactsDir "$RepoName-$Tag-source.zip"
Write-Info "Creating source archive $SourceZip from tag $Tag"
git archive --format=zip --prefix="$RepoName-$Tag/" -o $SourceZip $Tag || Write-ErrorAndExit "git archive failed."

# Publish binaries (exclude test projects)
$BinaryZip = $null
if ($IncludeBinaries) {
    Write-Info "Finding non-test projects to publish..."
    $projects = git ls-files '*.csproj' | Where-Object { $_ -notmatch 'Test' -and $_ -notmatch 'tests' }
    if (-not $projects) {
        Write-Info "No non-test projects found to publish."
    } else {
        foreach ($proj in $projects) {
            $projPath = Join-Path $RepoRoot $proj
            $projName = [System.IO.Path]::GetFileNameWithoutExtension($proj)
            $outDir = Join-Path $PublishDir $projName
            New-Item -ItemType Directory -Force -Path $outDir | Out-Null

            $publishCmd = @("dotnet", "publish", $projPath, "-c", "Release", "-o", $outDir)
            if ($Runtime) { $publishCmd += ("-r", $Runtime, "--self-contained", "false") }

            Write-Info "Publishing $projName -> $outDir"
            & dotnet publish $projPath -c Release -o $outDir @(if ($Runtime) { @("-r",$Runtime,"--self-contained","false") } ) 2>&1 | Write-Host
            if ($LASTEXITCODE -ne 0) { Write-ErrorAndExit "dotnet publish failed for $projName" }
        }

        # Zip published artifacts
        $BinaryZip = Join-Path $ArtifactsDir "$RepoName-$Tag-binaries.zip"
        if (Test-Path $BinaryZip) { Remove-Item $BinaryZip -Force }
        Write-Info "Creating binaries archive $BinaryZip"
        Compress-Archive -Path (Join-Path $PublishDir '*') -DestinationPath $BinaryZip -Force
    }
}

# Generate checksums
Write-Info "Generating checksums"
$checksums = @()
if (Test-Path $SourceZip) {
    $h = Get-FileHash $SourceZip -Algorithm SHA256
    $checksums += "$($h.Hash)  $([System.IO.Path]::GetFileName($SourceZip))"
}
if ($BinaryZip -and (Test-Path $BinaryZip)) {
    $h = Get-FileHash $BinaryZip -Algorithm SHA256
    $checksums += "$($h.Hash)  $([System.IO.Path]::GetFileName($BinaryZip))"
}
$ChecksumFile = Join-Path $ArtifactsDir "$RepoName-$Tag-checksums.txt"
$checksums | Out-File -FilePath $ChecksumFile -Encoding utf8
Write-Info "Checksums written to $ChecksumFile"

# Optionally create GitHub release with gh CLI
if ($CreateGithubRelease) {
    if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
        Write-ErrorAndExit "gh CLI not found. Install GitHub CLI or omit -CreateGithubRelease."
    }

    Write-Info "Creating GitHub release $Tag (gh CLI)"
    $assets = @($SourceZip)
    if ($BinaryZip) { $assets += $BinaryZip }
    $assetArgs = $assets | ForEach-Object { "--attach `"$($_)`"" } | Out-String
    # Use gh release create with explicit assets
    & gh release create $Tag $assets -t $Tag -n "Release $Tag" 2>&1 | Write-Host
    if ($LASTEXITCODE -ne 0) { Write-ErrorAndExit "gh release create failed." }
}

Write-Info "Release artifacts written to: $ArtifactsDir"
Write-Info "Done. Tag: $Tag"<#
.SYNOPSIS
  Create a source archive, publish binaries and tag the repository for a release.

.DESCRIPTION
  Usage examples:
    # dry run: just create tag and source zip, do not push or create GitHub release
    .\scripts\release.ps1 -Version "v1.0.0"

    # commit uncommitted changes automatically, push and create GitHub release (requires gh)
    .\scripts\release.ps1 -Version "1.0.0" -AutoCommit -Push -CreateGithubRelease

.PARAMETER Version
  Release version (required). If it doesn't start with 'v' the script will prefix with 'v'.

.PARAMETER AutoCommit
  If set, script will `git add -A` and commit uncommitted changes with a release message.

.PARAMETER Push
  If set, pushes current branch and the created tag to 'origin'.

.PARAMETER IncludeBinaries
  If set (default), publishes non-test csproj projects and includes binaries in the archive.

.PARAMETER Runtime
  Optional dotnet runtime identifier (e.g. win-x64) to pass to `dotnet publish`. If empty, framework-dependent publish is used.

.PARAMETER CreateGithubRelease
  If set, uses `gh` CLI to create a GitHub Release and attach artifacts (requires authenticated gh).

#>

param(
    [Parameter(Mandatory=$true)][string]$Version,
    [switch]$AutoCommit,
    [switch]$Push,
    [switch]$IncludeBinaries = $true,
    [string]$Runtime = "",
    [switch]$CreateGithubRelease
)

function Write-Info ($msg) { Write-Host "[INFO] $msg" -ForegroundColor Cyan }
function Write-ErrorAndExit ($msg) { Write-Host "[ERROR] $msg" -ForegroundColor Red; exit 1 }

# Normalize version/tag
if (-not $Version.StartsWith("v")) { $Tag = "v$Version" } else { $Tag = $Version }

# Ensure git available
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-ErrorAndExit "git is not available in PATH."
}

# Ensure dotnet available
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-ErrorAndExit "dotnet CLI is not available in PATH."
}

$RepoRoot = (git rev-parse --show-toplevel) 2>$null
if (-not $RepoRoot) { Write-ErrorAndExit "Not inside a git repository." }
Set-Location $RepoRoot

$ArtifactsDir = Join-Path $RepoRoot "artifacts"
$PublishDir = Join-Path $ArtifactsDir "publish"
New-Item -ItemType Directory -Force -Path $PublishDir | Out-Null

# Check working tree status
$status = git status --porcelain
if ($status) {
    if ($AutoCommit) {
        Write-Info "Uncommitted changes detected; auto-committing."
        git add -A
        git commit -m "chore(release): prepare $Tag" || Write-ErrorAndExit "git commit failed."
    } else {
        Write-ErrorAndExit "Uncommitted changes detected. Commit them or rerun with -AutoCommit."
    }
} else {
    Write-Info "Working tree clean."
}

# Ensure tag does not already exist
if (git tag -l $Tag) {
    Write-ErrorAndExit "Tag '$Tag' already exists. Aborting."
}

# Create annotated tag
Write-Info "Creating annotated tag $Tag"
git tag -a $Tag -m "Release $Tag" || Write-ErrorAndExit "Failed to create git tag."

# Optionally push branch and tag
if ($Push) {
    Write-Info "Pushing current branch and tag to origin"
    git push origin HEAD || Write-ErrorAndExit "Failed to push branch."
    git push origin $Tag || Write-ErrorAndExit "Failed to push tag."
}

# Create source archive from tag
$RepoName = Split-Path $RepoRoot -Leaf
$SourceZip = Join-Path $ArtifactsDir "$RepoName-$Tag-source.zip"
Write-Info "Creating source archive $SourceZip from tag $Tag"
git archive --format=zip --prefix="$RepoName-$Tag/" -o $SourceZip $Tag || Write-ErrorAndExit "git archive failed."

# Publish binaries (exclude test projects)
$BinaryZip = $null
if ($IncludeBinaries) {
    Write-Info "Finding non-test projects to publish..."
    $projects = git ls-files '*.csproj' | Where-Object { $_ -notmatch 'Test' -and $_ -notmatch 'tests' }
    if (-not $projects) {
        Write-Info "No non-test projects found to publish."
    } else {
        foreach ($proj in $projects) {
            $projPath = Join-Path $RepoRoot $proj
            $projName = [System.IO.Path]::GetFileNameWithoutExtension($proj)
            $outDir = Join-Path $PublishDir $projName
            New-Item -ItemType Directory -Force -Path $outDir | Out-Null

            $publishCmd = @("dotnet", "publish", $projPath, "-c", "Release", "-o", $outDir)
            if ($Runtime) { $publishCmd += ("-r", $Runtime, "--self-contained", "false") }

            Write-Info "Publishing $projName -> $outDir"
            & dotnet publish $projPath -c Release -o $outDir @(if ($Runtime) { @("-r",$Runtime,"--self-contained","false") } ) 2>&1 | Write-Host
            if ($LASTEXITCODE -ne 0) { Write-ErrorAndExit "dotnet publish failed for $projName" }
        }

        # Zip published artifacts
        $BinaryZip = Join-Path $ArtifactsDir "$RepoName-$Tag-binaries.zip"
        if (Test-Path $BinaryZip) { Remove-Item $BinaryZip -Force }
        Write-Info "Creating binaries archive $BinaryZip"
        Compress-Archive -Path (Join-Path $PublishDir '*') -DestinationPath $BinaryZip -Force
    }
}

# Generate checksums
Write-Info "Generating checksums"
$checksums = @()
if (Test-Path $SourceZip) {
    $h = Get-FileHash $SourceZip -Algorithm SHA256
    $checksums += "$($h.Hash)  $([System.IO.Path]::GetFileName($SourceZip))"
}
if ($BinaryZip -and (Test-Path $BinaryZip)) {
    $h = Get-FileHash $BinaryZip -Algorithm SHA256
    $checksums += "$($h.Hash)  $([System.IO.Path]::GetFileName($BinaryZip))"
}
$ChecksumFile = Join-Path $ArtifactsDir "$RepoName-$Tag-checksums.txt"
$checksums | Out-File -FilePath $ChecksumFile -Encoding utf8
Write-Info "Checksums written to $ChecksumFile"

# Optionally create GitHub release with gh CLI
if ($CreateGithubRelease) {
    if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
        Write-ErrorAndExit "gh CLI not found. Install GitHub CLI or omit -CreateGithubRelease."
    }

    Write-Info "Creating GitHub release $Tag (gh CLI)"
    $assets = @($SourceZip)
    if ($BinaryZip) { $assets += $BinaryZip }
    $assetArgs = $assets | ForEach-Object { "--attach `"$($_)`"" } | Out-String
    # Use gh release create with explicit assets
    & gh release create $Tag $assets -t $Tag -n "Release $Tag" 2>&1 | Write-Host
    if ($LASTEXITCODE -ne 0) { Write-ErrorAndExit "gh release create failed." }
}

Write-Info "Release artifacts written to: $ArtifactsDir"
Write-Info "Done. Tag: $Tag"