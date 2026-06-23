using FleetMaintenanceApi.DTOs;

namespace FleetMaintenanceApi.Services.Interfaces;

public interface IMaintenanceService
{
    Task<(bool Success, string Message, PagedResponseDto<MaintenanceResponseDto>? Data)> GetPagedMaintenanceRecordsAsync(MaintenanceFilterRequestDto filter);

    Task<(bool Success, string Message, MaintenanceResponseDto? Data)> AddMaintenanceRecordAsync(MaintenanceCreateDto maintenanceCreateDto);

    Task<(bool Success, string Message)> UpdateMaintenanceStatusAsync(int maintenanceId, string serviceStatus);

    Task<(bool Success, string Message)> DeleteMaintenanceRecordAsync(int maintenanceId);
}
