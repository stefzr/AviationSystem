using Aviation.Api.Data;
using Aviation.Api.Models;
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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Auto-seed μερικά αρχικά δεδομένα (mock data) για να φαίνεται γεμάτη η εφαρμογή μόλις ανοίξει
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AviationDbContext>();
    if (!context.MaintenanceTasks.Any())
    {
        context.MaintenanceTasks.AddRange(
            new MaintenanceTask { AircraftRegistration = "SX-DZA", Component = "Engine #1", Description = "Routine oil pressure check", Status = "PENDING", Priority = "HIGH" },
            new MaintenanceTask { AircraftRegistration = "SX-DZB", Component = "Landing Gear", Description = "Hydraulic fluid inspection", Status = "IN_PROGRESS", Priority = "AOG" }
        );
        context.SaveChanges();
    }
}

Log.Information("Aviation Workflow API is starting up...");
app.Run();