using FleetMaintenanceApi.Data;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Repositories.Implementations;

public class MaintenanceRepository : IMaintenanceRepository
{
    private readonly FleetMaintenanceDbContext _context;

    public MaintenanceRepository(FleetMaintenanceDbContext context)
    {
        _context = context;
    }

    public IQueryable<MaintenanceRecord> GetMaintenanceRecordsQueryable()
    {
        return _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .Include(m => m.Driver)
            .AsNoTracking()
            .AsQueryable();
    }

    public async Task AddMaintenanceRecordAsync(MaintenanceRecord maintenanceRecord)
    {
        _context.MaintenanceRecords.Add(maintenanceRecord);
        await _context.SaveChangesAsync();
    }

    public Task<MaintenanceRecord?> GetMaintenanceRecordByIdAsync(int maintenanceId)
    {
        return _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .Include(m => m.Driver)
            .FirstOrDefaultAsync(m => m.MaintenanceId == maintenanceId);
    }

    public async Task DeleteMaintenanceRecordAsync(MaintenanceRecord maintenanceRecord)
    {
        _context.MaintenanceRecords.Remove(maintenanceRecord);
        await _context.SaveChangesAsync();
    }
}
