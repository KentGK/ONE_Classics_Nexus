using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Pages;

public sealed class AppraisalInspectionPage : WorkspacePageBase
{
    private readonly Label _details;
    private readonly Button _openButton;
    private readonly Button _startButton;
    private Guid _appraisalId;
    private Guid? _inspectionId;

    public AppraisalInspectionPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Inspection"));
        stack.Controls.Add(CreateMuted("Inspection is the evidence source for the locked 12-system condition model."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 220;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label { Dock = DockStyle.Fill, Font = AppraisalWorkspaceTheme.BodyFont(10f), ForeColor = AppraisalWorkspaceTheme.TextPrimary };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        var actions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        _openButton = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Inspection");
        _startButton = AppraisalWorkspaceTheme.CreateSecondaryButton("Start Inspection");
        _openButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Inspection, _appraisalId, _inspectionId, "open");
        _startButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Inspection, _appraisalId, null, "create");
        actions.Controls.Add(_openButton);
        actions.Controls.Add(_startButton);
        stack.Controls.Add(actions);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Inspection;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _inspectionId = appraisal.LatestInspection?.InspectionId;
        _openButton.Enabled = _inspectionId.HasValue && _inspectionId != Guid.Empty;

        var inspection = appraisal.LatestInspection;
        _details.Text = inspection is null
            ? $"Workflow status: {Display(appraisal.InspectionStatus)}{Environment.NewLine}{Environment.NewLine}No latest inspection summary was supplied by the API."
            : string.Join(Environment.NewLine, new[]
            {
                $"Inspection ID: {inspection.InspectionId:D}",
                $"Type: {Display(inspection.InspectionType)}",
                $"Status: {Display(inspection.Status)}",
                $"Location: {Display(inspection.InspectionLocation)}",
                $"Inspected: {inspection.InspectedOnUtc?.ToLocalTime().ToString("MMM d, yyyy h:mm tt") ?? "—"}",
                $"Systems Complete: {inspection.CompletedSystemCount}/{inspection.TotalSystemCount}",
                $"Condition Index: {inspection.ConditionIndex?.ToString("0.00") ?? "—"}",
                $"Red Flags: {inspection.RedFlagCount}"
            });
    }
}
