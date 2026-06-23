using FleetMaintenanceApi.Models;

namespace FleetMaintenanceApi.Repositories.Interfaces;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllVehiclesAsync();

    Task<Vehicle?> GetVehicleByIdAsync(int vehicleId);

    Task AddVehicleAsync(Vehicle vehicle);

    Task<bool> VehicleExistsAsync(int vehicleId);
}
