using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalReviewPage : WorkspacePageBase
{
    private readonly CheckedListBox _checklist;
    private readonly Label _message;

    public AppraisalReviewPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Review Readiness"));
        stack.Controls.Add(CreateMuted("Readiness is derived from loaded workflow context. It does not bypass API-owned business rules."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 300;
        card.Margin = new Padding(0, 12, 0, 10);

        _message = new Label
        {
            Dock = DockStyle.Top,
            Height = 42,
            Font = AppraisalWorkspaceTheme.SectionFont(10.5f),
            ForeColor = AppraisalWorkspaceTheme.Primary
        };
        _checklist = new CheckedListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            CheckOnClick = false,
            Font = AppraisalWorkspaceTheme.BodyFont(10f),
            BackColor = AppraisalWorkspaceTheme.Surface
        };
        card.Controls.Add(_checklist);
        card.Controls.Add(_message);
        stack.Controls.Add(card);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Review;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _checklist.Items.Clear();
        Add("Subject vehicle linked", appraisal.VehicleId != Guid.Empty);
        Add("Intake workflow populated", !string.IsNullOrWhiteSpace(appraisal.IntakeStatus));
        Add("Inspection available", appraisal.LatestInspection is not null);
        Add("Inspection 12-system completion", appraisal.LatestInspection?.CompletedSystemCount >= 12);
        Add("Comparable evidence available", appraisal.ComparableCount > 0);
        Add("Valuation run available", appraisal.LatestValuation is not null);
        Add("No manual review hold", appraisal.LatestValuation?.ManualReviewRequired != true);
        Add("Report workflow initialized", !string.IsNullOrWhiteSpace(appraisal.ReportStatus) || appraisal.LatestReport is not null);

        var complete = Enumerable.Range(0, _checklist.Items.Count).Count(index => _checklist.GetItemChecked(index));
        _message.Text = $"{complete}/{_checklist.Items.Count} readiness checks satisfied";
        _message.ForeColor = complete == _checklist.Items.Count
            ? AppraisalWorkspaceTheme.Success
            : AppraisalWorkspaceTheme.Warning;
    }

    private void Add(string text, bool isComplete)
    {
        var index = _checklist.Items.Add(text);
        _checklist.SetItemChecked(index, isComplete);
    }
}
