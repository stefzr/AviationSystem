using Aviation.Api.Data;
using Aviation.Api.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
        })
        .WithTags("System Lookups")
        .WithSummary("Retrieves all statuses")
        .Produces<List<Status>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (long id, AviationDbContext context) =>
        {
            var status = await context.Statuses.FindAsync(id);
            return status is not null ? Results.Ok(status) : Results.NotFound();
        })
        .WithTags("System Lookups")
        .WithSummary("Retrieves a status by ID")
        .Produces<Status>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
