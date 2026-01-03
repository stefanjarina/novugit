using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;
using Spectre.Console.Cli;

namespace Novugit.Commands;

/// <summary>
/// Command to initialize a new git repository with remote provider.
/// </summary>
public class InitCommand : AsyncCommand<InitSettings>
{
    private readonly IRepoService _repoService;

    public InitCommand(IRepoService repoService)
    {
        _repoService = repoService;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        InitSettings settings,
        CancellationToken cancellationToken)
    {
        settings.ApplyGlobalOptions();

        var currentDir = Environment.CurrentDirectory;
        if (Directory.Exists(Path.Join(currentDir, ".git")))
        {
            ConsoleOutput.WriteWarning("Already a git repository, exiting...");
            return 0;
        }

        var repoType = Enum.Parse<Repos>(settings.Provider.Capitalize());

        var projectInfo = await _repoService.CreateRemoteRepo(repoType);

        await _repoService.CreateGitIgnoreFile(projectInfo);
        await _repoService.InitializeLocalGit(repoType, projectInfo);

        return 0;
    }
}
