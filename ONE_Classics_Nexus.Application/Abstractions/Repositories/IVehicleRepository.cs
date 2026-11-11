using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct);
    Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken ct);
    Task<Vehicle?> UpdateAsync(Vehicle vehicle, CancellationToken ct);
    Task<bool> ArchiveAsync(Guid vehicleId, CancellationToken ct);
    Task<(IReadOnlyList<Vehicle> Items, int TotalCount)> ListAsync(
        int page, int pageSize, string make, string model, int? year,
        string titleStatus, CancellationToken ct);
}