using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ONE_Classics_Nexus.WinForms.Models.Ui;

namespace ONE_Classics_Nexus.WinForms.Services.Api
{
    public sealed class VehicleApiClient
    {
        private readonly HttpClient _httpClient;

        public VehicleApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<VehicleResponseDto?> GetByIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync($"/api/vehicles/{vehicleId}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await EnsureSuccessAsync(response, cancellationToken);

            return await response.Content.ReadFromJsonAsync<VehicleResponseDto>(cancellationToken: cancellationToken);
        }

        public async Task<VehicleResponseDto> CreateAsync(CreateVehicleRequestDto request, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync("/api/vehicles", request, cancellationToken);
            await EnsureSuccessAsync(response, cancellationToken);

            var dto = await response.Content.ReadFromJsonAsync<VehicleResponseDto>(cancellationToken: cancellationToken);
            return dto ?? throw new InvalidOperationException("Vehicle create response was empty.");
        }

        public async Task<VehicleResponseDto> UpdateAsync(Guid vehicleId, UpdateVehicleRequestDto request, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PutAsJsonAsync($"/api/vehicles/{vehicleId}", request, cancellationToken);
            await EnsureSuccessAsync(response, cancellationToken);

            var dto = await response.Content.ReadFromJsonAsync<VehicleResponseDto>(cancellationToken: cancellationToken);
            return dto ?? throw new InvalidOperationException("Vehicle update response was empty.");
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
                return;

            var detail = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Vehicle API request failed. Status={(int)response.StatusCode}. Detail={detail}");
        }
    }
}