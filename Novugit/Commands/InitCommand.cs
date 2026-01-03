using JetBrains.Annotations;
using Novugit.API;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;
using Spectre.Console.Cli;

namespace Novugit.Commands;

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
        if (Directory.Exists(Path.Join(currentDir, ".git")))
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

        var repoType = Enum.Parse<Repos>(settings.Provider.Capitalize());

        var projectInfo = await repoService.CreateRemoteRepo(repoType);

        await repoService.CreateGitIgnoreFile(projectInfo);
        await repoService.InitializeLocalGit(repoType, projectInfo);

        return 0;
    }
}
