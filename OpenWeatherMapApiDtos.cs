namespace giuneco.wth;

public record Clouds(int All);

public record Coord(double Lon, double Lat);

public record Main(
    double Temp,
    double FeelsLike,
    double TempMin,
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
    int Dt,
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
    int Sunrise,
    int Sunset
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
    int Id,
    string Name,
    Coord Coord,
    string Country,
    int Population,
    int Timezone,
    int Sunrise,
    int Sunset
);


public record Forecast(
    int Dt,
    Main Main,
    IReadOnlyList<Weather> Weather,
    Clouds Clouds,
    Wind Wind,
    int Visibility,
    double Pop,
    string DtTxt
);


public record WeatherForecastResponse(
    string Cod,
    int Message,
    int Cnt,
    IReadOnlyList<Forecast> List,
    City City
);

