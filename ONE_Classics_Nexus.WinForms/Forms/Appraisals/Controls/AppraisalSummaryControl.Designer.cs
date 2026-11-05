#nullable enable
namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Controls;

partial class AppraisalSummaryControl
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel grid = null!;
    private Label lblTypeValue = null!;
    private Label lblEffectiveValue = null!;
    private Label lblAppraiserValue = null!;
    private Label lblReviewerValue = null!;
    private Label lblUpdatedValue = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        grid = new TableLayoutPanel();
        lblTypeValue = new Label();
        lblEffectiveValue = new Label();
        lblAppraiserValue = new Label();
        lblReviewerValue = new Label();
        lblUpdatedValue = new Label();
        SuspendLayout();

        grid.ColumnCount = 5;
        for (var i = 0; i < 5; i++)
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        grid.RowCount = 2;
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        grid.Dock = DockStyle.Fill;
        grid.Padding = new Padding(20, 10, 20, 10);

        AddCell(0, "Appraisal Type", lblTypeValue);
        AddCell(1, "Effective Date", lblEffectiveValue);
        AddCell(2, "Assigned Appraiser", lblAppraiserValue);
        AddCell(3, "Assigned Reviewer", lblReviewerValue);
        AddCell(4, "Last Updated", lblUpdatedValue);

        Controls.Add(grid);
        Name = "AppraisalSummaryControl";
        Size = new Size(1100, 92);
        ResumeLayout(false);
    }

    private void AddCell(int column, string caption, Label valueLabel)
    {
        var captionLabel = new Label
        {
            Text = caption.ToUpperInvariant(),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.BottomLeft,
            AutoEllipsis = true
        };
        valueLabel.Dock = DockStyle.Fill;
        valueLabel.TextAlign = ContentAlignment.TopLeft;
        valueLabel.AutoEllipsis = true;
        grid.Controls.Add(captionLabel, column, 0);
        grid.Controls.Add(valueLabel, column, 1);
    }
}
