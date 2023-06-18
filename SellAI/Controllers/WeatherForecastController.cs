using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;

namespace SellAI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WeatherForecastController : ControllerBase {
  private readonly IClaim _claim;
  private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  private readonly ILogger<WeatherForecastController> _logger;

  public WeatherForecastController(ILogger<WeatherForecastController> logger, IClaim claim)
  {
    _logger = logger;
    _claim = claim;
  }

  [HttpGet(Name = "GetWeatherForecast")]
  public IEnumerable<WeatherForecast> Get()
  {
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
      Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      TemperatureC = Random.Shared.Next(-20, 55),
      Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
  }
}

