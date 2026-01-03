using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Command to list all configuration.
/// </summary>
public class ConfigListAllCommand : Command<ConfigListAllSettings>
{
    private readonly IConfiguration _config;

    public ConfigListAllCommand(IConfiguration config)
    {
        _config = config;
    }

    public override int Execute(CommandContext context, ConfigListAllSettings settings, CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        ConsoleOutput.WriteInfo(Helpers.ConvertObjectToYaml(_config.Config));
        return 0;
    }
}
