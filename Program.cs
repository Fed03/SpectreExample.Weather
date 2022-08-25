// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using giuneco.wth;
using Refit;

var api = RestService.For<IOpenWeatherApiClient>("https://api.openweathermap.org/data/2.5");

var res = await api.GetWeatherForecastAsync(Environment.GetEnvironmentVariable("WEATHER_API"), "Piombino,it");

Console.WriteLine(JsonSerializer.Serialize(res,new JsonSerializerOptions{WriteIndented = true}));

public interface IOpenWeatherApiClient
{
    [Get("/weather?units=metric&lang=it")]
    Task<CurrentWeatherResponse> GetCurrentWeatherAsync([AliasAs("appid")] string apiKey, [AliasAs("q")] string location);
    
    [Get("/forecast?units=metric&lang=it")]
    Task<WeatherForecastResponse> GetWeatherForecastAsync([AliasAs("appid")] string apiKey, [AliasAs("q")] string location);
}

