namespace FleetMaintenanceApi.DTOs;

public class DriverCreateDto
{
    public string DriverName { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }
}
