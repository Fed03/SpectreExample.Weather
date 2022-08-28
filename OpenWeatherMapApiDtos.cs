using System.Text.Json.Serialization;

namespace SpectreExample.Weather;

public record Clouds(int All);

public record Coord(double Lon, double Lat);

public record Main(
    double Temp,
    [property: JsonPropertyName("feels_like")]
    double FeelsLike,
    [property: JsonPropertyName("temp_min")]
    double TempMin,
    [property: JsonPropertyName("temp_max")]
    double TempMax,
    int Pressure,
    int Humidity
);

public record CurrentWeatherResponse(
    Coord Coord,
    IReadOnlyList<Weather> Weather,
    string Base,
    Main Main,
    int Visibility,
    Wind Wind,
    Clouds Clouds,
    [property: JsonPropertyName("dt"), JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Date,
    Sys Sys,
    int Timezone,
    int Id,
    string Name,
    int Cod
);

public record Sys(
    int Type,
    int Id,
    string Country,
    [property: JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Sunrise,
    [property: JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Sunset
);

public record Weather(
    int Id,
    string Main,
    string Description,
    string Icon
);

public record Wind(double Speed, int Deg, double Gust);

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public record City(
    string Name,
    Coord Coord,
    string Country,
    int Population,
    int Timezone,
    [property: JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Sunrise,
    [property: JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Sunset
);

public record Forecast(
    [property: JsonPropertyName("dt"), JsonConverter(typeof(DateTimeJsonConverter))]
    DateTime Date,
    Main Main,
    IReadOnlyList<Weather> Weather,
    Clouds Clouds,
    Wind Wind,
    int Visibility,
    [property: JsonPropertyName("dt_txt")] string DtTxt,
    [property: JsonPropertyName("pop")] double PrecipitationProbability
);

public record WeatherForecastResponse(
    IReadOnlyList<Forecast> List,
    City City
);