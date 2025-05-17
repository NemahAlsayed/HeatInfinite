var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Endpoint: Heat Solver API
app.MapGet("/solve", async (HttpContext context) =>
{
    var query = context.Request.Query;
    double dx = double.TryParse(query["dx"], out var parsedDx) ? parsedDx / 100.0 : 0.1;
    double dy = dx;

    var result = HeatSolver.SolveHeatEquation(dx, dy); // Use the new class
    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
});

app.Run();
