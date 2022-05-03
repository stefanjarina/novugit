using Kurukuru;
using LibGit2Sharp;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;
using Novugit.Base.Models;

namespace Novugit.API.Services;

public class RepoService : IRepoService
{
    private readonly IConfiguration _config;
    private readonly IGitignoreService _gitignoreService;
    private readonly IAzureService _azureService;
    private readonly IGithubService _githubService;
    private readonly IGitlabService _gitlabService;

    public RepoService(IConfiguration config, IGitignoreService gitignoreService, IAzureService azureService, IGithubService githubService, IGitlabService gitlabService)
    {
        _config = config;
        _gitignoreService = gitignoreService;
        _azureService = azureService;
        _githubService = githubService;
        _gitlabService = gitlabService;
    }

    public async Task<ProjectInfo> CreateRemoteRepo(Repos repo)
    {
        var projectInfo = repo switch
        {
            Repos.Azure => await HandleAzure(),
            Repos.Github => await HandleGithub(),
            Repos.Gitlab => await HandleGitlab(),
            _ => throw new ArgumentOutOfRangeException(nameof(repo), repo, null)
        };

        return projectInfo;
    }

    public async Task CreateGitIgnoreFile(ProjectInfo projectInfo)
    {
        var spinner = new Spinner("Generating '.gitignore' file...");
        spinner.Start();

        try
        {
            var gitignoreString = "";

            if (projectInfo.ExcludedLocalFiles != null && projectInfo.ExcludedLocalFiles!.Any())
            {
                gitignoreString += "# CUSTOM SETTINGS\n\n";
                gitignoreString = projectInfo.ExcludedLocalFiles.Aggregate(gitignoreString, (current, excludedLocalFile) => current + excludedLocalFile);
                gitignoreString += "\n";
            }

            if (projectInfo.GitIgnoreConfigs != null && projectInfo.GitIgnoreConfigs!.Any())
            {
                gitignoreString += "# GITIGNORE.IO: \n";
                gitignoreString += await _gitignoreService.FetchConfig(projectInfo.GitIgnoreConfigs);
            }

            File.WriteAllText(@".gitignore", gitignoreString);
        }
        catch (Exception e)
        {
            spinner.Fail("Unable to create .gitignore file");
            Console.WriteLine(e);
            throw;
        }

        spinner.Succeed(".gitignore file created");
    }

    public void InitializeLocalGit(Repos repo, ProjectInfo projectInfo)
    {
        var spinner = new Spinner("Initializing local git...");
        spinner.Start();

        var initFolder = Environment.CurrentDirectory;

        try
        {
            // initialize repo in working directory
            Repository.Init(initFolder, false);

            // cleanup the strange _git2* symlinks
            var symlinks = Directory.GetDirectories(initFolder, "_git2_*");
            foreach (var symlink in symlinks)
            {
                Directory.Delete(symlink);
            }

            using var localRepo = new Repository(initFolder);

            var defaultBranch = _config.Config.DefaultBranch;

            // master => main
            if (defaultBranch != "master")
            {
                Helpers.ExecuteCommandAndGetStatus("git", $"branch -m master {defaultBranch}");
            }

            // get git configuration
            var config = localRepo.Config;
            var author = config.BuildSignature(DateTimeOffset.Now);

            // add files and commit
            localRepo.Index.Add(".gitignore");
            Commands.Stage(localRepo, "*");
            localRepo.Commit("initial commit", author, author);

            // configure remote
            localRepo.Network.Remotes.Add("origin", projectInfo.RemoteUrl);
            var remote = localRepo.Network.Remotes["origin"];

            // finally push to remote
            spinner.Succeed("Local git initialized, attempting to push to remote repository...");

            var pushResult = Helpers.ExecuteCommandInteractively("git", $"push --set-upstream origin {defaultBranch}", "Are you sure you want to continue connecting (yes/no/[fingerprint])?");

            if (pushResult)
            {
                Console.WriteLine($"✔ Local repository successfully pushed to {repo}");
            }
        }
        catch (Exception e)
        {
            spinner.Fail($"Unable to initialize git in '{initFolder}'");
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<ProjectInfo> HandleAzure()
    {
        var org = _config.GetValue("azure", "OrgName");
        if (string.IsNullOrEmpty(org))
        {
            org = Prompts.AskForOrgName();
            _config.UpdateValue("azure", "OrgName", org);
        }

        var token = _config.GetValue("azure", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken();
            _config.UpdateValue("azure", "token", token);
        }

        // fetch remote information
        var authSpinner = new Spinner("Authenticating and fetching info from Azure DevOps");
        authSpinner.Start();
        _azureService.Authenticate();
        var availableGitignoreConfigs = await _gitignoreService.List();
        var azureProjects = await _azureService.GetProjects();
        authSpinner.Succeed("Info successfully fetched from Azure DevOps");

        var projectInfo = Prompts.AskForProjectInfo(Repos.Azure, availableGitignoreConfigs);

        var azureProjectLocation = Prompts.AskForAzureProject(azureProjects);

        var repoCreationSpinner = new Spinner("Creating repo in Azure DevOps");
        repoCreationSpinner.Start();
        var url = await _azureService.CreateRepository(azureProjectLocation, projectInfo);
        repoCreationSpinner.Succeed("Remote repo created");

        projectInfo.RemoteUrl = url;

        return projectInfo;
    }

    private async Task<ProjectInfo> HandleGithub()
    {
        var token = _config.GetValue("github", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken();
            _config.UpdateValue("github", "token", token);
        }

        // fetch remote information
        var authSpinner = new Spinner("Authenticating and fetching info from Github");
        authSpinner.Start();
        _githubService.Authenticate();
        var availableGitignoreConfigs = await _gitignoreService.List();
        authSpinner.Succeed("Info successfully fetched from Github");

        var projectInfo = Prompts.AskForProjectInfo(Repos.Azure, availableGitignoreConfigs);

        var repoCreationSpinner = new Spinner("Creating repo in Azure DevOps");
        repoCreationSpinner.Start();
        var url = await _githubService.CreateRepository(projectInfo);
        repoCreationSpinner.Succeed("Remote repo created");

        projectInfo.RemoteUrl = url;

        return projectInfo;
    }

    private async Task<ProjectInfo> HandleGitlab()
    {
        var token = _config.GetValue("gitlab", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken();
            _config.UpdateValue("gitlab", "token", token);
        }

        // fetch remote information
        var authSpinner = new Spinner("Authenticating and fetching info from Gitlab");
        authSpinner.Start();
        _gitlabService.Authenticate();
        var availableGitignoreConfigs = await _gitignoreService.List();
        var groups = await _gitlabService.GetGroups();
        authSpinner.Succeed("Info successfully fetched from Gitlab");

        var projectInfo = Prompts.AskForProjectInfo(Repos.Gitlab, availableGitignoreConfigs);

        var gitlabGroup = Prompts.AskForGitlabGroup(groups);

        var repoCreationSpinner = new Spinner("Creating repo in Gitlab");
        repoCreationSpinner.Start();
        var url = await _gitlabService.CreateRepository(gitlabGroup, projectInfo);
        repoCreationSpinner.Succeed("Remote repo created");

        projectInfo.RemoteUrl = url;

        return projectInfo;
    }
}
