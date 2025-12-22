using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;

namespace Novugit.Commands;

[Command(Name = "init", Description = "Initialize new repository")]
public class InitCmd(IRepoService repoService) : RepoArgBase
{
    protected async Task<int> OnExecute(CommandLineApplication app)
    {
        var currentDir = Environment.CurrentDirectory;
        if (Directory.Exists(Path.Join(currentDir, ".git")))
        {
            Console.WriteLine("Already a git repository, exiting...");
            Environment.Exit(0);
        }

        var projectInfo = Repo switch
        {
            "azure" => await repoService.CreateRemoteRepo(Repos.Azure),
            "bitbucket" => await repoService.CreateRemoteRepo(Repos.Bitbucket),
            "github" => await repoService.CreateRemoteRepo(Repos.Github),
            "gitlab" => await repoService.CreateRemoteRepo(Repos.Gitlab),
            _ => null
        };

        await repoService.CreateGitIgnoreFile(projectInfo);
        await repoService.InitializeLocalGit((Repos)Enum.Parse(typeof(Repos), Repo.Capitalize()), projectInfo);

        return 0;
    }
}