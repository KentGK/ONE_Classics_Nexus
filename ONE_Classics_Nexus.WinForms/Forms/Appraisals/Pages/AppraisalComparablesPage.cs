using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Pages;

public sealed class AppraisalComparablesPage : WorkspacePageBase
{
    private readonly Label _count;
    private Guid _appraisalId;

    public AppraisalComparablesPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Comparable Sales"));
        stack.Controls.Add(CreateMuted("Market evidence linked to this appraisal. Detailed selection, similarity, and adjustments remain in the Comparables workspace."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 170;
        card.Margin = new Padding(0, 12, 0, 10);
        _count = new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = AppraisalWorkspaceTheme.HeaderFont(24f),
            ForeColor = AppraisalWorkspaceTheme.Primary
        };
        card.Controls.Add(_count);
        stack.Controls.Add(card);

        var open = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Comparables");
        open.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Comparables, _appraisalId, null, "open");
        stack.Controls.Add(open);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Comparables;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _count.Text = $"{appraisal.ComparableCount}{Environment.NewLine}linked comparables";
    }
}
