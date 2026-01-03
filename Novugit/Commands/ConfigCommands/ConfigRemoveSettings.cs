using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands.ConfigCommands;

/// <summary>
/// Settings for the config remove command.
/// </summary>
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
