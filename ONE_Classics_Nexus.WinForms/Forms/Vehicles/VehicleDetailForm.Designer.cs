// ============================================================
// VehicleDetailForm.Designer.cs
// ============================================================

using ONE_Classics_Nexus.Application.Requests;

namespace ONE_Classics_Nexus.WinForms.Forms.Vehicles
{
    partial class VehicleDetailForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelHeader = new Panel();
            lblSubtitle = new Label();
            lblTitle = new Label();
            panelToolbar = new Panel();
            btnDelete = new Button();
            btnNew = new Button();
            btnSave = new Button();
            cboSelectVehicle = new ComboBox();
            lblLookupVehicleId = new Label();
            panelContent = new Panel();
            tabControl = new TabControl();
            tabCore = new TabPage();
            tblCore = new TableLayoutPanel();
            lblVehicleId = new Label();
            txtVehicleId = new TextBox();
            lblVin = new Label();
            txtVin = new TextBox();
            lblYear = new Label();
            numYear = new NumericUpDown();
            lblMake = new Label();
            txtMake = new TextBox();
            lblModel = new Label();
            txtModel = new TextBox();
            lblTrim = new Label();
            txtTrim = new TextBox();
            lblSeriesName = new Label();
            txtSeriesName = new TextBox();
            lblBodyStyle = new Label();
            txtBodyStyle = new TextBox();
            lblDrivetrain = new Label();
            txtDrivetrain = new TextBox();
            lblEngine = new Label();
            txtEngine = new TextBox();
            lblTransmission = new Label();
            txtTransmission = new TextBox();
            lblMileage = new Label();
            numMileage = new NumericUpDown();
            lblOdometerUnit = new Label();
            cboOdometerUnit = new ComboBox();
            chkMileageUnknown = new CheckBox();
            tabAttributes = new TabPage();
            tblAttributes = new TableLayoutPanel();
            lblExteriorColor = new Label();
            txtExteriorColor = new TextBox();
            lblInteriorColor = new Label();
            txtInteriorColor = new TextBox();
            lblTitleStatus = new Label();
            cboTitleStatus = new ComboBox();
            cboAuthenticityProfile = new ComboBox();
            lblRestorationLevel = new Label();
            cboRestorationLevel = new ComboBox();
            lblDocumentationLevel = new Label();
            cboDocumentationLevel = new ComboBox();
            lblOverallConditionScore = new Label();
            numOverallConditionScore = new NumericUpDown();
            chkHasRedFlags = new CheckBox();
            lblOwnershipNotes = new Label();
            txtOwnershipNotes = new TextBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
            lblAuthenticityProfile = new Label();
            tabAudit = new TabPage();
            tblAudit = new TableLayoutPanel();
            lblCreatedAtUtc = new Label();
            txtCreatedAtUtc = new TextBox();
            lblUpdatedAtUtc = new Label();
            txtUpdatedAtUtc = new TextBox();
            panelFooter = new Panel();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            errorProvider = new ErrorProvider(components);
            panelHeader.SuspendLayout();
            panelToolbar.SuspendLayout();
            panelContent.SuspendLayout();
            tabControl.SuspendLayout();
            tabCore.SuspendLayout();
            tblCore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numYear).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMileage).BeginInit();
            tabAttributes.SuspendLayout();
            tblAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numOverallConditionScore).BeginInit();
            tabAudit.SuspendLayout();
            tblAudit.SuspendLayout();
            panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = SystemColors.Control;
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(24, 16, 24, 10);
            panelHeader.Size = new Size(1280, 82);
            panelHeader.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(90, 90, 90);
            lblSubtitle.Location = new Point(28, 48);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(0, 19);
            lblSubtitle.TabIndex = 1;
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
            lblTitle.Text = "Vehicle Detail";
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.FromArgb(245, 247, 250);
            panelToolbar.Controls.Add(btnDelete);
            panelToolbar.Controls.Add(btnNew);
            panelToolbar.Controls.Add(btnSave);
            panelToolbar.Controls.Add(cboSelectVehicle);
            panelToolbar.Controls.Add(lblLookupVehicleId);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 82);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Padding = new Padding(24, 10, 24, 10);
            panelToolbar.Size = new Size(1280, 64);
            panelToolbar.TabIndex = 1;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.Location = new Point(1144, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(108, 36);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnNew
            // 
            btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNew.Location = new Point(1030, 12);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(108, 36);
            btnNew.TabIndex = 4;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(31, 78, 121);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(916, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(108, 36);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // cboSelectVehicle
            // 
            cboSelectVehicle.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelectVehicle.FormattingEnabled = true;
            cboSelectVehicle.Location = new Point(132, 16);
            cboSelectVehicle.Name = "cboSelectVehicle";
            cboSelectVehicle.Size = new Size(410, 23);
            cboSelectVehicle.TabIndex = 1;
            cboSelectVehicle.SelectedIndexChanged += cboSelectVehicle_SelectedIndexChanged;
            // 
            // lblLookupVehicleId
            // 
            lblLookupVehicleId.AutoSize = true;
            lblLookupVehicleId.Location = new Point(24, 22);
            lblLookupVehicleId.Name = "lblLookupVehicleId";
            lblLookupVehicleId.Size = new Size(58, 15);
            lblLookupVehicleId.TabIndex = 0;
            lblLookupVehicleId.Text = "Vehicle ID";
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.FromArgb(245, 247, 250);
            panelContent.Controls.Add(tabControl);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 146);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(24, 12, 24, 12);
            panelContent.Size = new Size(1280, 522);
            panelContent.TabIndex = 2;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabCore);
            tabControl.Controls.Add(tabAttributes);
            tabControl.Controls.Add(tabAudit);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(24, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1232, 498);
            tabControl.TabIndex = 0;
            // 
            // tabCore
            // 
            tabCore.Controls.Add(tblCore);
            tabCore.Location = new Point(4, 24);
            tabCore.Name = "tabCore";
            tabCore.Padding = new Padding(12);
            tabCore.Size = new Size(1224, 470);
            tabCore.TabIndex = 0;
            tabCore.Text = "Core";
            tabCore.UseVisualStyleBackColor = true;
            // 
            // tblCore
            // 
            tblCore.ColumnCount = 4;
            tblCore.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tblCore.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblCore.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tblCore.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblCore.Controls.Add(lblVehicleId, 0, 0);
            tblCore.Controls.Add(txtVehicleId, 1, 0);
            tblCore.Controls.Add(lblVin, 2, 0);
            tblCore.Controls.Add(txtVin, 3, 0);
            tblCore.Controls.Add(lblYear, 0, 1);
            tblCore.Controls.Add(numYear, 1, 1);
            tblCore.Controls.Add(lblMake, 2, 1);
            tblCore.Controls.Add(txtMake, 3, 1);
            tblCore.Controls.Add(lblModel, 0, 2);
            tblCore.Controls.Add(txtModel, 1, 2);
            tblCore.Controls.Add(lblTrim, 2, 2);
            tblCore.Controls.Add(txtTrim, 3, 2);
            tblCore.Controls.Add(lblSeriesName, 0, 3);
            tblCore.Controls.Add(txtSeriesName, 1, 3);
            tblCore.Controls.Add(lblBodyStyle, 2, 3);
            tblCore.Controls.Add(txtBodyStyle, 3, 3);
            tblCore.Controls.Add(lblDrivetrain, 0, 4);
            tblCore.Controls.Add(txtDrivetrain, 1, 4);
            tblCore.Controls.Add(lblEngine, 2, 4);
            tblCore.Controls.Add(txtEngine, 3, 4);
            tblCore.Controls.Add(lblTransmission, 0, 5);
            tblCore.Controls.Add(txtTransmission, 1, 5);
            tblCore.Controls.Add(lblMileage, 2, 5);
            tblCore.Controls.Add(numMileage, 3, 5);
            tblCore.Controls.Add(lblOdometerUnit, 0, 6);
            tblCore.Controls.Add(cboOdometerUnit, 1, 6);
            tblCore.Controls.Add(chkMileageUnknown, 3, 6);
            tblCore.Dock = DockStyle.Fill;
            tblCore.Location = new Point(12, 12);
            tblCore.Name = "tblCore";
            tblCore.RowCount = 8;
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblCore.Size = new Size(1200, 446);
            tblCore.TabIndex = 0;
            // 
            // lblVehicleId
            // 
            lblVehicleId.Location = new Point(3, 0);
            lblVehicleId.Name = "lblVehicleId";
            lblVehicleId.Size = new Size(100, 23);
            lblVehicleId.TabIndex = 0;
            lblVehicleId.Text = "Vehicle ID";
            lblVehicleId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtVehicleId
            // 
            txtVehicleId.Dock = DockStyle.Fill;
            txtVehicleId.Location = new Point(143, 3);
            txtVehicleId.Name = "txtVehicleId";
            txtVehicleId.Size = new Size(454, 23);
            txtVehicleId.TabIndex = 1;
            txtVehicleId.TextChanged += txtVehicleId_TextChanged;
            // 
            // lblVin
            // 
            lblVin.Location = new Point(603, 0);
            lblVin.Name = "lblVin";
            lblVin.Size = new Size(100, 23);
            lblVin.TabIndex = 2;
            lblVin.Text = "VIN";
            lblVin.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtVin
            // 
            txtVin.Dock = DockStyle.Fill;
            txtVin.Location = new Point(743, 3);
            txtVin.Name = "txtVin";
            txtVin.Size = new Size(454, 23);
            txtVin.TabIndex = 3;
            // 
            // lblYear
            // 
            lblYear.Location = new Point(3, 42);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(100, 23);
            lblYear.TabIndex = 4;
            lblYear.Text = "Year *";
            lblYear.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numYear
            // 
            numYear.Dock = DockStyle.Left;
            numYear.Location = new Point(143, 45);
            numYear.Name = "numYear";
            numYear.Size = new Size(120, 23);
            numYear.TabIndex = 5;
            // 
            // lblMake
            // 
            lblMake.Location = new Point(603, 42);
            lblMake.Name = "lblMake";
            lblMake.Size = new Size(100, 23);
            lblMake.TabIndex = 6;
            lblMake.Text = "Make *";
            lblMake.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMake
            // 
            txtMake.Dock = DockStyle.Fill;
            txtMake.Location = new Point(743, 45);
            txtMake.Name = "txtMake";
            txtMake.Size = new Size(454, 23);
            txtMake.TabIndex = 7;
            // 
            // lblModel
            // 
            lblModel.Location = new Point(3, 84);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(100, 23);
            lblModel.TabIndex = 8;
            lblModel.Text = "Model *";
            lblModel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtModel
            // 
            txtModel.Dock = DockStyle.Fill;
            txtModel.Location = new Point(143, 87);
            txtModel.Name = "txtModel";
            txtModel.Size = new Size(454, 23);
            txtModel.TabIndex = 9;
            // 
            // lblTrim
            // 
            lblTrim.Location = new Point(603, 84);
            lblTrim.Name = "lblTrim";
            lblTrim.Size = new Size(100, 23);
            lblTrim.TabIndex = 10;
            lblTrim.Text = "Trim";
            lblTrim.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTrim
            // 
            txtTrim.Dock = DockStyle.Fill;
            txtTrim.Location = new Point(743, 87);
            txtTrim.Name = "txtTrim";
            txtTrim.Size = new Size(454, 23);
            txtTrim.TabIndex = 11;
            // 
            // lblSeriesName
            // 
            lblSeriesName.Location = new Point(3, 126);
            lblSeriesName.Name = "lblSeriesName";
            lblSeriesName.Size = new Size(100, 23);
            lblSeriesName.TabIndex = 12;
            lblSeriesName.Text = "Series Name";
            lblSeriesName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSeriesName
            // 
            txtSeriesName.Dock = DockStyle.Fill;
            txtSeriesName.Location = new Point(143, 129);
            txtSeriesName.Name = "txtSeriesName";
            txtSeriesName.Size = new Size(454, 23);
            txtSeriesName.TabIndex = 13;
            // 
            // lblBodyStyle
            // 
            lblBodyStyle.Location = new Point(603, 126);
            lblBodyStyle.Name = "lblBodyStyle";
            lblBodyStyle.Size = new Size(100, 23);
            lblBodyStyle.TabIndex = 14;
            lblBodyStyle.Text = "Body Style";
            lblBodyStyle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtBodyStyle
            // 
            txtBodyStyle.Dock = DockStyle.Fill;
            txtBodyStyle.Location = new Point(743, 129);
            txtBodyStyle.Name = "txtBodyStyle";
            txtBodyStyle.Size = new Size(454, 23);
            txtBodyStyle.TabIndex = 15;
            // 
            // lblDrivetrain
            // 
            lblDrivetrain.Location = new Point(3, 168);
            lblDrivetrain.Name = "lblDrivetrain";
            lblDrivetrain.Size = new Size(100, 23);
            lblDrivetrain.TabIndex = 16;
            lblDrivetrain.Text = "Drivetrain";
            lblDrivetrain.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDrivetrain
            // 
            txtDrivetrain.Dock = DockStyle.Fill;
            txtDrivetrain.Location = new Point(143, 171);
            txtDrivetrain.Name = "txtDrivetrain";
            txtDrivetrain.Size = new Size(454, 23);
            txtDrivetrain.TabIndex = 17;
            // 
            // lblEngine
            // 
            lblEngine.Location = new Point(603, 168);
            lblEngine.Name = "lblEngine";
            lblEngine.Size = new Size(100, 23);
            lblEngine.TabIndex = 18;
            lblEngine.Text = "Engine";
            lblEngine.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtEngine
            // 
            txtEngine.Dock = DockStyle.Fill;
            txtEngine.Location = new Point(743, 171);
            txtEngine.Name = "txtEngine";
            txtEngine.Size = new Size(454, 23);
            txtEngine.TabIndex = 19;
            // 
            // lblTransmission
            // 
            lblTransmission.Location = new Point(3, 210);
            lblTransmission.Name = "lblTransmission";
            lblTransmission.Size = new Size(100, 23);
            lblTransmission.TabIndex = 20;
            lblTransmission.Text = "Transmission";
            lblTransmission.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTransmission
            // 
            txtTransmission.Dock = DockStyle.Fill;
            txtTransmission.Location = new Point(143, 213);
            txtTransmission.Name = "txtTransmission";
            txtTransmission.Size = new Size(454, 23);
            txtTransmission.TabIndex = 21;
            // 
            // lblMileage
            // 
            lblMileage.Location = new Point(603, 210);
            lblMileage.Name = "lblMileage";
            lblMileage.Size = new Size(100, 23);
            lblMileage.TabIndex = 22;
            lblMileage.Text = "Mileage";
            lblMileage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numMileage
            // 
            numMileage.Dock = DockStyle.Left;
            numMileage.Location = new Point(743, 213);
            numMileage.Name = "numMileage";
            numMileage.Size = new Size(160, 23);
            numMileage.TabIndex = 23;
            // 
            // lblOdometerUnit
            // 
            lblOdometerUnit.Location = new Point(3, 252);
            lblOdometerUnit.Name = "lblOdometerUnit";
            lblOdometerUnit.Size = new Size(100, 23);
            lblOdometerUnit.TabIndex = 24;
            lblOdometerUnit.Text = "Odometer Unit";
            lblOdometerUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboOdometerUnit
            // 
            cboOdometerUnit.Dock = DockStyle.Left;
            cboOdometerUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOdometerUnit.Location = new Point(143, 255);
            cboOdometerUnit.Name = "cboOdometerUnit";
            cboOdometerUnit.Size = new Size(160, 23);
            cboOdometerUnit.TabIndex = 25;
            // 
            // chkMileageUnknown
            // 
            chkMileageUnknown.Anchor = AnchorStyles.Left;
            chkMileageUnknown.AutoSize = true;
            chkMileageUnknown.Location = new Point(743, 263);
            chkMileageUnknown.Name = "chkMileageUnknown";
            chkMileageUnknown.Size = new Size(122, 19);
            chkMileageUnknown.TabIndex = 26;
            chkMileageUnknown.Text = "Mileage Unknown";
            // 
            // tabAttributes
            // 
            tabAttributes.Controls.Add(tblAttributes);
            tabAttributes.Location = new Point(4, 24);
            tabAttributes.Name = "tabAttributes";
            tabAttributes.Padding = new Padding(12);
            tabAttributes.Size = new Size(1224, 470);
            tabAttributes.TabIndex = 1;
            tabAttributes.Text = "Attributes";
            tabAttributes.UseVisualStyleBackColor = true;
            // 
            // tblAttributes
            // 
            tblAttributes.ColumnCount = 4;
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 158F));
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 158F));
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblAttributes.Controls.Add(lblExteriorColor, 0, 0);
            tblAttributes.Controls.Add(txtExteriorColor, 1, 0);
            tblAttributes.Controls.Add(lblInteriorColor, 2, 0);
            tblAttributes.Controls.Add(txtInteriorColor, 3, 0);
            tblAttributes.Controls.Add(lblTitleStatus, 0, 1);
            tblAttributes.Controls.Add(cboTitleStatus, 1, 1);
            tblAttributes.Controls.Add(cboAuthenticityProfile, 3, 1);
            tblAttributes.Controls.Add(lblRestorationLevel, 0, 2);
            tblAttributes.Controls.Add(cboRestorationLevel, 1, 2);
            tblAttributes.Controls.Add(lblDocumentationLevel, 2, 2);
            tblAttributes.Controls.Add(cboDocumentationLevel, 3, 2);
            tblAttributes.Controls.Add(lblOverallConditionScore, 0, 3);
            tblAttributes.Controls.Add(numOverallConditionScore, 1, 3);
            tblAttributes.Controls.Add(chkHasRedFlags, 3, 3);
            tblAttributes.Controls.Add(lblOwnershipNotes, 0, 4);
            tblAttributes.Controls.Add(txtOwnershipNotes, 1, 4);
            tblAttributes.Controls.Add(lblNotes, 0, 5);
            tblAttributes.Controls.Add(txtNotes, 1, 5);
            tblAttributes.Controls.Add(lblAuthenticityProfile, 2, 1);
            tblAttributes.Dock = DockStyle.Fill;
            tblAttributes.Location = new Point(12, 12);
            tblAttributes.Name = "tblAttributes";
            tblAttributes.RowCount = 7;
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
            tblAttributes.Size = new Size(1200, 446);
            tblAttributes.TabIndex = 0;
            tblAttributes.Paint += tblAttributes_Paint;
            // 
            // lblExteriorColor
            // 
            lblExteriorColor.Location = new Point(3, 0);
            lblExteriorColor.Name = "lblExteriorColor";
            lblExteriorColor.Size = new Size(122, 23);
            lblExteriorColor.TabIndex = 0;
            lblExteriorColor.Text = "Exterior Color";
            lblExteriorColor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtExteriorColor
            // 
            txtExteriorColor.Dock = DockStyle.Fill;
            txtExteriorColor.Location = new Point(161, 3);
            txtExteriorColor.Name = "txtExteriorColor";
            txtExteriorColor.Size = new Size(436, 23);
            txtExteriorColor.TabIndex = 1;
            // 
            // lblInteriorColor
            // 
            lblInteriorColor.Location = new Point(603, 0);
            lblInteriorColor.Name = "lblInteriorColor";
            lblInteriorColor.Size = new Size(122, 23);
            lblInteriorColor.TabIndex = 2;
            lblInteriorColor.Text = "Interior Color";
            lblInteriorColor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtInteriorColor
            // 
            txtInteriorColor.Dock = DockStyle.Fill;
            txtInteriorColor.Location = new Point(761, 3);
            txtInteriorColor.Name = "txtInteriorColor";
            txtInteriorColor.Size = new Size(436, 23);
            txtInteriorColor.TabIndex = 3;
            // 
            // lblTitleStatus
            // 
            lblTitleStatus.Location = new Point(3, 42);
            lblTitleStatus.Name = "lblTitleStatus";
            lblTitleStatus.Size = new Size(122, 23);
            lblTitleStatus.TabIndex = 4;
            lblTitleStatus.Text = "Title Status";
            lblTitleStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboTitleStatus
            // 
            cboTitleStatus.Dock = DockStyle.Left;
            cboTitleStatus.Location = new Point(161, 45);
            cboTitleStatus.Name = "cboTitleStatus";
            cboTitleStatus.Size = new Size(180, 23);
            cboTitleStatus.TabIndex = 5;
            // 
            // cboAuthenticityProfile
            // 
            cboAuthenticityProfile.Dock = DockStyle.Left;
            cboAuthenticityProfile.Location = new Point(761, 45);
            cboAuthenticityProfile.Name = "cboAuthenticityProfile";
            cboAuthenticityProfile.Size = new Size(180, 23);
            cboAuthenticityProfile.TabIndex = 7;
            // 
            // lblRestorationLevel
            // 
            lblRestorationLevel.Location = new Point(3, 84);
            lblRestorationLevel.Name = "lblRestorationLevel";
            lblRestorationLevel.Size = new Size(122, 23);
            lblRestorationLevel.TabIndex = 8;
            lblRestorationLevel.Text = "Restoration";
            lblRestorationLevel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboRestorationLevel
            // 
            cboRestorationLevel.Dock = DockStyle.Left;
            cboRestorationLevel.Location = new Point(161, 87);
            cboRestorationLevel.Name = "cboRestorationLevel";
            cboRestorationLevel.Size = new Size(180, 23);
            cboRestorationLevel.TabIndex = 9;
            // 
            // lblDocumentationLevel
            // 
            lblDocumentationLevel.Location = new Point(603, 84);
            lblDocumentationLevel.Name = "lblDocumentationLevel";
            lblDocumentationLevel.Size = new Size(122, 23);
            lblDocumentationLevel.TabIndex = 10;
            lblDocumentationLevel.Text = "Documentation";
            lblDocumentationLevel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboDocumentationLevel
            // 
            cboDocumentationLevel.Dock = DockStyle.Left;
            cboDocumentationLevel.Location = new Point(761, 87);
            cboDocumentationLevel.Name = "cboDocumentationLevel";
            cboDocumentationLevel.Size = new Size(180, 23);
            cboDocumentationLevel.TabIndex = 11;
            // 
            // lblOverallConditionScore
            // 
            lblOverallConditionScore.Location = new Point(3, 126);
            lblOverallConditionScore.Name = "lblOverallConditionScore";
            lblOverallConditionScore.Size = new Size(149, 23);
            lblOverallConditionScore.TabIndex = 12;
            lblOverallConditionScore.Text = "Condition Score (0-5)";
            lblOverallConditionScore.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numOverallConditionScore
            // 
            numOverallConditionScore.DecimalPlaces = 1;
            numOverallConditionScore.Dock = DockStyle.Left;
            numOverallConditionScore.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numOverallConditionScore.Location = new Point(161, 129);
            numOverallConditionScore.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            numOverallConditionScore.Name = "numOverallConditionScore";
            numOverallConditionScore.Size = new Size(105, 23);
            numOverallConditionScore.TabIndex = 13;
            // 
            // chkHasRedFlags
            // 
            chkHasRedFlags.Anchor = AnchorStyles.Left;
            chkHasRedFlags.AutoSize = true;
            chkHasRedFlags.ForeColor = Color.FromArgb(192, 0, 0);
            chkHasRedFlags.Location = new Point(761, 137);
            chkHasRedFlags.Name = "chkHasRedFlags";
            chkHasRedFlags.Size = new Size(99, 19);
            chkHasRedFlags.TabIndex = 14;
            chkHasRedFlags.Text = "Has Red Flags";
            // 
            // lblOwnershipNotes
            // 
            lblOwnershipNotes.Location = new Point(3, 168);
            lblOwnershipNotes.Name = "lblOwnershipNotes";
            lblOwnershipNotes.Size = new Size(122, 23);
            lblOwnershipNotes.TabIndex = 15;
            lblOwnershipNotes.Text = "Ownership Notes";
            lblOwnershipNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtOwnershipNotes
            // 
            tblAttributes.SetColumnSpan(txtOwnershipNotes, 3);
            txtOwnershipNotes.Dock = DockStyle.Fill;
            txtOwnershipNotes.Location = new Point(161, 171);
            txtOwnershipNotes.Multiline = true;
            txtOwnershipNotes.Name = "txtOwnershipNotes";
            txtOwnershipNotes.ScrollBars = ScrollBars.Vertical;
            txtOwnershipNotes.Size = new Size(1036, 94);
            txtOwnershipNotes.TabIndex = 16;
            // 
            // lblNotes
            // 
            lblNotes.Location = new Point(3, 268);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(122, 23);
            lblNotes.TabIndex = 17;
            lblNotes.Text = "Notes";
            lblNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            tblAttributes.SetColumnSpan(txtNotes, 3);
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(161, 271);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(1036, 160);
            txtNotes.TabIndex = 18;
            // 
            // lblAuthenticityProfile
            // 
            lblAuthenticityProfile.Location = new Point(603, 42);
            lblAuthenticityProfile.Name = "lblAuthenticityProfile";
            lblAuthenticityProfile.Size = new Size(122, 23);
            lblAuthenticityProfile.TabIndex = 6;
            lblAuthenticityProfile.Text = "Authenticity";
            lblAuthenticityProfile.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabAudit
            // 
            tabAudit.Controls.Add(tblAudit);
            tabAudit.Location = new Point(4, 24);
            tabAudit.Name = "tabAudit";
            tabAudit.Padding = new Padding(12);
            tabAudit.Size = new Size(1224, 470);
            tabAudit.TabIndex = 2;
            tabAudit.Text = "Audit";
            tabAudit.UseVisualStyleBackColor = true;
            // 
            // tblAudit
            // 
            tblAudit.ColumnCount = 2;
            tblAudit.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tblAudit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblAudit.Controls.Add(lblCreatedAtUtc, 0, 0);
            tblAudit.Controls.Add(txtCreatedAtUtc, 1, 0);
            tblAudit.Controls.Add(lblUpdatedAtUtc, 0, 1);
            tblAudit.Controls.Add(txtUpdatedAtUtc, 1, 1);
            tblAudit.Dock = DockStyle.Top;
            tblAudit.Location = new Point(12, 12);
            tblAudit.Name = "tblAudit";
            tblAudit.RowCount = 3;
            tblAudit.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblAudit.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tblAudit.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblAudit.Size = new Size(1200, 120);
            tblAudit.TabIndex = 0;
            // 
            // lblCreatedAtUtc
            // 
            lblCreatedAtUtc.Location = new Point(3, 0);
            lblCreatedAtUtc.Name = "lblCreatedAtUtc";
            lblCreatedAtUtc.Size = new Size(100, 23);
            lblCreatedAtUtc.TabIndex = 0;
            lblCreatedAtUtc.Text = "Created UTC";
            lblCreatedAtUtc.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCreatedAtUtc
            // 
            txtCreatedAtUtc.Dock = DockStyle.Fill;
            txtCreatedAtUtc.Location = new Point(143, 3);
            txtCreatedAtUtc.Name = "txtCreatedAtUtc";
            txtCreatedAtUtc.Size = new Size(1054, 23);
            txtCreatedAtUtc.TabIndex = 1;
            // 
            // lblUpdatedAtUtc
            // 
            lblUpdatedAtUtc.Location = new Point(3, 42);
            lblUpdatedAtUtc.Name = "lblUpdatedAtUtc";
            lblUpdatedAtUtc.Size = new Size(100, 23);
            lblUpdatedAtUtc.TabIndex = 2;
            lblUpdatedAtUtc.Text = "Updated UTC";
            lblUpdatedAtUtc.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUpdatedAtUtc
            // 
            txtUpdatedAtUtc.Dock = DockStyle.Fill;
            txtUpdatedAtUtc.Location = new Point(143, 45);
            txtUpdatedAtUtc.Name = "txtUpdatedAtUtc";
            txtUpdatedAtUtc.Size = new Size(1054, 23);
            txtUpdatedAtUtc.TabIndex = 3;
            // 
            // panelFooter
            // 
            panelFooter.BackColor = Color.White;
            panelFooter.Controls.Add(progressBar);
            panelFooter.Controls.Add(lblStatus);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 668);
            panelFooter.Name = "panelFooter";
            panelFooter.Padding = new Padding(24, 8, 24, 8);
            panelFooter.Size = new Size(1280, 52);
            panelFooter.TabIndex = 3;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBar.Location = new Point(1060, 12);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(192, 23);
            progressBar.TabIndex = 1;
            progressBar.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(24, 18);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Ready";
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // VehicleDetailForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1280, 720);
            Controls.Add(panelContent);
            Controls.Add(panelFooter);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Name = "VehicleDetailForm";
            Text = "VehicleDetailForm";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelToolbar.ResumeLayout(false);
            panelToolbar.PerformLayout();
            panelContent.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabCore.ResumeLayout(false);
            tblCore.ResumeLayout(false);
            tblCore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numYear).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMileage).EndInit();
            tabAttributes.ResumeLayout(false);
            tblAttributes.ResumeLayout(false);
            tblAttributes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numOverallConditionScore).EndInit();
            tabAudit.ResumeLayout(false);
            tblAudit.ResumeLayout(false);
            tblAudit.PerformLayout();
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblSubtitle;
        private Label lblTitle;
        private Panel panelToolbar;
        private Button btnDelete;
        private Button btnNew;
        private Button btnSave;
        private ComboBox cboSelectVehicle;
        private Label lblLookupVehicleId;
        private Panel panelContent;
        private TabControl tabControl;
        private TabPage tabCore;
        private TabPage tabAttributes;
        private TabPage tabAudit;
        private TableLayoutPanel tblCore;
        private TextBox txtVehicleId;
        private Label lblVehicleId;
        private TextBox txtVin;
        private Label lblVin;
        private NumericUpDown numYear;
        private Label lblYear;
        private TextBox txtMake;
        private Label lblMake;
        private TextBox txtModel;
        private Label lblModel;
        private TextBox txtTrim;
        private Label lblTrim;
        private TextBox txtSeriesName;
        private Label lblSeriesName;
        private TextBox txtBodyStyle;
        private Label lblBodyStyle;
        private TextBox txtDrivetrain;
        private Label lblDrivetrain;
        private TextBox txtEngine;
        private Label lblEngine;
        private TextBox txtTransmission;
        private Label lblTransmission;
        private NumericUpDown numMileage;
        private Label lblMileage;
        private CheckBox chkMileageUnknown;
        private ComboBox cboOdometerUnit;
        private Label lblOdometerUnit;
        private TableLayoutPanel tblAttributes;
        private TextBox txtExteriorColor;
        private Label lblExteriorColor;
        private TextBox txtInteriorColor;
        private Label lblInteriorColor;
        private ComboBox cboTitleStatus;
        private Label lblTitleStatus;
        private ComboBox cboAuthenticityProfile;
        private Label lblAuthenticityProfile;
        private ComboBox cboRestorationLevel;
        private Label lblRestorationLevel;
        private ComboBox cboDocumentationLevel;
        private Label lblDocumentationLevel;
        private NumericUpDown numOverallConditionScore;
        private Label lblOverallConditionScore;
        private CheckBox chkHasRedFlags;
        private TextBox txtOwnershipNotes;
        private Label lblOwnershipNotes;
        private TextBox txtNotes;
        private Label lblNotes;
        private TableLayoutPanel tblAudit;
        private TextBox txtCreatedAtUtc;
        private Label lblCreatedAtUtc;
        private TextBox txtUpdatedAtUtc;
        private Label lblUpdatedAtUtc;
        private Panel panelFooter;
        private ProgressBar progressBar;
        private Label lblStatus;
        private ErrorProvider errorProvider;
    }
}