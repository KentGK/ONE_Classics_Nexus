using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using System.Net;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Services;

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
