using FleetMaintenanceApi.DTOs;

namespace FleetMaintenanceApi.Services.Interfaces;

public interface IVehicleService
{
    Task<List<VehicleResponseDto>> GetAllVehiclesAsync();

    Task<VehicleResponseDto?> GetVehicleByIdAsync(int vehicleId);

    Task<(bool Success, string Message, VehicleResponseDto? Data)> AddVehicleAsync(VehicleCreateDto vehicleCreateDto);
}
