using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Services.Api;
using ONE_Classics_Nexus.WinForms.Theming;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

public sealed partial class AppraisalRequestListForm : Form
{
    private const string AllClientsLabel = "All Clients";

    private readonly AppraisalRequestApiClient _api;
    private IReadOnlyList<AppraisalRequestSummary> _items = [];

    public AppraisalRequestListForm(AppraisalRequestApiClient api)
    {
        ArgumentNullException.ThrowIfNull(api);

        _api = api;

        InitializeComponent();
        ApplyTheme();
        SeedFilters();
        WireEvents();
    }

    private void ApplyTheme()
    {
        BackColor = NexusTheme.SurfaceAlt;
        ForeColor = NexusTheme.TextStrong;
        Font = NexusTheme.BodyFont;

        btnNewRequest.BackColor = NexusTheme.Primary;
        btnNewRequest.ForeColor = NexusTheme.White;
        btnRefresh.BackColor = NexusTheme.Surface;
        btnRefresh.ForeColor = NexusTheme.Primary;

        foreach (var button in new[] { btnNewRequest, btnRefresh, btnPrevPage, btnPageOne, btnPageTwo, btnNextPage })
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 218);
        }

        gridRequests.ColumnHeadersDefaultCellStyle.Font = NexusTheme.LabelFont;
        gridRequests.EnableHeadersVisualStyles = false;
        gridRequests.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(246, 248, 251);
        gridRequests.ColumnHeadersDefaultCellStyle.ForeColor = NexusTheme.TextStrong;
    }

    private void SeedFilters()
    {
        cboStatus.Items.AddRange([
            "All Statuses",
            "Draft",
            "Submitted",
            "UnderReview",
            "Accepted",
            "Converted",
            "Declined",
            "Cancelled",
            "Withdrawn"]);

        RebuildClientOptions(null);
        cboDateRange.Items.AddRange(["Requested Date", "Received Date"]);
        cboSort.Items.AddRange(["Requested Date (Desc)", "Requested Date (Asc)", "Updated (Desc)", "Updated (Asc)"]);

        cboStatus.SelectedIndex = 0;
        cboClient.SelectedIndex = 0;
        cboDateRange.SelectedIndex = 0;
        cboSort.SelectedIndex = 0;

        dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
    }

    private void WireEvents()
    {
        Shown += AppraisalRequestListForm_Shown;
        btnRefresh.Click += BtnRefresh_Click;
        cboStatus.SelectedIndexChanged += CboStatus_SelectedIndexChanged;
        cboClient.SelectedIndexChanged += (_, _) => ApplyClientFiltering();
        cboDateRange.SelectedIndexChanged += (_, _) => ApplyClientFiltering();
        dtpFrom.ValueChanged += (_, _) => ApplyClientFiltering();
        dtpTo.ValueChanged += (_, _) => ApplyClientFiltering();
        txtSearch.TextChanged += (_, _) => ApplyClientFiltering();
        cboSort.SelectedIndexChanged += (_, _) => ApplyClientFiltering();
    }

    private async void AppraisalRequestListForm_Shown(object? sender, EventArgs e) =>
        await LoadRowsSafelyAsync().ConfigureAwait(true);

    private async void BtnRefresh_Click(object? sender, EventArgs e) =>
        await LoadRowsSafelyAsync().ConfigureAwait(true);

    private async void CboStatus_SelectedIndexChanged(object? sender, EventArgs e) =>
        await LoadRowsSafelyAsync().ConfigureAwait(true);

    private async Task LoadRowsSafelyAsync()
    {
        try
        {
            await LoadRowsAsync().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                $"Unable to load appraisal requests.\n\n{ex.Message}",
                "Appraisal Requests",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async Task LoadRowsAsync(CancellationToken cancellationToken = default)
    {
        var selectedStatus = cboStatus.SelectedItem?.ToString();
        var status = selectedStatus switch
        {
            null => null,
            "All Statuses" => null,
            _ => selectedStatus
        };

        var result = await _api.SearchAsync(status, cancellationToken).ConfigureAwait(true);
        _items = result?.Items ?? [];

        RebuildClientOptions((cboClient.SelectedItem as ClientFilterOption)?.ClientId);

        lblRequestCount.Text = $"{_items.Count} requests found";
        lblPaging.Text = _items.Count == 0 ? "Showing 0 - 0 of 0" : $"Showing 1 - {_items.Count} of {_items.Count}";

        ApplyClientFiltering();
    }

    private void ApplyClientFiltering()
    {
        IEnumerable<AppraisalRequestSummary> query = _items;

        var selectedClientId = (cboClient.SelectedItem as ClientFilterOption)?.ClientId;
        if (selectedClientId.HasValue)
            query = query.Where(item => item.ClientId == selectedClientId.Value);

        var fromDate = dtpFrom.Value.Date;
        var toDateExclusive = dtpTo.Value.Date.AddDays(1);

        query = cboDateRange.SelectedItem?.ToString() switch
        {
            "Requested Date" => query.Where(item =>
                item.RequestedCompletionDate.HasValue &&
                item.RequestedCompletionDate.Value.ToDateTime(TimeOnly.MinValue) >= fromDate &&
                item.RequestedCompletionDate.Value.ToDateTime(TimeOnly.MinValue) < toDateExclusive),
            _ => query.Where(item =>
                item.ReceivedAtUtc.ToLocalTime() >= fromDate &&
                item.ReceivedAtUtc.ToLocalTime() < toDateExclusive)
        };

        var search = txtSearch.Text.Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item =>
                item.RequestNumber.Contains(search, StringComparison.OrdinalIgnoreCase)
                || (item.SubjectMake?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                || (item.SubjectModel?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                || (item.SubjectVin?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        query = cboSort.SelectedItem?.ToString() switch
        {
            "Requested Date (Asc)" => query.OrderBy(x => x.RequestedCompletionDate),
            "Updated (Asc)" => query.OrderBy(x => x.ReceivedAtUtc),
            "Updated (Desc)" => query.OrderByDescending(x => x.ReceivedAtUtc),
            _ => query.OrderByDescending(x => x.RequestedCompletionDate)
        };

        var rows = query.Select(x => new RequestListRow(
            x.RequestNumber,
            BuildVehicleDisplay(x),
            x.RequestStatus,
            x.RequestedCompletionDate?.ToString("M/d/yyyy") ?? string.Empty,
            x.ReceivedAtUtc.ToLocalTime().ToString("M/d h:mm tt")))
            .ToList();

        gridRequests.DataSource = rows;
        lblRequestCount.Text = $"{rows.Count} requests found";
        lblPaging.Text = rows.Count == 0 ? "Showing 0 - 0 of 0" : $"Showing 1 - {rows.Count} of {rows.Count}";
    }

    private void RebuildClientOptions(Guid? selectedClientId)
    {
        var options = _items
            .Select(item => item.ClientId)
            .Distinct()
            .OrderBy(id => id)
            .Select(id => new ClientFilterOption(id, id.ToString("D")))
            .ToList();

        options.Insert(0, new ClientFilterOption(null, AllClientsLabel));

        cboClient.BeginUpdate();
        try
        {
            cboClient.Items.Clear();
            cboClient.Items.AddRange(options.Cast<object>().ToArray());

            var selected = options.FirstOrDefault(option => option.ClientId == selectedClientId) ?? options[0];
            cboClient.SelectedItem = selected;
        }
        finally
        {
            cboClient.EndUpdate();
        }
    }

    private static string BuildVehicleDisplay(AppraisalRequestSummary summary)
    {
        var parts = new List<string>();
        if (summary.SubjectYear.HasValue)
            parts.Add(summary.SubjectYear.Value.ToString());
        if (!string.IsNullOrWhiteSpace(summary.SubjectMake))
            parts.Add(summary.SubjectMake);
        if (!string.IsNullOrWhiteSpace(summary.SubjectModel))
            parts.Add(summary.SubjectModel);

        return string.Join(' ', parts);
    }

    private void gridRequests_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private sealed record RequestListRow(
        string Request,
        string Vehicle,
        string Status,
        string Requested,
        string Updated);

    private sealed record ClientFilterOption(Guid? ClientId, string Label)
    {
        public override string ToString() => Label;
    }
}
