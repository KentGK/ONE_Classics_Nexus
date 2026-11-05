using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ONE_Classics_Nexus.WinForms.Forms.Inspections.Contracts;

namespace ONE_Classics_Nexus.WinForms.Forms.Inspections.Services;

public sealed class MockInspectionWorkspaceApiClient : IInspectionWorkspaceApiClient
{
    private readonly List<InspectionListItemResponse> _list;
    private InspectionWorkspaceResponse _workspace;

    public MockInspectionWorkspaceApiClient()
    {
        var inspectionId = Guid.Parse("b2a3f6bb-5b7a-4d7f-9a1d-3d4a6d1f0e21");
        var appraisalId = Guid.Parse("e4f8c9a1-2b3c-4d5e-9f6a-1c2b3d4e5f6a");
        var vehicleId = Guid.Parse("9d87bca2-5f13-4d44-b9b8-4963bd8d92e1");

        _list = new List<InspectionListItemResponse>
        {
            new()
            {
                InspectionId = inspectionId,
                AppraisalId = appraisalId,
                Status = "InProgress",
                InspectionType = "Physical",
                CreatedAtUtc = new DateTime(2025, 7, 21, 15, 15, 0, DateTimeKind.Utc),
                UpdatedAtUtc = new DateTime(2025, 7, 21, 16, 2, 0, DateTimeKind.Utc)
            },
            new()
            {
                InspectionId = Guid.Parse("1c6d2c1b-3e8a-4b21-8c9f-9b6e9a8d7d11"),
                AppraisalId = Guid.NewGuid(),
                Status = "Draft",
                InspectionType = "Standard",
                CreatedAtUtc = new DateTime(2025, 7, 10, 14, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                InspectionId = Guid.Parse("4e7d8f19-9b2c-4f3a-a7c8-2a1c2f8e9b33"),
                AppraisalId = Guid.NewGuid(),
                Status = "Completed",
                InspectionType = "VintageClassic",
                CreatedAtUtc = new DateTime(2025, 6, 28, 14, 0, 0, DateTimeKind.Utc)
            }
        };

        _workspace = new InspectionWorkspaceResponse
        {
            InspectionId = inspectionId,
            AppraisalId = appraisalId,
            InspectionTemplateId = Guid.Parse("b95932c7-42c0-4c4b-a8c6-4a4c2fe5a594"),
            InspectedByUserId = Guid.NewGuid(),
            InspectedByDisplayName = "Kent Klingler",
            InspectionType = "Physical",
            Status = "InProgress",
            TemplateName = "Standard Vehicle Inspection v1.0",
            InspectionLocation = "Dallas, TX",
            InspectedOnUtc = new DateTime(2025, 7, 21, 15, 0, 0, DateTimeKind.Utc),
            SummaryNotes =
                "Vehicle presents well overall.\r\n" +
                "Minor surface rust on rear frame rails.\r\n" +
                "Brake system needs service (front calipers sticking).\r\n" +
                "Electrical system shows aged wiring in engine bay.",
            CompletionPercentage = 75m,
            OverallConditionScore = 76.60m,
            HasRedFlags = true,
            RowVersion = 4,
            CreatedAtUtc = new DateTime(2025, 7, 21, 15, 15, 0, DateTimeKind.Utc),
            UpdatedAtUtc = new DateTime(2025, 7, 21, 16, 2, 0, DateTimeKind.Utc),
            ItemsScored = 47,
            TotalItems = 63,
            Appraisal = new AppraisalSummaryResponse
            {
                AppraisalId = appraisalId,
                AppraisalNumber = "APP-2025-000123",
                VehicleId = vehicleId,
                Status = "In Progress",
                AppraisalType = "Market Value",
                EffectiveDateUtc = new DateTime(2025, 7, 21, 0, 0, 0, DateTimeKind.Utc),
                ClientName = "ONE Classics Demo Client"
            },
            Vehicle = new VehicleSummaryResponse
            {
                VehicleId = vehicleId,
                Vin = "1GCEK14H1HZ123456",
                Year = 1970,
                Make = "Chevrolet",
                Model = "Chevelle SS",
                BodyStyle = "2 Door Coupe",
                Drivetrain = "RWD",
                Engine = "454 V8",
                Transmission = "4-Speed Muncie",
                Mileage = 52345,
                OdometerUnit = "mi"
            },
            ConditionScores = BuildScores(inspectionId),
            SectionSummaries = BuildSections(),
            RedFlags = new[]
            {
                new RedFlagSummaryResponse
                {
                    RedFlagId = Guid.NewGuid(),
                    SystemNumber = 6,
                    SystemName = "Braking System",
                    Severity = "High",
                    Title = "Brake Fluid Leak",
                    ItemName = "Front Caliper",
                    Description = "Leak detected at left front caliper."
                },
                new RedFlagSummaryResponse
                {
                    RedFlagId = Guid.NewGuid(),
                    SystemNumber = 7,
                    SystemName = "Electrical System",
                    Severity = "Medium",
                    Title = "Aged Wiring",
                    ItemName = "Engine Bay Harness",
                    Description = "Insulation brittle in several areas."
                }
            },
            MediaSummary = new MediaSummaryResponse
            {
                PhotoCount = 18,
                VideoCount = 2,
                DocumentCount = 3,
                OtherFileCount = 1
            }
        };
    }

    public Task<IReadOnlyList<InspectionListItemResponse>> GetInspectionListAsync(
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<InspectionListItemResponse>>(_list);

    public Task<InspectionWorkspaceResponse> GetWorkspaceAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        if (inspectionId == _workspace.InspectionId)
            return Task.FromResult(_workspace);

        var item = _list.First(x => x.InspectionId == inspectionId);
        var copy = new InspectionWorkspaceResponse
        {
            InspectionId = item.InspectionId,
            AppraisalId = item.AppraisalId,
            InspectionTemplateId = _workspace.InspectionTemplateId,
            InspectionType = item.InspectionType,
            Status = item.Status,
            TemplateName = _workspace.TemplateName,
            CreatedAtUtc = item.CreatedAtUtc,
            UpdatedAtUtc = item.UpdatedAtUtc ?? item.CreatedAtUtc,
            RowVersion = 1,
            ConditionScores = Array.Empty<ConditionScoreResponse>()
        };

        return Task.FromResult(copy);
    }

    public Task UpdateInspectionAsync(
        Guid inspectionId,
        UpdateInspectionRequest request,
        CancellationToken cancellationToken = default)
    {
        _workspace.Status = request.Status;
        _workspace.InspectionType = request.InspectionType;
        _workspace.InspectionLocation = request.InspectionLocation;
        _workspace.InspectedOnUtc = request.InspectedOnUtc;
        _workspace.SummaryNotes = request.SummaryNotes;
        _workspace.RowVersion++;
        _workspace.UpdatedAtUtc = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    public Task SaveConditionScoresAsync(
        Guid inspectionId,
        SaveConditionScoresRequest request,
        CancellationToken cancellationToken = default)
    {
        _workspace.ConditionScores = request.Scores.Select(x => new ConditionScoreResponse
        {
            ConditionScoreId = x.ConditionScoreId,
            InspectionId = inspectionId,
            SystemNumber = x.SystemNumber,
            SystemKey = x.SystemKey,
            Score0To5 = x.Score0To5,
            Weight = x.Weight,
            IsRedFlag = x.IsRedFlag,
            Notes = x.Notes
        }).ToList();

        return Task.CompletedTask;
    }

    public Task DeleteInspectionAsync(
        Guid inspectionId,
        CancellationToken cancellationToken = default)
    {
        _list.RemoveAll(x => x.InspectionId == inspectionId);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }

    private static IReadOnlyCollection<ConditionScoreResponse> BuildScores(Guid inspectionId)
    {
        var values = new[]
        {
            (1, "Body & Exterior", "body_exterior", 3.50m, 0.06m, false),
            (2, "Frame & Structure", "frame_structure", 4.00m, 0.10m, false),
            (3, "Engine / Powertrain", "powertrain", 4.50m, 0.18m, false),
            (4, "Transmission & Driveline", "transmission_driveline", 4.00m, 0.14m, false),
            (5, "Suspension & Steering", "suspension_steering", 3.50m, 0.10m, false),
            (6, "Braking System", "braking_system", 3.00m, 0.10m, true),
            (7, "Electrical System", "electrical_system", 3.50m, 0.10m, true),
            (8, "Interior", "interior", 4.00m, 0.04m, false),
            (9, "Wheels & Tires", "wheels_tires", 3.50m, 0.06m, false),
            (10, "Trim, Glass & Details", "trim_glass_details", 3.00m, 0.02m, false),
            (11, "Documentation & Provenance", "documentation_provenance", 4.50m, 0.04m, false),
            (12, "Road Test & Functional", "road_test_functional", 4.00m, 0.06m, false)
        };

        return values.Select(x => new ConditionScoreResponse
        {
            ConditionScoreId = Guid.NewGuid(),
            InspectionId = inspectionId,
            SystemNumber = x.Item1,
            SystemName = x.Item2,
            SystemKey = x.Item3,
            Score0To5 = x.Item4,
            Weight = x.Item5,
            IsRedFlag = x.Item6
        }).ToArray();
    }

    private static IReadOnlyCollection<SectionSummaryResponse> BuildSections()
    {
        return new[]
        {
            new SectionSummaryResponse { SystemNumber = 1, SectionName = "Body & Exterior", Score0To5 = 3.50m, ItemsScored = 6, TotalItems = 7 },
            new SectionSummaryResponse { SystemNumber = 2, SectionName = "Frame & Structure", Score0To5 = 4.00m, ItemsScored = 5, TotalItems = 6 },
            new SectionSummaryResponse { SystemNumber = 3, SectionName = "Engine / Powertrain", Score0To5 = 4.50m, ItemsScored = 7, TotalItems = 8 },
            new SectionSummaryResponse { SystemNumber = 4, SectionName = "Transmission & Driveline", Score0To5 = 4.00m, ItemsScored = 6, TotalItems = 6 },
            new SectionSummaryResponse { SystemNumber = 5, SectionName = "Suspension & Steering", Score0To5 = 3.50m, ItemsScored = 6, TotalItems = 6 },
            new SectionSummaryResponse { SystemNumber = 6, SectionName = "Braking System", Score0To5 = 3.00m, ItemsScored = 5, TotalItems = 6, RedFlagCount = 1 },
            new SectionSummaryResponse { SystemNumber = 7, SectionName = "Electrical System", Score0To5 = 3.50m, ItemsScored = 6, TotalItems = 6, RedFlagCount = 1 },
            new SectionSummaryResponse { SystemNumber = 8, SectionName = "Interior", Score0To5 = 4.00m, ItemsScored = 6, TotalItems = 6 },
            new SectionSummaryResponse { SystemNumber = 9, SectionName = "Wheels & Tires", Score0To5 = 3.50m, ItemsScored = 4, TotalItems = 5 },
            new SectionSummaryResponse { SystemNumber = 10, SectionName = "Trim, Glass & Details", Score0To5 = 3.00m, ItemsScored = 5, TotalItems = 5 },
            new SectionSummaryResponse { SystemNumber = 11, SectionName = "Documentation & Provenance", Score0To5 = 4.50m, ItemsScored = 4, TotalItems = 4 },
            new SectionSummaryResponse { SystemNumber = 12, SectionName = "Road Test & Functional", Score0To5 = 4.00m, ItemsScored = 5, TotalItems = 5 }
        };
    }
}
