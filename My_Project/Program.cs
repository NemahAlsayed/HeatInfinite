using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(); // Enable Razor Pages for ViewGrid display

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });

// Middleware for Global Error Handling
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Critical Error: {ex}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Internal Server Error", details = ex.Message }));
    }
});

// Root Endpoint (Sanity Check)
app.MapGet("/", () => "✅ Server is running!");

// Heat Equation Solver Endpoint
app.MapGet("/solve", async (HttpContext context) =>
{
    Console.WriteLine($"Received request: {context.Request.QueryString}");

    try
    {
        var query = context.Request.Query;

        if (!query.ContainsKey("dx") || !double.TryParse(query["dx"], out var parsedDx))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Invalid or missing 'dx' parameter." }));
            return;
        }

        double dx = parsedDx / 100.0;
        double dy = dx;

        var result = HeatSolver.SolveHeatEquation(dx, dy);

        // Convert `double[,]` to `List<List<double>>` for Razor View compatibility
        var formattedMatrix = new List<List<double>>();
        for (int i = 0; i < result.GetLength(0); i++)
        {
            var row = new List<double>();
            for (int j = 0; j < result.GetLength(1); j++)
            {
                row.Add(result[i, j]);
            }
            formattedMatrix.Add(row);
        }

        // Redirect to Razor View
        context.Response.Redirect($"/MatrixView?data={JsonSerializer.Serialize(formattedMatrix)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in API: {ex}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Internal Server Error", details = ex.Message }));
    }
});

app.Run();
