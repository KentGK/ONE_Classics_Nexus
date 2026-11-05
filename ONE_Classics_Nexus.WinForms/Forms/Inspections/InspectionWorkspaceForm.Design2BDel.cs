using System.Drawing;
using System.Windows.Forms;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections
{
    public partial class InspectionWorkspaceForm
    {
        // Header controls
        private TextBox txtInspectionId;
        private ComboBox cmbStatus;
        private ComboBox cmbInspectionType;
        private ComboBox cmbTemplate;
        private TextBox txtLocation;
        private DateTimePicker dtpInspectedOn;
        private ComboBox cmbInspectedBy;

        // Appraisal info
        private Label lblAppraisalId;
        private Label lblAppraisalNumber;
        private Label lblAppraisalStatus;
        private Label lblAppraisalType;
        private DateTimePicker dtpEffectiveDate;
        private Label lblClient;

        // Vehicle info
        private Label lblVin;
        private Label lblYearMakeModel;
        private Label lblBodyStyle;
        private Label lblDrivetrain;
        private Label lblEngineTransmission;
        private Label lblMileage;
        private PictureBox pbVehicleImage;

        // Tabs
        private TabControl tabControl;

        // 12-System Scores DataGridView
        private DataGridView dgvSystemScores;

        // Section Summary DataGridView
        private DataGridView dgvSectionSummary;

        // Metrics
        private Label lblConditionIndex;
        private Label lblConditionIndexSubtext;
        private Label lblTotalRedFlags;
        private Label lblInspectionProgress;
        private ProgressBar pbProgress;

        // Quick Notes
        private TextBox txtQuickNotes;

        // Next Steps Checklist
        private CheckedListBox clbNextSteps;

        // Toolbar buttons
        private Button btnNewInspection;
        private Button btnSaveDraft;
        private Button btnComplete;
        private Button btnDelete;
        private Button btnRefresh;

        private void InitializeComponent()
        {
            this.Text = "Inspection Workspace";
            this.Size = new Size(1400, 900);
            this.BackColor = Color.FromArgb(240, 240, 245);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void SetupCustomControls()
        {
            // Main container panel
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };
            this.Controls.Add(mainPanel);

            // Toolbar
            var toolbar = CreateToolbar();
            toolbar.Dock = DockStyle.Top;
            mainPanel.Controls.Add(toolbar);

            // Content panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0)
            };
            mainPanel.Controls.Add(contentPanel);

            // Top section - 3 columns (Inspection | Appraisal | Vehicle)
            var topSection = CreateTopSection();
            topSection.Dock = DockStyle.Top;
            topSection.Height = 280;
            contentPanel.Controls.Add(topSection);

            // Tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };
            contentPanel.Controls.Add(tabControl);

            // Overview tab
            var overviewTab = new TabPage("Overview");
            tabControl.TabPages.Add(overviewTab);
            SetupOverviewTab(overviewTab);

            // Additional tabs
            tabControl.TabPages.Add(new TabPage("12-System Scores"));
            tabControl.TabPages.Add(new TabPage("Sections & Items"));
            tabControl.TabPages.Add(new TabPage("Red Flags"));
            tabControl.TabPages.Add(new TabPage("Media"));
            tabControl.TabPages.Add(new TabPage("Notes"));
            tabControl.TabPages.Add(new TabPage("History"));

            // Status bar
            var statusBar = CreateStatusBar();
            statusBar.Dock = DockStyle.Bottom;
            this.Controls.Add(statusBar);
        }

        private Panel CreateToolbar()
        {
            var toolbar = new Panel
            {
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            var titleLabel = new Label
            {
                Text = "📋 Inspection Workspace",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 15),
                AutoSize = true
            };
            toolbar.Controls.Add(titleLabel);

            // int btnX = toolbar.Width - 650;
            int btnX = toolbar.Width - 1300;

            btnNewInspection = CreateButton("➕ New Inspection", btnX, Color.FromArgb(0, 120, 212));
            btnSaveDraft = CreateButton("💾 Save Draft", btnX + 130, Color.FromArgb(80, 80, 80));
            btnComplete = CreateButton("✅ Complete Inspection", btnX + 240, Color.FromArgb(16, 124, 16));
            btnDelete = CreateButton("🗑️ Delete (Soft)", btnX + 410, Color.FromArgb(164, 38, 44));
            btnRefresh = CreateButton("🔄 Refresh", btnX + 540, Color.FromArgb(80, 80, 80));

            toolbar.Controls.AddRange(new Control[] { btnNewInspection, btnSaveDraft, btnComplete, btnDelete, btnRefresh });

            return toolbar;
        }

        private Button CreateButton(string text, int x, Color backColor)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, 12),
                Size = new Size(120, 36),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
        }

        private Panel CreateTopSection()
        {
            var topPanel = new Panel
            {
                BackColor = Color.Transparent
            };

            // Inspection column
            var inspectionPanel = CreateCard("Inspection", 0);
            inspectionPanel.Width = 380;
            topPanel.Controls.Add(inspectionPanel);
            SetupInspectionPanel(inspectionPanel);

            // Appraisal column
            var appraisalPanel = CreateCard("Appraisal", 390);
            appraisalPanel.Width = 380;
            topPanel.Controls.Add(appraisalPanel);
            SetupAppraisalPanel(appraisalPanel);

            // Vehicle column
            var vehiclePanel = CreateCard("Vehicle", 780);
            vehiclePanel.Width = 380;
            topPanel.Controls.Add(vehiclePanel);
            SetupVehiclePanel(vehiclePanel);

            return topPanel;
        }

        private Panel CreateCard(string title, int x)
        {
            var card = new Panel
            {
                Location = new Point(x, 0),
                Height = 260,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };
            card.Controls.Add(titleLabel);

            return card;
        }

        private void SetupInspectionPanel(Panel panel)
        {
            int y = 45;

            AddLabel(panel, "Inspection ID", 15, y);
            txtInspectionId = AddTextBox(panel, 15, y + 20, 340, true);
            y += 60;

            AddLabel(panel, "Status", 15, y);
            cmbStatus = AddComboBox(panel, 15, y + 20, 340, new[] { "Not Started", "In Progress", "Completed", "On Hold" });
            y += 60;

            AddLabel(panel, "Inspection Type", 15, y);
            cmbInspectionType = AddComboBox(panel, 15, y + 20, 340, new[] { "Physical", "Virtual", "Hybrid" });
            y += 60;

            AddLabel(panel, "Template", 15, y);
            cmbTemplate = AddComboBox(panel, 15, y + 20, 340, new[] { "Standard Vehicle", "Custom Vehicle", "Classic Vehicle" });
        }

        private void SetupAppraisalPanel(Panel panel)
        {
            int y = 45;

            AddLabel(panel, "Appraisal ID", 15, y);
            lblAppraisalId = AddValueLabel(panel, 15, y + 20);
            y += 50;

            AddLabel(panel, "Appraisal Number", 15, y);
            lblAppraisalNumber = AddValueLabel(panel, 15, y + 20);
            y += 50;

            AddLabel(panel, "Status", 15, y);
            lblAppraisalStatus = AddValueLabel(panel, 15, y + 20);
            y += 50;

            AddLabel(panel, "Appraisal Type", 15, y);
            lblAppraisalType = AddValueLabel(panel, 15, y + 20);
        }

        private void SetupVehiclePanel(Panel panel)
        {
            int y = 45;

            // Vehicle image
            pbVehicleImage = new PictureBox
            {
                Location = new Point(220, 45),
                Size = new Size(140, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.Add(pbVehicleImage);

            AddLabel(panel, "VIN", 15, y);
            lblVin = AddValueLabel(panel, 15, y + 20);
            y += 40;

            AddLabel(panel, "Year / Make / Model", 15, y);
            lblYearMakeModel = AddValueLabel(panel, 15, y + 20);
            y += 40;

            AddLabel(panel, "Body Style", 15, y);
            lblBodyStyle = AddValueLabel(panel, 15, y + 20);
            y += 40;

            AddLabel(panel, "Drivetrain", 15, y);
            lblDrivetrain = AddValueLabel(panel, 15, y + 20);
            y += 40;

            AddLabel(panel, "Engine / Transmission", 15, y);
            lblEngineTransmission = AddValueLabel(panel, 15, y + 20);
        }

        private void SetupOverviewTab(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(240, 240, 245);
            tab.Padding = new Padding(15);

            // Left panel - System Scores
            var leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 650,
                Padding = new Padding(0, 0, 10, 0)
            };
            tab.Controls.Add(leftPanel);

            var systemScoresCard = CreateCard("12-System Condition Scores", 0);
            systemScoresCard.Dock = DockStyle.Fill;
            leftPanel.Controls.Add(systemScoresCard);

            dgvSystemScores = new DataGridView
            {
                Location = new Point(10, 45),
                Size = new Size(620, 380),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            systemScoresCard.Controls.Add(dgvSystemScores);

            SetupSystemScoresGrid();

            // Metrics panel below system scores
            var metricsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 150
            };
            leftPanel.Controls.Add(metricsPanel);
            SetupMetricsPanel(metricsPanel);

            // Right panel - Section Summary + Notes + Checklist
            var rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 0, 0)
            };
            tab.Controls.Add(rightPanel);

            var sectionSummaryCard = CreateCard("Section Summary (from Items)", 0);
            sectionSummaryCard.Dock = DockStyle.Top;
            sectionSummaryCard.Height = 320;
            rightPanel.Controls.Add(sectionSummaryCard);

            dgvSectionSummary = new DataGridView
            {
                Location = new Point(10, 45),
                // Size = new Size(500, 260),
                Size = new Size(250, 260),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            sectionSummaryCard.Controls.Add(dgvSectionSummary);
            SetupSectionSummaryGrid();

            // Bottom section - Quick Notes and Next Steps
            var bottomRightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0)
            };
            rightPanel.Controls.Add(bottomRightPanel);

            var notesCard = CreateCard("Quick Notes", 0);
            notesCard.Dock = DockStyle.Left;
            notesCard.Width = 350;
            bottomRightPanel.Controls.Add(notesCard);

            txtQuickNotes = new TextBox
            {
                Location = new Point(10, 45),
                Size = new Size(320, 200),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            notesCard.Controls.Add(txtQuickNotes);

            var checklistCard = CreateCard("Next Steps / Checklist", 360);
            checklistCard.Dock = DockStyle.Fill;
            bottomRightPanel.Controls.Add(checklistCard);

            clbNextSteps = new CheckedListBox
            {
                Location = new Point(10, 45),
                Size = new Size(320, 200)
            };
            clbNextSteps.Items.Add("Complete all section items", true);
            clbNextSteps.Items.Add("Review red flags", true);
            clbNextSteps.Items.Add("Add media evidence", true);
            clbNextSteps.Items.Add("Finalize notes", false);
            clbNextSteps.Items.Add("Complete inspection", false);
            checklistCard.Controls.Add(clbNextSteps);
        }

        private void SetupSystemScoresGrid()
        {
            dgvSystemScores.Columns.Add("SystemNumber", "System #");
            dgvSystemScores.Columns.Add("SystemName", "System Name");

            var scoreColumn = new DataGridViewComboBoxColumn
            {
                Name = "Score",
                HeaderText = "Score (0-5)",
                DataPropertyName = "Score",
                Items = { "0.00", "0.50", "1.00", "1.50", "2.00", "2.50", "3.00", "3.50", "4.00", "4.50", "5.00" }
            };
            dgvSystemScores.Columns.Add(scoreColumn);

            dgvSystemScores.Columns.Add("Weight", "Weight (Std)");
            dgvSystemScores.Columns.Add("WeightedScore", "Weighted Score");

            var redFlagColumn = new DataGridViewCheckBoxColumn
            {
                Name = "RedFlag",
                HeaderText = "Red Flag"
            };
            dgvSystemScores.Columns.Add(redFlagColumn);

            dgvSystemScores.Columns["SystemNumber"].Width = 80;
            dgvSystemScores.Columns["SystemName"].Width = 200;
            dgvSystemScores.Columns["Score"].Width = 120;
            dgvSystemScores.Columns["Weight"].Width = 100;
            dgvSystemScores.Columns["WeightedScore"].Width = 120;
            dgvSystemScores.Columns["RedFlag"].Width = 80;

            // Sample data
            var systems = new[]
            {
                new { Num = 1, Name = "Body & Exterior", Score = "3.50", Weight = "0.30", Weighted = "1.050", Red = false },
                new { Num = 2, Name = "Frame & Structure", Score = "4.00", Weight = "0.10", Weighted = "0.400", Red = false },
                new { Num = 3, Name = "Engine / Powertrain", Score = "4.50", Weight = "0.18", Weighted = "0.810", Red = false },
                new { Num = 4, Name = "Transmission & Driveline", Score = "4.00", Weight = "0.14", Weighted = "0.560", Red = false },
                new { Num = 5, Name = "Suspension & Steering", Score = "3.50", Weight = "0.10", Weighted = "0.350", Red = false },
                new { Num = 6, Name = "Braking System", Score = "3.00", Weight = "0.10", Weighted = "0.300", Red = true },
                new { Num = 7, Name = "Electrical System", Score = "3.50", Weight = "0.10", Weighted = "0.350", Red = true },
                new { Num = 8, Name = "Interior", Score = "4.00", Weight = "0.04", Weighted = "0.160", Red = false },
                new { Num = 9, Name = "Wheels & Tires", Score = "3.50", Weight = "0.06", Weighted = "0.210", Red = false },
                new { Num = 10, Name = "Trim, Glass & Details", Score = "3.00", Weight = "0.02", Weighted = "0.060", Red = false },
                new { Num = 11, Name = "Documentation & Provenance", Score = "4.50", Weight = "0.04", Weighted = "0.180", Red = false },
                new { Num = 12, Name = "Road Test & Functional", Score = "4.00", Weight = "0.06", Weighted = "0.240", Red = false }
            };

            foreach (var sys in systems)
            {
                dgvSystemScores.Rows.Add(sys.Num, sys.Name, sys.Score, sys.Weight, sys.Weighted, sys.Red);
            }

            dgvSystemScores.DataError += (s, e) =>
            {
                // Log or handle invalid cell values gracefully instead of showing the default dialog
                e.ThrowException = false;
            };
        }

        private void SetupSectionSummaryGrid()
        {
            dgvSectionSummary.Columns.Add("Section", "Section");
            dgvSectionSummary.Columns.Add("Score", "Section Score (0-5)");
            dgvSectionSummary.Columns.Add("Items", "Items Scored");
            dgvSectionSummary.Columns.Add("RedFlags", "Red Flags");

            var viewColumn = new DataGridViewButtonColumn
            {
                Name = "View",
                HeaderText = "",
                Text = "View",
                UseColumnTextForButtonValue = true
            };
            dgvSectionSummary.Columns.Add(viewColumn);

            dgvSectionSummary.Columns["Section"].Width = 180;
            dgvSectionSummary.Columns["Score"].Width = 100;
            dgvSectionSummary.Columns["Items"].Width = 80;
            dgvSectionSummary.Columns["RedFlags"].Width = 80;
            dgvSectionSummary.Columns["View"].Width = 60;

            // Sample data
            var sections = new[]
            {
                new { Section = "1. Body & Exterior", Score = "3.50", Items = "6 / 7", Flags = "0" },
                new { Section = "2. Frame & Structure", Score = "4.00", Items = "5 / 6", Flags = "0" },
                new { Section = "3. Engine / Powertrain", Score = "4.50", Items = "7 / 8", Flags = "0" },
                new { Section = "4. Transmission & Driveline", Score = "4.00", Items = "6 / 6", Flags = "0" },
                new { Section = "5. Suspension & Steering", Score = "3.50", Items = "6 / 6", Flags = "0" },
                new { Section = "6. Braking System", Score = "3.00", Items = "5 / 6", Flags = "1" },
                new { Section = "7. Electrical System", Score = "3.50", Items = "6 / 6", Flags = "1" },
                new { Section = "8. Interior", Score = "4.00", Items = "6 / 6", Flags = "0" },
                new { Section = "9. Wheels & Tires", Score = "3.50", Items = "4 / 5", Flags = "0" },
                new { Section = "10. Trim, Glass & Details", Score = "3.00", Items = "5 / 5", Flags = "0" },
                new { Section = "11. Documentation & Provenance", Score = "4.50", Items = "4 / 4", Flags = "0" },
                new { Section = "12. Road Test & Functional", Score = "4.00", Items = "5 / 5", Flags = "0" }
            };

            foreach (var sec in sections)
            {
                dgvSectionSummary.Rows.Add(sec.Section, sec.Score, sec.Items, sec.Flags);
            }
        }

        private void SetupMetricsPanel(Panel panel)
        {
            // Condition Index
            var ciCard = new Panel
            {
                Location = new Point(0, 10),
                Size = new Size(200, 130),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.Add(ciCard);

            var ciTitle = new Label
            {
                Text = "Condition Index (CI)",
                Location = new Point(10, 10),
                Size = new Size(180, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            ciCard.Controls.Add(ciTitle);

            lblConditionIndex = new Label
            {
                Text = "3.83",
                Location = new Point(10, 35),
                Size = new Size(180, 40),
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 212)
            };
            ciCard.Controls.Add(lblConditionIndex);

            lblConditionIndexSubtext = new Label
            {
                Text = "/ 5.00\n(Sum of Weighted Scores ÷ 20)",
                Location = new Point(10, 80),
                Size = new Size(180, 40),
                Font = new Font("Segoe UI", 8)
            };
            ciCard.Controls.Add(lblConditionIndexSubtext);

            // Total Red Flags
            var rfCard = new Panel
            {
                Location = new Point(210, 10),
                Size = new Size(200, 130),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.Add(rfCard);

            var rfTitle = new Label
            {
                Text = "Total Red Flags",
                Location = new Point(10, 10),
                Size = new Size(180, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            rfCard.Controls.Add(rfTitle);

            lblTotalRedFlags = new Label
            {
                Text = "2 🚩",
                Location = new Point(10, 35),
                Size = new Size(180, 40),
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(164, 38, 44)
            };
            rfCard.Controls.Add(lblTotalRedFlags);

            var btnViewRedFlags = new Button
            {
                Text = "View",
                Location = new Point(60, 90),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            rfCard.Controls.Add(btnViewRedFlags);

            // Inspection Progress
            var progressCard = new Panel
            {
                Location = new Point(420, 10),
                Size = new Size(220, 130),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.Add(progressCard);

            var progressTitle = new Label
            {
                Text = "Inspection Progress",
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            progressCard.Controls.Add(progressTitle);

            lblInspectionProgress = new Label
            {
                Text = "75%",
                Location = new Point(10, 35),
                Size = new Size(200, 40),
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(16, 124, 16)
            };
            progressCard.Controls.Add(lblInspectionProgress);

            pbProgress = new ProgressBar
            {
                Location = new Point(10, 85),
                Size = new Size(200, 30),
                Value = 75,
                Style = ProgressBarStyle.Continuous
            };
            progressCard.Controls.Add(pbProgress);
        }

        private Panel CreateStatusBar()
        {
            var statusBar = new Panel
            {
                Height = 30,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            var lblInspectionId = new Label
            {
                Text = $"Inspection ID: {_inspectionId}",
                Location = new Point(10, 7),
                ForeColor = Color.White,
                AutoSize = true
            };
            statusBar.Controls.Add(lblInspectionId);

            var lblCreated = new Label
            {
                Text = "Created: 7/21/2025 9:15 AM by Kent Klingler",
                Location = new Point(350, 7),
                ForeColor = Color.White,
                AutoSize = true
            };
            statusBar.Controls.Add(lblCreated);

            var lblUpdated = new Label
            {
                Text = "Updated: 7/21/2025 10:02 AM",
                Location = new Point(600, 7),
                ForeColor = Color.White,
                AutoSize = true
            };
            statusBar.Controls.Add(lblUpdated);

            var lblSaved = new Label
            {
                Text = "✓ All changes saved",
                Location = new Point(850, 7),
                ForeColor = Color.LightGreen,
                AutoSize = true
            };
            statusBar.Controls.Add(lblSaved);

            return statusBar;
        }

        private void AddLabel(Panel panel, string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            panel.Controls.Add(label);
        }

        private TextBox AddTextBox(Panel panel, int x, int y, int width, bool readOnly = false)
        {
            var textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 25),
                ReadOnly = readOnly,
                BackColor = readOnly ? Color.FromArgb(240, 240, 240) : Color.White
            };
            panel.Controls.Add(textBox);
            return textBox;
        }

        private ComboBox AddComboBox(Panel panel, int x, int y, int width, string[] items)
        {
            var comboBox = new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBox.Items.AddRange(items);
            if (items.Length > 0) comboBox.SelectedIndex = 0;
            panel.Controls.Add(comboBox);
            return comboBox;
        }

        private Label AddValueLabel(Panel panel, int x, int y)
        {
            var label = new Label
            {
                Location = new Point(x, y),
                Size = new Size(340, 20),
                Font = new Font("Segoe UI", 9),
                AutoSize = false
            };
            panel.Controls.Add(label);
            return label;
        }
    }
}