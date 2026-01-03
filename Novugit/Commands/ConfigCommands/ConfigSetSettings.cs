using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config set command.
/// </summary>
public class ConfigSetSettings : RepoSettings
{
    [CommandArgument(1, "<key>")]
    [Description("Configuration key to set")]
    public string Key { get; init; }

    [CommandArgument(2, "<value>")]
    [Description("Value to set")]
    public string Value { get; init; }

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
