using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ONE_Classics_Nexus.WinForms.Models.Ui;

namespace ONE_Classics_Nexus.WinForms.Services.Api
{
    public sealed class DashboardApiClient
    {
        private readonly HttpClient _httpClient;

        public DashboardApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<DashboardResponseDto> GetDashboardAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("/api/dashboard", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var detail = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidOperationException(
                    $"Dashboard API request failed. Status={(int)response.StatusCode}. Detail={detail}");
            }

            var dto = await response.Content.ReadFromJsonAsync<DashboardResponseDto>(cancellationToken);
            return dto ?? new DashboardResponseDto();
        }
    }
}