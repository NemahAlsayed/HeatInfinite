using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Endpoint: Heat Solver API
app.MapGet("/solve", async (HttpContext context) =>
{
    var query = context.Request.Query;
    double dx = double.TryParse(query["dx"], out var parsedDx) ? parsedDx / 100.0 : 0.1;
    double dy = dx;

    var result = SolveHeatEquation(dx, dy);
    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
});

app.Run();

// 🔹 Heat Solver Function: Computes Temperature Matrix
double[,] SolveHeatEquation(double dx, double dy)
{
    int nx = (int)(120 / dx) + 1;
    int ny = (int)(100 / dy) + 1;
    double[,] temperature = new double[ny, nx];

    for (int j = 0; j < ny; j++)
    {
        for (int i = 0; i < nx; i++)
        {
            temperature[j, i] = 25.0; // Initial condition
            if (i == 0) temperature[j, i] += 50.0 * dx / 1.0;
            if (i == nx - 1) temperature[j, i] += 25.0 * dx / 1.0;
            if (j == 0) temperature[j, i] = 25.0;
            if (j == ny - 1) temperature[j, i] = 100.0;
        }
    }

    return temperature;
}

