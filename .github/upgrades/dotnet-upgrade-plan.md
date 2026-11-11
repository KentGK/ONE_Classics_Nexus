# .NET 8 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 8 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8 upgrade.
3. Upgrade ONE_Classics_Nexus.Domain/ONE_Classics_Nexus.Domain.csproj
4. Upgrade ONE_Classics_Nexus.Application/ONE_Classics_Nexus.Application.csproj
5. Upgrade ONE_Classics_Nexus.Infrastructure/ONE_Classics_Nexus.Infrastructure.csproj
6. Upgrade ONE_Classics_Nexus.Tests/ONE_Classics_Nexus.Tests.csproj
7. Upgrade ONE_Classics_Nexus.WinForms/ONE_Classics_Nexus.WinForms.csproj
8. Upgrade ONE_Classics_Nexus.API/ONE_Classics_Nexus.API.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

No projects are excluded.

### Project upgrade details

#### ONE_Classics_Nexus.Domain/ONE_Classics_Nexus.Domain.csproj modifications

Project properties changes:
- Convert the existing non-SDK-style project to SDK-style.
- Target framework should be changed from `.NETFramework,Version=v4.8` to `net8.0`.

#### ONE_Classics_Nexus.Application/ONE_Classics_Nexus.Application.csproj modifications

Project properties changes:
- Convert the existing non-SDK-style project to SDK-style.
- Target framework should be changed from `.NETFramework,Version=v4.8` to `net8.0`.

#### ONE_Classics_Nexus.Infrastructure/ONE_Classics_Nexus.Infrastructure.csproj modifications

Other changes:
- Already targets .NET 8; verify compatibility with upgraded dependencies.

#### ONE_Classics_Nexus.Tests/ONE_Classics_Nexus.Tests.csproj modifications

Other changes:
- Already targets .NET 8; verify compatibility with upgraded dependencies.

#### ONE_Classics_Nexus.WinForms/ONE_Classics_Nexus.WinForms.csproj modifications

Other changes:
- Already targets .NET 8; verify compatibility with upgraded dependencies.

#### ONE_Classics_Nexus.API/ONE_Classics_Nexus.API.csproj modifications

Other changes:
- Already targets .NET 8; verify compatibility with upgraded dependencies.
