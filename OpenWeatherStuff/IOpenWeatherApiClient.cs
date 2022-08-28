using Refit;

namespace SpectreExample.Weather.OpenWeatherStuff;

public interface IOpenWeatherApiClient
{
    [Get("/weather?units=metric&lang=it")]
    Task<CurrentWeatherResponse> GetCurrentWeatherAsync([AliasAs("appid")] string apiKey,
                                                        [AliasAs("q")] string location
    );

    [Get("/forecast?units=metric&lang=it")]
    Task<WeatherForecastResponse> GetWeatherForecastAsync([AliasAs("appid")] string apiKey,
                                                          [AliasAs("q")] string location
    );
}