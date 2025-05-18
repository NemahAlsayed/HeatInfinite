using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text.Json;

public class MatrixViewModel : PageModel
{
    public List<List<double>> MatrixData { get; set; } = new();

    public void OnGet(string data)
    {
        MatrixData = JsonSerializer.Deserialize<List<List<double>>>(data);
    }
}
