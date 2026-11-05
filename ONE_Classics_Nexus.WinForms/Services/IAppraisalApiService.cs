using ONE_Classics_Nexus.API.Contracts.Appraisals;

namespace ONE_Classics_Nexus.WinForms.Services;

public interface IAppraisalApiService
{
    Task<AppraisalResponse?> GetByIdAsync(Guid appraisalId, CancellationToken cancellationToken = default);
    Task<AppraisalResponse> CreateAsync(CreateAppraisalRequest request, CancellationToken cancellationToken = default);
    Task<AppraisalResponse> UpdateAsync(Guid appraisalId, UpdateAppraisalRequest request, CancellationToken cancellationToken = default);
}