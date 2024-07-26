using Actioneer.Api.Models;
using Actioneer.Core;

namespace Actioneer.Api.Actions;

public class GetWeatherForecast(ILogger<GetWeatherForecast> logger) : IAsyncDispatchable<WeatherForecast[]>
{
    private readonly string[] _summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

    public async Task<WeatherForecast[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving weather forecast");

        // Act like we're doing some work...
        await Task.Delay(TimeSpan.FromMilliseconds(300), cancellationToken);

        var forecast = Enumerable
            .Range(1, 5)
            .Select(index => new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            ))
            .ToArray();

        return forecast;
    }
}

public class LogWeatherForecast : ISideEffect<GetWeatherForecast>
{
    public void Run(GetWeatherForecast action)
    {
        throw new NotImplementedException();
    }
}
