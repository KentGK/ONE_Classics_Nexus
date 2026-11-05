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
            btnAppraisalRequests = new Button();
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
            panelHeader.Margin = new Padding(3, 4, 3, 4);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(27, 24, 27, 16);
            panelHeader.Size = new Size(1463, 120);
            panelHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.Font = new Font("Segoe UI", 10F);
            lblSubTitle.ForeColor = Color.FromArgb(90, 90, 90);
            lblSubTitle.Location = new Point(32, 67);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(409, 23);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Inspection-driven appraisal and valuation workspace";
            lblSubTitle.Click += lblSubTitle_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(25, 35, 55);
            lblWelcome.Location = new Point(27, 13);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(515, 46);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome to ONE Classics Nexus";
            // 
            // panelActions
            // 
            panelActions.BackColor = Color.FromArgb(245, 247, 250);
            panelActions.Controls.Add(btnRefresh);
            panelActions.Controls.Add(btnRunValuation);
            panelActions.Controls.Add(btnNewInspection);
            panelActions.Controls.Add(btnAppraisalRequests);
            panelActions.Controls.Add(btnNewAppraisal);
            panelActions.Dock = DockStyle.Top;
            panelActions.Location = new Point(0, 120);
            panelActions.Margin = new Padding(3, 4, 3, 4);
            panelActions.Name = "panelActions";
            panelActions.Padding = new Padding(27, 13, 27, 13);
            panelActions.Size = new Size(1463, 80);
            panelActions.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.Location = new Point(1294, 13);
            btnRefresh.Margin = new Padding(3, 4, 3, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(137, 48);
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
            btnRunValuation.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRunValuation.ForeColor = Color.White;
            btnRunValuation.Location = new Point(507, 13);
            btnRunValuation.Margin = new Padding(3, 4, 3, 4);
            btnRunValuation.Name = "btnRunValuation";
            btnRunValuation.Size = new Size(174, 48);
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
            btnNewInspection.Font = new Font("Segoe UI", 10F);
            btnNewInspection.Location = new Point(187, 13);
            btnNewInspection.Margin = new Padding(3, 4, 3, 4);
            btnNewInspection.Name = "btnNewInspection";
            btnNewInspection.Size = new Size(146, 48);
            btnNewInspection.TabIndex = 1;
            btnNewInspection.Text = "New Inspection";
            btnNewInspection.UseVisualStyleBackColor = false;
            btnNewInspection.Click += btnNewInspection_Click;
            // 
            // btnAppraisalRequests
            // 
            btnAppraisalRequests.BackColor = Color.White;
            btnAppraisalRequests.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
            btnAppraisalRequests.FlatStyle = FlatStyle.Flat;
            btnAppraisalRequests.Font = new Font("Segoe UI", 10F);
            btnAppraisalRequests.Location = new Point(347, 13);
            btnAppraisalRequests.Margin = new Padding(3, 4, 3, 4);
            btnAppraisalRequests.Name = "btnAppraisalRequests";
            btnAppraisalRequests.Size = new Size(146, 48);
            btnAppraisalRequests.TabIndex = 2;
            btnAppraisalRequests.Text = "New Request";
            btnAppraisalRequests.UseVisualStyleBackColor = false;
            btnAppraisalRequests.Click += btnAppraisalRequests_Click;
            // 
            // btnNewAppraisal
            // 
            btnNewAppraisal.BackColor = Color.White;
            btnNewAppraisal.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
            btnNewAppraisal.FlatStyle = FlatStyle.Flat;
            btnNewAppraisal.Font = new Font("Segoe UI", 10F);
            btnNewAppraisal.Location = new Point(27, 13);
            btnNewAppraisal.Margin = new Padding(3, 4, 3, 4);
            btnNewAppraisal.Name = "btnNewAppraisal";
            btnNewAppraisal.Size = new Size(146, 48);
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
            panelStats.Location = new Point(0, 200);
            panelStats.Margin = new Padding(3, 4, 3, 4);
            panelStats.Name = "panelStats";
            panelStats.Padding = new Padding(27, 16, 27, 16);
            panelStats.RowCount = 1;
            panelStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelStats.Size = new Size(1463, 173);
            panelStats.TabIndex = 2;
            // 
            // pnlActiveAppraisals
            // 
            pnlActiveAppraisals.BackColor = Color.White;
            pnlActiveAppraisals.Controls.Add(lblActiveAppraisalsValue);
            pnlActiveAppraisals.Controls.Add(lblActiveAppraisalsTitle);
            pnlActiveAppraisals.Dock = DockStyle.Fill;
            pnlActiveAppraisals.Location = new Point(30, 20);
            pnlActiveAppraisals.Margin = new Padding(3, 4, 3, 4);
            pnlActiveAppraisals.Name = "pnlActiveAppraisals";
            pnlActiveAppraisals.Padding = new Padding(21, 21, 21, 21);
            pnlActiveAppraisals.Size = new Size(346, 133);
            pnlActiveAppraisals.TabIndex = 0;
            // 
            // lblActiveAppraisalsValue
            // 
            lblActiveAppraisalsValue.AutoSize = true;
            lblActiveAppraisalsValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            lblActiveAppraisalsValue.ForeColor = Color.FromArgb(31, 78, 121);
            lblActiveAppraisalsValue.Location = new Point(21, 53);
            lblActiveAppraisalsValue.Name = "lblActiveAppraisalsValue";
            lblActiveAppraisalsValue.Size = new Size(45, 54);
            lblActiveAppraisalsValue.TabIndex = 1;
            lblActiveAppraisalsValue.Text = "0";
            // 
            // lblActiveAppraisalsTitle
            // 
            lblActiveAppraisalsTitle.AutoSize = true;
            lblActiveAppraisalsTitle.Font = new Font("Segoe UI", 10F);
            lblActiveAppraisalsTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblActiveAppraisalsTitle.Location = new Point(21, 21);
            lblActiveAppraisalsTitle.Name = "lblActiveAppraisalsTitle";
            lblActiveAppraisalsTitle.Size = new Size(138, 23);
            lblActiveAppraisalsTitle.TabIndex = 0;
            lblActiveAppraisalsTitle.Text = "Active Appraisals";
            // 
            // pnlPendingInspections
            // 
            pnlPendingInspections.BackColor = Color.White;
            pnlPendingInspections.Controls.Add(lblPendingInspectionsValue);
            pnlPendingInspections.Controls.Add(lblPendingInspectionsTitle);
            pnlPendingInspections.Dock = DockStyle.Fill;
            pnlPendingInspections.Location = new Point(382, 20);
            pnlPendingInspections.Margin = new Padding(3, 4, 3, 4);
            pnlPendingInspections.Name = "pnlPendingInspections";
            pnlPendingInspections.Padding = new Padding(21, 21, 21, 21);
            pnlPendingInspections.Size = new Size(346, 133);
            pnlPendingInspections.TabIndex = 1;
            // 
            // lblPendingInspectionsValue
            // 
            lblPendingInspectionsValue.AutoSize = true;
            lblPendingInspectionsValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            lblPendingInspectionsValue.ForeColor = Color.FromArgb(31, 78, 121);
            lblPendingInspectionsValue.Location = new Point(21, 53);
            lblPendingInspectionsValue.Name = "lblPendingInspectionsValue";
            lblPendingInspectionsValue.Size = new Size(45, 54);
            lblPendingInspectionsValue.TabIndex = 1;
            lblPendingInspectionsValue.Text = "0";
            // 
            // lblPendingInspectionsTitle
            // 
            lblPendingInspectionsTitle.AutoSize = true;
            lblPendingInspectionsTitle.Font = new Font("Segoe UI", 10F);
            lblPendingInspectionsTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblPendingInspectionsTitle.Location = new Point(21, 21);
            lblPendingInspectionsTitle.Name = "lblPendingInspectionsTitle";
            lblPendingInspectionsTitle.Size = new Size(163, 23);
            lblPendingInspectionsTitle.TabIndex = 0;
            lblPendingInspectionsTitle.Text = "Pending Inspections";
            // 
            // pnlValuationsReady
            // 
            pnlValuationsReady.BackColor = Color.White;
            pnlValuationsReady.Controls.Add(lblValuationsReadyValue);
            pnlValuationsReady.Controls.Add(lblValuationsReadyTitle);
            pnlValuationsReady.Dock = DockStyle.Fill;
            pnlValuationsReady.Location = new Point(734, 20);
            pnlValuationsReady.Margin = new Padding(3, 4, 3, 4);
            pnlValuationsReady.Name = "pnlValuationsReady";
            pnlValuationsReady.Padding = new Padding(21, 21, 21, 21);
            pnlValuationsReady.Size = new Size(346, 133);
            pnlValuationsReady.TabIndex = 2;
            // 
            // lblValuationsReadyValue
            // 
            lblValuationsReadyValue.AutoSize = true;
            lblValuationsReadyValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            lblValuationsReadyValue.ForeColor = Color.FromArgb(31, 78, 121);
            lblValuationsReadyValue.Location = new Point(21, 53);
            lblValuationsReadyValue.Name = "lblValuationsReadyValue";
            lblValuationsReadyValue.Size = new Size(45, 54);
            lblValuationsReadyValue.TabIndex = 1;
            lblValuationsReadyValue.Text = "0";
            // 
            // lblValuationsReadyTitle
            // 
            lblValuationsReadyTitle.AutoSize = true;
            lblValuationsReadyTitle.Font = new Font("Segoe UI", 10F);
            lblValuationsReadyTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblValuationsReadyTitle.Location = new Point(21, 21);
            lblValuationsReadyTitle.Name = "lblValuationsReadyTitle";
            lblValuationsReadyTitle.Size = new Size(140, 23);
            lblValuationsReadyTitle.TabIndex = 0;
            lblValuationsReadyTitle.Text = "Valuations Ready";
            // 
            // pnlNeedsReview
            // 
            pnlNeedsReview.BackColor = Color.White;
            pnlNeedsReview.Controls.Add(lblNeedsReviewValue);
            pnlNeedsReview.Controls.Add(lblNeedsReviewTitle);
            pnlNeedsReview.Dock = DockStyle.Fill;
            pnlNeedsReview.Location = new Point(1086, 20);
            pnlNeedsReview.Margin = new Padding(3, 4, 3, 4);
            pnlNeedsReview.Name = "pnlNeedsReview";
            pnlNeedsReview.Padding = new Padding(21, 21, 21, 21);
            pnlNeedsReview.Size = new Size(347, 133);
            pnlNeedsReview.TabIndex = 3;
            // 
            // lblNeedsReviewValue
            // 
            lblNeedsReviewValue.AutoSize = true;
            lblNeedsReviewValue.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            lblNeedsReviewValue.ForeColor = Color.Firebrick;
            lblNeedsReviewValue.Location = new Point(21, 53);
            lblNeedsReviewValue.Name = "lblNeedsReviewValue";
            lblNeedsReviewValue.Size = new Size(45, 54);
            lblNeedsReviewValue.TabIndex = 1;
            lblNeedsReviewValue.Text = "0";
            // 
            // lblNeedsReviewTitle
            // 
            lblNeedsReviewTitle.AutoSize = true;
            lblNeedsReviewTitle.Font = new Font("Segoe UI", 10F);
            lblNeedsReviewTitle.ForeColor = Color.FromArgb(95, 99, 104);
            lblNeedsReviewTitle.Location = new Point(21, 21);
            lblNeedsReviewTitle.Name = "lblNeedsReviewTitle";
            lblNeedsReviewTitle.Size = new Size(115, 23);
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
            panelBody.Location = new Point(0, 373);
            panelBody.Margin = new Padding(3, 4, 3, 4);
            panelBody.Name = "panelBody";
            panelBody.Padding = new Padding(27, 11, 27, 11);
            panelBody.RowCount = 1;
            panelBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelBody.Size = new Size(1463, 518);
            panelBody.TabIndex = 3;
            // 
            // panelRecentActivity
            // 
            panelRecentActivity.BackColor = Color.White;
            panelRecentActivity.Controls.Add(lvRecentActivity);
            panelRecentActivity.Controls.Add(lblRecentActivity);
            panelRecentActivity.Dock = DockStyle.Fill;
            panelRecentActivity.Location = new Point(30, 15);
            panelRecentActivity.Margin = new Padding(3, 4, 3, 4);
            panelRecentActivity.Name = "panelRecentActivity";
            panelRecentActivity.Padding = new Padding(21, 21, 21, 24);
            panelRecentActivity.Size = new Size(768, 488);
            panelRecentActivity.TabIndex = 0;
            // 
            // lvRecentActivity
            // 
            lvRecentActivity.BorderStyle = BorderStyle.None;
            lvRecentActivity.Columns.AddRange(new ColumnHeader[] { colActivityDate, colActivityType, colActivityVehicle, colActivityStatus });
            lvRecentActivity.Dock = DockStyle.Fill;
            lvRecentActivity.Font = new Font("Segoe UI", 10F);
            lvRecentActivity.GridLines = true;
            lvRecentActivity.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvRecentActivity.Location = new Point(21, 60);
            lvRecentActivity.Margin = new Padding(3, 4, 3, 4);
            lvRecentActivity.Name = "lvRecentActivity";
            lvRecentActivity.Size = new Size(726, 404);
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
            lblRecentActivity.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblRecentActivity.ForeColor = Color.FromArgb(25, 35, 55);
            lblRecentActivity.Location = new Point(21, 21);
            lblRecentActivity.Name = "lblRecentActivity";
            lblRecentActivity.Size = new Size(726, 39);
            lblRecentActivity.TabIndex = 0;
            lblRecentActivity.Text = "Recent Activity";
            // 
            // panelWorkQueue
            // 
            panelWorkQueue.BackColor = Color.White;
            panelWorkQueue.Controls.Add(lvWorkQueue);
            panelWorkQueue.Controls.Add(lblWorkQueue);
            panelWorkQueue.Dock = DockStyle.Fill;
            panelWorkQueue.Location = new Point(804, 15);
            panelWorkQueue.Margin = new Padding(3, 4, 3, 4);
            panelWorkQueue.Name = "panelWorkQueue";
            panelWorkQueue.Padding = new Padding(21, 21, 21, 24);
            panelWorkQueue.Size = new Size(629, 488);
            panelWorkQueue.TabIndex = 1;
            // 
            // lvWorkQueue
            // 
            lvWorkQueue.BorderStyle = BorderStyle.None;
            lvWorkQueue.Columns.AddRange(new ColumnHeader[] { colQueueId, colQueueVehicle, colQueueStage, colQueuePriority });
            lvWorkQueue.Dock = DockStyle.Fill;
            lvWorkQueue.Font = new Font("Segoe UI", 10F);
            lvWorkQueue.GridLines = true;
            lvWorkQueue.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvWorkQueue.Location = new Point(21, 60);
            lvWorkQueue.Margin = new Padding(3, 4, 3, 4);
            lvWorkQueue.Name = "lvWorkQueue";
            lvWorkQueue.Size = new Size(587, 404);
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
            lblWorkQueue.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblWorkQueue.ForeColor = Color.FromArgb(25, 35, 55);
            lblWorkQueue.Location = new Point(21, 21);
            lblWorkQueue.Name = "lblWorkQueue";
            lblWorkQueue.Size = new Size(587, 39);
            lblWorkQueue.TabIndex = 0;
            lblWorkQueue.Text = "Work Queue";
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
            panelFooter.TabIndex = 4;
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
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.FromArgb(90, 90, 90);
            lblStatus.Location = new Point(27, 21);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(126, 20);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Dashboard ready.";
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1463, 960);
            Controls.Add(panelBody);
            Controls.Add(panelFooter);
            Controls.Add(panelStats);
            Controls.Add(panelActions);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "DashboardForm";
            Text = "Dashboard Form";
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
        private Button btnAppraisalRequests;
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