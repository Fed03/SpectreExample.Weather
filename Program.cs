using System.Globalization;
using giuneco.wth;
using Refit;
using Spectre.Console;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");
var textInfo = CultureInfo.CurrentCulture.TextInfo;

var api = RestService.For<IOpenWeatherApiClient>("https://api.openweathermap.org/data/2.5");

var res = await api.GetWeatherForecastAsync(Environment.GetEnvironmentVariable("WEATHER_API"), "Piombino,it");

var forecasts = res.List.GroupBy(x => x.Date.Date).Select(x => x.First()).Skip(1).OrderBy(x => x.Date);


var title = new Rule($"[orange1 underline]Previsione a 5 giorni per [bold]{res.City.Name}, {res.City.Country}[/][/]")
            .RoundedBorder()
            .RuleStyle("deepskyblue1")
            .LeftAligned();
AnsiConsole.Write(title);
AnsiConsole.WriteLine();
AnsiConsole.Write(new Columns(forecasts.Select(CreateForecastPanel)).Collapse());

Panel CreateForecastPanel(Forecast forecast)
{
    var infoTable = new Table().SimpleBorder().BorderColor(Color.Grey53)
                               .Collapse();
    infoTable.AddColumn($"[bold orange1]{textInfo.ToTitleCase(forecast.Weather[0].Description)}[/]");
    infoTable.AddRow(
        $"Temperatura [red]{forecast.Main.Temp:F0}°C[/], Percepita [deepskyblue1]{forecast.Main.FeelsLike:F0}°C[/]"
    );
    infoTable.AddRow($"Probabilità di precipitazioni {forecast.PrecipitationProbability:p0}");
    infoTable.AddRow($"Nuvolosità {forecast.Clouds.All}%");
    infoTable.AddRow($"Pressione {forecast.Main.Pressure} hPa");
    infoTable.AddRow($"Umidità {forecast.Main.Humidity}%");
    infoTable.AddRow($"Vento {forecast.Wind.Speed} m/s, {WindDirection(forecast.Wind.Deg)}");

    return new Panel(infoTable)
           .BorderColor(Color.Grey53)
           .Header($"[white underline bold]{textInfo.ToTitleCase(forecast.Date.ToString("dddd dd"))}[/]")
           .HeaderAlignment(Justify.Center)
           .RoundedBorder();
}

string WindDirection(int deg) => deg switch
{
    > 0 and < 90    => "N/E",
    > 90 and < 180  => "S/E",
    > 180 and < 270 => "S/O",
    > 270           => "N/O",
    0               => "N",
    90              => "E",
    180             => "S",
    270             => "O",
    _               => throw new ArgumentOutOfRangeException(nameof(deg), deg, null)
};