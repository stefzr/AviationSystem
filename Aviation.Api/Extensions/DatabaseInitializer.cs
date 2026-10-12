using Aviation.Api.Data;
using Aviation.Api.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Aviation.Api.Extensions;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        var connectionString = app.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AviationDbContext>();
        var serverConnectionString = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString;
        var serverCheckOptions = new DbContextOptionsBuilder<AviationDbContext>()
            .UseSqlServer(serverConnectionString)
            .Options;
        await using var serverCheckContext = new AviationDbContext(serverCheckOptions);

        if (!await serverCheckContext.Database.CanConnectAsync())
        {
            Log.Error("SQL Server is unavailable. Database migrations and data seeding cannot continue.");
            throw new InvalidOperationException("SQL Server is unavailable.");
        }

        await context.Database.MigrateAsync();
        await SeedDataAsync(context);
    }

    private static async Task SeedDataAsync(AviationDbContext context)
    {
        var seededAt = DateTime.UtcNow;

        if (!await context.Statuses.AnyAsync())
        {
            context.Statuses.AddRange(
                new Status { Name = "Pending", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new Status { Name = "In Progress", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new Status { Name = "Completed", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new Status { Name = "On Hold", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" }
            );
        }

        if (!await context.Priorities.AnyAsync())
        {
            context.Priorities.AddRange(
                new Priority { Name = "Low", Severity = 1, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new Priority { Name = "Medium", Severity = 2, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new Priority { Name = "Critical", Severity = 3, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" }
            );
        }

        await context.SaveChangesAsync();

        if (!await context.MaintenanceTasks.AnyAsync())
        {
            var statuses = await context.Statuses.ToDictionaryAsync(status => status.Name);
            var priorities = await context.Priorities.ToDictionaryAsync(priority => priority.Name);

            context.MaintenanceTasks.AddRange(
                new MaintenanceTask { AircraftTailNumber = "SX-DZA", AircraftRegistration = "SX-DZA", Component = "CFM56-7B Engine No. 1", Description = "Inspect the No. 1 engine oil filter for metal particles following an elevated oil pressure differential indication.", StatusId = statuses["In Progress"].Id, PriorityId = priorities["Critical"].Id, CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new MaintenanceTask { AircraftTailNumber = "SX-DZB", AircraftRegistration = "SX-DZB", Component = "Main Landing Gear", Description = "Investigate hydraulic fluid seepage at the left main landing gear actuator and perform an operational retraction test.", StatusId = statuses["Pending"].Id, PriorityId = priorities["Medium"].Id, CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new MaintenanceTask { AircraftTailNumber = "SX-DZC", AircraftRegistration = "SX-DZC", Component = "Pitot-Static System", Description = "Complete the scheduled pitot-static leak test and verify the standby altimeter and airspeed indicator against calibrated test equipment.", StatusId = statuses["Pending"].Id, PriorityId = priorities["Low"].Id, CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new MaintenanceTask { AircraftTailNumber = "SX-DZD", AircraftRegistration = "SX-DZD", Component = "APU Starter Generator", Description = "Replace the unserviceable APU starter generator after repeated start faults; carry out an operational test before release to service.", StatusId = statuses["On Hold"].Id, PriorityId = priorities["Critical"].Id, CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
                new MaintenanceTask { AircraftTailNumber = "SX-DZE", AircraftRegistration = "SX-DZE", Component = "Flap Drive Transmission", Description = "Inspect flap drive transmission lubrication and synchronisation after a post-flight asymmetry indication; record backlash measurements.", StatusId = statuses["Completed"].Id, PriorityId = priorities["Medium"].Id, CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" }
            );
        }

        await context.SaveChangesAsync();
    }
}
