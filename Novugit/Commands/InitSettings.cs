using System.ComponentModel;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Novugit.Commands;

/// <summary>
/// Settings for the init command.
/// </summary>
[UsedImplicitly]
public class InitSettings : GlobalSettings
{
    [CommandArgument(0, "<provider>")]
    [Description("Git platform provider (github, gitlab, azure, bitbucket, forgejo, gitea)")]
    public string Provider { get; init; }
    
    [CommandOption("-f|--force")]
    [Description("Force initialization even if a git repository already exists in the current directory.")]
    public bool Force { get; init; }

    public override ValidationResult Validate()
    {
        return ProviderValidation.ValidateProvider(Provider);
    }
}
