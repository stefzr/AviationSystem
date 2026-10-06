using Aviation.Api.Data;
using Microsoft.AspNetCore.Builder;
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
        });

        group.MapGet("/{id}", async (long id, AviationDbContext context) =>
        {
            var priority = await context.Priorities.FindAsync(id);
            return priority is not null ? Results.Ok(priority) : Results.NotFound();
        });
    }
}
