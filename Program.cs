// See https://aka.ms/new-console-template for more information

using System.Globalization;
using giuneco.wth;
using Refit;
using Spectre.Console;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");

var api = RestService.For<IOpenWeatherApiClient>("https://api.openweathermap.org/data/2.5");

var res = await api.GetWeatherForecastAsync(Environment.GetEnvironmentVariable("WEATHER_API"), "Piombino,it");

var title = new Rule("[orange1 bold]Weather Forecast[/]")
            .RoundedBorder()
            .RuleStyle("deepskyblue1")
            .LeftAligned();
AnsiConsole.Write(title);
AnsiConsole.WriteLine();

var tree = new Tree($"[red bold]{res.City.Name}, {res.City.Country}, {DateTime.Now:f}[/]");
tree.AddNode(new Markup($"{Emoji.Known.Sunrise} [bold]Sunrise:[/] {res.City.Sunrise:t}"));
tree.AddNode(new Markup($"{Emoji.Known.Sunset} [bold]Sunset:[/] {res.City.Sunset:t}"));
AnsiConsole.Write(tree);

var domani = res.List[20];
AnsiConsole.Write(CreateForecastPanel(domani));

Panel CreateForecastPanel(Forecast forecast)
{
    var infoTable = new Table().SimpleBorder().BorderColor(Color.Grey53);
    infoTable.AddColumns($"[bold orange1]{forecast.Weather[0].Description}[/]");
    infoTable.AddRow(
        $"Temp Max [red]{forecast.Main.TempMax:F0}°C[/], Temp Min [deepskyblue1]{forecast.Main.TempMin:F0}°C[/]"
    );
    infoTable.AddRow($"Probabilità di precipitazioni {forecast.PrecipitationProbability:p0}");
    infoTable.AddRow($"Nuvolosità {forecast.Clouds.All}%");
    infoTable.AddRow($"Pressione {forecast.Main.Pressure} hPa");
    infoTable.AddRow($"Umidità {forecast.Main.Humidity}%");
    infoTable.AddRow($"Vento {forecast.Wind.Speed} m/s, {forecast.Wind.Deg}");

    return new Panel(infoTable)
           .BorderColor(Color.Grey53)
           .Header($"[white underline bold]{forecast.Date:dddd dd}[/]")
           .HeaderAlignment(Justify.Center)
           .RoundedBorder();
}