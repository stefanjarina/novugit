using System.ComponentModel;
using JetBrains.Annotations;
using Novugit.Base;
using Novugit.Base.Contracts;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config set command.
/// </summary>
[UsedImplicitly]
public class ConfigSetSettings : RepoSettings
{
  [CommandArgument(1, "<key>")]
  [Description("Configuration key to set")]
  public string Key { get; init; }

  [CommandArgument(2, "<value>")]
  [Description("Value to set")]
  public string Value { get; init; }

  [CommandOption("-e|--encrypt")]
  [Description("Encrypt the value before storing it")]
  public bool Encrypt { get; init; } = false;

  public override ValidationResult Validate()
  {
    // First validate the provider (from RepoSettings)
    var baseValidation = base.Validate();
    if (!baseValidation.Successful)
    {
      return baseValidation;
    }

    // Then validate the key and value
    if (string.IsNullOrWhiteSpace(Key))
    {
      return ValidationResult.Error("Key is required");
    }

    if (string.IsNullOrWhiteSpace(Value))
    {
      return ValidationResult.Error("Value is required");
    }

    return ValidationResult.Success();
  }
}

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
