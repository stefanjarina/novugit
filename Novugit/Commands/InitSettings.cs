using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands;

/// <summary>
/// Settings for the init command.
/// </summary>
public class InitSettings : GlobalSettings
{
    [CommandArgument(0, "<provider>")]
    [Description("Git platform provider (github, gitlab, azure, bitbucket, forgejo, gitea)")]
    public string Provider { get; init; }

    public override ValidationResult Validate()
    {
        return ProviderValidation.ValidateProvider(Provider);
    }
}
