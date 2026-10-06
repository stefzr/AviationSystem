using Microsoft.EntityFrameworkCore;
using Aviation.Api.Entities;

namespace Aviation.Api.Data
{
    public class AviationDbContext : DbContext
    {   
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Εντοπίζει και εφαρμόζει όλα τα Configuration αρχεία
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AviationDbContext).Assembly);
        }
        public AviationDbContext(DbContextOptions<AviationDbContext> options) : base(options) { }

        public DbSet<MaintenanceTask> MaintenanceTasks => Set<MaintenanceTask>();
    }
}