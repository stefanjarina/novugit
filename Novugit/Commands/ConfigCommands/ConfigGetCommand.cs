using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Command to get a configuration value for a specified provider.
/// </summary>
public class ConfigGetCommand : Command<ConfigGetSettings>
{
    private readonly IConfiguration _config;

    public ConfigGetCommand(IConfiguration config)
    {
        _config = config;
    }

    public override int Execute(CommandContext context, ConfigGetSettings settings, CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        Console.WriteLine($"Configuration for '{settings.Provider}'");
        var value = _config.GetValue(settings.Provider, settings.Key);
        Console.WriteLine($"{settings.Key}: {value}");
        return 0;
    }
}
