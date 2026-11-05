using ONE_Classics_Nexus.WinForms.Services.Api;

namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;

/// <summary>
/// Hosts the new-request intake experience that starts with request information.
/// </summary>
public sealed class AppraisalRequestInformationForm : AppraisalRequestForm
{
    /// <summary>
    /// Creates the request information form with default dependencies.
    /// </summary>
    public AppraisalRequestInformationForm()
        : base()
    {
        Text = "New Request";
        StartPosition = FormStartPosition.CenterParent;
    }

    /// <summary>
    /// Creates the request information form with an API client.
    /// </summary>
    /// <param name="appraisalRequestApiClient">API client for appraisal request data operations.</param>
    public AppraisalRequestInformationForm(AppraisalRequestApiClient appraisalRequestApiClient)
        : base(appraisalRequestApiClient)
    {
        Text = "New Request";
        StartPosition = FormStartPosition.CenterParent;
    }
}
