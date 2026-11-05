namespace ONE_Classics_Nexus.Domain.AppraisalRequests;

public sealed class AppraisalRequestDomainException : InvalidOperationException
{
    public AppraisalRequestDomainException(string code, string message) : base(message) => Code = code;
    public string Code { get; }
}
