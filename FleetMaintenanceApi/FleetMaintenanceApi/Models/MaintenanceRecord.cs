namespace FleetMaintenanceApi.Models
{
    public class MaintenanceRecord
    {
        public int MaintenanceId { get; set; }

        public int VehicleId { get; set; }

        public int DriverId { get; set; }

        public DateOnly ServiceDate { get; set; }

        public string ServiceType { get; set; } = string.Empty;

        public decimal ServiceCost { get; set; }

        public string ServiceStatus { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation Properties

        public Vehicle? Vehicle { get; set; }

        public Driver? Driver { get; set; }
    }
}