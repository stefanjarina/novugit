using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Command to set a configuration value for a specified provider.
/// </summary>
public class ConfigSetCommand : Command<ConfigSetSettings>
{
    private readonly IConfiguration _config;

    public ConfigSetCommand(IConfiguration config)
    {
        _config = config;
    }

    public override int Execute(CommandContext context, ConfigSetSettings settings, CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        try
        {
            _config.UpdateValue(settings.Provider, settings.Key, settings.Value);
            ConsoleOutput.WriteSuccess($"{settings.Key} successfully set");
            return 0;
        }
        catch (Exception e)
        {
            ConsoleOutput.WriteError($"Failed to set {settings.Key}: {e.Message}", e);
            return 1;
        }
    }
}
