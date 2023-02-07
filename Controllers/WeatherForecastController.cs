using Microsoft.AspNetCore.Mvc;

namespace FastDeliveryApi.Controllers;

[ApiController]
[Route("forecast")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("get-weather-forecast/{name}")]
    public WeatherForecast Get(string name)
    {
        if(!string.IsNullOrWhiteSpace(name))
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).First(s => s.Summary == name);
        }
        WeatherForecast response = new();
        return response;
    }
}
