using ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Controls;

public sealed class AppraisalStatusBadge : Label
{
    private Color _borderColor = AppraisalWorkspaceTheme.Muted;

    public AppraisalStatusBadge()
    {
        AutoSize = false;
        Size = new Size(120, 28);
        TextAlign = ContentAlignment.MiddleCenter;
        Font = AppraisalWorkspaceTheme.BodyFont(9f);
        ForeColor = Color.White;
        BackColor = AppraisalWorkspaceTheme.Muted;
        Padding = new Padding(8, 2, 8, 2);
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    public void SetStatus(string? status)
    {
        Text = string.IsNullOrWhiteSpace(status) ? "Unknown" : status;
        BackColor = AppraisalWorkspaceTheme.StatusColor(status);
        _borderColor = BackColor;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? Color.White);
        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = AppraisalWorkspaceTheme.CreateRoundedPath(bounds, 8);
        using var brush = new SolidBrush(BackColor);
        using var pen = new Pen(_borderColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        e.Graphics.FillPath(brush, path);
        e.Graphics.DrawPath(pen, path);
        TextRenderer.DrawText(e.Graphics, Text, Font, bounds, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
