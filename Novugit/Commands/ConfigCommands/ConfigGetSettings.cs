using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config get command.
/// </summary>
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
        if (string.IsNullOrWhiteSpace(Key))
        {
            return ValidationResult.Error("Key is required");
        }

        return ValidationResult.Success();
    }
}
