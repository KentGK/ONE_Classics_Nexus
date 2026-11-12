# .NET 8 Upgrade Report — Application validated; solution pending

## Project target framework modifications

- `ONE_Classics_Nexus.Domain`: `net48` to SDK-style `net8.0` (4bb160d, 154e4ec, 2cdea66).
- `ONE_Classics_Nexus.Application`: `net48` to SDK-style `net8.0` (8a7bf85, 1a040aa, 88a9b19).

## All commits

- 4bb160d — Migrate ONE_Classics_Nexus.Domain.csproj to .NET 8 SDK.
- 154e4ec — Refactor project references and update to .NET 8.0.
- 2cdea66 — Move assembly metadata from AssemblyInfo.cs to csproj.
- 8a7bf85 — Refactor ONE_Classics_Nexus.Application.csproj to SDK style.
- 1a040aa — Update projects to target .NET 8 and clean references.
- 88a9b19 — Move assembly metadata from AssemblyInfo.cs to csproj.
- cfd9ade — Commit changes before fixing errors.
- 39a6c9c — Commit changes before fixing errors.
- 462349d8 — Replace unresolved vehicle list with NotImplementedException (historical).
- 0310c2d9 — Comment unresolved totalCount use (historical).
- 07df8102 — Comment unresolved VehicleTypeId use (historical).
- f2252d5b — Document missing ArchiveAsync repository method.
- ecee729e — Replace unresolved totalCount with NotImplementedException (historical).
- a3c220d7 — Comment unresolved vehicle items use (historical).
- 67ece38c — Comment missing ArchiveAsync repository call (historical).
- 0855f046 — Remove unresolved VehicleTypeId mapping (historical).
- 538ec7f2 — Document missing ArchiveAsync repository method.
- cb394035 — Add MergeAsync to repository interface.
- 5f8c848d — Add SearchAsync to repository interface.
- 908dcf13 — Change SearchVehiclesRequest import in repository interface.
- 141a3f2c — Record VehicleService compilation error fixes.
- 469612f1 — Fix appraisal/history errors; Application builds; four focused regression tests pass.

## Project changes

### ONE_Classics_Nexus.Domain
- Converted legacy project to SDK-style .NET 8.
- Previously restored Vehicle properties, factories, rehydration, status changes and condition summary.
- Added Appraisal metadata fields matching the existing detail DTO and SQL schema.
- Added Inspection.Status for persisted status in history events.

### ONE_Classics_Nexus.Application
- Converted legacy project to SDK-style .NET 8.
- Previously restored VehicleService list retrieval, vehicle mappings, SearchVehiclesRequest import, vehicle summary mapping, and ArchiveVehicleHandler's repository call.
- Added IAppraisalRepository.BrowseAsync and IInspectionRepository.GetByAppraisalIdAsync.
- Restored the VehicleUpdated history timestamp and conditional emission only after creation.
- Added ToDetailDto mapping using the actual appraisal metadata and vehicle DTO.
- All four appraisal/history errors are resolved; current Application error check returns no errors.
- Application build and upgrade validation succeeded. The Application upgrade step is now marked successful.

### ONE_Classics_Nexus.Infrastructure
- Previously added vehicle repository methods and a connection factory adapter; registered the richer vehicle repository.
- Registered appraisal repository now loads detail metadata and delegates BrowseAsync to the existing filtered/paged query implementation.
- Registered inspection repository now implements GetByAppraisalIdAsync and reads persisted status.
- Disambiguated Appraisal and Inspection entity types from repository namespaces in the edited files.
- Full Infrastructure validation is still pending; remaining solution build failures are not resolved by this Application-focused work.

## Verification

- Application command-line build: succeeded with zero errors and two existing warnings (Inspection.Create nullable argument and a duplicate request namespace import).
- Application post-upgrade validation: succeeded.
- Added AppraisalHistoryMappingTests.cs. All four focused tests passed using an isolated temporary test project referencing the actual Application project: full detail mapping, updated-event conditions, paged history traversal, event links and ordering.
- The normal test runner returned no project summaries and did not confirm any tests executed. The complete solution test suite has not passed.
- Full workspace build still fails on Infrastructure implementation/tuple-name mismatches and Tests dependencies/references, with dependent API build failures. No database integration tests were performed.

## Next steps

- Continue with Infrastructure validation and resolve its remaining build issues.
- Restore missing test dependencies/references and run the complete suite; validate the remaining API and WinForms projects.
- Review historical placeholder exceptions/comments for functional completeness before deployment.

## Usage metrics

Model input/output token counts and execution cost are unavailable in this environment.
