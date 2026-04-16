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
            btnLoad = new Button();
            txtLookupVehicleId = new TextBox();
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
            lblAuthenticityProfile = new Label();
            cboAuthenticityProfile = new ComboBox();
            lblRestorationLevel = new Label();
            cboRestorationLevel = new ComboBox();
            lblOwnershipSummary = new Label();
            txtOwnershipSummary = new TextBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
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
            tabAudit.SuspendLayout();
            tblAudit.SuspendLayout();
            panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 4, 3, 4);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(27, 21, 27, 13);
            panelHeader.Size = new Size(1463, 109);
            panelHeader.TabIndex = 0;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(90, 90, 90);
            lblSubtitle.Location = new Point(32, 64);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(422, 23);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Vehicle master record aligned to ONE Classics schema";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(25, 35, 55);
            lblTitle.Location = new Point(27, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(229, 46);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Vehicle Detail";
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.FromArgb(245, 247, 250);
            panelToolbar.Controls.Add(btnDelete);
            panelToolbar.Controls.Add(btnNew);
            panelToolbar.Controls.Add(btnSave);
            panelToolbar.Controls.Add(btnLoad);
            panelToolbar.Controls.Add(txtLookupVehicleId);
            panelToolbar.Controls.Add(lblLookupVehicleId);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 109);
            panelToolbar.Margin = new Padding(3, 4, 3, 4);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Padding = new Padding(27, 13, 27, 13);
            panelToolbar.Size = new Size(1463, 85);
            panelToolbar.TabIndex = 1;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.Location = new Point(1307, 16);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(123, 48);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnNew
            // 
            btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNew.Location = new Point(1177, 16);
            btnNew.Margin = new Padding(3, 4, 3, 4);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(123, 48);
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
            btnSave.Location = new Point(1047, 16);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(123, 48);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(638, 16);
            btnLoad.Margin = new Padding(3, 4, 3, 4);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(110, 48);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // txtLookupVehicleId
            // 
            txtLookupVehicleId.Location = new Point(151, 24);
            txtLookupVehicleId.Margin = new Padding(3, 4, 3, 4);
            txtLookupVehicleId.Name = "txtLookupVehicleId";
            txtLookupVehicleId.Size = new Size(468, 27);
            txtLookupVehicleId.TabIndex = 1;
            // 
            // lblLookupVehicleId
            // 
            lblLookupVehicleId.AutoSize = true;
            lblLookupVehicleId.Location = new Point(27, 29);
            lblLookupVehicleId.Name = "lblLookupVehicleId";
            lblLookupVehicleId.Size = new Size(75, 20);
            lblLookupVehicleId.TabIndex = 0;
            lblLookupVehicleId.Text = "Vehicle ID";
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.FromArgb(245, 247, 250);
            panelContent.Controls.Add(tabControl);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 194);
            panelContent.Margin = new Padding(3, 4, 3, 4);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(27, 16, 27, 16);
            panelContent.Size = new Size(1463, 697);
            panelContent.TabIndex = 2;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabCore);
            tabControl.Controls.Add(tabAttributes);
            tabControl.Controls.Add(tabAudit);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(27, 16);
            tabControl.Margin = new Padding(3, 4, 3, 4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1409, 665);
            tabControl.TabIndex = 0;
            // 
            // tabCore
            // 
            tabCore.Controls.Add(tblCore);
            tabCore.Location = new Point(4, 29);
            tabCore.Margin = new Padding(3, 4, 3, 4);
            tabCore.Name = "tabCore";
            tabCore.Padding = new Padding(14, 16, 14, 16);
            tabCore.Size = new Size(1401, 632);
            tabCore.TabIndex = 0;
            tabCore.Text = "Core";
            tabCore.UseVisualStyleBackColor = true;
            // 
            // tblCore
            // 
            tblCore.ColumnCount = 4;
            tblCore.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblCore.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblCore.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
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
            tblCore.Location = new Point(14, 16);
            tblCore.Margin = new Padding(3, 4, 3, 4);
            tblCore.Name = "tblCore";
            tblCore.RowCount = 8;
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblCore.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblCore.Size = new Size(1373, 600);
            tblCore.TabIndex = 0;
            // 
            // lblVehicleId
            // 
            lblVehicleId.Location = new Point(3, 0);
            lblVehicleId.Name = "lblVehicleId";
            lblVehicleId.Size = new Size(114, 31);
            lblVehicleId.TabIndex = 0;
            lblVehicleId.Text = "Vehicle ID";
            lblVehicleId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtVehicleId
            // 
            txtVehicleId.Dock = DockStyle.Fill;
            txtVehicleId.Location = new Point(163, 4);
            txtVehicleId.Margin = new Padding(3, 4, 3, 4);
            txtVehicleId.Name = "txtVehicleId";
            txtVehicleId.Size = new Size(520, 27);
            txtVehicleId.TabIndex = 1;
            // 
            // lblVin
            // 
            lblVin.Location = new Point(689, 0);
            lblVin.Name = "lblVin";
            lblVin.Size = new Size(114, 31);
            lblVin.TabIndex = 2;
            lblVin.Text = "VIN";
            lblVin.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtVin
            // 
            txtVin.Dock = DockStyle.Fill;
            txtVin.Location = new Point(849, 4);
            txtVin.Margin = new Padding(3, 4, 3, 4);
            txtVin.Name = "txtVin";
            txtVin.Size = new Size(521, 27);
            txtVin.TabIndex = 3;
            // 
            // lblYear
            // 
            lblYear.Location = new Point(3, 56);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(114, 31);
            lblYear.TabIndex = 4;
            lblYear.Text = "Year *";
            lblYear.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numYear
            // 
            numYear.Dock = DockStyle.Left;
            numYear.Location = new Point(163, 60);
            numYear.Margin = new Padding(3, 4, 3, 4);
            numYear.Name = "numYear";
            numYear.Size = new Size(137, 27);
            numYear.TabIndex = 5;
            // 
            // lblMake
            // 
            lblMake.Location = new Point(689, 56);
            lblMake.Name = "lblMake";
            lblMake.Size = new Size(114, 31);
            lblMake.TabIndex = 6;
            lblMake.Text = "Make *";
            lblMake.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMake
            // 
            txtMake.Dock = DockStyle.Fill;
            txtMake.Location = new Point(849, 60);
            txtMake.Margin = new Padding(3, 4, 3, 4);
            txtMake.Name = "txtMake";
            txtMake.Size = new Size(521, 27);
            txtMake.TabIndex = 7;
            // 
            // lblModel
            // 
            lblModel.Location = new Point(3, 112);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(114, 31);
            lblModel.TabIndex = 8;
            lblModel.Text = "Model *";
            lblModel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtModel
            // 
            txtModel.Dock = DockStyle.Fill;
            txtModel.Location = new Point(163, 116);
            txtModel.Margin = new Padding(3, 4, 3, 4);
            txtModel.Name = "txtModel";
            txtModel.Size = new Size(520, 27);
            txtModel.TabIndex = 9;
            // 
            // lblTrim
            // 
            lblTrim.Location = new Point(689, 112);
            lblTrim.Name = "lblTrim";
            lblTrim.Size = new Size(114, 31);
            lblTrim.TabIndex = 10;
            lblTrim.Text = "Trim";
            lblTrim.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTrim
            // 
            txtTrim.Dock = DockStyle.Fill;
            txtTrim.Location = new Point(849, 116);
            txtTrim.Margin = new Padding(3, 4, 3, 4);
            txtTrim.Name = "txtTrim";
            txtTrim.Size = new Size(521, 27);
            txtTrim.TabIndex = 11;
            // 
            // lblSeriesName
            // 
            lblSeriesName.Location = new Point(3, 168);
            lblSeriesName.Name = "lblSeriesName";
            lblSeriesName.Size = new Size(114, 31);
            lblSeriesName.TabIndex = 12;
            lblSeriesName.Text = "Series Name";
            lblSeriesName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSeriesName
            // 
            txtSeriesName.Dock = DockStyle.Fill;
            txtSeriesName.Location = new Point(163, 172);
            txtSeriesName.Margin = new Padding(3, 4, 3, 4);
            txtSeriesName.Name = "txtSeriesName";
            txtSeriesName.Size = new Size(520, 27);
            txtSeriesName.TabIndex = 13;
            // 
            // lblBodyStyle
            // 
            lblBodyStyle.Location = new Point(689, 168);
            lblBodyStyle.Name = "lblBodyStyle";
            lblBodyStyle.Size = new Size(114, 31);
            lblBodyStyle.TabIndex = 14;
            lblBodyStyle.Text = "Body Style";
            lblBodyStyle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtBodyStyle
            // 
            txtBodyStyle.Dock = DockStyle.Fill;
            txtBodyStyle.Location = new Point(849, 172);
            txtBodyStyle.Margin = new Padding(3, 4, 3, 4);
            txtBodyStyle.Name = "txtBodyStyle";
            txtBodyStyle.Size = new Size(521, 27);
            txtBodyStyle.TabIndex = 15;
            // 
            // lblDrivetrain
            // 
            lblDrivetrain.Location = new Point(3, 224);
            lblDrivetrain.Name = "lblDrivetrain";
            lblDrivetrain.Size = new Size(114, 31);
            lblDrivetrain.TabIndex = 16;
            lblDrivetrain.Text = "Drivetrain";
            lblDrivetrain.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDrivetrain
            // 
            txtDrivetrain.Dock = DockStyle.Fill;
            txtDrivetrain.Location = new Point(163, 228);
            txtDrivetrain.Margin = new Padding(3, 4, 3, 4);
            txtDrivetrain.Name = "txtDrivetrain";
            txtDrivetrain.Size = new Size(520, 27);
            txtDrivetrain.TabIndex = 17;
            // 
            // lblEngine
            // 
            lblEngine.Location = new Point(689, 224);
            lblEngine.Name = "lblEngine";
            lblEngine.Size = new Size(114, 31);
            lblEngine.TabIndex = 18;
            lblEngine.Text = "Engine";
            lblEngine.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtEngine
            // 
            txtEngine.Dock = DockStyle.Fill;
            txtEngine.Location = new Point(849, 228);
            txtEngine.Margin = new Padding(3, 4, 3, 4);
            txtEngine.Name = "txtEngine";
            txtEngine.Size = new Size(521, 27);
            txtEngine.TabIndex = 19;
            // 
            // lblTransmission
            // 
            lblTransmission.Location = new Point(3, 280);
            lblTransmission.Name = "lblTransmission";
            lblTransmission.Size = new Size(114, 31);
            lblTransmission.TabIndex = 20;
            lblTransmission.Text = "Transmission";
            lblTransmission.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTransmission
            // 
            txtTransmission.Dock = DockStyle.Fill;
            txtTransmission.Location = new Point(163, 284);
            txtTransmission.Margin = new Padding(3, 4, 3, 4);
            txtTransmission.Name = "txtTransmission";
            txtTransmission.Size = new Size(520, 27);
            txtTransmission.TabIndex = 21;
            // 
            // lblMileage
            // 
            lblMileage.Location = new Point(689, 280);
            lblMileage.Name = "lblMileage";
            lblMileage.Size = new Size(114, 31);
            lblMileage.TabIndex = 22;
            lblMileage.Text = "Mileage";
            lblMileage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numMileage
            // 
            numMileage.Dock = DockStyle.Left;
            numMileage.Location = new Point(849, 284);
            numMileage.Margin = new Padding(3, 4, 3, 4);
            numMileage.Name = "numMileage";
            numMileage.Size = new Size(183, 27);
            numMileage.TabIndex = 23;
            // 
            // lblOdometerUnit
            // 
            lblOdometerUnit.Location = new Point(3, 336);
            lblOdometerUnit.Name = "lblOdometerUnit";
            lblOdometerUnit.Size = new Size(114, 31);
            lblOdometerUnit.TabIndex = 24;
            lblOdometerUnit.Text = "Odometer Unit";
            lblOdometerUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboOdometerUnit
            // 
            cboOdometerUnit.Dock = DockStyle.Left;
            cboOdometerUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOdometerUnit.Location = new Point(163, 340);
            cboOdometerUnit.Margin = new Padding(3, 4, 3, 4);
            cboOdometerUnit.Name = "cboOdometerUnit";
            cboOdometerUnit.Size = new Size(182, 28);
            cboOdometerUnit.TabIndex = 25;
            // 
            // chkMileageUnknown
            // 
            chkMileageUnknown.Anchor = AnchorStyles.Left;
            chkMileageUnknown.AutoSize = true;
            chkMileageUnknown.Location = new Point(849, 352);
            chkMileageUnknown.Margin = new Padding(3, 4, 3, 4);
            chkMileageUnknown.Name = "chkMileageUnknown";
            chkMileageUnknown.Size = new Size(150, 24);
            chkMileageUnknown.TabIndex = 26;
            chkMileageUnknown.Text = "Mileage Unknown";
            // 
            // tabAttributes
            // 
            tabAttributes.Controls.Add(tblAttributes);
            tabAttributes.Location = new Point(4, 29);
            tabAttributes.Margin = new Padding(3, 4, 3, 4);
            tabAttributes.Name = "tabAttributes";
            tabAttributes.Padding = new Padding(14, 16, 14, 16);
            tabAttributes.Size = new Size(1401, 632);
            tabAttributes.TabIndex = 1;
            tabAttributes.Text = "Attributes";
            tabAttributes.UseVisualStyleBackColor = true;
            // 
            // tblAttributes
            // 
            tblAttributes.ColumnCount = 4;
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblAttributes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblAttributes.Controls.Add(lblExteriorColor, 0, 0);
            tblAttributes.Controls.Add(txtExteriorColor, 1, 0);
            tblAttributes.Controls.Add(lblInteriorColor, 2, 0);
            tblAttributes.Controls.Add(txtInteriorColor, 3, 0);
            tblAttributes.Controls.Add(lblTitleStatus, 0, 1);
            tblAttributes.Controls.Add(cboTitleStatus, 1, 1);
            tblAttributes.Controls.Add(lblAuthenticityProfile, 2, 1);
            tblAttributes.Controls.Add(cboAuthenticityProfile, 3, 1);
            tblAttributes.Controls.Add(lblRestorationLevel, 0, 2);
            tblAttributes.Controls.Add(cboRestorationLevel, 1, 2);
            tblAttributes.Controls.Add(lblOwnershipSummary, 0, 3);
            tblAttributes.Controls.Add(txtOwnershipSummary, 1, 3);
            tblAttributes.Controls.Add(lblNotes, 0, 4);
            tblAttributes.Controls.Add(txtNotes, 1, 4);
            tblAttributes.Dock = DockStyle.Fill;
            tblAttributes.Location = new Point(14, 16);
            tblAttributes.Margin = new Padding(3, 4, 3, 4);
            tblAttributes.Name = "tblAttributes";
            tblAttributes.RowCount = 6;
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 133F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblAttributes.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            tblAttributes.Size = new Size(1373, 600);
            tblAttributes.TabIndex = 0;
            // 
            // lblExteriorColor
            // 
            lblExteriorColor.Location = new Point(3, 0);
            lblExteriorColor.Name = "lblExteriorColor";
            lblExteriorColor.Size = new Size(114, 31);
            lblExteriorColor.TabIndex = 0;
            lblExteriorColor.Text = "Exterior Color";
            lblExteriorColor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtExteriorColor
            // 
            txtExteriorColor.Dock = DockStyle.Fill;
            txtExteriorColor.Location = new Point(163, 4);
            txtExteriorColor.Margin = new Padding(3, 4, 3, 4);
            txtExteriorColor.Name = "txtExteriorColor";
            txtExteriorColor.Size = new Size(520, 27);
            txtExteriorColor.TabIndex = 1;
            // 
            // lblInteriorColor
            // 
            lblInteriorColor.Location = new Point(689, 0);
            lblInteriorColor.Name = "lblInteriorColor";
            lblInteriorColor.Size = new Size(114, 31);
            lblInteriorColor.TabIndex = 2;
            lblInteriorColor.Text = "Interior Color";
            lblInteriorColor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtInteriorColor
            // 
            txtInteriorColor.Dock = DockStyle.Fill;
            txtInteriorColor.Location = new Point(849, 4);
            txtInteriorColor.Margin = new Padding(3, 4, 3, 4);
            txtInteriorColor.Name = "txtInteriorColor";
            txtInteriorColor.Size = new Size(521, 27);
            txtInteriorColor.TabIndex = 3;
            // 
            // lblTitleStatus
            // 
            lblTitleStatus.Location = new Point(3, 56);
            lblTitleStatus.Name = "lblTitleStatus";
            lblTitleStatus.Size = new Size(114, 31);
            lblTitleStatus.TabIndex = 4;
            lblTitleStatus.Text = "Title Status";
            lblTitleStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboTitleStatus
            // 
            cboTitleStatus.Dock = DockStyle.Left;
            cboTitleStatus.Location = new Point(163, 60);
            cboTitleStatus.Margin = new Padding(3, 4, 3, 4);
            cboTitleStatus.Name = "cboTitleStatus";
            cboTitleStatus.Size = new Size(205, 28);
            cboTitleStatus.TabIndex = 5;
            // 
            // lblAuthenticityProfile
            // 
            lblAuthenticityProfile.Location = new Point(689, 56);
            lblAuthenticityProfile.Name = "lblAuthenticityProfile";
            lblAuthenticityProfile.Size = new Size(114, 31);
            lblAuthenticityProfile.TabIndex = 6;
            lblAuthenticityProfile.Text = "Authenticity";
            lblAuthenticityProfile.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboAuthenticityProfile
            // 
            cboAuthenticityProfile.Dock = DockStyle.Left;
            cboAuthenticityProfile.Location = new Point(849, 60);
            cboAuthenticityProfile.Margin = new Padding(3, 4, 3, 4);
            cboAuthenticityProfile.Name = "cboAuthenticityProfile";
            cboAuthenticityProfile.Size = new Size(205, 28);
            cboAuthenticityProfile.TabIndex = 7;
            // 
            // lblRestorationLevel
            // 
            lblRestorationLevel.Location = new Point(3, 112);
            lblRestorationLevel.Name = "lblRestorationLevel";
            lblRestorationLevel.Size = new Size(114, 31);
            lblRestorationLevel.TabIndex = 8;
            lblRestorationLevel.Text = "Restoration";
            lblRestorationLevel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboRestorationLevel
            // 
            cboRestorationLevel.Dock = DockStyle.Left;
            cboRestorationLevel.Location = new Point(163, 116);
            cboRestorationLevel.Margin = new Padding(3, 4, 3, 4);
            cboRestorationLevel.Name = "cboRestorationLevel";
            cboRestorationLevel.Size = new Size(205, 28);
            cboRestorationLevel.TabIndex = 9;
            // 
            // lblOwnershipSummary
            // 
            lblOwnershipSummary.Location = new Point(3, 168);
            lblOwnershipSummary.Name = "lblOwnershipSummary";
            lblOwnershipSummary.Size = new Size(114, 31);
            lblOwnershipSummary.TabIndex = 10;
            lblOwnershipSummary.Text = "Ownership Summary";
            lblOwnershipSummary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtOwnershipSummary
            // 
            tblAttributes.SetColumnSpan(txtOwnershipSummary, 3);
            txtOwnershipSummary.Dock = DockStyle.Fill;
            txtOwnershipSummary.Location = new Point(163, 172);
            txtOwnershipSummary.Margin = new Padding(3, 4, 3, 4);
            txtOwnershipSummary.Multiline = true;
            txtOwnershipSummary.Name = "txtOwnershipSummary";
            txtOwnershipSummary.ScrollBars = ScrollBars.Vertical;
            txtOwnershipSummary.Size = new Size(1207, 125);
            txtOwnershipSummary.TabIndex = 11;
            // 
            // lblNotes
            // 
            lblNotes.Location = new Point(3, 301);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(114, 31);
            lblNotes.TabIndex = 12;
            lblNotes.Text = "Notes";
            lblNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            tblAttributes.SetColumnSpan(txtNotes, 3);
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(163, 305);
            txtNotes.Margin = new Padding(3, 4, 3, 4);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(1207, 275);
            txtNotes.TabIndex = 13;
            // 
            // tabAudit
            // 
            tabAudit.Controls.Add(tblAudit);
            tabAudit.Location = new Point(4, 29);
            tabAudit.Margin = new Padding(3, 4, 3, 4);
            tabAudit.Name = "tabAudit";
            tabAudit.Padding = new Padding(14, 16, 14, 16);
            tabAudit.Size = new Size(1401, 632);
            tabAudit.TabIndex = 2;
            tabAudit.Text = "Audit";
            tabAudit.UseVisualStyleBackColor = true;
            // 
            // tblAudit
            // 
            tblAudit.ColumnCount = 2;
            tblAudit.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblAudit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblAudit.Controls.Add(lblCreatedAtUtc, 0, 0);
            tblAudit.Controls.Add(txtCreatedAtUtc, 1, 0);
            tblAudit.Controls.Add(lblUpdatedAtUtc, 0, 1);
            tblAudit.Controls.Add(txtUpdatedAtUtc, 1, 1);
            tblAudit.Dock = DockStyle.Top;
            tblAudit.Location = new Point(14, 16);
            tblAudit.Margin = new Padding(3, 4, 3, 4);
            tblAudit.Name = "tblAudit";
            tblAudit.RowCount = 3;
            tblAudit.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblAudit.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblAudit.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblAudit.Size = new Size(1373, 160);
            tblAudit.TabIndex = 0;
            // 
            // lblCreatedAtUtc
            // 
            lblCreatedAtUtc.Location = new Point(3, 0);
            lblCreatedAtUtc.Name = "lblCreatedAtUtc";
            lblCreatedAtUtc.Size = new Size(114, 31);
            lblCreatedAtUtc.TabIndex = 0;
            lblCreatedAtUtc.Text = "Created UTC";
            lblCreatedAtUtc.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCreatedAtUtc
            // 
            txtCreatedAtUtc.Dock = DockStyle.Fill;
            txtCreatedAtUtc.Location = new Point(163, 4);
            txtCreatedAtUtc.Margin = new Padding(3, 4, 3, 4);
            txtCreatedAtUtc.Name = "txtCreatedAtUtc";
            txtCreatedAtUtc.Size = new Size(1207, 27);
            txtCreatedAtUtc.TabIndex = 1;
            // 
            // lblUpdatedAtUtc
            // 
            lblUpdatedAtUtc.Location = new Point(3, 56);
            lblUpdatedAtUtc.Name = "lblUpdatedAtUtc";
            lblUpdatedAtUtc.Size = new Size(114, 31);
            lblUpdatedAtUtc.TabIndex = 2;
            lblUpdatedAtUtc.Text = "Updated UTC";
            lblUpdatedAtUtc.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUpdatedAtUtc
            // 
            txtUpdatedAtUtc.Dock = DockStyle.Fill;
            txtUpdatedAtUtc.Location = new Point(163, 60);
            txtUpdatedAtUtc.Margin = new Padding(3, 4, 3, 4);
            txtUpdatedAtUtc.Name = "txtUpdatedAtUtc";
            txtUpdatedAtUtc.Size = new Size(1207, 27);
            txtUpdatedAtUtc.TabIndex = 3;
            // 
            // panelFooter
            // 
            panelFooter.BackColor = Color.White;
            panelFooter.Controls.Add(progressBar);
            panelFooter.Controls.Add(lblStatus);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 891);
            panelFooter.Margin = new Padding(3, 4, 3, 4);
            panelFooter.Name = "panelFooter";
            panelFooter.Padding = new Padding(27, 11, 27, 11);
            panelFooter.Size = new Size(1463, 69);
            panelFooter.TabIndex = 3;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBar.Location = new Point(1211, 16);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(219, 31);
            progressBar.TabIndex = 1;
            progressBar.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(27, 24);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Ready";
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // VehicleDetailForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1463, 960);
            Controls.Add(panelContent);
            Controls.Add(panelFooter);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
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
        private Button btnLoad;
        private TextBox txtLookupVehicleId;
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
        private TextBox txtOwnershipSummary;
        private Label lblOwnershipSummary;
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