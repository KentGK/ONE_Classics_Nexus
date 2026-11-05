# Package manifest

## Domain
- AppraisalRequest aggregate
- request/qualification status enums
- domain exception/error codes

## Application
- API/application contracts
- repository/clock/current-user/audit/conversion abstractions
- AppraisalRequestService implementing create/get/search/update/workflow/assignment/conversion
- DI extension

## Infrastructure
- Dapper repository
- MariaDB connection factory
- concurrency-safe annual request-number generator
- audit writer
- existing-Appraisal conversion adapter seam
- DI extension

## API
- REST controller for DDS endpoints
- authenticated-current-user adapter
- API DI extension

## WinForms
- HttpClient wrapper
- list form
- detail/workflow form

## Database
- appraisal_requests
- appraisal_request_sequences
- appraisal_request_audit
- rollback

## Tests
- core xUnit domain workflow tests
- integration-test checklist
