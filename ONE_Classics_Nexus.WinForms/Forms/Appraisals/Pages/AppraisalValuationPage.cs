using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Pages;

public sealed class AppraisalValuationPage : WorkspacePageBase
{
    private readonly Label _details;
    private readonly Button _openButton;
    private Guid _appraisalId;
    private Guid? _valuationRunId;

    public AppraisalValuationPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Valuation"));
        stack.Controls.Add(CreateMuted("Latest deterministic valuation summary and confidence context. Trace details remain owned by the Valuation workspace."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 240;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label { Dock = DockStyle.Fill, Font = AppraisalWorkspaceTheme.BodyFont(10f), ForeColor = AppraisalWorkspaceTheme.TextPrimary };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        var actions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        _openButton = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Valuation");
        var run = AppraisalWorkspaceTheme.CreateSecondaryButton("Run Valuation");
        _openButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Valuation, _appraisalId, _valuationRunId, "open");
        run.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Valuation, _appraisalId, null, "run");
        actions.Controls.Add(_openButton);
        actions.Controls.Add(run);
        stack.Controls.Add(actions);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Valuation;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _valuationRunId = appraisal.LatestValuation?.ValuationRunId;
        _openButton.Enabled = _valuationRunId.HasValue && _valuationRunId != Guid.Empty;

        var valuation = appraisal.LatestValuation;
        _details.Text = valuation is null
            ? $"Workflow status: {Display(appraisal.ValuationStatus)}{Environment.NewLine}{Environment.NewLine}No latest valuation summary was supplied by the API."
            : string.Join(Environment.NewLine, new[]
            {
                $"Run ID: {valuation.ValuationRunId:D}",
                $"Status: {Display(valuation.ValuationStatus ?? valuation.Status)}",
                $"Engine: {Display(valuation.EngineVersion)}",
                $"Condition Index: {valuation.ConditionIndex?.ToString("0.00") ?? "—"}",
                $"Value Range: {Money(valuation.ValueLow)}  –  {Money(valuation.ValueHigh)}",
                $"Final Value: {Money(valuation.FinalValue)}",
                $"Confidence: {Percent(valuation.ConfidenceScore)} {Display(valuation.ConfidenceBand)}",
                $"Manual Review: {(valuation.ManualReviewRequired ? "Required" : "No")}"
            });
    }

    private static string Money(decimal? value) => value?.ToString("C0") ?? "—";
    private static string Percent(decimal? value) => value.HasValue ? value.Value.ToString("P0") : "—";
}
