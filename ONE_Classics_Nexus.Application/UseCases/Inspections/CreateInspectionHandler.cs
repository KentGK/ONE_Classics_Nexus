using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Mappings;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Domain.Constants;
using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.UseCases.Inspections;

public sealed class CreateInspectionHandler
{
    private readonly IAppraisalRepository _appraisalRepository;
    private readonly IInspectionRepository _inspectionRepository;

    public CreateInspectionHandler(
        IAppraisalRepository appraisalRepository,
        IInspectionRepository inspectionRepository)
    {
        _appraisalRepository = appraisalRepository;
        _inspectionRepository = inspectionRepository;
    }

    public async Task<InspectionDto> HandleAsync(CreateInspectionRequest request, CancellationToken ct)
    {
        if (request is null)
            throw new ValidationException("Request is required.");

        if (request.AppraisalId == Guid.Empty)
            throw new ValidationException("AppraisalId is required.");

        if (string.IsNullOrWhiteSpace(request.InspectionType))
            throw new ValidationException("InspectionType is required.");

        var appraisal = await _appraisalRepository.GetByIdAsync(request.AppraisalId, ct);
        if (appraisal is null)
            throw new NotFoundException("Appraisal not found.");

        var inspection = Inspection.Create(request.AppraisalId, request.InspectionType);

        foreach (var item in InspectionSystems.All)
        {
            inspection.ConditionScores.Add(ConditionScore.Create(
                inspection.InspectionId,
                item.SystemNumber,
                item.SystemKey,
                0m,
                item.Weight,
                false,
                null));
        }

        var created = await _inspectionRepository.CreateAsync(inspection, ct);
        return created.ToDto();
    }
}