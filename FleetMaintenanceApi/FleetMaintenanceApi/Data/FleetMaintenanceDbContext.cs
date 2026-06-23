using FleetMaintenanceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Data
{
    public class FleetMaintenanceDbContext : DbContext
    {
        public FleetMaintenanceDbContext(DbContextOptions<FleetMaintenanceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceRecord>()
                .HasKey(m => m.MaintenanceId);

            modelBuilder.Entity<MaintenanceRecord>()
                .Property(m => m.ServiceCost)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}