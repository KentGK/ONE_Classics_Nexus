#nullable enable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

partial class AppraisalRequestWorkspaceForm
{
    private IContainer? components = null;

    private Panel pnlTitle = null!;
    private Panel pnlFilterBar = null!;
    private Panel pnlListSummaryBar = null!;
    private Label lblTitleIcon = null!;
    private Label lblTitle = null!;
    private Label lblDirty = null!;

    private TextBox txtSearch = null!;
    private ComboBox cmbStatusFilter = null!;
    private ComboBox cmbClientFilter = null!;
    private ComboBox cmbDateFilter = null!;
    private ComboBox cmbSort = null!;
    private DateTimePicker dtpRequestedFrom = null!;
    private DateTimePicker dtpRequestedTo = null!;
    private Button btnRefresh = null!;
    private Button btnNew = null!;
    private Label lblResultCount = null!;
    private Label lblPageSummary = null!;
    private Button btnPreviousPage = null!;
    private Button btnNextPage = null!;

    private DataGridView dgvRequests = null!;
    private DataGridViewTextBoxColumn colStatus = null!;

    private TabControl tabWorkspace = null!;
    private TabPage tabRequestInformation = null!;
    private TabPage tabValidation = null!;
    private TabPage tabIntendedUsers = null!;
    private TabPage tabAttachments = null!;
    private TabPage tabHistory = null!;

    private Label lblRequestNumber = null!;
    private Label lblStatus = null!;
    private Label lblRowVersion = null!;
    private Label lblAudit = null!;

    private ComboBox cmbClient = null!;
    private ComboBox cmbPrimaryContact = null!;
    private ComboBox cmbIntendedUse = null!;
    private TextBox txtIntendedUseNarrative = null!;
    private DateTimePicker dtpEffectiveDate = null!;
    private ComboBox cmbValueDefinition = null!;
    private ComboBox cmbAppraisalType = null!;
    private ComboBox cmbPriority = null!;
    private TextBox txtScopeSummary = null!;
    private TextBox txtVehicleSummary = null!;

    private Button btnSelectVehicle = null!;
    private Button btnAddIntendedUser = null!;
    private Button btnEditIntendedUser = null!;
    private Button btnDeactivateIntendedUser = null!;
    private Button btnAddAttachment = null!;

    private Button btnSave = null!;
    private Button btnCancelEdit = null!;
    private Button btnValidate = null!;
    private Button btnSubmit = null!;
    private Button btnDelete = null!;
    private Button btnMoreActions = null!;

    private Label lblValidationSummary = null!;
    private DataGridView dgvValidation = null!;
    private DataGridView dgvIntendedUsers = null!;
    private DataGridView dgvAttachments = null!;
    private DataGridView dgvHistory = null!;

    private ProgressBar prgBusy = null!;
    private Label lblWorkspaceStatus = null!;

    private ContextMenuStrip cmsMoreActions = null!;
    private ToolStripMenuItem mnuCancelRequest = null!;
    private ToolStripMenuItem mnuWithdrawRequest = null!;
    private ToolStripMenuItem mnuDeleteDraft = null!;

    private ErrorProvider errorProvider = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        pnlTitle = new Panel();
        lblTitleIcon = new Label();
        lblTitle = new Label();
        lblDirty = new Label();
        pnlFilterBar = new Panel();
        txtSearch = new TextBox();
        cmbStatusFilter = new ComboBox();
        cmbClientFilter = new ComboBox();
        cmbDateFilter = new ComboBox();
        cmbSort = new ComboBox();
        dtpRequestedFrom = new DateTimePicker();
        dtpRequestedTo = new DateTimePicker();
        btnRefresh = new Button();
        btnNew = new Button();
        pnlListSummaryBar = new Panel();
        lblResultCount = new Label();
        lblPageSummary = new Label();
        btnPreviousPage = new Button();
        btnNextPage = new Button();
        dgvRequests = new DataGridView();
        colStatus = new DataGridViewTextBoxColumn();
        tabWorkspace = new TabControl();
        tabRequestInformation = new TabPage();
        lblRequestNumber = new Label();
        lblStatus = new Label();
        lblRowVersion = new Label();
        lblAudit = new Label();
        cmbClient = new ComboBox();
        cmbPrimaryContact = new ComboBox();
        cmbIntendedUse = new ComboBox();
        txtIntendedUseNarrative = new TextBox();
        dtpEffectiveDate = new DateTimePicker();
        cmbValueDefinition = new ComboBox();
        cmbAppraisalType = new ComboBox();
        cmbPriority = new ComboBox();
        txtScopeSummary = new TextBox();
        txtVehicleSummary = new TextBox();
        btnSelectVehicle = new Button();
        btnAddIntendedUser = new Button();
        btnEditIntendedUser = new Button();
        btnDeactivateIntendedUser = new Button();
        btnAddAttachment = new Button();
        btnSave = new Button();
        btnCancelEdit = new Button();
        btnValidate = new Button();
        btnSubmit = new Button();
        btnDelete = new Button();
        btnMoreActions = new Button();
        tabValidation = new TabPage();
        lblValidationSummary = new Label();
        dgvValidation = new DataGridView();
        tabIntendedUsers = new TabPage();
        dgvIntendedUsers = new DataGridView();
        tabAttachments = new TabPage();
        dgvAttachments = new DataGridView();
        tabHistory = new TabPage();
        dgvHistory = new DataGridView();
        prgBusy = new ProgressBar();
        lblWorkspaceStatus = new Label();
        cmsMoreActions = new ContextMenuStrip(components);
        mnuCancelRequest = new ToolStripMenuItem();
        mnuWithdrawRequest = new ToolStripMenuItem();
        mnuDeleteDraft = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        pnlTitle.SuspendLayout();
        pnlFilterBar.SuspendLayout();
        pnlListSummaryBar.SuspendLayout();
        ((ISupportInitialize)dgvRequests).BeginInit();
        tabWorkspace.SuspendLayout();
        tabRequestInformation.SuspendLayout();
        tabValidation.SuspendLayout();
        ((ISupportInitialize)dgvValidation).BeginInit();
        tabIntendedUsers.SuspendLayout();
        ((ISupportInitialize)dgvIntendedUsers).BeginInit();
        tabAttachments.SuspendLayout();
        ((ISupportInitialize)dgvAttachments).BeginInit();
        tabHistory.SuspendLayout();
        ((ISupportInitialize)dgvHistory).BeginInit();
        cmsMoreActions.SuspendLayout();
        ((ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // pnlTitle
        // 
        pnlTitle.Controls.Add(lblTitleIcon);
        pnlTitle.Controls.Add(lblTitle);
        pnlTitle.Controls.Add(lblDirty);
        pnlTitle.Location = new Point(12, 12);
        pnlTitle.Name = "pnlTitle";
        pnlTitle.Size = new Size(1260, 44);
        pnlTitle.TabIndex = 0;
        // 
        // lblTitleIcon
        // 
        lblTitleIcon.AutoSize = true;
        lblTitleIcon.Location = new Point(8, 13);
        lblTitleIcon.Name = "lblTitleIcon";
        lblTitleIcon.Size = new Size(14, 15);
        lblTitleIcon.TabIndex = 0;
        lblTitleIcon.Text = "#";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTitle.Location = new Point(28, 11);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(209, 19);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Appraisal Request Workspace";
        // 
        // lblDirty
        // 
        lblDirty.AutoSize = true;
        lblDirty.ForeColor = Color.FromArgb(183, 28, 49);
        lblDirty.Location = new Point(256, 13);
        lblDirty.Name = "lblDirty";
        lblDirty.Size = new Size(80, 15);
        lblDirty.TabIndex = 2;
        lblDirty.Text = "Unsaved edits";
        lblDirty.Visible = false;
        // 
        // pnlFilterBar
        // 
        pnlFilterBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlFilterBar.Controls.Add(txtSearch);
        pnlFilterBar.Controls.Add(cmbStatusFilter);
        pnlFilterBar.Controls.Add(cmbClientFilter);
        pnlFilterBar.Controls.Add(cmbDateFilter);
        pnlFilterBar.Controls.Add(cmbSort);
        pnlFilterBar.Controls.Add(dtpRequestedFrom);
        pnlFilterBar.Controls.Add(dtpRequestedTo);
        pnlFilterBar.Controls.Add(btnRefresh);
        pnlFilterBar.Controls.Add(btnNew);
        pnlFilterBar.Location = new Point(12, 68);
        pnlFilterBar.Name = "pnlFilterBar";
        pnlFilterBar.Size = new Size(1260, 25);
        pnlFilterBar.TabIndex = 1;
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(0, 0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search requests";
        txtSearch.Size = new Size(220, 23);
        txtSearch.TabIndex = 1;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // cmbStatusFilter
        // 
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatusFilter.Location = new Point(226, 0);
        cmbStatusFilter.Name = "cmbStatusFilter";
        cmbStatusFilter.Size = new Size(150, 23);
        cmbStatusFilter.TabIndex = 2;
        cmbStatusFilter.SelectedIndexChanged += FilterControl_Changed;
        // 
        // cmbClientFilter
        // 
        cmbClientFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbClientFilter.Location = new Point(382, 0);
        cmbClientFilter.Name = "cmbClientFilter";
        cmbClientFilter.Size = new Size(170, 23);
        cmbClientFilter.TabIndex = 3;
        cmbClientFilter.SelectedIndexChanged += FilterControl_Changed;
        // 
        // cmbDateFilter
        // 
        cmbDateFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDateFilter.Location = new Point(558, 0);
        cmbDateFilter.Name = "cmbDateFilter";
        cmbDateFilter.Size = new Size(130, 23);
        cmbDateFilter.TabIndex = 4;
        cmbDateFilter.SelectedIndexChanged += FilterControl_Changed;
        // 
        // cmbSort
        // 
        cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSort.Location = new Point(694, 0);
        cmbSort.Name = "cmbSort";
        cmbSort.Size = new Size(150, 23);
        cmbSort.TabIndex = 5;
        cmbSort.SelectedIndexChanged += FilterControl_Changed;
        // 
        // dtpRequestedFrom
        // 
        dtpRequestedFrom.Format = DateTimePickerFormat.Short;
        dtpRequestedFrom.Location = new Point(850, 0);
        dtpRequestedFrom.Name = "dtpRequestedFrom";
        dtpRequestedFrom.Size = new Size(120, 23);
        dtpRequestedFrom.TabIndex = 6;
        dtpRequestedFrom.ValueChanged += FilterControl_Changed;
        // 
        // dtpRequestedTo
        // 
        dtpRequestedTo.Format = DateTimePickerFormat.Short;
        dtpRequestedTo.Location = new Point(976, 0);
        dtpRequestedTo.Name = "dtpRequestedTo";
        dtpRequestedTo.Size = new Size(120, 23);
        dtpRequestedTo.TabIndex = 7;
        dtpRequestedTo.ValueChanged += FilterControl_Changed;
        // 
        // btnRefresh
        // 
        btnRefresh.Location = new Point(1102, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(74, 25);
        btnRefresh.TabIndex = 8;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnNew
        // 
        btnNew.Location = new Point(1182, 0);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(78, 25);
        btnNew.TabIndex = 9;
        btnNew.Text = "New";
        btnNew.UseVisualStyleBackColor = true;
        btnNew.Click += btnNew_Click;
        // 
        // pnlListSummaryBar
        // 
        pnlListSummaryBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlListSummaryBar.Controls.Add(lblResultCount);
        pnlListSummaryBar.Controls.Add(lblPageSummary);
        pnlListSummaryBar.Controls.Add(btnPreviousPage);
        pnlListSummaryBar.Controls.Add(btnNextPage);
        pnlListSummaryBar.Location = new Point(12, 96);
        pnlListSummaryBar.Name = "pnlListSummaryBar";
        pnlListSummaryBar.Size = new Size(1260, 27);
        pnlListSummaryBar.TabIndex = 2;
        // 
        // lblResultCount
        // 
        lblResultCount.AutoSize = true;
        lblResultCount.Location = new Point(0, 4);
        lblResultCount.Name = "lblResultCount";
        lblResultCount.Size = new Size(95, 15);
        lblResultCount.TabIndex = 10;
        lblResultCount.Text = "0 requests found";
        // 
        // lblPageSummary
        // 
        lblPageSummary.AutoSize = true;
        lblPageSummary.Location = new Point(1148, 4);
        lblPageSummary.Name = "lblPageSummary";
        lblPageSummary.Size = new Size(70, 15);
        lblPageSummary.TabIndex = 11;
        lblPageSummary.Text = "No requests";
        // 
        // btnPreviousPage
        // 
        btnPreviousPage.Location = new Point(1220, 0);
        btnPreviousPage.Name = "btnPreviousPage";
        btnPreviousPage.Size = new Size(20, 23);
        btnPreviousPage.TabIndex = 12;
        btnPreviousPage.Text = "<";
        btnPreviousPage.UseVisualStyleBackColor = true;
        btnPreviousPage.Click += btnPreviousPage_Click;
        // 
        // btnNextPage
        // 
        btnNextPage.Location = new Point(1240, 0);
        btnNextPage.Name = "btnNextPage";
        btnNextPage.Size = new Size(20, 23);
        btnNextPage.TabIndex = 13;
        btnNextPage.Text = ">";
        btnNextPage.UseVisualStyleBackColor = true;
        btnNextPage.Click += btnNextPage_Click;
        // 
        // dgvRequests
        // 
        dgvRequests.AllowUserToAddRows = false;
        dgvRequests.AllowUserToDeleteRows = false;
        dgvRequests.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvRequests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvRequests.Columns.AddRange(new DataGridViewColumn[] { colStatus });
        dgvRequests.Location = new Point(12, 123);
        dgvRequests.MultiSelect = false;
        dgvRequests.Name = "dgvRequests";
        dgvRequests.ReadOnly = true;
        dgvRequests.RowHeadersVisible = false;
        dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRequests.Size = new Size(1260, 182);
        dgvRequests.TabIndex = 14;
        dgvRequests.CellFormatting += dgvRequests_CellFormatting;
        dgvRequests.SelectionChanged += dgvRequests_SelectionChanged;
        // 
        // colStatus
        // 
        colStatus.DataPropertyName = "Status";
        colStatus.HeaderText = "Status";
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        // 
        // tabWorkspace
        // 
        tabWorkspace.Controls.Add(tabRequestInformation);
        tabWorkspace.Controls.Add(tabValidation);
        tabWorkspace.Controls.Add(tabIntendedUsers);
        tabWorkspace.Controls.Add(tabAttachments);
        tabWorkspace.Controls.Add(tabHistory);
        tabWorkspace.Location = new Point(12, 311);
        tabWorkspace.Name = "tabWorkspace";
        tabWorkspace.SelectedIndex = 0;
        tabWorkspace.Size = new Size(1260, 335);
        tabWorkspace.TabIndex = 15;
        // 
        // tabRequestInformation
        // 
        tabRequestInformation.Controls.Add(lblRequestNumber);
        tabRequestInformation.Controls.Add(lblStatus);
        tabRequestInformation.Controls.Add(lblRowVersion);
        tabRequestInformation.Controls.Add(lblAudit);
        tabRequestInformation.Controls.Add(cmbClient);
        tabRequestInformation.Controls.Add(cmbPrimaryContact);
        tabRequestInformation.Controls.Add(cmbIntendedUse);
        tabRequestInformation.Controls.Add(txtIntendedUseNarrative);
        tabRequestInformation.Controls.Add(dtpEffectiveDate);
        tabRequestInformation.Controls.Add(cmbValueDefinition);
        tabRequestInformation.Controls.Add(cmbAppraisalType);
        tabRequestInformation.Controls.Add(cmbPriority);
        tabRequestInformation.Controls.Add(txtScopeSummary);
        tabRequestInformation.Controls.Add(txtVehicleSummary);
        tabRequestInformation.Controls.Add(btnSelectVehicle);
        tabRequestInformation.Controls.Add(btnAddIntendedUser);
        tabRequestInformation.Controls.Add(btnEditIntendedUser);
        tabRequestInformation.Controls.Add(btnDeactivateIntendedUser);
        tabRequestInformation.Controls.Add(btnAddAttachment);
        tabRequestInformation.Controls.Add(btnSave);
        tabRequestInformation.Controls.Add(btnCancelEdit);
        tabRequestInformation.Controls.Add(btnValidate);
        tabRequestInformation.Controls.Add(btnSubmit);
        tabRequestInformation.Controls.Add(btnDelete);
        tabRequestInformation.Controls.Add(btnMoreActions);
        tabRequestInformation.Location = new Point(4, 24);
        tabRequestInformation.Name = "tabRequestInformation";
        tabRequestInformation.Padding = new Padding(3);
        tabRequestInformation.Size = new Size(1252, 307);
        tabRequestInformation.TabIndex = 0;
        tabRequestInformation.Text = "Request Information";
        tabRequestInformation.UseVisualStyleBackColor = true;
        // 
        // lblRequestNumber
        // 
        lblRequestNumber.AutoSize = true;
        lblRequestNumber.Location = new Point(6, 10);
        lblRequestNumber.Name = "lblRequestNumber";
        lblRequestNumber.Size = new Size(111, 15);
        lblRequestNumber.TabIndex = 0;
        lblRequestNumber.Text = "No request selected";
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(320, 10);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(39, 15);
        lblStatus.TabIndex = 1;
        lblStatus.Text = "Status";
        // 
        // lblRowVersion
        // 
        lblRowVersion.AutoSize = true;
        lblRowVersion.Location = new Point(450, 10);
        lblRowVersion.Name = "lblRowVersion";
        lblRowVersion.Size = new Size(0, 15);
        lblRowVersion.TabIndex = 2;
        // 
        // lblAudit
        // 
        lblAudit.AutoSize = true;
        lblAudit.Location = new Point(6, 30);
        lblAudit.Name = "lblAudit";
        lblAudit.Size = new Size(0, 15);
        lblAudit.TabIndex = 3;
        // 
        // cmbClient
        // 
        cmbClient.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbClient.Location = new Point(6, 55);
        cmbClient.Name = "cmbClient";
        cmbClient.Size = new Size(240, 23);
        cmbClient.TabIndex = 4;
        cmbClient.SelectedIndexChanged += cmbClient_SelectedIndexChanged;
        // 
        // cmbPrimaryContact
        // 
        cmbPrimaryContact.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPrimaryContact.Location = new Point(252, 55);
        cmbPrimaryContact.Name = "cmbPrimaryContact";
        cmbPrimaryContact.Size = new Size(220, 23);
        cmbPrimaryContact.TabIndex = 5;
        cmbPrimaryContact.SelectedIndexChanged += EditorControl_Changed;
        // 
        // cmbIntendedUse
        // 
        cmbIntendedUse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbIntendedUse.Location = new Point(478, 55);
        cmbIntendedUse.Name = "cmbIntendedUse";
        cmbIntendedUse.Size = new Size(220, 23);
        cmbIntendedUse.TabIndex = 6;
        cmbIntendedUse.SelectedIndexChanged += EditorControl_Changed;
        // 
        // txtIntendedUseNarrative
        // 
        txtIntendedUseNarrative.Location = new Point(704, 55);
        txtIntendedUseNarrative.Name = "txtIntendedUseNarrative";
        txtIntendedUseNarrative.Size = new Size(250, 23);
        txtIntendedUseNarrative.TabIndex = 7;
        txtIntendedUseNarrative.TextChanged += EditorControl_Changed;
        // 
        // dtpEffectiveDate
        // 
        dtpEffectiveDate.Format = DateTimePickerFormat.Short;
        dtpEffectiveDate.Location = new Point(960, 55);
        dtpEffectiveDate.Name = "dtpEffectiveDate";
        dtpEffectiveDate.Size = new Size(130, 23);
        dtpEffectiveDate.TabIndex = 8;
        dtpEffectiveDate.ValueChanged += EditorControl_Changed;
        // 
        // cmbValueDefinition
        // 
        cmbValueDefinition.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbValueDefinition.Location = new Point(6, 84);
        cmbValueDefinition.Name = "cmbValueDefinition";
        cmbValueDefinition.Size = new Size(240, 23);
        cmbValueDefinition.TabIndex = 9;
        cmbValueDefinition.SelectedIndexChanged += EditorControl_Changed;
        // 
        // cmbAppraisalType
        // 
        cmbAppraisalType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbAppraisalType.Location = new Point(252, 84);
        cmbAppraisalType.Name = "cmbAppraisalType";
        cmbAppraisalType.Size = new Size(220, 23);
        cmbAppraisalType.TabIndex = 10;
        cmbAppraisalType.SelectedIndexChanged += EditorControl_Changed;
        // 
        // cmbPriority
        // 
        cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPriority.Location = new Point(478, 84);
        cmbPriority.Name = "cmbPriority";
        cmbPriority.Size = new Size(120, 23);
        cmbPriority.TabIndex = 11;
        cmbPriority.SelectedIndexChanged += EditorControl_Changed;
        // 
        // txtScopeSummary
        // 
        txtScopeSummary.Location = new Point(6, 113);
        txtScopeSummary.Multiline = true;
        txtScopeSummary.Name = "txtScopeSummary";
        txtScopeSummary.Size = new Size(592, 82);
        txtScopeSummary.TabIndex = 12;
        txtScopeSummary.TextChanged += EditorControl_Changed;
        // 
        // txtVehicleSummary
        // 
        txtVehicleSummary.Location = new Point(604, 113);
        txtVehicleSummary.Multiline = true;
        txtVehicleSummary.Name = "txtVehicleSummary";
        txtVehicleSummary.ReadOnly = true;
        txtVehicleSummary.Size = new Size(486, 82);
        txtVehicleSummary.TabIndex = 13;
        // 
        // btnSelectVehicle
        // 
        btnSelectVehicle.Location = new Point(1096, 113);
        btnSelectVehicle.Name = "btnSelectVehicle";
        btnSelectVehicle.Size = new Size(130, 25);
        btnSelectVehicle.TabIndex = 14;
        btnSelectVehicle.Text = "Select Vehicle";
        btnSelectVehicle.UseVisualStyleBackColor = true;
        btnSelectVehicle.Click += btnSelectVehicle_Click;
        // 
        // btnAddIntendedUser
        // 
        btnAddIntendedUser.Location = new Point(1096, 142);
        btnAddIntendedUser.Name = "btnAddIntendedUser";
        btnAddIntendedUser.Size = new Size(130, 25);
        btnAddIntendedUser.TabIndex = 15;
        btnAddIntendedUser.Text = "Add Intended User";
        btnAddIntendedUser.UseVisualStyleBackColor = true;
        btnAddIntendedUser.Click += btnAddIntendedUser_Click;
        // 
        // btnEditIntendedUser
        // 
        btnEditIntendedUser.Location = new Point(1096, 171);
        btnEditIntendedUser.Name = "btnEditIntendedUser";
        btnEditIntendedUser.Size = new Size(130, 25);
        btnEditIntendedUser.TabIndex = 16;
        btnEditIntendedUser.Text = "Edit Intended User";
        btnEditIntendedUser.UseVisualStyleBackColor = true;
        btnEditIntendedUser.Click += btnEditIntendedUser_Click;
        // 
        // btnDeactivateIntendedUser
        // 
        btnDeactivateIntendedUser.Location = new Point(1096, 200);
        btnDeactivateIntendedUser.Name = "btnDeactivateIntendedUser";
        btnDeactivateIntendedUser.Size = new Size(130, 25);
        btnDeactivateIntendedUser.TabIndex = 17;
        btnDeactivateIntendedUser.Text = "Deactivate User";
        btnDeactivateIntendedUser.UseVisualStyleBackColor = true;
        btnDeactivateIntendedUser.Click += btnDeactivateIntendedUser_Click;
        // 
        // btnAddAttachment
        // 
        btnAddAttachment.Location = new Point(1096, 229);
        btnAddAttachment.Name = "btnAddAttachment";
        btnAddAttachment.Size = new Size(130, 25);
        btnAddAttachment.TabIndex = 18;
        btnAddAttachment.Text = "Add Attachment";
        btnAddAttachment.UseVisualStyleBackColor = true;
        btnAddAttachment.Click += btnAddAttachment_Click;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(6, 270);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(80, 25);
        btnSave.TabIndex = 19;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancelEdit
        // 
        btnCancelEdit.Location = new Point(92, 270);
        btnCancelEdit.Name = "btnCancelEdit";
        btnCancelEdit.Size = new Size(80, 25);
        btnCancelEdit.TabIndex = 20;
        btnCancelEdit.Text = "Cancel";
        btnCancelEdit.UseVisualStyleBackColor = true;
        btnCancelEdit.Click += btnCancelEdit_Click;
        // 
        // btnValidate
        // 
        btnValidate.Location = new Point(178, 270);
        btnValidate.Name = "btnValidate";
        btnValidate.Size = new Size(80, 25);
        btnValidate.TabIndex = 21;
        btnValidate.Text = "Validate";
        btnValidate.UseVisualStyleBackColor = true;
        btnValidate.Click += btnValidate_Click;
        // 
        // btnSubmit
        // 
        btnSubmit.Location = new Point(264, 270);
        btnSubmit.Name = "btnSubmit";
        btnSubmit.Size = new Size(80, 25);
        btnSubmit.TabIndex = 22;
        btnSubmit.Text = "Submit";
        btnSubmit.UseVisualStyleBackColor = true;
        btnSubmit.Click += btnSubmit_Click;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(350, 270);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(80, 25);
        btnDelete.TabIndex = 23;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnMoreActions
        // 
        btnMoreActions.Location = new Point(436, 270);
        btnMoreActions.Name = "btnMoreActions";
        btnMoreActions.Size = new Size(100, 25);
        btnMoreActions.TabIndex = 24;
        btnMoreActions.Text = "More Actions";
        btnMoreActions.UseVisualStyleBackColor = true;
        btnMoreActions.Click += btnMoreActions_Click;
        // 
        // tabValidation
        // 
        tabValidation.Controls.Add(lblValidationSummary);
        tabValidation.Controls.Add(dgvValidation);
        tabValidation.Location = new Point(4, 24);
        tabValidation.Name = "tabValidation";
        tabValidation.Padding = new Padding(3);
        tabValidation.Size = new Size(1252, 307);
        tabValidation.TabIndex = 1;
        tabValidation.Text = "Validation";
        tabValidation.UseVisualStyleBackColor = true;
        // 
        // lblValidationSummary
        // 
        lblValidationSummary.AutoSize = true;
        lblValidationSummary.Location = new Point(6, 10);
        lblValidationSummary.Name = "lblValidationSummary";
        lblValidationSummary.Size = new Size(221, 15);
        lblValidationSummary.TabIndex = 0;
        lblValidationSummary.Text = "Validation has not been run for this view.";
        // 
        // dgvValidation
        // 
        dgvValidation.AllowUserToAddRows = false;
        dgvValidation.AllowUserToDeleteRows = false;
        dgvValidation.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvValidation.Location = new Point(6, 31);
        dgvValidation.Name = "dgvValidation";
        dgvValidation.ReadOnly = true;
        dgvValidation.RowHeadersVisible = false;
        dgvValidation.Size = new Size(1240, 270);
        dgvValidation.TabIndex = 1;
        // 
        // tabIntendedUsers
        // 
        tabIntendedUsers.Controls.Add(dgvIntendedUsers);
        tabIntendedUsers.Location = new Point(4, 24);
        tabIntendedUsers.Name = "tabIntendedUsers";
        tabIntendedUsers.Padding = new Padding(3);
        tabIntendedUsers.Size = new Size(1252, 307);
        tabIntendedUsers.TabIndex = 2;
        tabIntendedUsers.Text = "Intended Users";
        tabIntendedUsers.UseVisualStyleBackColor = true;
        // 
        // dgvIntendedUsers
        // 
        dgvIntendedUsers.AllowUserToAddRows = false;
        dgvIntendedUsers.AllowUserToDeleteRows = false;
        dgvIntendedUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvIntendedUsers.Dock = DockStyle.Fill;
        dgvIntendedUsers.Location = new Point(3, 3);
        dgvIntendedUsers.Name = "dgvIntendedUsers";
        dgvIntendedUsers.ReadOnly = true;
        dgvIntendedUsers.RowHeadersVisible = false;
        dgvIntendedUsers.Size = new Size(1246, 301);
        dgvIntendedUsers.TabIndex = 0;
        // 
        // tabAttachments
        // 
        tabAttachments.Controls.Add(dgvAttachments);
        tabAttachments.Location = new Point(4, 24);
        tabAttachments.Name = "tabAttachments";
        tabAttachments.Padding = new Padding(3);
        tabAttachments.Size = new Size(1252, 307);
        tabAttachments.TabIndex = 3;
        tabAttachments.Text = "Attachments";
        tabAttachments.UseVisualStyleBackColor = true;
        // 
        // dgvAttachments
        // 
        dgvAttachments.AllowUserToAddRows = false;
        dgvAttachments.AllowUserToDeleteRows = false;
        dgvAttachments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvAttachments.Dock = DockStyle.Fill;
        dgvAttachments.Location = new Point(3, 3);
        dgvAttachments.Name = "dgvAttachments";
        dgvAttachments.ReadOnly = true;
        dgvAttachments.RowHeadersVisible = false;
        dgvAttachments.Size = new Size(1246, 301);
        dgvAttachments.TabIndex = 0;
        // 
        // tabHistory
        // 
        tabHistory.Controls.Add(dgvHistory);
        tabHistory.Location = new Point(4, 24);
        tabHistory.Name = "tabHistory";
        tabHistory.Padding = new Padding(3);
        tabHistory.Size = new Size(1252, 307);
        tabHistory.TabIndex = 4;
        tabHistory.Text = "History";
        tabHistory.UseVisualStyleBackColor = true;
        // 
        // dgvHistory
        // 
        dgvHistory.AllowUserToAddRows = false;
        dgvHistory.AllowUserToDeleteRows = false;
        dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvHistory.Dock = DockStyle.Fill;
        dgvHistory.Location = new Point(3, 3);
        dgvHistory.Name = "dgvHistory";
        dgvHistory.ReadOnly = true;
        dgvHistory.RowHeadersVisible = false;
        dgvHistory.Size = new Size(1246, 301);
        dgvHistory.TabIndex = 0;
        // 
        // prgBusy
        // 
        prgBusy.Location = new Point(12, 652);
        prgBusy.MarqueeAnimationSpeed = 30;
        prgBusy.Name = "prgBusy";
        prgBusy.Size = new Size(140, 12);
        prgBusy.Style = ProgressBarStyle.Marquee;
        prgBusy.TabIndex = 16;
        prgBusy.Visible = false;
        // 
        // lblWorkspaceStatus
        // 
        lblWorkspaceStatus.AutoSize = true;
        lblWorkspaceStatus.Location = new Point(160, 650);
        lblWorkspaceStatus.Name = "lblWorkspaceStatus";
        lblWorkspaceStatus.Size = new Size(39, 15);
        lblWorkspaceStatus.TabIndex = 17;
        lblWorkspaceStatus.Text = "Ready";
        // 
        // cmsMoreActions
        // 
        cmsMoreActions.Items.AddRange(new ToolStripItem[] { mnuCancelRequest, mnuWithdrawRequest, mnuDeleteDraft });
        cmsMoreActions.Name = "cmsMoreActions";
        cmsMoreActions.Size = new Size(171, 70);
        // 
        // mnuCancelRequest
        // 
        mnuCancelRequest.Name = "mnuCancelRequest";
        mnuCancelRequest.Size = new Size(170, 22);
        mnuCancelRequest.Text = "Cancel Request";
        mnuCancelRequest.Click += mnuCancelRequest_Click;
        // 
        // mnuWithdrawRequest
        // 
        mnuWithdrawRequest.Name = "mnuWithdrawRequest";
        mnuWithdrawRequest.Size = new Size(170, 22);
        mnuWithdrawRequest.Text = "Withdraw Request";
        mnuWithdrawRequest.Click += mnuWithdrawRequest_Click;
        // 
        // mnuDeleteDraft
        // 
        mnuDeleteDraft.Name = "mnuDeleteDraft";
        mnuDeleteDraft.Size = new Size(170, 22);
        mnuDeleteDraft.Text = "Delete Draft";
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // AppraisalRequestWorkspaceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1284, 673);
        Controls.Add(lblWorkspaceStatus);
        Controls.Add(prgBusy);
        Controls.Add(tabWorkspace);
        Controls.Add(dgvRequests);
        Controls.Add(pnlListSummaryBar);
        Controls.Add(pnlFilterBar);
        Controls.Add(pnlTitle);
        Name = "AppraisalRequestWorkspaceForm";
        Text = "Appraisal Request Workspace";
        Load += AppraisalRequestWorkspaceForm_Load;
        pnlTitle.ResumeLayout(false);
        pnlTitle.PerformLayout();
        pnlFilterBar.ResumeLayout(false);
        pnlFilterBar.PerformLayout();
        pnlListSummaryBar.ResumeLayout(false);
        pnlListSummaryBar.PerformLayout();
        ((ISupportInitialize)dgvRequests).EndInit();
        tabWorkspace.ResumeLayout(false);
        tabRequestInformation.ResumeLayout(false);
        tabRequestInformation.PerformLayout();
        tabValidation.ResumeLayout(false);
        tabValidation.PerformLayout();
        ((ISupportInitialize)dgvValidation).EndInit();
        tabIntendedUsers.ResumeLayout(false);
        ((ISupportInitialize)dgvIntendedUsers).EndInit();
        tabAttachments.ResumeLayout(false);
        ((ISupportInitialize)dgvAttachments).EndInit();
        tabHistory.ResumeLayout(false);
        ((ISupportInitialize)dgvHistory).EndInit();
        cmsMoreActions.ResumeLayout(false);
        ((ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
