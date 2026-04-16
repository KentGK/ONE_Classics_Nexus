// ============================================================
// AdminForm.cs
// ============================================================
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.Admin
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            BuildPlaceholder("Admin", "Administration, lookup, and configuration workspace.");
        }

        private void BuildPlaceholder(string title, string subtitle)
        {
            BackColor = NexusTheme.SurfaceAlt;
            Controls.Clear();

            var titleLabel = new Label
            {
                AutoSize = true,
                Text = title,
                Font = NexusTheme.H1Font,
                ForeColor = NexusTheme.Primary,
                Location = new Point(32, 28)
            };

            var subtitleLabel = new Label
            {
                AutoSize = true,
                Text = subtitle,
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.TextMuted,
                Location = new Point(34, 66)
            };

            var panel = new Panel
            {
                Location = new Point(32, 110),
                Size = new Size(760, 180),
                BackColor = NexusTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var bodyLabel = new Label
            {
                AutoSize = true,
                Text = "Placeholder form loaded successfully. Admin UI will be implemented here.",
                Font = NexusTheme.H3Font,
                ForeColor = NexusTheme.TextStrong,
                Location = new Point(24, 28)
            };

            panel.Controls.Add(bodyLabel);
            Controls.Add(titleLabel);
            Controls.Add(subtitleLabel);
            Controls.Add(panel);
        }
    }
}