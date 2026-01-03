using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Command to remove a configuration value for a specified provider.
/// </summary>
public class ConfigRemoveCommand(IConfiguration config) : Command<ConfigRemoveSettings>
{
    public override int Execute(CommandContext context, ConfigRemoveSettings settings, CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        try
        {
            config.RemoveValue(settings.Provider, settings.Key);
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
