# Integration notes

## API Program.cs

```csharp
builder.Services.AddAppraisalRequestsInfrastructure(builder.Configuration);
builder.Services.AddAppraisalRequestsApplication();
```

If the project already has an exception middleware, keep it. `AppraisalRequestsController` returns request-specific ProblemDetails for expected application/domain errors.

## Existing appraisal creation adapter

Replace the `NotImplementedException` in:

`ONE_Classics_Nexus.Infrastructure/AppraisalRequests/ExistingAppraisalConversionGateway.cs`

with an adapter to the solution's current appraisal-creation use case. Preserve these rules:

1. Request must be Qualified or Accepted.
2. One request may create at most one appraisal.
3. Requested purpose/value/effective date are copied only as proposed inputs.
4. Conversion does not imply AssignmentReady.
5. Do not rewrite the originating request after conversion.

## Existing connection factory

The package includes `AppraisalRequestConnectionFactory` using `ConnectionStrings:Nexus`. If the solution already has `MariaDbConnectionFactory`/`MySqlConnectionFactory`, replace registration of `IAppraisalRequestConnectionFactory` with a small adapter to the existing factory.
