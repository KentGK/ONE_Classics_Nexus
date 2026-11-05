using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Pages;

public interface IAppraisalWorkspacePage
{
    AppraisalWorkspaceSection Section { get; }
    Control View { get; }
    void Bind(AppraisalWorkspaceDto appraisal);
}
