using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands;

/// <summary>
/// Base settings for config commands that require a provider argument.
/// Replaces RepoArgBase.
/// </summary>
public class RepoSettings : ConfigSettings
{
    [CommandArgument(0, "<provider>")]
    [Description("Git platform provider (github, gitlab, azure, bitbucket, forgejo, gitea)")]
    public string Provider { get; init; }

    public override ValidationResult Validate()
    {
        return ProviderValidation.ValidateProvider(Provider);
    }
}
