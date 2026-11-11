using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Domain.Entities;
using System.Linq;

namespace ONE_Classics_Nexus.Application.Mappings;

public static class DomainMappings
{
    public static VehicleDto ToDto(this Vehicle vehicle) => new(
        vehicle.VehicleId,
        vehicle.Vin,
        vehicle.Year,
        vehicle.Make,
        vehicle.Model,
        vehicle.Trim,
        vehicle.BodyStyle,
        vehicle.Engine,
        vehicle.Transmission,
        vehicle.Drivetrain,
        vehicle.TitleStatus,
        vehicle.CreatedAtUtc);

    public static VehicleSummaryDto ToSummaryDto(this Vehicle vehicle) => new(
        vehicle.VehicleId,
        vehicle.Vin,
        vehicle.Year,
        vehicle.Make,
        vehicle.Model,
        vehicle.Trim,
        vehicle.BodyStyle,
        vehicle.TitleStatus);

    public static AppraisalDto ToDto(this Appraisal appraisal) => new(
        appraisal.AppraisalId,
        appraisal.VehicleId,
        appraisal.Status,
        appraisal.CreatedAtUtc,
        appraisal.UpdatedAtUtc,
        appraisal.RowVersion);

    public static ConditionScoreDto ToDto(this ConditionScore score) => new(
        score.ConditionScoreId,
        score.InspectionId,
        score.SystemNumber,
        score.SystemKey,
        score.Score0To5,
        score.Weight,
        score.IsRedFlag,
        score.Notes);

    public static InspectionDto ToDto(this Inspection inspection) => new(
        inspection.InspectionId,
        inspection.AppraisalId,
        inspection.InspectionType,
        inspection.CreatedAtUtc,
        inspection.UpdatedAtUtc,
        inspection.ConditionScores.Select(ToDto).ToList());
}