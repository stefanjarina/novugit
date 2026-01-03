using System.ComponentModel;
using JetBrains.Annotations;
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
