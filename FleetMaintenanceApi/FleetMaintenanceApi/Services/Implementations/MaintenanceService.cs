using FleetMaintenanceApi.DTOs;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using FleetMaintenanceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Services.Implementations;

public class MaintenanceService : IMaintenanceService
{
    private static readonly HashSet<string> AllowedSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "maintenanceid",
        "servicedate",
        "servicetype",
        "servicecost",
        "servicestatus",
        "vehiclenumber",
        "drivername",
        "createddate"
    };

    private static readonly HashSet<string> AllowedSortDirections = new(StringComparer.OrdinalIgnoreCase)
    {
        "asc",
        "desc"
    };

    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "scheduled",
        "inprogress",
        "completed",
        "cancelled"
    };

    private readonly IMaintenanceRepository _maintenanceRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDriverRepository _driverRepository;

    public MaintenanceService(
        IMaintenanceRepository maintenanceRepository,
        IVehicleRepository vehicleRepository,
        IDriverRepository driverRepository)
    {
        _maintenanceRepository = maintenanceRepository;
        _vehicleRepository = vehicleRepository;
        _driverRepository = driverRepository;
    }

    public async Task<(bool Success, string Message, PagedResponseDto<MaintenanceResponseDto>? Data)> GetPagedMaintenanceRecordsAsync(MaintenanceFilterRequestDto filter)
    {
        if (filter.PageNumber <= 0)
        {
            return (false, "Page number must be greater than zero", null);
        }

        if (filter.PageSize <= 0)
        {
            return (false, "Page size must be greater than zero", null);
        }

        if (filter.PageSize > 100)
        {
            return (false, "Page size cannot be greater than 100", null);
        }

        var sortBy = string.IsNullOrWhiteSpace(filter.SortBy) ? "serviceDate" : filter.SortBy.Trim();
        var sortDirection = string.IsNullOrWhiteSpace(filter.SortDirection) ? "asc" : filter.SortDirection.Trim();

        if (!AllowedSortFields.Contains(sortBy))
        {
            return (false, "Invalid sort field", null);
        }

        if (!AllowedSortDirections.Contains(sortDirection))
        {
            return (false, "Invalid sort direction. Allowed values are asc and desc", null);
        }

        IQueryable<MaintenanceRecord> query = _maintenanceRepository.GetMaintenanceRecordsQueryable();

        if (filter.VehicleId.HasValue)
        {
            query = query.Where(record => record.VehicleId == filter.VehicleId.Value);
        }

        if (filter.DriverId.HasValue)
        {
            query = query.Where(record => record.DriverId == filter.DriverId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.ServiceStatus))
        {
            query = query.Where(record => record.ServiceStatus.ToLower() == filter.ServiceStatus.ToLower());
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(record => record.ServiceDate >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(record => record.ServiceDate <= filter.ToDate.Value);
        }

        query = ApplySorting(query, sortBy, sortDirection);

        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

        var records = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var response = new PagedResponseDto<MaintenanceResponseDto>
        {
            StatusCode = 200,
            Message = "Maintenance records retrieved successfully",
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            HasPreviousPage = filter.PageNumber > 1,
            HasNextPage = filter.PageNumber < totalPages,
            Data = records.Select(MapToResponseDto).ToList()
        };

        return (true, response.Message, response);
    }

    public async Task<(bool Success, string Message, MaintenanceResponseDto? Data)> AddMaintenanceRecordAsync(MaintenanceCreateDto maintenanceCreateDto)
    {
        if (!await _maintenanceRepository.GetMaintenanceRecordsQueryable().AnyAsync(record => false))
        {
        }

        if (!await _vehicleRepository.VehicleExistsAsync(maintenanceCreateDto.VehicleId))
        {
            return (false, "VehicleId must exist", null);
        }

        if (!await _driverRepository.DriverExistsAsync(maintenanceCreateDto.DriverId))
        {
            return (false, "DriverId must exist", null);
        }

        if (maintenanceCreateDto.ServiceDate == default)
        {
            return (false, "Service date is required", null);
        }

        if (maintenanceCreateDto.ServiceCost <= 0)
        {
            return (false, "Service cost must be greater than 0", null);
        }

        if (string.IsNullOrWhiteSpace(maintenanceCreateDto.ServiceStatus))
        {
            return (false, "Service status is required", null);
        }

        if (!AllowedStatuses.Contains(maintenanceCreateDto.ServiceStatus))
        {
            return (false, "Invalid service status", null);
        }

        if (string.IsNullOrWhiteSpace(maintenanceCreateDto.ServiceType))
        {
            return (false, "Service type is required", null);
        }

        var maintenanceRecord = new MaintenanceRecord
        {
            VehicleId = maintenanceCreateDto.VehicleId,
            DriverId = maintenanceCreateDto.DriverId,
            ServiceDate = maintenanceCreateDto.ServiceDate,
            ServiceType = maintenanceCreateDto.ServiceType.Trim(),
            ServiceCost = maintenanceCreateDto.ServiceCost,
            ServiceStatus = maintenanceCreateDto.ServiceStatus.Trim(),
            Remarks = string.IsNullOrWhiteSpace(maintenanceCreateDto.Remarks) ? null : maintenanceCreateDto.Remarks.Trim(),
            CreatedDate = DateTime.UtcNow
        };

        await _maintenanceRepository.AddMaintenanceRecordAsync(maintenanceRecord);

        var created = await _maintenanceRepository.GetMaintenanceRecordByIdAsync(maintenanceRecord.MaintenanceId);
        return (true, "Maintenance record added successfully", created is null ? null : MapToResponseDto(created));
    }

    public async Task<(bool Success, string Message)> UpdateMaintenanceStatusAsync(int maintenanceId, string serviceStatus)
    {
        if (maintenanceId <= 0)
        {
            return (false, "Maintenance id must be greater than zero");
        }

        if (string.IsNullOrWhiteSpace(serviceStatus))
        {
            return (false, "Service status is required");
        }

        if (!AllowedStatuses.Contains(serviceStatus))
        {
            return (false, "Invalid service status");
        }

        return (false, "Update maintenance status is not configured for this data layer");
    }

    public async Task<(bool Success, string Message)> DeleteMaintenanceRecordAsync(int maintenanceId)
    {
        if (maintenanceId <= 0)
        {
            return (false, "Maintenance id must be greater than zero");
        }

        var record = await _maintenanceRepository.GetMaintenanceRecordByIdAsync(maintenanceId);
        if (record is null)
        {
            return (false, "Maintenance record not found");
        }

        await _maintenanceRepository.DeleteMaintenanceRecordAsync(record);
        return (true, "Maintenance record deleted successfully");
    }

    private static IQueryable<MaintenanceRecord> ApplySorting(IQueryable<MaintenanceRecord> query, string sortBy, string sortDirection)
    {
        var descending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return sortBy.ToLowerInvariant() switch
        {
            "maintenanceid" => descending ? query.OrderByDescending(record => record.MaintenanceId) : query.OrderBy(record => record.MaintenanceId),
            "servicedate" => descending ? query.OrderByDescending(record => record.ServiceDate) : query.OrderBy(record => record.ServiceDate),
            "servicetype" => descending ? query.OrderByDescending(record => record.ServiceType) : query.OrderBy(record => record.ServiceType),
            "servicecost" => descending ? query.OrderByDescending(record => record.ServiceCost) : query.OrderBy(record => record.ServiceCost),
            "servicestatus" => descending ? query.OrderByDescending(record => record.ServiceStatus) : query.OrderBy(record => record.ServiceStatus),
            "vehiclenumber" => descending ? query.OrderByDescending(record => record.Vehicle!.VehicleNumber) : query.OrderBy(record => record.Vehicle!.VehicleNumber),
            "drivername" => descending ? query.OrderByDescending(record => record.Driver!.DriverName) : query.OrderBy(record => record.Driver!.DriverName),
            "createddate" => descending ? query.OrderByDescending(record => record.CreatedDate) : query.OrderBy(record => record.CreatedDate),
            _ => query.OrderBy(record => record.ServiceDate)
        };
    }

    private static MaintenanceResponseDto MapToResponseDto(MaintenanceRecord maintenanceRecord)
    {
        return new MaintenanceResponseDto
        {
            MaintenanceId = maintenanceRecord.MaintenanceId,
            VehicleId = maintenanceRecord.VehicleId,
            VehicleNumber = maintenanceRecord.Vehicle?.VehicleNumber ?? string.Empty,
            VehicleType = maintenanceRecord.Vehicle?.VehicleType ?? string.Empty,
            DriverId = maintenanceRecord.DriverId,
            DriverName = maintenanceRecord.Driver?.DriverName ?? string.Empty,
            ServiceDate = maintenanceRecord.ServiceDate,
            ServiceType = maintenanceRecord.ServiceType,
            ServiceCost = maintenanceRecord.ServiceCost,
            ServiceStatus = maintenanceRecord.ServiceStatus,
            Remarks = maintenanceRecord.Remarks,
            CreatedDate = maintenanceRecord.CreatedDate
        };
    }
}
