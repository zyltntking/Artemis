using Artemis.Extensions.Web.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.WebApi.Controllers;

/// <summary>
///     �����ձ�
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
[ArtemisClaim]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    /// <summary>
    ///     ����
    /// </summary>
    /// <param name="logger"></param>
    public WeatherForecastController(
        ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     ��ȡ�����ձ�
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}