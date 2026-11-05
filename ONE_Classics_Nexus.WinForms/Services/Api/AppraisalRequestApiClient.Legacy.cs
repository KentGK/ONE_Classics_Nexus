using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.WinForms.Services.Api;

public sealed partial class AppraisalRequestApiClient
{
    public Task<ONE_Classics_Nexus.Application.AppraisalRequests.PagedResult<AppraisalRequestSummary>?> SearchAsync(
        string? status,
        CancellationToken cancellationToken = default)
    {
        var encoded = Uri.EscapeDataString(status ?? string.Empty);
        return _httpClient.GetFromJsonAsync<ONE_Classics_Nexus.Application.AppraisalRequests.PagedResult<AppraisalRequestSummary>>(
            $"api/appraisal-requests?status={encoded}",
            cancellationToken);
    }

    public Task<PartyIdentificationResponse?> GetPartyIdentificationAsync(
        Guid appraisalRequestId,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetFromJsonAsync<PartyIdentificationResponse>(
            $"api/appraisal-requests/{appraisalRequestId}/party-identification",
            cancellationToken);

    public async Task<PartyIdentificationResponse?> SetClientAsync(
        Guid appraisalRequestId,
        SetAppraisalRequestClientRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"api/appraisal-requests/{appraisalRequestId}/party-identification/client", request, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PartyIdentificationResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<PartyIdentificationResponse?> SetPrimaryClientContactAsync(
        Guid appraisalRequestId,
        SetPrimaryClientContactRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"api/appraisal-requests/{appraisalRequestId}/party-identification/primary-contact", request, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PartyIdentificationResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<PartyIdentificationResponse?> SetProposedIntendedUseAsync(
        Guid appraisalRequestId,
        SetIntendedUseRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"api/appraisal-requests/{appraisalRequestId}/party-identification/proposed-intended-use", request, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PartyIdentificationResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<ProposedIntendedUserSnapshot?> AddProposedIntendedUserAsync(
        Guid appraisalRequestId,
        CreateIntendedUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync($"api/appraisal-requests/{appraisalRequestId}/party-identification/proposed-intended-users", request, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProposedIntendedUserSnapshot>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<ProposedIntendedUserSnapshot?> UpdateProposedIntendedUserAsync(
        Guid appraisalRequestId,
        Guid intendedUserId,
        UpdateIntendedUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"api/appraisal-requests/{appraisalRequestId}/party-identification/proposed-intended-users/{intendedUserId}", request, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProposedIntendedUserSnapshot>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<PartyIdentificationResponse?> RemoveProposedIntendedUserAsync(
        Guid appraisalRequestId,
        Guid intendedUserId,
        RemoveIntendedUserRequest request,
        CancellationToken cancellationToken = default)
    {
        using var message = new HttpRequestMessage(HttpMethod.Delete, $"api/appraisal-requests/{appraisalRequestId}/party-identification/proposed-intended-users/{intendedUserId}")
        {
            Content = JsonContent.Create(request)
        };

        using var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PartyIdentificationResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<PartyIdentificationResponse?> SaveClientExclusionAsync(
        Guid appraisalRequestId,
        SaveClientExclusionRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync($"api/appraisal-requests/{appraisalRequestId}/party-identification/client-exclusion", request, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PartyIdentificationResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<ValidationSummary?> ValidatePartyIdentificationAsync(
        Guid appraisalRequestId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsync($"api/appraisal-requests/{appraisalRequestId}/party-identification/validate", null, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ValidationSummary>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public Task<Clients?> GetClientAsync(
        Guid clientId,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetFromJsonAsync<Clients>($"api/clients/{clientId}", cancellationToken);

    public async Task<IReadOnlyList<ClientContact>> GetClientContactsAsync(
        Guid clientId,
        CancellationToken cancellationToken = default)
    {
        var contacts = await _httpClient
            .GetFromJsonAsync<List<ClientContact>>($"api/clients/{clientId}/contacts", cancellationToken)
            .ConfigureAwait(false);

        return contacts ?? [];
    }
}
