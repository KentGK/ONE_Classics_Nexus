// ============================================================
// InspctionWorkspaceForm.Designer.cs
// ============================================================

using ONE_Classics_Nexus.WinForms.Services;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections;

partial class InspectionWorkspaceForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    //
    // Header Controls
    //
    private TableLayoutPanel rootLayout = null!;

    private Panel pnlHeader = null!;
    private Label lblTitle = null!;

    private FlowLayoutPanel headerActions = null!;

    private Button btnNewInspection = null!;
    private Button btnSaveDraft = null!;
    private Button btnCompleteInspection = null!;
    private Button btnDeleteInspection = null!;
    private Button btnRefresh = null!;
    private ProgressBar progressBusy = null!;

    private TableLayoutPanel summaryLayout = null!;

    //
    // Inspection Summary Controls
    //
    private Panel pnlInspectionSummary = null!;
    private Label lblInspectionSummaryTitle = null!;
    private ComboBox cmbInspectionId = null!;
    private ComboBox cmbStatus = null!;
    private ComboBox cmbInspectionType = null!;
    private ComboBox cmbTemplate = null!;
    private TextBox txtLocation = null!;
    private ComboBox cmbInspectedBy = null!;
    private DateTimePicker dtpInspectedOn = null!;

    //
    // Appraisal Summary Controls
    //
    private Panel pnlAppraisalSummary = null!;
    private Label lblAppraisalTitle = null!;
    private Label lblAppraisalIdValue = null!;
    private Label lblAppraisalNumberValue = null!;
    private Label lblAppraisalStatusValue = null!;
    private Label lblAppraisalTypeValue = null!;
    private Label lblEffectiveDateValue = null!;
    private Label lblClientValue = null!;

    //
    // Vehicle Summary Controls
    //
    private Panel pnlVehicleSummary = null!;
    private Label lblVehicleTitle = null!;
    private Label lblVinValue = null!;
    private Label lblYearMakeModelValue = null!;
    private Label lblBodyStyleValue = null!;
    private Label lblDrivetrainValue = null!;
    private Label lblEngineTransmissionValue = null!;
    private Label lblMileageValue = null!;
    private PictureBox pictureVehicle = null!;
    private Label lblVehicleImagePlaceholder = null!;
    private Button btnViewVehicle = null!;

    //
    // Selector Progress Controls
    //
    private Panel pnlSelectorProgress = null!;
    private Label lblSelectorProgress = null!;
    private Label lblSelectorProgressDetail = null!;

    //
    // Workspace Tabs
    //
    private TabControl tabWorkspace = null!;
    private TabPage tabOverview = null!;
    private TabPage tabScores = null!;
    private TabPage tabSections = null!;
    private TabPage tabRedFlags = null!;
    private TabPage tabMedia = null!;
    private TabPage tabNotes = null!;
    private TabPage tabHistory = null!;


    private TableLayoutPanel overviewLayout = null!;
    //
    // Scores Panel Controls
    //
    private Panel pnlScores = null!;
    private Label lblScoresTitle = null!;
    private DataGridView gridScores = null!;
    private Label lblConditionIndexCaption = null!;
    private Label lblConditionIndexValue = null!;
    private Label lblWeightedScoreValue = null!;
    private Button btnRecalculate = null!;

    //
    // Sections Panel Controls
    //
    private Panel pnlSections = null!;
    private Label lblSectionsTitle = null!;
    private DataGridView gridSections = null!;

    //
    // Red Flags Panel Controls
    //
    private Panel pnlRedFlags = null!;
    private Label lblRedFlagsTitle = null!;
    private DataGridView gridRedFlags = null!;

    //
    // Media Panel Controls
    //
    private Panel pnlMedia = null!;
    private Label lblMediaTitle = null!;
    private FlowLayoutPanel mediaFlow = null!;
    private Label lblPhotoCount = null!;
    private Label lblVideoCount = null!;
    private Label lblDocumentCount = null!;
    private Label lblOtherFileCount = null!;
    private Button btnViewAllMedia = null!;

    private TableLayoutPanel rightRailLayout = null!;
    //
    // Highlights Panel Controls
    //
    private Panel pnlHighlights = null!;
    private Label lblHighlightsTitle = null!;
    private Label lblConditionIndexHighlight = null!;
    private Label lblTotalRedFlagsCaption = null!;
    private Label lblTotalRedFlagsValue = null!;
    private Label lblItemsScoredCaption = null!;
    private Label lblItemsScoredValue = null!;
    private Label lblProgressCaption = null!;
    private Label lblProgressValue = null!;
    private ProgressBar progressInspection = null!;
    private Label lblProgressDetail = null!;

    //
    // Quick Notes Panel Controls
    //
    private Panel pnlQuickNotes = null!;
    private Label lblQuickNotesTitle = null!;
    private TextBox txtQuickNotes = null!;

    //
    // Checklist Panel Controls
    //
    private Panel pnlChecklist = null!;
    private Label lblChecklistTitle = null!;
    private CheckedListBox checklist = null!;

    //
    // Footer Panel Controls
    //
    private Panel pnlFooter = null!;
    private Label lblFooterInspectionId = null!;
    private Label lblFooterCreated = null!;
    private Label lblFooterUpdated = null!;
    private Label lblSaveState = null!;
    private Label lblStatus = null!;
    private ErrorProvider errorProvider = null!;

    //
    // InitializeComponent method
    //
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblTitle = new Label();
        headerActions = new FlowLayoutPanel();
        btnNewInspection = new Button();
        btnSaveDraft = new Button();
        btnCompleteInspection = new Button();
        btnDeleteInspection = new Button();
        btnRefresh = new Button();
        progressBusy = new ProgressBar();
        pnlSelectorProgress = new Panel();
        lblSelectorProgress = new Label();
        lblSelectorProgressDetail = new Label();
        pnlInspectionSummary = new Panel();
        cmbInspectionId = new ComboBox();
        cmbStatus = new ComboBox();
        cmbInspectionType = new ComboBox();
        cmbTemplate = new ComboBox();
        txtLocation = new TextBox();
        cmbInspectedBy = new ComboBox();
        dtpInspectedOn = new DateTimePicker();
        pnlAppraisalSummary = new Panel();
        lblAppraisalTitle = new Label();
        lblAppraisalIdValue = new Label();
        lblAppraisalNumberValue = new Label();
        lblAppraisalStatusValue = new Label();
        lblAppraisalTypeValue = new Label();
        lblEffectiveDateValue = new Label();
        lblClientValue = new Label();
        pnlVehicleSummary = new Panel();
        lblVehicleTitle = new Label();
        lblVinValue = new Label();
        lblYearMakeModelValue = new Label();
        lblBodyStyleValue = new Label();
        lblDrivetrainValue = new Label();
        lblEngineTransmissionValue = new Label();
        lblMileageValue = new Label();
        pictureVehicle = new PictureBox();
        lblVehicleImagePlaceholder = new Label();
        btnViewVehicle = new Button();
        tabOverview = new TabPage();
        tabSections = new TabPage();
        tabScores = new TabPage();
        tabRedFlags = new TabPage();
        tabMedia = new TabPage();
        tabNotes = new TabPage();
        tabHistory = new TabPage();
        overviewLayout = new TableLayoutPanel();
        pnlScores = new Panel();
        lblInspectionSummaryTitle = new Label();
        lblScoresTitle = new Label();
        gridScores = new DataGridView();
        lblConditionIndexCaption = new Label();
        lblConditionIndexValue = new Label();
        lblWeightedScoreValue = new Label();
        btnRecalculate = new Button();
        pnlSections = new Panel();
        lblSectionsTitle = new Label();
        gridSections = new DataGridView();
        pnlRedFlags = new Panel();
        lblRedFlagsTitle = new Label();
        gridRedFlags = new DataGridView();
        rightRailLayout = new TableLayoutPanel();
        pnlMedia = new Panel();
        lblMediaTitle = new Label();
        mediaFlow = new FlowLayoutPanel();
        lblPhotoCount = new Label();
        lblVideoCount = new Label();
        lblDocumentCount = new Label();
        lblOtherFileCount = new Label();
        btnViewAllMedia = new Button();
        pnlHighlights = new Panel();
        lblHighlightsTitle = new Label();
        lblConditionIndexHighlight = new Label();
        lblTotalRedFlagsCaption = new Label();
        lblTotalRedFlagsValue = new Label();
        lblItemsScoredCaption = new Label();
        lblItemsScoredValue = new Label();
        lblProgressCaption = new Label();
        lblProgressValue = new Label();
        progressInspection = new ProgressBar();
        lblProgressDetail = new Label();
        pnlQuickNotes = new Panel();
        lblQuickNotesTitle = new Label();
        txtQuickNotes = new TextBox();
        pnlChecklist = new Panel();
        lblChecklistTitle = new Label();
        checklist = new CheckedListBox();
        pnlFooter = new Panel();
        lblFooterInspectionId = new Label();
        lblFooterCreated = new Label();
        lblFooterUpdated = new Label();
        lblSaveState = new Label();
        lblStatus = new Label();
        errorProvider = new ErrorProvider(components);
        summaryLayout = new TableLayoutPanel();
        tabWorkspace = new TabControl();
        pnlHeader = new Panel();
        rootLayout = new TableLayoutPanel();
        ((System.ComponentModel.ISupportInitialize)pictureVehicle).BeginInit();
        ((System.ComponentModel.ISupportInitialize)gridScores).BeginInit();
        ((System.ComponentModel.ISupportInitialize)gridSections).BeginInit();
        ((System.ComponentModel.ISupportInitialize)gridRedFlags).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        rootLayout.SuspendLayout();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(25, 35, 55);
        lblTitle.Location = new Point(24, 8);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(150, 30);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Inspection Workspace";
        // 
        // headerActions
        // 
        headerActions.Location = new Point(0, 0);
        headerActions.Name = "headerActions";
        headerActions.Size = new Size(200, 100);
        headerActions.TabIndex = 0;
        // 
        // btnNewInspection
        // 
        btnNewInspection.Location = new Point(0, 0);
        btnNewInspection.Name = "btnNewInspection";
        btnNewInspection.Size = new Size(75, 23);
        btnNewInspection.TabIndex = 0;
        // 
        // btnSaveDraft
        // 
        btnSaveDraft.Location = new Point(0, 0);
        btnSaveDraft.Name = "btnSaveDraft";
        btnSaveDraft.Size = new Size(75, 23);
        btnSaveDraft.TabIndex = 0;
        // 
        // btnCompleteInspection
        // 
        btnCompleteInspection.Location = new Point(0, 0);
        btnCompleteInspection.Name = "btnCompleteInspection";
        btnCompleteInspection.Size = new Size(75, 23);
        btnCompleteInspection.TabIndex = 0;
        // 
        // btnDeleteInspection
        // 
        btnDeleteInspection.Location = new Point(0, 0);
        btnDeleteInspection.Name = "btnDeleteInspection";
        btnDeleteInspection.Size = new Size(75, 23);
        btnDeleteInspection.TabIndex = 0;
        // 
        // btnRefresh
        // 
        btnRefresh.Location = new Point(0, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(75, 23);
        btnRefresh.TabIndex = 0;
        // 
        // progressBusy
        // 
        progressBusy.Location = new Point(0, 0);
        progressBusy.Name = "progressBusy";
        progressBusy.Size = new Size(100, 23);
        progressBusy.TabIndex = 0;
        // 
        // pnlSelectorProgress
        // 
        pnlSelectorProgress.Location = new Point(0, 0);
        pnlSelectorProgress.Name = "pnlSelectorProgress";
        pnlSelectorProgress.Size = new Size(200, 100);
        pnlSelectorProgress.TabIndex = 0;
        // 
        // lblSelectorProgress
        // 
        lblSelectorProgress.Location = new Point(0, 0);
        lblSelectorProgress.Name = "lblSelectorProgress";
        lblSelectorProgress.Size = new Size(100, 23);
        lblSelectorProgress.TabIndex = 0;
        // 
        // lblSelectorProgressDetail
        // 
        lblSelectorProgressDetail.Location = new Point(0, 0);
        lblSelectorProgressDetail.Name = "lblSelectorProgressDetail";
        lblSelectorProgressDetail.Size = new Size(100, 23);
        lblSelectorProgressDetail.TabIndex = 0;
        // 
        // pnlInspectionSummary
        // 
        pnlInspectionSummary.Location = new Point(0, 0);
        pnlInspectionSummary.Name = "pnlInspectionSummary";
        pnlInspectionSummary.Size = new Size(200, 100);
        pnlInspectionSummary.TabIndex = 0;
        // 
        // cmbInspectionId
        // 
        cmbInspectionId.Location = new Point(0, 0);
        cmbInspectionId.Name = "cmbInspectionId";
        cmbInspectionId.Size = new Size(121, 23);
        cmbInspectionId.TabIndex = 0;
        // 
        // cmbStatus
        // 
        cmbStatus.Location = new Point(0, 0);
        cmbStatus.Name = "cmbStatus";
        cmbStatus.Size = new Size(121, 23);
        cmbStatus.TabIndex = 0;
        // 
        // cmbInspectionType
        // 
        cmbInspectionType.Location = new Point(0, 0);
        cmbInspectionType.Name = "cmbInspectionType";
        cmbInspectionType.Size = new Size(121, 23);
        cmbInspectionType.TabIndex = 0;
        // 
        // cmbTemplate
        // 
        cmbTemplate.Location = new Point(0, 0);
        cmbTemplate.Name = "cmbTemplate";
        cmbTemplate.Size = new Size(121, 23);
        cmbTemplate.TabIndex = 0;
        // 
        // txtLocation
        // 
        txtLocation.Location = new Point(0, 0);
        txtLocation.Name = "txtLocation";
        txtLocation.Size = new Size(100, 23);
        txtLocation.TabIndex = 0;
        // 
        // cmbInspectedBy
        // 
        cmbInspectedBy.Location = new Point(0, 0);
        cmbInspectedBy.Name = "cmbInspectedBy";
        cmbInspectedBy.Size = new Size(121, 23);
        cmbInspectedBy.TabIndex = 0;
        // 
        // dtpInspectedOn
        // 
        dtpInspectedOn.Location = new Point(0, 0);
        dtpInspectedOn.Name = "dtpInspectedOn";
        dtpInspectedOn.Size = new Size(200, 23);
        dtpInspectedOn.TabIndex = 0;
        // 
        // pnlAppraisalSummary
        // 
        pnlAppraisalSummary.Location = new Point(0, 0);
        pnlAppraisalSummary.Name = "pnlAppraisalSummary";
        pnlAppraisalSummary.Size = new Size(200, 100);
        pnlAppraisalSummary.TabIndex = 0;
        // 
        // lblAppraisalTitle
        // 
        lblAppraisalTitle.Location = new Point(0, 0);
        lblAppraisalTitle.Name = "lblAppraisalTitle";
        lblAppraisalTitle.Size = new Size(100, 23);
        lblAppraisalTitle.TabIndex = 0;
        // 
        // lblAppraisalIdValue
        // 
        lblAppraisalIdValue.BorderStyle = BorderStyle.FixedSingle;
        lblAppraisalIdValue.Location = new Point(0, 0);
        lblAppraisalIdValue.Name = "lblAppraisalIdValue";
        lblAppraisalIdValue.Size = new Size(100, 23);
        lblAppraisalIdValue.TabIndex = 0;
        // 
        // lblAppraisalNumberValue
        // 
        lblAppraisalNumberValue.BorderStyle = BorderStyle.FixedSingle;
        lblAppraisalNumberValue.Location = new Point(0, 0);
        lblAppraisalNumberValue.Name = "lblAppraisalNumberValue";
        lblAppraisalNumberValue.Size = new Size(100, 23);
        lblAppraisalNumberValue.TabIndex = 0;
        // 
        // lblAppraisalStatusValue
        // 
        lblAppraisalStatusValue.Location = new Point(0, 0);
        lblAppraisalStatusValue.Name = "lblAppraisalStatusValue";
        lblAppraisalStatusValue.Size = new Size(100, 23);
        lblAppraisalStatusValue.TabIndex = 0;
        // 
        // lblAppraisalTypeValue
        // 
        lblAppraisalTypeValue.Location = new Point(0, 0);
        lblAppraisalTypeValue.Name = "lblAppraisalTypeValue";
        lblAppraisalTypeValue.Size = new Size(100, 23);
        lblAppraisalTypeValue.TabIndex = 0;
        // 
        // lblEffectiveDateValue
        // 
        lblEffectiveDateValue.Location = new Point(0, 0);
        lblEffectiveDateValue.Name = "lblEffectiveDateValue";
        lblEffectiveDateValue.Size = new Size(100, 23);
        lblEffectiveDateValue.TabIndex = 0;
        // 
        // lblClientValue
        // 
        lblClientValue.Location = new Point(0, 0);
        lblClientValue.Name = "lblClientValue";
        lblClientValue.Size = new Size(100, 23);
        lblClientValue.TabIndex = 0;
        // 
        // pnlVehicleSummary
        // 
        pnlVehicleSummary.Location = new Point(0, 0);
        pnlVehicleSummary.Name = "pnlVehicleSummary";
        pnlVehicleSummary.Size = new Size(200, 100);
        pnlVehicleSummary.TabIndex = 0;
        // 
        // lblVehicleTitle
        // 
        lblVehicleTitle.Location = new Point(0, 0);
        lblVehicleTitle.Name = "lblVehicleTitle";
        lblVehicleTitle.Size = new Size(100, 23);
        lblVehicleTitle.TabIndex = 0;
        // 
        // lblVinValue
        // 
        lblVinValue.Location = new Point(0, 0);
        lblVinValue.Name = "lblVinValue";
        lblVinValue.Size = new Size(100, 23);
        lblVinValue.TabIndex = 0;
        // 
        // lblYearMakeModelValue
        // 
        lblYearMakeModelValue.Location = new Point(0, 0);
        lblYearMakeModelValue.Name = "lblYearMakeModelValue";
        lblYearMakeModelValue.Size = new Size(100, 23);
        lblYearMakeModelValue.TabIndex = 0;
        // 
        // lblBodyStyleValue
        // 
        lblBodyStyleValue.Location = new Point(0, 0);
        lblBodyStyleValue.Name = "lblBodyStyleValue";
        lblBodyStyleValue.Size = new Size(100, 23);
        lblBodyStyleValue.TabIndex = 0;
        // 
        // lblDrivetrainValue
        // 
        lblDrivetrainValue.Location = new Point(0, 0);
        lblDrivetrainValue.Name = "lblDrivetrainValue";
        lblDrivetrainValue.Size = new Size(100, 23);
        lblDrivetrainValue.TabIndex = 0;
        // 
        // lblEngineTransmissionValue
        // 
        lblEngineTransmissionValue.Location = new Point(0, 0);
        lblEngineTransmissionValue.Name = "lblEngineTransmissionValue";
        lblEngineTransmissionValue.Size = new Size(100, 23);
        lblEngineTransmissionValue.TabIndex = 0;
        // 
        // lblMileageValue
        // 
        lblMileageValue.Location = new Point(0, 0);
        lblMileageValue.Name = "lblMileageValue";
        lblMileageValue.Size = new Size(100, 23);
        lblMileageValue.TabIndex = 0;
        // 
        // pictureVehicle
        // 
        pictureVehicle.Location = new Point(0, 0);
        pictureVehicle.Name = "pictureVehicle";
        pictureVehicle.Size = new Size(100, 50);
        pictureVehicle.TabIndex = 0;
        pictureVehicle.TabStop = false;
        // 
        // lblVehicleImagePlaceholder
        // 
        lblVehicleImagePlaceholder.Location = new Point(0, 0);
        lblVehicleImagePlaceholder.Name = "lblVehicleImagePlaceholder";
        lblVehicleImagePlaceholder.Size = new Size(100, 23);
        lblVehicleImagePlaceholder.TabIndex = 0;
        // 
        // btnViewVehicle
        // 
        btnViewVehicle.Location = new Point(0, 0);
        btnViewVehicle.Name = "btnViewVehicle";
        btnViewVehicle.Size = new Size(75, 23);
        btnViewVehicle.TabIndex = 0;
        // 
        // tabOverview
        // 
        tabOverview.Location = new Point(0, 0);
        tabOverview.Name = "tabOverview";
        tabOverview.Size = new Size(200, 100);
        tabOverview.TabIndex = 0;
        // 
        // tabSections
        // 
        tabSections.Location = new Point(0, 0);
        tabSections.Name = "tabSections";
        tabSections.Size = new Size(200, 100);
        tabSections.TabIndex = 0;
        // 
        // tabScores
        // 
        tabScores.Location = new Point(0, 0);
        tabScores.Name = "tabScores";
        tabScores.Size = new Size(200, 100);
        tabScores.TabIndex = 0;
        // 
        // tabRedFlags
        // 
        tabRedFlags.Location = new Point(0, 0);
        tabRedFlags.Name = "tabRedFlags";
        tabRedFlags.Size = new Size(200, 100);
        tabRedFlags.TabIndex = 0;
        // 
        // tabMedia
        // 
        tabMedia.Location = new Point(0, 0);
        tabMedia.Name = "tabMedia";
        tabMedia.Size = new Size(200, 100);
        tabMedia.TabIndex = 0;
        // 
        // tabNotes
        // 
        tabNotes.Location = new Point(0, 0);
        tabNotes.Name = "tabNotes";
        tabNotes.Size = new Size(200, 100);
        tabNotes.TabIndex = 0;
        // 
        // tabHistory
        // 
        tabHistory.Location = new Point(0, 0);
        tabHistory.Name = "tabHistory";
        tabHistory.Size = new Size(200, 100);
        tabHistory.TabIndex = 0;
        // 
        // overviewLayout
        // 
        overviewLayout.Location = new Point(0, 0);
        overviewLayout.Name = "overviewLayout";
        overviewLayout.Size = new Size(200, 100);
        overviewLayout.TabIndex = 0;
        // 
        // pnlScores
        // 
        pnlScores.Location = new Point(0, 0);
        pnlScores.Name = "pnlScores";
        pnlScores.Size = new Size(200, 100);
        pnlScores.TabIndex = 0;
        // 
        // lblInspectionSummaryTitle
        // 
        lblInspectionSummaryTitle.Location = new Point(0, 0);
        lblInspectionSummaryTitle.Name = "lblInspectionSummaryTitle";
        lblInspectionSummaryTitle.Size = new Size(100, 23);
        lblInspectionSummaryTitle.TabIndex = 0;
        // 
        // lblScoresTitle
        // 
        lblScoresTitle.Location = new Point(0, 0);
        lblScoresTitle.Name = "lblScoresTitle";
        lblScoresTitle.Size = new Size(100, 23);
        lblScoresTitle.TabIndex = 0;
        // 
        // gridScores
        // 
        gridScores.Location = new Point(0, 0);
        gridScores.Name = "gridScores";
        gridScores.Size = new Size(240, 150);
        gridScores.TabIndex = 0;
        // 
        // lblConditionIndexCaption
        // 
        lblConditionIndexCaption.Location = new Point(0, 0);
        lblConditionIndexCaption.Name = "lblConditionIndexCaption";
        lblConditionIndexCaption.Size = new Size(100, 23);
        lblConditionIndexCaption.TabIndex = 0;
        // 
        // lblConditionIndexValue
        // 
        lblConditionIndexValue.Location = new Point(0, 0);
        lblConditionIndexValue.Name = "lblConditionIndexValue";
        lblConditionIndexValue.Size = new Size(100, 23);
        lblConditionIndexValue.TabIndex = 0;
        // 
        // lblWeightedScoreValue
        // 
        lblWeightedScoreValue.Location = new Point(0, 0);
        lblWeightedScoreValue.Name = "lblWeightedScoreValue";
        lblWeightedScoreValue.Size = new Size(100, 23);
        lblWeightedScoreValue.TabIndex = 0;
        // 
        // btnRecalculate
        // 
        btnRecalculate.Location = new Point(0, 0);
        btnRecalculate.Name = "btnRecalculate";
        btnRecalculate.Size = new Size(75, 23);
        btnRecalculate.TabIndex = 0;
        // 
        // pnlSections
        // 
        pnlSections.Location = new Point(0, 0);
        pnlSections.Name = "pnlSections";
        pnlSections.Size = new Size(200, 100);
        pnlSections.TabIndex = 0;
        // 
        // lblSectionsTitle
        // 
        lblSectionsTitle.Location = new Point(0, 0);
        lblSectionsTitle.Name = "lblSectionsTitle";
        lblSectionsTitle.Size = new Size(100, 23);
        lblSectionsTitle.TabIndex = 0;
        // 
        // gridSections
        // 
        gridSections.Location = new Point(0, 0);
        gridSections.Name = "gridSections";
        gridSections.Size = new Size(240, 150);
        gridSections.TabIndex = 0;
        // 
        // pnlRedFlags
        // 
        pnlRedFlags.Location = new Point(0, 0);
        pnlRedFlags.Name = "pnlRedFlags";
        pnlRedFlags.Size = new Size(200, 100);
        pnlRedFlags.TabIndex = 0;
        // 
        // lblRedFlagsTitle
        // 
        lblRedFlagsTitle.Location = new Point(0, 0);
        lblRedFlagsTitle.Name = "lblRedFlagsTitle";
        lblRedFlagsTitle.Size = new Size(100, 23);
        lblRedFlagsTitle.TabIndex = 0;
        // 
        // gridRedFlags
        // 
        gridRedFlags.Location = new Point(0, 0);
        gridRedFlags.Name = "gridRedFlags";
        gridRedFlags.Size = new Size(240, 150);
        gridRedFlags.TabIndex = 0;
        // 
        // rightRailLayout
        // 
        rightRailLayout.Location = new Point(0, 0);
        rightRailLayout.Name = "rightRailLayout";
        rightRailLayout.Size = new Size(200, 100);
        rightRailLayout.TabIndex = 0;
        // 
        // pnlMedia
        // 
        pnlMedia.Location = new Point(0, 0);
        pnlMedia.Name = "pnlMedia";
        pnlMedia.Size = new Size(200, 100);
        pnlMedia.TabIndex = 0;
        // 
        // lblMediaTitle
        // 
        lblMediaTitle.Location = new Point(0, 0);
        lblMediaTitle.Name = "lblMediaTitle";
        lblMediaTitle.Size = new Size(100, 23);
        lblMediaTitle.TabIndex = 0;
        // 
        // mediaFlow
        // 
        mediaFlow.Location = new Point(0, 0);
        mediaFlow.Name = "mediaFlow";
        mediaFlow.Size = new Size(200, 100);
        mediaFlow.TabIndex = 0;
        // 
        // lblPhotoCount
        // 
        lblPhotoCount.Location = new Point(0, 0);
        lblPhotoCount.Name = "lblPhotoCount";
        lblPhotoCount.Size = new Size(100, 23);
        lblPhotoCount.TabIndex = 0;
        // 
        // lblVideoCount
        // 
        lblVideoCount.Location = new Point(0, 0);
        lblVideoCount.Name = "lblVideoCount";
        lblVideoCount.Size = new Size(100, 23);
        lblVideoCount.TabIndex = 0;
        // 
        // lblDocumentCount
        // 
        lblDocumentCount.Location = new Point(0, 0);
        lblDocumentCount.Name = "lblDocumentCount";
        lblDocumentCount.Size = new Size(100, 23);
        lblDocumentCount.TabIndex = 0;
        // 
        // lblOtherFileCount
        // 
        lblOtherFileCount.Location = new Point(0, 0);
        lblOtherFileCount.Name = "lblOtherFileCount";
        lblOtherFileCount.Size = new Size(100, 23);
        lblOtherFileCount.TabIndex = 0;
        // 
        // btnViewAllMedia
        // 
        btnViewAllMedia.Location = new Point(0, 0);
        btnViewAllMedia.Name = "btnViewAllMedia";
        btnViewAllMedia.Size = new Size(75, 23);
        btnViewAllMedia.TabIndex = 0;
        // 
        // pnlHighlights
        // 
        pnlHighlights.Location = new Point(0, 0);
        pnlHighlights.Name = "pnlHighlights";
        pnlHighlights.Size = new Size(200, 100);
        pnlHighlights.TabIndex = 0;
        // 
        // lblHighlightsTitle
        // 
        lblHighlightsTitle.Location = new Point(0, 0);
        lblHighlightsTitle.Name = "lblHighlightsTitle";
        lblHighlightsTitle.Size = new Size(100, 23);
        lblHighlightsTitle.TabIndex = 0;
        // 
        // lblConditionIndexHighlight
        // 
        lblConditionIndexHighlight.Location = new Point(0, 0);
        lblConditionIndexHighlight.Name = "lblConditionIndexHighlight";
        lblConditionIndexHighlight.Size = new Size(100, 23);
        lblConditionIndexHighlight.TabIndex = 0;
        // 
        // lblTotalRedFlagsCaption
        // 
        lblTotalRedFlagsCaption.Location = new Point(0, 0);
        lblTotalRedFlagsCaption.Name = "lblTotalRedFlagsCaption";
        lblTotalRedFlagsCaption.Size = new Size(100, 23);
        lblTotalRedFlagsCaption.TabIndex = 0;
        // 
        // lblTotalRedFlagsValue
        // 
        lblTotalRedFlagsValue.Location = new Point(0, 0);
        lblTotalRedFlagsValue.Name = "lblTotalRedFlagsValue";
        lblTotalRedFlagsValue.Size = new Size(100, 23);
        lblTotalRedFlagsValue.TabIndex = 0;
        // 
        // lblItemsScoredCaption
        // 
        lblItemsScoredCaption.Location = new Point(0, 0);
        lblItemsScoredCaption.Name = "lblItemsScoredCaption";
        lblItemsScoredCaption.Size = new Size(100, 23);
        lblItemsScoredCaption.TabIndex = 0;
        // 
        // lblItemsScoredValue
        // 
        lblItemsScoredValue.Location = new Point(0, 0);
        lblItemsScoredValue.Name = "lblItemsScoredValue";
        lblItemsScoredValue.Size = new Size(100, 23);
        lblItemsScoredValue.TabIndex = 0;
        // 
        // lblProgressCaption
        // 
        lblProgressCaption.Location = new Point(0, 0);
        lblProgressCaption.Name = "lblProgressCaption";
        lblProgressCaption.Size = new Size(100, 23);
        lblProgressCaption.TabIndex = 0;
        // 
        // lblProgressValue
        // 
        lblProgressValue.Location = new Point(0, 0);
        lblProgressValue.Name = "lblProgressValue";
        lblProgressValue.Size = new Size(100, 23);
        lblProgressValue.TabIndex = 0;
        // 
        // progressInspection
        // 
        progressInspection.Location = new Point(0, 0);
        progressInspection.Name = "progressInspection";
        progressInspection.Size = new Size(100, 23);
        progressInspection.TabIndex = 0;
        // 
        // lblProgressDetail
        // 
        lblProgressDetail.Location = new Point(0, 0);
        lblProgressDetail.Name = "lblProgressDetail";
        lblProgressDetail.Size = new Size(100, 23);
        lblProgressDetail.TabIndex = 0;
        // 
        // pnlQuickNotes
        // 
        pnlQuickNotes.Location = new Point(0, 0);
        pnlQuickNotes.Name = "pnlQuickNotes";
        pnlQuickNotes.Size = new Size(200, 100);
        pnlQuickNotes.TabIndex = 0;
        // 
        // lblQuickNotesTitle
        // 
        lblQuickNotesTitle.Location = new Point(0, 0);
        lblQuickNotesTitle.Name = "lblQuickNotesTitle";
        lblQuickNotesTitle.Size = new Size(100, 23);
        lblQuickNotesTitle.TabIndex = 0;
        // 
        // txtQuickNotes
        // 
        txtQuickNotes.Location = new Point(0, 0);
        txtQuickNotes.Name = "txtQuickNotes";
        txtQuickNotes.Size = new Size(100, 23);
        txtQuickNotes.TabIndex = 0;
        // 
        // pnlChecklist
        // 
        pnlChecklist.Location = new Point(0, 0);
        pnlChecklist.Name = "pnlChecklist";
        pnlChecklist.Size = new Size(200, 100);
        pnlChecklist.TabIndex = 0;
        // 
        // lblChecklistTitle
        // 
        lblChecklistTitle.Location = new Point(0, 0);
        lblChecklistTitle.Name = "lblChecklistTitle";
        lblChecklistTitle.Size = new Size(100, 23);
        lblChecklistTitle.TabIndex = 0;
        // 
        // checklist
        // 
        checklist.Location = new Point(0, 0);
        checklist.Name = "checklist";
        checklist.Size = new Size(120, 96);
        checklist.TabIndex = 0;
        // 
        // pnlFooter
        // 
        pnlFooter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        pnlFooter.Location = new Point(15, 851);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(466, 34);
        pnlFooter.TabIndex = 4;
        // 
        // lblFooterInspectionId
        // 
        lblFooterInspectionId.Location = new Point(0, 0);
        lblFooterInspectionId.Name = "lblFooterInspectionId";
        lblFooterInspectionId.Size = new Size(100, 23);
        lblFooterInspectionId.TabIndex = 0;
        // 
        // lblFooterCreated
        // 
        lblFooterCreated.Location = new Point(0, 0);
        lblFooterCreated.Name = "lblFooterCreated";
        lblFooterCreated.Size = new Size(100, 23);
        lblFooterCreated.TabIndex = 0;
        // 
        // lblFooterUpdated
        // 
        lblFooterUpdated.Location = new Point(0, 0);
        lblFooterUpdated.Name = "lblFooterUpdated";
        lblFooterUpdated.Size = new Size(100, 23);
        lblFooterUpdated.TabIndex = 0;
        // 
        // lblSaveState
        // 
        lblSaveState.Location = new Point(0, 0);
        lblSaveState.Name = "lblSaveState";
        lblSaveState.Size = new Size(100, 23);
        lblSaveState.TabIndex = 0;
        // 
        // lblStatus
        // 
        lblStatus.Location = new Point(0, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(100, 23);
        lblStatus.TabIndex = 0;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // summaryLayout
        // 
        summaryLayout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        summaryLayout.Location = new Point(15, 186);
        summaryLayout.Name = "summaryLayout";
        summaryLayout.Size = new Size(1390, 184);
        summaryLayout.TabIndex = 2;
        // 
        // tabWorkspace
        // 
        tabWorkspace.Location = new Point(15, 376);
        tabWorkspace.Name = "tabWorkspace";
        tabWorkspace.SelectedIndex = 0;
        tabWorkspace.Size = new Size(1390, 458);
        tabWorkspace.TabIndex = 3;
        // 
        // pnlHeader
        // 
        pnlHeader.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        pnlHeader.Location = new Point(15, 15);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1390, 52);
        pnlHeader.TabIndex = 0;
        // 
        // rootLayout
        // 
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(pnlHeader, 0, 0);
        rootLayout.Controls.Add(summaryLayout, 0, 2);
        rootLayout.Controls.Add(tabWorkspace, 0, 3);
        rootLayout.Controls.Add(pnlFooter, 0, 4);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(12);
        rootLayout.RowCount = 5;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 86F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 9F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 266F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        rootLayout.Size = new Size(1420, 900);
        rootLayout.TabIndex = 0;
        // 
        // InspectionWorkspaceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1420, 900);
        Controls.Add(rootLayout);
        FormBorderStyle = FormBorderStyle.None;
        Name = "InspectionWorkspaceForm";
        Text = "Inspection Workspace";
        ((System.ComponentModel.ISupportInitialize)pictureVehicle).EndInit();
        ((System.ComponentModel.ISupportInitialize)gridScores).EndInit();
        ((System.ComponentModel.ISupportInitialize)gridSections).EndInit();
        ((System.ComponentModel.ISupportInitialize)gridRedFlags).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        rootLayout.ResumeLayout(false);
        ResumeLayout(false);
    }

    private void BuildHeader()
    {
        pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlHeader.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);

        lblTitle.AutoSize = true;
        lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
        lblTitle.Location = new System.Drawing.Point(16, 8);
        lblTitle.Text = "Inspection Overview";

        headerActions.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Right;
        headerActions.AutoSize = true;
        headerActions.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
        headerActions.Location = new System.Drawing.Point(670, 8);
        headerActions.WrapContents = false;

        ConfigureHeaderButton(btnNewInspection, "New Inspection", 132);
        ConfigureHeaderButton(btnSaveDraft, "Save Draft", 108);
        ConfigureHeaderButton(btnCompleteInspection, "Complete Inspection", 156);
        ConfigureHeaderButton(btnDeleteInspection, "Delete (Soft)", 120);
        ConfigureHeaderButton(btnRefresh, "Refresh", 94);

        headerActions.Controls.Add(btnNewInspection);
        headerActions.Controls.Add(btnSaveDraft);
        headerActions.Controls.Add(btnCompleteInspection);
        headerActions.Controls.Add(btnDeleteInspection);
        headerActions.Controls.Add(btnRefresh);

        progressBusy.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Right;
        progressBusy.Location = new System.Drawing.Point(1230, 45);
        progressBusy.MarqueeAnimationSpeed = 25;
        progressBusy.Size = new System.Drawing.Size(150, 8);
        progressBusy.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
        progressBusy.Visible = false;

        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(headerActions);
        pnlHeader.Controls.Add(progressBusy);
    }

    private void BuildSelector()
    {
        dtpInspectedOn.ShowCheckBox = true;
        dtpInspectedOn.Format = System.Windows.Forms.DateTimePickerFormat.Short;

        pnlSelectorProgress.Size = new System.Drawing.Size(165, 44);
        pnlSelectorProgress.Margin = new System.Windows.Forms.Padding(18, 0, 0, 0);
        pnlSelectorProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblSelectorProgress.AutoSize = false;
        lblSelectorProgress.Dock = System.Windows.Forms.DockStyle.Top;
        lblSelectorProgress.Height = 23;
        lblSelectorProgress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        lblSelectorProgress.Text = "Progress 0%";
        lblSelectorProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        lblSelectorProgressDetail.AutoSize = false;
        lblSelectorProgressDetail.Dock = System.Windows.Forms.DockStyle.Fill;
        lblSelectorProgressDetail.Text = "0 / 0 items scored";
        lblSelectorProgressDetail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        pnlSelectorProgress.Controls.Add(lblSelectorProgressDetail);
        pnlSelectorProgress.Controls.Add(lblSelectorProgress);
    }

    private void BuildSummary()
    {
        summaryLayout.ColumnCount = 3;
        summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
        summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
        summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
        summaryLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        summaryLayout.RowCount = 1;
        summaryLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

        BuildInspectionSummaryPanel();
        BuildAppraisalSummaryPanel();
        BuildVehicleSummaryPanel();

        summaryLayout.Controls.Add(pnlInspectionSummary, 0, 0);
        summaryLayout.Controls.Add(pnlAppraisalSummary, 1, 0);
        summaryLayout.Controls.Add(pnlVehicleSummary, 2, 0);
    }

    private void BuildInspectionSummaryPanel()
    {
        pnlInspectionSummary.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlInspectionSummary.Margin = new System.Windows.Forms.Padding(0, 0, 8, 8);
        pnlInspectionSummary.Padding = new System.Windows.Forms.Padding(14);
        pnlInspectionSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblInspectionSummaryTitle.AutoSize = true;
        lblInspectionSummaryTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        lblInspectionSummaryTitle.Location = new System.Drawing.Point(14, 10);
        lblInspectionSummaryTitle.Text = "Inspection Information";

        var layout = CreateTwoColumnDetailsLayout(7);
        layout.Location = new System.Drawing.Point(14, 38);
        layout.Size = new System.Drawing.Size(405, 224);

        AddDetailRow(layout, 0, "Inspection ID", cmbInspectionId);
        AddDetailRow(layout, 1, "Status", cmbStatus);
        AddDetailRow(layout, 2, "Inspection Type", cmbInspectionType);
        AddDetailRow(layout, 3, "Template", cmbTemplate);
        AddDetailRow(layout, 4, "Location", txtLocation);
        AddDetailRow(layout, 5, "Inspected By", cmbInspectedBy);
        AddDetailRow(layout, 6, "Inspected On", dtpInspectedOn);

        pnlInspectionSummary.Controls.Add(lblInspectionSummaryTitle);
        pnlInspectionSummary.Controls.Add(layout);
    }

    private void BuildAppraisalSummaryPanel()
    {
        pnlAppraisalSummary.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlAppraisalSummary.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
        pnlAppraisalSummary.Padding = new System.Windows.Forms.Padding(14);
        pnlAppraisalSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblAppraisalTitle.AutoSize = true;
        lblAppraisalTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        lblAppraisalTitle.Location = new System.Drawing.Point(14, 10);
        lblAppraisalTitle.Text = "Appraisal Information";

        var layout = CreateTwoColumnDetailsLayout(6);
        layout.Location = new System.Drawing.Point(14, 38);
        layout.Size = new System.Drawing.Size(350, 192);

        AddReadOnlyDetailRow(layout, 0, "Appraisal ID", lblAppraisalIdValue);
        AddReadOnlyDetailRow(layout, 1, "Number", lblAppraisalNumberValue);
        AddReadOnlyDetailRow(layout, 2, "Status", lblAppraisalStatusValue);
        AddReadOnlyDetailRow(layout, 3, "Type", lblAppraisalTypeValue);
        AddReadOnlyDetailRow(layout, 4, "Effective Date", lblEffectiveDateValue);
        AddReadOnlyDetailRow(layout, 5, "Client", lblClientValue);

        pnlAppraisalSummary.Controls.Add(lblAppraisalTitle);
        pnlAppraisalSummary.Controls.Add(layout);
    }

    private void BuildVehicleSummaryPanel()
    {
        pnlVehicleSummary.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlVehicleSummary.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
        pnlVehicleSummary.Padding = new System.Windows.Forms.Padding(14);
        pnlVehicleSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblVehicleTitle.AutoSize = true;
        lblVehicleTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        lblVehicleTitle.Location = new System.Drawing.Point(14, 10);
        lblVehicleTitle.Text = "Vehicle Information";

        var layout = CreateTwoColumnDetailsLayout(6);
        layout.Location = new System.Drawing.Point(14, 38);
        layout.Size = new System.Drawing.Size(310, 192);

        AddReadOnlyDetailRow(layout, 0, "VIN", lblVinValue);
        AddReadOnlyDetailRow(layout, 1, "Year / Make / Model", lblYearMakeModelValue);
        AddReadOnlyDetailRow(layout, 2, "Body Style", lblBodyStyleValue);
        AddReadOnlyDetailRow(layout, 3, "Drivetrain", lblDrivetrainValue);
        AddReadOnlyDetailRow(layout, 4, "Engine / Trans.", lblEngineTransmissionValue);
        AddReadOnlyDetailRow(layout, 5, "Mileage", lblMileageValue);

        pictureVehicle.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Right;
        pictureVehicle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        pictureVehicle.Location = new System.Drawing.Point(335, 38);
        pictureVehicle.Size = new System.Drawing.Size(205, 105);
        pictureVehicle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

        lblVehicleImagePlaceholder.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Right;
        lblVehicleImagePlaceholder.AutoSize = false;
        lblVehicleImagePlaceholder.Location = pictureVehicle.Location;
        lblVehicleImagePlaceholder.Size = pictureVehicle.Size;
        lblVehicleImagePlaceholder.Text = "Vehicle image";
        lblVehicleImagePlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        btnViewVehicle.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Right;
        btnViewVehicle.Location = new System.Drawing.Point(335, 148);
        btnViewVehicle.Size = new System.Drawing.Size(205, 32);
        btnViewVehicle.Text = "View Vehicle";

        pnlVehicleSummary.Controls.Add(lblVehicleTitle);
        pnlVehicleSummary.Controls.Add(layout);
        pnlVehicleSummary.Controls.Add(pictureVehicle);
        pnlVehicleSummary.Controls.Add(lblVehicleImagePlaceholder);
        pnlVehicleSummary.Controls.Add(btnViewVehicle);
    }

    private void BuildTabs()
    {
        tabWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
        tabWorkspace.Margin = new System.Windows.Forms.Padding(0);
        tabWorkspace.Controls.Add(tabOverview);
        tabWorkspace.Controls.Add(tabScores);
        tabWorkspace.Controls.Add(tabSections);
        tabWorkspace.Controls.Add(tabRedFlags);
        tabWorkspace.Controls.Add(tabMedia);
        tabWorkspace.Controls.Add(tabNotes);
        tabWorkspace.Controls.Add(tabHistory);

        tabOverview.Text = "Overview";
        tabScores.Text = "12-System Scores";
        tabSections.Text = "Sections & Items";
        tabRedFlags.Text = "Red Flags";
        tabMedia.Text = "Media";
        tabNotes.Text = "Notes";
        tabHistory.Text = "History";

        AddPlaceholder(tabScores, "The detailed 12-System Scores page will load here.");
        AddPlaceholder(tabSections, "The Sections & Items page will load here.");
        AddPlaceholder(tabRedFlags, "The Red Flags page will load here.");
        AddPlaceholder(tabMedia, "The Media page will load here.");
        AddPlaceholder(tabNotes, "The Notes page will load here.");
        AddPlaceholder(tabHistory, "The History page will load here.");

        BuildOverviewTab();
    }

    private void BuildOverviewTab()
    {
        tabOverview.Padding = new System.Windows.Forms.Padding(6);
        overviewLayout.ColumnCount = 3;
        overviewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F));
        overviewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
        overviewLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
        overviewLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        overviewLayout.RowCount = 2;
        overviewLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66F));
        overviewLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));

        BuildScoresPanel();
        BuildSectionsPanel();
        BuildRedFlagsPanel();
        BuildMediaPanel();
        BuildRightRail();

        overviewLayout.Controls.Add(pnlScores, 0, 0);
        overviewLayout.Controls.Add(pnlSections, 1, 0);
        overviewLayout.Controls.Add(pnlRedFlags, 0, 1);
        overviewLayout.Controls.Add(pnlMedia, 1, 1);
        overviewLayout.Controls.Add(rightRailLayout, 2, 0);
        overviewLayout.SetRowSpan(rightRailLayout, 2);
        tabOverview.Controls.Add(overviewLayout);
    }

    private void BuildScoresPanel()
    {
        pnlScores.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlScores.Margin = new System.Windows.Forms.Padding(0, 0, 8, 8);
        pnlScores.Padding = new System.Windows.Forms.Padding(12);
        pnlScores.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblScoresTitle.AutoSize = true;
        lblScoresTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblScoresTitle.Location = new System.Drawing.Point(12, 8);
        lblScoresTitle.Text = "12-System Condition Scores";

        gridScores.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left |
            System.Windows.Forms.AnchorStyles.Right;
        gridScores.Location = new System.Drawing.Point(12, 34);
        gridScores.Size = new System.Drawing.Size(590, 286);

        lblConditionIndexCaption.Anchor =
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left;
        lblConditionIndexCaption.AutoSize = true;
        lblConditionIndexCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        lblConditionIndexCaption.Location = new System.Drawing.Point(18, 330);
        lblConditionIndexCaption.Text = "Condition Index";

        lblConditionIndexValue.Anchor =
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left;
        lblConditionIndexValue.AutoSize = true;
        lblConditionIndexValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
        lblConditionIndexValue.Location = new System.Drawing.Point(118, 324);
        lblConditionIndexValue.Text = "0.00 / 100";

        lblWeightedScoreValue.Anchor =
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left;
        lblWeightedScoreValue.AutoSize = true;
        lblWeightedScoreValue.Location = new System.Drawing.Point(18, 357);
        lblWeightedScoreValue.Text = "Weighted score: 0.00 / 5.00";

        btnRecalculate.Anchor =
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Right;
        btnRecalculate.Location = new System.Drawing.Point(490, 330);
        btnRecalculate.Size = new System.Drawing.Size(112, 34);
        btnRecalculate.Text = "Recalculate";

        pnlScores.Controls.Add(lblScoresTitle);
        pnlScores.Controls.Add(gridScores);
        pnlScores.Controls.Add(lblConditionIndexCaption);
        pnlScores.Controls.Add(lblConditionIndexValue);
        pnlScores.Controls.Add(lblWeightedScoreValue);
        pnlScores.Controls.Add(btnRecalculate);
    }

    private void BuildSectionsPanel()
    {
        pnlSections.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlSections.Margin = new System.Windows.Forms.Padding(0, 0, 8, 8);
        pnlSections.Padding = new System.Windows.Forms.Padding(12);
        pnlSections.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblSectionsTitle.AutoSize = true;
        lblSectionsTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblSectionsTitle.Location = new System.Drawing.Point(12, 8);
        lblSectionsTitle.Text = "Section Summary (from Items)";

        gridSections.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left |
            System.Windows.Forms.AnchorStyles.Right;
        gridSections.Location = new System.Drawing.Point(12, 34);
        gridSections.Size = new System.Drawing.Size(440, 330);

        pnlSections.Controls.Add(lblSectionsTitle);
        pnlSections.Controls.Add(gridSections);
    }

    private void BuildRedFlagsPanel()
    {
        pnlRedFlags.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlRedFlags.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
        pnlRedFlags.Padding = new System.Windows.Forms.Padding(12);
        pnlRedFlags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblRedFlagsTitle.AutoSize = true;
        lblRedFlagsTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblRedFlagsTitle.Location = new System.Drawing.Point(12, 8);
        lblRedFlagsTitle.Text = "Recent Red Flags";

        gridRedFlags.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left |
            System.Windows.Forms.AnchorStyles.Right;
        gridRedFlags.Location = new System.Drawing.Point(12, 34);
        gridRedFlags.Size = new System.Drawing.Size(590, 152);

        pnlRedFlags.Controls.Add(lblRedFlagsTitle);
        pnlRedFlags.Controls.Add(gridRedFlags);
    }

    private void BuildMediaPanel()
    {
        pnlMedia.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlMedia.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
        pnlMedia.Padding = new System.Windows.Forms.Padding(12);
        pnlMedia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblMediaTitle.AutoSize = true;
        lblMediaTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblMediaTitle.Location = new System.Drawing.Point(12, 8);
        lblMediaTitle.Text = "Media Summary";

        mediaFlow.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left |
            System.Windows.Forms.AnchorStyles.Right;
        mediaFlow.Location = new System.Drawing.Point(12, 38);
        mediaFlow.Size = new System.Drawing.Size(440, 145);
        mediaFlow.WrapContents = true;

        AddMediaCard(mediaFlow, "Photos", lblPhotoCount);
        AddMediaCard(mediaFlow, "Videos", lblVideoCount);
        AddMediaCard(mediaFlow, "Documents", lblDocumentCount);
        AddMediaCard(mediaFlow, "Other Files", lblOtherFileCount);

        btnViewAllMedia.Size = new System.Drawing.Size(120, 42);
        btnViewAllMedia.Text = "View All Media";
        mediaFlow.Controls.Add(btnViewAllMedia);

        pnlMedia.Controls.Add(lblMediaTitle);
        pnlMedia.Controls.Add(mediaFlow);
    }

    private void BuildRightRail()
    {
        rightRailLayout.ColumnCount = 1;
        rightRailLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        rightRailLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        rightRailLayout.Margin = new System.Windows.Forms.Padding(0);
        rightRailLayout.RowCount = 3;
        rightRailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36F));
        rightRailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
        rightRailLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));

        BuildHighlightsPanel();
        BuildQuickNotesPanel();
        BuildChecklistPanel();

        rightRailLayout.Controls.Add(pnlHighlights, 0, 0);
        rightRailLayout.Controls.Add(pnlQuickNotes, 0, 1);
        rightRailLayout.Controls.Add(pnlChecklist, 0, 2);
    }

    private void BuildHighlightsPanel()
    {
        pnlHighlights.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlHighlights.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
        pnlHighlights.Padding = new System.Windows.Forms.Padding(12);
        pnlHighlights.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblHighlightsTitle.AutoSize = true;
        lblHighlightsTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblHighlightsTitle.Location = new System.Drawing.Point(12, 8);
        lblHighlightsTitle.Text = "Inspection Highlights";

        lblConditionIndexHighlight.AutoSize = true;
        lblConditionIndexHighlight.Location = new System.Drawing.Point(16, 44);
        lblConditionIndexHighlight.Text = "Condition Index";

        lblTotalRedFlagsCaption.AutoSize = true;
        lblTotalRedFlagsCaption.Location = new System.Drawing.Point(16, 72);
        lblTotalRedFlagsCaption.Text = "Total Red Flags";
        lblTotalRedFlagsValue.AutoSize = true;
        lblTotalRedFlagsValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblTotalRedFlagsValue.Location = new System.Drawing.Point(180, 68);
        lblTotalRedFlagsValue.Text = "0";

        lblItemsScoredCaption.AutoSize = true;
        lblItemsScoredCaption.Location = new System.Drawing.Point(16, 102);
        lblItemsScoredCaption.Text = "Items Scored";
        lblItemsScoredValue.AutoSize = true;
        lblItemsScoredValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        lblItemsScoredValue.Location = new System.Drawing.Point(160, 98);
        lblItemsScoredValue.Text = "0 / 0";

        lblProgressCaption.AutoSize = true;
        lblProgressCaption.Location = new System.Drawing.Point(16, 130);
        lblProgressCaption.Text = "Inspection Progress";
        lblProgressValue.AutoSize = true;
        lblProgressValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        lblProgressValue.Location = new System.Drawing.Point(175, 126);
        lblProgressValue.Text = "0%";

        progressInspection.Location = new System.Drawing.Point(16, 158);
        progressInspection.Size = new System.Drawing.Size(225, 14);

        lblProgressDetail.AutoSize = true;
        lblProgressDetail.Location = new System.Drawing.Point(50, 180);
        lblProgressDetail.Text = "0 of 0 items scored";

        pnlHighlights.Controls.Add(lblHighlightsTitle);
        pnlHighlights.Controls.Add(lblConditionIndexHighlight);
        pnlHighlights.Controls.Add(lblTotalRedFlagsCaption);
        pnlHighlights.Controls.Add(lblTotalRedFlagsValue);
        pnlHighlights.Controls.Add(lblItemsScoredCaption);
        pnlHighlights.Controls.Add(lblItemsScoredValue);
        pnlHighlights.Controls.Add(lblProgressCaption);
        pnlHighlights.Controls.Add(lblProgressValue);
        pnlHighlights.Controls.Add(progressInspection);
        pnlHighlights.Controls.Add(lblProgressDetail);
    }

    private void BuildQuickNotesPanel()
    {
        pnlQuickNotes.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlQuickNotes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
        pnlQuickNotes.Padding = new System.Windows.Forms.Padding(12);
        pnlQuickNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblQuickNotesTitle.AutoSize = true;
        lblQuickNotesTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblQuickNotesTitle.Location = new System.Drawing.Point(12, 8);
        lblQuickNotesTitle.Text = "Quick Notes";

        txtQuickNotes.AcceptsReturn = true;
        txtQuickNotes.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left |
            System.Windows.Forms.AnchorStyles.Right;
        txtQuickNotes.Location = new System.Drawing.Point(12, 36);
        txtQuickNotes.Multiline = true;
        txtQuickNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        txtQuickNotes.Size = new System.Drawing.Size(235, 150);

        pnlQuickNotes.Controls.Add(lblQuickNotesTitle);
        pnlQuickNotes.Controls.Add(txtQuickNotes);
    }

    private void BuildChecklistPanel()
    {
        pnlChecklist.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlChecklist.Padding = new System.Windows.Forms.Padding(12);
        pnlChecklist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblChecklistTitle.AutoSize = true;
        lblChecklistTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        lblChecklistTitle.Location = new System.Drawing.Point(12, 8);
        lblChecklistTitle.Text = "Next Steps / Checklist";

        checklist.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Bottom |
            System.Windows.Forms.AnchorStyles.Left |
            System.Windows.Forms.AnchorStyles.Right;
        checklist.BorderStyle = System.Windows.Forms.BorderStyle.None;
        checklist.CheckOnClick = true;
        checklist.Location = new System.Drawing.Point(12, 36);
        checklist.Size = new System.Drawing.Size(235, 128);
        checklist.Items.AddRange(new object[]
        {
            "Complete all section items",
            "Review red flags",
            "Add media evidence",
            "Finalize notes",
            "Complete inspection"
        });

        pnlChecklist.Controls.Add(lblChecklistTitle);
        pnlChecklist.Controls.Add(checklist);
    }

    private void BuildFooter()
    {
        pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlFooter.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0); //

        lblFooterInspectionId.AutoSize = true;
        lblFooterInspectionId.Location = new System.Drawing.Point(14, 12);
        lblFooterInspectionId.Text = "Inspection ID: —";

        lblFooterCreated.AutoSize = true;
        lblFooterCreated.Location = new System.Drawing.Point(370, 12);
        lblFooterCreated.Text = "Created: —";

        lblFooterUpdated.AutoSize = true;
        lblFooterUpdated.Location = new System.Drawing.Point(650, 12);
        lblFooterUpdated.Text = "Updated: —";

        lblStatus.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Right;
        lblStatus.AutoSize = true;
        lblStatus.Location = new System.Drawing.Point(970, 12);
        lblStatus.Text = "Ready";

        lblSaveState.Anchor =
            System.Windows.Forms.AnchorStyles.Top |
            System.Windows.Forms.AnchorStyles.Right;
        lblSaveState.AutoSize = true;
        lblSaveState.Location = new System.Drawing.Point(1230, 12);
        lblSaveState.Text = "All changes saved";

        pnlFooter.Controls.Add(lblFooterInspectionId);
        pnlFooter.Controls.Add(lblFooterCreated);
        pnlFooter.Controls.Add(lblFooterUpdated);
        pnlFooter.Controls.Add(lblStatus);
        pnlFooter.Controls.Add(lblSaveState);
    }

    private static void ConfigureHeaderButton(
        System.Windows.Forms.Button button,
        string text,
        int width)
    {
        button.Size = new System.Drawing.Size(width, 38);
        button.Text = text;
        button.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
    }

    private static void AddSelectorPair(
        System.Windows.Forms.FlowLayoutPanel parent,
        string labelText,
        System.Windows.Forms.Control control,
        int width)
    {
        var panel = new System.Windows.Forms.Panel
        {
            Size = new System.Drawing.Size(width + 92, 42),
            Margin = new System.Windows.Forms.Padding(0, 0, 12, 0)
        };

        var label = new System.Windows.Forms.Label
        {
            AutoSize = true,
            Location = new System.Drawing.Point(0, 13),
            Text = labelText
        };

        control.Location = new System.Drawing.Point(88, 5);
        control.Size = new System.Drawing.Size(width, 30);

        panel.Controls.Add(label);
        panel.Controls.Add(control);
        parent.Controls.Add(panel);
    }

    private static System.Windows.Forms.TableLayoutPanel CreateTwoColumnDetailsLayout(int rowCount)
    {
        var layout = new System.Windows.Forms.TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = rowCount
        };
        layout.ColumnStyles.Add(
            new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Absolute,
                125F));
        layout.ColumnStyles.Add(
            new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent,
                100F));

        for (var row = 0; row < rowCount; row++)
        {
            layout.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Absolute,
                    32F));
        }

        return layout;
    }

    private static void AddDetailRow(
        System.Windows.Forms.TableLayoutPanel layout,
        int row,
        string labelText,
        System.Windows.Forms.Control valueControl)
    {
        var label = new System.Windows.Forms.Label
        {
            AutoSize = true,
            Anchor = System.Windows.Forms.AnchorStyles.Left,
            Text = labelText
        };

        valueControl.Dock = System.Windows.Forms.DockStyle.Fill;
        valueControl.Margin = new System.Windows.Forms.Padding(0, 3, 0, 5);

        layout.Controls.Add(label, 0, row);
        layout.Controls.Add(valueControl, 1, row);
    }

    private static void AddReadOnlyDetailRow(
        System.Windows.Forms.TableLayoutPanel layout,
        int row,
        string labelText,
        System.Windows.Forms.Label valueLabel)
    {
        var label = new System.Windows.Forms.Label
        {
            AutoSize = true,
            Anchor = System.Windows.Forms.AnchorStyles.Left,
            Text = labelText
        };

        valueLabel.AutoEllipsis = true;
        valueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        valueLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 5);
        valueLabel.Text = "—";
        valueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        layout.Controls.Add(label, 0, row);
        layout.Controls.Add(valueLabel, 1, row);
    }

    private static void AddPlaceholder(
        System.Windows.Forms.TabPage page,
        string message)
    {
        var label = new System.Windows.Forms.Label
        {
            AutoSize = true,
            Font = new System.Drawing.Font(
                "Segoe UI",
                14F,
                System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(36, 36),
            Text = message
        };
        page.Controls.Add(label);
    }

    private static void AddMediaCard(
        System.Windows.Forms.FlowLayoutPanel parent,
        string caption,
        System.Windows.Forms.Label countLabel)
    {
        var panel = new System.Windows.Forms.Panel
        {
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
            Size = new System.Drawing.Size(78, 72),
            Margin = new System.Windows.Forms.Padding(0, 0, 8, 8)
        };

        countLabel.AutoSize = false;
        countLabel.Dock = System.Windows.Forms.DockStyle.Top;
        countLabel.Font = new System.Drawing.Font(
            "Segoe UI",
            16F,
            System.Drawing.FontStyle.Bold);
        countLabel.Height = 40;
        countLabel.Text = "0";
        countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        var captionLabel = new System.Windows.Forms.Label
        {
            AutoSize = false,
            Dock = System.Windows.Forms.DockStyle.Fill,
            Text = caption,
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };

        panel.Controls.Add(captionLabel);
        panel.Controls.Add(countLabel);
        parent.Controls.Add(panel);
    }
}
