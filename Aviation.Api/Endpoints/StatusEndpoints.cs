using Aviation.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Aviation.Api.Endpoints;

public static class StatusEndpoints
{
    public static void MapStatusEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Statuses");

        group.MapGet("/", async (AviationDbContext context) =>
        {
            var statuses = await context.Statuses.ToListAsync();
            return Results.Ok(statuses);
        });

        group.MapGet("/{id}", async (long id, AviationDbContext context) =>
        {
            var status = await context.Statuses.FindAsync(id);
            return status is not null ? Results.Ok(status) : Results.NotFound();
        });
    }
}
