using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Cli;
using Spectre.Console;

namespace giuneco.wth.Infrastructure;

public static class HostingExtensions
{
    public static IServiceCollection AddCommandLine(
        this IServiceCollection services,
        Action<IConfigurator>? configurator = null
    )
    {
        var app = new CommandApp(new TypeRegistrar(services));
        if (configurator != null)
        {
            app.Configure(configurator);
        }

        services.AddSingleton<ICommandApp>(app);

        return services;
    }

    public static IServiceCollection AddCommandLine<TDefaultCommand>(
        this IServiceCollection services,
        Action<IConfigurator>? configurator = null
    )
        where TDefaultCommand : class, ICommand
    {
        var app = new CommandApp<TDefaultCommand>(new TypeRegistrar(services));
        if (configurator != null)
        {
            app.Configure(configurator);
        }

        services.AddSingleton<ICommandApp>(app);

        return services;
    }

    public static async Task<int> RunSpectre(this IHost host, string[] args)
    {
        if (host is null)
        {
            throw new ArgumentNullException(nameof(host));
        }

        var app = host.Services.GetService<ICommandApp>();
        if (app == null)
        {
            throw new InvalidOperationException("Command application has not been configured.");
        }

        try
        {
            return await app.RunAsync(args);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            return -99;
        }
    }
}