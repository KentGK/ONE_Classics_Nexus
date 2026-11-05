using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Pages;

public sealed class AppraisalVehiclePage : WorkspacePageBase
{
    private readonly Label _details;
    private readonly Button _openButton;
    private Guid _appraisalId;
    private Guid _vehicleId;

    public AppraisalVehiclePage()
    {
        var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        Controls.Add(stack);
        stack.Controls.Add(CreateTitle("Subject Vehicle"));
        stack.Controls.Add(CreateMuted("The appraisal points to one canonical vehicle record. Vehicle edits remain owned by the Vehicles module."));

        var card = AppraisalWorkspaceTheme.CreateCard();
        card.Width = 900;
        card.Height = 280;
        card.Margin = new Padding(0, 12, 0, 10);
        _details = new Label
        {
            Dock = DockStyle.Fill,
            Font = AppraisalWorkspaceTheme.BodyFont(10f),
            ForeColor = AppraisalWorkspaceTheme.TextPrimary,
            Padding = new Padding(4),
            AutoEllipsis = true
        };
        card.Controls.Add(_details);
        stack.Controls.Add(card);

        _openButton = AppraisalWorkspaceTheme.CreatePrimaryButton("Open Vehicle");
        _openButton.Click += (_, _) => RequestNavigation(AppraisalWorkspaceSection.Vehicle, _appraisalId, _vehicleId, "open");
        stack.Controls.Add(_openButton);
    }

    public override AppraisalWorkspaceSection Section => AppraisalWorkspaceSection.Vehicle;

    public override void Bind(AppraisalWorkspaceDto appraisal)
    {
        _appraisalId = appraisal.AppraisalId;
        _vehicleId = appraisal.VehicleId;
        var vehicle = appraisal.Vehicle;
        _openButton.Enabled = _vehicleId != Guid.Empty;

        _details.Text = vehicle is null
            ? $"Vehicle ID: {appraisal.VehicleId:D}{Environment.NewLine}{Environment.NewLine}The current API response did not include a nested vehicle summary."
            : string.Join(Environment.NewLine, new[]
            {
                $"Vehicle: {vehicle.DisplayName}",
                $"VIN: {Display(vehicle.Vin)}",
                $"Series: {Display(vehicle.SeriesName)}",
                $"Body Style: {Display(vehicle.BodyStyle)}",
                $"Drivetrain: {Display(vehicle.Drivetrain)}",
                $"Engine: {Display(vehicle.Engine)}",
                $"Transmission: {Display(vehicle.Transmission)}",
                $"Mileage: {(vehicle.Mileage.HasValue ? $"{vehicle.Mileage:N0} {Display(vehicle.OdometerUnit)}" : "—")}",
                $"Condition Score: {vehicle.OverallConditionScore?.ToString("0.00") ?? "—"}",
                $"Red Flags: {(vehicle.HasRedFlags ? "Yes" : "No")}"
            });
    }
}
