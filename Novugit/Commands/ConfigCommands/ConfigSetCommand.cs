using JetBrains.Annotations;
using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Command to set a configuration value for a specified provider.
/// </summary>
[UsedImplicitly]
public class ConfigSetCommand(IConfiguration config) : Command<ConfigSetSettings>
{
    public override int Execute(CommandContext context, ConfigSetSettings settings, CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        try
        {
            config.UpdateValue(settings.Provider, settings.Key, settings.Value, settings.Encrypt);
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
