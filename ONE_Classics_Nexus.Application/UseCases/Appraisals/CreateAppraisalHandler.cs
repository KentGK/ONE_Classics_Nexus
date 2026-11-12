using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Mappings;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.UseCases.Appraisals;

public sealed class CreateAppraisalHandler
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAppraisalRepository _appraisalRepository;
    private readonly IAutoNumberGenerator _autoNumberGenerator;

    public CreateAppraisalHandler(
        IVehicleRepository vehicleRepository,
        IAppraisalRepository appraisalRepository,
        IAutoNumberGenerator autoNumberGenerator)
    {
        _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        _appraisalRepository = appraisalRepository ?? throw new ArgumentNullException(nameof(appraisalRepository));
        _autoNumberGenerator = autoNumberGenerator ?? throw new ArgumentNullException(nameof(autoNumberGenerator));
    }

    public async Task<AppraisalDto> HandleAsync(CreateAppraisalRequest request, CancellationToken ct)
    {
        if (request is null)
            throw new ValidationException("Request is required.");

        if (request.VehicleId == Guid.Empty)
            throw new ValidationException("VehicleId is required.");

        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, ct);
        if (vehicle is null)
            throw new NotFoundException("Vehicle not found.");

        var appraisalNumber = string.IsNullOrWhiteSpace(request.AppraisalNumber)
            ? await _autoNumberGenerator.NextAsync("appraisal", "AN", 6, ct)
            : request.AppraisalNumber.Trim();

        var appraisal = Appraisal.Create(request.VehicleId);
        appraisal.AppraisalNumber = appraisalNumber;
        appraisal.AppraisalRequestId = request.AppraisalRequestId;
        appraisal.ClientId = request.ClientId;
        appraisal.AssignedAppraiserId = request.AssignedAppraiserId;
        appraisal.AssignedReviewerId = request.AssignedReviewerId;
        appraisal.AppraisalType = string.IsNullOrWhiteSpace(request.AppraisalType)
            ? "Standard"
            : request.AppraisalType.Trim();
        appraisal.Status = string.IsNullOrWhiteSpace(request.Status) ? "Draft" : request.Status.Trim();
        appraisal.IntakeStatus = request.IntakeStatus;
        appraisal.InspectionStatus = request.InspectionStatus;
        appraisal.ValuationStatus = request.ValuationStatus;
        appraisal.ReportStatus = request.ReportStatus;
        appraisal.EffectiveDateUtc = request.EffectiveDateUtc;
        var created = await _appraisalRepository.CreateAsync(appraisal, ct);

        return created.ToDto();
    }
}