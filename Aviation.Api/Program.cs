using Aviation.Api.Data;
using Aviation.Api.Endpoints;
using Aviation.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Config Serilog για δομημένο logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Σύνδεση στη βάση SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

builder.Services.AddDbContext<AviationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAviationSpa", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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

app.UseCors("AllowAviationSpa");

app.UseAuthorization();

app.MapMaintenanceTaskEndpoints();
app.MapStatusEndpoints();
app.MapPriorityEndpoints();

Log.Information("Aviation Workflow API is starting up...");
await app.InitializeDatabaseAsync();
app.Run();