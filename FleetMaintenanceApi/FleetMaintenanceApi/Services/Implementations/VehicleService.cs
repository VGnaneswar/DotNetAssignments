using FleetMaintenanceApi.DTOs;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using FleetMaintenanceApi.Services.Interfaces;

namespace FleetMaintenanceApi.Services.Implementations;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<List<VehicleResponseDto>> GetAllVehiclesAsync()
    {
        var vehicles = await _vehicleRepository.GetAllVehiclesAsync();
        return vehicles.Select(MapToResponseDto).ToList();
    }

    public async Task<VehicleResponseDto?> GetVehicleByIdAsync(int vehicleId)
    {
        var vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
        return vehicle is null ? null : MapToResponseDto(vehicle);
    }

    public async Task<(bool Success, string Message, VehicleResponseDto? Data)> AddVehicleAsync(VehicleCreateDto vehicleCreateDto)
    {
        if (string.IsNullOrWhiteSpace(vehicleCreateDto.VehicleNumber))
        {
            return (false, "Vehicle number is required", null);
        }

        if (string.IsNullOrWhiteSpace(vehicleCreateDto.VehicleType))
        {
            return (false, "Vehicle type is required", null);
        }

        if (string.IsNullOrWhiteSpace(vehicleCreateDto.Brand))
        {
            return (false, "Brand is required", null);
        }

        if (vehicleCreateDto.PurchaseYear <= 2000)
        {
            return (false, "Purchase year must be greater than 2000", null);
        }

        var vehicle = new Vehicle
        {
            VehicleNumber = vehicleCreateDto.VehicleNumber.Trim(),
            VehicleType = vehicleCreateDto.VehicleType.Trim(),
            Brand = vehicleCreateDto.Brand.Trim(),
            Model = vehicleCreateDto.Model.Trim(),
            PurchaseYear = vehicleCreateDto.PurchaseYear,
            IsActive = vehicleCreateDto.IsActive
        };

        await _vehicleRepository.AddVehicleAsync(vehicle);

        return (true, "Vehicle added successfully", MapToResponseDto(vehicle));
    }

    private static VehicleResponseDto MapToResponseDto(Vehicle vehicle)
    {
        return new VehicleResponseDto
        {
            VehicleId = vehicle.VehicleId,
            VehicleNumber = vehicle.VehicleNumber,
            VehicleType = vehicle.VehicleType,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            PurchaseYear = vehicle.PurchaseYear,
            IsActive = vehicle.IsActive
        };
    }
}
