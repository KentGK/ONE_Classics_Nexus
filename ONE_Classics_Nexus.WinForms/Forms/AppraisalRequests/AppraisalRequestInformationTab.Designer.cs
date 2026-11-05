using System.Drawing;
using System.Windows.Forms;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

partial class AppraisalRequestInformationTab
{
    private System.ComponentModel.IContainer? components = null;

    private TableLayoutPanel rootLayout = null!;
    private TableLayoutPanel mainGrid = null!;
    private TableLayoutPanel leftColumn = null!;
    private TableLayoutPanel centerColumn = null!;
    private TableLayoutPanel rightColumn = null!;

    private GroupBox grpRequestOverview = null!;
    private GroupBox grpPreliminaryValue = null!;
    private GroupBox grpRequesterClient = null!;
    private GroupBox grpPreferredSchedule = null!;
    private GroupBox grpLocationAccess = null!;
    private GroupBox grpReferral = null!;

    private TableLayoutPanel requestOverviewLayout = null!;
    private TableLayoutPanel preliminaryValueLayout = null!;
    private TableLayoutPanel requesterClientLayout = null!;
    private TableLayoutPanel scheduleLayout = null!;
    private TableLayoutPanel locationAccessLayout = null!;
    private TableLayoutPanel referralLayout = null!;

    private Label lblProvisionalNotice = null!;

    private Label lblRequestPurposeDescription = null!;
    private TextBox txtRequestPurposeDescription = null!;
    private Label lblRequestedAppraisalType = null!;
    private ComboBox cboRequestedAppraisalType = null!;
    private Label lblStatedPurpose = null!;
    private ComboBox cboStatedPurpose = null!;
    private Label lblRequestedIntendedUse = null!;
    private ComboBox cboRequestedIntendedUse = null!;
    private Label lblStatedIntendedUsers = null!;
    private TextBox txtStatedIntendedUsers = null!;
    private Label lblSpecialRequestsNotes = null!;
    private TextBox txtSpecialRequestsNotes = null!;

    private Label lblPreliminaryValueLow = null!;
    private NumericUpDown nudPreliminaryValueLow = null!;
    private Label lblPreliminaryValueHigh = null!;
    private NumericUpDown nudPreliminaryValueHigh = null!;
    private Label lblCurrency = null!;
    private ComboBox cboCurrency = null!;
    private Label lblPreliminaryValueNotes = null!;
    private TextBox txtPreliminaryValueNotes = null!;

    private Label lblClient = null!;
    private TextBox txtClient = null!;
    private Button btnBrowseClient = null!;
    private Label lblRelationshipToClient = null!;
    private ComboBox cboRelationshipToClient = null!;
    private Label lblPrimaryContact = null!;
    private TextBox txtPrimaryContact = null!;
    private Label lblPhone = null!;
    private TextBox txtPhone = null!;
    private Label lblEmail = null!;
    private TextBox txtEmail = null!;
    private CheckBox chkAddToContacts = null!;

    private Label lblNeededBy = null!;
    private DateTimePicker dtpNeededBy = null!;
    private Label lblFlexibility = null!;
    private ComboBox cboFlexibility = null!;
    private Label lblPreferredInspectionWindow = null!;
    private ComboBox cboPreferredInspectionWindow = null!;
    private Label lblTimingNotes = null!;
    private TextBox txtTimingNotes = null!;

    private Label lblSubjectLocation = null!;
    private ComboBox cboSubjectLocation = null!;
    private Label lblAddress = null!;
    private TextBox txtAddress = null!;
    private Label lblAccessAvailability = null!;
    private TextBox txtAccessAvailability = null!;
    private Label lblWillVehicleBeDriven = null!;
    private ComboBox cboWillVehicleBeDriven = null!;
    private Label lblTransportRequired = null!;
    private ComboBox cboTransportRequired = null!;

    private Label lblReferredBy = null!;
    private ComboBox cboReferredBy = null!;
    private Label lblReferralContact = null!;
    private TextBox txtReferralContact = null!;
    private Label lblReferralOther = null!;
    private TextBox txtReferralOther = null!;

    private ErrorProvider errorProvider = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        rootLayout = new TableLayoutPanel();
        lblProvisionalNotice = new Label();
        mainGrid = new TableLayoutPanel();
        leftColumn = new TableLayoutPanel();
        grpRequestOverview = new GroupBox();
        grpPreliminaryValue = new GroupBox();
        centerColumn = new TableLayoutPanel();
        grpRequesterClient = new GroupBox();
        grpPreferredSchedule = new GroupBox();
        rightColumn = new TableLayoutPanel();
        grpLocationAccess = new GroupBox();
        grpReferral = new GroupBox();
        requestOverviewLayout = new TableLayoutPanel();
        preliminaryValueLayout = new TableLayoutPanel();
        requesterClientLayout = new TableLayoutPanel();
        scheduleLayout = new TableLayoutPanel();
        locationAccessLayout = new TableLayoutPanel();
        referralLayout = new TableLayoutPanel();
        lblRequestPurposeDescription = new Label();
        txtRequestPurposeDescription = new TextBox();
        lblRequestedAppraisalType = new Label();
        cboRequestedAppraisalType = new ComboBox();
        lblStatedPurpose = new Label();
        cboStatedPurpose = new ComboBox();
        lblRequestedIntendedUse = new Label();
        cboRequestedIntendedUse = new ComboBox();
        lblStatedIntendedUsers = new Label();
        txtStatedIntendedUsers = new TextBox();
        lblSpecialRequestsNotes = new Label();
        txtSpecialRequestsNotes = new TextBox();
        lblPreliminaryValueLow = new Label();
        nudPreliminaryValueLow = new NumericUpDown();
        lblPreliminaryValueHigh = new Label();
        nudPreliminaryValueHigh = new NumericUpDown();
        lblCurrency = new Label();
        cboCurrency = new ComboBox();
        lblPreliminaryValueNotes = new Label();
        txtPreliminaryValueNotes = new TextBox();
        lblClient = new Label();
        txtClient = new TextBox();
        btnBrowseClient = new Button();
        lblRelationshipToClient = new Label();
        cboRelationshipToClient = new ComboBox();
        lblPrimaryContact = new Label();
        txtPrimaryContact = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        chkAddToContacts = new CheckBox();
        lblNeededBy = new Label();
        dtpNeededBy = new DateTimePicker();
        lblFlexibility = new Label();
        cboFlexibility = new ComboBox();
        lblPreferredInspectionWindow = new Label();
        cboPreferredInspectionWindow = new ComboBox();
        lblTimingNotes = new Label();
        txtTimingNotes = new TextBox();
        lblSubjectLocation = new Label();
        cboSubjectLocation = new ComboBox();
        lblAddress = new Label();
        txtAddress = new TextBox();
        lblAccessAvailability = new Label();
        txtAccessAvailability = new TextBox();
        lblWillVehicleBeDriven = new Label();
        cboWillVehicleBeDriven = new ComboBox();
        lblTransportRequired = new Label();
        cboTransportRequired = new ComboBox();
        lblReferredBy = new Label();
        cboReferredBy = new ComboBox();
        lblReferralContact = new Label();
        txtReferralContact = new TextBox();
        lblReferralOther = new Label();
        txtReferralOther = new TextBox();
        errorProvider = new ErrorProvider(components);
        rootLayout.SuspendLayout();
        mainGrid.SuspendLayout();
        leftColumn.SuspendLayout();
        centerColumn.SuspendLayout();
        rightColumn.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPreliminaryValueLow).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudPreliminaryValueHigh).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // rootLayout
        // 
        rootLayout.AutoSize = true;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        rootLayout.Controls.Add(lblProvisionalNotice, 0, 0);
        rootLayout.Controls.Add(mainGrid, 0, 1);
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        rootLayout.Size = new Size(200, 100);
        rootLayout.TabIndex = 0;
        // 
        // lblProvisionalNotice
        // 
        lblProvisionalNotice.Location = new Point(3, 0);
        lblProvisionalNotice.Name = "lblProvisionalNotice";
        lblProvisionalNotice.Size = new Size(100, 20);
        lblProvisionalNotice.TabIndex = 0;
        // 
        // mainGrid
        // 
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        mainGrid.Controls.Add(leftColumn, 0, 0);
        mainGrid.Controls.Add(centerColumn, 1, 0);
        mainGrid.Controls.Add(rightColumn, 2, 0);
        mainGrid.Location = new Point(3, 23);
        mainGrid.Name = "mainGrid";
        mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        mainGrid.Size = new Size(194, 74);
        mainGrid.TabIndex = 1;
        // 
        // leftColumn
        // 
        leftColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        leftColumn.Controls.Add(grpRequestOverview, 0, 0);
        leftColumn.Controls.Add(grpPreliminaryValue, 0, 1);
        leftColumn.Location = new Point(3, 3);
        leftColumn.Name = "leftColumn";
        leftColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        leftColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        leftColumn.Size = new Size(14, 68);
        leftColumn.TabIndex = 0;
        // 
        // grpRequestOverview
        // 
        grpRequestOverview.Location = new Point(3, 3);
        grpRequestOverview.Name = "grpRequestOverview";
        grpRequestOverview.Size = new Size(14, 14);
        grpRequestOverview.TabIndex = 0;
        grpRequestOverview.TabStop = false;
        // 
        // grpPreliminaryValue
        // 
        grpPreliminaryValue.Location = new Point(3, 23);
        grpPreliminaryValue.Name = "grpPreliminaryValue";
        grpPreliminaryValue.Size = new Size(14, 42);
        grpPreliminaryValue.TabIndex = 1;
        grpPreliminaryValue.TabStop = false;
        // 
        // centerColumn
        // 
        centerColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        centerColumn.Controls.Add(grpRequesterClient, 0, 0);
        centerColumn.Controls.Add(grpPreferredSchedule, 0, 1);
        centerColumn.Location = new Point(23, 3);
        centerColumn.Name = "centerColumn";
        centerColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        centerColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        centerColumn.Size = new Size(14, 68);
        centerColumn.TabIndex = 1;
        // 
        // grpRequesterClient
        // 
        grpRequesterClient.Location = new Point(3, 3);
        grpRequesterClient.Name = "grpRequesterClient";
        grpRequesterClient.Size = new Size(14, 14);
        grpRequesterClient.TabIndex = 0;
        grpRequesterClient.TabStop = false;
        // 
        // grpPreferredSchedule
        // 
        grpPreferredSchedule.Location = new Point(3, 23);
        grpPreferredSchedule.Name = "grpPreferredSchedule";
        grpPreferredSchedule.Size = new Size(14, 42);
        grpPreferredSchedule.TabIndex = 1;
        grpPreferredSchedule.TabStop = false;
        // 
        // rightColumn
        // 
        rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        rightColumn.Controls.Add(grpLocationAccess, 0, 0);
        rightColumn.Controls.Add(grpReferral, 0, 1);
        rightColumn.Location = new Point(43, 3);
        rightColumn.Name = "rightColumn";
        rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        rightColumn.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        rightColumn.Size = new Size(148, 68);
        rightColumn.TabIndex = 2;
        // 
        // grpLocationAccess
        // 
        grpLocationAccess.Location = new Point(3, 3);
        grpLocationAccess.Name = "grpLocationAccess";
        grpLocationAccess.Size = new Size(142, 14);
        grpLocationAccess.TabIndex = 0;
        grpLocationAccess.TabStop = false;
        // 
        // grpReferral
        // 
        grpReferral.Location = new Point(3, 23);
        grpReferral.Name = "grpReferral";
        grpReferral.Size = new Size(142, 42);
        grpReferral.TabIndex = 1;
        grpReferral.TabStop = false;
        // 
        // requestOverviewLayout
        // 
        requestOverviewLayout.Location = new Point(0, 0);
        requestOverviewLayout.Name = "requestOverviewLayout";
        requestOverviewLayout.Size = new Size(200, 100);
        requestOverviewLayout.TabIndex = 0;
        // 
        // preliminaryValueLayout
        // 
        preliminaryValueLayout.Location = new Point(0, 0);
        preliminaryValueLayout.Name = "preliminaryValueLayout";
        preliminaryValueLayout.Size = new Size(200, 100);
        preliminaryValueLayout.TabIndex = 0;
        // 
        // requesterClientLayout
        // 
        requesterClientLayout.Location = new Point(0, 0);
        requesterClientLayout.Name = "requesterClientLayout";
        requesterClientLayout.Size = new Size(200, 100);
        requesterClientLayout.TabIndex = 0;
        // 
        // scheduleLayout
        // 
        scheduleLayout.Location = new Point(0, 0);
        scheduleLayout.Name = "scheduleLayout";
        scheduleLayout.Size = new Size(200, 100);
        scheduleLayout.TabIndex = 0;
        // 
        // locationAccessLayout
        // 
        locationAccessLayout.Location = new Point(0, 0);
        locationAccessLayout.Name = "locationAccessLayout";
        locationAccessLayout.Size = new Size(200, 100);
        locationAccessLayout.TabIndex = 0;
        // 
        // referralLayout
        // 
        referralLayout.Location = new Point(0, 0);
        referralLayout.Name = "referralLayout";
        referralLayout.Size = new Size(200, 100);
        referralLayout.TabIndex = 0;
        // 
        // lblRequestPurposeDescription
        // 
        lblRequestPurposeDescription.Location = new Point(0, 0);
        lblRequestPurposeDescription.Name = "lblRequestPurposeDescription";
        lblRequestPurposeDescription.Size = new Size(100, 23);
        lblRequestPurposeDescription.TabIndex = 0;
        // 
        // txtRequestPurposeDescription
        // 
        txtRequestPurposeDescription.Location = new Point(0, 0);
        txtRequestPurposeDescription.Name = "txtRequestPurposeDescription";
        txtRequestPurposeDescription.Size = new Size(100, 23);
        txtRequestPurposeDescription.TabIndex = 0;
        // 
        // lblRequestedAppraisalType
        // 
        lblRequestedAppraisalType.Location = new Point(0, 0);
        lblRequestedAppraisalType.Name = "lblRequestedAppraisalType";
        lblRequestedAppraisalType.Size = new Size(100, 23);
        lblRequestedAppraisalType.TabIndex = 0;
        // 
        // cboRequestedAppraisalType
        // 
        cboRequestedAppraisalType.Location = new Point(0, 0);
        cboRequestedAppraisalType.Name = "cboRequestedAppraisalType";
        cboRequestedAppraisalType.Size = new Size(121, 23);
        cboRequestedAppraisalType.TabIndex = 0;
        // 
        // lblStatedPurpose
        // 
        lblStatedPurpose.Location = new Point(0, 0);
        lblStatedPurpose.Name = "lblStatedPurpose";
        lblStatedPurpose.Size = new Size(100, 23);
        lblStatedPurpose.TabIndex = 0;
        // 
        // cboStatedPurpose
        // 
        cboStatedPurpose.Location = new Point(0, 0);
        cboStatedPurpose.Name = "cboStatedPurpose";
        cboStatedPurpose.Size = new Size(121, 23);
        cboStatedPurpose.TabIndex = 0;
        // 
        // lblRequestedIntendedUse
        // 
        lblRequestedIntendedUse.Location = new Point(0, 0);
        lblRequestedIntendedUse.Name = "lblRequestedIntendedUse";
        lblRequestedIntendedUse.Size = new Size(100, 23);
        lblRequestedIntendedUse.TabIndex = 0;
        // 
        // cboRequestedIntendedUse
        // 
        cboRequestedIntendedUse.Location = new Point(0, 0);
        cboRequestedIntendedUse.Name = "cboRequestedIntendedUse";
        cboRequestedIntendedUse.Size = new Size(121, 23);
        cboRequestedIntendedUse.TabIndex = 0;
        // 
        // lblStatedIntendedUsers
        // 
        lblStatedIntendedUsers.Location = new Point(0, 0);
        lblStatedIntendedUsers.Name = "lblStatedIntendedUsers";
        lblStatedIntendedUsers.Size = new Size(100, 23);
        lblStatedIntendedUsers.TabIndex = 0;
        // 
        // txtStatedIntendedUsers
        // 
        txtStatedIntendedUsers.Location = new Point(0, 0);
        txtStatedIntendedUsers.Name = "txtStatedIntendedUsers";
        txtStatedIntendedUsers.Size = new Size(100, 23);
        txtStatedIntendedUsers.TabIndex = 0;
        // 
        // lblSpecialRequestsNotes
        // 
        lblSpecialRequestsNotes.Location = new Point(0, 0);
        lblSpecialRequestsNotes.Name = "lblSpecialRequestsNotes";
        lblSpecialRequestsNotes.Size = new Size(100, 23);
        lblSpecialRequestsNotes.TabIndex = 0;
        // 
        // txtSpecialRequestsNotes
        // 
        txtSpecialRequestsNotes.Location = new Point(0, 0);
        txtSpecialRequestsNotes.Name = "txtSpecialRequestsNotes";
        txtSpecialRequestsNotes.Size = new Size(100, 23);
        txtSpecialRequestsNotes.TabIndex = 0;
        // 
        // lblPreliminaryValueLow
        // 
        lblPreliminaryValueLow.Location = new Point(0, 0);
        lblPreliminaryValueLow.Name = "lblPreliminaryValueLow";
        lblPreliminaryValueLow.Size = new Size(100, 23);
        lblPreliminaryValueLow.TabIndex = 0;
        // 
        // nudPreliminaryValueLow
        // 
        nudPreliminaryValueLow.Location = new Point(0, 0);
        nudPreliminaryValueLow.Name = "nudPreliminaryValueLow";
        nudPreliminaryValueLow.Size = new Size(120, 23);
        nudPreliminaryValueLow.TabIndex = 0;
        // 
        // lblPreliminaryValueHigh
        // 
        lblPreliminaryValueHigh.Location = new Point(0, 0);
        lblPreliminaryValueHigh.Name = "lblPreliminaryValueHigh";
        lblPreliminaryValueHigh.Size = new Size(100, 23);
        lblPreliminaryValueHigh.TabIndex = 0;
        // 
        // nudPreliminaryValueHigh
        // 
        nudPreliminaryValueHigh.Location = new Point(0, 0);
        nudPreliminaryValueHigh.Name = "nudPreliminaryValueHigh";
        nudPreliminaryValueHigh.Size = new Size(120, 23);
        nudPreliminaryValueHigh.TabIndex = 0;
        // 
        // lblCurrency
        // 
        lblCurrency.Location = new Point(0, 0);
        lblCurrency.Name = "lblCurrency";
        lblCurrency.Size = new Size(100, 23);
        lblCurrency.TabIndex = 0;
        // 
        // cboCurrency
        // 
        cboCurrency.Location = new Point(0, 0);
        cboCurrency.Name = "cboCurrency";
        cboCurrency.Size = new Size(121, 23);
        cboCurrency.TabIndex = 0;
        // 
        // lblPreliminaryValueNotes
        // 
        lblPreliminaryValueNotes.Location = new Point(0, 0);
        lblPreliminaryValueNotes.Name = "lblPreliminaryValueNotes";
        lblPreliminaryValueNotes.Size = new Size(100, 23);
        lblPreliminaryValueNotes.TabIndex = 0;
        // 
        // txtPreliminaryValueNotes
        // 
        txtPreliminaryValueNotes.Location = new Point(0, 0);
        txtPreliminaryValueNotes.Name = "txtPreliminaryValueNotes";
        txtPreliminaryValueNotes.Size = new Size(100, 23);
        txtPreliminaryValueNotes.TabIndex = 0;
        // 
        // lblClient
        // 
        lblClient.Location = new Point(0, 0);
        lblClient.Name = "lblClient";
        lblClient.Size = new Size(100, 23);
        lblClient.TabIndex = 0;
        // 
        // txtClient
        // 
        txtClient.Location = new Point(0, 0);
        txtClient.Name = "txtClient";
        txtClient.Size = new Size(100, 23);
        txtClient.TabIndex = 0;
        // 
        // btnBrowseClient
        // 
        btnBrowseClient.Location = new Point(0, 0);
        btnBrowseClient.Name = "btnBrowseClient";
        btnBrowseClient.Size = new Size(75, 23);
        btnBrowseClient.TabIndex = 0;
        // 
        // lblRelationshipToClient
        // 
        lblRelationshipToClient.Location = new Point(0, 0);
        lblRelationshipToClient.Name = "lblRelationshipToClient";
        lblRelationshipToClient.Size = new Size(100, 23);
        lblRelationshipToClient.TabIndex = 0;
        // 
        // cboRelationshipToClient
        // 
        cboRelationshipToClient.Location = new Point(0, 0);
        cboRelationshipToClient.Name = "cboRelationshipToClient";
        cboRelationshipToClient.Size = new Size(121, 23);
        cboRelationshipToClient.TabIndex = 0;
        // 
        // lblPrimaryContact
        // 
        lblPrimaryContact.Location = new Point(0, 0);
        lblPrimaryContact.Name = "lblPrimaryContact";
        lblPrimaryContact.Size = new Size(100, 23);
        lblPrimaryContact.TabIndex = 0;
        // 
        // txtPrimaryContact
        // 
        txtPrimaryContact.Location = new Point(0, 0);
        txtPrimaryContact.Name = "txtPrimaryContact";
        txtPrimaryContact.Size = new Size(100, 23);
        txtPrimaryContact.TabIndex = 0;
        // 
        // lblPhone
        // 
        lblPhone.Location = new Point(0, 0);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(100, 23);
        lblPhone.TabIndex = 0;
        // 
        // txtPhone
        // 
        txtPhone.Location = new Point(0, 0);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(100, 23);
        txtPhone.TabIndex = 0;
        // 
        // lblEmail
        // 
        lblEmail.Location = new Point(0, 0);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(100, 23);
        lblEmail.TabIndex = 0;
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(0, 0);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(100, 23);
        txtEmail.TabIndex = 0;
        // 
        // chkAddToContacts
        // 
        chkAddToContacts.Location = new Point(0, 0);
        chkAddToContacts.Name = "chkAddToContacts";
        chkAddToContacts.Size = new Size(104, 24);
        chkAddToContacts.TabIndex = 0;
        // 
        // lblNeededBy
        // 
        lblNeededBy.Location = new Point(0, 0);
        lblNeededBy.Name = "lblNeededBy";
        lblNeededBy.Size = new Size(100, 23);
        lblNeededBy.TabIndex = 0;
        // 
        // dtpNeededBy
        // 
        dtpNeededBy.Location = new Point(0, 0);
        dtpNeededBy.Name = "dtpNeededBy";
        dtpNeededBy.Size = new Size(200, 23);
        dtpNeededBy.TabIndex = 0;
        // 
        // lblFlexibility
        // 
        lblFlexibility.Location = new Point(0, 0);
        lblFlexibility.Name = "lblFlexibility";
        lblFlexibility.Size = new Size(100, 23);
        lblFlexibility.TabIndex = 0;
        // 
        // cboFlexibility
        // 
        cboFlexibility.Location = new Point(0, 0);
        cboFlexibility.Name = "cboFlexibility";
        cboFlexibility.Size = new Size(121, 23);
        cboFlexibility.TabIndex = 0;
        // 
        // lblPreferredInspectionWindow
        // 
        lblPreferredInspectionWindow.Location = new Point(0, 0);
        lblPreferredInspectionWindow.Name = "lblPreferredInspectionWindow";
        lblPreferredInspectionWindow.Size = new Size(100, 23);
        lblPreferredInspectionWindow.TabIndex = 0;
        // 
        // cboPreferredInspectionWindow
        // 
        cboPreferredInspectionWindow.Location = new Point(0, 0);
        cboPreferredInspectionWindow.Name = "cboPreferredInspectionWindow";
        cboPreferredInspectionWindow.Size = new Size(121, 23);
        cboPreferredInspectionWindow.TabIndex = 0;
        // 
        // lblTimingNotes
        // 
        lblTimingNotes.Location = new Point(0, 0);
        lblTimingNotes.Name = "lblTimingNotes";
        lblTimingNotes.Size = new Size(100, 23);
        lblTimingNotes.TabIndex = 0;
        // 
        // txtTimingNotes
        // 
        txtTimingNotes.Location = new Point(0, 0);
        txtTimingNotes.Name = "txtTimingNotes";
        txtTimingNotes.Size = new Size(100, 23);
        txtTimingNotes.TabIndex = 0;
        // 
        // lblSubjectLocation
        // 
        lblSubjectLocation.Location = new Point(0, 0);
        lblSubjectLocation.Name = "lblSubjectLocation";
        lblSubjectLocation.Size = new Size(100, 23);
        lblSubjectLocation.TabIndex = 0;
        // 
        // cboSubjectLocation
        // 
        cboSubjectLocation.Location = new Point(0, 0);
        cboSubjectLocation.Name = "cboSubjectLocation";
        cboSubjectLocation.Size = new Size(121, 23);
        cboSubjectLocation.TabIndex = 0;
        // 
        // lblAddress
        // 
        lblAddress.Location = new Point(0, 0);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(100, 23);
        lblAddress.TabIndex = 0;
        // 
        // txtAddress
        // 
        txtAddress.Location = new Point(0, 0);
        txtAddress.Name = "txtAddress";
        txtAddress.Size = new Size(100, 23);
        txtAddress.TabIndex = 0;
        // 
        // lblAccessAvailability
        // 
        lblAccessAvailability.Location = new Point(0, 0);
        lblAccessAvailability.Name = "lblAccessAvailability";
        lblAccessAvailability.Size = new Size(100, 23);
        lblAccessAvailability.TabIndex = 0;
        // 
        // txtAccessAvailability
        // 
        txtAccessAvailability.Location = new Point(0, 0);
        txtAccessAvailability.Name = "txtAccessAvailability";
        txtAccessAvailability.Size = new Size(100, 23);
        txtAccessAvailability.TabIndex = 0;
        // 
        // lblWillVehicleBeDriven
        // 
        lblWillVehicleBeDriven.Location = new Point(0, 0);
        lblWillVehicleBeDriven.Name = "lblWillVehicleBeDriven";
        lblWillVehicleBeDriven.Size = new Size(100, 23);
        lblWillVehicleBeDriven.TabIndex = 0;
        // 
        // cboWillVehicleBeDriven
        // 
        cboWillVehicleBeDriven.Location = new Point(0, 0);
        cboWillVehicleBeDriven.Name = "cboWillVehicleBeDriven";
        cboWillVehicleBeDriven.Size = new Size(121, 23);
        cboWillVehicleBeDriven.TabIndex = 0;
        // 
        // lblTransportRequired
        // 
        lblTransportRequired.Location = new Point(0, 0);
        lblTransportRequired.Name = "lblTransportRequired";
        lblTransportRequired.Size = new Size(100, 23);
        lblTransportRequired.TabIndex = 0;
        // 
        // cboTransportRequired
        // 
        cboTransportRequired.Location = new Point(0, 0);
        cboTransportRequired.Name = "cboTransportRequired";
        cboTransportRequired.Size = new Size(121, 23);
        cboTransportRequired.TabIndex = 0;
        // 
        // lblReferredBy
        // 
        lblReferredBy.Location = new Point(0, 0);
        lblReferredBy.Name = "lblReferredBy";
        lblReferredBy.Size = new Size(100, 23);
        lblReferredBy.TabIndex = 0;
        // 
        // cboReferredBy
        // 
        cboReferredBy.Location = new Point(0, 0);
        cboReferredBy.Name = "cboReferredBy";
        cboReferredBy.Size = new Size(121, 23);
        cboReferredBy.TabIndex = 0;
        // 
        // lblReferralContact
        // 
        lblReferralContact.Location = new Point(0, 0);
        lblReferralContact.Name = "lblReferralContact";
        lblReferralContact.Size = new Size(100, 23);
        lblReferralContact.TabIndex = 0;
        // 
        // txtReferralContact
        // 
        txtReferralContact.Location = new Point(0, 0);
        txtReferralContact.Name = "txtReferralContact";
        txtReferralContact.Size = new Size(100, 23);
        txtReferralContact.TabIndex = 0;
        // 
        // lblReferralOther
        // 
        lblReferralOther.Location = new Point(0, 0);
        lblReferralOther.Name = "lblReferralOther";
        lblReferralOther.Size = new Size(100, 23);
        lblReferralOther.TabIndex = 0;
        // 
        // txtReferralOther
        // 
        txtReferralOther.Location = new Point(0, 0);
        txtReferralOther.Name = "txtReferralOther";
        txtReferralOther.Size = new Size(100, 23);
        txtReferralOther.TabIndex = 0;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // AppraisalRequestInformationTab
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        Controls.Add(rootLayout);
        MinimumSize = new Size(1120, 680);
        Name = "AppraisalRequestInformationTab";
        Padding = new Padding(8);
        Size = new Size(1280, 760);

        ConfigureRootLayout();
        ConfigureNotice();
        ConfigureMainGrid();
        ConfigureColumns();
        ConfigureGroups();

        BuildRequestOverview();
        BuildPreliminaryValue();
        BuildRequesterClient();
        BuildPreferredSchedule();
        BuildLocationAccess();
        BuildReferral();

        rootLayout.ResumeLayout(false);
        mainGrid.ResumeLayout(false);
        leftColumn.ResumeLayout(false);
        centerColumn.ResumeLayout(false);
        rightColumn.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudPreliminaryValueLow).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudPreliminaryValueHigh).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private void ConfigureRootLayout()
    {
        rootLayout.AutoSize = false;
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Clear();
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.RowCount = 2;
        rootLayout.RowStyles.Clear();
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.Margin = Padding.Empty;
        rootLayout.Padding = Padding.Empty;
        rootLayout.Name = "rootLayout";
    }

    private void ConfigureNotice()
    {
        lblProvisionalNotice.AutoSize = false;
        lblProvisionalNotice.Dock = DockStyle.Fill;
        lblProvisionalNotice.Text =
            "Client/requester statements on this tab are preliminary intake data. " +
            "They do not become approved appraisal facts until reviewed during Appraisal problem identification.";
        lblProvisionalNotice.TextAlign = ContentAlignment.MiddleLeft;
        lblProvisionalNotice.Padding = new Padding(8, 0, 8, 0);
        lblProvisionalNotice.Name = "lblProvisionalNotice";
    }

    private void ConfigureMainGrid()
    {
        mainGrid.ColumnCount = 3;
        mainGrid.ColumnStyles.Clear();
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        mainGrid.Dock = DockStyle.Fill;
        mainGrid.RowCount = 1;
        mainGrid.RowStyles.Clear();
        mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainGrid.Margin = Padding.Empty;
        mainGrid.Padding = Padding.Empty;
        mainGrid.Name = "mainGrid";
    }

    private void ConfigureColumns()
    {
        ConfigureColumn(leftColumn);
        ConfigureColumn(centerColumn);
        ConfigureColumn(rightColumn);

        leftColumn.RowStyles.Clear();
        leftColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 68F));
        leftColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 32F));

        centerColumn.RowStyles.Clear();
        centerColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
        centerColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));

        rightColumn.RowStyles.Clear();
        rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

        leftColumn.Margin = new Padding(0, 0, 8, 0);
        centerColumn.Margin = new Padding(4, 0, 4, 0);
        rightColumn.Margin = new Padding(8, 0, 0, 0);
    }

    private static void ConfigureColumn(TableLayoutPanel panel)
    {
        panel.ColumnCount = 1;
        panel.ColumnStyles.Clear();
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        panel.Dock = DockStyle.Fill;
        panel.RowCount = 2;
        panel.RowStyles.Clear();
        panel.Padding = Padding.Empty;
        panel.Name = "columnLayout";
    }

    private void ConfigureGroups()
    {
        ConfigureGroup(grpRequestOverview, "Request Information");
        ConfigureGroup(grpPreliminaryValue, "Scope Summary");
        ConfigureGroup(grpRequesterClient, "Client and Contact");
        ConfigureGroup(grpPreferredSchedule, "Valuation Details");
        ConfigureGroup(grpLocationAccess, "Vehicle and Access");
        ConfigureGroup(grpReferral, "Referral and Intake Source");
    }

    private static void ConfigureGroup(GroupBox group, string text)
    {
        group.Dock = DockStyle.Fill;
        group.Text = text;
        group.Padding = new Padding(10, 16, 10, 10);
        group.Margin = new Padding(0, 0, 0, 8);
        group.BackColor = Color.White;
    }

    private void BuildRequestOverview()
    {
        ConfigureEditorGrid(requestOverviewLayout, 2, 12);
        requestOverviewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        requestOverviewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));

        ConfigureLabel(lblRequestPurposeDescription, "Request Purpose / Description");
        ConfigureMultiline(txtRequestPurposeDescription, 74);

        ConfigureLabel(lblRequestedAppraisalType, "Appraisal Type *");
        ConfigureCombo(cboRequestedAppraisalType);

        ConfigureLabel(lblStatedPurpose, "Value Definition *");
        ConfigureCombo(cboStatedPurpose);

        ConfigureLabel(lblRequestedIntendedUse, "Proposed Intended Use *");
        ConfigureCombo(cboRequestedIntendedUse);

        ConfigureLabel(lblStatedIntendedUsers, "Intended Users (Stated)");
        ConfigureTextBox(txtStatedIntendedUsers);

        ConfigureLabel(lblSpecialRequestsNotes, "Special Requests / Notes");
        ConfigureMultiline(txtSpecialRequestsNotes, 68);

        AddSpanningLabel(requestOverviewLayout, lblRequestPurposeDescription, 0);
        AddSpanningControl(requestOverviewLayout, txtRequestPurposeDescription, 1);

        AddLabeledControl(requestOverviewLayout, lblRequestedAppraisalType, cboRequestedAppraisalType, 2);
        AddLabeledControl(requestOverviewLayout, lblStatedPurpose, cboStatedPurpose, 3);
        AddLabeledControl(requestOverviewLayout, lblRequestedIntendedUse, cboRequestedIntendedUse, 4);
        AddLabeledControl(requestOverviewLayout, lblStatedIntendedUsers, txtStatedIntendedUsers, 5);

        AddSpanningLabel(requestOverviewLayout, lblSpecialRequestsNotes, 6);
        AddSpanningControl(requestOverviewLayout, txtSpecialRequestsNotes, 7);

        requestOverviewLayout.RowStyles.Clear();
        requestOverviewLayout.RowCount = 8;
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        requestOverviewLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        grpRequestOverview.Controls.Add(requestOverviewLayout);
    }

    private void BuildPreliminaryValue()
    {
        ConfigureEditorGrid(preliminaryValueLayout, 2, 5);
        preliminaryValueLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        preliminaryValueLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));

        ConfigureLabel(lblPreliminaryValueLow, "Estimated Low");
        ConfigureCurrencyNumeric(nudPreliminaryValueLow);

        ConfigureLabel(lblPreliminaryValueHigh, "Estimated High");
        ConfigureCurrencyNumeric(nudPreliminaryValueHigh);

        ConfigureLabel(lblCurrency, "Currency");
        ConfigureCombo(cboCurrency);

        ConfigureLabel(lblPreliminaryValueNotes, "Notes");
        ConfigureMultiline(txtPreliminaryValueNotes, 60);

        AddLabeledControl(preliminaryValueLayout, lblPreliminaryValueLow, nudPreliminaryValueLow, 0);
        AddLabeledControl(preliminaryValueLayout, lblPreliminaryValueHigh, nudPreliminaryValueHigh, 1);
        AddLabeledControl(preliminaryValueLayout, lblCurrency, cboCurrency, 2);
        AddSpanningLabel(preliminaryValueLayout, lblPreliminaryValueNotes, 3);
        AddSpanningControl(preliminaryValueLayout, txtPreliminaryValueNotes, 4);

        preliminaryValueLayout.RowStyles.Clear();
        preliminaryValueLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        preliminaryValueLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        preliminaryValueLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        preliminaryValueLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        preliminaryValueLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        grpPreliminaryValue.Controls.Add(preliminaryValueLayout);
    }

    private void BuildRequesterClient()
    {
        requesterClientLayout.ColumnCount = 3;
        requesterClientLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
        requesterClientLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
        requesterClientLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38F));
        requesterClientLayout.Dock = DockStyle.Fill;
        requesterClientLayout.RowCount = 7;
        requesterClientLayout.Padding = Padding.Empty;

        ConfigureLabel(lblClient, "Client *");
        ConfigureTextBox(txtClient);
        txtClient.ReadOnly = true;

        btnBrowseClient.Dock = DockStyle.Fill;
        btnBrowseClient.Text = "...";
        btnBrowseClient.Margin = new Padding(4, 3, 0, 3);
        btnBrowseClient.Name = "btnBrowseClient";

        ConfigureLabel(lblRelationshipToClient, "Relationship to Client");
        ConfigureCombo(cboRelationshipToClient);

        ConfigureLabel(lblPrimaryContact, "Primary Contact *");
        ConfigureTextBox(txtPrimaryContact);

        ConfigureLabel(lblPhone, "Phone");
        ConfigureTextBox(txtPhone);

        ConfigureLabel(lblEmail, "Email");
        ConfigureTextBox(txtEmail);

        chkAddToContacts.AutoSize = true;
        chkAddToContacts.Text = "Add to Contacts";
        chkAddToContacts.Anchor = AnchorStyles.Left;
        chkAddToContacts.Margin = new Padding(3, 8, 3, 3);
        chkAddToContacts.Name = "chkAddToContacts";

        requesterClientLayout.Controls.Add(lblClient, 0, 0);
        requesterClientLayout.Controls.Add(txtClient, 1, 0);
        requesterClientLayout.Controls.Add(btnBrowseClient, 2, 0);

        AddLabeledControlAcrossThree(requesterClientLayout, lblRelationshipToClient, cboRelationshipToClient, 1);
        AddLabeledControlAcrossThree(requesterClientLayout, lblPrimaryContact, txtPrimaryContact, 2);
        AddLabeledControlAcrossThree(requesterClientLayout, lblPhone, txtPhone, 3);
        AddLabeledControlAcrossThree(requesterClientLayout, lblEmail, txtEmail, 4);

        requesterClientLayout.Controls.Add(chkAddToContacts, 1, 5);
        requesterClientLayout.SetColumnSpan(chkAddToContacts, 2);

        for (var i = 0; i < 6; i++)
            requesterClientLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));

        requesterClientLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        grpRequesterClient.Controls.Add(requesterClientLayout);
    }

    private void BuildPreferredSchedule()
    {
        ConfigureEditorGrid(scheduleLayout, 2, 5);
        scheduleLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        scheduleLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));

        ConfigureLabel(lblNeededBy, "Effective Date *");
        dtpNeededBy.Dock = DockStyle.Fill;
        dtpNeededBy.Format = DateTimePickerFormat.Short;
        dtpNeededBy.ShowCheckBox = true;
        dtpNeededBy.Checked = false;
        dtpNeededBy.Margin = new Padding(3);
        dtpNeededBy.Name = "dtpNeededBy";

        ConfigureLabel(lblFlexibility, "Priority *");
        ConfigureCombo(cboFlexibility);

        ConfigureLabel(lblPreferredInspectionWindow, "Preferred Inspection Window");
        ConfigureCombo(cboPreferredInspectionWindow);

        ConfigureLabel(lblTimingNotes, "Other Timing Notes");
        ConfigureMultiline(txtTimingNotes, 64);

        AddLabeledControl(scheduleLayout, lblNeededBy, dtpNeededBy, 0);
        AddLabeledControl(scheduleLayout, lblFlexibility, cboFlexibility, 1);
        AddLabeledControl(scheduleLayout, lblPreferredInspectionWindow, cboPreferredInspectionWindow, 2);
        AddSpanningLabel(scheduleLayout, lblTimingNotes, 3);
        AddSpanningControl(scheduleLayout, txtTimingNotes, 4);

        scheduleLayout.RowStyles.Clear();
        scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        scheduleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        grpPreferredSchedule.Controls.Add(scheduleLayout);
    }

    private void BuildLocationAccess()
    {
        ConfigureEditorGrid(locationAccessLayout, 2, 7);
        locationAccessLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        locationAccessLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

        ConfigureLabel(lblSubjectLocation, "Subject Location");
        ConfigureCombo(cboSubjectLocation);

        ConfigureLabel(lblAddress, "Address");
        ConfigureMultiline(txtAddress, 70);

        ConfigureLabel(lblAccessAvailability, "Access / Availability");
        ConfigureMultiline(txtAccessAvailability, 70);

        ConfigureLabel(lblWillVehicleBeDriven, "Will Vehicle Be Driven?");
        ConfigureCombo(cboWillVehicleBeDriven);

        ConfigureLabel(lblTransportRequired, "Transport Required?");
        ConfigureCombo(cboTransportRequired);

        AddLabeledControl(locationAccessLayout, lblSubjectLocation, cboSubjectLocation, 0);

        AddSpanningLabel(locationAccessLayout, lblAddress, 1);
        AddSpanningControl(locationAccessLayout, txtAddress, 2);

        AddSpanningLabel(locationAccessLayout, lblAccessAvailability, 3);
        AddSpanningControl(locationAccessLayout, txtAccessAvailability, 4);

        AddLabeledControl(locationAccessLayout, lblWillVehicleBeDriven, cboWillVehicleBeDriven, 5);
        AddLabeledControl(locationAccessLayout, lblTransportRequired, cboTransportRequired, 6);

        locationAccessLayout.RowStyles.Clear();
        locationAccessLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        locationAccessLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        locationAccessLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
        locationAccessLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        locationAccessLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        locationAccessLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        locationAccessLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));

        grpLocationAccess.Controls.Add(locationAccessLayout);
    }

    private void BuildReferral()
    {
        ConfigureEditorGrid(referralLayout, 2, 4);
        referralLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        referralLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

        ConfigureLabel(lblReferredBy, "Referred By");
        ConfigureCombo(cboReferredBy);

        ConfigureLabel(lblReferralContact, "Referral Contact");
        ConfigureTextBox(txtReferralContact);

        ConfigureLabel(lblReferralOther, "Other (Please Specify)");
        ConfigureTextBox(txtReferralOther);

        AddLabeledControl(referralLayout, lblReferredBy, cboReferredBy, 0);
        AddLabeledControl(referralLayout, lblReferralContact, txtReferralContact, 1);
        AddLabeledControl(referralLayout, lblReferralOther, txtReferralOther, 2);

        referralLayout.RowStyles.Clear();
        referralLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        referralLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        referralLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        referralLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        grpReferral.Controls.Add(referralLayout);
    }

    private static void ConfigureEditorGrid(TableLayoutPanel panel, int columns, int rows)
    {
        panel.ColumnCount = columns;
        panel.RowCount = rows;
        panel.ColumnStyles.Clear();
        panel.RowStyles.Clear();
        panel.Controls.Clear();
        panel.Dock = DockStyle.Fill;
        panel.Padding = Padding.Empty;
        panel.Margin = Padding.Empty;
    }

    private static void ConfigureLabel(Label label, string text)
    {
        label.AutoSize = true;
        label.Text = text;
        label.Anchor = AnchorStyles.Left;
        label.Margin = new Padding(3, 6, 3, 3);
    }

    private static void ConfigureTextBox(TextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(3);
        textBox.BorderStyle = BorderStyle.FixedSingle;
    }

    private static void ConfigureMultiline(TextBox textBox, int minimumHeight)
    {
        ConfigureTextBox(textBox);
        textBox.Multiline = true;
        textBox.ScrollBars = ScrollBars.Vertical;
        textBox.MinimumSize = new Size(0, minimumHeight);
    }

    private static void ConfigureCombo(ComboBox combo)
    {
        combo.Dock = DockStyle.Fill;
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        combo.Margin = new Padding(3);
        combo.IntegralHeight = false;
        combo.MaxDropDownItems = 12;
    }

    private static void ConfigureCurrencyNumeric(NumericUpDown numeric)
    {
        numeric.Dock = DockStyle.Fill;
        numeric.DecimalPlaces = 2;
        numeric.ThousandsSeparator = true;
        numeric.Maximum = 1000000000m;
        numeric.Minimum = 0m;
        numeric.Increment = 1000m;
        numeric.TextAlign = HorizontalAlignment.Right;
        numeric.Margin = new Padding(3);
    }

    private static void AddLabeledControl(
        TableLayoutPanel panel,
        Label label,
        Control control,
        int row)
    {
        panel.Controls.Add(label, 0, row);
        panel.Controls.Add(control, 1, row);
    }

    private static void AddLabeledControlAcrossThree(
        TableLayoutPanel panel,
        Label label,
        Control control,
        int row)
    {
        panel.Controls.Add(label, 0, row);
        panel.Controls.Add(control, 1, row);
        panel.SetColumnSpan(control, 2);
    }

    private static void AddSpanningLabel(
        TableLayoutPanel panel,
        Label label,
        int row)
    {
        panel.Controls.Add(label, 0, row);
        panel.SetColumnSpan(label, panel.ColumnCount);
    }

    private static void AddSpanningControl(
        TableLayoutPanel panel,
        Control control,
        int row)
    {
        panel.Controls.Add(control, 0, row);
        panel.SetColumnSpan(control, panel.ColumnCount);
    }
}
