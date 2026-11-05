namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

partial class AppraisalHeaderControl
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel layout;
    private Label lblTitle;
    private Label lblVehicle;
    private AppraisalStatusBadge statusBadge;
    private FlowLayoutPanel actionsPanel;
    private Button btnRefresh;
    private Button btnSave;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        layout = new TableLayoutPanel();
        lblTitle = new Label();
        lblVehicle = new Label();
        statusBadge = new AppraisalStatusBadge();
        actionsPanel = new FlowLayoutPanel();
        btnRefresh = new Button();
        btnSave = new Button();
        SuspendLayout();

        layout.ColumnCount = 3;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.RowCount = 2;
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(24, 12, 18, 10);

        lblTitle.Dock = DockStyle.Fill;
        lblTitle.TextAlign = ContentAlignment.BottomLeft;
        lblTitle.AutoEllipsis = true;
        layout.Controls.Add(lblTitle, 0, 0);

        lblVehicle.Dock = DockStyle.Fill;
        lblVehicle.TextAlign = ContentAlignment.TopLeft;
        lblVehicle.AutoEllipsis = true;
        layout.Controls.Add(lblVehicle, 0, 1);

        statusBadge.Anchor = AnchorStyles.None;
        layout.SetRowSpan(statusBadge, 2);
        layout.Controls.Add(statusBadge, 1, 0);

        actionsPanel.AutoSize = true;
        actionsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.FlowDirection = FlowDirection.LeftToRight;
        actionsPanel.WrapContents = false;
        actionsPanel.Padding = new Padding(4, 16, 0, 0);
        layout.SetRowSpan(actionsPanel, 2);
        layout.Controls.Add(actionsPanel, 2, 0);

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

        Controls.Add(layout);
        Name = "AppraisalHeaderControl";
        Size = new Size(1100, 92);
        ResumeLayout(false);
    }
}
