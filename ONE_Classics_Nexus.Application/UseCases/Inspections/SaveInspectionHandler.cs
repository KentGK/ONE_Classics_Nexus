using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using ONE_Classics_Nexus.Application.Mappings;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.UseCases.Inspections;

public sealed class SaveInspectionHandler
{
    private readonly IInspectionRepository _inspectionRepository;

    public SaveInspectionHandler(IInspectionRepository inspectionRepository)
    {
        _inspectionRepository = inspectionRepository;
    }

    public async Task<InspectionDto> HandleAsync(SaveInspectionRequest request, CancellationToken ct)
    {
        if (request is null)
            throw new ValidationException("Request is required.");

        if (request.InspectionId == Guid.Empty)
            throw new ValidationException("InspectionId is required.");

        if (request.Scores is null || request.Scores.Count == 0)
            throw new ValidationException("At least one score is required.");

        var inspection = await _inspectionRepository.GetByIdAsync(request.InspectionId, ct);
        if (inspection is null)
            throw new NotFoundException("Inspection not found.");

        var scores = request.Scores
            .Select(x =>
            {
                if (x.SystemNumber < 1 || x.SystemNumber > 12)
                    throw new ValidationException("SystemNumber must be between 1 and 12.");

                if (string.IsNullOrWhiteSpace(x.SystemKey))
                    throw new ValidationException("SystemKey is required.");

                if (x.Score0To5 < 0m || x.Score0To5 > 5m)
                    throw new ValidationException("Score must be between 0 and 5.");

                if (x.Weight < 0m)
                    throw new ValidationException("Weight cannot be negative.");

                return ConditionScore.Create(
                    request.InspectionId,
                    x.SystemNumber,
                    x.SystemKey,
                    x.Score0To5,
                    x.Weight,
                    x.IsRedFlag,
                    x.Notes);
            })
            .ToList();

        await _inspectionRepository.SaveScoresAsync(request.InspectionId, scores, ct);

        var updated = await _inspectionRepository.GetByIdAsync(request.InspectionId, ct);
        if (updated is null)
            throw new NotFoundException("Inspection not found after save.");

        return updated.ToDto();
    }
}