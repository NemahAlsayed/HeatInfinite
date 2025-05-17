using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Endpoint: Heat Solver API
app.MapGet("/solve", async (HttpContext context) =>
{
    var query = context.Request.Query;

    // Validate input
    if (!query.ContainsKey("dx") || !double.TryParse(query["dx"], out var parsedDx))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Invalid or missing 'dx' parameter." }));
        return;
    }

    double dx = parsedDx / 100.0;
    double dy = dx;

    try
    {
        var result = HeatSolver.SolveHeatEquation(dx, dy); // Use the new class
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Internal Server Error", details = ex.Message }));
    }
});

app.Run();
