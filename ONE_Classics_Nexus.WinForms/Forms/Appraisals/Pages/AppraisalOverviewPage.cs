using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Pages;

[System.ComponentModel.DesignerCategory("Code")]
public sealed class AppraisalOverviewPage : WorkspacePageBase
{
    private readonly TableLayoutPanel _cards;
    private readonly Label _lifecycle;
    private readonly Label _vehicle;
    private readonly Label _inspection;
    private readonly Label _valuation;
    private readonly Label _market;
    private readonly Label _report;

    public AppraisalOverviewPage()
    {
        var stack = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Appraisal Overview"));
        stack.Controls.Add(CreateMuted("At-a-glance lifecycle context for the subject vehicle, inspection, market evidence, valuation, and report."));

        _cards = new TableLayoutPanel
        {
            ColumnCount = 3,
            RowCount = 2,
            Width = 900,
            Height = 330,
            Margin = new Padding(0, 12, 0, 0)
        };
        for (var i = 0; i < 3; i++)
            _cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
        for (var i = 0; i < 2; i++)
            _cards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

        _lifecycle = AddCard(0, 0, "Lifecycle");
        _vehicle = AddCard(1, 0, "Subject Vehicle");
        _inspection = AddCard(2, 0, "Inspection");
        _market = AddCard(0, 1, "Comparables");
        _valuation = AddCard(1, 1, "Valuation");
        _report = AddCard(2, 1, "Report");
        stack.Controls.Add(_cards);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Overview;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _lifecycle.Text = $"Status: {Display(appraisal.Status)}{Environment.NewLine}Intake: {Display(appraisal.IntakeStatus)}";
        _vehicle.Text = appraisal.Vehicle is null
            ? $"Vehicle ID: {appraisal.VehicleId:D}"
            : $"{appraisal.Vehicle.DisplayName}{Environment.NewLine}VIN: {Display(appraisal.Vehicle.Vin)}";
        _inspection.Text = appraisal.LatestInspection is null
            ? $"Status: {Display(appraisal.InspectionStatus)}{Environment.NewLine}No inspection summary loaded"
            : $"Status: {Display(appraisal.LatestInspection.Status)}{Environment.NewLine}Systems: {appraisal.LatestInspection.CompletedSystemCount}/{appraisal.LatestInspection.TotalSystemCount}";
        _market.Text = $"Linked comparables: {appraisal.ComparableCount}";
        _valuation.Text = appraisal.LatestValuation is null
            ? $"Status: {Display(appraisal.ValuationStatus)}{Environment.NewLine}No valuation summary loaded"
            : $"Value: {Money(appraisal.LatestValuation.FinalValue)}{Environment.NewLine}Confidence: {Percent(appraisal.LatestValuation.ConfidenceScore)}";
        _report.Text = appraisal.LatestReport is null
            ? $"Status: {Display(appraisal.ReportStatus)}{Environment.NewLine}No report summary loaded"
            : $"Status: {Display(appraisal.LatestReport.ReportStatus)}{Environment.NewLine}Version: {Display(appraisal.LatestReport.ReportVersion)}";
    }

    private Label AddCard(int column, int row, string title)
    {
        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Dock = DockStyle.Fill;
        card.Margin = new Padding(6);

        var titleLabel = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 30,
            Font = AppraisalWorkspaceTheme.SectionFont(10.5f),
            ForeColor = AppraisalWorkspaceTheme.Primary
        };
        var value = new Label
        {
            Dock = DockStyle.Fill,
            Font = AppraisalWorkspaceTheme.BodyFont(9.5f),
            ForeColor = AppraisalWorkspaceTheme.TextPrimary,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };
        card.Controls.Add(value);
        card.Controls.Add(titleLabel);
        _cards.Controls.Add(card, column, row);
        return value;
    }

    private static string Money(decimal? value) => value?.ToString("C0") ?? "—";
    private static string Percent(decimal? value) => value.HasValue ? $"{value.Value:P0}" : "—";
}

#if DEBUG
public partial class YourForm : UserControl // or Form
{
    public YourForm()
    {
        // If you do not have a designer file, comment out or remove this line:
        // InitializeComponent();
    }
}
#else
public partial class YourForm : WorkspacePageBase
{
    public YourForm()
    {
        // If you do not have a designer file, comment out or remove this line:
        // InitializeComponent();
    }
}
#endif
