using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;

/// <summary>Adapter point to the existing Appraisal creation use case. Keep proposed intake values provisional.</summary>
public sealed class ExistingAppraisalConversionGateway : IAppraisalRequestConversionGateway
{
    public Task<CreatedAppraisal> CreateAppraisalFromRequestAsync(AppraisalSeed seed, CancellationToken ct)
    {
        throw new NotImplementedException(
            "Wire this adapter to the existing CreateAppraisalHandler/Appraisal factory. " +
            "Do not mark intended use, value definition, effective date, scope or AssignmentReady as approved merely because the request was converted.");
    }
}
