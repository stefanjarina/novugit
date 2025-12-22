using McMaster.Extensions.CommandLineUtils;
using Novugit.API;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Models;

namespace Novugit.Commands;

[Command(Name = "gitignore", Description = "Generate .gitignore file")]
public class GitignoreCmd(IGitignoreService gitignoreService, IRepoService repoService)
{
    protected async Task<int> OnExecute(CommandLineApplication app)
    {
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