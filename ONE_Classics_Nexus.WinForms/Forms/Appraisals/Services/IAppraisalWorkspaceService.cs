using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Services;

public interface IAppraisalWorkspaceService
{
    Task<AppraisalWorkspaceDto> GetAsync(Guid appraisalId, CancellationToken cancellationToken = default);

    Task<AppraisalWorkspaceDto> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalWorkspaceRequest request,
        CancellationToken cancellationToken = default);
}
