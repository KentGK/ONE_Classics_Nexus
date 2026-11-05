// ============================================================
// ClientWorkspaceForm.Designer.cs
// Individual Client Workspace - see
// docs/07_UI_UX/Mockups/Client/ONE-Classics-Nexus-Individual-Client-Workspace.md
// ============================================================

namespace ONE_Classics_Nexus.WinForms.Forms.Clients;

partial class ClientWorkspaceForm
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
        splitWorkspace = new SplitContainer();
        tlpBrowser = new TableLayoutPanel();
        lblClientsHeading = new Label();
        tlpSearch = new TableLayoutPanel();
        txtSearch = new TextBox();
        btnClearSearch = new Button();
        tlpBrowserFilters = new TableLayoutPanel();
        lblClientTypeFilterCaption = new Label();
        lblStatusFilterCaption = new Label();
        cmbClientTypeFilter = new ComboBox();
        cmbStatusFilter = new ComboBox();
        btnCreateClient = new Button();
        gridClients = new DataGridView();
        lblNoResults = new Label();
        tlpPagination = new TableLayoutPanel();
        flpPages = new FlowLayoutPanel();
        btnPreviousPage = new Button();
        btnPage1 = new Button();
        btnPage2 = new Button();
        btnPage3 = new Button();
        btnPage4 = new Button();
        btnPage5 = new Button();
        btnNextPage = new Button();
        lblPageRange = new Label();
        tlpDetail = new TableLayoutPanel();
        tlpDetailHeader = new TableLayoutPanel();
        lblClientHeading = new Label();
        lblClientStatusBadge = new Label();
        lblClientSummary = new Label();
        lblLastUpdated = new Label();
        tabClient = new TabControl();
        tabProfile = new TabPage();
        pnlProfileScroll = new Panel();
        tlpProfile = new TableLayoutPanel();
        lblClientTypeCaption = new Label();
        tlpClientType = new TableLayoutPanel();
        cmbClientTypeValue = new ComboBox();
        lblClientTypeLock = new Label();
        lblSectionClientInformation = new Label();
        sepClientInformation = new Panel();
        lblFirstNameCaption = new Label();
        txtFirstName = new TextBox();
        lblMiddleNameCaption = new Label();
        txtMiddleName = new TextBox();
        lblLastNameCaption = new Label();
        txtLastName = new TextBox();
        lblSuffixCaption = new Label();
        cmbSuffix = new ComboBox();
        lblDisplayNameCaption = new Label();
        txtDisplayName = new TextBox();
        lblDisplayNameHelper = new Label();
        lblSectionContactInformation = new Label();
        sepContactInformation = new Panel();
        lblEmailCaption = new Label();
        txtEmail = new TextBox();
        lblPhoneCaption = new Label();
        txtPhone = new TextBox();
        lblSectionMailingAddress = new Label();
        sepMailingAddress = new Panel();
        lblAddressLine1Caption = new Label();
        txtAddressLine1 = new TextBox();
        lblAddressLine2Caption = new Label();
        txtAddressLine2 = new TextBox();
        lblCityCaption = new Label();
        tlpCityStateZip = new TableLayoutPanel();
        txtCity = new TextBox();
        lblStateCaption = new Label();
        cmbState = new ComboBox();
        lblZipCodeCaption = new Label();
        txtZipCode = new TextBox();
        lblSectionCommunication = new Label();
        sepCommunication = new Panel();
        lblPreferredMethodCaption = new Label();
        cmbPreferredMethod = new ComboBox();
        lblClientTypeRule = new Label();
        lblConsentNote = new Label();
        lblEmailAvailability = new Label();
        tabContacts = new TabPage();
        tabHistory = new TabPage();
        pnlDetailFooter = new Panel();
        flpFooterActions = new FlowLayoutPanel();
        btnSave = new Button();
        btnCancel = new Button();
        errorProvider = new ErrorProvider(components);
        toolTip = new ToolTip(components);
        ((System.ComponentModel.ISupportInitialize)splitWorkspace).BeginInit();
        splitWorkspace.Panel1.SuspendLayout();
        splitWorkspace.Panel2.SuspendLayout();
        splitWorkspace.SuspendLayout();
        tlpBrowser.SuspendLayout();
        tlpSearch.SuspendLayout();
        tlpBrowserFilters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridClients).BeginInit();
        tlpPagination.SuspendLayout();
        flpPages.SuspendLayout();
        tlpDetail.SuspendLayout();
        tlpDetailHeader.SuspendLayout();
        tabClient.SuspendLayout();
        tabProfile.SuspendLayout();
        pnlProfileScroll.SuspendLayout();
        tlpProfile.SuspendLayout();
        tlpClientType.SuspendLayout();
        tlpCityStateZip.SuspendLayout();
        pnlDetailFooter.SuspendLayout();
        flpFooterActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // splitWorkspace
        // 
        splitWorkspace.Dock = DockStyle.Fill;
        splitWorkspace.FixedPanel = FixedPanel.Panel1;
        splitWorkspace.Location = new Point(0, 0);
        splitWorkspace.Name = "splitWorkspace";
        // 
        // splitWorkspace.Panel1
        // 
        splitWorkspace.Panel1.Controls.Add(tlpBrowser);
        splitWorkspace.Panel1MinSize = 360;
        // 
        // splitWorkspace.Panel2
        // 
        splitWorkspace.Panel2.Controls.Add(tlpDetail);
        splitWorkspace.Panel2MinSize = 520;
        splitWorkspace.Size = new Size(1360, 740);
        splitWorkspace.SplitterDistance = 561;
        splitWorkspace.SplitterWidth = 6;
        splitWorkspace.TabIndex = 0;
        // 
        // tlpBrowser
        // 
        tlpBrowser.ColumnCount = 1;
        tlpBrowser.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpBrowser.Controls.Add(lblClientsHeading, 0, 0);
        tlpBrowser.Controls.Add(tlpSearch, 0, 1);
        tlpBrowser.Controls.Add(tlpBrowserFilters, 0, 2);
        tlpBrowser.Controls.Add(btnCreateClient, 0, 3);
        tlpBrowser.Controls.Add(gridClients, 0, 4);
        tlpBrowser.Controls.Add(lblNoResults, 0, 5);
        tlpBrowser.Controls.Add(tlpPagination, 0, 6);
        tlpBrowser.Dock = DockStyle.Fill;
        tlpBrowser.Location = new Point(0, 0);
        tlpBrowser.Name = "tlpBrowser";
        tlpBrowser.Padding = new Padding(16);
        tlpBrowser.RowCount = 7;
        tlpBrowser.RowStyles.Add(new RowStyle());
        tlpBrowser.RowStyles.Add(new RowStyle());
        tlpBrowser.RowStyles.Add(new RowStyle());
        tlpBrowser.RowStyles.Add(new RowStyle());
        tlpBrowser.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpBrowser.RowStyles.Add(new RowStyle());
        tlpBrowser.RowStyles.Add(new RowStyle());
        tlpBrowser.Size = new Size(561, 740);
        tlpBrowser.TabIndex = 0;
        // 
        // lblClientsHeading
        // 
        lblClientsHeading.AutoSize = true;
        lblClientsHeading.Dock = DockStyle.Fill;
        lblClientsHeading.Location = new Point(16, 16);
        lblClientsHeading.Margin = new Padding(0, 0, 0, 10);
        lblClientsHeading.Name = "lblClientsHeading";
        lblClientsHeading.Size = new Size(529, 15);
        lblClientsHeading.TabIndex = 0;
        lblClientsHeading.Text = "Clients";
        // 
        // tlpSearch
        // 
        tlpSearch.AutoSize = true;
        tlpSearch.ColumnCount = 2;
        tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpSearch.ColumnStyles.Add(new ColumnStyle());
        tlpSearch.Controls.Add(txtSearch, 0, 0);
        tlpSearch.Controls.Add(btnClearSearch, 1, 0);
        tlpSearch.Dock = DockStyle.Fill;
        tlpSearch.Location = new Point(16, 41);
        tlpSearch.Margin = new Padding(0, 0, 0, 10);
        tlpSearch.Name = "tlpSearch";
        tlpSearch.RowCount = 1;
        tlpSearch.RowStyles.Add(new RowStyle());
        tlpSearch.Size = new Size(529, 29);
        tlpSearch.TabIndex = 1;
        // 
        // txtSearch
        // 
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Location = new Point(0, 0);
        txtSearch.Margin = new Padding(0, 0, 4, 0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search clients by name or email...";
        txtSearch.Size = new Size(497, 23);
        txtSearch.TabIndex = 0;
        // 
        // btnClearSearch
        // 
        btnClearSearch.Dock = DockStyle.Fill;
        btnClearSearch.FlatStyle = FlatStyle.Flat;
        btnClearSearch.Location = new Point(501, 0);
        btnClearSearch.Margin = new Padding(0);
        btnClearSearch.Name = "btnClearSearch";
        btnClearSearch.Size = new Size(28, 29);
        btnClearSearch.TabIndex = 1;
        btnClearSearch.Text = "X";
        btnClearSearch.UseVisualStyleBackColor = true;
        btnClearSearch.Visible = false;
        btnClearSearch.Click += BtnClearSearch_Click;
        // 
        // tlpBrowserFilters
        // 
        tlpBrowserFilters.AutoSize = true;
        tlpBrowserFilters.ColumnCount = 2;
        tlpBrowserFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpBrowserFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpBrowserFilters.Controls.Add(lblClientTypeFilterCaption, 0, 0);
        tlpBrowserFilters.Controls.Add(lblStatusFilterCaption, 1, 0);
        tlpBrowserFilters.Controls.Add(cmbClientTypeFilter, 0, 1);
        tlpBrowserFilters.Controls.Add(cmbStatusFilter, 1, 1);
        tlpBrowserFilters.Dock = DockStyle.Fill;
        tlpBrowserFilters.Location = new Point(16, 80);
        tlpBrowserFilters.Margin = new Padding(0, 0, 0, 10);
        tlpBrowserFilters.Name = "tlpBrowserFilters";
        tlpBrowserFilters.RowCount = 2;
        tlpBrowserFilters.RowStyles.Add(new RowStyle());
        tlpBrowserFilters.RowStyles.Add(new RowStyle());
        tlpBrowserFilters.Size = new Size(529, 40);
        tlpBrowserFilters.TabIndex = 1;
        // 
        // lblClientTypeFilterCaption
        // 
        lblClientTypeFilterCaption.AutoSize = true;
        lblClientTypeFilterCaption.Dock = DockStyle.Fill;
        lblClientTypeFilterCaption.Location = new Point(0, 0);
        lblClientTypeFilterCaption.Margin = new Padding(0, 0, 8, 2);
        lblClientTypeFilterCaption.Name = "lblClientTypeFilterCaption";
        lblClientTypeFilterCaption.Size = new Size(256, 15);
        lblClientTypeFilterCaption.TabIndex = 0;
        lblClientTypeFilterCaption.Text = "Client Type";
        // 
        // lblStatusFilterCaption
        // 
        lblStatusFilterCaption.AutoSize = true;
        lblStatusFilterCaption.Dock = DockStyle.Fill;
        lblStatusFilterCaption.Location = new Point(264, 0);
        lblStatusFilterCaption.Margin = new Padding(0, 0, 0, 2);
        lblStatusFilterCaption.Name = "lblStatusFilterCaption";
        lblStatusFilterCaption.Size = new Size(265, 15);
        lblStatusFilterCaption.TabIndex = 2;
        lblStatusFilterCaption.Text = "Status";
        // 
        // cmbClientTypeFilter
        // 
        cmbClientTypeFilter.Dock = DockStyle.Fill;
        cmbClientTypeFilter.Location = new Point(0, 17);
        cmbClientTypeFilter.Margin = new Padding(0, 0, 8, 0);
        cmbClientTypeFilter.Name = "cmbClientTypeFilter";
        cmbClientTypeFilter.Size = new Size(256, 23);
        cmbClientTypeFilter.TabIndex = 1;
        // 
        // cmbStatusFilter
        // 
        cmbStatusFilter.Dock = DockStyle.Fill;
        cmbStatusFilter.Location = new Point(264, 17);
        cmbStatusFilter.Margin = new Padding(0);
        cmbStatusFilter.Name = "cmbStatusFilter";
        cmbStatusFilter.Size = new Size(265, 23);
        cmbStatusFilter.TabIndex = 3;
        // 
        // btnCreateClient
        // 
        btnCreateClient.Dock = DockStyle.Fill;
        btnCreateClient.Location = new Point(16, 130);
        btnCreateClient.Margin = new Padding(0, 0, 0, 12);
        btnCreateClient.Name = "btnCreateClient";
        btnCreateClient.Size = new Size(529, 40);
        btnCreateClient.TabIndex = 2;
        btnCreateClient.Text = "+  Create Client";
        btnCreateClient.UseVisualStyleBackColor = true;
        btnCreateClient.Click += BtnCreateClient_Click;
        // 
        // gridClients
        // 
        gridClients.Dock = DockStyle.Fill;
        gridClients.Location = new Point(16, 182);
        gridClients.Margin = new Padding(0, 0, 0, 10);
        gridClients.Name = "gridClients";
        gridClients.Size = new Size(529, 471);
        gridClients.TabIndex = 3;
        gridClients.SelectionChanged += GridClients_SelectionChanged;
        // 
        // lblNoResults
        // 
        lblNoResults.AutoSize = true;
        lblNoResults.Dock = DockStyle.Fill;
        lblNoResults.Location = new Point(16, 671);
        lblNoResults.Margin = new Padding(0, 8, 0, 8);
        lblNoResults.Name = "lblNoResults";
        lblNoResults.Size = new Size(529, 15);
        lblNoResults.TabIndex = 5;
        lblNoResults.Text = "No clients match your search criteria.";
        lblNoResults.TextAlign = ContentAlignment.MiddleCenter;
        lblNoResults.Visible = false;
        // 
        // tlpPagination
        // 
        tlpPagination.AutoSize = true;
        tlpPagination.ColumnCount = 2;
        tlpPagination.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPagination.ColumnStyles.Add(new ColumnStyle());
        tlpPagination.Controls.Add(flpPages, 0, 0);
        tlpPagination.Controls.Add(lblPageRange, 1, 0);
        tlpPagination.Dock = DockStyle.Fill;
        tlpPagination.Location = new Point(16, 694);
        tlpPagination.Margin = new Padding(0);
        tlpPagination.Name = "tlpPagination";
        tlpPagination.RowCount = 1;
        tlpPagination.RowStyles.Add(new RowStyle());
        tlpPagination.Size = new Size(529, 30);
        tlpPagination.TabIndex = 4;
        // 
        // flpPages
        // 
        flpPages.AutoSize = true;
        flpPages.Controls.Add(btnPreviousPage);
        flpPages.Controls.Add(btnPage1);
        flpPages.Controls.Add(btnPage2);
        flpPages.Controls.Add(btnPage3);
        flpPages.Controls.Add(btnPage4);
        flpPages.Controls.Add(btnPage5);
        flpPages.Controls.Add(btnNextPage);
        flpPages.Dock = DockStyle.Fill;
        flpPages.Location = new Point(0, 0);
        flpPages.Margin = new Padding(0);
        flpPages.Name = "flpPages";
        flpPages.Size = new Size(474, 30);
        flpPages.TabIndex = 0;
        flpPages.WrapContents = false;
        // 
        // btnPreviousPage
        // 
        btnPreviousPage.Location = new Point(0, 0);
        btnPreviousPage.Margin = new Padding(0, 0, 4, 0);
        btnPreviousPage.Name = "btnPreviousPage";
        btnPreviousPage.Size = new Size(34, 30);
        btnPreviousPage.TabIndex = 0;
        btnPreviousPage.Text = "<";
        btnPreviousPage.UseVisualStyleBackColor = true;
        btnPreviousPage.Click += BtnPreviousPage_Click;
        // 
        // btnPage1
        // 
        btnPage1.Location = new Point(38, 0);
        btnPage1.Margin = new Padding(0, 0, 4, 0);
        btnPage1.Name = "btnPage1";
        btnPage1.Size = new Size(34, 30);
        btnPage1.TabIndex = 1;
        btnPage1.Tag = "1";
        btnPage1.Text = "1";
        btnPage1.UseVisualStyleBackColor = true;
        btnPage1.Click += PageNumber_Click;
        // 
        // btnPage2
        // 
        btnPage2.Location = new Point(76, 0);
        btnPage2.Margin = new Padding(0, 0, 4, 0);
        btnPage2.Name = "btnPage2";
        btnPage2.Size = new Size(34, 30);
        btnPage2.TabIndex = 2;
        btnPage2.Tag = "2";
        btnPage2.Text = "2";
        btnPage2.UseVisualStyleBackColor = true;
        btnPage2.Click += PageNumber_Click;
        // 
        // btnPage3
        // 
        btnPage3.Location = new Point(114, 0);
        btnPage3.Margin = new Padding(0, 0, 4, 0);
        btnPage3.Name = "btnPage3";
        btnPage3.Size = new Size(34, 30);
        btnPage3.TabIndex = 3;
        btnPage3.Tag = "3";
        btnPage3.Text = "3";
        btnPage3.UseVisualStyleBackColor = true;
        btnPage3.Click += PageNumber_Click;
        // 
        // btnPage4
        // 
        btnPage4.Location = new Point(152, 0);
        btnPage4.Margin = new Padding(0, 0, 4, 0);
        btnPage4.Name = "btnPage4";
        btnPage4.Size = new Size(34, 30);
        btnPage4.TabIndex = 4;
        btnPage4.Tag = "4";
        btnPage4.Text = "4";
        btnPage4.UseVisualStyleBackColor = true;
        btnPage4.Click += PageNumber_Click;
        // 
        // btnPage5
        // 
        btnPage5.Location = new Point(190, 0);
        btnPage5.Margin = new Padding(0, 0, 4, 0);
        btnPage5.Name = "btnPage5";
        btnPage5.Size = new Size(34, 30);
        btnPage5.TabIndex = 5;
        btnPage5.Tag = "5";
        btnPage5.Text = "5";
        btnPage5.UseVisualStyleBackColor = true;
        btnPage5.Click += PageNumber_Click;
        // 
        // btnNextPage
        // 
        btnNextPage.Location = new Point(228, 0);
        btnNextPage.Margin = new Padding(0);
        btnNextPage.Name = "btnNextPage";
        btnNextPage.Size = new Size(34, 30);
        btnNextPage.TabIndex = 6;
        btnNextPage.Text = ">";
        btnNextPage.UseVisualStyleBackColor = true;
        btnNextPage.Click += BtnNextPage_Click;
        // 
        // lblPageRange
        // 
        lblPageRange.Anchor = AnchorStyles.Right;
        lblPageRange.AutoSize = true;
        lblPageRange.Location = new Point(482, 7);
        lblPageRange.Margin = new Padding(8, 0, 0, 0);
        lblPageRange.Name = "lblPageRange";
        lblPageRange.Size = new Size(47, 15);
        lblPageRange.TabIndex = 1;
        lblPageRange.Text = "0-0 of 0";
        lblPageRange.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tlpDetail
        // 
        tlpDetail.ColumnCount = 1;
        tlpDetail.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpDetail.Controls.Add(tlpDetailHeader, 0, 0);
        tlpDetail.Controls.Add(tabClient, 0, 1);
        tlpDetail.Controls.Add(pnlDetailFooter, 0, 2);
        tlpDetail.Dock = DockStyle.Fill;
        tlpDetail.Location = new Point(0, 0);
        tlpDetail.Name = "tlpDetail";
        tlpDetail.Padding = new Padding(20);
        tlpDetail.RowCount = 3;
        tlpDetail.RowStyles.Add(new RowStyle());
        tlpDetail.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpDetail.RowStyles.Add(new RowStyle());
        tlpDetail.Size = new Size(793, 740);
        tlpDetail.TabIndex = 0;
        // 
        // tlpDetailHeader
        // 
        tlpDetailHeader.AutoSize = true;
        tlpDetailHeader.ColumnCount = 2;
        tlpDetailHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpDetailHeader.ColumnStyles.Add(new ColumnStyle());
        tlpDetailHeader.Controls.Add(lblClientHeading, 0, 0);
        tlpDetailHeader.Controls.Add(lblClientStatusBadge, 1, 0);
        tlpDetailHeader.Controls.Add(lblClientSummary, 0, 1);
        tlpDetailHeader.Controls.Add(lblLastUpdated, 1, 1);
        tlpDetailHeader.Dock = DockStyle.Fill;
        tlpDetailHeader.Location = new Point(20, 20);
        tlpDetailHeader.Margin = new Padding(0, 0, 0, 12);
        tlpDetailHeader.Name = "tlpDetailHeader";
        tlpDetailHeader.RowCount = 2;
        tlpDetailHeader.RowStyles.Add(new RowStyle());
        tlpDetailHeader.RowStyles.Add(new RowStyle());
        tlpDetailHeader.Size = new Size(753, 50);
        tlpDetailHeader.TabIndex = 0;
        // 
        // lblClientHeading
        // 
        lblClientHeading.AutoSize = true;
        lblClientHeading.Location = new Point(0, 0);
        lblClientHeading.Margin = new Padding(0, 0, 0, 4);
        lblClientHeading.Name = "lblClientHeading";
        lblClientHeading.Size = new Size(101, 15);
        lblClientHeading.TabIndex = 0;
        lblClientHeading.Text = "No client selected";
        // 
        // lblClientStatusBadge
        // 
        lblClientStatusBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblClientStatusBadge.AutoSize = true;
        lblClientStatusBadge.Location = new Point(717, 6);
        lblClientStatusBadge.Margin = new Padding(0, 6, 0, 4);
        lblClientStatusBadge.Name = "lblClientStatusBadge";
        lblClientStatusBadge.Padding = new Padding(12, 5, 12, 5);
        lblClientStatusBadge.Size = new Size(36, 25);
        lblClientStatusBadge.TabIndex = 2;
        lblClientStatusBadge.Text = "-";
        lblClientStatusBadge.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblClientSummary
        // 
        lblClientSummary.AutoSize = true;
        lblClientSummary.Location = new Point(0, 35);
        lblClientSummary.Margin = new Padding(0);
        lblClientSummary.Name = "lblClientSummary";
        lblClientSummary.Size = new Size(12, 15);
        lblClientSummary.TabIndex = 1;
        lblClientSummary.Text = "-";
        // 
        // lblLastUpdated
        // 
        lblLastUpdated.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblLastUpdated.AutoSize = true;
        lblLastUpdated.Location = new Point(753, 35);
        lblLastUpdated.Margin = new Padding(0);
        lblLastUpdated.Name = "lblLastUpdated";
        lblLastUpdated.Size = new Size(0, 15);
        lblLastUpdated.TabIndex = 3;
        lblLastUpdated.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabClient
        // 
        tabClient.Controls.Add(tabProfile);
        tabClient.Controls.Add(tabContacts);
        tabClient.Controls.Add(tabHistory);
        tabClient.Dock = DockStyle.Fill;
        tabClient.Location = new Point(20, 82);
        tabClient.Margin = new Padding(0);
        tabClient.Name = "tabClient";
        tabClient.SelectedIndex = 0;
        tabClient.Size = new Size(753, 574);
        tabClient.TabIndex = 1;
        // 
        // tabProfile
        // 
        tabProfile.Controls.Add(pnlProfileScroll);
        tabProfile.Location = new Point(4, 24);
        tabProfile.Name = "tabProfile";
        tabProfile.Padding = new Padding(3);
        tabProfile.Size = new Size(745, 546);
        tabProfile.TabIndex = 0;
        tabProfile.Text = "Profile";
        tabProfile.UseVisualStyleBackColor = true;
        // 
        // pnlProfileScroll
        // 
        pnlProfileScroll.AutoScroll = true;
        pnlProfileScroll.Controls.Add(tlpProfile);
        pnlProfileScroll.Dock = DockStyle.Fill;
        pnlProfileScroll.Location = new Point(3, 3);
        pnlProfileScroll.Margin = new Padding(0);
        pnlProfileScroll.Name = "pnlProfileScroll";
        pnlProfileScroll.Padding = new Padding(20);
        pnlProfileScroll.Size = new Size(739, 540);
        pnlProfileScroll.TabIndex = 0;
        // 
        // tlpProfile
        // 
        tlpProfile.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpProfile.ColumnCount = 4;
        tlpProfile.ColumnStyles.Add(new ColumnStyle());
        tlpProfile.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpProfile.ColumnStyles.Add(new ColumnStyle());
        tlpProfile.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpProfile.Controls.Add(lblClientTypeCaption, 0, 0);
        tlpProfile.Controls.Add(tlpClientType, 1, 0);
        tlpProfile.Controls.Add(lblSectionClientInformation, 0, 2);
        tlpProfile.Controls.Add(sepClientInformation, 0, 3);
        tlpProfile.Controls.Add(lblFirstNameCaption, 0, 4);
        tlpProfile.Controls.Add(txtFirstName, 1, 4);
        tlpProfile.Controls.Add(lblMiddleNameCaption, 2, 4);
        tlpProfile.Controls.Add(txtMiddleName, 3, 4);
        tlpProfile.Controls.Add(lblLastNameCaption, 0, 5);
        tlpProfile.Controls.Add(txtLastName, 1, 5);
        tlpProfile.Controls.Add(lblSuffixCaption, 2, 5);
        tlpProfile.Controls.Add(cmbSuffix, 3, 5);
        tlpProfile.Controls.Add(lblDisplayNameCaption, 0, 6);
        tlpProfile.Controls.Add(txtDisplayName, 1, 6);
        tlpProfile.Controls.Add(lblDisplayNameHelper, 1, 7);
        tlpProfile.Controls.Add(lblSectionContactInformation, 0, 8);
        tlpProfile.Controls.Add(sepContactInformation, 0, 9);
        tlpProfile.Controls.Add(lblEmailCaption, 0, 10);
        tlpProfile.Controls.Add(txtEmail, 1, 10);
        tlpProfile.Controls.Add(lblPhoneCaption, 0, 11);
        tlpProfile.Controls.Add(txtPhone, 1, 11);
        tlpProfile.Controls.Add(lblSectionMailingAddress, 0, 12);
        tlpProfile.Controls.Add(sepMailingAddress, 0, 13);
        tlpProfile.Controls.Add(lblAddressLine1Caption, 0, 14);
        tlpProfile.Controls.Add(txtAddressLine1, 1, 14);
        tlpProfile.Controls.Add(lblAddressLine2Caption, 0, 15);
        tlpProfile.Controls.Add(txtAddressLine2, 1, 15);
        tlpProfile.Controls.Add(lblCityCaption, 0, 16);
        tlpProfile.Controls.Add(tlpCityStateZip, 1, 16);
        tlpProfile.Controls.Add(lblSectionCommunication, 0, 17);
        tlpProfile.Controls.Add(sepCommunication, 0, 18);
        tlpProfile.Controls.Add(lblPreferredMethodCaption, 0, 19);
        tlpProfile.Controls.Add(cmbPreferredMethod, 1, 19);
        tlpProfile.Controls.Add(lblClientTypeRule, 1, 1);
        tlpProfile.Controls.Add(lblConsentNote, 1, 20);
        tlpProfile.Controls.Add(lblEmailAvailability, 1, 21);
        tlpProfile.Location = new Point(20, 20);
        tlpProfile.Margin = new Padding(0);
        tlpProfile.Name = "tlpProfile";
        tlpProfile.RowCount = 22;
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.RowStyles.Add(new RowStyle());
        tlpProfile.Size = new Size(682, 560);
        tlpProfile.TabIndex = 0;
        // 
        // lblClientTypeCaption
        // 
        lblClientTypeCaption.Anchor = AnchorStyles.Right;
        lblClientTypeCaption.AutoSize = true;
        lblClientTypeCaption.Location = new Point(124, 1);
        lblClientTypeCaption.Margin = new Padding(0, 0, 12, 10);
        lblClientTypeCaption.Name = "lblClientTypeCaption";
        lblClientTypeCaption.Size = new Size(66, 15);
        lblClientTypeCaption.TabIndex = 2;
        lblClientTypeCaption.Text = "Client Type";
        // 
        // tlpClientType
        // 
        tlpClientType.AutoSize = true;
        tlpClientType.ColumnCount = 3;
        tlpProfile.SetColumnSpan(tlpClientType, 2);
        tlpClientType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpClientType.ColumnStyles.Add(new ColumnStyle());
        tlpClientType.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tlpClientType.Controls.Add(cmbClientTypeValue, 0, 0);
        tlpClientType.Controls.Add(lblClientTypeLock, 1, 0);
        tlpClientType.Dock = DockStyle.Fill;
        tlpClientType.Location = new Point(202, 0);
        tlpClientType.Margin = new Padding(0, 0, 0, 4);
        tlpClientType.Name = "tlpClientType";
        tlpClientType.RowCount = 1;
        tlpClientType.RowStyles.Add(new RowStyle());
        tlpClientType.Size = new Size(285, 23);
        tlpClientType.TabIndex = 3;
        // 
        // cmbClientTypeValue
        // 
        cmbClientTypeValue.Dock = DockStyle.Fill;
        cmbClientTypeValue.Enabled = false;
        cmbClientTypeValue.Location = new Point(0, 0);
        cmbClientTypeValue.Margin = new Padding(0, 0, 8, 0);
        cmbClientTypeValue.Name = "cmbClientTypeValue";
        cmbClientTypeValue.Size = new Size(238, 23);
        cmbClientTypeValue.TabIndex = 0;
        // 
        // lblClientTypeLock
        // 
        lblClientTypeLock.Anchor = AnchorStyles.Left;
        lblClientTypeLock.AutoSize = true;
        lblClientTypeLock.Location = new Point(246, 4);
        lblClientTypeLock.Margin = new Padding(0);
        lblClientTypeLock.Name = "lblClientTypeLock";
        lblClientTypeLock.Size = new Size(19, 15);
        lblClientTypeLock.TabIndex = 2;
        lblClientTypeLock.Text = "🔒";
        // 
        // lblSectionClientInformation
        // 
        lblSectionClientInformation.AutoSize = true;
        tlpProfile.SetColumnSpan(lblSectionClientInformation, 4);
        lblSectionClientInformation.Location = new Point(0, 52);
        lblSectionClientInformation.Margin = new Padding(0, 0, 0, 6);
        lblSectionClientInformation.Name = "lblSectionClientInformation";
        lblSectionClientInformation.Size = new Size(104, 15);
        lblSectionClientInformation.TabIndex = 0;
        lblSectionClientInformation.Text = "Client Information";
        // 
        // sepClientInformation
        // 
        tlpProfile.SetColumnSpan(sepClientInformation, 4);
        sepClientInformation.Dock = DockStyle.Fill;
        sepClientInformation.Location = new Point(0, 73);
        sepClientInformation.Margin = new Padding(0, 0, 0, 12);
        sepClientInformation.Name = "sepClientInformation";
        sepClientInformation.Size = new Size(682, 1);
        sepClientInformation.TabIndex = 1;
        // 
        // lblFirstNameCaption
        // 
        lblFirstNameCaption.Anchor = AnchorStyles.Right;
        lblFirstNameCaption.AutoSize = true;
        lblFirstNameCaption.Location = new Point(126, 90);
        lblFirstNameCaption.Margin = new Padding(0, 0, 12, 10);
        lblFirstNameCaption.Name = "lblFirstNameCaption";
        lblFirstNameCaption.Size = new Size(64, 15);
        lblFirstNameCaption.TabIndex = 5;
        lblFirstNameCaption.Text = "First Name";
        // 
        // txtFirstName
        // 
        txtFirstName.AutoCompleteMode = AutoCompleteMode.Suggest;
        txtFirstName.Dock = DockStyle.Fill;
        txtFirstName.Location = new Point(202, 86);
        txtFirstName.Margin = new Padding(0, 0, 10, 10);
        txtFirstName.Name = "txtFirstName";
        txtFirstName.Size = new Size(184, 23);
        txtFirstName.TabIndex = 6;
        txtFirstName.TextChanged += ProfileField_Changed;
        // 
        // lblMiddleNameCaption
        // 
        lblMiddleNameCaption.Anchor = AnchorStyles.Right;
        lblMiddleNameCaption.AutoSize = true;
        lblMiddleNameCaption.Location = new Point(396, 90);
        lblMiddleNameCaption.Margin = new Padding(0, 0, 12, 10);
        lblMiddleNameCaption.Name = "lblMiddleNameCaption";
        lblMiddleNameCaption.Size = new Size(79, 15);
        lblMiddleNameCaption.TabIndex = 7;
        lblMiddleNameCaption.Text = "Middle Name";
        // 
        // txtMiddleName
        // 
        txtMiddleName.Dock = DockStyle.Fill;
        txtMiddleName.Location = new Point(487, 86);
        txtMiddleName.Margin = new Padding(0, 0, 0, 10);
        txtMiddleName.Name = "txtMiddleName";
        txtMiddleName.Size = new Size(195, 23);
        txtMiddleName.TabIndex = 8;
        txtMiddleName.TextChanged += ProfileField_Changed;
        // 
        // lblLastNameCaption
        // 
        lblLastNameCaption.Anchor = AnchorStyles.Right;
        lblLastNameCaption.AutoSize = true;
        lblLastNameCaption.Location = new Point(127, 123);
        lblLastNameCaption.Margin = new Padding(0, 0, 12, 10);
        lblLastNameCaption.Name = "lblLastNameCaption";
        lblLastNameCaption.Size = new Size(63, 15);
        lblLastNameCaption.TabIndex = 9;
        lblLastNameCaption.Text = "Last Name";
        // 
        // txtLastName
        // 
        txtLastName.Dock = DockStyle.Fill;
        txtLastName.Location = new Point(202, 119);
        txtLastName.Margin = new Padding(0, 0, 10, 10);
        txtLastName.Name = "txtLastName";
        txtLastName.Size = new Size(184, 23);
        txtLastName.TabIndex = 10;
        txtLastName.TextChanged += ProfileField_Changed;
        // 
        // lblSuffixCaption
        // 
        lblSuffixCaption.Anchor = AnchorStyles.Right;
        lblSuffixCaption.AutoSize = true;
        lblSuffixCaption.Location = new Point(439, 123);
        lblSuffixCaption.Margin = new Padding(0, 0, 12, 10);
        lblSuffixCaption.Name = "lblSuffixCaption";
        lblSuffixCaption.Size = new Size(36, 15);
        lblSuffixCaption.TabIndex = 11;
        lblSuffixCaption.Text = "Suffix";
        // 
        // cmbSuffix
        // 
        cmbSuffix.Dock = DockStyle.Fill;
        cmbSuffix.Location = new Point(487, 119);
        cmbSuffix.Margin = new Padding(0, 0, 0, 10);
        cmbSuffix.MaximumSize = new Size(75, 0);
        cmbSuffix.Name = "cmbSuffix";
        cmbSuffix.Size = new Size(75, 23);
        cmbSuffix.TabIndex = 12;
        cmbSuffix.SelectedIndexChanged += ProfileField_Changed;
        // 
        // lblDisplayNameCaption
        // 
        lblDisplayNameCaption.Anchor = AnchorStyles.Right;
        lblDisplayNameCaption.AutoSize = true;
        lblDisplayNameCaption.Location = new Point(110, 156);
        lblDisplayNameCaption.Margin = new Padding(0, 0, 12, 4);
        lblDisplayNameCaption.Name = "lblDisplayNameCaption";
        lblDisplayNameCaption.Size = new Size(80, 15);
        lblDisplayNameCaption.TabIndex = 13;
        lblDisplayNameCaption.Text = "Display Name";
        // 
        // txtDisplayName
        // 
        tlpProfile.SetColumnSpan(txtDisplayName, 3);
        txtDisplayName.Dock = DockStyle.Fill;
        txtDisplayName.Location = new Point(202, 152);
        txtDisplayName.Margin = new Padding(0, 0, 0, 4);
        txtDisplayName.Name = "txtDisplayName";
        txtDisplayName.Size = new Size(480, 23);
        txtDisplayName.TabIndex = 14;
        txtDisplayName.TextChanged += ProfileField_Changed;
        // 
        // lblDisplayNameHelper
        // 
        lblDisplayNameHelper.AutoSize = true;
        tlpProfile.SetColumnSpan(lblDisplayNameHelper, 3);
        lblDisplayNameHelper.Location = new Point(202, 179);
        lblDisplayNameHelper.Margin = new Padding(0, 0, 0, 10);
        lblDisplayNameHelper.Name = "lblDisplayNameHelper";
        lblDisplayNameHelper.Size = new Size(333, 15);
        lblDisplayNameHelper.TabIndex = 15;
        lblDisplayNameHelper.Text = "This is how the client will appear in lists and communications.";
        // 
        // lblSectionContactInformation
        // 
        lblSectionContactInformation.AutoSize = true;
        tlpProfile.SetColumnSpan(lblSectionContactInformation, 4);
        lblSectionContactInformation.Location = new Point(0, 204);
        lblSectionContactInformation.Margin = new Padding(0, 0, 0, 6);
        lblSectionContactInformation.Name = "lblSectionContactInformation";
        lblSectionContactInformation.Size = new Size(115, 15);
        lblSectionContactInformation.TabIndex = 16;
        lblSectionContactInformation.Text = "Contact Information";
        // 
        // sepContactInformation
        // 
        tlpProfile.SetColumnSpan(sepContactInformation, 4);
        sepContactInformation.Dock = DockStyle.Fill;
        sepContactInformation.Location = new Point(0, 225);
        sepContactInformation.Margin = new Padding(0, 0, 0, 12);
        sepContactInformation.Name = "sepContactInformation";
        sepContactInformation.Size = new Size(682, 1);
        sepContactInformation.TabIndex = 17;
        // 
        // lblEmailCaption
        // 
        lblEmailCaption.Anchor = AnchorStyles.Right;
        lblEmailCaption.AutoSize = true;
        lblEmailCaption.Location = new Point(109, 242);
        lblEmailCaption.Margin = new Padding(0, 0, 12, 10);
        lblEmailCaption.Name = "lblEmailCaption";
        lblEmailCaption.Size = new Size(81, 15);
        lblEmailCaption.TabIndex = 18;
        lblEmailCaption.Text = "Email Address";
        // 
        // txtEmail
        // 
        tlpProfile.SetColumnSpan(txtEmail, 3);
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.Location = new Point(202, 238);
        txtEmail.Margin = new Padding(0, 0, 0, 10);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(480, 23);
        txtEmail.TabIndex = 19;
        txtEmail.TextChanged += ProfileField_Changed;
        // 
        // lblPhoneCaption
        // 
        lblPhoneCaption.Anchor = AnchorStyles.Right;
        lblPhoneCaption.AutoSize = true;
        lblPhoneCaption.Location = new Point(102, 275);
        lblPhoneCaption.Margin = new Padding(0, 0, 12, 16);
        lblPhoneCaption.Name = "lblPhoneCaption";
        lblPhoneCaption.Size = new Size(88, 15);
        lblPhoneCaption.TabIndex = 20;
        lblPhoneCaption.Text = "Phone Number";
        // 
        // txtPhone
        // 
        txtPhone.Dock = DockStyle.Fill;
        txtPhone.Location = new Point(202, 271);
        txtPhone.Margin = new Padding(0, 0, 0, 16);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(194, 23);
        txtPhone.TabIndex = 21;
        txtPhone.TextChanged += ProfileField_Changed;
        // 
        // lblSectionMailingAddress
        // 
        lblSectionMailingAddress.AutoSize = true;
        tlpProfile.SetColumnSpan(lblSectionMailingAddress, 4);
        lblSectionMailingAddress.Location = new Point(0, 310);
        lblSectionMailingAddress.Margin = new Padding(0, 0, 0, 6);
        lblSectionMailingAddress.Name = "lblSectionMailingAddress";
        lblSectionMailingAddress.Size = new Size(92, 15);
        lblSectionMailingAddress.TabIndex = 22;
        lblSectionMailingAddress.Text = "Mailing Address";
        // 
        // sepMailingAddress
        // 
        tlpProfile.SetColumnSpan(sepMailingAddress, 4);
        sepMailingAddress.Dock = DockStyle.Fill;
        sepMailingAddress.Location = new Point(0, 331);
        sepMailingAddress.Margin = new Padding(0, 0, 0, 12);
        sepMailingAddress.Name = "sepMailingAddress";
        sepMailingAddress.Size = new Size(682, 1);
        sepMailingAddress.TabIndex = 23;
        // 
        // lblAddressLine1Caption
        // 
        lblAddressLine1Caption.Anchor = AnchorStyles.Right;
        lblAddressLine1Caption.AutoSize = true;
        lblAddressLine1Caption.Location = new Point(107, 348);
        lblAddressLine1Caption.Margin = new Padding(0, 0, 12, 10);
        lblAddressLine1Caption.Name = "lblAddressLine1Caption";
        lblAddressLine1Caption.Size = new Size(83, 15);
        lblAddressLine1Caption.TabIndex = 24;
        lblAddressLine1Caption.Text = "Address Line 1";
        // 
        // txtAddressLine1
        // 
        tlpProfile.SetColumnSpan(txtAddressLine1, 3);
        txtAddressLine1.Dock = DockStyle.Fill;
        txtAddressLine1.Location = new Point(202, 344);
        txtAddressLine1.Margin = new Padding(0, 0, 0, 10);
        txtAddressLine1.Name = "txtAddressLine1";
        txtAddressLine1.Size = new Size(480, 23);
        txtAddressLine1.TabIndex = 25;
        txtAddressLine1.TextChanged += ProfileField_Changed;
        // 
        // lblAddressLine2Caption
        // 
        lblAddressLine2Caption.Anchor = AnchorStyles.Right;
        lblAddressLine2Caption.AutoSize = true;
        lblAddressLine2Caption.Location = new Point(107, 381);
        lblAddressLine2Caption.Margin = new Padding(0, 0, 12, 10);
        lblAddressLine2Caption.Name = "lblAddressLine2Caption";
        lblAddressLine2Caption.Size = new Size(83, 15);
        lblAddressLine2Caption.TabIndex = 26;
        lblAddressLine2Caption.Text = "Address Line 2";
        // 
        // txtAddressLine2
        // 
        tlpProfile.SetColumnSpan(txtAddressLine2, 3);
        txtAddressLine2.Dock = DockStyle.Fill;
        txtAddressLine2.Location = new Point(202, 377);
        txtAddressLine2.Margin = new Padding(0, 0, 0, 10);
        txtAddressLine2.Name = "txtAddressLine2";
        txtAddressLine2.Size = new Size(480, 23);
        txtAddressLine2.TabIndex = 27;
        txtAddressLine2.TextChanged += ProfileField_Changed;
        // 
        // lblCityCaption
        // 
        lblCityCaption.Anchor = AnchorStyles.Right;
        lblCityCaption.AutoSize = true;
        lblCityCaption.Location = new Point(162, 414);
        lblCityCaption.Margin = new Padding(0, 0, 12, 16);
        lblCityCaption.Name = "lblCityCaption";
        lblCityCaption.Size = new Size(28, 15);
        lblCityCaption.TabIndex = 28;
        lblCityCaption.Text = "City";
        // 
        // tlpCityStateZip
        // 
        tlpCityStateZip.AutoSize = true;
        tlpCityStateZip.ColumnCount = 5;
        tlpProfile.SetColumnSpan(tlpCityStateZip, 3);
        tlpCityStateZip.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCityStateZip.ColumnStyles.Add(new ColumnStyle());
        tlpCityStateZip.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpCityStateZip.ColumnStyles.Add(new ColumnStyle());
        tlpCityStateZip.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpCityStateZip.Controls.Add(txtCity, 0, 0);
        tlpCityStateZip.Controls.Add(lblStateCaption, 1, 0);
        tlpCityStateZip.Controls.Add(cmbState, 2, 0);
        tlpCityStateZip.Controls.Add(lblZipCodeCaption, 3, 0);
        tlpCityStateZip.Controls.Add(txtZipCode, 4, 0);
        tlpCityStateZip.Dock = DockStyle.Fill;
        tlpCityStateZip.Location = new Point(202, 410);
        tlpCityStateZip.Margin = new Padding(0, 0, 0, 16);
        tlpCityStateZip.Name = "tlpCityStateZip";
        tlpCityStateZip.RowCount = 1;
        tlpCityStateZip.RowStyles.Add(new RowStyle());
        tlpCityStateZip.Size = new Size(480, 23);
        tlpCityStateZip.TabIndex = 29;
        // 
        // txtCity
        // 
        txtCity.Dock = DockStyle.Fill;
        txtCity.Location = new Point(0, 0);
        txtCity.Margin = new Padding(0, 0, 16, 0);
        txtCity.Name = "txtCity";
        txtCity.Size = new Size(172, 23);
        txtCity.TabIndex = 0;
        txtCity.TextChanged += ProfileField_Changed;
        // 
        // lblStateCaption
        // 
        lblStateCaption.Anchor = AnchorStyles.Left;
        lblStateCaption.AutoSize = true;
        lblStateCaption.Location = new Point(188, 4);
        lblStateCaption.Margin = new Padding(0, 0, 8, 0);
        lblStateCaption.Name = "lblStateCaption";
        lblStateCaption.Size = new Size(33, 15);
        lblStateCaption.TabIndex = 1;
        lblStateCaption.Text = "State";
        // 
        // cmbState
        // 
        cmbState.Dock = DockStyle.Fill;
        cmbState.Location = new Point(229, 0);
        cmbState.Margin = new Padding(0, 0, 16, 0);
        cmbState.Name = "cmbState";
        cmbState.Size = new Size(78, 23);
        cmbState.TabIndex = 2;
        cmbState.SelectedIndexChanged += ProfileField_Changed;
        // 
        // lblZipCodeCaption
        // 
        lblZipCodeCaption.Anchor = AnchorStyles.Left;
        lblZipCodeCaption.AutoSize = true;
        lblZipCodeCaption.Location = new Point(323, 4);
        lblZipCodeCaption.Margin = new Padding(0, 0, 8, 0);
        lblZipCodeCaption.Name = "lblZipCodeCaption";
        lblZipCodeCaption.Size = new Size(55, 15);
        lblZipCodeCaption.TabIndex = 3;
        lblZipCodeCaption.Text = "ZIP Code";
        // 
        // txtZipCode
        // 
        txtZipCode.Dock = DockStyle.Fill;
        txtZipCode.Location = new Point(386, 0);
        txtZipCode.Margin = new Padding(0);
        txtZipCode.Name = "txtZipCode";
        txtZipCode.Size = new Size(94, 23);
        txtZipCode.TabIndex = 4;
        txtZipCode.TextChanged += ProfileField_Changed;
        // 
        // lblSectionCommunication
        // 
        lblSectionCommunication.AutoSize = true;
        tlpProfile.SetColumnSpan(lblSectionCommunication, 4);
        lblSectionCommunication.Location = new Point(0, 449);
        lblSectionCommunication.Margin = new Padding(0, 0, 0, 6);
        lblSectionCommunication.Name = "lblSectionCommunication";
        lblSectionCommunication.Size = new Size(158, 15);
        lblSectionCommunication.TabIndex = 30;
        lblSectionCommunication.Text = "Communication Preferences";
        // 
        // sepCommunication
        // 
        tlpProfile.SetColumnSpan(sepCommunication, 4);
        sepCommunication.Dock = DockStyle.Fill;
        sepCommunication.Location = new Point(0, 470);
        sepCommunication.Margin = new Padding(0, 0, 0, 12);
        sepCommunication.Name = "sepCommunication";
        sepCommunication.Size = new Size(682, 1);
        sepCommunication.TabIndex = 31;
        // 
        // lblPreferredMethodCaption
        // 
        lblPreferredMethodCaption.Anchor = AnchorStyles.Right;
        lblPreferredMethodCaption.AutoSize = true;
        lblPreferredMethodCaption.Location = new Point(0, 487);
        lblPreferredMethodCaption.Margin = new Padding(0, 0, 12, 6);
        lblPreferredMethodCaption.Name = "lblPreferredMethodCaption";
        lblPreferredMethodCaption.Size = new Size(190, 15);
        lblPreferredMethodCaption.TabIndex = 32;
        lblPreferredMethodCaption.Text = "Preferred Communication Method";
        // 
        // cmbPreferredMethod
        // 
        cmbPreferredMethod.Dock = DockStyle.Fill;
        cmbPreferredMethod.Location = new Point(202, 483);
        cmbPreferredMethod.Margin = new Padding(0, 0, 0, 6);
        cmbPreferredMethod.Name = "cmbPreferredMethod";
        cmbPreferredMethod.Size = new Size(194, 23);
        cmbPreferredMethod.TabIndex = 33;
        cmbPreferredMethod.SelectedIndexChanged += CmbPreferredMethod_SelectedIndexChanged;
        // 
        // lblClientTypeRule
        // 
        lblClientTypeRule.AutoSize = true;
        tlpProfile.SetColumnSpan(lblClientTypeRule, 3);
        lblClientTypeRule.Location = new Point(202, 27);
        lblClientTypeRule.Margin = new Padding(0, 0, 0, 10);
        lblClientTypeRule.Name = "lblClientTypeRule";
        lblClientTypeRule.Size = new Size(304, 15);
        lblClientTypeRule.TabIndex = 4;
        lblClientTypeRule.Text = "Client type cannot be changed after the client is created.";
        // 
        // lblConsentNote
        // 
        lblConsentNote.AutoSize = true;
        tlpProfile.SetColumnSpan(lblConsentNote, 3);
        lblConsentNote.Location = new Point(202, 512);
        lblConsentNote.Margin = new Padding(0, 0, 0, 10);
        lblConsentNote.Name = "lblConsentNote";
        lblConsentNote.Size = new Size(396, 15);
        lblConsentNote.TabIndex = 35;
        lblConsentNote.Text = "This preference is a routing convenience and does not constitute consent.";
        // 
        // lblEmailAvailability
        // 
        lblEmailAvailability.AutoSize = true;
        tlpProfile.SetColumnSpan(lblEmailAvailability, 3);
        lblEmailAvailability.Location = new Point(202, 537);
        lblEmailAvailability.Margin = new Padding(0, 0, 0, 4);
        lblEmailAvailability.Name = "lblEmailAvailability";
        lblEmailAvailability.Size = new Size(0, 15);
        lblEmailAvailability.TabIndex = 34;
        // 
        // tabContacts
        // 
        tabContacts.Location = new Point(4, 24);
        tabContacts.Name = "tabContacts";
        tabContacts.Padding = new Padding(3);
        tabContacts.Size = new Size(745, 546);
        tabContacts.TabIndex = 1;
        tabContacts.Text = "Contacts";
        tabContacts.UseVisualStyleBackColor = true;
        // 
        // tabHistory
        // 
        tabHistory.Location = new Point(4, 24);
        tabHistory.Name = "tabHistory";
        tabHistory.Padding = new Padding(3);
        tabHistory.Size = new Size(745, 546);
        tabHistory.TabIndex = 2;
        tabHistory.Text = "History";
        tabHistory.UseVisualStyleBackColor = true;
        // 
        // pnlDetailFooter
        // 
        pnlDetailFooter.Controls.Add(flpFooterActions);
        pnlDetailFooter.Dock = DockStyle.Fill;
        pnlDetailFooter.Location = new Point(20, 668);
        pnlDetailFooter.Margin = new Padding(0, 12, 0, 0);
        pnlDetailFooter.Name = "pnlDetailFooter";
        pnlDetailFooter.Padding = new Padding(0, 6, 0, 6);
        pnlDetailFooter.Size = new Size(753, 52);
        pnlDetailFooter.TabIndex = 2;
        // 
        // flpFooterActions
        // 
        flpFooterActions.AutoSize = true;
        flpFooterActions.Controls.Add(btnSave);
        flpFooterActions.Controls.Add(btnCancel);
        flpFooterActions.Dock = DockStyle.Right;
        flpFooterActions.FlowDirection = FlowDirection.RightToLeft;
        flpFooterActions.Location = new Point(523, 6);
        flpFooterActions.Margin = new Padding(0);
        flpFooterActions.Name = "flpFooterActions";
        flpFooterActions.Size = new Size(230, 40);
        flpFooterActions.TabIndex = 0;
        flpFooterActions.WrapContents = false;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(120, 0);
        btnSave.Margin = new Padding(0);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(110, 40);
        btnSave.TabIndex = 0;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(0, 0);
        btnCancel.Margin = new Padding(0, 0, 10, 0);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(110, 40);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // ClientWorkspaceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1360, 740);
        Controls.Add(splitWorkspace);
        MinimumSize = new Size(1024, 680);
        Name = "ClientWorkspaceForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ONE Classics Nexus - Clients";
        splitWorkspace.Panel1.ResumeLayout(false);
        splitWorkspace.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitWorkspace).EndInit();
        splitWorkspace.ResumeLayout(false);
        tlpBrowser.ResumeLayout(false);
        tlpBrowser.PerformLayout();
        tlpSearch.ResumeLayout(false);
        tlpSearch.PerformLayout();
        tlpBrowserFilters.ResumeLayout(false);
        tlpBrowserFilters.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)gridClients).EndInit();
        tlpPagination.ResumeLayout(false);
        tlpPagination.PerformLayout();
        flpPages.ResumeLayout(false);
        tlpDetail.ResumeLayout(false);
        tlpDetail.PerformLayout();
        tlpDetailHeader.ResumeLayout(false);
        tlpDetailHeader.PerformLayout();
        tabClient.ResumeLayout(false);
        tabProfile.ResumeLayout(false);
        pnlProfileScroll.ResumeLayout(false);
        tlpProfile.ResumeLayout(false);
        tlpProfile.PerformLayout();
        tlpClientType.ResumeLayout(false);
        tlpClientType.PerformLayout();
        tlpCityStateZip.ResumeLayout(false);
        tlpCityStateZip.PerformLayout();
        pnlDetailFooter.ResumeLayout(false);
        pnlDetailFooter.PerformLayout();
        flpFooterActions.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    #endregion

    // Workspace shell
    private SplitContainer splitWorkspace;

    // Client browser (left pane)
    private TableLayoutPanel tlpBrowser;
    private Label lblClientsHeading;
    private TableLayoutPanel tlpSearch;
    private TextBox txtSearch;
    private Button btnClearSearch;
    private TableLayoutPanel tlpBrowserFilters;
    private Label lblClientTypeFilterCaption;
    private ComboBox cmbClientTypeFilter;
    private Label lblStatusFilterCaption;
    private ComboBox cmbStatusFilter;
    private Button btnCreateClient;
    private DataGridView gridClients;
    private Label lblNoResults;
    private DataGridViewTextBoxColumn colClientName;
    private DataGridViewTextBoxColumn colClientType;
    private DataGridViewTextBoxColumn colClientStatus;
    private TableLayoutPanel tlpPagination;
    private FlowLayoutPanel flpPages;
    private Button btnPreviousPage;
    private Button btnPage1;
    private Button btnPage2;
    private Button btnPage3;
    private Button btnPage4;
    private Button btnPage5;
    private Button btnNextPage;
    private Label lblPageRange;

    // Client detail (right pane)
    private TableLayoutPanel tlpDetail;
    private TableLayoutPanel tlpDetailHeader;
    private Label lblClientHeading;
    private Label lblClientSummary;
    private Label lblClientStatusBadge;
    private Label lblLastUpdated;
    private TabControl tabClient;
    private TabPage tabProfile;
    private TabPage tabContacts;
    private TabPage tabHistory;

    // Profile form
    private Panel pnlProfileScroll;
    private TableLayoutPanel tlpProfile;
    private Label lblSectionClientInformation;
    private Panel sepClientInformation;
    private Label lblClientTypeCaption;
    private TableLayoutPanel tlpClientType;
    private ComboBox cmbClientTypeValue;
    private Label lblClientTypeRule;
    private Label lblFirstNameCaption;
    private TextBox txtFirstName;
    private Label lblMiddleNameCaption;
    private TextBox txtMiddleName;
    private Label lblLastNameCaption;
    private TextBox txtLastName;
    private Label lblSuffixCaption;
    private ComboBox cmbSuffix;
    private Label lblDisplayNameCaption;
    private TextBox txtDisplayName;
    private Label lblDisplayNameHelper;
    private Label lblSectionContactInformation;
    private Panel sepContactInformation;
    private Label lblEmailCaption;
    private TextBox txtEmail;
    private Label lblPhoneCaption;
    private TextBox txtPhone;
    private Label lblSectionMailingAddress;
    private Panel sepMailingAddress;
    private Label lblAddressLine1Caption;
    private TextBox txtAddressLine1;
    private Label lblAddressLine2Caption;
    private TextBox txtAddressLine2;
    private Label lblCityCaption;
    private TableLayoutPanel tlpCityStateZip;
    private TextBox txtCity;
    private Label lblStateCaption;
    private ComboBox cmbState;
    private Label lblZipCodeCaption;
    private TextBox txtZipCode;
    private Label lblSectionCommunication;
    private Panel sepCommunication;
    private Label lblPreferredMethodCaption;
    private ComboBox cmbPreferredMethod;
    private Label lblEmailAvailability;
    private Label lblConsentNote;

    // Footer
    private Panel pnlDetailFooter;
    private FlowLayoutPanel flpFooterActions;
    private Button btnSave;
    private Button btnCancel;

    private ErrorProvider errorProvider;
    private ToolTip toolTip;
    private Label lblClientTypeLock;
}
