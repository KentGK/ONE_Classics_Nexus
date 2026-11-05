using System.Drawing;
using System.Windows.Forms;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

partial class AppraisalRequestListForm
{
    private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
    private TableLayoutPanel rootLayout = null!;
    private TableLayoutPanel filterLayout = null!;
    private Button btnNewRequest = null!;
    private Button btnRefresh = null!;
    private TextBox txtSearch = null!;
    private ComboBox cboStatus = null!;
    private ComboBox cboClient = null!;
    private ComboBox cboDateRange = null!;
    private DateTimePicker dtpFrom = null!;
    private DateTimePicker dtpTo = null!;
    private ComboBox cboSort = null!;
    private Label lblRequestCount = null!;
    private DataGridView gridRequests = null!;
    private FlowLayoutPanel pagerLayout = null!;
    private Button btnPrevPage = null!;
    private Button btnPageOne = null!;
    private Button btnPageTwo = null!;
    private Button btnNextPage = null!;
    private Label lblPaging = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        filterLayout = new TableLayoutPanel();
        btnNewRequest = new Button();
        btnRefresh = new Button();
        txtSearch = new TextBox();
        cboStatus = new ComboBox();
        cboClient = new ComboBox();
        cboDateRange = new ComboBox();
        dtpFrom = new DateTimePicker();
        dtpTo = new DateTimePicker();
        cboSort = new ComboBox();
        lblRequestCount = new Label();
        gridRequests = new DataGridView();
        pagerLayout = new FlowLayoutPanel();
        btnPrevPage = new Button();
        btnPageOne = new Button();
        btnPageTwo = new Button();
        btnNextPage = new Button();
        lblPaging = new Label();
        rootLayout.SuspendLayout();
        filterLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridRequests).BeginInit();
        pagerLayout.SuspendLayout();
        SuspendLayout();
        // 
        // rootLayout
        // 
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(filterLayout, 0, 0);
        rootLayout.Controls.Add(gridRequests, 0, 1);
        rootLayout.Controls.Add(pagerLayout, 0, 2);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(16, 12, 16, 12);
        rootLayout.RowCount = 3;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 214F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        rootLayout.Size = new Size(1280, 760);
        rootLayout.TabIndex = 0;
        // 
        // filterLayout
        // 
        filterLayout.ColumnCount = 4;
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        filterLayout.Controls.Add(btnNewRequest, 0, 0);
        filterLayout.Controls.Add(btnRefresh, 3, 0);
        filterLayout.Controls.Add(txtSearch, 0, 1);
        filterLayout.Controls.Add(cboStatus, 0, 2);
        filterLayout.Controls.Add(cboClient, 1, 2);
        filterLayout.Controls.Add(cboDateRange, 0, 3);
        filterLayout.Controls.Add(dtpFrom, 1, 3);
        filterLayout.Controls.Add(dtpTo, 2, 3);
        filterLayout.Controls.Add(cboSort, 3, 3);
        filterLayout.Controls.Add(lblRequestCount, 0, 4);
        filterLayout.Dock = DockStyle.Fill;
        filterLayout.Location = new Point(19, 15);
        filterLayout.Name = "filterLayout";
        filterLayout.RowCount = 5;
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        filterLayout.Size = new Size(1242, 208);
        filterLayout.TabIndex = 0;
        // 
        // btnNewRequest
        // 
        btnNewRequest.Dock = DockStyle.Left;
        btnNewRequest.Location = new Point(0, 0);
        btnNewRequest.Margin = new Padding(0, 0, 8, 8);
        btnNewRequest.Name = "btnNewRequest";
        btnNewRequest.Size = new Size(156, 36);
        btnNewRequest.TabIndex = 0;
        btnNewRequest.Text = "+  New Request";
        btnNewRequest.UseVisualStyleBackColor = false;
        // 
        // btnRefresh
        // 
        btnRefresh.Dock = DockStyle.Right;
        btnRefresh.Location = new Point(1130, 0);
        btnRefresh.Margin = new Padding(8, 0, 0, 8);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(112, 36);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // txtSearch
        // 
        filterLayout.SetColumnSpan(txtSearch, 4);
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Location = new Point(0, 44);
        txtSearch.Margin = new Padding(0, 0, 0, 8);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search requests, client, or vehicle...";
        txtSearch.Size = new Size(1242, 23);
        txtSearch.TabIndex = 2;
        // 
        // cboStatus
        // 
        cboStatus.Dock = DockStyle.Fill;
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboStatus.Location = new Point(0, 86);
        cboStatus.Margin = new Padding(0, 0, 8, 8);
        cboStatus.Name = "cboStatus";
        cboStatus.Size = new Size(302, 23);
        cboStatus.TabIndex = 3;
        // 
        // cboClient
        // 
        cboClient.Dock = DockStyle.Fill;
        cboClient.DropDownStyle = ComboBoxStyle.DropDownList;
        cboClient.Location = new Point(310, 86);
        cboClient.Margin = new Padding(0, 0, 8, 8);
        cboClient.Name = "cboClient";
        cboClient.Size = new Size(302, 23);
        cboClient.TabIndex = 4;
        // 
        // cboDateRange
        // 
        cboDateRange.Dock = DockStyle.Fill;
        cboDateRange.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDateRange.Location = new Point(0, 128);
        cboDateRange.Margin = new Padding(0, 0, 8, 8);
        cboDateRange.Name = "cboDateRange";
        cboDateRange.Size = new Size(302, 23);
        cboDateRange.TabIndex = 5;
        // 
        // dtpFrom
        // 
        dtpFrom.Dock = DockStyle.Fill;
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(310, 128);
        dtpFrom.Margin = new Padding(0, 0, 8, 8);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(302, 23);
        dtpFrom.TabIndex = 6;
        // 
        // dtpTo
        // 
        dtpTo.Dock = DockStyle.Fill;
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(620, 128);
        dtpTo.Margin = new Padding(0, 0, 8, 8);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(302, 23);
        dtpTo.TabIndex = 7;
        // 
        // cboSort
        // 
        cboSort.Dock = DockStyle.Fill;
        cboSort.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSort.Location = new Point(930, 128);
        cboSort.Margin = new Padding(0, 0, 0, 8);
        cboSort.Name = "cboSort";
        cboSort.Size = new Size(312, 23);
        cboSort.TabIndex = 8;
        // 
        // lblRequestCount
        // 
        lblRequestCount.AutoSize = true;
        filterLayout.SetColumnSpan(lblRequestCount, 2);
        lblRequestCount.Dock = DockStyle.Left;
        lblRequestCount.Location = new Point(3, 170);
        lblRequestCount.Name = "lblRequestCount";
        lblRequestCount.Size = new Size(95, 38);
        lblRequestCount.TabIndex = 9;
        lblRequestCount.Text = "0 requests found";
        lblRequestCount.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // gridRequests
        // 
        gridRequests.AllowUserToAddRows = false;
        gridRequests.AllowUserToDeleteRows = false;
        gridRequests.AllowUserToResizeRows = false;
        gridRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridRequests.BackgroundColor = Color.White;
        gridRequests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        gridRequests.Dock = DockStyle.Fill;
        gridRequests.Location = new Point(19, 229);
        gridRequests.MultiSelect = false;
        gridRequests.Name = "gridRequests";
        gridRequests.ReadOnly = true;
        gridRequests.RowHeadersVisible = false;
        gridRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridRequests.Size = new Size(1242, 464);
        gridRequests.TabIndex = 1;
        gridRequests.CellContentClick += gridRequests_CellContentClick;
        // 
        // pagerLayout
        // 
        pagerLayout.Controls.Add(btnPrevPage);
        pagerLayout.Controls.Add(btnPageOne);
        pagerLayout.Controls.Add(btnPageTwo);
        pagerLayout.Controls.Add(btnNextPage);
        pagerLayout.Controls.Add(lblPaging);
        pagerLayout.Dock = DockStyle.Fill;
        pagerLayout.Location = new Point(19, 699);
        pagerLayout.Name = "pagerLayout";
        pagerLayout.Padding = new Padding(0, 6, 0, 0);
        pagerLayout.Size = new Size(1242, 46);
        pagerLayout.TabIndex = 2;
        pagerLayout.WrapContents = false;
        // 
        // btnPrevPage
        // 
        btnPrevPage.Enabled = false;
        btnPrevPage.Location = new Point(3, 9);
        btnPrevPage.Name = "btnPrevPage";
        btnPrevPage.Size = new Size(34, 30);
        btnPrevPage.TabIndex = 0;
        btnPrevPage.Text = "<";
        // 
        // btnPageOne
        // 
        btnPageOne.Location = new Point(43, 9);
        btnPageOne.Name = "btnPageOne";
        btnPageOne.Size = new Size(34, 30);
        btnPageOne.TabIndex = 1;
        btnPageOne.Text = "1";
        // 
        // btnPageTwo
        // 
        btnPageTwo.Location = new Point(83, 9);
        btnPageTwo.Name = "btnPageTwo";
        btnPageTwo.Size = new Size(34, 30);
        btnPageTwo.TabIndex = 2;
        btnPageTwo.Text = "2";
        // 
        // btnNextPage
        // 
        btnNextPage.Location = new Point(123, 9);
        btnNextPage.Name = "btnNextPage";
        btnNextPage.Size = new Size(34, 30);
        btnNextPage.TabIndex = 3;
        btnNextPage.Text = ">";
        // 
        // lblPaging
        // 
        lblPaging.AutoSize = true;
        lblPaging.Location = new Point(178, 13);
        lblPaging.Margin = new Padding(18, 7, 0, 0);
        lblPaging.Name = "lblPaging";
        lblPaging.Size = new Size(102, 15);
        lblPaging.TabIndex = 4;
        lblPaging.Text = "Showing 1 - 0 of 0";
        // 
        // AppraisalRequestListForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1280, 760);
        Controls.Add(rootLayout);
        MinimumSize = new Size(980, 600);
        Name = "AppraisalRequestListForm";
        Text = "Appraisal Requests";
        rootLayout.ResumeLayout(false);
        filterLayout.ResumeLayout(false);
        filterLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)gridRequests).EndInit();
        pagerLayout.ResumeLayout(false);
        pagerLayout.PerformLayout();
        ResumeLayout(false);
    }
}
