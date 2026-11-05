using ONE_Classics_Nexus.WinForms.Forms.Dashboard;
using ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Forms.Clients;
using ONE_Classics_Nexus.WinForms.Forms.Vehicles;
using ONE_Classics_Nexus.WinForms.Forms.Appraisals;
using ONE_Classics_Nexus.WinForms.Forms.Inspections;
using ONE_Classics_Nexus.WinForms.Services;
using ONE_Classics_Nexus.WinForms.Theming;
using ONE_Classics_Nexus.API.Contracts.Appraisals;
using System.Reflection;

namespace ONE_Classics_Nexus.WinForms.Forms.Shell
{
    public partial class MainShellForm : NexusBaseForm
    {
        private readonly IFormFactory _formFactory;
        private readonly IAppraisalApiService _appraisalApiService;
        private readonly IInspectionApiService _inspectionApiService;
        private readonly IVehicleApiService _vehicleApiService;

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
        private Button btnAppraisalRequests = null!;
        private Button btnClients = null!;
        private Button btnVehicles = null!;
        private Button btnAppraisals = null!;
        private Button btnInspections = null!;
        private Button btnComparables = null!;
        private Button btnValuations = null!;
        private Button btnReports = null!;
        private Button btnAdmin = null!;

        private Form? _currentChildForm;
        private Control? _appraisalsHomeControl;

        private object? _shellStatusBridgeSource;
        private EventInfo? _shellStatusBridgeEvent;
        private Delegate? _shellStatusBridgeHandler;

        public MainShellForm()
        {
            _formFactory = null!;
            _appraisalApiService = null!;
            _inspectionApiService = null!;
            _vehicleApiService = null!;

            InitializeComponent();
            Text = "ONE Classics";
            Name = nameof(MainShellForm);
        }

        public MainShellForm(
            IFormFactory formFactory,
            IAppraisalApiService appraisalApiService,
            IInspectionApiService inspectionApiService,
            IVehicleApiService vehicleApiService)
        {
            _formFactory = formFactory ?? throw new ArgumentNullException(nameof(formFactory));
            _appraisalApiService = appraisalApiService ?? throw new ArgumentNullException(nameof(appraisalApiService));
            _inspectionApiService = inspectionApiService ?? throw new ArgumentNullException(nameof(inspectionApiService));
            _vehicleApiService = vehicleApiService ?? throw new ArgumentNullException(nameof(vehicleApiService));

            InitializeComponent();

            Text = "ONE Classics";
            Name = "MainShellForm";
            WindowState = FormWindowState.Maximized;

            BuildMainShell();
            SetActiveNav(btnDashboard);
            ShowDashboard();
            ResetShellChromeForModule(
                "Dashboard",
                "Unified workspace for intake, inspection, valuation, and reporting.",
                "Ready | API Connected | No blocking validation errors");
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
                WrapContents = true,
                AutoSize = true,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = Color.Transparent
            };

            lblAppName = new Label
            {
                AutoSize = true,
                Font = NexusTheme.BodyFont,
                // Font = NexusTheme.MetaFont,
                ForeColor = NexusTheme.TextMuted,
                Text = "ONE Classics",
                Margin = new Padding(0)
            };

            lblModuleTitle = new Label
            {
                AutoSize = true,
                // Font = NexusTheme.H1Font,
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.Primary,
                Text = "lblModuleTitle1-Dashboard",
                Margin = new Padding(0)
            };

            lblContext = new Label
            {
                AutoSize = true,
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.Text,
                //Text = "Unified workspace for intake, inspection, valuation, and reporting.",
                Text = "Unified workspace",
                Margin = new Padding(0)
            };

            //titleStack.Controls.Add(lblAppName);
            titleStack.Controls.Add(lblModuleTitle);
            //titleStack.Controls.Add(lblContext);

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
                Padding = new Padding(20)
            };

            var lblBrandTop = new Label
            {
                AutoSize = true,
                Text = "ONE Classics",
                Font = NexusTheme.LabelFont,
                ForeColor = NexusTheme.White,
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
                // Font = NexusTheme.MetaFont,
                Font = NexusTheme.LabelFont,
                ForeColor = NexusTheme.White,
                Location = new Point(20, 74)
            };

            brandPanel.Controls.Add(lblBrandTop);
            brandPanel.Controls.Add(lblBrandName);
            //brandPanel.Controls.Add(lblBrandMeta);

            navFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                // Padding = new Padding(10),
                Padding = new Padding(16),
                AutoScroll = true,
                BackColor = NexusTheme.Surface
            };

            btnDashboard = CreateNavButton("Dashboard", true);
            btnAppraisalRequests = CreateNavButton("Appraisal Requests");
            btnClients = CreateNavButton("Clients");
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
                btnAppraisalRequests,
                btnClients,
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
                // Padding = new Padding(16, 12, 16, 16),
                Padding = new Padding(10, 12, 16, 10),
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
            //currentContextCard.Controls.Add(lblCurrentContextValue);
            //currentContextCard.Controls.Add(lblCurrentContextMeta);
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
            if (sender is not Button clicked)
                return;

            SetActiveNav(clicked);

            switch (clicked.Text)
            {
                case "Dashboard":
                    ShowDashboard();
                    ResetShellChromeForModule(
                        "Dashboard",
                        "Unified workspace for intake, inspection, valuation, and reporting.",
                        "Dashboard - Ready");
                    break;

                case "Appraisal Request":
                    ShowAppraisalRequestWorkspace();
                    break;

                case "Clients":
                    ShowClientWorkspace();
                    break;

                case "Vehicles":
                    var vehicleListForm = _formFactory.CreateForm<VehicleListForm>();
                    vehicleListForm.VehicleOpenRequested += VehicleListForm_VehicleOpenRequested;
                    LoadChildForm(vehicleListForm);
                    ResetShellChromeForModule(
                        "Vehicles",
                        "Vehicle list workspace loaded.",
                        "Vehicles - Ready");
                    break;

                case "Appraisals":
                    LoadChildForm(_formFactory.CreateForm<AppraisalWorkspaceForm>());
                    ResetShellChromeForModule(
                        "Appraisals",
                        "Start a new appraisal or open an existing appraisal record.",
                        "Appraisals - Ready");
                    break;

                case "Inspections":
                    {
                        var inspectionId = Guid.Parse("47d77c4b-c10c-4c8c-bcac-0bd106251f43");
                        var workspaceForm = new InspectionWorkspaceForm(
                            _inspectionApiService,
                            _appraisalApiService,
                            _vehicleApiService);
                        LoadChildForm(workspaceForm);
                        _ = workspaceForm.LoadInspectionAsync(inspectionId);
                        ResetShellChromeForModule(
                            "Inspections",
                            $"Inspection {inspectionId:D}",
                            "Inspection workspace loaded");
                    }
                    break;

                case "Comparables":
                    ShowComingSoon("Comparables");
                    ResetShellChromeForModule(
                        "Comparables",
                        "Comparables module coming soon.",
                        "Comparables - Coming Soon");
                    break;

                case "Valuations":
                    ShowComingSoon("Valuations");
                    ResetShellChromeForModule(
                        "Valuations",
                        "Valuations module coming soon.",
                        "Valuations - Coming Soon");
                    break;

                case "Reports":
                    ShowComingSoon("Reports");
                    ResetShellChromeForModule(
                        "Reports",
                        "Reports module coming soon.",
                        "Reports - Coming Soon");
                    break;

                case "Admin":
                    ShowComingSoon("Admin");
                    ResetShellChromeForModule(
                        "Admin",
                        "Admin module coming soon.",
                        "Admin - Coming Soon");
                    break;

                default:
                    ShowDashboard();
                    ResetShellChromeForModule(
                        "Dashboard",
                        "Unknown module selected.",
                        "Ready");
                    break;
            }
        }

        private void SetActiveNav(Button activeButton)
        {
            foreach (Control control in navFlow.Controls)
            {
                if (control is Button button)
                    ApplyNavVisual(button, ReferenceEquals(button, activeButton));
            }
        }

        private void ApplyNavVisual(Button button, bool active)
        {
            button.BackColor = active ? NexusTheme.Primary : NexusTheme.SurfaceAlt;
            button.ForeColor = active ? NexusTheme.White : NexusTheme.TextStrong;
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

        private void ShowDashboard()
        {
            var dashboard = _formFactory.CreateForm<DashboardForm>();
            dashboard.NewAppraisalRequested += Dashboard_NewAppraisalRequested;
            dashboard.AppraisalRequestWorkspaceRequested += Dashboard_AppraisalRequestWorkspaceRequested;
            dashboard.AppraisalRequested += Dashboard_AppraisalRequested;
            LoadChildForm(dashboard);
        }

        private void Dashboard_AppraisalRequestWorkspaceRequested(object? sender, EventArgs e)
        {
            SetActiveNav(btnAppraisalRequests);
            ShowAppraisalRequestWorkspace();
        }

        private void ShowAppraisalRequestWorkspace()
        {
            var appraisalRequestForm = _formFactory.CreateForm<AppraisalRequestInformationForm>();
            LoadChildForm(appraisalRequestForm);

            ResetShellChromeForModule(
                "Appraisal Requests",
                "New request information workspace loaded.",
                "Appraisal Requests - Ready");
        }
        private void ShowClientWorkspace()
        {
            var clientForm = _formFactory.CreateForm<ClientWorkspaceForm>();
            LoadChildForm(clientForm);

            ResetShellChromeForModule(
                "Clients",
                "New client information workspace loaded.",
                "Clients - Ready");
        }

        private void VehicleListForm_VehicleOpenRequested(object? sender, Guid vehicleId)
        {
            var detailForm = _formFactory.CreateForm<VehicleDetailForm>(vehicleId);
            LoadChildForm(detailForm);

            ResetShellChromeForModule(
                "Vehicles",
                $"Vehicle {vehicleId:D}",
                "Vehicle detail loaded");
        }

        private async void Dashboard_AppraisalRequested(object? sender, Guid appraisalId)
        {
            var workspace = _formFactory.CreateForm<AppraisalWorkspaceForm>();
            LoadChildForm(workspace);
            await workspace.LoadAppraisalAsync(appraisalId);
            SetActiveNav(btnAppraisals);
            ResetShellChromeForModule(
                "Appraisals",
                $"Appraisal {appraisalId:D}",
                "Appraisal workspace loaded");
        }

        private void Dashboard_NewAppraisalRequested(object? sender, EventArgs e)
        {
            SetActiveNav(btnAppraisals);
            ShowAppraisalsHome();
            ResetShellChromeForModule(
                "Appraisals",
                "Start a new appraisal or open an existing appraisal record.",
                "Appraisals - Ready");
        }

        private void ShowAppraisalsHome()
        {
            if (_appraisalsHomeControl is null || _appraisalsHomeControl.IsDisposed)
                _appraisalsHomeControl = BuildAppraisalsHome();

            LoadChildForm(_appraisalsHomeControl);
        }

        private Control BuildAppraisalsHome()
        {
            var root = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.SurfaceAlt,
                Name = "appraisalsHomePanel"
            };

            var stack = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };

            var title = new Label
            {
                AutoSize = true,
                Text = "Appraisals",
                Font = NexusTheme.H1Font,
                ForeColor = NexusTheme.Primary,
                Margin = new Padding(0, 0, 0, 8)
            };

            var subtitle = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(980, 0),
                Text = "Open an existing appraisal by identifier, or create a new appraisal from a subject vehicle identifier.",
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.TextMuted,
                Margin = new Padding(0, 0, 0, 18)
            };

            var cards = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Width = 980,
                Height = 255,
                Margin = new Padding(0, 0, 0, 18),
                BackColor = Color.Transparent
            };
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            cards.Controls.Add(BuildOpenExistingAppraisalCard(), 0, 0);
            cards.Controls.Add(BuildCreateAppraisalCard(), 1, 0);

            var notePanel = new Panel
            {
                Width = 980,
                Height = 92,
                BackColor = NexusTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18),
                Margin = new Padding(0)
            };

            var noteTitle = new Label
            {
                AutoSize = true,
                Text = "Module Note",
                Font = NexusTheme.LabelFont,
                ForeColor = NexusTheme.Primary,
                Location = new Point(18, 16)
            };

            var noteBody = new Label
            {
                AutoSize = false,
                Size = new Size(930, 40),
                Text = "The shell currently hosts an Appraisals landing page because the API does not yet expose a browse/list endpoint for appraisal records. The workspace opens only after a concrete appraisal context exists.",
                Font = NexusTheme.MetaFont,
                ForeColor = NexusTheme.TextMuted,
                Location = new Point(18, 40)
            };

            notePanel.Controls.Add(noteTitle);
            notePanel.Controls.Add(noteBody);

            stack.Controls.Add(title);
            stack.Controls.Add(subtitle);
            stack.Controls.Add(cards);
            stack.Controls.Add(notePanel);

            root.Controls.Add(stack);
            return root;
        }

        private Panel BuildOpenExistingAppraisalCard()
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18),
                Margin = new Padding(0, 0, 12, 0)
            };

            var title = new Label
            {
                AutoSize = true,
                Text = "Open Existing Appraisal",
                Font = NexusTheme.H3Font,
                ForeColor = NexusTheme.Primary,
                Location = new Point(18, 16)
            };

            var subtitle = new Label
            {
                AutoSize = false,
                Size = new Size(410, 42),
                Text = "Enter the appraisal GUID to load the workspace for an existing appraisal record.",
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.TextMuted,
                Location = new Point(18, 48)
            };

            var inputLabel = new Label
            {
                AutoSize = true,
                Text = "Appraisal ID",
                Font = NexusTheme.LabelFont,
                ForeColor = NexusTheme.TextStrong,
                Location = new Point(18, 104)
            };

            var appraisalIdTextBox = new TextBox
            {
                Width = 410,
                Font = NexusTheme.BodyFont,
                Location = new Point(18, 130)
            };

            var button = NexusButtonFactory.CreatePrimary("Open Appraisal");
            button.AutoSize = false;
            button.Size = new Size(160, 40);
            button.Location = new Point(18, 178);
            button.Click += async (_, _) => await OpenAppraisalByIdAsync(appraisalIdTextBox.Text);

            card.Controls.Add(title);
            card.Controls.Add(subtitle);
            card.Controls.Add(inputLabel);
            card.Controls.Add(appraisalIdTextBox);
            card.Controls.Add(button);

            return card;
        }

        private Panel BuildCreateAppraisalCard()
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18),
                Margin = new Padding(12, 0, 0, 0)
            };

            var title = new Label
            {
                AutoSize = true,
                Text = "Create New Appraisal",
                Font = NexusTheme.H3Font,
                ForeColor = NexusTheme.Primary,
                Location = new Point(18, 16)
            };

            var subtitle = new Label
            {
                AutoSize = false,
                Size = new Size(410, 42),
                Text = "Enter the subject vehicle GUID to create a new appraisal, then open the appraisal workspace.",
                Font = NexusTheme.BodyFont,
                ForeColor = NexusTheme.TextMuted,
                Location = new Point(18, 48)
            };

            var inputLabel = new Label
            {
                AutoSize = true,
                Text = "Vehicle ID",
                Font = NexusTheme.LabelFont,
                ForeColor = NexusTheme.TextStrong,
                Location = new Point(18, 104)
            };

            var vehicleIdTextBox = new TextBox
            {
                Width = 410,
                Font = NexusTheme.BodyFont,
                Location = new Point(18, 130)
            };

            var button = NexusButtonFactory.CreatePrimary("Create Appraisal");
            button.AutoSize = false;
            button.Size = new Size(170, 40);
            button.Location = new Point(18, 178);
            button.Click += async (_, _) => await CreateAndOpenAppraisalAsync(vehicleIdTextBox.Text);

            card.Controls.Add(title);
            card.Controls.Add(subtitle);
            card.Controls.Add(inputLabel);
            card.Controls.Add(vehicleIdTextBox);
            card.Controls.Add(button);

            return card;
        }

        private async Task OpenAppraisalWorkspaceAsync(Guid appraisalId)
        {
            SetActiveNav(btnAppraisals);

            var appraisalForm = _formFactory.CreateForm<AppraisalWorkspaceForm>();
            LoadAppraisalWorkspaceForm(appraisalForm);

            await appraisalForm.LoadAppraisalAsync(appraisalId);

            if (!ReferenceEquals(_shellStatusBridgeSource, appraisalForm))
            {
                lblModuleTitle.Text = "lblModuleTitle2-Appraisals";
                lblContext.Text = $"Appraisal {appraisalId:D}";
                SetStatus("Appraisal workspace loaded");
                lblSaveState.Text = "All Changes Saved";
                lblSaveState.BackColor = NexusTheme.Success;
                lblSaveState.ForeColor = NexusTheme.White;
            }
        }

        private async Task OpenAppraisalByIdAsync(string? appraisalIdText)
        {
            if (!TryParseGuid(appraisalIdText, "Appraisal ID", out var appraisalId, out var errorMessage))
            {
                MessageBox.Show(
                    this,
                    errorMessage ?? "Appraisal ID is invalid.",
                    "Open Appraisal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ResetShellChromeForModule(
                    "Appraisals",
                    "Validating appraisal record...",
                    "Appraisals - Validating");

                var appraisal = await _appraisalApiService.GetByIdAsync(appraisalId);

                if (appraisal is null)
                {
                    ResetShellChromeForModule(
                        "Appraisals",
                        "Start a new appraisal or open an existing appraisal record.",
                        "Appraisal not found");

                    MessageBox.Show(
                        this,
                        $"No appraisal was found for ID {appraisalId:D}.",
                        "Open Appraisal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                await OpenAppraisalWorkspaceAsync(appraisal.AppraisalId);
            }
            catch (Exception ex)
            {
                ResetShellChromeForModule(
                    "Appraisals",
                    "Start a new appraisal or open an existing appraisal record.",
                    "Appraisal lookup failed");

                MessageBox.Show(
                    this,
                    ex.Message,
                    "Open Appraisal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async Task CreateAndOpenAppraisalAsync(string? vehicleIdText)
        {
            if (!TryParseGuid(vehicleIdText, "Vehicle ID", out var vehicleId, out var errorMessage))
            {
                MessageBox.Show(
                    this,
                    errorMessage ?? "Vehicle ID is invalid.",
                    "Create Appraisal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ResetShellChromeForModule(
                    "Appraisals",
                    "Creating a new appraisal from the supplied vehicle...",
                    "Appraisals - Creating");

                var created = await _appraisalApiService.CreateAsync(
                    new CreateAppraisalRequest(
                        vehicleId,
                        null,
                        null,
                        null,
                        null,
                        null,
                        "Standard",
                        "Draft",
                        "Open",
                        "NotStarted",
                        "NotStarted",
                        "NotStarted",
                        null));

                await OpenAppraisalWorkspaceAsync(created.AppraisalId);
            }
            catch (Exception ex)
            {
                ResetShellChromeForModule(
                    "Appraisals",
                    "Start a new appraisal or open an existing appraisal record.",
                    "Appraisal create failed");

                MessageBox.Show(
                    this,
                    ex.Message,
                    "Create Appraisal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static bool TryParseGuid(
            string? text,
            string fieldName,
            out Guid value,
            out string? errorMessage)
        {
            value = Guid.Empty;
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(text))
            {
                errorMessage = $"{fieldName} is required.";
                return false;
            }

            if (!Guid.TryParse(text.Trim(), out value) || value == Guid.Empty)
            {
                errorMessage = $"{fieldName} must be a valid non-empty GUID.";
                value = Guid.Empty;
                return false;
            }

            return true;
        }

        private void ShowComingSoon(string moduleName)
        {
            var comingSoonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = NexusTheme.Surface,
                Name = "comingSoonPanel"
            };

            var label = new Label
            {
                Text = $"{moduleName} Module\n\nComing Soon...",
                Font = NexusTheme.H1Font,
                ForeColor = NexusTheme.TextMuted,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            comingSoonPanel.Controls.Add(label);
            LoadChildForm(comingSoonPanel);
        }

        private void LoadAppraisalWorkspaceForm(AppraisalWorkspaceForm appraisalForm)
        {
            if (appraisalForm is null)
                throw new ArgumentNullException(nameof(appraisalForm));

            lblModuleTitle.Text = "lblModuleTitle3-Appraisals";
            lblContext.Text = "Appraisal workspace";
            SetStatus("Appraisal workspace loading...");
            lblSaveState.Text = "Working...";
            lblSaveState.BackColor = NexusTheme.Warning;
            lblSaveState.ForeColor = NexusTheme.White;

            LoadChildForm(appraisalForm);
        }

        private void WireChildStatusBridge(Control childControl)
        {
            if (childControl is not AppraisalWorkspaceForm)
                return;

            var eventInfo = childControl.GetType().GetEvent("ShellStatusChanged", BindingFlags.Instance | BindingFlags.Public);
            if (eventInfo?.EventHandlerType is null)
                return;

            var handlerMethod = GetType().GetMethod(
                nameof(AppraisalWorkspaceForm_ShellStatusChanged),
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (handlerMethod is null)
                return;

            var handler = Delegate.CreateDelegate(
                eventInfo.EventHandlerType,
                this,
                handlerMethod,
                false);

            if (handler is null)
                return;

            eventInfo.AddEventHandler(childControl, handler);

            _shellStatusBridgeSource = childControl;
            _shellStatusBridgeEvent = eventInfo;
            _shellStatusBridgeHandler = handler;
        }

        private void UnwireChildStatusBridge(Control childControl)
        {
            if (_shellStatusBridgeSource is null
                || _shellStatusBridgeEvent is null
                || _shellStatusBridgeHandler is null)
            {
                return;
            }

            if (!ReferenceEquals(_shellStatusBridgeSource, childControl))
                return;

            _shellStatusBridgeEvent.RemoveEventHandler(_shellStatusBridgeSource, _shellStatusBridgeHandler);
            _shellStatusBridgeSource = null;
            _shellStatusBridgeEvent = null;
            _shellStatusBridgeHandler = null;
        }

        private void AppraisalWorkspaceForm_ShellStatusChanged(object? sender, EventArgs e)
        {
            ApplyAppraisalWorkspaceShellChrome(e);
            ApplyAppraisalShellSaveState(e);
        }

        private void ApplyAppraisalWorkspaceShellChrome(EventArgs e)
        {
            var displayNumber = ReadEventText(e, "DisplayNumber");
            var contextText = ReadEventText(e, "ContextText");
            var statusText = ReadEventText(e, "StatusText");
            var activeSection = ReadEventText(e, "ActiveSection");

            if (string.IsNullOrWhiteSpace(contextText))
            {
                if (!string.IsNullOrWhiteSpace(displayNumber) && !string.IsNullOrWhiteSpace(activeSection))
                    contextText = $"Appraisal {displayNumber} • {activeSection}";
                else if (!string.IsNullOrWhiteSpace(displayNumber))
                    contextText = $"Appraisal {displayNumber}";
                else
                    contextText = "Appraisal workspace";
            }

            lblModuleTitle.Text = "lblModuleTitle4-Appraisals";
            lblContext.Text = contextText;
            SetStatus(string.IsNullOrWhiteSpace(statusText) ? "Appraisal workspace loaded" : statusText);
        }

        private void ApplyAppraisalShellSaveState(EventArgs e)
        {
            var isBusy = ReadEventFlag(e, "IsBusy");
            var hasError = ReadEventFlag(e, "HasError");
            var hasUnsavedChanges = ReadEventFlag(e, "HasUnsavedChanges");

            if (hasError)
            {
                lblSaveState.Text = "Attention Required";
                lblSaveState.BackColor = NexusTheme.Danger;
                lblSaveState.ForeColor = NexusTheme.White;
                return;
            }

            if (isBusy)
            {
                lblSaveState.Text = "Working...";
                lblSaveState.BackColor = NexusTheme.Warning;
                lblSaveState.ForeColor = NexusTheme.White;
                return;
            }

            if (hasUnsavedChanges)
            {
                lblSaveState.Text = "Unsaved Changes";
                lblSaveState.BackColor = NexusTheme.Warning;
                lblSaveState.ForeColor = NexusTheme.White;
                return;
            }

            lblSaveState.Text = "All Changes Saved";
            lblSaveState.BackColor = NexusTheme.Success;
            lblSaveState.ForeColor = NexusTheme.White;
        }

        private void ResetShellChromeForModule(
            string moduleTitle,
            string contextText,
            string statusText)
        {
            lblModuleTitle.Text = moduleTitle;
            lblContext.Text = contextText;
            lblSaveState.Text = "All Changes Saved";
            lblSaveState.BackColor = NexusTheme.Success;
            lblSaveState.ForeColor = NexusTheme.White;
            SetStatus(statusText);
        }

        private static string? ReadEventText(EventArgs e, string propertyName)
        {
            var property = e.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            var value = property?.GetValue(e);
            return value?.ToString();
        }

        private static bool ReadEventFlag(EventArgs e, string propertyName)
        {
            var property = e.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            var value = property?.GetValue(e);
            return value is bool flag && flag;
        }

        private void LoadChildForm(Control childControl)
        {
            if (_currentChildForm is not null)
                UnwireChildStatusBridge(_currentChildForm);

            DisposeCurrentChildForm();

            contentPanel.Controls.Clear();

            if (childControl is Form childForm)
            {
                _currentChildForm = childForm;
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                WireChildStatusBridge(childForm);

                contentPanel.Controls.Add(childForm);
                childForm.Show();
            }
            else
            {
                childControl.Dock = DockStyle.Fill;
                contentPanel.Controls.Add(childControl);
            }
        }

        private void DisposeCurrentChildForm()
        {
            if (_currentChildForm is null)
                return;

            UnwireChildStatusBridge(_currentChildForm);
            _currentChildForm.Close(); // already disposes since TopLevel = false
            _currentChildForm = null;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DisposeCurrentChildForm();
            base.OnFormClosing(e);
        }

        private void lblBrand_Click(object sender, EventArgs e)
        {

        }
    }
}
