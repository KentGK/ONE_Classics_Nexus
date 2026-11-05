using FluentValidation;
using ONE_Classics_Nexus.API.Contracts.Vehicles;

namespace ONE_Classics_Nexus.API.Validation.Vehicles;

public sealed class CreateVehicleRequestValidator : AbstractValidator<CreateVehicleRequest>
{
    public CreateVehicleRequestValidator()
    {
        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Mileage must be greater than or equal to 0.");

        RuleFor(x => x.OdometerUnit)
            .Must(OdometerUnitValidation.IsAllowed)
            .WithMessage($"OdometerUnit must be one of: {string.Join(", ", OdometerUnitValidation.AllowedUnits)}.");

        RuleFor(x => x.OdometerStatus)
            .Must(OdometerStatusValidation.IsAllowed)
            .WithMessage($"OdometerStatus must be one of: {string.Join(", ", OdometerStatusValidation.AllowedStatuses)}.");

        RuleFor(x => x.TitleStatus)
            .Must(TitleStatusValidation.IsAllowed)
            .WithMessage($"TitleStatus must be one of: {string.Join(", ", TitleStatusValidation.AllowedStatuses)}.");

        RuleFor(x => x.AuthenticityProfile)
            .Must(AuthenticityProfileValidation.IsAllowed)
            .WithMessage($"AuthenticityProfile must be one of: {string.Join(", ", AuthenticityProfileValidation.AllowedProfiles)}.");

        RuleFor(x => x.OriginalityProfile)
            .Must(OriginalityProfileValidation.IsAllowed)
            .WithMessage($"OriginalityProfile must be one of: {string.Join(", ", OriginalityProfileValidation.AllowedProfiles)}.");

        RuleFor(x => x.RestorationLevel)
            .Must(RestorationLevelValidation.IsAllowed)
            .WithMessage($"RestorationLevel must be one of: {string.Join(", ", RestorationLevelValidation.AllowedLevels)}.");

        RuleFor(x => x.DocumentationLevel)
            .Must(DocumentationLevelValidation.IsAllowed)
            .WithMessage($"DocumentationLevel must be one of: {string.Join(", ", DocumentationLevelValidation.AllowedLevels)}.");

        RuleFor(x => x.VehicleStatus)
            .Must(VehicleStatusValidation.IsAllowed)
            .WithMessage($"VehicleStatus must be one of: {string.Join(", ", VehicleStatusValidation.AllowedStatuses)}.");
    }
}