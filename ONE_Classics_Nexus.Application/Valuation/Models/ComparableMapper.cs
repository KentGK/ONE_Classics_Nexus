using ONE_Classics_Nexus.Application.DTOs;
using ONE_Classics_Nexus.Application.Valuation.Models;

namespace ONE_Classics_Nexus.Application.Valuation.Models;

public static class ComparableMapper
{
    public static ComparableInput ToInput(ComparableDto dto)
        => new(
            ComparableId: dto.ComparableId,
            Year: dto.Year,
            Make: dto.Make,
            Model: dto.Model,
            Trim: dto.Trim,
            Source: dto.Source,
            SaleDate: dto.SaleDate,
            SalePrice: dto.SalePrice,
            BodyStyle: dto.BodyStyle,
            Drivetrain: dto.Drivetrain,
            Engine: dto.Engine,
            Transmission: dto.Transmission,
            Mileage: dto.Mileage,
            OriginalityLevel: dto.OriginalityLevel,
            RestorationLevel: dto.RestorationLevel,
            TitleStatus: dto.TitleStatus,
            OverallConditionScore: dto.OverallConditionScore,
            HasRedFlags: dto.HasRedFlags,
            Notes: dto.Notes
        );
}