using Swagger2Docx.Models;

namespace Swagger2Docx.Endpoints
{
    public static class WeatherforecastEndpoints
    {
        public static void MapWeatherforecastEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Weatherforecast");

            group.MapGet("/SelectWeatherforecast", () =>
            {
                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };


                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateTime.Now.AddDays(index),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();


                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();
        }
    }
}