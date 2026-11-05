using System.Net;
using ONE_Classics_Nexus.WinForms.Features.Appraisals.Models;

namespace ONE_Classics_Nexus.WinForms.Features.Appraisals.Services;

public sealed class AppraisalConcurrencyException : AppraisalApiException
{
    public AppraisalConcurrencyException(string message, ApiProblemDetails? problemDetails = null)
        : base(HttpStatusCode.Conflict, message, problemDetails)
    {
    }
}
