using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Domain.Entities;
using ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests.Dialogs;
using ONE_Classics_Nexus.WinForms.Services.Api;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

public sealed class AppraisalRequestPartyIdentificationController : IDisposable
{
    private readonly AppraisalRequestForm _form;
    private readonly AppraisalRequestApiClient _apiClient;
    private PartyIdentificationResponse? _workspace;
    private Guid _appraisalRequestId;
    private bool _disposed;
    private bool _busy;

    public AppraisalRequestPartyIdentificationController(
        AppraisalRequestForm form,
        AppraisalRequestApiClient apiClient)
    {
        _form = form ?? throw new ArgumentNullException(nameof(form));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

        WireEvents();
    }

    public async Task LoadAsync(Guid appraisalRequestId, CancellationToken cancellationToken = default)
    {
        if (appraisalRequestId == Guid.Empty)
            return;

        _appraisalRequestId = appraisalRequestId;
        await RefreshWorkspaceAsync(cancellationToken).ConfigureAwait(false);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        UnwireEvents();
        _disposed = true;
    }

    private void WireEvents()
    {
        var tab = _form.PartyIdentificationTab;
        tab.SelectClientRequested += PartyTab_SelectClientRequested;
        tab.ChangeClientRequested += PartyTab_SelectClientRequested;
        tab.OpenClientRequested += PartyTab_OpenClientRequested;
        tab.AddContactRequested += PartyTab_AddContactRequested;
        tab.EditContactRequested += PartyTab_EditContactRequested;
        tab.PrimaryClientContactChanged += PartyTab_PrimaryClientContactChanged;
        tab.ProposedIntendedUseChanged += PartyTab_ProposedIntendedUseChanged;
        tab.AddClientAsUserRequested += PartyTab_AddClientAsUserRequested;
        tab.ClientNotNowRequested += PartyTab_ClientNotNowRequested;
        tab.ClientNoRequested += PartyTab_ClientNoRequested;
        tab.AddIntendedUserRequested += PartyTab_AddIntendedUserRequested;
        tab.EditIntendedUserRequested += PartyTab_EditIntendedUserRequested;
        tab.RemoveIntendedUserRequested += PartyTab_RemoveProposedIntendedUserRequested;
        tab.ValidateRequested += PartyTab_ValidateRequested;
    }

    private void UnwireEvents()
    {
        var tab = _form.PartyIdentificationTab;
        tab.SelectClientRequested -= PartyTab_SelectClientRequested;
        tab.ChangeClientRequested -= PartyTab_SelectClientRequested;
        tab.OpenClientRequested -= PartyTab_OpenClientRequested;
        tab.AddContactRequested -= PartyTab_AddContactRequested;
        tab.EditContactRequested -= PartyTab_EditContactRequested;
        tab.PrimaryClientContactChanged -= PartyTab_PrimaryClientContactChanged;
        tab.ProposedIntendedUseChanged -= PartyTab_ProposedIntendedUseChanged;
        tab.AddClientAsUserRequested -= PartyTab_AddClientAsUserRequested;
        tab.ClientNotNowRequested -= PartyTab_ClientNotNowRequested;
        tab.ClientNoRequested -= PartyTab_ClientNoRequested;
        tab.AddIntendedUserRequested -= PartyTab_AddIntendedUserRequested;
        tab.EditIntendedUserRequested -= PartyTab_EditIntendedUserRequested;
        tab.RemoveIntendedUserRequested -= PartyTab_RemoveProposedIntendedUserRequested;
        tab.ValidateRequested -= PartyTab_ValidateRequested;
    }

    private void PartyTab_OpenClientRequested(object? sender, EventArgs e)
    {
        if (_workspace?.Client is null)
        {
            ShowInfo("Select a client to view details.");
            return;
        }

        ShowInfo($"Client: {_workspace.Client.DisplayName}");
    }

    private void PartyTab_AddContactRequested(object? sender, EventArgs e)
    {
        ShowInfo("Use the client workspace to add a new contact, then refresh party identification.");
    }

    private void PartyTab_EditContactRequested(object? sender, EventArgs e)
    {
        if (_workspace?.PrimaryClientContact is null)
        {
            ShowInfo("Select a primary contact before editing.");
            return;
        }

        ShowInfo("Use the client workspace to edit this contact, then refresh party identification.");
    }

    private void PartyTab_SelectClientRequested(object? sender, EventArgs e)
    {
        Run(async ct =>
        {
            using var dialog = new ClientPickerDialog(_apiClient, _workspace?.ClientId);
            if (dialog.ShowDialog(_form) != DialogResult.OK || !dialog.SelectedClientId.HasValue)
                return;

            var updated = await _apiClient.SetClientAsync(
                _appraisalRequestId,
                new SetAppraisalRequestClientRequest(dialog.SelectedClientId.Value, GetExpectedRowVersion()),
                ct).ConfigureAwait(false);

            await ApplyUpdatedWorkspaceAsync(updated, ct).ConfigureAwait(false);

            var selectedContactId = dialog.SelectedPrimaryContactId;
            if (!selectedContactId.HasValue)
                return;

            var withContact = await _apiClient.SetPrimaryClientContactAsync(
                _appraisalRequestId,
                new SetPrimaryClientContactRequest(selectedContactId, GetExpectedRowVersion()),
                ct).ConfigureAwait(false);

            await ApplyUpdatedWorkspaceAsync(withContact, ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_PrimaryClientContactChanged(object? sender, Guid? selectedContactId)
    {
        if (!selectedContactId.HasValue && _workspace?.PrimaryClientContactId is null)
            return;

        Run(async ct =>
        {
            var updated = await _apiClient.SetPrimaryClientContactAsync(
                _appraisalRequestId,
                new SetPrimaryClientContactRequest(selectedContactId, GetExpectedRowVersion()),
                ct).ConfigureAwait(false);

            await ApplyUpdatedWorkspaceAsync(updated, ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_ProposedIntendedUseChanged(object? sender, Guid? intendedUseId)
    {
        if (!intendedUseId.HasValue)
            return;

        Run(async ct =>
        {
            var updated = await _apiClient.SetProposedIntendedUseAsync(
                _appraisalRequestId,
                new SetIntendedUseRequest(intendedUseId.Value, _form.PartyIdentificationTab.CustomNarrative, GetExpectedRowVersion()),
                ct).ConfigureAwait(false);

            await ApplyUpdatedWorkspaceAsync(updated, ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_AddClientAsUserRequested(object? sender, EventArgs e)
    {
        Run(async ct =>
        {
            if (_workspace?.Client is null)
            {
                ShowInfo("Select a client before adding the client as a proposed intended user.");
                return;
            }

            var userTypeId = ResolveUserTypeId(_workspace.ProposedIntendedUsers);
            if (!userTypeId.HasValue)
            {
                ShowInfo("Add at least one intended-user type first, then retry Add Client As User.");
                return;
            }

            var request = new CreateIntendedUserRequest(
                userTypeId.Value,
                true,
                _workspace.Client.DisplayName,
                _workspace.Client.OrganizationName,
                "Client",
                _workspace.Client.Address,
                _workspace.Client.Email,
                _workspace.Client.Phone,
                null,
                NextSortOrder(_workspace.ProposedIntendedUsers),
                GetExpectedRowVersion(),
                _workspace.Client.ClientId,
                _workspace.PrimaryClientContactId);

            await _apiClient.AddProposedIntendedUserAsync(_appraisalRequestId, request, ct).ConfigureAwait(false);
            await RefreshWorkspaceAsync(ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_ClientNotNowRequested(object? sender, EventArgs e)
    {
        ShowInfo("Client intended-user confirmation deferred. Continue by adding at least one proposed intended user before acceptance/conversion.");
    }

    private void PartyTab_ClientNoRequested(object? sender, EventArgs e)
    {
        Run(async ct =>
        {
            using var dialog = new ClientExclusionDialog();
            if (dialog.ShowDialog(_form) != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.Explanation))
                return;

            var updated = await _apiClient.SaveClientExclusionAsync(
                _appraisalRequestId,
                new SaveClientExclusionRequest(dialog.Explanation, GetExpectedRowVersion()),
                ct).ConfigureAwait(false);

            await ApplyUpdatedWorkspaceAsync(updated, ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_AddIntendedUserRequested(object? sender, EventArgs e)
    {
        Run(async ct =>
        {
            var userTypeId = ResolveUserTypeId(_workspace?.ProposedIntendedUsers);
            if (!userTypeId.HasValue)
            {
                ShowInfo("No intended-user type id is available yet. Add a user through a full editor first.");
                return;
            }

            using var dialog = new IntendedUserEditorDialog("Add Proposed Intended User");
            if (dialog.ShowDialog(_form) != DialogResult.OK)
                return;

            var displayName = dialog.DisplayName;
            var relationship = dialog.RelationshipToClient;
            if (string.IsNullOrWhiteSpace(displayName) || string.IsNullOrWhiteSpace(relationship))
                return;

            var request = new CreateIntendedUserRequest(
                userTypeId.Value,
                false,
                displayName.Trim(),
                dialog.OrganizationName,
                relationship.Trim(),
                null,
                dialog.Email,
                dialog.Phone,
                dialog.UseRestrictions,
                NextSortOrder(_workspace?.ProposedIntendedUsers),
                GetExpectedRowVersion(),
                _workspace?.ClientId,
                _workspace?.PrimaryClientContactId);

            await _apiClient.AddProposedIntendedUserAsync(_appraisalRequestId, request, ct).ConfigureAwait(false);
            await RefreshWorkspaceAsync(ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_EditIntendedUserRequested(object? sender, EventArgs e)
    {
        Run(async ct =>
        {
            if (!_form.PartyIdentificationTab.TryGetSelectedProposedIntendedUser(out var selected) || selected is null)
            {
                ShowInfo("Select a proposed intended user to edit.");
                return;
            }

            using var dialog = new IntendedUserEditorDialog("Edit Proposed Intended User", selected);
            if (dialog.ShowDialog(_form) != DialogResult.OK)
                return;

            var displayName = dialog.DisplayName;
            var relationship = dialog.RelationshipToClient;
            if (string.IsNullOrWhiteSpace(displayName) || string.IsNullOrWhiteSpace(relationship))
                return;

            var request = new UpdateIntendedUserRequest(
                selected.UserTypeId,
                selected.IsClient,
                displayName.Trim(),
                dialog.OrganizationName,
                relationship.Trim(),
                selected.Address,
                dialog.Email,
                dialog.Phone,
                dialog.UseRestrictions,
                selected.SortOrder,
                GetExpectedRowVersion(),
                selected.IsActive,
                _workspace?.ClientId,
                _workspace?.PrimaryClientContactId);

            await _apiClient.UpdateProposedIntendedUserAsync(_appraisalRequestId, selected.IntendedUserId, request, ct).ConfigureAwait(false);
            await RefreshWorkspaceAsync(ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_RemoveProposedIntendedUserRequested(object? sender, EventArgs e)
    {
        Run(async ct =>
        {
            if (!_form.PartyIdentificationTab.TryGetSelectedProposedIntendedUser(out var selected) || selected is null)
            {
                ShowInfo("Select a proposed intended user to remove.");
                return;
            }

            var updated = await _apiClient.RemoveProposedIntendedUserAsync(
                _appraisalRequestId,
                selected.IntendedUserId,
                new RemoveIntendedUserRequest(GetExpectedRowVersion()),
                ct).ConfigureAwait(false);

            await ApplyUpdatedWorkspaceAsync(updated, ct).ConfigureAwait(false);
        });
    }

    private void PartyTab_ValidateRequested(object? sender, EventArgs e)
    {
        Run(async ct =>
        {
            var validation = await _apiClient.ValidatePartyIdentificationAsync(_appraisalRequestId, ct).ConfigureAwait(false);
            if (validation is null)
                return;

            _form.PartyIdentificationTab.SetValidationSummary(validation);
        });
    }

    private async Task RefreshWorkspaceAsync(CancellationToken cancellationToken)
    {
        var workspace = await _apiClient.GetPartyIdentificationAsync(_appraisalRequestId, cancellationToken).ConfigureAwait(false);

        if (workspace?.ClientId is Guid clientId && clientId != Guid.Empty)
        {
            var contacts = await _apiClient.GetClientContactsAsync(clientId, cancellationToken).ConfigureAwait(false);
            var options = contacts
                .Where(x => x.IsActive)
                .Select(MapContact)
                .ToList();

            _form.PartyIdentificationTab.SetContactOptions(options);
        }
        else
        {
            _form.PartyIdentificationTab.SetContactOptions([]);
        }

        await ApplyUpdatedWorkspaceAsync(workspace, cancellationToken).ConfigureAwait(false);
    }

    private Task ApplyUpdatedWorkspaceAsync(PartyIdentificationResponse? updated, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (updated is null)
            return Task.CompletedTask;

        _workspace = updated;

        if (_form.InvokeRequired)
        {
            _form.BeginInvoke(new Action(() => _form.PartyIdentificationTab.Bind(_workspace)));
            return Task.CompletedTask;
        }

        _form.PartyIdentificationTab.Bind(_workspace);
        return Task.CompletedTask;
    }

    private ulong GetExpectedRowVersion()
    {
        if (_workspace is not null)
            return _workspace.AppraisalRequestRowVersion;

        return _form.RowVersion <= 0 ? 0UL : (ulong)_form.RowVersion;
    }

    private static Guid? ResolveUserTypeId(IReadOnlyList<ProposedIntendedUserSnapshot>? users)
    {
        var clientType = users?.FirstOrDefault(x => x.IsClient)?.UserTypeId;
        if (clientType.HasValue && clientType.Value != Guid.Empty)
            return clientType;

        var first = users?.FirstOrDefault()?.UserTypeId;
        if (first.HasValue && first.Value != Guid.Empty)
            return first;

        return null;
    }

    private static int NextSortOrder(IReadOnlyList<ProposedIntendedUserSnapshot>? users)
    {
        if (users is null || users.Count == 0)
            return 1;

        return users.Max(x => x.SortOrder) + 1;
    }

    private void Run(Func<CancellationToken, Task> work)
    {
        if (_disposed || _busy || _appraisalRequestId == Guid.Empty)
            return;

        _busy = true;
        _ = ExecuteAsync(work);
    }

    private async Task ExecuteAsync(Func<CancellationToken, Task> work)
    {
        try
        {
            await work(CancellationToken.None).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ShowInfo(ex.Message);
        }
        finally
        {
            _busy = false;
        }
    }

    private static ClientContactSnapshot MapContact(ClientContact contact)
    {
        var name = BuildContactName(contact);

        return new ClientContactSnapshot(
            contact.ClientContactId,
            name,
            contact.JobTitle,
            contact.ContactType,
            contact.Email,
            contact.Phone,
            null,
            (ulong)Math.Max(0, contact.RowVersion));
    }

    private static string BuildContactName(ClientContact contact)
    {
        var fullName = $"{contact.FirstName} {contact.LastName}".Trim();
        return string.IsNullOrWhiteSpace(fullName) ? contact.Email ?? contact.Phone ?? contact.ClientContactId.ToString() : fullName;
    }

    private void ShowInfo(string message)
    {
        if (_form.InvokeRequired)
        {
            _form.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(_form, message, "Party Identification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }));
            return;
        }

        MessageBox.Show(_form, message, "Party Identification", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
