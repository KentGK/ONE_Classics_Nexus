namespace ONE_Classics_Nexus.WinForms.Forms.Dashboard
{
    partial class DashboardForm
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
            panelHeader = new Panel();
            lblSubTitle = new Label();
            lblWelcome = new Label();
            panelActions = new Panel();
            btnRefresh = new Button();
            btnRunValuation = new Button();
            btnNewInspection = new Button();
            btnNewAppraisal = new Button();
            panelStats = new TableLayoutPanel();
            pnlActiveAppraisals = new Panel();
            lblActiveAppraisalsValue = new Label();
            lblActiveAppraisalsTitle = new Label();
            pnlPendingInspections = new Panel();
            lblPendingInspectionsValue = new Label();
            lblPendingInspectionsTitle = new Label();
            pnlValuationsReady = new Panel();
            lblValuationsReadyValue = new Label();
            lblValuationsReadyTitle = new Label();
            pnlNeedsReview = new Panel();
            lblNeedsReviewValue = new Label();
            lblNeedsReviewTitle = new Label();
            panelBody = new TableLayoutPanel();
            panelRecentActivity = new Panel();
            lvRecentActivity = new ListView();
            colActivityDate = new ColumnHeader();
            colActivityType = new ColumnHeader();
            colActivityVehicle = new ColumnHeader();
            colActivityStatus = new ColumnHeader();
            lblRecentActivity = new Label();
            panelWorkQueue = new Panel();
            lvWorkQueue = new ListView();
            colQueueId = new ColumnHeader();
            colQueueVehicle = new ColumnHeader();
            colQueueStage = new ColumnHeader();
            colQueuePriority = new ColumnHeader();
            lblWorkQueue = new Label();
            panelFooter = new Panel();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            panelHeader.SuspendLayout();
            panelActions.SuspendLayout();
            panelStats.SuspendLayout();
            pnlActiveAppraisals.SuspendLayout();
            pnlPendingInspections.SuspendLayout();
            pnlValuationsReady.SuspendLayout();
            pnlNeedsReview.SuspendLayout();
            panelBody.SuspendLayout();
            panelRecentActivity.SuspendLayout();
            panelWorkQueue.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(lblSubTitle);
            panelHeader.Controls.Add(lblWelcome);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(24, 18, 24, 12);
            panelHeader.Size = new Size(1280, 90);
            panelHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubTitle.ForeColor = Color.FromArgb(90, 90, 90);
            lblSubTitle.Location = new Point(28, 50);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(322, 19);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Inspection-driven appraisal and valuation workspace";
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblWelcome.ForeColor = Color.FromArgb(25, 35, 55);
            lblWelcome.Location = new Point(24, 10);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(404, 37);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome to ONE Classics Nexus";
            // 
            // panelActions
            // 
            panelActions.BackColor = Color.FromArgb(245, 247, 250);
            panelActions.Controls.Add(btnRefresh);
            panelActions.Controls.Add(btnRunValuation);
            panelActions.Controls.Add(btnNewInspection);
            panelActions.Controls.Add(btnNewAppraisal);
            panelActions.Dock = DockStyle.Top;
            panelActions.Location = new Point(0, 90);
            panelActions.Name = "panelActions";
            panelActions.Padding = new Padding(24, 10, 24, 10);
            panelActions.Size = new Size(1280, 60);
            panelActions.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnRefresh.Location = new Point(1132, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnRunValuation
            // 
            btnRunValuation.BackColor = Color.FromArgb(31, 78, 121);
            btnRunValuation.FlatAppearance.BorderSize = 0;
            btnRunValuation.FlatStyle = FlatStyle.Flat;
            btnRunValuation.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnRunValuation.ForeColor = Color.White;
            btnRunValuation.Location = new Point(304, 10);
            btnRunValuation.Name = "btnRunValuation";
            btnRunValuation.Size = new Size(152, 36);
            btnRunValuation.TabIndex = 2;
            btnRunValuation.Text = "Run Valuation";
            btnRunValuation.UseVisualStyleBackColor = false;
            btnRunValuation.Click += btnRunValuation_Click;
            // 
            // btnNewInspection
            // 
            btnNewInspection.BackColor = Color.White;
            btnNewInspection.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
            btnNewInspection.FlatStyle = FlatStyle.Flat;
            btnNewInspection.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnNewInspection.Location = new Point(164, 10);
            btnNewInspection.Name = "btnNewInspection";
            btnNewInspection.Size = new Size(128, 36);
            btnNewInspection.TabIndex = 1;
            btnNewInspection.Text = "New Inspection";
            btnNewInspection.UseVisualStyleBackColor = false;
            btnNewInspection.Click += btnNewInspection_Click;
            // 
            // btnNewAppraisal
            // 
            btnNewAppraisal.BackColor = Color.White;
            btnNewAppraisal.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
            btnNewAppraisal.FlatStyle = FlatStyle.Flat;
            btnNewAppraisal.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnNewAppraisal.Location = new Point(24, 10);
            btnNewAppraisal.Name = "btnNewAppraisal";
            btnNewAppraisal.Size = new Size(128, 36);
            btnNewAppraisal.TabIndex = 0;
            btnNewAppraisal.Text = "New Appraisal";
            btnNewAppraisal.UseVisualStyleBackColor = false;
            btnNewAppraisal.Click += btnNewAppraisal_Click;
            // 
            // panelStats
            // 
            panelStats.BackColor = Color.FromArgb(245, 247, 250);
            panelStats.ColumnCount = 4;
            panelStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panelStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panelStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panelStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            panelStats.Controls.Add(pnlActiveAppraisals, 0, 0);
            panelStats.Controls.Add(pnlPendingInspections, 1, 0);
            panelStats.Controls.Add(pnlValuationsReady, 2, 0);
            panelStats.Controls.Add(pnlNeedsReview, 3, 0);
            panelStats.Dock = DockStyle.Top;
            panelStats.Location = new Point(0, 150);
            panelStats.Name = "panelStats";
            panelStats.Padding = new Padding(24, 12, 24, 12);
            panelStats.RowCount = 1;
            panelStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelStats.Size = new Size(1280, 130);
            panelStats.TabIndex = 2;
            // 
            // pnlActiveAppraisals
            // 
            pnlActiveAppraisals.BackColor = Color.White;
            pnlActiveAppraisals.Controls.Add(lblActiveAppraisalsValue);
            pnlActiveAppraisals.Controls.Add(lblActiveAppraisalsTitle);
            pnlActiveAppraisals.Dock = DockStyle.Fill;
            pnlActiveAppraisals.Location = new Point(27, 15);
            pnlActiveAppraisals.Name = "pnlActiveAppraisals";
            pnlActiveAppraisals.Padding = new Padding(18, 16, 18, 16);
            pnlActiveAppraisals.Size = new Size(299, 100);
            pnlActiveAppraisals.TabIndex = 0;
            // 
            // lblActiveAppraisalsValue
            // 
            lblActiveAppraisalsValue.AutoSize = true;
            lblActiveAppraisalsValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lblActiveAppraisalsValue.ForeColor = Color.FromArgb(31, 78, 121);
            lblActiveAppraisalsValue.Location = new Point(18, 40);
            lblActiveAppraisalsValue.Name = "lblActiveAppraisalsValue";
            lblActiveAppraisalsValue.Size = new Size(37, 45);
            lblActiveAppraisalsValue.TabIndex = 1;
            lblActiveAppraisalsValue.Text = "0";
            // 
            // lblActiveAppraisalsTitle
            // 
            lblActiveAppraisalsTitle.AutoSize = true;
            lblActiveAppraisalsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblActiveAppraisalsTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblActiveAppraisalsTitle.Location = new Point(18, 16);
            lblActiveAppraisalsTitle.Name = "lblActiveAppraisalsTitle";
            lblActiveAppraisalsTitle.Size = new Size(106, 19);
            lblActiveAppraisalsTitle.TabIndex = 0;
            lblActiveAppraisalsTitle.Text = "Active Appraisals";
            // 
            // pnlPendingInspections
            // 
            pnlPendingInspections.BackColor = Color.White;
            pnlPendingInspections.Controls.Add(lblPendingInspectionsValue);
            pnlPendingInspections.Controls.Add(lblPendingInspectionsTitle);
            pnlPendingInspections.Dock = DockStyle.Fill;
            pnlPendingInspections.Location = new Point(332, 15);
            pnlPendingInspections.Name = "pnlPendingInspections";
            pnlPendingInspections.Padding = new Padding(18, 16, 18, 16);
            pnlPendingInspections.Size = new Size(299, 100);
            pnlPendingInspections.TabIndex = 1;
            // 
            // lblPendingInspectionsValue
            // 
            lblPendingInspectionsValue.AutoSize = true;
            lblPendingInspectionsValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lblPendingInspectionsValue.ForeColor = Color.FromArgb(31, 78, 121);
            lblPendingInspectionsValue.Location = new Point(18, 40);
            lblPendingInspectionsValue.Name = "lblPendingInspectionsValue";
            lblPendingInspectionsValue.Size = new Size(37, 45);
            lblPendingInspectionsValue.TabIndex = 1;
            lblPendingInspectionsValue.Text = "0";
            // 
            // lblPendingInspectionsTitle
            // 
            lblPendingInspectionsTitle.AutoSize = true;
            lblPendingInspectionsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPendingInspectionsTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblPendingInspectionsTitle.Location = new Point(18, 16);
            lblPendingInspectionsTitle.Name = "lblPendingInspectionsTitle";
            lblPendingInspectionsTitle.Size = new Size(131, 19);
            lblPendingInspectionsTitle.TabIndex = 0;
            lblPendingInspectionsTitle.Text = "Pending Inspections";
            // 
            // pnlValuationsReady
            // 
            pnlValuationsReady.BackColor = Color.White;
            pnlValuationsReady.Controls.Add(lblValuationsReadyValue);
            pnlValuationsReady.Controls.Add(lblValuationsReadyTitle);
            pnlValuationsReady.Dock = DockStyle.Fill;
            pnlValuationsReady.Location = new Point(637, 15);
            pnlValuationsReady.Name = "pnlValuationsReady";
            pnlValuationsReady.Padding = new Padding(18, 16, 18, 16);
            pnlValuationsReady.Size = new Size(299, 100);
            pnlValuationsReady.TabIndex = 2;
            // 
            // lblValuationsReadyValue
            // 
            lblValuationsReadyValue.AutoSize = true;
            lblValuationsReadyValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lblValuationsReadyValue.ForeColor = Color.FromArgb(31, 78, 121);
            lblValuationsReadyValue.Location = new Point(18, 40);
            lblValuationsReadyValue.Name = "lblValuationsReadyValue";
            lblValuationsReadyValue.Size = new Size(37, 45);
            lblValuationsReadyValue.TabIndex = 1;
            lblValuationsReadyValue.Text = "0";
            // 
            // lblValuationsReadyTitle
            // 
            lblValuationsReadyTitle.AutoSize = true;
            lblValuationsReadyTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblValuationsReadyTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblValuationsReadyTitle.Location = new Point(18, 16);
            lblValuationsReadyTitle.Name = "lblValuationsReadyTitle";
            lblValuationsReadyTitle.Size = new Size(111, 19);
            lblValuationsReadyTitle.TabIndex = 0;
            lblValuationsReadyTitle.Text = "Valuations Ready";
            // 
            // pnlNeedsReview
            // 
            pnlNeedsReview.BackColor = Color.White;
            pnlNeedsReview.Controls.Add(lblNeedsReviewValue);
            pnlNeedsReview.Controls.Add(lblNeedsReviewTitle);
            pnlNeedsReview.Dock = DockStyle.Fill;
            pnlNeedsReview.Location = new Point(942, 15);
            pnlNeedsReview.Name = "pnlNeedsReview";
            pnlNeedsReview.Padding = new Padding(18, 16, 18, 16);
            pnlNeedsReview.Size = new Size(311, 100);
            pnlNeedsReview.TabIndex = 3;
            // 
            // lblNeedsReviewValue
            // 
            lblNeedsReviewValue.AutoSize = true;
            lblNeedsReviewValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lblNeedsReviewValue.ForeColor = Color.Firebrick;
            lblNeedsReviewValue.Location = new Point(18, 40);
            lblNeedsReviewValue.Name = "lblNeedsReviewValue";
            lblNeedsReviewValue.Size = new Size(37, 45);
            lblNeedsReviewValue.TabIndex = 1;
            lblNeedsReviewValue.Text = "0";
            // 
            // lblNeedsReviewTitle
            // 
            lblNeedsReviewTitle.AutoSize = true;
            lblNeedsReviewTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblNeedsReviewTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblNeedsReviewTitle.Location = new Point(18, 16);
            lblNeedsReviewTitle.Name = "lblNeedsReviewTitle";
            lblNeedsReviewTitle.Size = new Size(95, 19);
            lblNeedsReviewTitle.TabIndex = 0;
            lblNeedsReviewTitle.Text = "Needs Review";
            // 
            // panelBody
            // 
            panelBody.BackColor = Color.FromArgb(245, 247, 250);
            panelBody.ColumnCount = 2;
            panelBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            panelBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            panelBody.Controls.Add(panelRecentActivity, 0, 0);
            panelBody.Controls.Add(panelWorkQueue, 1, 0);
            panelBody.Dock = DockStyle.Fill;
            panelBody.Location = new Point(0, 280);
            panelBody.Name = "panelBody";
            panelBody.Padding = new Padding(24, 8, 24, 8);
            panelBody.RowCount = 1;
            panelBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelBody.Size = new Size(1280, 388);
            panelBody.TabIndex = 3;
            // 
            // panelRecentActivity
            // 
            panelRecentActivity.BackColor = Color.White;
            panelRecentActivity.Controls.Add(lvRecentActivity);
            panelRecentActivity.Controls.Add(lblRecentActivity);
            panelRecentActivity.Dock = DockStyle.Fill;
            panelRecentActivity.Location = new Point(27, 11);
            panelRecentActivity.Name = "panelRecentActivity";
            panelRecentActivity.Padding = new Padding(18, 16, 18, 18);
            panelRecentActivity.Size = new Size(674, 366);
            panelRecentActivity.TabIndex = 0;
            // 
            // lvRecentActivity
            // 
            lvRecentActivity.BorderStyle = BorderStyle.None;
            lvRecentActivity.Columns.AddRange(new ColumnHeader[] { colActivityDate, colActivityType, colActivityVehicle, colActivityStatus });
            lvRecentActivity.Dock = DockStyle.Fill;
            lvRecentActivity.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lvRecentActivity.GridLines = true;
            lvRecentActivity.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvRecentActivity.Location = new Point(18, 45);
            lvRecentActivity.Name = "lvRecentActivity";
            lvRecentActivity.Size = new Size(638, 303);
            lvRecentActivity.TabIndex = 1;
            lvRecentActivity.UseCompatibleStateImageBehavior = false;
            lvRecentActivity.View = View.Details;
            // 
            // colActivityDate
            // 
            colActivityDate.Text = "Date/Time";
            colActivityDate.Width = 145;
            // 
            // colActivityType
            // 
            colActivityType.Text = "Activity";
            colActivityType.Width = 160;
            // 
            // colActivityVehicle
            // 
            colActivityVehicle.Text = "Vehicle";
            colActivityVehicle.Width = 225;
            // 
            // colActivityStatus
            // 
            colActivityStatus.Text = "Status";
            colActivityStatus.Width = 100;
            // 
            // lblRecentActivity
            // 
            lblRecentActivity.Dock = DockStyle.Top;
            lblRecentActivity.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblRecentActivity.ForeColor = Color.FromArgb(25, 35, 55);
            lblRecentActivity.Location = new Point(18, 16);
            lblRecentActivity.Name = "lblRecentActivity";
            lblRecentActivity.Size = new Size(638, 29);
            lblRecentActivity.TabIndex = 0;
            lblRecentActivity.Text = "Recent Activity";
            // 
            // panelWorkQueue
            // 
            panelWorkQueue.BackColor = Color.White;
            panelWorkQueue.Controls.Add(lvWorkQueue);
            panelWorkQueue.Controls.Add(lblWorkQueue);
            panelWorkQueue.Dock = DockStyle.Fill;
            panelWorkQueue.Location = new Point(707, 11);
            panelWorkQueue.Name = "panelWorkQueue";
            panelWorkQueue.Padding = new Padding(18, 16, 18, 18);
            panelWorkQueue.Size = new Size(546, 366);
            panelWorkQueue.TabIndex = 1;
            // 
            // lvWorkQueue
            // 
            lvWorkQueue.BorderStyle = BorderStyle.None;
            lvWorkQueue.Columns.AddRange(new ColumnHeader[] { colQueueId, colQueueVehicle, colQueueStage, colQueuePriority });
            lvWorkQueue.Dock = DockStyle.Fill;
            lvWorkQueue.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lvWorkQueue.GridLines = true;
            lvWorkQueue.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvWorkQueue.Location = new Point(18, 45);
            lvWorkQueue.Name = "lvWorkQueue";
            lvWorkQueue.Size = new Size(510, 303);
            lvWorkQueue.TabIndex = 1;
            lvWorkQueue.UseCompatibleStateImageBehavior = false;
            lvWorkQueue.View = View.Details;
            // 
            // colQueueId
            // 
            colQueueId.Text = "Appraisal #";
            colQueueId.Width = 105;
            // 
            // colQueueVehicle
            // 
            colQueueVehicle.Text = "Vehicle";
            colQueueVehicle.Width = 190;
            // 
            // colQueueStage
            // 
            colQueueStage.Text = "Stage";
            colQueueStage.Width = 130;
            // 
            // colQueuePriority
            // 
            colQueuePriority.Text = "Priority";
            colQueuePriority.Width = 80;
            // 
            // lblWorkQueue
            // 
            lblWorkQueue.Dock = DockStyle.Top;
            lblWorkQueue.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblWorkQueue.ForeColor = Color.FromArgb(25, 35, 55);
            lblWorkQueue.Location = new Point(18, 16);
            lblWorkQueue.Name = "lblWorkQueue";
            lblWorkQueue.Size = new Size(510, 29);
            lblWorkQueue.TabIndex = 0;
            lblWorkQueue.Text = "Work Queue";
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
            panelFooter.TabIndex = 4;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBar.Location = new Point(1060, 12);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(192, 23);
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.TabIndex = 1;
            progressBar.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.FromArgb(90, 90, 90);
            lblStatus.Location = new Point(24, 16);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(99, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Dashboard ready.";
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1280, 720);
            Controls.Add(panelBody);
            Controls.Add(panelFooter);
            Controls.Add(panelStats);
            Controls.Add(panelActions);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Name = "DashboardForm";
            Text = "DashboardForm";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelActions.ResumeLayout(false);
            panelStats.ResumeLayout(false);
            pnlActiveAppraisals.ResumeLayout(false);
            pnlActiveAppraisals.PerformLayout();
            pnlPendingInspections.ResumeLayout(false);
            pnlPendingInspections.PerformLayout();
            pnlValuationsReady.ResumeLayout(false);
            pnlValuationsReady.PerformLayout();
            pnlNeedsReview.ResumeLayout(false);
            pnlNeedsReview.PerformLayout();
            panelBody.ResumeLayout(false);
            panelRecentActivity.ResumeLayout(false);
            panelWorkQueue.ResumeLayout(false);
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblSubTitle;
        private Label lblWelcome;
        private Panel panelActions;
        private Button btnRefresh;
        private Button btnRunValuation;
        private Button btnNewInspection;
        private Button btnNewAppraisal;
        private TableLayoutPanel panelStats;
        private Panel pnlActiveAppraisals;
        private Label lblActiveAppraisalsValue;
        private Label lblActiveAppraisalsTitle;
        private Panel pnlPendingInspections;
        private Label lblPendingInspectionsValue;
        private Label lblPendingInspectionsTitle;
        private Panel pnlValuationsReady;
        private Label lblValuationsReadyValue;
        private Label lblValuationsReadyTitle;
        private Panel pnlNeedsReview;
        private Label lblNeedsReviewValue;
        private Label lblNeedsReviewTitle;
        private TableLayoutPanel panelBody;
        private Panel panelRecentActivity;
        private ListView lvRecentActivity;
        private ColumnHeader colActivityDate;
        private ColumnHeader colActivityType;
        private ColumnHeader colActivityVehicle;
        private ColumnHeader colActivityStatus;
        private Label lblRecentActivity;
        private Panel panelWorkQueue;
        private ListView lvWorkQueue;
        private ColumnHeader colQueueId;
        private ColumnHeader colQueueVehicle;
        private ColumnHeader colQueueStage;
        private ColumnHeader colQueuePriority;
        private Label lblWorkQueue;
        private Panel panelFooter;
        private ProgressBar progressBar;
        private Label lblStatus;
    }
}