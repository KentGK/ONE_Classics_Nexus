namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals;

public sealed class AppraisalWorkspaceNavigationEventArgs : EventArgs
{
    public AppraisalWorkspaceNavigationEventArgs(
        AppraisalWorkspaceSection target,
        Guid appraisalId,
        Guid? relatedEntityId = null,
        string? action = null)
    {
        Target = target;
        AppraisalId = appraisalId;
        RelatedEntityId = relatedEntityId;
        Action = action;
    }

    public AppraisalWorkspaceSection Target { get; }
    public Guid AppraisalId { get; }
    public Guid? RelatedEntityId { get; }
    public string? Action { get; }
}
