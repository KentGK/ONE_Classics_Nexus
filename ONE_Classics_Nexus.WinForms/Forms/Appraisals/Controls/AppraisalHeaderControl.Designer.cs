#nullable enable
namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Controls;

partial class AppraisalHeaderControl
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel layoutTable = null!;
    private Label lblTitle = null!;
    private Label lblVehicle = null!;
    private AppraisalStatusBadge statusBadge = null!;
    private FlowLayoutPanel actionsPanel = null!;
    private Button btnRefresh = null!;
    private Button btnSave = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        layoutTable = new TableLayoutPanel(); // Renamed from 'layout'
        lblTitle = new Label();
        lblVehicle = new Label();
        statusBadge = new AppraisalStatusBadge();
        actionsPanel = new FlowLayoutPanel();
        btnRefresh = new Button();
        btnSave = new Button();
        SuspendLayout();

        layoutTable.ColumnCount = 3;
        layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layoutTable.RowCount = 2;
        layoutTable.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        layoutTable.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        layoutTable.Dock = DockStyle.Fill;
        layoutTable.Padding = new Padding(24, 12, 18, 10);

        lblTitle.Dock = DockStyle.Fill;
        lblTitle.TextAlign = ContentAlignment.BottomLeft;
        lblTitle.AutoEllipsis = true;
        layoutTable.Controls.Add(lblTitle, 0, 0);

        lblVehicle.Dock = DockStyle.Fill;
        lblVehicle.TextAlign = ContentAlignment.TopLeft;
        lblVehicle.AutoEllipsis = true;
        layoutTable.Controls.Add(lblVehicle, 0, 1); 

        statusBadge.Anchor = AnchorStyles.None;
        layoutTable.SetRowSpan(statusBadge, 2);
        layoutTable.Controls.Add(statusBadge, 1, 0);

        actionsPanel.AutoSize = true;
        actionsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.FlowDirection = FlowDirection.LeftToRight;
        actionsPanel.WrapContents = false;
        actionsPanel.Padding = new Padding(4, 16, 0, 0);
        layoutTable.SetRowSpan(actionsPanel, 2);
        layoutTable.Controls.Add(actionsPanel, 2, 0);

        btnRefresh.Text = "Refresh";
        btnRefresh.AutoSize = true;
        btnRefresh.MinimumSize = new Size(86, 34);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        actionsPanel.Controls.Add(btnRefresh);

        btnSave.Text = "Save";
        btnSave.AutoSize = true;
        btnSave.MinimumSize = new Size(86, 34);
        btnSave.FlatStyle = FlatStyle.Flat;
        actionsPanel.Controls.Add(btnSave);

        Controls.Add(layoutTable);
        Name = "AppraisalHeaderControl";
        Size = new Size(1100, 92);
        ResumeLayout(false);
    }
}
