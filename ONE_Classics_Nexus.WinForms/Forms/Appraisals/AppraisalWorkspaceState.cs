using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals;

public sealed class AppraisalWorkspaceState
{
    public AppraisalWorkspaceDto? Appraisal { get; internal set; }
    public AppraisalWorkspaceSection ActiveSection { get; internal set; } = AppraisalWorkspaceSection.Overview;
    public bool IsBusy { get; internal set; }
    public bool HasUnsavedChanges { get; internal set; }
    public string? ErrorMessage { get; internal set; }
    public DateTimeOffset? LoadedAtUtc { get; internal set; }

    public bool IsLoaded => Appraisal is not null;
}
