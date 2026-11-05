using System;

namespace ONE_Classics_Nexus.Application.Contracts.Vehicles
{
    public sealed record ListVehiclesQuery(
        int PageNumber = 1,
        int PageSize = 20,
        string? Search = null,
        string? SortBy = null,
        bool SortDescending = false);

    public sealed record VehicleListItemDto(
        Guid VehicleId,
        string Make,
        string Model,
        int Year,
        string? Vin,
        bool IsArchived,
        long RowVersion);
}