using ONE_Classics_Nexus.Domain.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Models.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Services.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

public partial class AppraisalRequestWorkspaceForm : Form
{
    private static readonly Color NexusNavy = Color.FromArgb(14, 40, 65);
    private static readonly Color NexusBlue = Color.FromArgb(8, 119, 220);
    private static readonly Color SelectedRowBlue = Color.FromArgb(220, 239, 252);

    private readonly IAppraisalRequestApiClient _apiClient;
    private readonly BindingList<AppraisalRequestListItemDto> _requests = [];
    private readonly BindingList<ValidationIssueDto> _validationIssues = [];
    private readonly BindingList<IntendedUserDto> _intendedUsers = [];
    private readonly BindingList<AttachmentDto> _attachments = [];
    private readonly BindingList<AuditEntryDto> _history = [];
    private readonly System.Windows.Forms.Timer _searchDebounceTimer;

    private CancellationTokenSource? _operationCts;
    private AppraisalRequestDetailDto? _currentRequest;
    private int _currentPage = 1;
    private int _totalPages = 1;
    private bool _suppressEvents;
    private bool _isDirty;
    private bool _referenceDataLoaded;

    public event EventHandler<AppraisalRequestCommandEventArgs>? VehicleSelectionRequested;
    public event EventHandler<AppraisalRequestCommandEventArgs>? AddIntendedUserRequested;
    public event EventHandler<AppraisalRequestCommandEventArgs>? EditIntendedUserRequested;
    public event EventHandler<AppraisalRequestCommandEventArgs>? DeactivateIntendedUserRequested;
    public event EventHandler<AppraisalRequestCommandEventArgs>? AddAttachmentRequested;

    public AppraisalRequestWorkspaceForm(IAppraisalRequestApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

        InitializeComponent();
        ConfigureRuntimeBindings();
        ApplyNexusTheme();

        _searchDebounceTimer = new System.Windows.Forms.Timer(components!)
        {
            Interval = 350
        };
        _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
    }

    private void ConfigureRuntimeBindings()
    {
        dgvRequests.AutoGenerateColumns = false;
        dgvRequests.DataSource = _requests;

        dgvValidation.AutoGenerateColumns = false;
        dgvValidation.DataSource = _validationIssues;

        dgvIntendedUsers.AutoGenerateColumns = false;
        dgvIntendedUsers.DataSource = _intendedUsers;

        dgvAttachments.AutoGenerateColumns = false;
        dgvAttachments.DataSource = _attachments;

        dgvHistory.AutoGenerateColumns = false;
        dgvHistory.DataSource = _history;

        cmbStatusFilter.Items.Add("All Statuses");
        foreach (var status in AppraisalRequestStatuses.All)
            cmbStatusFilter.Items.Add(status);
        cmbStatusFilter.SelectedIndex = 0;

        cmbDateFilter.Items.AddRange(["Requested Date", "Updated Date"]);
        cmbDateFilter.SelectedIndex = 0;

        cmbSort.Items.AddRange(["requestedDate:desc", "updatedAt:desc", "client:asc"]);
        cmbSort.SelectedIndex = 0;

        cmbPriority.Items.AddRange(["Low", "Normal", "High"]);
        cmbPriority.SelectedItem = "Normal";

        dtpRequestedFrom.Value = new DateTime(DateTime.UtcNow.Year, 1, 1);
        dtpRequestedTo.Value = DateTime.Today.AddDays(1).AddTicks(-1);
        dtpEffectiveDate.ShowCheckBox = true;
    }

    private void ApplyNexusTheme()
    {
        BackColor = Color.FromArgb(246, 248, 250);
        pnlTitle.BackColor = Color.White;
        lblTitleIcon.ForeColor = NexusNavy;
        lblTitle.ForeColor = Color.FromArgb(19, 35, 58);

        btnNew.BackColor = NexusBlue;
        btnNew.ForeColor = Color.White;
        btnSave.BackColor = NexusBlue;
        btnSave.ForeColor = Color.White;

        dgvRequests.DefaultCellStyle.SelectionBackColor = SelectedRowBlue;
        dgvRequests.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 36, 58);
        dgvRequests.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 248);
        dgvRequests.EnableHeadersVisualStyles = false;

        foreach (var grid in new[] { dgvValidation, dgvIntendedUsers, dgvAttachments, dgvHistory })
        {
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 248);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(35, 56, 78);
            grid.EnableHeadersVisualStyles = false;
        }
    }

    private async void AppraisalRequestWorkspaceForm_Load(object? sender, EventArgs e)
    {
        try
        {
            SetBusy(true, "Loading reference data...");
            await LoadReferenceDataAsync().ConfigureAwait(true);
            await RefreshListAsync(preserveSelection: false).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to load the Appraisal Request workspace.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _searchDebounceTimer.Stop();
        CancelPendingOperation();
        base.OnFormClosed(e);
    }

    private async Task LoadReferenceDataAsync()
    {
        var linkedCts = ReplaceOperationToken();
        var cancellationToken = linkedCts.Token;

        var clientsTask = _apiClient.ListClientsAsync(cancellationToken);
        var usesTask = _apiClient.ListIntendedUsesAsync(cancellationToken);
        var valuesTask = _apiClient.ListValueDefinitionsAsync(cancellationToken);
        var typesTask = _apiClient.ListAppraisalTypesAsync(cancellationToken);

        await Task.WhenAll(clientsTask, usesTask, valuesTask, typesTask).ConfigureAwait(true);

        _suppressEvents = true;
        try
        {
            BindReferenceOptions(cmbClientFilter, clientsTask.Result, includeEmpty: true, emptyText: "All Clients");
            BindReferenceOptions(cmbClient, clientsTask.Result, includeEmpty: true, emptyText: "Select client");
            BindReferenceOptions(cmbIntendedUse, usesTask.Result, includeEmpty: true, emptyText: "Select intended use");
            BindReferenceOptions(cmbValueDefinition, valuesTask.Result, includeEmpty: true, emptyText: "Select value definition");
            BindReferenceOptions(cmbAppraisalType, typesTask.Result, includeEmpty: true, emptyText: "Select appraisal type");
            _referenceDataLoaded = true;
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    private async Task RefreshListAsync(bool preserveSelection = true)
    {
        var selectedId = preserveSelection ? _currentRequest?.AppraisalRequestId : null;
        var linkedCts = ReplaceOperationToken();
        var query = new AppraisalRequestListQuery(
            Query: NullIfWhiteSpace(txtSearch.Text),
            Status: cmbStatusFilter.SelectedIndex <= 0 ? null : cmbStatusFilter.Text,
            ClientId: GetSelectedReferenceId(cmbClientFilter),
            RequestedFromUtc: dtpRequestedFrom.Value.Date.ToUniversalTime(),
            RequestedToUtc: dtpRequestedTo.Value.Date.AddDays(1).AddTicks(-1).ToUniversalTime(),
            Sort: cmbSort.SelectedItem?.ToString() ?? "requestedDate:desc",
            Page: _currentPage,
            PageSize: 25);

        SetBusy(true, "Loading Appraisal Requests...");
        try
        {
            var result = await _apiClient.ListAsync(query, linkedCts.Token).ConfigureAwait(true);

            _suppressEvents = true;
            _requests.RaiseListChangedEvents = false;
            _requests.Clear();
            foreach (var request in result.Items)
                _requests.Add(request);
            _requests.RaiseListChangedEvents = true;
            _requests.ResetBindings();

            _currentPage = result.Page;
            _totalPages = result.TotalPages;
            lblResultCount.Text = $"{result.TotalCount:N0} requests found";
            lblPageSummary.Text = result.TotalCount == 0
                ? "No requests"
                : $"Page {_currentPage:N0} of {_totalPages:N0}";
            btnPreviousPage.Enabled = _currentPage > 1;
            btnNextPage.Enabled = _currentPage < _totalPages;

            RestoreSelection(selectedId);
        }
        finally
        {
            _suppressEvents = false;
            SetBusy(false);
        }

        if (dgvRequests.SelectedRows.Count == 0 && _requests.Count > 0)
        {
            dgvRequests.Rows[0].Selected = true;
            await LoadSelectedRequestAsync().ConfigureAwait(true);
        }
        else if (selectedId.HasValue)
        {
            await LoadSelectedRequestAsync().ConfigureAwait(true);
        }
    }

    private async Task LoadSelectedRequestAsync()
    {
        if (_suppressEvents || dgvRequests.SelectedRows.Count == 0)
            return;

        if (dgvRequests.SelectedRows[0].DataBoundItem is not AppraisalRequestListItemDto item)
            return;

        if (_isDirty && !ConfirmDiscardChanges())
        {
            RestoreSelection(_currentRequest?.AppraisalRequestId);
            return;
        }

        SetBusy(true, $"Loading {item.RequestNumber}...");
        try
        {
            var linkedCts = ReplaceOperationToken();
            var detail = await _apiClient.GetAsync(item.AppraisalRequestId, linkedCts.Token).ConfigureAwait(true);
            await DisplayRequestAsync(detail).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // A later selection replaced this request.
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to load the selected Appraisal Request.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task DisplayRequestAsync(AppraisalRequestDetailDto detail)
    {
        _suppressEvents = true;
        try
        {
            _currentRequest = detail;
            var workflow = AppraisalRequestWorkflow.FromStatus(detail.Status);

            lblRequestNumber.Text = detail.RequestNumber;
            lblStatus.Text = $"{FormatStatus(detail.Status)} | {workflow.Phase}";
            lblStatus.AccessibleDescription = $"Workflow phase: {workflow.Phase}. Data collection area: {workflow.DataCollectionAreaPath}";
            lblRowVersion.Text = $"Row Version: {detail.RowVersion:N0}";
            lblAudit.Text = $"Created: {detail.CreatedAtUtc.ToLocalTime():g}   |   Updated: {detail.UpdatedAtUtc.ToLocalTime():g}";

            SelectReference(cmbClient, detail.ClientId);
            await LoadContactsForClientAsync(detail.ClientId, detail.PrimaryClientContactId).ConfigureAwait(true);
            SelectReference(cmbIntendedUse, detail.IntendedUseId);
            txtIntendedUseNarrative.Text = detail.IntendedUseNarrative ?? string.Empty;

            dtpEffectiveDate.Checked = detail.RequestedEffectiveDateUtc.HasValue;
            if (detail.RequestedEffectiveDateUtc.HasValue)
                dtpEffectiveDate.Value = detail.RequestedEffectiveDateUtc.Value.ToLocalTime();

            SelectReference(cmbValueDefinition, detail.RequestedValueDefinitionId);
            SelectReferenceByCode(cmbAppraisalType, detail.AppraisalTypeCode);
            cmbPriority.SelectedItem = detail.PriorityCode ?? "Normal";
            txtScopeSummary.Text = detail.RequestedScopeSummary ?? string.Empty;
            txtVehicleSummary.Text = detail.VehicleSummary ?? "Vehicle not selected";

            ReplaceBindingList(_intendedUsers, detail.IntendedUsers);
            ReplaceBindingList(_attachments, detail.Attachments);
            ReplaceBindingList(_history, detail.History);
            _validationIssues.Clear();

            lblValidationSummary.Text = "Validation has not been run for this view.";
            ApplyLifecyclePermissions(detail);
            SetDirty(false);
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        try
        {
            await RefreshListAsync().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to refresh the Appraisal Request list.");
        }
    }

    private async void btnNew_Click(object? sender, EventArgs e)
    {
        if (_isDirty && !ConfirmDiscardChanges())
            return;

        try
        {
            SetBusy(true, "Creating Draft request...");
            var linkedCts = ReplaceOperationToken();
            var created = await _apiClient.CreateAsync(
                new CreateAppraisalRequestRequest(
                    ClientId: null,
                    PrimaryClientContactId: null,
                    IntendedUseId: null,
                    IntendedUseNarrative: null,
                    RequestedEffectiveDateUtc: null,
                    RequestedEffectiveDatePrecision: "DATE_ONLY",
                    RequestedEffectiveDateTimeZone: TimeZoneInfo.Local.Id,
                    RequestedValueDefinitionId: null,
                    AppraisalTypeCode: null,
                    PriorityCode: "Normal",
                    RequestedScopeSummary: null,
                    VehicleId: null),
                linkedCts.Token).ConfigureAwait(true);

            _currentPage = 1;
            await RefreshListAsync(preserveSelection: false).ConfigureAwait(true);
            RestoreSelection(created.AppraisalRequestId);
            await DisplayRequestAsync(created).ConfigureAwait(true);
            tabWorkspace.SelectedTab = tabRequestInformation;
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to create the Draft Appraisal Request.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_currentRequest is null)
            return;

        if (!ValidateEditorFields())
            return;

        try
        {
            SetBusy(true, "Saving request...");
            var linkedCts = ReplaceOperationToken();
            var updated = await _apiClient.UpdateAsync(
                _currentRequest.AppraisalRequestId,
                BuildUpdateRequest(_currentRequest.RowVersion),
                linkedCts.Token).ConfigureAwait(true);

            await DisplayRequestAsync(updated).ConfigureAwait(true);
            UpdateCurrentListRow(updated);
            ShowStatusMessage("Appraisal Request saved.", isError: false);
        }
        catch (AppraisalRequestConcurrencyException ex)
        {
            await HandleConcurrencyConflictAsync(ex).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to save the Appraisal Request.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnValidate_Click(object? sender, EventArgs e)
    {
        if (_currentRequest is null)
            return;

        try
        {
            SetBusy(true, "Validating request...");
            var linkedCts = ReplaceOperationToken();
            var result = await _apiClient
                .ValidateAsync(_currentRequest.AppraisalRequestId, linkedCts.Token)
                .ConfigureAwait(true);
            DisplayValidation(result);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to validate the Appraisal Request.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnSubmit_Click(object? sender, EventArgs e)
    {
        if (_currentRequest is null)
            return;

        try
        {
            SetBusy(true, "Validating request...");
            var validateCts = ReplaceOperationToken();
            var validation = await _apiClient
                .ValidateAsync(_currentRequest.AppraisalRequestId, validateCts.Token)
                .ConfigureAwait(true);
            DisplayValidation(validation);

            if (validation.BlockingCount > 0)
            {
                MessageBox.Show(
                    this,
                    "Submission is blocked. Resolve the validation errors first.",
                    "Appraisal Request Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var confirmation = MessageBox.Show(
                this,
                "Submit this Appraisal Request and capture its submission snapshots?",
                "Submit Appraisal Request",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes)
                return;

            SetBusy(true, "Submitting request...");
            var submitCts = ReplaceOperationToken();
            var submitted = await _apiClient.SubmitAsync(
                _currentRequest.AppraisalRequestId,
                new ExpectedRowVersionRequest(_currentRequest.RowVersion),
                submitCts.Token).ConfigureAwait(true);

            await DisplayRequestAsync(submitted).ConfigureAwait(true);
            UpdateCurrentListRow(submitted);
            ShowStatusMessage("Appraisal Request submitted.", isError: false);
        }
        catch (AppraisalRequestConcurrencyException ex)
        {
            await HandleConcurrencyConflictAsync(ex).ConfigureAwait(true);
        }
        catch (AppraisalRequestApiException ex)
        {
            DisplayProblemIssues(ex.Problem);
            ShowError(ex, "Submission was rejected.");
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to submit the Appraisal Request.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void btnMoreActions_Click(object? sender, EventArgs e)
    {
        cmsMoreActions.Show(btnMoreActions, new Point(0, btnMoreActions.Height));
    }

    private async void mnuCancelRequest_Click(object? sender, EventArgs e)
    {
        await ApplyDispositionAsync(isWithdrawal: false).ConfigureAwait(true);
    }

    private async void mnuWithdrawRequest_Click(object? sender, EventArgs e)
    {
        await ApplyDispositionAsync(isWithdrawal: true).ConfigureAwait(true);
    }

    private async Task ApplyDispositionAsync(bool isWithdrawal)
    {
        if (_currentRequest is null)
            return;

        var actionName = isWithdrawal ? "Withdraw" : "Cancel";
        var reason = PromptForReason(actionName);
        if (reason is null)
            return;

        try
        {
            SetBusy(true, $"{actionName}ling request...");
            var linkedCts = ReplaceOperationToken();
            var request = new DispositionRequest(reason, _currentRequest.RowVersion);
            var result = isWithdrawal
                ? await _apiClient.WithdrawAsync(_currentRequest.AppraisalRequestId, request, linkedCts.Token).ConfigureAwait(true)
                : await _apiClient.CancelAsync(_currentRequest.AppraisalRequestId, request, linkedCts.Token).ConfigureAwait(true);

            await DisplayRequestAsync(result).ConfigureAwait(true);
            UpdateCurrentListRow(result);
            ShowStatusMessage(
                isWithdrawal ? "Appraisal Request withdrawn." : "Appraisal Request cancelled.",
                isError: false);
        }
        catch (AppraisalRequestConcurrencyException ex)
        {
            await HandleConcurrencyConflictAsync(ex).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            ShowError(ex, $"Unable to {actionName.ToLowerInvariant()} the Appraisal Request.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_currentRequest is null || !_currentRequest.CanHardDelete)
            return;

        var confirmation = MessageBox.Show(
            this,
            "Permanently delete this eligible, unreferenced Draft request? This action cannot be undone.",
            "Delete Draft Appraisal Request",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2);

        if (confirmation != DialogResult.Yes)
            return;

        try
        {
            SetBusy(true, "Deleting Draft request...");
            var linkedCts = ReplaceOperationToken();
            await _apiClient.DeleteDraftAsync(
                _currentRequest.AppraisalRequestId,
                new ExpectedRowVersionRequest(_currentRequest.RowVersion),
                linkedCts.Token).ConfigureAwait(true);

            _currentRequest = null;
            ClearEditor();
            await RefreshListAsync(preserveSelection: false).ConfigureAwait(true);
            ShowStatusMessage("Draft Appraisal Request deleted.", isError: false);
        }
        catch (AppraisalRequestConcurrencyException ex)
        {
            await HandleConcurrencyConflictAsync(ex).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to delete the Draft Appraisal Request.");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void dgvRequests_SelectionChanged(object? sender, EventArgs e)
    {
        await LoadSelectedRequestAsync().ConfigureAwait(true);
    }

    private void dgvRequests_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvRequests.Columns[e.ColumnIndex].Name != colStatus.Name || e.Value is not string status)
            return;

        if (e.CellStyle is null)
            return;

        e.Value = FormatStatus(status);
        e.CellStyle.ForeColor = status switch
        {
            AppraisalRequestStatuses.Submitted => Color.FromArgb(6, 90, 159),
            AppraisalRequestStatuses.UnderReview => Color.FromArgb(128, 83, 0),
            AppraisalRequestStatuses.Accepted => Color.FromArgb(75, 42, 154),
            AppraisalRequestStatuses.Converted => Color.FromArgb(18, 99, 49),
            _ => Color.FromArgb(39, 50, 62)
        };
        e.CellStyle.Font = new Font(dgvRequests.Font, FontStyle.Bold);
    }

    private async void FilterControl_Changed(object? sender, EventArgs e)
    {
        if (_suppressEvents || !_referenceDataLoaded)
            return;

        _currentPage = 1;
        try
        {
            await RefreshListAsync().ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // A newer filter request superseded this one.
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to apply the Appraisal Request filters.");
        }
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        if (_suppressEvents)
            return;

        _searchDebounceTimer.Stop();
        _searchDebounceTimer.Start();
    }

    private async void SearchDebounceTimer_Tick(object? sender, EventArgs e)
    {
        _searchDebounceTimer.Stop();
        _currentPage = 1;
        try
        {
            await RefreshListAsync().ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            // A newer search superseded this one.
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to search Appraisal Requests.");
        }
    }

    private async void btnPreviousPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage <= 1)
            return;
        _currentPage--;
        await RefreshListSafelyAsync().ConfigureAwait(true);
    }

    private async void btnNextPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage >= _totalPages)
            return;
        _currentPage++;
        await RefreshListSafelyAsync().ConfigureAwait(true);
    }

    private async Task RefreshListSafelyAsync()
    {
        try
        {
            await RefreshListAsync().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to change pages.");
        }
    }

    private async void cmbClient_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressEvents)
            return;

        SetDirty(true);
        await LoadContactsForClientAsync(GetSelectedReferenceId(cmbClient), selectedContactId: null).ConfigureAwait(true);
    }

    private void EditorControl_Changed(object? sender, EventArgs e)
    {
        if (!_suppressEvents)
            SetDirty(true);
    }

    private async void btnCancelEdit_Click(object? sender, EventArgs e)
    {
        if (_currentRequest is not null)
            await DisplayRequestAsync(_currentRequest).ConfigureAwait(true);
    }

    private async Task LoadContactsForClientAsync(Guid? clientId, Guid? selectedContactId)
    {
        if (!clientId.HasValue || clientId == Guid.Empty)
        {
            BindReferenceOptions(cmbPrimaryContact, [], includeEmpty: true, emptyText: "Select contact");
            return;
        }

        try
        {
            var contacts = await _apiClient.ListClientContactsAsync(clientId.Value).ConfigureAwait(true);
            var previousSuppress = _suppressEvents;
            _suppressEvents = true;
            try
            {
                BindReferenceOptions(cmbPrimaryContact, contacts, includeEmpty: true, emptyText: "Select contact");
                SelectReference(cmbPrimaryContact, selectedContactId);
            }
            finally
            {
                _suppressEvents = previousSuppress;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex, "Unable to load Client Contacts.");
        }
    }

    private ONE_Classics_Nexus.WinForms.Models.AppraisalRequests.UpdateAppraisalRequestRequest BuildUpdateRequest(long expectedRowVersion) =>
        new(
            ClientId: GetSelectedReferenceId(cmbClient),
            PrimaryClientContactId: GetSelectedReferenceId(cmbPrimaryContact),
            IntendedUseId: GetSelectedReferenceId(cmbIntendedUse),
            IntendedUseNarrative: NullIfWhiteSpace(txtIntendedUseNarrative.Text),
            RequestedEffectiveDateUtc: dtpEffectiveDate.Checked
                ? DateTime.SpecifyKind(dtpEffectiveDate.Value.Date, DateTimeKind.Local).ToUniversalTime()
                : null,
            RequestedEffectiveDatePrecision: "DATE_ONLY",
            RequestedEffectiveDateTimeZone: TimeZoneInfo.Local.Id,
            RequestedValueDefinitionId: GetSelectedReferenceId(cmbValueDefinition),
            AppraisalTypeCode: GetSelectedReferenceCode(cmbAppraisalType),
            PriorityCode: cmbPriority.SelectedItem?.ToString(),
            RequestedScopeSummary: NullIfWhiteSpace(txtScopeSummary.Text),
            VehicleId: _currentRequest?.VehicleId,
            ExpectedRowVersion: expectedRowVersion);

    private bool ValidateEditorFields()
    {
        errorProvider.Clear();
        var valid = true;

        if (!GetSelectedReferenceId(cmbClient).HasValue)
        {
            errorProvider.SetError(cmbClient, "Client is required.");
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(txtScopeSummary.Text))
        {
            errorProvider.SetError(txtScopeSummary, "Scope Summary is required.");
            valid = false;
        }

        return valid;
    }

    private void DisplayValidation(ValidationResponse response)
    {
        ReplaceBindingList(_validationIssues, response.Issues);
        lblValidationSummary.Text = $"{response.BlockingCount:N0} blocking issues   |   {response.WarningCount:N0} warnings";
        lblValidationSummary.ForeColor = response.BlockingCount > 0
            ? Color.FromArgb(183, 28, 49)
            : response.WarningCount > 0
                ? Color.FromArgb(182, 108, 0)
                : Color.FromArgb(21, 148, 74);
    }

    private void DisplayProblemIssues(ONE_Classics_Nexus.WinForms.Models.AppraisalRequests.ApiProblemDetails? problem)
    {
        if (problem?.Issues is { Count: > 0 })
            DisplayValidation(new ValidationResponse(problem.Issues));
    }

    private void ApplyLifecyclePermissions(AppraisalRequestDetailDto detail)
    {
        SetEditorEnabled(detail.CanEdit);
        btnSave.Enabled = detail.CanEdit;
        btnSubmit.Enabled = detail.CanSubmit;
        mnuCancelRequest.Enabled = detail.CanCancel;
        mnuWithdrawRequest.Enabled = detail.CanWithdraw;
        mnuDeleteDraft.Enabled = detail.CanHardDelete;
        btnDelete.Enabled = detail.CanHardDelete;
        btnDelete.Visible = detail.CanHardDelete;

        lblStatus.BackColor = detail.Status switch
        {
            AppraisalRequestStatuses.Submitted => Color.FromArgb(203, 233, 255),
            AppraisalRequestStatuses.UnderReview => Color.FromArgb(255, 234, 179),
            AppraisalRequestStatuses.Accepted => Color.FromArgb(220, 211, 255),
            AppraisalRequestStatuses.Converted => Color.FromArgb(204, 239, 217),
            _ => Color.FromArgb(223, 228, 232)
        };
    }

    private void SetEditorEnabled(bool enabled)
    {
        cmbClient.Enabled = enabled;
        cmbPrimaryContact.Enabled = enabled;
        cmbIntendedUse.Enabled = enabled;
        txtIntendedUseNarrative.ReadOnly = !enabled;
        dtpEffectiveDate.Enabled = enabled;
        cmbValueDefinition.Enabled = enabled;
        cmbAppraisalType.Enabled = enabled;
        cmbPriority.Enabled = enabled;
        txtScopeSummary.ReadOnly = !enabled;
        btnSelectVehicle.Enabled = enabled;
        btnAddIntendedUser.Enabled = enabled;
        btnEditIntendedUser.Enabled = enabled;
        btnDeactivateIntendedUser.Enabled = enabled;
        btnAddAttachment.Enabled = enabled;
    }

    private void btnSelectVehicle_Click(object? sender, EventArgs e) =>
        RaiseIntegrationCommand(VehicleSelectionRequested, null, "vehicle selector");

    private void btnAddIntendedUser_Click(object? sender, EventArgs e) =>
        RaiseIntegrationCommand(AddIntendedUserRequested, null, "Intended User editor");

    private void btnEditIntendedUser_Click(object? sender, EventArgs e)
    {
        var selected = dgvIntendedUsers.CurrentRow?.DataBoundItem as IntendedUserDto;
        if (selected is null)
        {
            ShowStatusMessage("Select an Intended User to edit.", isError: true);
            return;
        }

        RaiseIntegrationCommand(EditIntendedUserRequested, selected, "Intended User editor");
    }

    private void btnDeactivateIntendedUser_Click(object? sender, EventArgs e)
    {
        var selected = dgvIntendedUsers.CurrentRow?.DataBoundItem as IntendedUserDto;
        if (selected is null)
        {
            ShowStatusMessage("Select an Intended User to deactivate.", isError: true);
            return;
        }

        RaiseIntegrationCommand(DeactivateIntendedUserRequested, selected, "Intended User editor");
    }

    private void btnAddAttachment_Click(object? sender, EventArgs e) =>
        RaiseIntegrationCommand(AddAttachmentRequested, null, "attachment workflow");

    private void RaiseIntegrationCommand(
        EventHandler<AppraisalRequestCommandEventArgs>? handler,
        object? selectedItem,
        string integrationName)
    {
        if (_currentRequest is null)
        {
            ShowStatusMessage("Create or select an Appraisal Request first.", isError: true);
            return;
        }

        if (handler is null)
        {
            ShowStatusMessage($"Connect the existing {integrationName} to this workspace event.", isError: true);
            return;
        }

        handler(this, new AppraisalRequestCommandEventArgs(
            _currentRequest.AppraisalRequestId,
            _currentRequest.RowVersion,
            selectedItem));
    }

    private void UpdateCurrentListRow(AppraisalRequestDetailDto detail)
    {
        for (var index = 0; index < _requests.Count; index++)
        {
            if (_requests[index].AppraisalRequestId != detail.AppraisalRequestId)
                continue;

            _requests[index] = new AppraisalRequestListItemDto(
                detail.AppraisalRequestId,
                detail.RequestNumber,
                detail.ClientDisplayName ?? "Client not selected",
                detail.VehicleSummary ?? "Vehicle not selected",
                detail.Status,
                detail.RequestedEffectiveDateUtc,
                detail.UpdatedAtUtc,
                detail.RowVersion);
            break;
        }
    }

    private async Task HandleConcurrencyConflictAsync(AppraisalRequestConcurrencyException exception)
    {
        var choice = MessageBox.Show(
            this,
            $"{exception.Message}\n\nReload the current server version?",
            "Appraisal Request Changed",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button1);

        if (choice == DialogResult.Yes && _currentRequest is not null)
        {
            var current = await _apiClient.GetAsync(_currentRequest.AppraisalRequestId).ConfigureAwait(true);
            await DisplayRequestAsync(current).ConfigureAwait(true);
            UpdateCurrentListRow(current);
        }
    }

    private void ClearEditor()
    {
        _suppressEvents = true;
        try
        {
            lblRequestNumber.Text = "No request selected";
            lblStatus.Text = string.Empty;
            lblRowVersion.Text = string.Empty;
            lblAudit.Text = string.Empty;
            cmbClient.SelectedIndex = cmbClient.Items.Count > 0 ? 0 : -1;
            cmbPrimaryContact.DataSource = null;
            cmbIntendedUse.SelectedIndex = cmbIntendedUse.Items.Count > 0 ? 0 : -1;
            txtIntendedUseNarrative.Clear();
            dtpEffectiveDate.Checked = false;
            cmbValueDefinition.SelectedIndex = cmbValueDefinition.Items.Count > 0 ? 0 : -1;
            cmbAppraisalType.SelectedIndex = cmbAppraisalType.Items.Count > 0 ? 0 : -1;
            cmbPriority.SelectedItem = "Normal";
            txtScopeSummary.Clear();
            txtVehicleSummary.Clear();
            _intendedUsers.Clear();
            _attachments.Clear();
            _history.Clear();
            _validationIssues.Clear();
            SetEditorEnabled(false);
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            btnDelete.Enabled = false;
        }
        finally
        {
            _suppressEvents = false;
            SetDirty(false);
        }
    }

    private void SetBusy(bool busy, string? message = null)
    {
        useWaitCursor = busy;
        prgBusy.Visible = busy;
        lblWorkspaceStatus.Text = message ?? (busy ? "Working..." : "Ready");
        btnRefresh.Enabled = !busy;
        btnNew.Enabled = !busy;
    }

    private bool useWaitCursor
    {
        set
        {
            UseWaitCursor = value;
            Cursor = value ? Cursors.WaitCursor : Cursors.Default;
        }
    }

    private CancellationTokenSource ReplaceOperationToken()
    {
        CancelPendingOperation();
        _operationCts = new CancellationTokenSource();
        return _operationCts;
    }

    private void CancelPendingOperation()
    {
        if (_operationCts is null)
            return;

        _operationCts.Cancel();
        _operationCts.Dispose();
        _operationCts = null;
    }

    private void SetDirty(bool dirty)
    {
        _isDirty = dirty;
        lblDirty.Visible = dirty;
    }

    private bool ConfirmDiscardChanges() =>
        MessageBox.Show(
            this,
            "Discard the unsaved changes to this Appraisal Request?",
            "Unsaved Changes",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    private void RestoreSelection(Guid? appraisalRequestId)
    {
        dgvRequests.ClearSelection();
        if (!appraisalRequestId.HasValue)
            return;

        foreach (DataGridViewRow row in dgvRequests.Rows)
        {
            if (row.DataBoundItem is AppraisalRequestListItemDto item &&
                item.AppraisalRequestId == appraisalRequestId.Value)
            {
                row.Selected = true;
                dgvRequests.CurrentCell = row.Cells[0];
                return;
            }
        }
    }

    private static void BindReferenceOptions(
        ComboBox comboBox,
        IReadOnlyList<ReferenceOptionDto> options,
        bool includeEmpty,
        string emptyText)
    {
        var list = new List<ReferenceOptionDto>();
        if (includeEmpty)
            list.Add(new ReferenceOptionDto(Guid.Empty, string.Empty, emptyText));
        list.AddRange(options.Where(option => option.IsActive));

        comboBox.DisplayMember = nameof(ReferenceOptionDto.Name);
        comboBox.ValueMember = nameof(ReferenceOptionDto.Id);
        comboBox.DataSource = list;
    }

    private static Guid? GetSelectedReferenceId(ComboBox comboBox)
    {
        if (comboBox.SelectedItem is not ReferenceOptionDto option || option.Id == Guid.Empty)
            return null;
        return option.Id;
    }

    private static string? GetSelectedReferenceCode(ComboBox comboBox) =>
        comboBox.SelectedItem is ReferenceOptionDto option && option.Id != Guid.Empty
            ? option.Code
            : null;

    private static void SelectReference(ComboBox comboBox, Guid? id)
    {
        comboBox.SelectedValue = id ?? Guid.Empty;
        if (comboBox.SelectedIndex < 0 && comboBox.Items.Count > 0)
            comboBox.SelectedIndex = 0;
    }

    private static void SelectReferenceByCode(ComboBox comboBox, string? code)
    {
        for (var index = 0; index < comboBox.Items.Count; index++)
        {
            if (comboBox.Items[index] is ReferenceOptionDto option &&
                string.Equals(option.Code, code, StringComparison.OrdinalIgnoreCase))
            {
                comboBox.SelectedIndex = index;
                return;
            }
        }

        if (comboBox.Items.Count > 0)
            comboBox.SelectedIndex = 0;
    }

    private static void ReplaceBindingList<T>(BindingList<T> target, IEnumerable<T> source)
    {
        target.RaiseListChangedEvents = false;
        target.Clear();
        foreach (var item in source)
            target.Add(item);
        target.RaiseListChangedEvents = true;
        target.ResetBindings();
    }

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string FormatStatus(string status) =>
        status == AppraisalRequestStatuses.UnderReview ? "Under Review" : status;

    private string? PromptForReason(string actionName)
    {
        using var dialog = new Form
        {
            Text = $"{actionName} Appraisal Request",
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MinimizeBox = false,
            MaximizeBox = false,
            ShowInTaskbar = false,
            ClientSize = new Size(480, 210),
            Font = new Font("Segoe UI", 9F)
        };

        var label = new Label
        {
            AutoSize = true,
            Text = $"Reason to {actionName.ToLowerInvariant()} this request:",
            Location = new Point(16, 16)
        };
        var textBox = new TextBox
        {
            Multiline = true,
            MaxLength = 1000,
            Location = new Point(16, 44),
            Size = new Size(448, 105),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        var ok = new Button
        {
            Text = actionName,
            DialogResult = DialogResult.OK,
            Location = new Point(286, 166),
            Size = new Size(85, 30)
        };
        var cancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new Point(379, 166),
            Size = new Size(85, 30)
        };

        dialog.Controls.AddRange([label, textBox, ok, cancel]);
        dialog.AcceptButton = ok;
        dialog.CancelButton = cancel;

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return null;

        return string.IsNullOrWhiteSpace(textBox.Text) ? null : textBox.Text.Trim();
    }

    private void ShowStatusMessage(string message, bool isError)
    {
        lblWorkspaceStatus.Text = message;
        lblWorkspaceStatus.ForeColor = isError
            ? Color.FromArgb(183, 28, 49)
            : Color.FromArgb(18, 99, 49);
    }

    private void ShowError(Exception exception, string heading)
    {
        ShowStatusMessage(heading, isError: true);

        var detail = exception switch
        {
            AppraisalRequestApiException apiException when apiException.StatusCode == HttpStatusCode.NotFound =>
                "The Appraisal Request no longer exists.",
            AppraisalRequestApiException apiException => apiException.Message,
            _ => exception.Message
        };

        MessageBox.Show(
            this,
            $"{heading}\n\n{detail}",
            "ONE Classics Nexus",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}

public sealed class AppraisalRequestCommandEventArgs(
    Guid appraisalRequestId,
    long rowVersion,
    object? selectedItem) : EventArgs
{
    public Guid AppraisalRequestId { get; } = appraisalRequestId;
    public long RowVersion { get; } = rowVersion;
    public object? SelectedItem { get; } = selectedItem;
}
