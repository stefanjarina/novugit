using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Command to remove a configuration value for a specified provider.
/// </summary>
public class ConfigRemoveCommand : Command<ConfigRemoveSettings>
{
    private readonly IConfiguration _config;

    public ConfigRemoveCommand(IConfiguration config)
    {
        _config = config;
    }

    public override int Execute(CommandContext context, ConfigRemoveSettings settings, CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        try
        {
            _config.RemoveValue(settings.Provider, settings.Key);
            ConsoleOutput.WriteSuccess("Configuration successfully removed");
            return 0;
        }
        catch (Exception e)
        {
            ConsoleOutput.WriteError($"Failed to remove {settings.Key}: {e.Message}", e);
            return 1;
        }
    }
}
