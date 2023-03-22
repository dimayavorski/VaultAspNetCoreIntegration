using Microsoft.AspNetCore.Mvc;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp;
using Microsoft.Extensions.Options;
using VaultSharp.V1.Commons;

namespace AspNetCoreVaultIntegration.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly SomeSettings _someSettings;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<SomeSettings> someSettings)
    {
        _logger = logger;
        _someSettings = someSettings.Value;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        return Ok(_someSettings);   
    }
}
