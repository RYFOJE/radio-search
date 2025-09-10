using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Radio_Search.Importer.CountryData.Functions;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function("Function1")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }

    // Converts GeoJSON coordinates array to WKT string
    private static string ConvertCoordinatesToWkt(JToken coords)
    {
        // Simplified for first polygon; in reality, you may need to handle MultiPolygons
        var points = coords[0];
        string pointStr = string.Join(",", points.Select(p => $"{p[0]} {p[1]}"));
        return $"POLYGON(({pointStr}))";
    }
}