using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public sealed class AppraisalWorkspaceService : IAppraisalWorkspaceService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public AppraisalWorkspaceService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
    }

    public async Task<AppraisalWorkspaceDto> GetAsync(
        Guid appraisalId,
        CancellationToken cancellationToken = default)
    {
        EnsureAppraisalId(appraisalId);

        using var response = await _httpClient.GetAsync(
            $"api/appraisals/{appraisalId:D}",
            cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        await EnsureSuccessAsync(response, payload, cancellationToken);

        return ParseWorkspace(payload, appraisalId);
    }

    public async Task<AppraisalWorkspaceDto> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalWorkspaceRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureAppraisalId(appraisalId);
        ArgumentNullException.ThrowIfNull(request);

        if (request.AppraisalId != Guid.Empty && request.AppraisalId != appraisalId)
            throw new ArgumentException("Request appraisal id does not match the route appraisal id.", nameof(request));

        request.AppraisalId = appraisalId;

        using var response = await _httpClient.PutAsJsonAsync(
            $"api/appraisals/{appraisalId:D}",
            request,
            _jsonOptions,
            cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var problem = TryParseProblem(payload);
            throw new AppraisalConcurrencyException(
                problem?.ToDisplayMessage()
                ?? "The appraisal was changed by another user. Refresh the workspace and retry your changes.",
                problem);
        }

        await EnsureSuccessAsync(response, payload, cancellationToken);

        // Some update endpoints legitimately return 204 No Content. Reload so the UI
        // receives the server-owned row_version and timestamps after a successful save.
        return string.IsNullOrWhiteSpace(payload)
            ? await GetAsync(appraisalId, cancellationToken)
            : ParseWorkspace(payload, appraisalId);
    }

    private AppraisalWorkspaceDto ParseWorkspace(string payload, Guid expectedAppraisalId)
    {
        if (string.IsNullOrWhiteSpace(payload))
            throw new AppraisalApiException(
                HttpStatusCode.OK,
                "The Nexus API returned an empty appraisal response.");

        try
        {
            using var document = JsonDocument.Parse(payload);
            var root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
                throw new JsonException("Expected a JSON object.");

            AppraisalWorkspaceDto? result = null;

            if (TryGetProperty(root, "appraisal", out var appraisalElement)
                && appraisalElement.ValueKind == JsonValueKind.Object)
            {
                result = appraisalElement.Deserialize<AppraisalWorkspaceDto>(_jsonOptions);
                if (result is not null)
                    MergeEnvelope(root, result);
            }
            else
            {
                result = root.Deserialize<AppraisalWorkspaceDto>(_jsonOptions);
                if (result is not null)
                    MergeAliases(root, result);
            }

            if (result is null)
                throw new JsonException("Could not deserialize appraisal response.");

            if (result.AppraisalId == Guid.Empty)
                result.AppraisalId = expectedAppraisalId;

            if (result.AppraisalId != expectedAppraisalId)
                throw new JsonException("The API response appraisal id does not match the requested appraisal id.");

            return result;
        }
        catch (AppraisalApiException)
        {
            throw;
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            throw new AppraisalApiException(
                HttpStatusCode.OK,
                "The appraisal response could not be parsed. Verify the WinForms DTO contract matches the API response.",
                innerException: ex);
        }
    }

    private void MergeEnvelope(JsonElement root, AppraisalWorkspaceDto target)
    {
        target.Vehicle ??= DeserializeProperty<VehicleSummaryDto>(root, "vehicle")
            ?? DeserializeProperty<VehicleSummaryDto>(root, "subjectVehicle");
        target.Client ??= DeserializeProperty<ClientSummaryDto>(root, "client");
        target.AssignedAppraiser ??= DeserializeProperty<UserSummaryDto>(root, "assignedAppraiser");
        target.AssignedReviewer ??= DeserializeProperty<UserSummaryDto>(root, "assignedReviewer");
        target.LatestInspection ??= DeserializeProperty<InspectionSummaryDto>(root, "latestInspection")
            ?? DeserializeProperty<InspectionSummaryDto>(root, "inspection");
        target.LatestValuation ??= DeserializeProperty<ValuationSummaryDto>(root, "latestValuation")
            ?? DeserializeProperty<ValuationSummaryDto>(root, "valuation");
        target.LatestReport ??= DeserializeProperty<ReportSummaryDto>(root, "latestReport")
            ?? DeserializeProperty<ReportSummaryDto>(root, "report");

        if (target.ComparableCount == 0)
            target.ComparableCount = ReadInt32(root, "comparableCount") ?? ReadInt32(root, "comparablesCount") ?? 0;
    }

    private void MergeAliases(JsonElement root, AppraisalWorkspaceDto target)
    {
        target.Vehicle ??= DeserializeProperty<VehicleSummaryDto>(root, "subjectVehicle");
        target.LatestInspection ??= DeserializeProperty<InspectionSummaryDto>(root, "inspection");
        target.LatestValuation ??= DeserializeProperty<ValuationSummaryDto>(root, "valuation");
        target.LatestReport ??= DeserializeProperty<ReportSummaryDto>(root, "report");
    }

    private T? DeserializeProperty<T>(JsonElement root, string propertyName)
    {
        return TryGetProperty(root, propertyName, out var element)
               && element.ValueKind is not JsonValueKind.Null and not JsonValueKind.Undefined
            ? element.Deserialize<T>(_jsonOptions)
            : default;
    }

    private static int? ReadInt32(JsonElement root, string propertyName)
    {
        if (!TryGetProperty(root, propertyName, out var element))
            return null;

        if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out var value))
            return value;

        if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out value))
            return value;

        return null;
    }

    private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        string payload,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var problem = TryParseProblem(payload);
        var message = problem?.ToDisplayMessage()
            ?? $"The Nexus API returned HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).";

        await Task.CompletedTask.WaitAsync(cancellationToken);
        throw new AppraisalApiException(response.StatusCode, message, problem);
    }

    private ApiProblemDetails? TryParseProblem(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return null;

        try
        {
            return JsonSerializer.Deserialize<ApiProblemDetails>(payload, _jsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static void EnsureAppraisalId(Guid appraisalId)
    {
        if (appraisalId == Guid.Empty)
            throw new ArgumentException("Appraisal id is required.", nameof(appraisalId));
    }
}
