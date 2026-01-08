using System.ComponentModel;
using JetBrains.Annotations;
using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config remove command.
/// </summary>
[UsedImplicitly]
public class ConfigRemoveSettings : RepoSettings
{
  [CommandArgument(1, "<key>")]
  [Description("Configuration key to remove")]
  public string Key { get; init; }

  public override ValidationResult Validate()
  {
    // First validate the provider (from RepoSettings)
    var baseValidation = base.Validate();
    if (!baseValidation.Successful)
    {
      return baseValidation;
    }

    // Then validate the key
    if (string.IsNullOrWhiteSpace(Key))
    {
      return ValidationResult.Error("Key is required");
    }

    return ValidationResult.Success();
  }
}

/// <summary>
/// Command to remove a configuration value for a specified provider.
/// </summary>
[UsedImplicitly]
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
