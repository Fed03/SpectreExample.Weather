using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;
using Spectre.Cli;
using SpectreExample.Weather;
using SpectreExample.Weather.Commands;
using SpectreExample.Weather.Infrastructure;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");

var builder = Host.CreateDefaultBuilder();
builder.ConfigureLogging(
    (context, loggingBuilder) =>
    {
        if (context.HostingEnvironment.IsProduction())
        {
            loggingBuilder.ClearProviders();
        }
    }
);

builder.ConfigureServices(
    (context, services) =>
    {
        services.AddRefitClient<IOpenWeatherApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5"));
        services.AddCommandLine<ForecastCommand>(
            config =>
            {
                config.SetApplicationName("wth");

                config.AddCommand<AddKeyCommand>("add-key")
                      .WithDescription("Save your personal OpenWeatherMap ApiKey");

                if (context.HostingEnvironment.IsDevelopment())
                {
                    config.PropagateExceptions();
                    config.ValidateExamples();
                }
            }
        );
    }
);

var host = builder.Build();

return await host.RunSpectre(args);