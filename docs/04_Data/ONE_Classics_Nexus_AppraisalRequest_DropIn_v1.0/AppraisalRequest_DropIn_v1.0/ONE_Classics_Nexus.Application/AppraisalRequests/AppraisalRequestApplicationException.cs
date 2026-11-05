namespace ONE_Classics_Nexus.Application.AppraisalRequests;

public sealed class AppraisalRequestApplicationException : Exception
{
    public AppraisalRequestApplicationException(string code, string message, int statusCode) : base(message)
    { Code = code; StatusCode = statusCode; }
    public string Code { get; }
    public int StatusCode { get; }
}
