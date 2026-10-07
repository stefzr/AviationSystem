using Aviation.Api.Data;
using Aviation.Api.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Aviation.Api.Endpoints;

public static class PriorityEndpoints
{
    public static void MapPriorityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Priorities");

        group.MapGet("/", async (AviationDbContext context) =>
        {
            var priorities = await context.Priorities.ToListAsync();
            return Results.Ok(priorities);
        })
        .WithTags("System Lookups")
        .WithSummary("Retrieves all priorities")
        .Produces<List<Priority>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (long id, AviationDbContext context) =>
        {
            var priority = await context.Priorities.FindAsync(id);
            return priority is not null ? Results.Ok(priority) : Results.NotFound();
        })
        .WithTags("System Lookups")
        .WithSummary("Retrieves a priority by ID")
        .Produces<Priority>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
