// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Refit;

var api = RestService.For<IOpenWeatherApiClient>("https://api.openweathermap.org/data/2.5");

var res = await api.GetWeatherForecastAsync(Environment.GetEnvironmentVariable("WEATHER_API"), "Piombino,it");

Console.WriteLine(JsonSerializer.Serialize(res,new JsonSerializerOptions{WriteIndented = true}));