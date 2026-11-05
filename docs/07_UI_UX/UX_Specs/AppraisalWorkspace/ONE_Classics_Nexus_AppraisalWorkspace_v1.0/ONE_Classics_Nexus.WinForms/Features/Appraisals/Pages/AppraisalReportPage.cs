using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalReportPage : WorkspacePageBase
{
    private readonly Label _details;
    private Guid _appraisalId;
    private Guid? _reportId;

    public AppraisalReportPage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Report"));
        stack.Controls.Add(CreateMuted("Report generation, preview, version history, and export are delegated to the Reports module while retaining appraisal context."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 220;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label { Dock = DockStyle.Fill, Font = AppraisalWorkspaceTheme.BodyFont(10f), ForeColor = AppraisalWorkspaceTheme.TextPrimary };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        var actions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        var open = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Reports");
        var generate = AppraisalWorkspaceTheme.CreateSecondaryButton("Generate Report");
        open.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Report, _appraisalId, _reportId, "open");
        generate.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Report, _appraisalId, null, "generate");
        actions.Controls.Add(open);
        actions.Controls.Add(generate);
        stack.Controls.Add(actions);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Report;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _reportId = appraisal.LatestReport?.AppraisalReportId;
        var report = appraisal.LatestReport;

        _details.Text = report is null
            ? $"Workflow status: {Display(appraisal.ReportStatus)}{Environment.NewLine}{Environment.NewLine}No latest report summary was supplied by the API."
            : string.Join(Environment.NewLine, new[]
            {
                $"Report ID: {report.AppraisalReportId:D}",
                $"Type: {Display(report.ReportType)}",
                $"Version: {Display(report.ReportVersion)}",
                $"Status: {Display(report.ReportStatus)}",
                $"Template: {Display(report.TemplateName)}",
                $"Generated: {report.GeneratedOnUtc?.ToLocalTime().ToString("MMM d, yyyy h:mm tt") ?? "—"}"
            });
    }
}
