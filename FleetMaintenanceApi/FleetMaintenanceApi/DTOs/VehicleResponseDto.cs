namespace FleetMaintenanceApi.DTOs;

public class VehicleResponseDto
{
    public int VehicleId { get; set; }

    public string VehicleNumber { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int PurchaseYear { get; set; }

    public bool IsActive { get; set; }
}
