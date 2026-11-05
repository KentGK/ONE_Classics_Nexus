using System.Drawing.Drawing2D;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Styling;

public static class AppraisalWorkspaceTheme
{
    // ONE Classics Nexus locked primary theme color.
    public static readonly Color Primary = ColorTranslator.FromHtml("#0E2841");
    public static readonly Color PrimaryHover = ColorTranslator.FromHtml("#173D5E");
    public static readonly Color Accent = ColorTranslator.FromHtml("#2F6F9F");
    public static readonly Color Canvas = ColorTranslator.FromHtml("#F4F7FA");
    public static readonly Color Surface = Color.White;
    public static readonly Color Border = ColorTranslator.FromHtml("#D7E0E8");
    public static readonly Color TextPrimary = ColorTranslator.FromHtml("#1E2933");
    public static readonly Color TextSecondary = ColorTranslator.FromHtml("#667482");
    public static readonly Color Success = ColorTranslator.FromHtml("#2E7D32");
    public static readonly Color Warning = ColorTranslator.FromHtml("#A86400");
    public static readonly Color Danger = ColorTranslator.FromHtml("#B3261E");
    public static readonly Color Muted = ColorTranslator.FromHtml("#6B7785");

    public static Font HeaderFont(float size = 18f) => new("Segoe UI Semibold", size, FontStyle.Bold);
    public static Font SectionFont(float size = 12f) => new("Segoe UI Semibold", size, FontStyle.Bold);
    public static Font BodyFont(float size = 9.5f) => new("Segoe UI", size, FontStyle.Regular);

    public static Button CreatePrimaryButton(string text)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(96, 34),
            BackColor = Primary,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = BodyFont(),
            Cursor = Cursors.Hand,
            Padding = new Padding(10, 2, 10, 2)
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = PrimaryHover;
        return button;
    }

    public static Button CreateSecondaryButton(string text)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MinimumSize = new Size(96, 34),
            BackColor = Surface,
            ForeColor = Primary,
            FlatStyle = FlatStyle.Flat,
            Font = BodyFont(),
            Cursor = Cursors.Hand,
            Padding = new Padding(10, 2, 10, 2)
        };
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.MouseOverBackColor = Canvas;
        return button;
    }

    public static Panel CreateCard()
    {
        return new Panel
        {
            BackColor = Surface,
            Padding = new Padding(16),
            Margin = new Padding(6)
        };
    }

    public static Color StatusColor(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return Muted;

        var normalized = status.Trim().Replace("_", string.Empty).Replace("-", string.Empty).ToLowerInvariant();
        return normalized switch
        {
            "complete" or "completed" or "approved" or "issued" or "success" => Success,
            "failed" or "rejected" or "cancelled" or "canceled" or "blocked" => Danger,
            "needsreview" or "pendingreview" or "warning" or "inprogress" => Warning,
            _ => Accent
        };
    }

    public static void DrawRoundedRectangle(Graphics graphics, Pen pen, Rectangle bounds, int radius)
    {
        using var path = CreateRoundedPath(bounds, radius);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.DrawPath(pen, path);
    }

    public static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
    {
        var diameter = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
        path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
        path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }

}
