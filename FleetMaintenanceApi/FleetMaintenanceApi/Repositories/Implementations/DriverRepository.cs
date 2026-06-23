using FleetMaintenanceApi.Data;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Repositories.Implementations;

public class DriverRepository : IDriverRepository
{
    private readonly FleetMaintenanceDbContext _context;

    public DriverRepository(FleetMaintenanceDbContext context)
    {
        _context = context;
    }

    public Task<List<Driver>> GetAllDriversAsync()
    {
        return _context.Drivers.AsNoTracking().ToListAsync();
    }

    public Task<Driver?> GetDriverByIdAsync(int driverId)
    {
        return _context.Drivers.AsNoTracking().FirstOrDefaultAsync(driver => driver.DriverId == driverId);
    }

    public async Task AddDriverAsync(Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();
    }

    public Task<bool> DriverExistsAsync(int driverId)
    {
        return _context.Drivers.AnyAsync(driver => driver.DriverId == driverId);
    }
}
