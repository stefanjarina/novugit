using Novugit.API;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Models;
using Spectre.Console.Cli;

namespace Novugit.Commands;

/// <summary>
/// Command to generate .gitignore file from gitignore.io templates.
/// </summary>
public class GitignoreCommand(IGitignoreService gitignoreService, IRepoService repoService)
    : AsyncCommand<GitignoreSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        GitignoreSettings settings,
        CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        var availableGitignoreConfigs = await gitignoreService.List();

        var currentDirInfo = Helpers.GetCurrentDirInfo();

        var (gitIgnoreConfigs, excludedLocalFiles) =
            Prompts.AskForGitignoreDetails(currentDirInfo, availableGitignoreConfigs);

        var projectInfo = new ProjectInfo
        {
            Name = null,
            Description = null,
            Visibility = null,
            GitIgnoreConfigs = gitIgnoreConfigs,
            ExcludedLocalFiles = excludedLocalFiles
        };

        await repoService.CreateGitIgnoreFile(projectInfo);

        return 0;
    }
}
