namespace FleetMaintenanceApi.DTOs;

public class MaintenanceFilterRequestDto
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public int? VehicleId { get; set; }

    public int? DriverId { get; set; }

    public string? ServiceStatus { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? SortBy { get; set; } = "serviceDate";

    public string? SortDirection { get; set; } = "asc";
}
