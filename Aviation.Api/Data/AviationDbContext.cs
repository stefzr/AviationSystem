using Microsoft.EntityFrameworkCore;
using Aviation.Api.Entities;

namespace Aviation.Api.Data
{
    public class AviationDbContext : DbContext
    {
        public AviationDbContext(DbContextOptions<AviationDbContext> options) : base(options) { }

        public DbSet<MaintenanceTask> MaintenanceTasks => Set<MaintenanceTask>();
    }
}