using FleetMaintenanceApi.Models;

namespace FleetMaintenanceApi.Repositories.Interfaces;

public interface IMaintenanceRepository
{
    IQueryable<MaintenanceRecord> GetMaintenanceRecordsQueryable();

    Task AddMaintenanceRecordAsync(MaintenanceRecord maintenanceRecord);

    Task<MaintenanceRecord?> GetMaintenanceRecordByIdAsync(int maintenanceId);

    Task DeleteMaintenanceRecordAsync(MaintenanceRecord maintenanceRecord);
}
