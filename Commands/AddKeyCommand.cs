using System.ComponentModel;
using Spectre.Cli;

namespace giuneco.wth.Commands;

internal class AddKeyCommand : AsyncCommand<AddKeyCommand.Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        await ApiKeyService.SaveApiKey(settings.ApiKey);
        return 0;
    }

    internal sealed class Settings : CommandSettings
    {
        public Settings(string apiKey)
        {
            ApiKey = apiKey;
        }

        [CommandArgument(0, "<ApiKey>")]
        [Description("Your OpenWeatherMap ApiKey")]
        public string ApiKey { get; }
    }
}