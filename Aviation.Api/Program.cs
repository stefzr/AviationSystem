using Aviation.Api.Endpoints;
using Aviation.Api.Data;
using Aviation.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Config Serilog για δομημένο logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Προσθήκη In-Memory Database για το Entity Framework
builder.Services.AddDbContext<AviationDbContext>(options =>
    options.UseInMemoryDatabase("AviationWorkflowDb"));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Aviation Workflow API",
        Version = "v0.1.0",
        Description = "Σύστημα παρακολούθησης εργασιών συντήρησης και ανταλλακτικών αεροσκαφών.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Στέφανος",
            Url = new Uri("https://github.com/stefzr/AviationSystem")
        }
    });
});

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapMaintenanceTaskEndpoints();
app.MapStatusEndpoints();
app.MapPriorityEndpoints();

// Auto-seed μερικά αρχικά δεδομένα (mock data) για να φαίνεται γεμάτη η εφαρμογή μόλις ανοίξει
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AviationDbContext>();
    var seededAt = DateTime.UtcNow;

    if (!context.Statuses.Any())
    {
        context.Statuses.AddRange(
            new Status { Name = "Pending", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new Status { Name = "In Progress", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new Status { Name = "Completed", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new Status { Name = "On Hold", CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" }
        );
    }

    if (!context.Priorities.Any())
    {
        context.Priorities.AddRange(
            new Priority { Name = "Low", Severity = 1, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new Priority { Name = "Medium", Severity = 2, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new Priority { Name = "Critical", Severity = 3, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" }
        );
    }

    if (!context.MaintenanceTasks.Any())
    {
        context.MaintenanceTasks.AddRange(
            new MaintenanceTask { AircraftTailNumber = "SX-DZA", AircraftRegistration = "SX-DZA", Component = "CFM56-7B Engine No. 1", Description = "Inspect the No. 1 engine oil filter for metal particles following an elevated oil pressure differential indication.", Status = "In Progress", Priority = "Critical", CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new MaintenanceTask { AircraftTailNumber = "SX-DZB", AircraftRegistration = "SX-DZB", Component = "Main Landing Gear", Description = "Investigate hydraulic fluid seepage at the left main landing gear actuator and perform an operational retraction test.", Status = "Pending", Priority = "Medium", CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new MaintenanceTask { AircraftTailNumber = "SX-DZC", AircraftRegistration = "SX-DZC", Component = "Pitot-Static System", Description = "Complete the scheduled pitot-static leak test and verify the standby altimeter and airspeed indicator against calibrated test equipment.", Status = "Pending", Priority = "Low", CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new MaintenanceTask { AircraftTailNumber = "SX-DZD", AircraftRegistration = "SX-DZD", Component = "APU Starter Generator", Description = "Replace the unserviceable APU starter generator after repeated start faults; carry out an operational test before release to service.", Status = "On Hold", Priority = "Critical", CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" },
            new MaintenanceTask { AircraftTailNumber = "SX-DZE", AircraftRegistration = "SX-DZE", Component = "Flap Drive Transmission", Description = "Inspect flap drive transmission lubrication and synchronisation after a post-flight asymmetry indication; record backlash measurements.", Status = "Completed", Priority = "Medium", CreatedAt = seededAt, CreatedOn = seededAt, CreatedBy = "System Seed", LastUpdatedOn = seededAt, LastUpdatedBy = "System Seed" }
        );
    }

    context.SaveChanges();
}

Log.Information("Aviation Workflow API is starting up...");
app.Run();