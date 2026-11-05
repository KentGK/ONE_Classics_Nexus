using ONE_Classics_Nexus.API.ApplicationPorts;
using ONE_Classics_Nexus.API.Contracts.Appraisals;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.Exceptions;
using AppraisalEntity = ONE_Classics_Nexus.Domain.Entities.Appraisal;

namespace ONE_Classics_Nexus.API.Services;

public sealed class AppraisalApiService : IAppraisalApiService
{
    private readonly IAppraisalRepository _appraisalRepository;
    private readonly IAutoNumberGenerator _autoNumberGenerator;

    public AppraisalApiService(
        IAppraisalRepository appraisalRepository,
        IAutoNumberGenerator autoNumberGenerator)
    {
        _appraisalRepository = appraisalRepository;
        _autoNumberGenerator = autoNumberGenerator;
    }

    public async Task<AppraisalResponse> CreateAsync(
        CreateAppraisalRequest request,
        CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;
        var appraisalNumber = request.AppraisalNumber;

        if (string.IsNullOrWhiteSpace(appraisalNumber))
        {
            appraisalNumber = await _autoNumberGenerator
                .NextAsync("appraisal", "AN", 6, cancellationToken)
                .ConfigureAwait(false);
        }

        var appraisal = new AppraisalEntity
        {
            AppraisalId = Guid.NewGuid(),
            VehicleId = request.VehicleId,
            AppraisalNumber = appraisalNumber,
            AppraisalRequestId = request.AppraisalRequestId,
            ClientId = request.ClientId,
            AssignedAppraiserId = request.AssignedAppraiserId,
            AssignedReviewerId = request.AssignedReviewerId,
            AppraisalType = request.AppraisalType ?? string.Empty,
            Status = request.Status,
            IntakeStatus = request.IntakeStatus,
            InspectionStatus = request.InspectionStatus,
            ValuationStatus = request.ValuationStatus,
            ReportStatus = request.ReportStatus,
            EffectiveDateUtc = request.EffectiveDateUtc,
            RowVersion = 1,
            CreatedAtUtc = utcNow,
            UpdatedAtUtc = utcNow
        };

        var inserted = await _appraisalRepository.InsertAsync(appraisal, cancellationToken);

        return MapToResponse(inserted);
    }

    public async Task<AppraisalResponse?> GetByIdAsync(
        Guid appraisalId,
        CancellationToken cancellationToken)
    {
        var appraisal = await _appraisalRepository.GetByIdAsync(
            appraisalId,
            cancellationToken);

        return appraisal is null ? null : MapToResponse(appraisal);
    }

    public async Task<AppraisalResponse?> UpdateAsync(
        Guid appraisalId,
        UpdateAppraisalRequest request,
        CancellationToken cancellationToken)
    {
        var appraisal = await _appraisalRepository.GetByIdAsync(
            appraisalId,
            cancellationToken);

        if (appraisal is null)
        {
            return null;
        }

        appraisal.VehicleId = request.VehicleID;
        appraisal.AppraisalNumber = request.AppraisalNumber;
        appraisal.AppraisalRequestId = request.AppraisalRequestId;
        appraisal.ClientId = request.ClientId;
        appraisal.AssignedAppraiserId = request.AssignedAppraiserId;
        appraisal.AssignedReviewerId = request.AssignedReviewerId;
        appraisal.AppraisalType = request.AppraisalType ?? "Standard";
        appraisal.Status = request.Status;
        appraisal.IntakeStatus = request.IntakeStatus;
        appraisal.InspectionStatus = request.InspectionStatus;
        appraisal.ValuationStatus = request.ValuationStatus;
        appraisal.ReportStatus = request.ReportStatus;
        appraisal.EffectiveDateUtc = request.EffectiveDateUtc;
        appraisal.RowVersion = request.RowVersion;
        appraisal.UpdatedAtUtc = DateTime.UtcNow;

        var updated = await _appraisalRepository.UpdateAsync(appraisal, cancellationToken);

        if (updated is null)
        {
            throw new ConflictException(
                $"Appraisal '{appraisalId}' was modified by another request. Refresh it and retry with the current row version.");
        }

        return MapToResponse(updated);
    }

    public async Task<AppraisalListResponse> BrowseAsync(
        BrowseAppraisalsRequest request,
        CancellationToken cancellationToken)
    {
        var filter = new AppraisalBrowseFilter
        {
            VehicleId = request.VehicleId,
            ClientId = request.ClientId,
            AssignedAppraiserId = request.AssignedAppraiserId,
            AssignedReviewerId = request.AssignedReviewerId,
            Status = request.Status,
            AppraisalType = request.AppraisalType,
            Search = request.Search,
            EffectiveDateFromUtc = request.EffectiveDateFromUtc,
            EffectiveDateToUtc = request.EffectiveDateToUtc,
            Page = request.Page,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending
        };

        var result = await _appraisalRepository.BrowseAsync(filter, cancellationToken);

        return new AppraisalListResponse
        {
            Items = result.Items.Select(MapToResponse).ToArray(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    private static AppraisalResponse MapToResponse(AppraisalEntity appraisal)
    {
        return new AppraisalResponse
        {
            VehicleId = appraisal.VehicleId,
            AppraisalId = appraisal.AppraisalId,
            AppraisalNumber = appraisal.AppraisalNumber,
            AppraisalRequestId = appraisal.AppraisalRequestId,
            ClientId = appraisal.ClientId,
            AssignedAppraiserId = appraisal.AssignedAppraiserId,
            AssignedReviewerId = appraisal.AssignedReviewerId,
            AppraisalType = appraisal.AppraisalType,
            Status = appraisal.Status,
            IntakeStatus = appraisal.IntakeStatus,
            InspectionStatus = appraisal.InspectionStatus,
            ValuationStatus = appraisal.ValuationStatus,
            ReportStatus = appraisal.ReportStatus,
            EffectiveDateUtc = appraisal.EffectiveDateUtc,
            RowVersion = appraisal.RowVersion,
            CreatedAtUtc = appraisal.CreatedAtUtc,
            UpdatedAtUtc = appraisal.UpdatedAtUtc
        };
    }
}