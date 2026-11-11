using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Mappings;
using System;

namespace ONE_Classics_Nexus.Application.UseCases.Appraisals;

public sealed class GetAppraisalHandler
{
    private readonly IAppraisalRepository _appraisalRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public GetAppraisalHandler(
        IAppraisalRepository appraisalRepository,
        IVehicleRepository vehicleRepository)
    {
        _appraisalRepository = appraisalRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<AppraisalDetailDto> HandleAsync(
        Guid appraisalId,
        CancellationToken ct)
    {
        var appraisal = await _appraisalRepository.GetByIdAsync(appraisalId, ct);

        if (appraisal is null)
            throw new NotFoundException("Appraisal not found.");

        var vehicle = await _vehicleRepository.GetByIdAsync(appraisal.VehicleId, ct);

        if (vehicle is null)
            throw new NotFoundException("Vehicle not found.");

        return appraisal.ToDetailDto(vehicle);
    }
}