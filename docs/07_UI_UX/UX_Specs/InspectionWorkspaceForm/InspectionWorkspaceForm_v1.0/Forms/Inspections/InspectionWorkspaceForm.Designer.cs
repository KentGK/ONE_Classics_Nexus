namespace ONE_Classics_Nexus.WinForms.Forms.Inspections;

partial class InspectionWorkspaceForm
{
    private System.ComponentModel.IContainer? components = null;

    private System.Windows.Forms.TableLayoutPanel rootLayout = null!;
    private System.Windows.Forms.Panel pnlHeader = null!;
    private System.Windows.Forms.Label lblTitle = null!;
    private System.Windows.Forms.FlowLayoutPanel headerActions = null!;
    private System.Windows.Forms.Button btnNewInspection = null!;
    private System.Windows.Forms.Button btnSaveDraft = null!;
    private System.Windows.Forms.Button btnCompleteInspection = null!;
    private System.Windows.Forms.Button btnDeleteInspection = null!;
    private System.Windows.Forms.Button btnRefresh = null!;
    private System.Windows.Forms.ProgressBar progressBusy = null!;

    private System.Windows.Forms.Panel pnlSelector = null!;
    private System.Windows.Forms.FlowLayoutPanel selectorFlow = null!;
    private System.Windows.Forms.ComboBox cmbInspectionId = null!;
    private System.Windows.Forms.ComboBox cmbStatus = null!;
    private System.Windows.Forms.DateTimePicker dtpInspectedOn = null!;
    private System.Windows.Forms.ComboBox cmbInspectedBy = null!;
    private System.Windows.Forms.Panel pnlSelectorProgress = null!;
    private System.Windows.Forms.Label lblSelectorProgress = null!;
    private System.Windows.Forms.Label lblSelectorProgressDetail = null!;

    private System.Windows.Forms.TableLayoutPanel summaryLayout = null!;
    private System.Windows.Forms.Panel pnlInspectionSummary = null!;
    private System.Windows.Forms.Panel pnlAppraisalSummary = null!;
    private System.Windows.Forms.Panel pnlVehicleSummary = null!;
    private System.Windows.Forms.Label lblInspectionSummaryTitle = null!;
    private System.Windows.Forms.ComboBox cmbInspectionType = null!;
    private System.Windows.Forms.ComboBox cmbTemplate = null!;
    private System.Windows.Forms.TextBox txtLocation = null!;
    private System.Windows.Forms.Label lblAppraisalTitle = null!;
    private System.Windows.Forms.Label lblAppraisalIdValue = null!;
    private System.Windows.Forms.Label lblAppraisalNumberValue = null!;
    private System.Windows.Forms.Label lblAppraisalStatusValue = null!;
    private System.Windows.Forms.Label lblAppraisalTypeValue = null!;
    private System.Windows.Forms.Label lblEffectiveDateValue = null!;
    private System.Windows.Forms.Label lblClientValue = null!;
    private System.Windows.Forms.Label lblVehicleTitle = null!;
    private System.Windows.Forms.Label lblVinValue = null!;
    private System.Windows.Forms.Label lblYearMakeModelValue = null!;
    private System.Windows.Forms.Label lblBodyStyleValue = null!;
    private System.Windows.Forms.Label lblDrivetrainValue = null!;
    private System.Windows.Forms.Label lblEngineTransmissionValue = null!;
    private System.Windows.Forms.Label lblMileageValue = null!;
    private System.Windows.Forms.PictureBox pictureVehicle = null!;
    private System.Windows.Forms.Label lblVehicleImagePlaceholder = null!;
    private System.Windows.Forms.Button btnViewVehicle = null!;

    private System.Windows.Forms.TabControl tabWorkspace = null!;
    private System.Windows.Forms.TabPage tabOverview = null!;
    private System.Windows.Forms.TabPage tabScores = null!;
    private System.Windows.Forms.TabPage tabSections = null!;
    private System.Windows.Forms.TabPage tabRedFlags = null!;
    private System.Windows.Forms.TabPage tabMedia = null!;
    private System.Windows.Forms.TabPage tabNotes = null!;
    private System.Windows.Forms.TabPage tabHistory = null!;

    private System.Windows.Forms.TableLayoutPanel overviewLayout = null!;
    private System.Windows.Forms.Panel pnlScores = null!;
    private System.Windows.Forms.Label lblScoresTitle = null!;
    private System.Windows.Forms.DataGridView gridScores = null!;
    private System.Windows.Forms.Label lblConditionIndexCaption = null!;
    private System.Windows.Forms.Label lblConditionIndexValue = null!;
    private System.Windows.Forms.Label lblWeightedScoreValue = null!;
    private System.Windows.Forms.Button btnRecalculate = null!;

    private System.Windows.Forms.Panel pnlSections = null!;
    private System.Windows.Forms.Label lblSectionsTitle = null!;
    private System.Windows.Forms.DataGridView gridSections = null!;

    private System.Windows.Forms.Panel pnlRedFlags = null!;
    private System.Windows.Forms.Label lblRedFlagsTitle = null!;
    private System.Windows.Forms.DataGridView gridRedFlags = null!;

    private System.Windows.Forms.Panel pnlMedia = null!;
    private System.Windows.Forms.Label lblMediaTitle = null!;
    private System.Windows.Forms.FlowLayoutPanel mediaFlow = null!;
    private System.Windows.Forms.Label lblPhotoCount = null!;
    private System.Windows.Forms.Label lblVideoCount = null!;
    private System.Windows.Forms.Label lblDocumentCount = null!;
    private System.Windows.Forms.Label lblOtherFileCount = null!;
    private System.Windows.Forms.Button btnViewAllMedia = null!;

    private System.Windows.Forms.TableLayoutPanel rightRailLayout = null!;
    private System.Windows.Forms.Panel pnlHighlights = null!;
    private System.Windows.Forms.Label lblHighlightsTitle = null!;
    private System.Windows.Forms.Label lblConditionIndexHighlight = null!;
    private System.Windows.Forms.Label lblTotalRedFlagsCaption = null!;
    private System.Windows.Forms.Label lblTotalRedFlagsValue = null!;
    private System.Windows.Forms.Label lblItemsScoredCaption = null!;
    private System.Windows.Forms.Label lblItemsScoredValue = null!;
    private System.Windows.Forms.Label lblProgressCaption = null!;
    private System.Windows.Forms.Label lblProgressValue = null!;
    private System.Windows.Forms.ProgressBar progressInspection = null!;
    private System.Windows.Forms.Label lblProgressDetail = null!;

    private System.Windows.Forms.Panel pnlQuickNotes = null!;
    private System.Windows.Forms.Label lblQuickNotesTitle = null!;
    private System.Windows.Forms.TextBox txtQuickNotes = null!;

    private System.Windows.Forms.Panel pnlChecklist = null!;
    private System.Windows.Forms.Label lblChecklistTitle = null!;
    private System.Windows.Forms.CheckedListBox checklist = null!;

    private System.Windows.Forms.Panel pnlFooter = null!;
    private System.Windows.Forms.Label lblFooterInspectionId = null!;
    private System.Windows.Forms.Label lblFooterCreated = null!;
    private System.Windows.Forms.Label lblFooterUpdated = null!;
    private System.Windows.Forms.Label lblSaveState = null!;
    private System.Windows.Forms.Label lblStatus = null!;
    private System.Windows.Forms.ErrorProvider errorProvider = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _operationCts?.Dispose();

            if (_disposeApiClient)
                _apiClient.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        rootLayout = new System.Windows.Forms.TableLayoutPanel();
        pnlHeader = new System.Windows.Forms.Panel();
        lblTitle = new System.Windows.Forms.Label();
        headerActions = new System.Windows.Forms.FlowLayoutPanel();
        btnNewInspection = new System.Windows.Forms.Button();
        btnSaveDraft = new System.Windows.Forms.Button();
        btnCompleteInspection = new System.Windows.Forms.Button();
        btnDeleteInspection = new System.Windows.Forms.Button();
        btnRefresh = new System.Windows.Forms.Button();
        progressBusy = new System.Windows.Forms.ProgressBar();

        pnlSelector = new System.Windows.Forms.Panel();
        selectorFlow = new System.Windows.Forms.FlowLayoutPanel();
        cmbInspectionId = new System.Windows.Forms.ComboBox();
        cmbStatus = new System.Windows.Forms.ComboBox();
        dtpInspectedOn = new System.Windows.Forms.DateTimePicker();
        cmbInspectedBy = new System.Windows.Forms.ComboBox();
        pnlSelectorProgress = new System.Windows.Forms.Panel();
        lblSelectorProgress = new System.Windows.Forms.Label();
        lblSelectorProgressDetail = new System.Windows.Forms.Label();

        summaryLayout = new System.Windows.Forms.TableLayoutPanel();
        pnlInspectionSummary = new System.Windows.Forms.Panel();
        pnlAppraisalSummary = new System.Windows.Forms.Panel();
        pnlVehicleSummary = new System.Windows.Forms.Panel();
        lblInspectionSummaryTitle = new System.Windows.Forms.Label();
        cmbInspectionType = new System.Windows.Forms.ComboBox();
        cmbTemplate = new System.Windows.Forms.ComboBox();
        txtLocation = new System.Windows.Forms.TextBox();

        lblAppraisalTitle = new System.Windows.Forms.Label();
        lblAppraisalIdValue = new System.Windows.Forms.Label();
        lblAppraisalNumberValue = new System.Windows.Forms.Label();
        lblAppraisalStatusValue = new System.Windows.Forms.Label();
        lblAppraisalTypeValue = new System.Windows.Forms.Label();
        lblEffectiveDateValue = new System.Windows.Forms.Label();
        lblClientValue = new System.Windows.Forms.Label();

        lblVehicleTitle = new System.Windows.Forms.Label();
        lblVinValue = new System.Windows.Forms.Label();
        lblYearMakeModelValue = new System.Windows.Forms.Label();
        lblBodyStyleValue = new System.Windows.Forms.Label();
        lblDrivetrainValue = new System.Windows.Forms.Label();
        lblEngineTransmissionValue = new System.Windows.Forms.Label();
        lblMileageValue = new System.Windows.Forms.Label();
        pictureVehicle = new System.Windows.Forms.PictureBox();
        lblVehicleImagePlaceholder = new System.Windows.Forms.Label();
        btnViewVehicle = new System.Windows.Forms.Button();

        tabWorkspace = new System.Windows.Forms.TabControl();
        tabOverview = new System.Windows.Forms.TabPage();
        tabScores = new System.Windows.Forms.TabPage();
        tabSections = new System.Windows.Forms.TabPage();
        tabRedFlags = new System.Windows.Forms.TabPage();
        tabMedia = new System.Windows.Forms.TabPage();
        tabNotes = new System.Windows.Forms.TabPage();
        tabHistory = new System.Windows.Forms.TabPage();

        overviewLayout = new System.Windows.Forms.TableLayoutPanel();
        pnlScores = new System.Windows.Forms.Panel();
        lblScoresTitle = new System.Windows.Forms.Label();
        gridScores = new System.Windows.Forms.DataGridView();
        lblConditionIndexCaption = new System.Windows.Forms.Label();
        lblConditionIndexValue = new System.Windows.Forms.Label();
        lblWeightedScoreValue = new System.Windows.Forms.Label();
        btnRecalculate = new System.Windows.Forms.Button();

        pnlSections = new System.Windows.Forms.Panel();
        lblSectionsTitle = new System.Windows.Forms.Label();
        gridSections = new System.Windows.Forms.DataGridView();

        pnlRedFlags = new System.Windows.Forms.Panel();
        lblRedFlagsTitle = new System.Windows.Forms.Label();
        gridRedFlags = new System.Windows.Forms.DataGridView();

        pnlMedia = new System.Windows.Forms.Panel();
        lblMediaTitle = new System.Windows.Forms.Label();
        mediaFlow = new System.Windows.Forms.FlowLayoutPanel();
        lblPhotoCount = new System.Windows.Forms.Label();
        lblVideoCount = new System.Windows.Forms.Label();
        lblDocumentCount = new System.Windows.Forms.Label();
        lblOtherFileCount = new System.Windows.Forms.Label();
        btnViewAllMedia = new System.Windows.Forms.Button();

        rightRailLayout = new System.Windows.Forms.TableLayoutPanel();
        pnlHighlights = new System.Windows.Forms.Panel();
        lblHighlightsTitle = new System.Windows.Forms.Label();
        lblConditionIndexHighlight = new System.Windows.Forms.Label();
        lblTotalRedFlagsCaption = new System.Windows.Forms.Label();
        lblTotalRedFlagsValue = new System.Windows.Forms.Label();
        lblItemsScoredCaption = new System.Windows.Forms.Label();
        lblItemsScoredValue = new System.Windows.Forms.Label();
        lblProgressCaption = new System.Windows.Forms.Label();
        lblProgressValue = new System.Windows.Forms.Label();
        progressInspection = new System.Windows.Forms.ProgressBar();
        lblProgressDetail = new System.Windows.Forms.Label();

        pnlQuickNotes = new System.Windows.Forms.Panel();
        lblQuickNotesTitle = new System.Windows.Forms.Label();
        txtQuickNotes = new System.Windows.Forms.TextBox();

        pnlChecklist = new System.Windows.Forms.Panel();
        lblChecklistTitle = new System.Windows.Forms.Label();
        checklist = new System.Windows.Forms.CheckedListBox();

        pnlFooter = new System.Windows.Forms.Panel();
        lblFooterInspectionId = new System.Windows.Forms.Label();
        lblFooterCreated = new System.Windows.Forms.Label();
        lblFooterUpdated = new System.Windows.Forms.Label();
        lblSaveState = new System.Windows.Forms.Label();
        lblStatus = new System.Windows.Forms.Label();
        errorProvider = new System.Windows.Forms.ErrorProvider(components);

        ((System.ComponentModel.ISupportInitialize)pictureVehicle).BeginInit();
        ((System.ComponentModel.ISupportInitialize)gridScores).BeginInit();
        ((System.ComponentModel.ISupportInitialize)gridSections).BeginInit();
        ((System.ComponentModel.ISupportInitialize)gridRedFlags).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();

        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        rootLayout.Padding = new System.Windows.Forms.Padding(12);
        rootLayout.RowCount = 5;
        rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
        rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
        rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 196F));
        rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));

        BuildHeader();
        BuildSelector();
        BuildSummary();
        BuildTabs();
        BuildFooter();

        rootLayout.Controls.Add(pnlHeader, 0, 0);
        rootLayout.Controls.Add(pnlSelector, 0, 1);
        rootLayout.Controls.Add(summaryLayout, 0, 2);
        rootLayout.Controls.Add(tabWorkspace, 0, 3);
        rootLayout.Controls.Add(pnlFooter, 0, 4);

        errorProvider.ContainerControl = this;
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1420, 900);
        Controls.Add(rootLayout);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        MinimumSize = new System.Drawing.Size(1180, 720);
        Name = "InspectionWorkspaceForm";
        Text = "Inspection Workspace";
        ((System.ComponentModel.ISupportInitialize)pictureVehicle).EndInit();
        ((System.ComponentModel.ISupportInitialize)gridScores).EndInit();
        ((System.ComponentModel.ISupportInitialize)gridSections).EndInit();
        ((System.ComponentModel.ISupportInitialize)gridRedFlags).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
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
        pnlSelector.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlSelector.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
        pnlSelector.Padding = new System.Windows.Forms.Padding(12);

        selectorFlow.Dock = System.Windows.Forms.DockStyle.Fill;
        selectorFlow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
        selectorFlow.WrapContents = false;

        AddSelectorPair(selectorFlow, "Inspection ID", cmbInspectionId, 305);
        AddSelectorPair(selectorFlow, "Status", cmbStatus, 130);

        dtpInspectedOn.ShowCheckBox = true;
        dtpInspectedOn.Format = System.Windows.Forms.DateTimePickerFormat.Short;
        AddSelectorPair(selectorFlow, "Inspected On", dtpInspectedOn, 132);
        AddSelectorPair(selectorFlow, "Inspected By", cmbInspectedBy, 180);

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
        selectorFlow.Controls.Add(pnlSelectorProgress);
        pnlSelector.Controls.Add(selectorFlow);
    }

    private void BuildSummary()
    {
        summaryLayout.ColumnCount = 3;
        summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
        summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
        summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
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

        var layout = CreateTwoColumnDetailsLayout();
        layout.Location = new System.Drawing.Point(14, 38);
        layout.Size = new System.Drawing.Size(405, 138);

        AddDetailRow(layout, 0, "Inspection Type", cmbInspectionType);
        AddDetailRow(layout, 1, "Template", cmbTemplate);
        AddDetailRow(layout, 2, "Location", txtLocation);

        pnlInspectionSummary.Controls.Add(lblInspectionSummaryTitle);
        pnlInspectionSummary.Controls.Add(layout);
    }

    private void BuildAppraisalSummaryPanel()
    {
        pnlAppraisalSummary.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlAppraisalSummary.Margin = new System.Windows.Forms.Padding(0, 0, 8, 8);
        pnlAppraisalSummary.Padding = new System.Windows.Forms.Padding(14);
        pnlAppraisalSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        lblAppraisalTitle.AutoSize = true;
        lblAppraisalTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        lblAppraisalTitle.Location = new System.Drawing.Point(14, 10);
        lblAppraisalTitle.Text = "Appraisal";

        var layout = CreateTwoColumnDetailsLayout();
        layout.Location = new System.Drawing.Point(14, 38);
        layout.Size = new System.Drawing.Size(350, 138);

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
        lblVehicleTitle.Text = "Vehicle";

        var layout = CreateTwoColumnDetailsLayout();
        layout.Location = new System.Drawing.Point(14, 38);
        layout.Size = new System.Drawing.Size(310, 138);

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
        pnlFooter.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);

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

    private static System.Windows.Forms.TableLayoutPanel CreateTwoColumnDetailsLayout()
    {
        var layout = new System.Windows.Forms.TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 6
        };
        layout.ColumnStyles.Add(
            new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Absolute,
                125F));
        layout.ColumnStyles.Add(
            new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent,
                100F));

        for (var row = 0; row < 6; row++)
        {
            layout.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Absolute,
                    26F));
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
        valueControl.Margin = new System.Windows.Forms.Padding(0, 1, 0, 2);

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
