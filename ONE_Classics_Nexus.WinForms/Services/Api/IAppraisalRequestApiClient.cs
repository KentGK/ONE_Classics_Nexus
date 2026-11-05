using ONE_Classics_Nexus.WinForms.Models.AppraisalRequests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.WinForms.Services.Api;

public interface IAppraisalRequestApiClient
{
    Task<PagedResult<AppraisalRequestListItemDto>> ListAsync(
        AppraisalRequestListQuery query,
        CancellationToken cancellationToken = default);

    Task<AppraisalRequestDetailDto> GetAsync(
        Guid appraisalRequestId,
        CancellationToken cancellationToken = default);

    Task<AppraisalRequestDetailDto> CreateAsync(
        CreateAppraisalRequestRequest request,
        CancellationToken cancellationToken = default);

    Task<AppraisalRequestDetailDto> UpdateAsync(
        Guid appraisalRequestId,
        UpdateAppraisalRequestRequest request,
        CancellationToken cancellationToken = default);

    Task<ValidationResponse> ValidateAsync(
        Guid appraisalRequestId,
        CancellationToken cancellationToken = default);

    Task<AppraisalRequestDetailDto> SubmitAsync(
        Guid appraisalRequestId,
        ExpectedRowVersionRequest request,
        CancellationToken cancellationToken = default);

    Task<AppraisalRequestDetailDto> CancelAsync(
        Guid appraisalRequestId,
        DispositionRequest request,
        CancellationToken cancellationToken = default);

    Task<AppraisalRequestDetailDto> WithdrawAsync(
        Guid appraisalRequestId,
        DispositionRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteDraftAsync(
        Guid appraisalRequestId,
        ExpectedRowVersionRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReferenceOptionDto>> ListClientsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReferenceOptionDto>> ListClientContactsAsync(
        Guid clientId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReferenceOptionDto>> ListIntendedUsesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReferenceOptionDto>> ListValueDefinitionsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReferenceOptionDto>> ListAppraisalTypesAsync(
        CancellationToken cancellationToken = default);
}
