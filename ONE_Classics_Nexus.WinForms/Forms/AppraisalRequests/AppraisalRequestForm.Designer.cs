#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

partial class AppraisalRequestForm
{
    private System.ComponentModel.IContainer? components = null;

    private Panel pnlScrollHost = null!;
    private TableLayoutPanel rootLayout = null!;
    private Panel pnlCommandBar = null!;
    private FlowLayoutPanel commandFlow = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnSaveClose = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;
    private Button btnPrint = null!;
    private Button btnClose = null!;

    private Panel pnlHeader = null!;
    private Label lblFormTitle = null!;
    private TableLayoutPanel headerGrid = null!;
    private Label lblRequestNumber = null!;
    private TextBox txtRequestNumber = null!;
    private Label lblStatus = null!;
    private ComboBox cboStatus = null!;
    private Label lblPriority = null!;
    private ComboBox cboPriority = null!;
    private Label lblDateCreated = null!;
    private TextBox txtDateCreated = null!;
    private Label lblSource = null!;
    private ComboBox cboSource = null!;
    private Label lblLastUpdated = null!;
    private TextBox txtLastUpdated = null!;
    private Label lblCreatedBy = null!;
    private TextBox txtCreatedBy = null!;

    private SplitContainer splitWorkspace = null!;
    private TabControl tabMain = null!;
    private TabPage tabRequestInformation = null!;
    private TabPage tabSubjectInformation = null!;
    private TabPage tabRequesterContact = null!;
    private TabPage tabServiceTiming = null!;
    private TabPage tabIntakeDetails = null!;
    private TabPage tabReviewDisposition = null!;
    private TabPage tabHistory = null!;

    private Panel pnlRightSidebar = null!;
    private TableLayoutPanel sidebarLayout = null!;

    private GroupBox grpRequestStatus = null!;
    private TableLayoutPanel statusLayout = null!;
    private Label lblStatusCaption = null!;
    private Panel pnlStatusBadge = null!;
    private Label lblStatusValue = null!;
    private Label lblPriorityCaption = null!;
    private Label lblPriorityValue = null!;
    private Label lblQualificationCaption = null!;
    private Label lblQualificationValue = null!;
    private Label lblDispositionCaption = null!;
    private Label lblDispositionValue = null!;

    private GroupBox grpLinkedRecords = null!;
    private TableLayoutPanel linkedLayout = null!;
    private Label lblClientCaption = null!;
    private LinkLabel lnkClient = null!;
    private Label lblVehicleCaption = null!;
    private LinkLabel lnkVehicle = null!;
    private Label lblAppraisalCaption = null!;
    private LinkLabel lnkAppraisal = null!;

    private GroupBox grpQuickNotes = null!;
    private TableLayoutPanel quickNotesLayout = null!;
    private TextBox txtQuickNote = null!;
    private Button btnSaveQuickNote = null!;

    private GroupBox grpAttachments = null!;
    private TableLayoutPanel attachmentsLayout = null!;
    private FlowLayoutPanel attachmentCommandFlow = null!;
    private Button btnAddAttachment = null!;
    private Button btnOpenAttachment = null!;
    private Button btnRemoveAttachment = null!;
    private ListView lvAttachments = null!;
    private ColumnHeader colAttachmentName = null!;
    private ColumnHeader colAttachmentSize = null!;
    private ColumnHeader colAttachmentAdded = null!;

    private Panel pnlFooter = null!;
    private TableLayoutPanel footerLayout = null!;
    private Label lblRecordIdCaption = null!;
    private Label lblRecordIdValue = null!;
    private Label lblFooterCreatedByCaption = null!;
    private Label lblFooterCreatedByValue = null!;
    private Label lblFooterCreatedAtValue = null!;
    private Label lblFooterUpdatedByCaption = null!;
    private Label lblFooterUpdatedByValue = null!;
    private Label lblFooterUpdatedAtValue = null!;
    private Label lblEditMode = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlScrollHost = new Panel();
        rootLayout = new TableLayoutPanel();
        pnlCommandBar = new Panel();
        commandFlow = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnSaveClose = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnPrint = new Button();
        btnClose = new Button();
        pnlHeader = new Panel();
        lblFormTitle = new Label();
        headerGrid = new TableLayoutPanel();
        lblRequestNumber = new Label();
        txtRequestNumber = new TextBox();
        lblStatus = new Label();
        cboStatus = new ComboBox();
        lblPriority = new Label();
        cboPriority = new ComboBox();
        lblDateCreated = new Label();
        txtDateCreated = new TextBox();
        lblSource = new Label();
        cboSource = new ComboBox();
        lblLastUpdated = new Label();
        txtLastUpdated = new TextBox();
        lblCreatedBy = new Label();
        txtCreatedBy = new TextBox();
        splitWorkspace = new SplitContainer();
        tabMain = new TabControl();
        tabRequestInformation = new TabPage();
        tabSubjectInformation = new TabPage();
        tabRequesterContact = new TabPage();
        tabServiceTiming = new TabPage();
        tabIntakeDetails = new TabPage();
        tabReviewDisposition = new TabPage();
        tabHistory = new TabPage();
        pnlRightSidebar = new Panel();
        sidebarLayout = new TableLayoutPanel();
        grpRequestStatus = new GroupBox();
        statusLayout = new TableLayoutPanel();
        lblStatusCaption = new Label();
        pnlStatusBadge = new Panel();
        lblStatusValue = new Label();
        lblPriorityCaption = new Label();
        lblPriorityValue = new Label();
        lblQualificationCaption = new Label();
        lblQualificationValue = new Label();
        lblDispositionCaption = new Label();
        lblDispositionValue = new Label();
        grpLinkedRecords = new GroupBox();
        linkedLayout = new TableLayoutPanel();
        lblClientCaption = new Label();
        lnkClient = new LinkLabel();
        lblVehicleCaption = new Label();
        lnkVehicle = new LinkLabel();
        lblAppraisalCaption = new Label();
        lnkAppraisal = new LinkLabel();
        grpQuickNotes = new GroupBox();
        quickNotesLayout = new TableLayoutPanel();
        txtQuickNote = new TextBox();
        btnSaveQuickNote = new Button();
        grpAttachments = new GroupBox();
        attachmentsLayout = new TableLayoutPanel();
        attachmentCommandFlow = new FlowLayoutPanel();
        btnAddAttachment = new Button();
        btnOpenAttachment = new Button();
        btnRemoveAttachment = new Button();
        lvAttachments = new ListView();
        colAttachmentName = new ColumnHeader();
        colAttachmentSize = new ColumnHeader();
        colAttachmentAdded = new ColumnHeader();
        pnlFooter = new Panel();
        footerLayout = new TableLayoutPanel();
        lblRecordIdCaption = new Label();
        lblRecordIdValue = new Label();
        lblFooterCreatedByCaption = new Label();
        lblFooterCreatedByValue = new Label();
        lblFooterCreatedAtValue = new Label();
        lblFooterUpdatedByCaption = new Label();
        lblFooterUpdatedByValue = new Label();
        lblFooterUpdatedAtValue = new Label();
        lblEditMode = new Label();
        ((System.ComponentModel.ISupportInitialize)splitWorkspace).BeginInit();
        splitWorkspace.SuspendLayout();
        SuspendLayout();
        // 
        // pnlScrollHost
        // 
        pnlScrollHost.BorderStyle = BorderStyle.Fixed3D;
        pnlScrollHost.Location = new Point(539, 272);
        pnlScrollHost.Name = "pnlScrollHost";
        pnlScrollHost.Size = new Size(200, 100);
        pnlScrollHost.TabIndex = 0;
        // 
        // rootLayout
        // 
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Size = new Size(200, 100);
        rootLayout.TabIndex = 0;
        // 
        // pnlCommandBar
        // 
        pnlCommandBar.Location = new Point(0, 0);
        pnlCommandBar.Name = "pnlCommandBar";
        pnlCommandBar.Size = new Size(200, 100);
        pnlCommandBar.TabIndex = 0;
        // 
        // commandFlow
        // 
        commandFlow.Location = new Point(0, 0);
        commandFlow.Name = "commandFlow";
        commandFlow.Size = new Size(200, 100);
        commandFlow.TabIndex = 0;
        // 
        // btnNew
        // 
        btnNew.Location = new Point(0, 0);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(75, 23);
        btnNew.TabIndex = 0;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(0, 0);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 23);
        btnSave.TabIndex = 0;
        // 
        // btnSaveClose
        // 
        btnSaveClose.Location = new Point(0, 0);
        btnSaveClose.Name = "btnSaveClose";
        btnSaveClose.Size = new Size(75, 23);
        btnSaveClose.TabIndex = 0;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(0, 0);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(75, 23);
        btnDelete.TabIndex = 0;
        // 
        // btnRefresh
        // 
        btnRefresh.Location = new Point(0, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(75, 23);
        btnRefresh.TabIndex = 0;
        // 
        // btnPrint
        // 
        btnPrint.Location = new Point(0, 0);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(75, 23);
        btnPrint.TabIndex = 0;
        // 
        // btnClose
        // 
        btnClose.Location = new Point(0, 0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 23);
        btnClose.TabIndex = 0;
        // 
        // pnlHeader
        // 
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(200, 100);
        pnlHeader.TabIndex = 0;
        // 
        // lblFormTitle
        // 
        lblFormTitle.Location = new Point(0, 0);
        lblFormTitle.Name = "lblFormTitle";
        lblFormTitle.Size = new Size(100, 23);
        lblFormTitle.TabIndex = 0;
        // 
        // headerGrid
        // 
        headerGrid.Location = new Point(0, 0);
        headerGrid.Name = "headerGrid";
        headerGrid.Size = new Size(200, 100);
        headerGrid.TabIndex = 0;
        // 
        // lblRequestNumber
        // 
        lblRequestNumber.Location = new Point(0, 0);
        lblRequestNumber.Name = "lblRequestNumber";
        lblRequestNumber.Size = new Size(100, 23);
        lblRequestNumber.TabIndex = 0;
        // 
        // txtRequestNumber
        // 
        txtRequestNumber.Location = new Point(0, 0);
        txtRequestNumber.Name = "txtRequestNumber";
        txtRequestNumber.Size = new Size(100, 23);
        txtRequestNumber.TabIndex = 0;
        // 
        // lblStatus
        // 
        lblStatus.Location = new Point(0, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(100, 23);
        lblStatus.TabIndex = 0;
        // 
        // cboStatus
        // 
        cboStatus.Location = new Point(0, 0);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(121, 23);
        cboStatus.TabIndex = 0;
        // 
        // lblPriority
        // 
        lblPriority.Location = new Point(0, 0);
        lblPriority.Name = "lblPriority";
        lblPriority.Size = new Size(100, 23);
        lblPriority.TabIndex = 0;
        // 
        // cboPriority
        // 
        cboPriority.Location = new Point(0, 0);
        cboPriority.Name = "cboPriority";
        cboPriority.Size = new Size(121, 23);
        cboPriority.TabIndex = 0;
        // 
        // lblDateCreated
        // 
        lblDateCreated.Location = new Point(0, 0);
        lblDateCreated.Name = "lblDateCreated";
        lblDateCreated.Size = new Size(100, 23);
        lblDateCreated.TabIndex = 0;
        // 
        // txtDateCreated
        // 
        txtDateCreated.Location = new Point(0, 0);
        txtDateCreated.Name = "txtDateCreated";
        txtDateCreated.Size = new Size(100, 23);
        txtDateCreated.TabIndex = 0;
        // 
        // lblSource
        // 
        lblSource.Location = new Point(0, 0);
        lblSource.Name = "lblSource";
        lblSource.Size = new Size(100, 23);
        lblSource.TabIndex = 0;
        // 
        // cboSource
        // 
        cboSource.Location = new Point(0, 0);
        cboSource.Name = "cboSource";
        cboSource.Size = new Size(121, 23);
        cboSource.TabIndex = 0;
        // 
        // lblLastUpdated
        // 
        lblLastUpdated.Location = new Point(0, 0);
        lblLastUpdated.Name = "lblLastUpdated";
        lblLastUpdated.Size = new Size(100, 23);
        lblLastUpdated.TabIndex = 0;
        // 
        // txtLastUpdated
        // 
        txtLastUpdated.Location = new Point(0, 0);
        txtLastUpdated.Name = "txtLastUpdated";
        txtLastUpdated.Size = new Size(100, 23);
        txtLastUpdated.TabIndex = 0;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.Location = new Point(0, 0);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.Size = new Size(100, 23);
        lblCreatedBy.TabIndex = 0;
        // 
        // txtCreatedBy
        // 
        txtCreatedBy.Location = new Point(0, 0);
        txtCreatedBy.Name = "txtCreatedBy";
        txtCreatedBy.Size = new Size(100, 23);
        txtCreatedBy.TabIndex = 0;
        // 
        // splitWorkspace
        // 
        splitWorkspace.Location = new Point(0, 0);
        splitWorkspace.Name = "splitWorkspace";
        splitWorkspace.Size = new Size(150, 100);
        splitWorkspace.TabIndex = 0;
        // 
        // tabMain
        // 
        tabMain.Location = new Point(0, 0);
        tabMain.Name = "tabMain";
        tabMain.SelectedIndex = 0;
        tabMain.Size = new Size(200, 100);
        tabMain.TabIndex = 0;
        // 
        // tabRequestInformation
        // 
        tabRequestInformation.Location = new Point(0, 0);
        tabRequestInformation.Name = "tabRequestInformation";
        tabRequestInformation.Size = new Size(200, 100);
        tabRequestInformation.TabIndex = 0;
        // 
        // tabSubjectInformation
        // 
        tabSubjectInformation.Location = new Point(0, 0);
        tabSubjectInformation.Name = "tabSubjectInformation";
        tabSubjectInformation.Size = new Size(200, 100);
        tabSubjectInformation.TabIndex = 0;
        // 
        // tabRequesterContact
        // 
        tabRequesterContact.Location = new Point(0, 0);
        tabRequesterContact.Name = "tabRequesterContact";
        tabRequesterContact.Size = new Size(200, 100);
        tabRequesterContact.TabIndex = 0;
        // 
        // tabServiceTiming
        // 
        tabServiceTiming.Location = new Point(0, 0);
        tabServiceTiming.Name = "tabServiceTiming";
        tabServiceTiming.Size = new Size(200, 100);
        tabServiceTiming.TabIndex = 0;
        // 
        // tabIntakeDetails
        // 
        tabIntakeDetails.Location = new Point(0, 0);
        tabIntakeDetails.Name = "tabIntakeDetails";
        tabIntakeDetails.Size = new Size(200, 100);
        tabIntakeDetails.TabIndex = 0;
        // 
        // tabReviewDisposition
        // 
        tabReviewDisposition.Location = new Point(0, 0);
        tabReviewDisposition.Name = "tabReviewDisposition";
        tabReviewDisposition.Size = new Size(200, 100);
        tabReviewDisposition.TabIndex = 0;
        // 
        // tabHistory
        // 
        tabHistory.Location = new Point(0, 0);
        tabHistory.Name = "tabHistory";
        tabHistory.Size = new Size(200, 100);
        tabHistory.TabIndex = 0;
        // 
        // pnlRightSidebar
        // 
        pnlRightSidebar.Location = new Point(0, 0);
        pnlRightSidebar.MinimumSize = new Size(200, 100);
        pnlRightSidebar.Name = "pnlRightSidebar";
        pnlRightSidebar.Size = new Size(200, 100);
        pnlRightSidebar.TabIndex = 0;
        // 
        // sidebarLayout
        // 
        sidebarLayout.Location = new Point(0, 0);
        sidebarLayout.Name = "sidebarLayout";
        sidebarLayout.Size = new Size(200, 100);
        sidebarLayout.TabIndex = 0;
        // 
        // grpRequestStatus
        // 
        grpRequestStatus.Location = new Point(0, 0);
        grpRequestStatus.Name = "grpRequestStatus";
        grpRequestStatus.Size = new Size(200, 100);
        grpRequestStatus.TabIndex = 0;
        grpRequestStatus.TabStop = false;
        // 
        // statusLayout
        // 
        statusLayout.Location = new Point(0, 0);
        statusLayout.Name = "statusLayout";
        statusLayout.Size = new Size(200, 100);
        statusLayout.TabIndex = 0;
        // 
        // lblStatusCaption
        // 
        lblStatusCaption.Location = new Point(0, 0);
        lblStatusCaption.Name = "lblStatusCaption";
        lblStatusCaption.Size = new Size(100, 23);
        lblStatusCaption.TabIndex = 0;
        // 
        // pnlStatusBadge
        // 
        pnlStatusBadge.Location = new Point(0, 0);
        pnlStatusBadge.Name = "pnlStatusBadge";
        pnlStatusBadge.Size = new Size(200, 100);
        pnlStatusBadge.TabIndex = 0;
        // 
        // lblStatusValue
        // 
        lblStatusValue.Location = new Point(0, 0);
        lblStatusValue.Name = "lblStatusValue";
        lblStatusValue.Size = new Size(100, 23);
        lblStatusValue.TabIndex = 0;
        // 
        // lblPriorityCaption
        // 
        lblPriorityCaption.Location = new Point(0, 0);
        lblPriorityCaption.Name = "lblPriorityCaption";
        lblPriorityCaption.Size = new Size(100, 23);
        lblPriorityCaption.TabIndex = 0;
        // 
        // lblPriorityValue
        // 
        lblPriorityValue.Location = new Point(0, 0);
        lblPriorityValue.Name = "lblPriorityValue";
        lblPriorityValue.Size = new Size(100, 23);
        lblPriorityValue.TabIndex = 0;
        // 
        // lblQualificationCaption
        // 
        lblQualificationCaption.Location = new Point(0, 0);
        lblQualificationCaption.Name = "lblQualificationCaption";
        lblQualificationCaption.Size = new Size(100, 23);
        lblQualificationCaption.TabIndex = 0;
        // 
        // lblQualificationValue
        // 
        lblQualificationValue.Location = new Point(0, 0);
        lblQualificationValue.Name = "lblQualificationValue";
        lblQualificationValue.Size = new Size(100, 23);
        lblQualificationValue.TabIndex = 0;
        // 
        // lblDispositionCaption
        // 
        lblDispositionCaption.Location = new Point(0, 0);
        lblDispositionCaption.Name = "lblDispositionCaption";
        lblDispositionCaption.Size = new Size(100, 23);
        lblDispositionCaption.TabIndex = 0;
        // 
        // lblDispositionValue
        // 
        lblDispositionValue.Location = new Point(0, 0);
        lblDispositionValue.Name = "lblDispositionValue";
        lblDispositionValue.Size = new Size(100, 23);
        lblDispositionValue.TabIndex = 0;
        // 
        // grpLinkedRecords
        // 
        grpLinkedRecords.Location = new Point(0, 0);
        grpLinkedRecords.Name = "grpLinkedRecords";
        grpLinkedRecords.Size = new Size(200, 100);
        grpLinkedRecords.TabIndex = 0;
        grpLinkedRecords.TabStop = false;
        // 
        // linkedLayout
        // 
        linkedLayout.Location = new Point(0, 0);
        linkedLayout.Name = "linkedLayout";
        linkedLayout.Size = new Size(200, 100);
        linkedLayout.TabIndex = 0;
        // 
        // lblClientCaption
        // 
        lblClientCaption.Location = new Point(0, 0);
        lblClientCaption.Name = "lblClientCaption";
        lblClientCaption.Size = new Size(100, 23);
        lblClientCaption.TabIndex = 0;
        // 
        // lnkClient
        // 
        lnkClient.Location = new Point(0, 0);
        lnkClient.Name = "lnkClient";
        lnkClient.Size = new Size(100, 23);
        lnkClient.TabIndex = 0;
        // 
        // lblVehicleCaption
        // 
        lblVehicleCaption.Location = new Point(0, 0);
        lblVehicleCaption.Name = "lblVehicleCaption";
        lblVehicleCaption.Size = new Size(100, 23);
        lblVehicleCaption.TabIndex = 0;
        // 
        // lnkVehicle
        // 
        lnkVehicle.Location = new Point(0, 0);
        lnkVehicle.Name = "lnkVehicle";
        lnkVehicle.Size = new Size(100, 23);
        lnkVehicle.TabIndex = 0;
        // 
        // lblAppraisalCaption
        // 
        lblAppraisalCaption.Location = new Point(0, 0);
        lblAppraisalCaption.Name = "lblAppraisalCaption";
        lblAppraisalCaption.Size = new Size(100, 23);
        lblAppraisalCaption.TabIndex = 0;
        // 
        // lnkAppraisal
        // 
        lnkAppraisal.Location = new Point(0, 0);
        lnkAppraisal.Name = "lnkAppraisal";
        lnkAppraisal.Size = new Size(100, 23);
        lnkAppraisal.TabIndex = 0;
        // 
        // grpQuickNotes
        // 
        grpQuickNotes.Location = new Point(0, 0);
        grpQuickNotes.Name = "grpQuickNotes";
        grpQuickNotes.Size = new Size(200, 100);
        grpQuickNotes.TabIndex = 0;
        grpQuickNotes.TabStop = false;
        // 
        // quickNotesLayout
        // 
        quickNotesLayout.Location = new Point(0, 0);
        quickNotesLayout.Name = "quickNotesLayout";
        quickNotesLayout.Size = new Size(200, 100);
        quickNotesLayout.TabIndex = 0;
        // 
        // txtQuickNote
        // 
        txtQuickNote.Location = new Point(0, 0);
        txtQuickNote.Name = "txtQuickNote";
        txtQuickNote.Size = new Size(100, 23);
        txtQuickNote.TabIndex = 0;
        // 
        // btnSaveQuickNote
        // 
        btnSaveQuickNote.Location = new Point(0, 0);
        btnSaveQuickNote.Name = "btnSaveQuickNote";
        btnSaveQuickNote.Size = new Size(75, 23);
        btnSaveQuickNote.TabIndex = 0;
        // 
        // grpAttachments
        // 
        grpAttachments.Location = new Point(0, 0);
        grpAttachments.Name = "grpAttachments";
        grpAttachments.Size = new Size(200, 100);
        grpAttachments.TabIndex = 0;
        grpAttachments.TabStop = false;
        // 
        // attachmentsLayout
        // 
        attachmentsLayout.Location = new Point(0, 0);
        attachmentsLayout.Name = "attachmentsLayout";
        attachmentsLayout.Size = new Size(200, 100);
        attachmentsLayout.TabIndex = 0;
        // 
        // attachmentCommandFlow
        // 
        attachmentCommandFlow.Location = new Point(0, 0);
        attachmentCommandFlow.Name = "attachmentCommandFlow";
        attachmentCommandFlow.Size = new Size(200, 100);
        attachmentCommandFlow.TabIndex = 0;
        // 
        // btnAddAttachment
        // 
        btnAddAttachment.Location = new Point(0, 0);
        btnAddAttachment.Name = "btnAddAttachment";
        btnAddAttachment.Size = new Size(75, 23);
        btnAddAttachment.TabIndex = 0;
        // 
        // btnOpenAttachment
        // 
        btnOpenAttachment.Location = new Point(0, 0);
        btnOpenAttachment.Name = "btnOpenAttachment";
        btnOpenAttachment.Size = new Size(75, 23);
        btnOpenAttachment.TabIndex = 0;
        // 
        // btnRemoveAttachment
        // 
        btnRemoveAttachment.Location = new Point(0, 0);
        btnRemoveAttachment.Name = "btnRemoveAttachment";
        btnRemoveAttachment.Size = new Size(75, 23);
        btnRemoveAttachment.TabIndex = 0;
        // 
        // lvAttachments
        // 
        lvAttachments.Location = new Point(0, 0);
        lvAttachments.Name = "lvAttachments";
        lvAttachments.Size = new Size(121, 97);
        lvAttachments.TabIndex = 0;
        lvAttachments.UseCompatibleStateImageBehavior = false;
        // 
        // pnlFooter
        // 
        pnlFooter.Location = new Point(0, 0);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(200, 100);
        pnlFooter.TabIndex = 0;
        // 
        // footerLayout
        // 
        footerLayout.Location = new Point(0, 0);
        footerLayout.Name = "footerLayout";
        footerLayout.Size = new Size(200, 100);
        footerLayout.TabIndex = 0;
        // 
        // lblRecordIdCaption
        // 
        lblRecordIdCaption.Location = new Point(0, 0);
        lblRecordIdCaption.Name = "lblRecordIdCaption";
        lblRecordIdCaption.Size = new Size(100, 23);
        lblRecordIdCaption.TabIndex = 0;
        // 
        // lblRecordIdValue
        // 
        lblRecordIdValue.Location = new Point(0, 0);
        lblRecordIdValue.Name = "lblRecordIdValue";
        lblRecordIdValue.Size = new Size(100, 23);
        lblRecordIdValue.TabIndex = 0;
        // 
        // lblFooterCreatedByCaption
        // 
        lblFooterCreatedByCaption.Location = new Point(0, 0);
        lblFooterCreatedByCaption.Name = "lblFooterCreatedByCaption";
        lblFooterCreatedByCaption.Size = new Size(100, 23);
        lblFooterCreatedByCaption.TabIndex = 0;
        // 
        // lblFooterCreatedByValue
        // 
        lblFooterCreatedByValue.Location = new Point(0, 0);
        lblFooterCreatedByValue.Name = "lblFooterCreatedByValue";
        lblFooterCreatedByValue.Size = new Size(100, 23);
        lblFooterCreatedByValue.TabIndex = 0;
        // 
        // lblFooterCreatedAtValue
        // 
        lblFooterCreatedAtValue.Location = new Point(0, 0);
        lblFooterCreatedAtValue.Name = "lblFooterCreatedAtValue";
        lblFooterCreatedAtValue.Size = new Size(100, 23);
        lblFooterCreatedAtValue.TabIndex = 0;
        // 
        // lblFooterUpdatedByCaption
        // 
        lblFooterUpdatedByCaption.Location = new Point(0, 0);
        lblFooterUpdatedByCaption.Name = "lblFooterUpdatedByCaption";
        lblFooterUpdatedByCaption.Size = new Size(100, 23);
        lblFooterUpdatedByCaption.TabIndex = 0;
        // 
        // lblFooterUpdatedByValue
        // 
        lblFooterUpdatedByValue.Location = new Point(0, 0);
        lblFooterUpdatedByValue.Name = "lblFooterUpdatedByValue";
        lblFooterUpdatedByValue.Size = new Size(100, 23);
        lblFooterUpdatedByValue.TabIndex = 0;
        // 
        // lblFooterUpdatedAtValue
        // 
        lblFooterUpdatedAtValue.Location = new Point(0, 0);
        lblFooterUpdatedAtValue.Name = "lblFooterUpdatedAtValue";
        lblFooterUpdatedAtValue.Size = new Size(100, 23);
        lblFooterUpdatedAtValue.TabIndex = 0;
        // 
        // lblEditMode
        // 
        lblEditMode.Location = new Point(0, 0);
        lblEditMode.Name = "lblEditMode";
        lblEditMode.Size = new Size(100, 23);
        lblEditMode.TabIndex = 0;

        BuildRoot();
        BuildCommandBar();
        BuildHeader();
        BuildWorkspace();
        BuildSidebar();
        BuildFooter();
        // 
        // AppraisalRequestForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1920, 1080);
        Controls.Add(pnlScrollHost);
        FormBorderStyle = FormBorderStyle.None;
        MinimumSize = new Size(1180, 720);
        Name = "AppraisalRequestForm";
        Text = "Appraisal Request";
        Shown += AppraisalRequestForm_Shown;
        splitWorkspace.SizeChanged += SplitWorkspace_SizeChanged;
        ((System.ComponentModel.ISupportInitialize)splitWorkspace).EndInit();
        splitWorkspace.ResumeLayout(false);
        ApplyWorkspaceSplitterLayout();
        ResumeLayout(false);
    }

    private void BuildRoot()
    {
        pnlScrollHost.Dock = DockStyle.Fill;
        pnlScrollHost.AutoScroll = true;
        pnlScrollHost.Margin = Padding.Empty;
        pnlScrollHost.Padding = Padding.Empty;

        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Location = new Point(0, 0);
        rootLayout.Size = new Size(1920, 1080);
        rootLayout.MinimumSize = new Size(1920, 1080);
        rootLayout.RowCount = 4;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 158F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        rootLayout.Margin = Padding.Empty;
        rootLayout.Padding = Padding.Empty;

        rootLayout.Controls.Add(pnlCommandBar, 0, 0);
        rootLayout.Controls.Add(pnlHeader, 0, 1);
        rootLayout.Controls.Add(splitWorkspace, 0, 2);
        rootLayout.Controls.Add(pnlFooter, 0, 3);

        pnlScrollHost.Controls.Add(rootLayout);
    }

    private void BuildCommandBar()
    {
        pnlCommandBar.Dock = DockStyle.Fill;
        pnlCommandBar.Padding = new Padding(12, 8, 12, 8);
        pnlCommandBar.BorderStyle = BorderStyle.FixedSingle;

        commandFlow.Dock = DockStyle.Fill;
        commandFlow.FlowDirection = FlowDirection.LeftToRight;
        commandFlow.WrapContents = false;
        commandFlow.Padding = Padding.Empty;
        commandFlow.Margin = Padding.Empty;

        ConfigureCommandButton(btnNew, "+ New Request", 122);
        ConfigureCommandButton(btnSave, "Save", 88);
        ConfigureCommandButton(btnSaveClose, "Validate", 88);
        ConfigureCommandButton(btnDelete, "Submit", 88);
        ConfigureCommandButton(btnRefresh, "Refresh", 88);
        ConfigureCommandButton(btnPrint, "More Actions", 112);
        ConfigureCommandButton(btnClose, "Cancel", 86);

        commandFlow.Controls.Add(btnNew);
        commandFlow.Controls.Add(btnSave);
        commandFlow.Controls.Add(btnSaveClose);
        commandFlow.Controls.Add(btnDelete);
        commandFlow.Controls.Add(btnRefresh);
        commandFlow.Controls.Add(btnPrint);
        commandFlow.Controls.Add(btnClose);

        pnlCommandBar.Controls.Add(commandFlow);
    }

    private static void ConfigureCommandButton(Button button, string text, int width)
    {
        button.Text = text;
        button.Width = width;
        button.Height = 38;
        button.Margin = new Padding(0, 0, 8, 0);
        button.TextAlign = ContentAlignment.MiddleCenter;
        button.UseVisualStyleBackColor = false;
    }

    private void BuildHeader()
    {
        pnlHeader.Dock = DockStyle.Fill;
        pnlHeader.Padding = new Padding(18, 10, 18, 10);

        lblFormTitle.AutoSize = true;
        lblFormTitle.Text = "Appraisal Request";
        lblFormTitle.Location = new Point(18, 8);
        lblFormTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

        headerGrid.ColumnCount = 6;
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
        headerGrid.RowCount = 3;
        headerGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
        headerGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
        headerGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
        headerGrid.Location = new Point(18, 42);
        headerGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        headerGrid.Size = new Size(1100, 98);
        headerGrid.Margin = Padding.Empty;

        ConfigureFieldLabel(lblRequestNumber, "Request Number");
        ConfigureReadOnlyText(txtRequestNumber);

        ConfigureFieldLabel(lblStatus, "Status");
        ConfigureCombo(cboStatus);

        ConfigureFieldLabel(lblPriority, "Priority");
        ConfigureCombo(cboPriority);

        ConfigureFieldLabel(lblDateCreated, "Date Created");
        ConfigureReadOnlyText(txtDateCreated);

        ConfigureFieldLabel(lblSource, "Source");
        ConfigureCombo(cboSource);

        ConfigureFieldLabel(lblLastUpdated, "Last Updated");
        ConfigureReadOnlyText(txtLastUpdated);

        ConfigureFieldLabel(lblCreatedBy, "Created By");
        ConfigureReadOnlyText(txtCreatedBy);

        headerGrid.Controls.Add(lblRequestNumber, 0, 0);
        headerGrid.Controls.Add(txtRequestNumber, 1, 0);
        headerGrid.Controls.Add(lblStatus, 2, 0);
        headerGrid.Controls.Add(cboStatus, 3, 0);
        headerGrid.Controls.Add(lblPriority, 4, 0);
        headerGrid.Controls.Add(cboPriority, 5, 0);

        headerGrid.Controls.Add(lblDateCreated, 0, 1);
        headerGrid.Controls.Add(txtDateCreated, 1, 1);
        headerGrid.Controls.Add(lblSource, 2, 1);
        headerGrid.Controls.Add(cboSource, 3, 1);
        headerGrid.Controls.Add(lblLastUpdated, 4, 1);
        headerGrid.Controls.Add(txtLastUpdated, 5, 1);

        headerGrid.Controls.Add(lblCreatedBy, 0, 2);
        headerGrid.Controls.Add(txtCreatedBy, 1, 2);
        headerGrid.SetColumnSpan(txtCreatedBy, 2);

        pnlHeader.Controls.Add(lblFormTitle);
        pnlHeader.Controls.Add(headerGrid);
    }

    private static void ConfigureFieldLabel(Label label, string text)
    {
        label.AutoSize = true;
        label.Text = text;
        label.Anchor = AnchorStyles.Left;
        label.Margin = new Padding(3, 0, 6, 0);
    }

    private static void ConfigureReadOnlyText(TextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.ReadOnly = true;
        textBox.Margin = new Padding(3, 4, 12, 4);
        textBox.BorderStyle = BorderStyle.FixedSingle;
    }

    private static void ConfigureCombo(ComboBox combo)
    {
        combo.Dock = DockStyle.Fill;
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        combo.Margin = new Padding(3, 4, 12, 4);
        combo.IntegralHeight = false;
        combo.MaxDropDownItems = 12;
    }

    private void BuildWorkspace()
    {
        splitWorkspace.Dock = DockStyle.Fill;
        splitWorkspace.FixedPanel = FixedPanel.Panel2;
        splitWorkspace.IsSplitterFixed = false;

        // REMOVE these (too early during EndInit):
        // splitWorkspace.Panel1MinSize = 760;
        // splitWorkspace.Panel2MinSize = 250;
        // splitWorkspace.SplitterDistance = 1110;

        splitWorkspace.SplitterWidth = 5;
        splitWorkspace.BackColor = Color.FromArgb(224, 228, 232);

        tabMain.Dock = DockStyle.Fill;
        tabMain.Padding = new Point(16, 5);
        tabMain.Multiline = false;
        tabMain.SizeMode = TabSizeMode.Normal;

        tabRequestInformation.Text = "Request Information";
        tabSubjectInformation.Text = "Vehicle";
        tabRequesterContact.Text = "Client and Intended Users";
        tabServiceTiming.Text = "Notes and Attachments";
        tabIntakeDetails.Text = "Review";
        tabReviewDisposition.Text = "Disposition";
        tabHistory.Text = "History";

        foreach (var page in new[]
        {
            tabRequestInformation,
            tabSubjectInformation,
            tabRequesterContact,
            tabServiceTiming,
            tabIntakeDetails,
            tabReviewDisposition,
            tabHistory
        })
        {
            page.BackColor = Color.White;
            page.Padding = new Padding(0);
        }

        tabMain.Controls.Add(tabRequestInformation);
        tabMain.Controls.Add(tabSubjectInformation);
        tabMain.Controls.Add(tabRequesterContact);
        tabMain.Controls.Add(tabServiceTiming);
        tabMain.Controls.Add(tabIntakeDetails);
        tabMain.Controls.Add(tabReviewDisposition);
        tabMain.Controls.Add(tabHistory);

        splitWorkspace.Panel1.Padding = new Padding(8, 0, 0, 0);
        splitWorkspace.Panel1.Controls.Add(tabMain);

        pnlRightSidebar.Dock = DockStyle.Fill;
        splitWorkspace.Panel2.Controls.Add(pnlRightSidebar);
    }

    private void BuildSidebar()
    {
        pnlRightSidebar.Padding = new Padding(8, 0, 8, 0);

        sidebarLayout.ColumnCount = 1;
        sidebarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        sidebarLayout.Dock = DockStyle.Fill;
        sidebarLayout.RowCount = 4;
        sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
        sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 155F));
        sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
        sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));

        BuildRequestStatusGroup();
        BuildLinkedRecordsGroup();
        BuildQuickNotesGroup();
        BuildAttachmentsGroup();

        sidebarLayout.Controls.Add(grpRequestStatus, 0, 0);
        sidebarLayout.Controls.Add(grpLinkedRecords, 0, 1);
        sidebarLayout.Controls.Add(grpQuickNotes, 0, 2);
        sidebarLayout.Controls.Add(grpAttachments, 0, 3);

        pnlRightSidebar.Controls.Add(sidebarLayout);
    }

    private void BuildRequestStatusGroup()
    {
        ConfigureSidebarGroup(grpRequestStatus, "Request Status");

        statusLayout.ColumnCount = 2;
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54F));
        statusLayout.Dock = DockStyle.Fill;
        statusLayout.RowCount = 4;
        for (var i = 0; i < 4; i++)
            statusLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

        ConfigureSidebarCaption(lblStatusCaption, "Status");
        ConfigureSidebarCaption(lblPriorityCaption, "Priority");
        ConfigureSidebarCaption(lblQualificationCaption, "Qualification");
        ConfigureSidebarCaption(lblDispositionCaption, "Disposition");

        pnlStatusBadge.Dock = DockStyle.Fill;
        pnlStatusBadge.Margin = new Padding(3, 6, 3, 6);
        pnlStatusBadge.Padding = new Padding(4);

        lblStatusValue.Dock = DockStyle.Fill;
        lblStatusValue.Text = "Draft";
        lblStatusValue.TextAlign = ContentAlignment.MiddleCenter;
        lblStatusValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        pnlStatusBadge.Controls.Add(lblStatusValue);

        ConfigureSidebarValue(lblPriorityValue, "Normal");
        ConfigureSidebarValue(lblQualificationValue, "NotReviewed");
        ConfigureSidebarValue(lblDispositionValue, "Not Set");

        statusLayout.Controls.Add(lblStatusCaption, 0, 0);
        statusLayout.Controls.Add(pnlStatusBadge, 1, 0);
        statusLayout.Controls.Add(lblPriorityCaption, 0, 1);
        statusLayout.Controls.Add(lblPriorityValue, 1, 1);
        statusLayout.Controls.Add(lblQualificationCaption, 0, 2);
        statusLayout.Controls.Add(lblQualificationValue, 1, 2);
        statusLayout.Controls.Add(lblDispositionCaption, 0, 3);
        statusLayout.Controls.Add(lblDispositionValue, 1, 3);

        grpRequestStatus.Controls.Add(statusLayout);
    }

    private void BuildLinkedRecordsGroup()
    {
        ConfigureSidebarGroup(grpLinkedRecords, "Linked Records");

        linkedLayout.ColumnCount = 2;
        linkedLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        linkedLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
        linkedLayout.Dock = DockStyle.Fill;
        linkedLayout.RowCount = 3;
        linkedLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
        linkedLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));
        linkedLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333F));

        ConfigureSidebarCaption(lblClientCaption, "Client");
        ConfigureSidebarCaption(lblVehicleCaption, "Vehicle");
        ConfigureSidebarCaption(lblAppraisalCaption, "Appraisal");

        ConfigureLink(lnkClient, "Not Linked");
        ConfigureLink(lnkVehicle, "Not Linked");
        ConfigureLink(lnkAppraisal, "Not Created");

        linkedLayout.Controls.Add(lblClientCaption, 0, 0);
        linkedLayout.Controls.Add(lnkClient, 1, 0);
        linkedLayout.Controls.Add(lblVehicleCaption, 0, 1);
        linkedLayout.Controls.Add(lnkVehicle, 1, 1);
        linkedLayout.Controls.Add(lblAppraisalCaption, 0, 2);
        linkedLayout.Controls.Add(lnkAppraisal, 1, 2);

        grpLinkedRecords.Controls.Add(linkedLayout);
    }

    private void BuildQuickNotesGroup()
    {
        ConfigureSidebarGroup(grpQuickNotes, "Quick Notes");

        quickNotesLayout.ColumnCount = 1;
        quickNotesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        quickNotesLayout.Dock = DockStyle.Fill;
        quickNotesLayout.RowCount = 2;
        quickNotesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        quickNotesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));

        txtQuickNote.Dock = DockStyle.Fill;
        txtQuickNote.Multiline = true;
        txtQuickNote.ScrollBars = ScrollBars.Vertical;
        txtQuickNote.PlaceholderText = "Enter note...";
        txtQuickNote.Margin = new Padding(3, 6, 3, 6);

        btnSaveQuickNote.Text = "Save Note";
        btnSaveQuickNote.Width = 86;
        btnSaveQuickNote.Height = 30;
        btnSaveQuickNote.Anchor = AnchorStyles.Right;
        btnSaveQuickNote.Enabled = false;
        btnSaveQuickNote.Margin = new Padding(3);

        quickNotesLayout.Controls.Add(txtQuickNote, 0, 0);
        quickNotesLayout.Controls.Add(btnSaveQuickNote, 0, 1);

        grpQuickNotes.Controls.Add(quickNotesLayout);
    }

    private void BuildAttachmentsGroup()
    {
        ConfigureSidebarGroup(grpAttachments, "Attachments (0)");

        attachmentsLayout.ColumnCount = 1;
        attachmentsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        attachmentsLayout.Dock = DockStyle.Fill;
        attachmentsLayout.RowCount = 2;
        attachmentsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        attachmentsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        attachmentCommandFlow.Dock = DockStyle.Fill;
        attachmentCommandFlow.FlowDirection = FlowDirection.LeftToRight;
        attachmentCommandFlow.WrapContents = false;
        attachmentCommandFlow.Padding = new Padding(0, 2, 0, 2);

        ConfigureSmallButton(btnAddAttachment, "＋ Add", 70);
        ConfigureSmallButton(btnOpenAttachment, "Open", 62);
        ConfigureSmallButton(btnRemoveAttachment, "Remove", 70);
        btnOpenAttachment.Enabled = false;
        btnRemoveAttachment.Enabled = false;

        attachmentCommandFlow.Controls.Add(btnAddAttachment);
        attachmentCommandFlow.Controls.Add(btnOpenAttachment);
        attachmentCommandFlow.Controls.Add(btnRemoveAttachment);

        lvAttachments.Dock = DockStyle.Fill;
        lvAttachments.FullRowSelect = true;
        lvAttachments.GridLines = false;
        lvAttachments.HideSelection = false;
        lvAttachments.MultiSelect = false;
        lvAttachments.View = View.Details;
        lvAttachments.BorderStyle = BorderStyle.FixedSingle;

        colAttachmentName.Text = "File";
        colAttachmentName.Width = 150;
        colAttachmentSize.Text = "Size";
        colAttachmentSize.Width = 65;
        colAttachmentAdded.Text = "Added";
        colAttachmentAdded.Width = 115;

        lvAttachments.Columns.AddRange(
            new[] { colAttachmentName, colAttachmentSize, colAttachmentAdded });

        attachmentsLayout.Controls.Add(attachmentCommandFlow, 0, 0);
        attachmentsLayout.Controls.Add(lvAttachments, 0, 1);

        grpAttachments.Controls.Add(attachmentsLayout);
    }

    private static void ConfigureSidebarGroup(GroupBox group, string title)
    {
        group.Dock = DockStyle.Fill;
        group.Text = title;
        group.Margin = new Padding(0, 0, 0, 8);
        group.Padding = new Padding(8, 14, 8, 8);
        group.BackColor = Color.White;
    }

    private static void ConfigureSidebarCaption(Label label, string text)
    {
        label.AutoSize = true;
        label.Text = text;
        label.Anchor = AnchorStyles.Left;
        label.Margin = new Padding(3);
    }

    private static void ConfigureSidebarValue(Label label, string text)
    {
        label.AutoSize = false;
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleLeft;
        label.Margin = new Padding(3);
    }

    private static void ConfigureLink(LinkLabel link, string text)
    {
        link.AutoSize = false;
        link.Text = text;
        link.Dock = DockStyle.Fill;
        link.TextAlign = ContentAlignment.MiddleLeft;
        link.Margin = new Padding(3);
        link.LinkBehavior = LinkBehavior.HoverUnderline;
    }

    private static void ConfigureSmallButton(Button button, string text, int width)
    {
        button.Text = text;
        button.Width = width;
        button.Height = 28;
        button.Margin = new Padding(0, 0, 6, 0);
    }

    private void BuildFooter()
    {
        pnlFooter.Dock = DockStyle.Fill;
        pnlFooter.Padding = new Padding(14, 6, 14, 6);
        pnlFooter.BorderStyle = BorderStyle.FixedSingle;

        footerLayout.ColumnCount = 9;
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 64F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
        footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        footerLayout.Dock = DockStyle.Fill;
        footerLayout.RowCount = 1;
        footerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        ConfigureFooterCaption(lblRecordIdCaption, "Record ID:");
        ConfigureFooterValue(lblRecordIdValue, "Not yet saved");

        ConfigureFooterCaption(lblFooterCreatedByCaption, "Created By:");
        ConfigureFooterValue(lblFooterCreatedByValue, "—");
        ConfigureFooterValue(lblFooterCreatedAtValue, "—");

        ConfigureFooterCaption(lblFooterUpdatedByCaption, "Last Updated By:");
        ConfigureFooterValue(lblFooterUpdatedByValue, "—");
        ConfigureFooterValue(lblFooterUpdatedAtValue, "—");

        lblEditMode.Text = "Edit Mode";
        lblEditMode.Dock = DockStyle.Fill;
        lblEditMode.TextAlign = ContentAlignment.MiddleRight;
        lblEditMode.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);

        footerLayout.Controls.Add(lblRecordIdCaption, 0, 0);
        footerLayout.Controls.Add(lblRecordIdValue, 1, 0);
        footerLayout.Controls.Add(lblFooterCreatedByCaption, 2, 0);
        footerLayout.Controls.Add(lblFooterCreatedByValue, 3, 0);
        footerLayout.Controls.Add(lblFooterCreatedAtValue, 4, 0);
        footerLayout.Controls.Add(lblFooterUpdatedByCaption, 5, 0);
        footerLayout.Controls.Add(lblFooterUpdatedByValue, 6, 0);
        footerLayout.Controls.Add(lblFooterUpdatedAtValue, 7, 0);
        footerLayout.Controls.Add(lblEditMode, 8, 0);

        pnlFooter.Controls.Add(footerLayout);
    }

    private static void ConfigureFooterCaption(Label label, string text)
    {
        label.Text = text;
        label.AutoSize = false;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleLeft;
        label.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
    }

    private static void ConfigureFooterValue(Label label, string text)
    {
        label.Text = text;
        label.AutoEllipsis = true;
        label.AutoSize = false;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleLeft;
        label.Font = new Font("Segoe UI", 8F);
    }

    private bool _workspaceSplitterInitialized;

    private const int WorkspacePanel1MinSize = 760;
    private const int WorkspacePanel2MinSize = 250;
    private const int WorkspacePreferredSplitterDistance = 1110;

    private void ApplyWorkspaceSplitterLayout()
    {
        if (_workspaceSplitterInitialized || splitWorkspace.IsDisposed)
            return;

        var total = splitWorkspace.Orientation == Orientation.Vertical
            ? splitWorkspace.Width
            : splitWorkspace.Height;

        if (total <= 0)
            return;

        var panel1Min = WorkspacePanel1MinSize;
        var panel2Min = WorkspacePanel2MinSize;

        // Ensure constraints are always valid for current size.
        if (panel1Min + panel2Min > total)
        {
            panel1Min = Math.Min(panel1Min, total);
            panel2Min = Math.Max(0, total - panel1Min);
        }

        splitWorkspace.Panel1MinSize = panel1Min;
        splitWorkspace.Panel2MinSize = panel2Min;

        var minDistance = splitWorkspace.Panel1MinSize;
        var maxDistance = total - splitWorkspace.Panel2MinSize;
        var distance = Math.Clamp(WorkspacePreferredSplitterDistance, minDistance, maxDistance);

        splitWorkspace.SplitterDistance = distance;
        _workspaceSplitterInitialized = true;
    }
}
