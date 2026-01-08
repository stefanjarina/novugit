using JetBrains.Annotations;
using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config all command.
/// No arguments needed - just inherits global options.
/// </summary>
[UsedImplicitly]
public class ConfigListAllSettings : ConfigSettings
{
  // No additional properties - command takes no arguments
}

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
