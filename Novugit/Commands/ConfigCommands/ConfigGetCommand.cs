using System.ComponentModel;
using JetBrains.Annotations;
using Novugit.Base.Contracts;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config get command.
/// </summary>
[UsedImplicitly]
public class ConfigGetSettings : RepoSettings
{
  [CommandArgument(1, "<key>")]
  [Description("Configuration key to retrieve")]
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
    return string.IsNullOrWhiteSpace(Key) ? ValidationResult.Error("Key is required") : ValidationResult.Success();
  }
}

/// <summary>
/// Command to get a configuration value for a specified provider.
/// </summary>
[UsedImplicitly]
public class ConfigGetCommand(IConfiguration config) : Command<ConfigGetSettings>
{
  public override int Execute(CommandContext context, ConfigGetSettings settings, CancellationToken cancellationToken)
  {
    settings.ApplyGlobalOptions();

    Console.WriteLine($"Configuration for '{settings.Provider}'");
    var value = config.GetValue(settings.Provider, settings.Key);
    Console.WriteLine($"{settings.Key}: {value}");
    return 0;
  }
}
