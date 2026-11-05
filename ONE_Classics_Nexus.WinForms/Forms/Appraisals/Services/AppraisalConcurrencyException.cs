using ONE_Classics_Nexus.WinForms.Forms.Appraisals.Models;
using System.Net;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals.Services;

public sealed class AppraisalConcurrencyException : AppraisalApiException
{
    public AppraisalConcurrencyException(string message, ApiProblemDetails? problemDetails = null)
        : base(HttpStatusCode.Conflict, message, problemDetails)
    {
    }
}
