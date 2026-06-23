using FleetMaintenanceApi.Data;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Repositories.Implementations;

public class VehicleRepository : IVehicleRepository
{
    private readonly FleetMaintenanceDbContext _context;

    public VehicleRepository(FleetMaintenanceDbContext context)
    {
        _context = context;
    }

    public Task<List<Vehicle>> GetAllVehiclesAsync()
    {
        return _context.Vehicles.AsNoTracking().ToListAsync();
    }

    public Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
    {
        return _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(vehicle => vehicle.VehicleId == vehicleId);
    }

    public async Task AddVehicleAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
    }

    public Task<bool> VehicleExistsAsync(int vehicleId)
    {
        return _context.Vehicles.AnyAsync(vehicle => vehicle.VehicleId == vehicleId);
    }
}
