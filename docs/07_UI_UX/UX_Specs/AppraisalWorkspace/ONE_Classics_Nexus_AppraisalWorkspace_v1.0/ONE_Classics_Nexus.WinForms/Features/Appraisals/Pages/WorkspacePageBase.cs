using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public abstract class WorkspacePageBase : UserControl, IAppraisalWorkspacePage
{
    protected WorkspacePageBase()
    {
        Dock = DockStyle.Fill;
        BackColor = AppraisalWorkspaceTheme.Canvas;
        AutoScroll = true;
        Padding = new Padding(18);
    }

    public abstract AppraisalWorkspaceSection Section { get; }
    public Control View => this;
    public abstract void Bind(AppraisalWorkspaceDto appraisal);

    public event EventHandler<AppraisalWorkspaceNavigationEventArgs>? NavigationRequested;

    protected void RequestNavigation(
        AppraisalWorkspaceSection target,
        Guid appraisalId,
        Guid? relatedEntityId = null,
        string? action = null)
    {
        NavigationRequested?.Invoke(
            this,
            new AppraisalWorkspaceNavigationEventArgs(target, appraisalId, relatedEntityId, action));
    }

    protected static Label CreateTitle(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = AppraisalWorkspaceTheme.HeaderFont(16f),
        ForeColor = AppraisalWorkspaceTheme.Primary,
        Margin = new Padding(0, 0, 0, 12)
    };

    protected static Label CreateMuted(string text) => new()
    {
        Text = text,
        AutoSize = true,
        MaximumSize = new Size(900, 0),
        Font = AppraisalWorkspaceTheme.BodyFont(9.5f),
        ForeColor = AppraisalWorkspaceTheme.TextSecondary,
        Margin = new Padding(0, 0, 0, 8)
    };

    protected static string Display(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value;
}
