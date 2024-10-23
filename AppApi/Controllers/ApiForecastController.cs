using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AppApi.Controllers
{
    [ApiController]
    [Route("apiForecast")]
    public class ApiForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ApiForecastController> _logger;

        public ApiForecastController(ILogger<ApiForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("headers")]
        public IActionResult ExtractFromBasic()
        {
            Dictionary<string, StringValues> headers = new();
            foreach (var header in Request.Headers)
            {
                headers.Add(header.Key, header.Value);
            }
            return Ok(headers);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
