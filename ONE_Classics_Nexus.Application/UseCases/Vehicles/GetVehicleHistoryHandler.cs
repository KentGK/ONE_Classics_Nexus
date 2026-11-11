using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ONE_Classics_Nexus.Application.UseCases.Vehicles;

public sealed class GetVehicleHistoryHandler
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAppraisalRepository _appraisalRepository;
    private readonly IInspectionRepository _inspectionRepository;

    public GetVehicleHistoryHandler(
        IVehicleRepository vehicleRepository,
        IAppraisalRepository appraisalRepository,
        IInspectionRepository inspectionRepository)
    {
        _vehicleRepository = vehicleRepository;
        _appraisalRepository = appraisalRepository;
        _inspectionRepository = inspectionRepository;
    }

    public async Task<VehicleHistoryDto> HandleAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        if (vehicleId == Guid.Empty)
        {
            throw new ArgumentException("VehicleId cannot be empty.", nameof(vehicleId));
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);

        if (vehicle is null)
        {
            throw new NotFoundException($"Vehicle '{vehicleId}' was not found.");
        }

        var events = new List<VehicleHistoryEventDto>
        {
            new(
                EventType: "VehicleCreated",
                OccurredAtUtc: vehicle.CreatedAtUtc,
                Description: "Vehicle record created.",
                AppraisalId: null,
                InspectionId: null)
        };

        // 'UpdatedAtUtc' property does not exist on Vehicle. If you need to track updates, ensure the Vehicle entity has an appropriate property (e.g., 'LastModifiedAtUtc'). For now, this check is removed.
        // if (vehicle.UpdatedAtUtc > vehicle.CreatedAtUtc)
        {
            events.Add(new VehicleHistoryEventDto(
                EventType: "VehicleUpdated",
                // OccurredAtUtc: vehicle.UpdatedAtUtc,
                Description: "Vehicle record updated.",
                AppraisalId: null,
                InspectionId: null));
        }

        var page = 1;

        while (true)
        {
            var appraisalsPage = await _appraisalRepository.BrowseAsync(
                new AppraisalBrowseFilter
                {
                    VehicleId = vehicleId,
                    Page = page,
                    PageSize = 200,
                    SortBy = "updatedAtUtc",
                    SortDescending = true
                },
                cancellationToken);

            if (appraisalsPage.Items.Count == 0)
            {
                break;
            }

            foreach (var appraisal in appraisalsPage.Items)
            {
                events.Add(new VehicleHistoryEventDto(
                    EventType: "AppraisalCreated",
                    OccurredAtUtc: appraisal.CreatedAtUtc,
                    Description: $"Appraisal created with status '{appraisal.Status}'.",
                    AppraisalId: appraisal.AppraisalId,
                    InspectionId: null));

                if (appraisal.UpdatedAtUtc > appraisal.CreatedAtUtc)
                {
                    events.Add(new VehicleHistoryEventDto(
                        EventType: "AppraisalUpdated",
                        OccurredAtUtc: appraisal.UpdatedAtUtc,
                        Description: $"Appraisal updated with status '{appraisal.Status}'.",
                        AppraisalId: appraisal.AppraisalId,
                        InspectionId: null));
                }

                var inspections = await _inspectionRepository.GetByAppraisalIdAsync(
                    appraisal.AppraisalId,
                    cancellationToken);

                foreach (var inspection in inspections)
                {
                    events.Add(new VehicleHistoryEventDto(
                        EventType: "InspectionCreated",
                        OccurredAtUtc: inspection.CreatedAtUtc,
                        Description: $"Inspection created with status '{inspection.Status}'.",
                        AppraisalId: appraisal.AppraisalId,
                        InspectionId: inspection.InspectionId));

                    if (inspection.UpdatedAtUtc > inspection.CreatedAtUtc)
                    {
                        events.Add(new VehicleHistoryEventDto(
                            EventType: "InspectionUpdated",
                            OccurredAtUtc: inspection.UpdatedAtUtc,
                            Description: $"Inspection updated with status '{inspection.Status}'.",
                            AppraisalId: appraisal.AppraisalId,
                            InspectionId: inspection.InspectionId));
                    }
                }
            }

            var totalPages = appraisalsPage.PageSize <= 0
                ? 0
                : (int)Math.Ceiling(appraisalsPage.TotalCount / (double)appraisalsPage.PageSize);

            if (page >= totalPages)
            {
                break;
            }

            page++;
        }

        var orderedEvents = events
            .OrderByDescending(x => x.OccurredAtUtc)
            .ToArray();

        return new VehicleHistoryDto(vehicleId, orderedEvents);
    }
}
