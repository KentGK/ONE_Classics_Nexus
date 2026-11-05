using ONE_Classics_Nexus.Domain.AppraisalRequests;
using Xunit;
namespace ONE_Classics_Nexus.Tests.AppraisalRequests;
public sealed class AppraisalRequestDomainTests
{
    private static readonly DateTime Now = new(2026, 8, 31, 18, 0, 0, DateTimeKind.Utc);
    private static AppraisalRequest New(bool ack = true) => AppraisalRequest.Create(Guid.NewGuid(), "AR-2026-000001", Guid.NewGuid(), null, "Collector Vehicle Appraisal", "Insurance", null, null, null, null, "Physical", null, null, 1969, "Chevrolet", "Camaro", "123", null, null, "Portal", ack, Now);
    [Fact] public void CreateRequest_WithValidInputs_Succeeds() => Assert.Equal(AppraisalRequestStatus.Draft, New().RequestStatus);
    [Fact] public void SubmitRequest_FromDraft_Succeeds() { var x = New(); x.Submit(Now.AddMinutes(1)); Assert.Equal(AppraisalRequestStatus.Submitted, x.RequestStatus); }
    [Fact] public void SubmitRequest_MissingAcknowledgement_Fails() { var x = New(false); var e = Assert.Throws<AppraisalRequestDomainException>(() => x.Submit(Now)); Assert.Equal("APPRAISAL_REQUEST_INCOMPLETE", e.Code); }
    [Fact] public void GoldenWorkflow_Converts() { var x = New(); x.Submit(Now); x.BeginReview(Now); x.MarkQualified(Guid.NewGuid(), null, Now); x.Accept(Now); var app = Guid.NewGuid(); x.Convert(app, Now); Assert.Equal(app, x.ConvertedAppraisalId); Assert.Equal(AppraisalRequestStatus.Converted, x.RequestStatus); }
    [Fact] public void ConvertRequest_FromDraft_Fails() => Assert.Throws<AppraisalRequestDomainException>(() => New().Convert(Guid.NewGuid(), Now));
    [Fact] public void ConvertRequest_Twice_Fails() { var x = New(); x.Submit(Now); x.BeginReview(Now); x.MarkQualified(Guid.NewGuid(), null, Now); x.Convert(Guid.NewGuid(), Now); Assert.Throws<AppraisalRequestDomainException>(() => x.Convert(Guid.NewGuid(), Now)); }
    [Fact] public void ConvertedRequest_CannotBeEdited() { var x = New(); x.Submit(Now); x.BeginReview(Now); x.MarkQualified(Guid.NewGuid(), null, Now); x.Convert(Guid.NewGuid(), Now); Assert.Throws<AppraisalRequestDomainException>(() => x.Assign(Guid.NewGuid(), Now)); }
}
