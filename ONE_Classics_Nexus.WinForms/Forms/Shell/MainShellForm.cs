using System;
using System.Drawing;
using System.Windows.Forms;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.Shell
{
    public partial class MainShellForm : NexusBaseForm
    {
        private TableLayoutPanel rootLayout = null!;
        private Panel headerPanel = null!;
        private Panel navPanel = null!;
        private Panel contentPanel = null!;
        private Panel statusSummaryPanel = null!;

        private Label lblAppName = null!;
        private Label lblModuleTitle = null!;
        private Label lblContext = null!;
        private Label lblSignedIn = null!;
        private Label lblSaveState = null!;

        private FlowLayoutPanel navFlow = null!;
        private Button btnDashboard = null!;
        private Button btnVehicles = null!;
        private Button btnAppraisals = null!;
        private Button btnInspections = null!;
        private Button btnComparables = null!;
        private Button btnValuations = null!;
        private Button btnReports = null!;
        private Button btnAdmin = null!;

        private Panel dashboardHost = null!;
        private FlowLayoutPanel cardFlow = null!;
        private Panel queuePanel = null!;
        private DataGridView dgvQueue = null!;
        private FlowLayoutPanel quickActionFlow = null!;

        public MainShellForm()
        {
            Text = "ONE Classics Nexus";
            Name = "MainShellForm";
            WindowState = FormWindowState.Maximized;

            BuildMainShell();
            SeedPrototypeData();
            SetActiveNav(btnDashboard);
            SetStatus("Ready | API Connected | No blocking validation errors");
        }

        private void BuildMainShell()
        {
            PageHost.Padding = Padding.Empty;
            PageHost.Controls.Clear();

            rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = NexusTheme.SurfaceAlt,
                Name = "rootLayout"
            };
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            BuildHeader();
            BuildNavigation();
            BuildContentHost();

            rootLayout.Controls.Add(headerPanel, 1, 0);
            rootLayout.Controls.Add(navPanel, 0, 0);
            rootLayout.SetRowSpan(navPanel, 2);
            rootLayout.Controls.Add(contentPanel, 1, 1);

            PageHost.Controls.Add(rootLayout);
        }

        private void BuildHeader()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Surface,
                Padding = new Padding(24, 18, 24, 18),
                Name = "headerPanel"
            };

            var headerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var titleStack = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.Transparent
            };

            lblAppName = new Label
            {
                AutoSize = true,
                Font = NexusTheme.MetaFont,
                ForeColor = NexusTheme.TextMuted,
                Text = "ONE Classics Nexus",
                Margin = new Padding(0)
            };

            lblModuleTitle = new Label
            {
                AutoSize = true,
                Font = NexusTheme.H1Font,
                ForeColor = NexusTheme.Primary,
                Text = "Main Shell Form",
                Margin = new Padding(0, 2, 0, 0)
            };

            lblContext = new Label
            {
                AutoSize = true,
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.Text,
                Text = "Unified workspace for intake, inspection, valuation, and reporting.",
                Margin = new Padding(0, 4, 0, 0)
            };

            titleStack.Controls.Add(lblAppName);
            titleStack.Controls.Add(lblModuleTitle);
            titleStack.Controls.Add(lblContext);

            var rightStack = new FlowLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.Transparent
            };

            lblSaveState = BuildHeaderBadge("All Changes Saved", NexusTheme.Success, NexusTheme.White, 160);
            lblSignedIn = BuildHeaderBadge("Signed In: UI / UX Designer", NexusTheme.Primary, NexusTheme.White, 210);

            rightStack.Controls.Add(lblSaveState);
            rightStack.Controls.Add(lblSignedIn);

            headerLayout.Controls.Add(titleStack, 0, 0);
            headerLayout.Controls.Add(rightStack, 1, 0);

            headerPanel.Controls.Add(headerLayout);
        }

        private void BuildNavigation()
        {
            navPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Surface,
                Padding = new Padding(0),
                Name = "navPanel"
            };

            var navLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = NexusTheme.Surface
            };
            navLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            navLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            navLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));

            var brandPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Primary,
                Padding = new Padding(20, 20, 20, 20)
            };

            var lblBrandTop = new Label
            {
                AutoSize = true,
                Text = "ONE Classics",
                Font = NexusTheme.LabelFont,
                ForeColor = Color.Gainsboro,
                Location = new Point(20, 22)
            };

            var lblBrandName = new Label
            {
                AutoSize = true,
                Text = "Nexus",
                Font = NexusTheme.H1Font,
                ForeColor = NexusTheme.White,
                Location = new Point(20, 42)
            };

            var lblBrandMeta = new Label
            {
                AutoSize = true,
                Text = "WinForms Main Shell",
                Font = NexusTheme.MetaFont,
                ForeColor = Color.Gainsboro,
                Location = new Point(20, 74)
            };

            brandPanel.Controls.Add(lblBrandTop);
            brandPanel.Controls.Add(lblBrandName);
            brandPanel.Controls.Add(lblBrandMeta);

            navFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(16, 16, 16, 16),
                AutoScroll = true,
                BackColor = NexusTheme.Surface
            };

            btnDashboard = CreateNavButton("Dashboard", true);
            btnVehicles = CreateNavButton("Vehicles");
            btnAppraisals = CreateNavButton("Appraisals");
            btnInspections = CreateNavButton("Inspections");
            btnComparables = CreateNavButton("Comparables");
            btnValuations = CreateNavButton("Valuations");
            btnReports = CreateNavButton("Reports");
            btnAdmin = CreateNavButton("Admin");

            navFlow.Controls.AddRange(new Control[]
            {
                btnDashboard,
                btnVehicles,
                btnAppraisals,
                btnInspections,
                btnComparables,
                btnValuations,
                btnReports,
                btnAdmin
            });

            statusSummaryPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 12, 16, 16),
                BackColor = NexusTheme.Surface
            };

            var currentContextCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Primary,
                Padding = new Padding(14),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblCurrentContextTitle = new Label
            {
                AutoSize = true,
                Text = "Current Context",
                Font = NexusTheme.MetaFont,
                ForeColor = Color.Gainsboro,
                Location = new Point(14, 14)
            };

            var lblCurrentContextValue = new Label
            {
                AutoSize = true,
                Text = "1970 Chevrolet K10 Appraisal",
                Font = NexusTheme.LabelFont,
                ForeColor = NexusTheme.White,
                Location = new Point(14, 34)
            };

            var lblCurrentContextMeta = new Label
            {
                AutoSize = true,
                Text = "Inspection 7 of 12 systems complete",
                Font = NexusTheme.MetaFont,
                ForeColor = Color.Gainsboro,
                Location = new Point(14, 58)
            };

            currentContextCard.Controls.Add(lblCurrentContextTitle);
            currentContextCard.Controls.Add(lblCurrentContextValue);
            currentContextCard.Controls.Add(lblCurrentContextMeta);
            statusSummaryPanel.Controls.Add(currentContextCard);

            navLayout.Controls.Add(brandPanel, 0, 0);
            navLayout.Controls.Add(navFlow, 0, 1);
            navLayout.Controls.Add(statusSummaryPanel, 0, 2);

            navPanel.Controls.Add(navLayout);
        }

        private void BuildContentHost()
        {
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.SurfaceAlt,
                Padding = new Padding(24),
                Name = "contentPanel"
            };

            var contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = NexusTheme.SurfaceAlt
            };
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var breadcrumbPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var lblBreadcrumb = new Label
            {
                AutoSize = true,
                Text = "Dashboard / Work Queue",
                Font = NexusTheme.MetaFont,
                ForeColor = NexusTheme.TextMuted,
                Padding = new Padding(10, 8, 10, 8),
                BackColor = NexusTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 6)
            };
            breadcrumbPanel.Controls.Add(lblBreadcrumb);

            cardFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.Transparent
            };

            cardFlow.Controls.Add(CreateKpiCard("Active Appraisals", "18", "+3 this week"));
            cardFlow.Controls.Add(CreateKpiCard("Pending Inspections", "6", "2 field-ready today"));
            cardFlow.Controls.Add(CreateKpiCard("Needs Review", "4", "Valuation confidence < 0.80"));
            cardFlow.Controls.Add(CreateKpiCard("Reports Ready", "9", "3 awaiting export"));

            var lowerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            lowerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
            lowerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));

            queuePanel = BuildQueuePanel();
            var sidePanel = BuildRightRail();

            lowerLayout.Controls.Add(queuePanel, 0, 0);
            lowerLayout.Controls.Add(sidePanel, 1, 0);

            contentLayout.Controls.Add(breadcrumbPanel, 0, 0);
            contentLayout.Controls.Add(cardFlow, 0, 1);
            contentLayout.Controls.Add(lowerLayout, 0, 2);

            dashboardHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.SurfaceAlt
            };
            dashboardHost.Controls.Add(contentLayout);

            contentPanel.Controls.Add(dashboardHost);
        }

        private Panel BuildQueuePanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Surface,
                Padding = new Padding(18),
                Margin = new Padding(0, 0, 16, 0),
                BorderStyle = BorderStyle.FixedSingle
            };

            var title = new Label
            {
                AutoSize = true,
                Text = "Active Work Queue",
                Font = NexusTheme.H3Font,
                ForeColor = NexusTheme.Primary,
                Location = new Point(18, 16)
            };

            var subtitle = new Label
            {
                AutoSize = true,
                Text = "Resume the highest-priority appraisal workflows.",
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.TextMuted,
                Location = new Point(18, 44)
            };

            var btnViewAll = NexusButtonFactory.CreatePrimary("View All");
            btnViewAll.AutoSize = false;
            btnViewAll.Size = new Size(110, 38);
            btnViewAll.Location = new Point(0, 0);
            btnViewAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            dgvQueue = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = 360,
                Name = "dgvQueue"
            };
            NexusControlStyles.ApplyDataGridView(dgvQueue);

            panel.Resize += (_, _) =>
            {
                btnViewAll.Location = new Point(panel.ClientSize.Width - btnViewAll.Width - 18, 16);
            };

            panel.Controls.Add(title);
            panel.Controls.Add(subtitle);
            panel.Controls.Add(btnViewAll);
            panel.Controls.Add(dgvQueue);

            return panel;
        }

        private Panel BuildRightRail()
        {
            var container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };

            var topCard = new Panel
            {
                Dock = DockStyle.Top,
                Height = 210,
                BackColor = NexusTheme.Surface,
                Padding = new Padding(18),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblQuickActions = new Label
            {
                AutoSize = true,
                Text = "Quick Actions",
                Font = NexusTheme.H3Font,
                ForeColor = NexusTheme.Primary,
                Location = new Point(18, 16)
            };

            quickActionFlow = new FlowLayoutPanel
            {
                Location = new Point(18, 50),
                Size = new Size(320, 130),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent
            };

            foreach (var text in new[] { "New Vehicle", "Create Appraisal", "Resume Inspection", "Manage Comparables", "Run Valuation", "Open Report" })
            {
                var button = NexusButtonFactory.CreateSecondary(text);
                button.AutoSize = false;
                button.Size = new Size(145, 40);
                quickActionFlow.Controls.Add(button);
            }

            topCard.Controls.Add(lblQuickActions);
            topCard.Controls.Add(quickActionFlow);

            var bottomCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Surface,
                Padding = new Padding(18),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblAlerts = new Label
            {
                AutoSize = true,
                Text = "Status & Alerts",
                Font = NexusTheme.H3Font,
                ForeColor = NexusTheme.Primary,
                Location = new Point(18, 16)
            };

            bottomCard.Controls.Add(lblAlerts);
            bottomCard.Controls.Add(CreateAlertPanel("4 valuation runs need manual review due to low confidence.", NexusTheme.Warning, new Point(18, 52)));
            bottomCard.Controls.Add(CreateAlertPanel("2 inspections contain red flags requiring sign-off.", NexusTheme.Danger, new Point(18, 104)));
            bottomCard.Controls.Add(CreateAlertPanel("Report export service connected and available.", NexusTheme.Success, new Point(18, 156)));

            container.Controls.Add(bottomCard);
            container.Controls.Add(topCard);

            return container;
        }

        private void SeedPrototypeData()
        {
            dgvQueue.Columns.Clear();
            dgvQueue.Columns.Add("Vehicle", "Vehicle");
            dgvQueue.Columns.Add("Vin", "VIN");
            dgvQueue.Columns.Add("Status", "Status");
            dgvQueue.Columns.Add("Owner", "Owner");
            dgvQueue.Columns.Add("Updated", "Updated");

            dgvQueue.Rows.Add("1970 Chevrolet K10", "CE140Z123456", "Inspection In Progress", "Kent Klingler", "11 min ago");
            dgvQueue.Rows.Add("1969 Chevrolet Corvette", "194679S765432", "Valuation Pending", "Office Intake", "34 min ago");
            dgvQueue.Rows.Add("1930 Ford Model A", "A1029384", "Report Draft", "Field Appraiser", "1 hr ago");
            dgvQueue.Rows.Add("1972 GMC K20", "TKL242F998877", "Needs Review", "Admin Review", "2 hr ago");
        }

        private Button CreateNavButton(string text, bool active = false)
        {
            var button = new Button
            {
                Text = text,
                Width = 200,
                Height = 44,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 0, 0, 8),
                FlatStyle = FlatStyle.Flat,
                Font = NexusTheme.LabelFont,
                Cursor = Cursors.Hand,
                Tag = "nav"
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += NavButton_Click;

            ApplyNavVisual(button, active);
            return button;
        }

        private void NavButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button clicked)
            {
                SetActiveNav(clicked);
                lblModuleTitle.Text = clicked.Text;
                lblContext.Text = $"{clicked.Text} workspace loaded in the shared MainShellForm content host.";
                SetStatus($"{clicked.Text} selected");
            }
        }

        private void SetActiveNav(Button activeButton)
        {
            foreach (Control control in navFlow.Controls)
            {
                if (control is Button button)
                {
                    ApplyNavVisual(button, ReferenceEquals(button, activeButton));
                }
            }
        }

        private void ApplyNavVisual(Button button, bool active)
        {
            button.BackColor = active ? NexusTheme.Primary : NexusTheme.SurfaceAlt;
            button.ForeColor = active ? NexusTheme.White : NexusTheme.TextStrong;
        }

        private Panel CreateKpiCard(string title, string value, string subtitle)
        {
            var card = new Panel
            {
                Width = 250,
                Height = 130,
                BackColor = NexusTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 16, 16),
                Padding = new Padding(18)
            };

            var lblTitle = new Label
            {
                AutoSize = true,
                Text = title,
                Font = NexusTheme.LabelFont,
                ForeColor = NexusTheme.TextMuted,
                Location = new Point(18, 16)
            };

            var lblValue = new Label
            {
                AutoSize = true,
                Text = value,
                Font = NexusTheme.KpiFont,
                ForeColor = NexusTheme.Primary,
                Location = new Point(18, 40)
            };

            var lblSub = new Label
            {
                AutoSize = true,
                Text = subtitle,
                Font = NexusTheme.MetaFont,
                ForeColor = NexusTheme.TextMuted,
                Location = new Point(18, 92)
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblSub);
            return card;
        }

        private Panel CreateAlertPanel(string message, Color accent, Point location)
        {
            var panel = new Panel
            {
                Location = location,
                Size = new Size(320, 40),
                BackColor = NexusTheme.SurfaceAlt,
                BorderStyle = BorderStyle.FixedSingle
            };

            var stripe = new Panel
            {
                Dock = DockStyle.Left,
                Width = 6,
                BackColor = accent
            };

            var label = new Label
            {
                AutoSize = false,
                Text = message,
                Font = NexusTheme.MetaFont,
                ForeColor = NexusTheme.TextStrong,
                Location = new Point(14, 9),
                Size = new Size(296, 22)
            };

            panel.Controls.Add(stripe);
            panel.Controls.Add(label);
            return panel;
        }

        private Label BuildHeaderBadge(string text, Color backColor, Color foreColor, int width)
        {
            return new Label
            {
                AutoSize = false,
                Width = width,
                Height = 42,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = backColor,
                ForeColor = foreColor,
                Font = NexusTheme.LabelFont,
                Margin = new Padding(0, 0, 12, 0),
                BorderStyle = BorderStyle.FixedSingle
            };
        }
        private void LoadChildForm(Form childForm)
        {
            contentPanel.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            contentPanel.Controls.Add(childForm);
            childForm.Show();
        }
    }
}
