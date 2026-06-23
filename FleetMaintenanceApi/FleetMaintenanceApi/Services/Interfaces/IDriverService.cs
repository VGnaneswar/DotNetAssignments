using FleetMaintenanceApi.DTOs;

namespace FleetMaintenanceApi.Services.Interfaces;

public interface IDriverService
{
    Task<List<DriverResponseDto>> GetAllDriversAsync();

    Task<DriverResponseDto?> GetDriverByIdAsync(int driverId);

    Task<(bool Success, string Message, DriverResponseDto? Data)> AddDriverAsync(DriverCreateDto driverCreateDto);
}
