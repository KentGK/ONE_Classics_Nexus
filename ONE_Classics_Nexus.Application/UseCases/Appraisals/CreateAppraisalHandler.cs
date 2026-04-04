using ONE_Classics_Nexus.Application.Abstractions.Repositories;
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

    public CreateAppraisalHandler(
        IVehicleRepository vehicleRepository,
        IAppraisalRepository appraisalRepository)
    {
        _vehicleRepository = vehicleRepository;
        _appraisalRepository = appraisalRepository;
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

        var appraisal = Appraisal.Create(request.VehicleId);
        var created = await _appraisalRepository.CreateAsync(appraisal, ct);

        return created.ToDto();
    }
}