namespace ONE_Classics_Nexus.API.Contracts.Common;

/// <summary>
/// Standard API error response shape.
/// Mirrors RFC7807 ProblemDetails format used by ASP.NET.
/// </summary>
public sealed class ProblemDetailsResponse
{
    /// <summary>
    /// Short summary of the error.
    /// Example: "Server error"
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// HTTP status code.
    /// Example: 500
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Detailed error message.
    /// Example:
    /// "Unknown column 'system_key' in 'SELECT'"
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// API endpoint instance.
    /// Example:
    /// "/api/appraisals/{id}/valuation/run"
    /// </summary>
    public string? Instance { get; set; }

    /// <summary>
    /// Optional request trace id.
    /// Helpful for debugging.
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Optional validation errors.
    /// Used for 400 validation responses.
    /// </summary>
    public IDictionary<string, string[]>? Errors { get; set; }

    /// <summary>
    /// Factory method for server errors.
    /// </summary>
    public static ProblemDetailsResponse ServerError(
        string detail,
        string? instance = null,
        string? traceId = null)
    {
        return new ProblemDetailsResponse
        {
            Title = "Server error",
            Status = 500,
            Detail = detail,
            Instance = instance,
            TraceId = traceId
        };
    }

    /// <summary>
    /// Factory method for validation errors.
    /// </summary>
    public static ProblemDetailsResponse ValidationError(
        IDictionary<string, string[]> errors,
        string? instance = null,
        string? traceId = null)
    {
        return new ProblemDetailsResponse
        {
            Title = "Validation error",
            Status = 400,
            Detail = "One or more validation errors occurred.",
            Instance = instance,
            TraceId = traceId,
            Errors = errors
        };
    }

    /// <summary>
    /// Factory method for not found responses.
    /// </summary>
    public static ProblemDetailsResponse NotFound(
        string detail,
        string? instance = null,
        string? traceId = null)
    {
        return new ProblemDetailsResponse
        {
            Title = "Not found",
            Status = 404,
            Detail = detail,
            Instance = instance,
            TraceId = traceId
        };
    }
}