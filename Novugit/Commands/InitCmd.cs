using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;

namespace Novugit.Commands
{
    [Command(Name = "init", Description = "Initialize new repository")]
    public class InitCmd : RepoArgBase
    {
        private readonly IRepoService _repoService;

        public InitCmd(IRepoService repoService)
        {
            _repoService = repoService;
        }

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
                "azure" => await _repoService.CreateRemoteRepo(Repos.Azure),
                "github" => await _repoService.CreateRemoteRepo(Repos.Github),
                "gitlab" => await _repoService.CreateRemoteRepo(Repos.Gitlab),
                _ => null
            };

            await _repoService.CreateGitIgnoreFile(projectInfo);
            _repoService.InitializeLocalGit((Repos)Enum.Parse(typeof(Repos), Repo.Capitalize()), projectInfo);

            return 1;
        }
    }
}
