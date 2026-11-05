using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

public sealed class AppraisalWorkflowControl : UserControl
{
    private readonly FlowLayoutPanel _flow;
    private readonly Dictionary<AppraisalWorkspaceSection, Button> _buttons = new();
    private AppraisalWorkspaceSection _selected = AppraisalWorkspaceSection.Overview;

    public AppraisalWorkflowControl()
    {
        BackColor = AppraisalWorkspaceTheme.Surface;
        Padding = new Padding(10, 14, 10, 10);

        _flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            BackColor = AppraisalWorkspaceTheme.Surface
        };
        Controls.Add(_flow);

        foreach (var section in Enum.GetValues<AppraisalWorkspaceSection>())
        {
            var button = CreateButton(section);
            _buttons.Add(section, button);
            _flow.Controls.Add(button);
        }

        SetSelected(_selected);
    }

    public event EventHandler<AppraisalWorkspaceSection>? SectionSelected;

    public void Bind(AppraisalWorkspaceDto appraisal)
    {
        ArgumentNullException.ThrowIfNull(appraisal);

        SetButtonText(AppraisalWorkspaceSection.Overview, "Overview", appraisal.Status);
        SetButtonText(AppraisalWorkspaceSection.Intake, "Intake", appraisal.IntakeStatus);
        SetButtonText(AppraisalWorkspaceSection.Vehicle, "Vehicle", appraisal.Vehicle is null ? null : "Linked");
        SetButtonText(AppraisalWorkspaceSection.Inspection, "Inspection", appraisal.InspectionStatus);
        SetButtonText(AppraisalWorkspaceSection.Comparables, "Comparables", $"{appraisal.ComparableCount} linked");
        SetButtonText(AppraisalWorkspaceSection.Valuation, "Valuation", appraisal.ValuationStatus);
        SetButtonText(AppraisalWorkspaceSection.Review, "Review", appraisal.LatestValuation?.ManualReviewRequired == true ? "Required" : null);
        SetButtonText(AppraisalWorkspaceSection.Report, "Report", appraisal.ReportStatus);
    }

    public void SetSelected(AppraisalWorkspaceSection section)
    {
        _selected = section;
        foreach (var pair in _buttons)
        {
            var selected = pair.Key == section;
            pair.Value.BackColor = selected ? AppraisalWorkspaceTheme.Primary : AppraisalWorkspaceTheme.Surface;
            pair.Value.ForeColor = selected ? Color.White : AppraisalWorkspaceTheme.TextPrimary;
            pair.Value.FlatAppearance.BorderColor = selected ? AppraisalWorkspaceTheme.Primary : AppraisalWorkspaceTheme.Border;
        }
    }

    private Button CreateButton(AppraisalWorkspaceSection section)
    {
        var button = new Button
        {
            Tag = section,
            Text = section.ToString(),
            Size = new Size(190, 52),
            Margin = new Padding(0, 0, 0, 8),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 2, 8, 2),
            FlatStyle = FlatStyle.Flat,
            Font = AppraisalWorkspaceTheme.BodyFont(9.5f),
            Cursor = Cursors.Hand,
            UseMnemonic = false
        };
        button.FlatAppearance.BorderColor = AppraisalWorkspaceTheme.Border;
        button.Click += (_, _) =>
        {
            SetSelected(section);
            SectionSelected?.Invoke(this, section);
        };
        return button;
    }

    private void SetButtonText(AppraisalWorkspaceSection section, string caption, string? status)
    {
        if (!_buttons.TryGetValue(section, out var button))
            return;

        button.Text = string.IsNullOrWhiteSpace(status)
            ? caption
            : $"{caption}{Environment.NewLine}{status}";
    }
}
