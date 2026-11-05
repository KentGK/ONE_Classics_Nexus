using System.Net;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public class AppraisalApiException : Exception
{
    public AppraisalApiException(
        HttpStatusCode statusCode,
        string message,
        ApiProblemDetails? problemDetails = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ProblemDetails = problemDetails;
    }

    public HttpStatusCode StatusCode { get; }
    public ApiProblemDetails? ProblemDetails { get; }
}
