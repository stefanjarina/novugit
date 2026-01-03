using Novugit.API;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Models;
using Spectre.Console.Cli;

namespace Novugit.Commands;

/// <summary>
/// Command to generate .gitignore file from gitignore.io templates.
/// </summary>
public class GitignoreCommand : AsyncCommand<GitignoreSettings>
{
    private readonly IGitignoreService _gitignoreService;
    private readonly IRepoService _repoService;

    public GitignoreCommand(IGitignoreService gitignoreService, IRepoService repoService)
    {
        _gitignoreService = gitignoreService;
        _repoService = repoService;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        GitignoreSettings settings,
        CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        var availableGitignoreConfigs = await _gitignoreService.List();

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

        await _repoService.CreateGitIgnoreFile(projectInfo);

        return 0;
    }
}
