using System.ComponentModel;
using System.Globalization;
using Spectre.Cli;
using Spectre.Console;

namespace SpectreExample.Weather.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
internal class ForecastCommand : AsyncCommand<ForecastCommand.Settings>
{
    private static readonly TextInfo TextInfo = CultureInfo.CurrentCulture.TextInfo;
    private readonly IOpenWeatherApiClient _weatherApiClient;

    public ForecastCommand(IOpenWeatherApiClient weatherApiClient)
    {
        _weatherApiClient = weatherApiClient;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var apiKey = await ApiKeyService.GetApiKey();
        if (apiKey is null)
        {
            AnsiConsole.MarkupLine(
                "[red][bold]No ApiKey configured![/] Please run the 'wth add-key <ApiKey>' command.[/]"
            );
            return -99;
        }

        var response = await _weatherApiClient.GetWeatherForecastAsync(apiKey, settings.Location);
        var forecasts = OneWeatherForEachDay(response).Skip(1).OrderBy(x => x.Date);


        AnsiConsole.Write(Title(response.City));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Columns(forecasts.Select(CreateForecastPanel)).Collapse());

        return 0;
    }

    private static Rule Title(City city)
    {
        return new Rule($"[orange1 underline]Previsione a 5 giorni per [bold]{city.Name}, {city.Country}[/][/]")
               .RoundedBorder()
               .RuleStyle("deepskyblue1")
               .LeftAligned();
    }

    private static IEnumerable<Forecast> OneWeatherForEachDay(WeatherForecastResponse res)
    {
        return res.List.GroupBy(x => x.Date.Date).Select(x => x.First());
    }

    private static Panel CreateForecastPanel(Forecast forecast)
    {
        var infoTable = new Table().SimpleBorder().BorderColor(Color.Grey53)
                                   .Collapse();
        infoTable.AddColumn($"[bold orange1]{TextInfo.ToTitleCase(forecast.Weather[0].Description)}[/]");
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
               .Header($"[white underline bold]{TextInfo.ToTitleCase(forecast.Date.ToString("dddd dd"))}[/]")
               .HeaderAlignment(Justify.Center)
               .RoundedBorder();
    }

    private static string WindDirection(int deg) => deg switch
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

    internal sealed class Settings : CommandSettings
    {
        public Settings(string location)
        {
            Location = location;
        }

        [CommandArgument(0, "<location>")]
        [Description("The place where you want to know the forecast for")]
        public string Location { get; }
    }
}