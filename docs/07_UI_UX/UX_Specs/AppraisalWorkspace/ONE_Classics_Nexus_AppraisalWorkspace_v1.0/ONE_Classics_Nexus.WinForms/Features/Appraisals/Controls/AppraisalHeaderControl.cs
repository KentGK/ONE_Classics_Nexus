using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

public partial class AppraisalHeaderControl : UserControl
{
    public AppraisalHeaderControl()
    {
        InitializeComponent();
        BackColor = AppraisalWorkspaceTheme.Primary;

        lblTitle.Font = AppraisalWorkspaceTheme.HeaderFont(18f);
        lblTitle.ForeColor = Color.White;
        lblTitle.Text = "Appraisal Workspace";

        lblVehicle.Font = AppraisalWorkspaceTheme.BodyFont(10f);
        lblVehicle.ForeColor = Color.FromArgb(218, 230, 241);

        btnRefresh.BackColor = Color.White;
        btnRefresh.ForeColor = AppraisalWorkspaceTheme.Primary;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(110, 145, 175);
        btnRefresh.Cursor = Cursors.Hand;

        btnSave.BackColor = AppraisalWorkspaceTheme.Accent;
        btnSave.ForeColor = Color.White;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Cursor = Cursors.Hand;

        btnRefresh.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        btnSave.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? RefreshRequested;
    public event EventHandler? SaveRequested;

    public void Bind(AppraisalWorkspaceDto appraisal, bool hasUnsavedChanges, bool isBusy)
    {
        ArgumentNullException.ThrowIfNull(appraisal);

        lblTitle.Text = $"Appraisal {appraisal.DisplayNumber}";
        lblVehicle.Text = appraisal.VehicleDisplayName;
        statusBadge.SetStatus(appraisal.Status);
        btnSave.Text = hasUnsavedChanges ? "Save *" : "Save";
        btnSave.Enabled = !isBusy;
        btnRefresh.Enabled = !isBusy;
    }

    public void SetBusy(bool isBusy)
    {
        btnSave.Enabled = !isBusy;
        btnRefresh.Enabled = !isBusy;
    }
}
