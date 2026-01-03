using JetBrains.Annotations;
using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Command to list all configuration.
/// </summary>
[UsedImplicitly]
public class ConfigListAllCommand(IConfiguration config) : Command<ConfigListAllSettings>
{
    public override int Execute(CommandContext context, ConfigListAllSettings settings, CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        ConsoleOutput.WriteInfo(Helpers.ConvertObjectToYaml(config.Config));
        return 0;
    }
}
