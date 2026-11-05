namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

public sealed class ApiProblemDetails
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }

    public string ToDisplayMessage()
    {
        var validation = Errors is null
            ? null
            : string.Join(Environment.NewLine, Errors.SelectMany(pair =>
                pair.Value.Select(message => $"{pair.Key}: {message}")));

        if (!string.IsNullOrWhiteSpace(validation))
            return validation;

        if (!string.IsNullOrWhiteSpace(Detail))
            return Detail!;

        if (!string.IsNullOrWhiteSpace(Title))
            return Title!;

        return "The Nexus API returned an error.";
    }
}
