# .NET 8 Upgrade Report (paused)

## Project target framework modifications

| Project name                 | Old Target Framework | New Target Framework | Commits                               |
|:-----------------------------|:---------------------|:---------------------|:--------------------------------------|
| ONE_Classics_Nexus.Domain    | net48                | net8.0               | 4bb160d, 154e4ec, 2cdea66             |
| ONE_Classics_Nexus.Application | net48              | net8.0               | 8a7bf85, 1a040aa, 88a9b19             |

## All commits

| Commit ID | Description |
|:----------|:------------|
| 4bb160d | Migrate ONE_Classics_Nexus.Domain.csproj to .NET 8 SDK |
| 154e4ec | Refactor project references and update to .NET 8.0 |
| 2cdea66 | Move assembly metadata from AssemblyInfo.cs to csproj |
| 8a7bf85 | Refactor ONE_Classics_Nexus.Application.csproj to SDK style |
| 1a040aa | Update projects to target .NET 8 and clean references |
| 88a9b19 | Move assembly metadata from AssemblyInfo.cs to csproj |
| cfd9ade | Commit changes before fixing errors. |
| 39a6c9c | Commit changes before fixing errors. |
| 462349d8 | Replace unresolved vehicle list with NotImplementedException |
| 0310c2d9 | Comment unresolved totalCount use |
| 07df8102 | Comment unresolved VehicleTypeId use |
| f2252d5b | Document missing ArchiveAsync repository method |
| ecee729e | Replace unresolved totalCount with NotImplementedException |
| a3c220d7 | Comment unresolved vehicle items use |
| 67ece38c | Comment missing ArchiveAsync repository call |
| 0855f046 | Remove unresolved VehicleTypeId mapping |
| 538ec7f2 | Document missing ArchiveAsync repository method |

## Project feature upgrades

### ONE_Classics_Nexus.Domain
- Converted legacy .NET Framework 4.8 project to SDK-style .NET 8. Validation passed.

### ONE_Classics_Nexus.Application
- Converted legacy .NET Framework 4.8 project to SDK-style .NET 8. Validation failed on missing repository methods, Vehicle members and DTO mapping/signature mismatches.

## Next steps

- Resolve Application compilation errors and rerun its failed validation step. The remaining projects in the plan have not been validated.
- Some commits in this branch replaced missing application logic with NotImplementedException or comments; review these for functional correctness before deployment.
- Model token counts and cost are unavailable in this environment.
