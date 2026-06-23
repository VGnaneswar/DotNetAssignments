using FleetMaintenanceApi.DTOs;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using FleetMaintenanceApi.Services.Interfaces;

namespace FleetMaintenanceApi.Services.Implementations;

public class DriverService : IDriverService
{
    private readonly IDriverRepository _driverRepository;

    public DriverService(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<List<DriverResponseDto>> GetAllDriversAsync()
    {
        var drivers = await _driverRepository.GetAllDriversAsync();
        return drivers.Select(MapToResponseDto).ToList();
    }

    public async Task<DriverResponseDto?> GetDriverByIdAsync(int driverId)
    {
        var driver = await _driverRepository.GetDriverByIdAsync(driverId);
        return driver is null ? null : MapToResponseDto(driver);
    }

    public async Task<(bool Success, string Message, DriverResponseDto? Data)> AddDriverAsync(DriverCreateDto driverCreateDto)
    {
        if (string.IsNullOrWhiteSpace(driverCreateDto.DriverName))
        {
            return (false, "Driver name is required", null);
        }

        if (string.IsNullOrWhiteSpace(driverCreateDto.LicenseNumber))
        {
            return (false, "License number is required", null);
        }

        if (string.IsNullOrWhiteSpace(driverCreateDto.PhoneNumber))
        {
            return (false, "Phone number is required", null);
        }

        if (string.IsNullOrWhiteSpace(driverCreateDto.City))
        {
            return (false, "City is required", null);
        }

        var driver = new Driver
        {
            DriverName = driverCreateDto.DriverName.Trim(),
            LicenseNumber = driverCreateDto.LicenseNumber.Trim(),
            PhoneNumber = driverCreateDto.PhoneNumber.Trim(),
            City = driverCreateDto.City.Trim(),
            IsAvailable = driverCreateDto.IsAvailable
        };

        await _driverRepository.AddDriverAsync(driver);

        return (true, "Driver added successfully", MapToResponseDto(driver));
    }

    private static DriverResponseDto MapToResponseDto(Driver driver)
    {
        return new DriverResponseDto
        {
            DriverId = driver.DriverId,
            DriverName = driver.DriverName,
            LicenseNumber = driver.LicenseNumber,
            PhoneNumber = driver.PhoneNumber,
            City = driver.City,
            IsAvailable = driver.IsAvailable
        };
    }
}
