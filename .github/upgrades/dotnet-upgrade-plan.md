# .NET 8 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed. The goal is a stable, rebuildable solution, not merely a target-framework change.

1. Validate that a .NET 8 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8 upgrade.
3. Establish a restore and build baseline for the entire solution, recording failures and environmental prerequisites without discarding existing work.
4. Upgrade ONE_Classics_Nexus.Domain/ONE_Classics_Nexus.Domain.csproj
5. Upgrade ONE_Classics_Nexus.Application/ONE_Classics_Nexus.Application.csproj
6. Upgrade ONE_Classics_Nexus.Infrastructure/ONE_Classics_Nexus.Infrastructure.csproj
7. Upgrade ONE_Classics_Nexus.Tests/ONE_Classics_Nexus.Tests.csproj
8. Upgrade ONE_Classics_Nexus.WinForms/ONE_Classics_Nexus.WinForms.csproj
9. Upgrade ONE_Classics_Nexus.API/ONE_Classics_Nexus.API.csproj
10. Run tests in ONE_Classics_Nexus.Tests/ONE_Classics_Nexus.Tests.csproj and resolve upgrade-related failures.
11. Verify a clean restore and rebuild of the entire solution, then document prerequisites and any remaining blockers.

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

No projects are excluded. Include all solution projects and their dependencies.

### Project upgrade details

All projects already use SDK-style project files. Preserve their current .NET 8 target frameworks and assess restore, compilation, dependencies, and project references. Resolve verified issues rather than changing package versions speculatively. The automated test discovery service returned no result, but the test project explicitly declares `IsTestProject` as `true`.

#### ONE_Classics_Nexus.Domain/ONE_Classics_Nexus.Domain.csproj modifications

Project properties changes:
- Retain `net8.0`.

Other changes:
- Verify restore and compilation; correct any confirmed upgrade-related issues.

#### ONE_Classics_Nexus.Application/ONE_Classics_Nexus.Application.csproj modifications

Project properties changes:
- Retain `net8.0`.

Other changes:
- Verify the Domain project reference, package restore and compilation; correct any confirmed upgrade-related issues.

#### ONE_Classics_Nexus.Infrastructure/ONE_Classics_Nexus.Infrastructure.csproj modifications

Project properties changes:
- Retain `net8.0`.

Other changes:
- Verify Application and Domain references, package restore and compilation; correct any confirmed upgrade-related issues.

#### ONE_Classics_Nexus.Tests/ONE_Classics_Nexus.Tests.csproj modifications

Project properties changes:
- Retain `net8.0`.

Other changes:
- Verify test infrastructure, references, package restore and compilation; correct any confirmed upgrade-related issues.

#### ONE_Classics_Nexus.WinForms/ONE_Classics_Nexus.WinForms.csproj modifications

Project properties changes:
- Retain `net8.0-windows` and Windows Forms support.

Other changes:
- Verify Windows-specific build prerequisites, package restore, Application reference and compilation; correct any confirmed upgrade-related issues.

#### ONE_Classics_Nexus.API/ONE_Classics_Nexus.API.csproj modifications

Project properties changes:
- Retain `net8.0`.

Other changes:
- Verify Application and Infrastructure references, package restore and compilation; correct any confirmed upgrade-related issues.
