// ============================================================
// ClientWorkspaceForm.cs
// Individual Client Workspace - see
// docs/07_UI_UX/Mockups/Client/ONE-Classics-Nexus-Individual-Client-Workspace.md
// ============================================================

using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.WinForms.Theming;
using ONE_Classics_Nexus.WinForms.Services.Api;
using ONE_Classics_Nexus.Application.DTOs;
using System.ComponentModel;
using System.Globalization;

namespace ONE_Classics_Nexus.WinForms.Forms.Clients;

/// <summary>
/// Clients workspace: browse clients on the left, edit the selected client's profile on the right.
/// </summary>
public partial class ClientWorkspaceForm : Form
{
    private const int PageSize = 10;
    private const string AllTypes = "All Types";
    private const string AllStatuses = "All Statuses";

    private const string DisplayNameHeader = "Display Name";
    private const string TypeHeader = "Type";
    private const string StatusHeader = "Status";

    private readonly BindingList<ClientListItem> _pageItems = new();
    private readonly List<ClientListItem> _allClients = new();
    private readonly ClientApiClient? _apiClient;

    private ClientListItem? _current;
    private bool _binding;
    private bool _dirty;
    private int _currentPage = 1;
    private int _totalCount;
    private int _totalPages = 1;
    private ClientSortBy _sortBy = ClientSortBy.DisplayName;
    private ClientSortDirection _sortDirection = ClientSortDirection.Ascending;
    private CancellationTokenSource? _loadCts;

    public ClientWorkspaceForm()
        : this(null)
    {
    }

    /// <summary>
    /// Creates the workspace backed by the Clients API. When
    /// <paramref name="apiClient"/> is null the workspace filters and pages the
    /// in-memory collection supplied through <see cref="LoadClients"/> instead,
    /// which keeps the designer and tests self-contained.
    /// </summary>
    public ClientWorkspaceForm(ClientApiClient? apiClient)
    {
        _apiClient = apiClient;

        InitializeComponent();

        ApplyTheme();
        ConfigureLookups();
        ConfigureGrid();
        ConfigureAccessibility();

        RefreshBrowser();
        UpdateDetail();

        if (_apiClient is not null)
        {
            Shown += ClientWorkspaceForm_Shown;
        }
    }

    private async void ClientWorkspaceForm_Shown(object? sender, EventArgs e) =>
        await ReloadFromServerSafelyAsync().ConfigureAwait(true);

    /// <summary>Raised when the user requests creation of a new client.</summary>
    public event EventHandler? CreateClientRequested;

    /// <summary>Raised when the user saves changes to the selected client.</summary>
    public event EventHandler<ClientSavedEventArgs>? ClientSaved;

    /// <summary>Identifier of the client currently shown in the detail pane.</summary>
    public Guid? SelectedClientId => _current?.ClientId;

    public string ApiAllStatuses { get; private set; }

    /// <summary>Replaces the workspace client collection and reselects the first page.</summary>
    public void LoadClients(IEnumerable<ClientListItem> clients)
    {
        ArgumentNullException.ThrowIfNull(clients);

        _allClients.Clear();
        _allClients.AddRange(clients);
        _currentPage = 1;

        RefreshBrowser();
    }

    private void ApplyTheme()
    {
        NexusControlStyles.ApplyForm(this);

        tlpBrowser.BackColor = NexusTheme.Surface;
        tlpDetail.BackColor = NexusTheme.Surface;
        pnlProfileScroll.BackColor = NexusTheme.Surface;
        tlpProfile.BackColor = NexusTheme.Surface;
        pnlDetailFooter.BackColor = NexusTheme.SurfaceAlt;

        NexusControlStyles.ApplyTextBox(txtSearch);
        NexusControlStyles.ApplyComboBox(cmbClientTypeFilter);
        NexusControlStyles.ApplyComboBox(cmbStatusFilter);
        NexusControlStyles.ApplyPrimaryButton(btnCreateClient);
        NexusControlStyles.ApplyDataGridView(gridClients);

        NexusControlStyles.ApplyHeaderLabel(lblClientsHeading);

        NexusControlStyles.ApplySecondaryButton(btnClearSearch);
        btnClearSearch.AccessibleName = "Clear search";

        NexusControlStyles.ApplyMetaLabel(lblNoResults);
        lblNoResults.ForeColor = NexusTheme.TextMuted;

        NexusControlStyles.ApplyMetaLabel(lblClientTypeFilterCaption);
        NexusControlStyles.ApplyMetaLabel(lblStatusFilterCaption);
        NexusControlStyles.ApplyMetaLabel(lblPageRange);

        NexusControlStyles.ApplyHeaderLabel(lblClientHeading);
        NexusControlStyles.ApplyBodyLabel(lblClientSummary);
        lblClientSummary.ForeColor = NexusTheme.TextMuted;
        NexusControlStyles.ApplyMetaLabel(lblLastUpdated);

        lblClientStatusBadge.AutoSize = true;
        lblClientStatusBadge.Font = NexusTheme.LabelFont;
        lblClientStatusBadge.BackColor = NexusTheme.PrimarySoft;
        lblClientStatusBadge.ForeColor = NexusTheme.Success;

        foreach (Label section in new[]
        {
            lblSectionClientInformation,
            lblSectionContactInformation,
            lblSectionMailingAddress,
            lblSectionCommunication
        })
        {
            NexusControlStyles.ApplySectionLabel(section);
        }

        foreach (Panel separator in new[]
        {
            sepClientInformation,
            sepContactInformation,
            sepMailingAddress,
            sepCommunication
        })
        {
            separator.BackColor = NexusTheme.Border;
        }

        foreach (Label caption in new[]
        {
            lblClientTypeCaption,
            lblFirstNameCaption,
            lblMiddleNameCaption,
            lblLastNameCaption,
            lblSuffixCaption,
            lblDisplayNameCaption,
            lblEmailCaption,
            lblPhoneCaption,
            lblAddressLine1Caption,
            lblAddressLine2Caption,
            lblCityCaption,
            lblStateCaption,
            lblZipCodeCaption,
            lblPreferredMethodCaption
        })
        {
            caption.Font = NexusTheme.LabelFont;
            caption.ForeColor = NexusTheme.TextStrong;
        }

        foreach (TextBox input in new[]
        {
            txtFirstName,
            txtMiddleName,
            txtLastName,
            txtDisplayName,
            txtEmail,
            txtPhone,
            txtAddressLine1,
            txtAddressLine2,
            txtCity,
            txtZipCode
        })
        {
            NexusControlStyles.ApplyTextBox(input);
        }

        NexusControlStyles.ApplyComboBox(cmbClientTypeValue);
        NexusControlStyles.ApplyComboBox(cmbSuffix);
        NexusControlStyles.ApplyComboBox(cmbState);
        NexusControlStyles.ApplyComboBox(cmbPreferredMethod);

        cmbClientTypeValue.BackColor = NexusTheme.SurfaceAlt;
        cmbClientTypeValue.ForeColor = NexusTheme.TextMuted;
        lblClientTypeLock.ForeColor = NexusTheme.TextMuted;

        NexusControlStyles.ApplyMetaLabel(lblClientTypeRule);
        NexusControlStyles.ApplyMetaLabel(lblDisplayNameHelper);
        NexusControlStyles.ApplyMetaLabel(lblConsentNote);
        lblConsentNote.Font = new Font(NexusTheme.MetaFont, FontStyle.Italic);

        lblEmailAvailability.Font = NexusTheme.MetaFont;
        lblEmailAvailability.ForeColor = NexusTheme.Success;

        NexusControlStyles.ApplyPrimaryButton(btnSave);
        NexusControlStyles.ApplySecondaryButton(btnCancel);

        foreach (Button page in new[] { btnPreviousPage, btnPage1, btnPage2, btnPage3, btnPage4, btnPage5, btnNextPage })
        {
            NexusControlStyles.ApplySecondaryButton(page);
            page.Height = 30;
        }
    }

    private void ConfigureLookups()
    {
        cmbClientTypeFilter.Items.AddRange(new object[] { AllTypes, "Individual", "Organization" });
        cmbClientTypeFilter.SelectedIndex = 0;
        cmbClientTypeFilter.SelectedIndexChanged += Filter_Changed;

        cmbStatusFilter.Items.AddRange(new object[] { AllStatuses, "Active", "Inactive", "Archived" });
        // The Client List opens on Active records only.
        cmbStatusFilter.SelectedItem = "Active";
        cmbStatusFilter.SelectedIndexChanged += Filter_Changed;

        txtSearch.TextChanged += Filter_Changed;

        cmbClientTypeValue.Items.AddRange(new object[] { "Individual", "Organization" });

        cmbSuffix.Items.AddRange(new object[] { string.Empty, "Jr.", "Sr.", "II", "III", "IV", "MD", "PhD" });

        cmbState.Items.AddRange(UsStates);

        cmbPreferredMethod.Items.AddRange(new object[] { "Email", "Phone", "Mail" });
    }

    private void ConfigureGrid()
    {
        gridClients.AutoGenerateColumns = false;
        gridClients.Columns.Clear();

        colClientName = new DataGridViewTextBoxColumn
        {
            Name = nameof(colClientName),
            HeaderText = DisplayNameHeader,
            DataPropertyName = nameof(ClientListItem.DisplayName),
            FillWeight = 200,
            SortMode = DataGridViewColumnSortMode.Programmatic
        };
        colClientType = new DataGridViewTextBoxColumn
        {
            Name = nameof(colClientType),
            HeaderText = TypeHeader,
            DataPropertyName = nameof(ClientListItem.ClientType),
            FillWeight = 100,
            SortMode = DataGridViewColumnSortMode.Programmatic
        };
        colClientStatus = new DataGridViewTextBoxColumn
        {
            Name = nameof(colClientStatus),
            HeaderText = StatusHeader,
            DataPropertyName = nameof(ClientListItem.Status),
            FillWeight = 90,
            SortMode = DataGridViewColumnSortMode.Programmatic
        };

        gridClients.Columns.AddRange(colClientName, colClientType, colClientStatus);
        gridClients.DataSource = _pageItems;

        gridClients.ColumnHeaderMouseClick += GridClients_ColumnHeaderMouseClick;

        UpdateSortIndicators();
    }

    /// <summary>
    /// Appends an arrow to the sorted column's heading so the sort direction
    /// is visible, and clears it from the others.
    /// </summary>
    private void UpdateSortIndicators()
    {
        colClientName.HeaderText = BuildHeaderText(DisplayNameHeader, ClientSortBy.DisplayName);
        colClientType.HeaderText = BuildHeaderText(TypeHeader, ClientSortBy.ClientTypeCode);
        colClientStatus.HeaderText = BuildHeaderText(StatusHeader, ClientSortBy.StatusCode);
    }

    private string BuildHeaderText(string caption, ClientSortBy column)
    {
        if (_sortBy != column)
            return caption;

        string arrow = _sortDirection == ClientSortDirection.Ascending
            ? "\u25B2"
            : "\u25BC";

        return $"{caption}  {arrow}";
    }

    private void GridClients_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.ColumnIndex < 0)
            return;

        ClientSortBy? requested = gridClients.Columns[e.ColumnIndex].Name switch
        {
            nameof(colClientName) => ClientSortBy.DisplayName,
            nameof(colClientType) => ClientSortBy.ClientTypeCode,
            nameof(colClientStatus) => ClientSortBy.StatusCode,
            _ => null
        };

        if (requested is not { } column)
            return;

        // Re-clicking the sorted column toggles direction; a new column starts
        // ascending.
        if (_sortBy == column)
        {
            _sortDirection = _sortDirection == ClientSortDirection.Ascending
                ? ClientSortDirection.Descending
                : ClientSortDirection.Ascending;
        }
        else
        {
            _sortBy = column;
            _sortDirection = ClientSortDirection.Ascending;
        }

        UpdateSortIndicators();

        _currentPage = 1;
        RefreshBrowser();
    }

    private void ConfigureAccessibility()
    {
        txtSearch.AccessibleName = "Search clients";
        btnClearSearch.AccessibleName = "Clear search";
        lblClientsHeading.AccessibleName = "Clients";
        lblNoResults.AccessibleName = "No search results";
        cmbClientTypeFilter.AccessibleName = "Client type filter";
        cmbStatusFilter.AccessibleName = "Status filter";
        btnCreateClient.AccessibleName = "Create client";
        gridClients.AccessibleName = "Client list";
        btnPreviousPage.AccessibleName = "Previous page";
        btnNextPage.AccessibleName = "Next page";
        btnSave.AccessibleName = "Save client";
        btnCancel.AccessibleName = "Cancel client changes";

        toolTip.SetToolTip(cmbClientTypeValue, "Client type cannot be changed after the client is created.");
        toolTip.SetToolTip(lblClientTypeLock, "Client type cannot be changed after the client is created.");

        AcceptButton = btnSave;
        CancelButton = btnCancel;
    }

    private IReadOnlyList<ClientListItem> GetFilteredClients()
    {
        IEnumerable<ClientListItem> query = _allClients;

        string search = txtSearch.Text.Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            // Matches the server-side search, which targets display name and
            // email only.
            query = query.Where(c =>
                Contains(c.DisplayName, search) ||
                Contains(c.Email, search));
        }

        if (cmbClientTypeFilter.SelectedItem is string type && !string.Equals(type, AllTypes, StringComparison.Ordinal))
            query = query.Where(c => string.Equals(c.ClientType, type, StringComparison.OrdinalIgnoreCase));

        if (cmbStatusFilter.SelectedItem is string status && !string.Equals(status, AllStatuses, StringComparison.Ordinal))
            query = query.Where(c => string.Equals(c.Status, status, StringComparison.OrdinalIgnoreCase));

        return ApplySort(query).ToList();
    }

    private IEnumerable<ClientListItem> ApplySort(IEnumerable<ClientListItem> query)
    {
        bool ascending = _sortDirection == ClientSortDirection.Ascending;

        // Secondary sort on display name keeps paging stable when the primary
        // key repeats, which Type and Status both do.
        return _sortBy switch
        {
            ClientSortBy.ClientTypeCode => ascending
                ? query.OrderBy(c => c.ClientType, StringComparer.CurrentCultureIgnoreCase)
                       .ThenBy(c => c.DisplayName, StringComparer.CurrentCultureIgnoreCase)
                : query.OrderByDescending(c => c.ClientType, StringComparer.CurrentCultureIgnoreCase)
                       .ThenBy(c => c.DisplayName, StringComparer.CurrentCultureIgnoreCase),
            ClientSortBy.StatusCode => ascending
                ? query.OrderBy(c => c.Status, StringComparer.CurrentCultureIgnoreCase)
                       .ThenBy(c => c.DisplayName, StringComparer.CurrentCultureIgnoreCase)
                : query.OrderByDescending(c => c.Status, StringComparer.CurrentCultureIgnoreCase)
                       .ThenBy(c => c.DisplayName, StringComparer.CurrentCultureIgnoreCase),
            _ => ascending
                ? query.OrderBy(c => c.DisplayName, StringComparer.CurrentCultureIgnoreCase)
                : query.OrderByDescending(c => c.DisplayName, StringComparer.CurrentCultureIgnoreCase)
        };
    }

    private static bool Contains(string? value, string search) =>
        value is not null && value.Contains(search, StringComparison.OrdinalIgnoreCase);

    private void RefreshBrowser()
    {
        if (_apiClient is not null)
        {
            // Server-backed: filtering, sorting and paging all happen in the
            // database, so fire a reload rather than filtering locally.
            _ = ReloadFromServerSafelyAsync();
            return;
        }

        IReadOnlyList<ClientListItem> filtered = GetFilteredClients();
        int pageCount = Math.Max(1, (int)Math.Ceiling(filtered.Count / (double)PageSize));
        _currentPage = Math.Clamp(_currentPage, 1, pageCount);

        _pageItems.RaiseListChangedEvents = false;
        _pageItems.Clear();

        foreach (ClientListItem item in filtered.Skip((_currentPage - 1) * PageSize).Take(PageSize))
            _pageItems.Add(item);

        _pageItems.RaiseListChangedEvents = true;
        _pageItems.ResetBindings();

        _totalCount = filtered.Count;
        _totalPages = pageCount;

        UpdateEmptyState(filtered.Count);
        UpdatePagination(filtered.Count, pageCount);
    }

    private async Task ReloadFromServerSafelyAsync()
    {
        try
        {
            await ReloadFromServerAsync().ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // A newer reload superseded this one.
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                $"Unable to load clients.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                "Clients",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async Task ReloadFromServerAsync()
    {
        if (_apiClient is null)
            return;

        // Cancel any reload still in flight so rapid typing in the search box
        // cannot apply results out of order.
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();

        CancellationToken cancellationToken = _loadCts.Token;

        var query = new ClientListQuery
        {
            Search = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim(),
            ClientTypeCode = cmbClientTypeFilter.SelectedItem is string type
                && !string.Equals(type, AllTypes, StringComparison.Ordinal)
                    ? type
                    : null,
            StatusCode = cmbStatusFilter.SelectedItem is string status
                && !string.Equals(status, AllStatuses, StringComparison.Ordinal)
                    ? status
                    : ApiAllStatuses,
            SortBy = _sortBy,
            SortDirection = _sortDirection,
            Page = _currentPage,
            PageSize = PageSize
        };

        var result = await _apiClient
            .ListAsync(query, cancellationToken)
            .ConfigureAwait(true);

        cancellationToken.ThrowIfCancellationRequested();

        _pageItems.RaiseListChangedEvents = false;
        _pageItems.Clear();

        foreach (ClientListItemDto dto in result.Items)
            _pageItems.Add(ToListItem(dto));

        _pageItems.RaiseListChangedEvents = true;
        _pageItems.ResetBindings();

        _totalCount = (int)result.TotalCount;
        _totalPages = Math.Max(1, result.TotalPages);

        UpdateEmptyState(result.Items.Count);
        UpdatePagination(_totalCount, _totalPages);
    }

    private static ClientListItem ToListItem(ClientListItemDto dto) =>
        new()
        {
            ClientId = dto.ClientId,
            DisplayName = dto.DisplayName,
            Email = dto.Email,
            ClientType = dto.ClientTypeCode,
            Status = dto.StatusCode
        };

    /// <summary>
    /// Shows the no-results message in place of the grid when nothing matches
    /// the current search and filter combination.
    /// </summary>
    private void UpdateEmptyState(int matchCount)
    {
        bool hasMatches = matchCount > 0;

        gridClients.Visible = hasMatches;
        lblNoResults.Visible = !hasMatches;

        if (hasMatches)
            return;

        bool hasSearchText = !string.IsNullOrWhiteSpace(txtSearch.Text);

        lblNoResults.Text = hasSearchText
            ? $"No clients match \u201C{txtSearch.Text.Trim()}\u201D."
            : "No clients match the selected filters.";
    }

    private void UpdatePagination(int totalCount, int pageCount)
    {
        Button[] pageButtons = { btnPage1, btnPage2, btnPage3, btnPage4, btnPage5 };
        int firstPage = Math.Max(1, Math.Min(_currentPage - 2, pageCount - pageButtons.Length + 1));

        for (int i = 0; i < pageButtons.Length; i++)
        {
            int pageNumber = firstPage + i;
            Button button = pageButtons[i];
            button.Visible = pageNumber <= pageCount;
            button.Text = pageNumber.ToString(CultureInfo.CurrentCulture);
            button.Tag = pageNumber;

            bool selected = pageNumber == _currentPage;
            button.BackColor = selected ? NexusTheme.Primary : NexusTheme.Surface;
            button.ForeColor = selected ? NexusTheme.White : NexusTheme.TextStrong;
        }

        btnPreviousPage.Enabled = _currentPage > 1;
        btnNextPage.Enabled = _currentPage < pageCount;

        int first = totalCount == 0 ? 0 : ((_currentPage - 1) * PageSize) + 1;
        int last = Math.Min(_currentPage * PageSize, totalCount);
        lblPageRange.Text = $"{first}-{last} of {totalCount}";
    }

    private void UpdateDetail()
    {
        _binding = true;

        try
        {
            bool hasClient = _current is not null;

            tabClient.Enabled = hasClient;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            if (_current is null)
            {
                lblClientHeading.Text = "No client selected";
                lblClientSummary.Text = "-";
                lblClientStatusBadge.Text = "-";
                lblClientStatusBadge.ForeColor = NexusTheme.TextMuted;
                lblLastUpdated.Text = string.Empty;
                ClearProfile();
                return;
            }

            lblClientHeading.Text = _current.DisplayName;
            lblClientSummary.Text = string.Join("  |  ", new[]
            {
                _current.ClientType,
                _current.Email,
                _current.Phone
            }.Where(part => !string.IsNullOrWhiteSpace(part)));

            lblClientStatusBadge.Text = $"\u25CF  {_current.Status}";
            lblClientStatusBadge.ForeColor = string.Equals(_current.Status, "Active", StringComparison.OrdinalIgnoreCase)
                ? NexusTheme.Success
                : NexusTheme.TextMuted;

            lblLastUpdated.Text = _current.LastUpdatedUtc is { } updated
                ? $"Last updated {updated.ToLocalTime().ToString("MMM d, yyyy h:mm tt", CultureInfo.CurrentCulture)}"
                : string.Empty;

            cmbClientTypeValue.SelectedItem = _current.ClientType;
            txtFirstName.Text = _current.FirstName;
            txtMiddleName.Text = _current.MiddleName;
            txtLastName.Text = _current.LastName;
            cmbSuffix.SelectedItem = _current.Suffix ?? string.Empty;
            txtDisplayName.Text = _current.DisplayName;
            txtEmail.Text = _current.Email;
            txtPhone.Text = _current.Phone;
            txtAddressLine1.Text = _current.AddressLine1;
            txtAddressLine2.Text = _current.AddressLine2;
            txtCity.Text = _current.City;
            cmbState.SelectedItem = _current.State;
            txtZipCode.Text = _current.ZipCode;
            cmbPreferredMethod.SelectedItem = _current.PreferredContactMethod ?? "Email";

            UpdateEmailAvailability();
        }
        finally
        {
            _binding = false;
            _dirty = false;
            errorProvider.Clear();
        }
    }

    private void ClearProfile()
    {
        cmbClientTypeValue.SelectedIndex = -1;
        cmbSuffix.SelectedIndex = -1;
        cmbState.SelectedIndex = -1;
        cmbPreferredMethod.SelectedIndex = -1;
        lblEmailAvailability.Text = string.Empty;

        foreach (TextBox input in new[]
        {
            txtFirstName, txtMiddleName, txtLastName, txtDisplayName,
            txtEmail, txtPhone, txtAddressLine1, txtAddressLine2, txtCity, txtZipCode
        })
        {
            input.Clear();
        }
    }

    private void UpdateEmailAvailability()
    {
        string email = txtEmail.Text.Trim();

        if (string.IsNullOrWhiteSpace(email) ||
            cmbPreferredMethod.SelectedItem is not string method ||
            !string.Equals(method, "Email", StringComparison.OrdinalIgnoreCase))
        {
            lblEmailAvailability.Text = string.Empty;
            return;
        }

        lblEmailAvailability.ForeColor = NexusTheme.Success;
        lblEmailAvailability.Text = "\u2714  Email address is available.";
    }

    private void MarkDirty()
    {
        if (_binding || _current is null)
            return;

        _dirty = true;
        btnSave.Enabled = true;
        btnCancel.Enabled = true;
    }

    private bool ValidateProfile()
    {
        errorProvider.Clear();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
        {
            errorProvider.SetError(txtFirstName, "First Name is required.");
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(txtLastName.Text))
        {
            errorProvider.SetError(txtLastName, "Last Name is required.");
            valid = false;
        }

        return valid;
    }

    private void ApplyProfileToCurrent()
    {
        if (_current is null)
            return;

        _current.FirstName = txtFirstName.Text.Trim();
        _current.MiddleName = txtMiddleName.Text.Trim();
        _current.LastName = txtLastName.Text.Trim();
        _current.Suffix = cmbSuffix.SelectedItem as string;
        _current.DisplayName = string.IsNullOrWhiteSpace(txtDisplayName.Text)
            ? $"{_current.FirstName} {_current.LastName}".Trim()
            : txtDisplayName.Text.Trim();
        _current.Email = txtEmail.Text.Trim();
        _current.Phone = txtPhone.Text.Trim();
        _current.AddressLine1 = txtAddressLine1.Text.Trim();
        _current.AddressLine2 = txtAddressLine2.Text.Trim();
        _current.City = txtCity.Text.Trim();
        _current.State = cmbState.SelectedItem as string;
        _current.ZipCode = txtZipCode.Text.Trim();
        _current.PreferredContactMethod = cmbPreferredMethod.SelectedItem as string;
        _current.LastUpdatedUtc = DateTime.UtcNow;
    }

    private bool ConfirmDiscardChanges()
    {
        if (!_dirty)
            return true;

        DialogResult result = MessageBox.Show(
            this,
            "Discard unsaved changes to this client?",
            "Unsaved Changes",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        return result == DialogResult.Yes;
    }

    private void Filter_Changed(object? sender, EventArgs e)
    {
        // The clear affordance is only meaningful when there is text to clear.
        btnClearSearch.Visible = txtSearch.Text.Length > 0;

        _currentPage = 1;
        RefreshBrowser();
    }

    private void BtnClearSearch_Click(object? sender, EventArgs e)
    {
        txtSearch.Clear();
        txtSearch.Focus();
    }

    private void ProfileField_Changed(object? sender, EventArgs e)
    {
        MarkDirty();

        if (!_binding && sender == txtEmail)
            UpdateEmailAvailability();
    }

    private void CmbPreferredMethod_SelectedIndexChanged(object? sender, EventArgs e)
    {
        MarkDirty();

        if (!_binding)
            UpdateEmailAvailability();
    }

    private void GridClients_SelectionChanged(object? sender, EventArgs e)
    {
        if (_binding)
            return;

        ClientListItem? selected = gridClients.CurrentRow?.DataBoundItem as ClientListItem;
        if (ReferenceEquals(selected, _current))
            return;

        if (!ConfirmDiscardChanges())
            return;

        _current = selected;
        UpdateDetail();
    }

    private void PageNumber_Click(object? sender, EventArgs e)
    {
        if (sender is not Button { Tag: int pageNumber })
            return;

        _currentPage = pageNumber;
        RefreshBrowser();
    }

    private void BtnPreviousPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage <= 1)
            return;

        _currentPage--;
        RefreshBrowser();
    }

    private void BtnNextPage_Click(object? sender, EventArgs e)
    {
        _currentPage++;
        RefreshBrowser();
    }

    private void BtnCreateClient_Click(object? sender, EventArgs e)
    {
        if (!ConfirmDiscardChanges())
            return;

        CreateClientRequested?.Invoke(this, EventArgs.Empty);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_current is null || !ValidateProfile())
            return;

        ApplyProfileToCurrent();
        _dirty = false;
        btnSave.Enabled = false;
        btnCancel.Enabled = false;

        RefreshBrowser();
        UpdateDetail();

        ClientSaved?.Invoke(this, new ClientSavedEventArgs(_current));
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        if (!ConfirmDiscardChanges())
            return;

        UpdateDetail();
    }

    private static readonly object[] UsStates =
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN",
        "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH",
        "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT",
        "VT", "VA", "WA", "WV", "WI", "WY"
    };

    /// <summary>Editable client row backing both the browser list and the profile form.</summary>
    public sealed class ClientListItem
    {
        public Guid ClientId { get; init; }

        public string ClientType { get; init; } = "Individual";

        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string? Suffix { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string? Organization { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? PreferredContactMethod { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime? LastUpdatedUtc { get; set; }
    }

    /// <summary>Carries the client that was saved from the workspace.</summary>
    public sealed class ClientSavedEventArgs : EventArgs
    {
        public ClientSavedEventArgs(ClientListItem client)
        {
            ArgumentNullException.ThrowIfNull(client);
            Client = client;
        }

        public ClientListItem Client { get; }
    }
}