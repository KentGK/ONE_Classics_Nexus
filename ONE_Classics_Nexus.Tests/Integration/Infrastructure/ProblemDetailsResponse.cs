using System.Text.Json;
using System.Text.Json.Serialization;

namespace ONE_Classics_Nexus.Tests.Integration.Infrastructure;

public sealed class ProblemDetailsResponse
{
    public string? Type { get; set; }

    public string? Title { get; set; }

    public int? Status { get; set; }

    public string? Detail { get; set; }

    public string? Instance { get; set; }

    // Keep this flexible because different middleware/components may emit
    // validation `errors` in different JSON shapes.
    [JsonPropertyName("errors")]
    public JsonElement? Errors { get; set; }
}