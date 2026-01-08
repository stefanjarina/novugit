using System.ComponentModel;
using JetBrains.Annotations;
using Novugit.API;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;
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

  [CommandOption("--only-remote")]
  [Description("Do not create a local repository, only create the remote repository.")]
  public bool OnlyRemote { get; init; }

  [CommandOption("--only-push")]
  [Description("Only push the existing local repository.")]
  public bool OnlyPush { get; init; }

  public override ValidationResult Validate()
  {
    return ProviderValidation.ValidateProvider(Provider);
  }
}

/// <summary>
/// Command to initialize a new git repository with remote provider.
/// </summary>
[UsedImplicitly]
public class InitCommand(IRepoService repoService) : AsyncCommand<InitSettings>
{
  public override async Task<int> ExecuteAsync(
      CommandContext context,
      InitSettings settings,
      CancellationToken cancellationToken)
  {
    settings.ApplyGlobalOptions();

    var currentDir = Environment.CurrentDirectory;
    if (Directory.Exists(Path.Join(currentDir, ".git")) && !settings.OnlyPush && !settings.OnlyRemote)
    {
      var removeRepo = false || settings.Force;

      if (!removeRepo)
        removeRepo = Prompts.AskToDeleteCurrentLocalRepo();

      if (removeRepo)
      {
        Directory.Delete(Path.Join(currentDir, ".git"), true);
      }
      else
      {
        ConsoleOutput.WriteError("Operation cancelled. Existing git repository found.");
        return 0;
      }
    }

    if (Directory.Exists(Path.Join(currentDir, ".git")) && (settings.OnlyPush || settings.OnlyRemote))
    {
      ConsoleOutput.WriteInfo("Existing git repository found. --only-* option detected. Proceeding with the operation.");
    }

    var repoType = Enum.Parse<Repos>(settings.Provider.Capitalize());

    if (settings.OnlyPush)
    {
      await repoService.PushToRemote();
      return 0;
    }

    var projectInfo = await repoService.CreateRemoteRepo(repoType);

    await repoService.CreateGitIgnoreFile(projectInfo);

    if (!settings.OnlyRemote)
      await repoService.InitializeLocalGit(repoType, projectInfo);

    await repoService.CreateRemote(projectInfo.RemoteUrl);

    await repoService.PushToRemote();

    return 0;
  }
}
