using System.Net.Http.Json;
using System.Text.Json;

namespace ONE_Classics_Nexus.Tests.Integration.Infrastructure;

public static class JsonTestExtensions
{
    public static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<T?> ReadAsAsync<T>(this HttpContent content, CancellationToken cancellationToken = default)
    {
        return await content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
    }
}