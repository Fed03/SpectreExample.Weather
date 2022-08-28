namespace SpectreExample.Weather;

internal static class ApiKeyService
{
    private static readonly string UserDirectory = Environment.GetFolderPath(
        Environment.SpecialFolder.UserProfile,
        Environment.SpecialFolderOption.DoNotVerify
    );

    private static readonly string SettingsFolder = Path.Join(UserDirectory, ".wth");
    private static readonly string SettingsFilePath = Path.Join(SettingsFolder, "api_key.txt");

    public static Task SaveApiKey(string apiKey)
    {
        if (!Directory.Exists(SettingsFolder))
        {
            Directory.CreateDirectory(SettingsFolder);
        }

        return File.WriteAllTextAsync(SettingsFilePath, apiKey);
    }

    public static async Task<string?> GetApiKey()
    {
        try
        {
            return await File.ReadAllTextAsync(SettingsFilePath);
        }
        catch (DirectoryNotFoundException)
        {
            return null;
        }
    }
}