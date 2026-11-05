using System.Net.Http.Json;
using ONE_Classics_Nexus.Application.AppraisalRequests;
namespace ONE_Classics_Nexus.WinForms.Services.Api;
public sealed class AppraisalRequestApiClient(HttpClient http)
{
    public async Task<PagedResult<AppraisalRequestSummary>?> SearchAsync(string? status,CancellationToken ct=default)=>await http.GetFromJsonAsync<PagedResult<AppraisalRequestSummary>>($"api/appraisal-requests?status={Uri.EscapeDataString(status??"")}",ct);
    public Task<AppraisalRequestResponse?> GetAsync(Guid id,CancellationToken ct=default)=>http.GetFromJsonAsync<AppraisalRequestResponse>($"api/appraisal-requests/{id}",ct);
    public async Task<AppraisalRequestResponse?> CreateAsync(CreateAppraisalRequestRequest r,CancellationToken ct=default){var m=await http.PostAsJsonAsync("api/appraisal-requests",r,ct);m.EnsureSuccessStatusCode();return await m.Content.ReadFromJsonAsync<AppraisalRequestResponse>(cancellationToken:ct);}
    public async Task<AppraisalRequestResponse?> UpdateAsync(Guid id,UpdateAppraisalRequestRequest r,CancellationToken ct=default){var m=await http.PutAsJsonAsync($"api/appraisal-requests/{id}",r,ct);m.EnsureSuccessStatusCode();return await m.Content.ReadFromJsonAsync<AppraisalRequestResponse>(cancellationToken:ct);}
    public async Task<AppraisalRequestResponse?> WorkflowAsync(Guid id,string action,object body,CancellationToken ct=default){var m=await http.PostAsJsonAsync($"api/appraisal-requests/{id}/{action}",body,ct);m.EnsureSuccessStatusCode();return await m.Content.ReadFromJsonAsync<AppraisalRequestResponse>(cancellationToken:ct);}
    public async Task<ConvertAppraisalRequestResult?> ConvertAsync(Guid id,ConvertAppraisalRequestCommand body,CancellationToken ct=default){var m=await http.PostAsJsonAsync($"api/appraisal-requests/{id}/convert",body,ct);m.EnsureSuccessStatusCode();return await m.Content.ReadFromJsonAsync<ConvertAppraisalRequestResult>(cancellationToken:ct);}
}
