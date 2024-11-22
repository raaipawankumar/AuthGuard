using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.SSO.TestApi
{
    [Authorize]
    [Route("Weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        readonly List<string> Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];


        [HttpGet("forecast")]
        public ActionResult<IEnumerable<WeatherForecast>> GetForecast()
        {
            var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Count)]
            )).ToArray();
            return Ok(forecast);
        }
    }
    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
