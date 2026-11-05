param(
    [string]$srcSolution = "C:\Users\kentk\OneDrive\ONEClassics\ONE_Classics_Nexus_App\ONE_Classics_Nexus.sln",
    [string]$destDir     = "C:\Users\kentk\OneDrive\ONE_Classics_Nexus_App"
)

if (-not (Test-Path $srcSolution)) {
    Write-Error "Source solution not found: $srcSolution"
    exit 1
}

$destPath = Join-Path $destDir (Split-Path $srcSolution -Leaf)

# Move .sln to destination
Move-Item -Path $srcSolution -Destination $destPath -Force
Write-Output "Moved solution to $destPath"

# Replace known old path fragments inside solution and project files
$oldFragment1 = 'OneDrive\ONEClassics\ONE_Classics_Nexus_App'
$newFragment1 = 'OneDrive\ONE_Classics_Nexus_App'

Get-ChildItem -Path $destDir -Include *.sln,*.csproj,*.props,*.targets -Recurse | ForEach-Object {
    (Get-Content $_.FullName -Raw) -replace [regex]::Escape($oldFragment1), $newFragment1 | Set-Content $_.FullName
    Write-Output "Patched: $($_.FullName)"
}

Write-Output "Done. Run `dotnet clean` then `dotnet build` from the solution folder. Delete bin/obj if needed."