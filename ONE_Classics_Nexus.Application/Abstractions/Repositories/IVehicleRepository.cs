using ONE_Classics_Nexus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Abstractions.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct);
    Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken ct);
}