using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Controls;

public partial class AppraisalSummaryControl : UserControl
{
    public AppraisalSummaryControl()
    {
        InitializeComponent();
        BackColor = AppraisalWorkspaceTheme.Surface;

        foreach (Control control in grid.Controls)
        {
            if (control is not Label label)
                continue;

            var position = grid.GetPositionFromControl(label);
            var isCaption = position.Row == 0;
            label.Font = isCaption
                ? AppraisalWorkspaceTheme.BodyFont(8f)
                : AppraisalWorkspaceTheme.BodyFont(10f);
            label.ForeColor = isCaption
                ? AppraisalWorkspaceTheme.TextSecondary
                : AppraisalWorkspaceTheme.TextPrimary;
        }
    }

    public void Bind(AppraisalWorkspaceDto appraisal)
    {
        ArgumentNullException.ThrowIfNull(appraisal);

        lblTypeValue.Text = EmptyAsDash(appraisal.AppraisalType);
        lblEffectiveValue.Text = appraisal.EffectiveDateUtc?.ToLocalTime().ToString("MMM d, yyyy") ?? "—";
        lblAppraiserValue.Text = appraisal.AssignedAppraiser?.DisplayText
            ?? "Unassigned";
        lblReviewerValue.Text = appraisal.AssignedReviewer?.DisplayText
            ?? "Unassigned";
        lblUpdatedValue.Text = appraisal.UpdatedAtUtc == default
            ? "—"
            : appraisal.UpdatedAtUtc.ToLocalTime().ToString("MMM d, yyyy h:mm tt");
    }

    private static string EmptyAsDash(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value;
}
