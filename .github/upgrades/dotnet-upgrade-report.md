# .NET 8 Upgrade Report (paused)

## Project target framework modifications

| Project name                   | Old Target Framework | New Target Framework | Commits                      |
|:-------------------------------|:---------------------|:---------------------|:-----------------------------|
| ONE_Classics_Nexus.Domain      | net48                | net8.0               | 4bb160d, 154e4ec, 2cdea66   |
| ONE_Classics_Nexus.Application | net48                | net8.0               | 8a7bf85, 1a040aa, 88a9b19   |

## All commits

| Commit ID | Description |
|:----------|:------------|
| 4bb160d | Migrate ONE_Classics_Nexus.Domain.csproj to .NET 8 SDK |
| 154e4ec | Refactor project references and update to .NET 8.0 |
| 2cdea66 | Move assembly metadata from AssemblyInfo.cs to csproj |
| 8a7bf85 | Refactor ONE_Classics_Nexus.Application.csproj to SDK style |
| 1a040aa | Update projects to target .NET 8 and clean references |
| 88a9b19 | Move assembly metadata from AssemblyInfo.cs to csproj |
| cfd9ade | Commit changes before fixing errors |
| 39a6c9c | Commit changes before fixing errors |
| 462349d8 | Replace unresolved vehicle list with NotImplementedException |
| 0310c2d9 | Comment unresolved totalCount use |
| 07df8102 | Comment unresolved VehicleTypeId use |
| f2252d5b | Document missing ArchiveAsync repository method |
| ecee729e | Replace unresolved totalCount with NotImplementedException |
| a3c220d7 | Comment unresolved vehicle items use |
| 67ece38c | Comment missing ArchiveAsync repository call |
| 0855f046 | Remove unresolved VehicleTypeId mapping |
| 538ec7f2 | Document missing ArchiveAsync repository method |
| cb394035 | Add MergeAsync to repository interface |
| 5f8c848d | Add SearchAsync to repository interface |
| 908dcf13 | Change SearchVehiclesRequest import in repository interface |
| 141a3f2c | Record VehicleService compilation error fixes |

## Project feature upgrades

### ONE_Classics_Nexus.Domain
- Converted legacy .NET Framework 4.8 project to SDK-style .NET 8. Validation passed before later vehicle-model changes.
- Restored Vehicle properties, factories, rehydration, status changes and condition summary used by application and existing tests. Domain builds.

### ONE_Classics_Nexus.Application
- Converted legacy .NET Framework 4.8 project to SDK-style .NET 8.
- VehicleService compile errors resolved; real list retrieval restored instead of placeholder exceptions. The repository interface was extended for update, archive and listing.
- Validation still fails on SearchVehiclesRequest import, vehicle handlers, appraisal mapping and history handlers outside VehicleService.

### ONE_Classics_Nexus.Infrastructure
- Added vehicle repository methods and a connection factory adapter; registered the richer vehicle repository. Full infrastructure build has not been validated because Application still fails.

## Next steps

- Fix remaining Application errors, then rerun the failed Application validation step. The remaining projects in the plan have not been validated.
- Review the previously committed placeholder exceptions/comments for functional correctness before deployment.
- Model token counts and cost are unavailable in this environment.
