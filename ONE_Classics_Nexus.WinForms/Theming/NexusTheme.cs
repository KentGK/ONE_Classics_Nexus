using System;
using System.Drawing;
using System.Windows.Forms;

namespace ONE_Classics_Nexus.WinForms.Theming
{
    /// <summary>
    /// Nexus theme tokens. Primary theme color is locked to ONE Classics brand blue.
    /// </summary>
    public static class NexusTheme
    {
        public static readonly Color Primary = ColorTranslator.FromHtml("#0E2841");
        public static readonly Color PrimaryHover = ColorTranslator.FromHtml("#133856");
        public static readonly Color PrimarySoft = ColorTranslator.FromHtml("#EAF0F5");
        public static readonly Color Surface = Color.White;
        public static readonly Color SurfaceAlt = ColorTranslator.FromHtml("#F6F8FA");
        public static readonly Color Border = ColorTranslator.FromHtml("#D7DEE5");
        public static readonly Color TextStrong = ColorTranslator.FromHtml("#1B2430");
        public static readonly Color Text = ColorTranslator.FromHtml("#3E4A59");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#6C7A89");
        public static readonly Color Success = ColorTranslator.FromHtml("#2E7D32");
        public static readonly Color Warning = ColorTranslator.FromHtml("#B7791F");
        public static readonly Color Danger = ColorTranslator.FromHtml("#B42318");
        public static readonly Color Info = ColorTranslator.FromHtml("#155EEF");
        public static readonly Color White = Color.White;

        public static readonly Font DisplayFont = new("Segoe UI", 28F, FontStyle.Bold);
        public static readonly Font H1Font = new("Segoe UI", 24F, FontStyle.Bold);
        public static readonly Font H2Font = new("Segoe UI", 20F, FontStyle.Bold);
        public static readonly Font H3Font = new("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font BodyFont = new("Segoe UI", 10.5F, FontStyle.Regular);
        public static readonly Font LabelFont = new("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font MetaFont = new("Segoe UI", 8.5F, FontStyle.Regular);
        public static readonly Font KpiFont = new("Segoe UI", 30F, FontStyle.Bold);
    }

    public static class NexusSpacing
    {
        public const int Xs = 4;
        public const int Sm = 8;
        public const int Md = 12;
        public const int Lg = 16;
        public const int Xl = 24;
        public const int Xxl = 32;
        public const int PagePadding = 24;
        public const int SectionGap = 24;
        public const int GroupGap = 16;

        public static readonly Padding Page = new(PagePadding);
        public static readonly Padding Card = new(20);
        public static readonly Padding Section = new(24);
        public static readonly Padding Input = new(10, 6, 10, 6);
    }

    public static class NexusControlStyles
    {
        public static void ApplyForm(Form form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));

            form.BackColor = NexusTheme.SurfaceAlt;
            form.Font = NexusTheme.BodyFont;
            form.ForeColor = NexusTheme.Text;
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        public static void ApplyPageHost(Control control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            control.BackColor = NexusTheme.SurfaceAlt;
            control.ForeColor = NexusTheme.Text;
            control.Font = NexusTheme.BodyFont;
            control.Padding = NexusSpacing.Page;
        }

        public static void ApplyCard(Panel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            panel.BackColor = NexusTheme.Surface;
            panel.ForeColor = NexusTheme.Text;
            panel.Padding = NexusSpacing.Card;
            panel.Margin = new Padding(0, 0, 0, NexusSpacing.SectionGap);
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void ApplyHeaderLabel(Label label)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            label.AutoSize = true;
            label.Font = NexusTheme.H1Font;
            label.ForeColor = NexusTheme.Primary;
            label.BackColor = Color.Transparent;
        }

        public static void ApplySectionLabel(Label label)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            label.AutoSize = true;
            label.Font = NexusTheme.H3Font;
            label.ForeColor = NexusTheme.Primary;
            label.BackColor = Color.Transparent;
        }

        public static void ApplyBodyLabel(Label label)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            label.AutoSize = true;
            label.Font = NexusTheme.BodyFont;
            label.ForeColor = NexusTheme.Text;
            label.BackColor = Color.Transparent;
        }

        public static void ApplyMetaLabel(Label label)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            label.AutoSize = true;
            label.Font = NexusTheme.MetaFont;
            label.ForeColor = NexusTheme.TextMuted;
            label.BackColor = Color.Transparent;
        }

        public static void ApplyTextBox(TextBox textBox)
        {
            if (textBox == null) throw new ArgumentNullException(nameof(textBox));

            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = NexusTheme.Surface;
            textBox.ForeColor = NexusTheme.TextStrong;
            textBox.Font = NexusTheme.BodyFont;
            textBox.Margin = new Padding(0, 0, 0, NexusSpacing.GroupGap);
        }

        public static void ApplyComboBox(ComboBox comboBox)
        {
            if (comboBox == null) throw new ArgumentNullException(nameof(comboBox));

            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.BackColor = NexusTheme.Surface;
            comboBox.ForeColor = NexusTheme.TextStrong;
            comboBox.Font = NexusTheme.BodyFont;
            comboBox.Margin = new Padding(0, 0, 0, NexusSpacing.GroupGap);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public static void ApplyPrimaryButton(Button button)
        {
            if (button == null) throw new ArgumentNullException(nameof(button));

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = NexusTheme.Primary;
            button.ForeColor = NexusTheme.White;
            button.Font = NexusTheme.LabelFont;
            button.Height = 40;
            button.Padding = new Padding(14, 8, 14, 8);
            button.Cursor = Cursors.Hand;

            button.MouseEnter -= PrimaryButtonMouseEnter;
            button.MouseLeave -= PrimaryButtonMouseLeave;
            button.MouseEnter += PrimaryButtonMouseEnter;
            button.MouseLeave += PrimaryButtonMouseLeave;
        }

        public static void ApplySecondaryButton(Button button)
        {
            if (button == null) throw new ArgumentNullException(nameof(button));

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = NexusTheme.Border;
            button.BackColor = NexusTheme.Surface;
            button.ForeColor = NexusTheme.TextStrong;
            button.Font = NexusTheme.LabelFont;
            button.Height = 40;
            button.Padding = new Padding(14, 8, 14, 8);
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyDangerButton(Button button)
        {
            if (button == null) throw new ArgumentNullException(nameof(button));

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = NexusTheme.Danger;
            button.ForeColor = NexusTheme.White;
            button.Font = NexusTheme.LabelFont;
            button.Height = 40;
            button.Padding = new Padding(14, 8, 14, 8);
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyDataGridView(DataGridView grid)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));

            grid.BackgroundColor = NexusTheme.Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = NexusTheme.Border;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.Font = NexusTheme.BodyFont;
            grid.DefaultCellStyle.BackColor = NexusTheme.Surface;
            grid.DefaultCellStyle.ForeColor = NexusTheme.Text;
            grid.DefaultCellStyle.SelectionBackColor = NexusTheme.PrimarySoft;
            grid.DefaultCellStyle.SelectionForeColor = NexusTheme.TextStrong;
            grid.ColumnHeadersDefaultCellStyle.BackColor = NexusTheme.SurfaceAlt;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = NexusTheme.TextStrong;
            grid.ColumnHeadersDefaultCellStyle.Font = NexusTheme.LabelFont;
            grid.ColumnHeadersHeight = 40;
            grid.RowTemplate.Height = 36;
        }

        public static void ApplyStatusStrip(StatusStrip strip)
        {
            if (strip == null) throw new ArgumentNullException(nameof(strip));

            strip.BackColor = NexusTheme.Surface;
            strip.ForeColor = NexusTheme.TextMuted;
            strip.Font = NexusTheme.MetaFont;
            strip.SizingGrip = false;
        }

        private static void PrimaryButtonMouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = NexusTheme.PrimaryHover;
            }
        }

        private static void PrimaryButtonMouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackColor = NexusTheme.Primary;
            }
        }
    }

    public static class NexusStatusStyles
    {
        public static void ApplySuccess(Label label) => ApplyStatus(label, NexusTheme.Success, NexusTheme.White);
        public static void ApplyWarning(Label label) => ApplyStatus(label, NexusTheme.Warning, NexusTheme.White);
        public static void ApplyDanger(Label label) => ApplyStatus(label, NexusTheme.Danger, NexusTheme.White);
        public static void ApplyInfo(Label label) => ApplyStatus(label, NexusTheme.Info, NexusTheme.White);
        public static void ApplyPrimary(Label label) => ApplyStatus(label, NexusTheme.Primary, NexusTheme.White);

        private static void ApplyStatus(Label label, Color backColor, Color foreColor)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = NexusTheme.MetaFont;
            label.BackColor = backColor;
            label.ForeColor = foreColor;
            label.Padding = new Padding(8, 4, 8, 4);
            label.Height = 24;
            label.MinimumSize = new Size(90, 24);
        }
    }

    public static class NexusButtonFactory
    {
        public static Button CreatePrimary(string text, EventHandler? onClick = null)
        {
            var button = new Button { Text = text, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
            NexusControlStyles.ApplyPrimaryButton(button);
            if (onClick != null) button.Click += onClick;
            return button;
        }

        public static Button CreateSecondary(string text, EventHandler? onClick = null)
        {
            var button = new Button { Text = text, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
            NexusControlStyles.ApplySecondaryButton(button);
            if (onClick != null) button.Click += onClick;
            return button;
        }

        public static Button CreateDanger(string text, EventHandler? onClick = null)
        {
            var button = new Button { Text = text, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
            NexusControlStyles.ApplyDangerButton(button);
            if (onClick != null) button.Click += onClick;
            return button;
        }
    }

    /// <summary>
    /// Optional base form for rapid adoption across the WinForms app.
    /// </summary>
    public class NexusBaseForm : Form
    {
        protected Panel PageHost = null!;
        protected StatusStrip AppStatusStrip = null!;
        protected ToolStripStatusLabel StatusLabel = null!;

        public NexusBaseForm()
        {
            InitializeBaseLayout();
        }

        private void InitializeBaseLayout()
        {
            NexusControlStyles.ApplyForm(this);
            MinimumSize = new Size(1280, 800);

            PageHost = new Panel
            {
                Dock = DockStyle.Fill,
                Name = "pageHost"
            };
            NexusControlStyles.ApplyPageHost(PageHost);

            AppStatusStrip = new StatusStrip
            {
                Dock = DockStyle.Bottom,
                Name = "appStatusStrip"
            };
            NexusControlStyles.ApplyStatusStrip(AppStatusStrip);

            StatusLabel = new ToolStripStatusLabel
            {
                Name = "statusLabel",
                Text = "Ready"
            };

            AppStatusStrip.Items.Add(StatusLabel);
            Controls.Add(PageHost);
            Controls.Add(AppStatusStrip);
        }

        protected void SetStatus(string message)
        {
            StatusLabel.Text = message;
        }
    }
}
