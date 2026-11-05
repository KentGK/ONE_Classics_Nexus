using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public interface IAppraisalWorkspaceService
{
    Task<AppraisalWorkspaceDto> GetAsync(Guid appraisalId, CancellationToken cancellationToken = default);

    Task<AppraisalWorkspaceDto> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalWorkspaceRequest request,
        CancellationToken cancellationToken = default);
}
