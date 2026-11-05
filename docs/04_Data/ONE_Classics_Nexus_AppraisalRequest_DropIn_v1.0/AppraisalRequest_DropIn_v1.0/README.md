# ONE Classics Nexus — Appraisal Request Drop-In Code Set v1.0

This package implements the Appraisal Request vertical slice described by `Appraisal_Request_DDS_v1.0` for the existing ONE_Classics_Nexus layered .NET 8 solution.

## Included

- Domain aggregate, statuses, invariants, workflow transitions, immutability after conversion.
- Application contracts, repository abstraction, CRUD/workflow/conversion services.
- MariaDB/Dapper repository and request-number generator.
- API controller, ProblemDetails mapping, DI registration.
- WinForms API client and lightweight list/detail forms.
- MariaDB migration and rollback script.
- xUnit domain tests and application-service test doubles.

## Important integration seam

The uploaded `.sln` exposes the project boundaries but not the current source files for the existing `Appraisal` creation pipeline, connection factory, current-user service, or exception middleware. To avoid hard-coding guesses, conversion calls `IAppraisalRequestConversionGateway`. Implement the supplied `ExistingAppraisalConversionGateway` adapter against the current `CreateAppraisalHandler`/Appraisal factory. The request is marked Converted only after that gateway succeeds.

For true single-database atomic conversion, the existing Appraisal creation path and `AppraisalRequestRepository` should share the same connection/transaction. The included gateway interface deliberately exposes this boundary.

## Deliberate schema completion

DDS §11 requires requester acknowledgement that submitted information may require verification, but DDS §23 and §28 omit a persisted field for it. This implementation adds:

`requester_acknowledged_verification BOOLEAN NOT NULL DEFAULT 0`

This is the only deliberate completion of a DDS omission.

## Install

Copy each top-level project folder into the matching existing project. Apply `database/001_appraisal_requests.sql`. Add the registration calls shown in `INTEGRATION.md`. Then implement/replace the body of `ExistingAppraisalConversionGateway` to call the current Appraisal creation pipeline.

## Namespace assumptions

Namespaces follow the project names already present in the solution:

- `ONE_Classics_Nexus.Domain`
- `ONE_Classics_Nexus.Application`
- `ONE_Classics_Nexus.Infrastructure`
- `ONE_Classics_Nexus.API`
- `ONE_Classics_Nexus.WinForms`
- `ONE_Classics_Nexus.Tests`
