using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public interface IAppraisalWorkspacePage
{
    AppraisalWorkspaceSection Section { get; }
    Control View { get; }
    void Bind(AppraisalWorkspaceDto appraisal);
}
