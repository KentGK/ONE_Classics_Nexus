using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;
public sealed class AppraisalRequestNumberGenerator(IAutoNumberGenerator autoNumberGenerator) : IAppraisalRequestNumberGenerator
{
    public async Task<string> NextAsync(DateTime utcNow, CancellationToken ct)
    {
        return await autoNumberGenerator
            .NextAsync("appraisal_request", "RN", 6, ct)
            .ConfigureAwait(false);
    }
}
