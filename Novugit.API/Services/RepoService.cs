using Kurukuru;
using LibGit2Sharp;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;
using Novugit.Base.Models;

namespace Novugit.API.Services;

public class RepoService(
    IConfiguration config,
    IGitignoreService gitignoreService,
    IAzureService azureService,
    IBitBucketService bitBucketService,
    IGithubService githubService,
    IGitlabService gitlabService,
    IGiteaService giteaService)
    : IRepoService
{
    public async Task<ProjectInfo> CreateRemoteRepo(Repos repo)
    {
        var projectInfo = repo switch
        {
            Repos.Azure => await HandleAzure(),
            Repos.Bitbucket => await HandleBitbucket(),
            Repos.Gitea => await HandleGitea(),
            Repos.Github => await HandleGithub(),
            Repos.Gitlab => await HandleGitlab(),
            _ => throw new ArgumentOutOfRangeException(nameof(repo), repo, null)
        };

        return projectInfo;
    }

    public async Task CreateGitIgnoreFile(ProjectInfo projectInfo)
    {
        if (!projectInfo.GitIgnoreConfigs!.Any() && !projectInfo.ExcludedLocalFiles!.Any())
            return;

        var spinner = new Spinner("Generating '.gitignore' file...");
        spinner.Start();

        try
        {
            var gitignoreString = "";

            if (projectInfo.ExcludedLocalFiles != null && projectInfo.ExcludedLocalFiles!.Any())
            {
                gitignoreString += "# CUSTOM SETTINGS\n\n";
                gitignoreString = projectInfo.ExcludedLocalFiles.Aggregate(gitignoreString,
                    (current, excludedLocalFile) => current + excludedLocalFile);
                gitignoreString += "\n";
            }

            if (projectInfo.GitIgnoreConfigs != null && projectInfo.GitIgnoreConfigs!.Any())
            {
                gitignoreString += "# GITIGNORE.IO: \n";
                gitignoreString += await gitignoreService.FetchConfig(projectInfo.GitIgnoreConfigs);
            }

            await File.WriteAllTextAsync(@".gitignore", gitignoreString);
        }
        catch (Exception e)
        {
            spinner.Fail("Unable to create .gitignore file");
            Console.WriteLine(e);
            throw;
        }

        spinner.Succeed(".gitignore file created");
    }

    public async Task InitializeLocalGit(Repos repo, ProjectInfo projectInfo)
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

            var defaultBranch = config.Config.DefaultBranch;

            // get git configuration
            var config1 = localRepo.Config;
            var author = config1.BuildSignature(DateTimeOffset.Now);

            // add files and commit
            localRepo.Index.Add(".gitignore");
            Commands.Stage(localRepo, "*");
            localRepo.Commit("initial commit", author, author);

            // configure remote
            localRepo.Network.Remotes.Add("origin", projectInfo.RemoteUrl);
            _ = localRepo.Network.Remotes["origin"];

            // finally push to remote
            spinner.Succeed("Local git initialized, attempting to push to remote repository...");

            var pushResult = await Helpers.ExecuteCommandInteractivelyAsync("git", $"push --set-upstream origin {defaultBranch}",
                "yes");

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
        var org = config.GetValue("azure", "OrgName");
        if (string.IsNullOrEmpty(org))
        {
            org = Prompts.AskForOrgName();
            config.UpdateValue("azure", "OrgName", org);
        }

        var token = config.GetValue("azure", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken("Azure");
            config.UpdateValue("azure", "token", token);
        }
        
        var availableGitignoreConfigs = await GetAvailableGitignoreConfigs();

        // fetch remote information
        var authSpinner = new Spinner("Authenticating and fetching info from Azure DevOps");
        authSpinner.Start();
        azureService.Authenticate();
        var azureProjects = await azureService.GetProjects();
        authSpinner.Succeed("Info successfully fetched from Azure DevOps");

        var projectInfo = Prompts.AskForProjectInfo(Repos.Azure, availableGitignoreConfigs);

        var azureProjectLocation = Prompts.AskForAzureProject(azureProjects);

        var repoCreationSpinner = new Spinner("Creating repo in Azure DevOps");
        repoCreationSpinner.Start();
        var url = await azureService.CreateRepository(azureProjectLocation, projectInfo);
        repoCreationSpinner.Succeed("Remote repo created");

        projectInfo.RemoteUrl = url;

        return projectInfo;
    }

    private async Task<ProjectInfo> HandleGithub()
    {
        var token = config.GetValue("github", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken("Github");
            config.UpdateValue("github", "token", token);
        }

        var availableGitignoreConfigs = await GetAvailableGitignoreConfigs();

        var authSpinner = new Spinner("Authenticating to Github");
        authSpinner.Start();
        githubService.Authenticate();
        authSpinner.Succeed();
        
        var projectInfo = Prompts.AskForProjectInfo(Repos.Azure, availableGitignoreConfigs);

        var repoCreationSpinner = new Spinner("Creating repo on Github");
        repoCreationSpinner.Start();
        var url = await githubService.CreateRepository(projectInfo);
        repoCreationSpinner.Succeed("Remote repo created");

        projectInfo.RemoteUrl = url;

        return projectInfo;
    }

    private async Task<ProjectInfo> HandleGitlab()
    {
        var token = config.GetValue("gitlab", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken("Gitlab");
            config.UpdateValue("gitlab", "token", token);
        }

        var availableGitignoreConfigs = await GetAvailableGitignoreConfigs();

        var projectInfo = Prompts.AskForProjectInfo(Repos.Gitlab, availableGitignoreConfigs);

        var groupsSpinner = new Spinner("Fetching info about groups from Gitlab based on project visibility");
        groupsSpinner.Start();
        gitlabService.Authenticate();
        var groups = await gitlabService.GetGroups(projectInfo.Visibility);
        groupsSpinner.Succeed("Info successfully fetched from Gitlab");

        var gitlabGroup = Prompts.AskForGitlabGroup(groups);

        var repoCreationSpinner = new Spinner("Creating repo on Gitlab");
        repoCreationSpinner.Start();
        var url = await gitlabService.CreateRepository(gitlabGroup, projectInfo);
        repoCreationSpinner.Succeed("Remote repo created");

        projectInfo.RemoteUrl = url;

        return projectInfo;
    }

    private async Task<ProjectInfo> HandleBitbucket()
    {
        var token = config.GetValue("bitbucket", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken("Bitbucket");
            config.UpdateValue("bitbucket", "token", token);
        }
        
        var availableGitignoreConfigs = await GetAvailableGitignoreConfigs();

        var projectInfo = Prompts.AskForProjectInfo(Repos.Bitbucket, availableGitignoreConfigs);
        var groupsSpinner = new Spinner("Fetching workspaces from Bitbucket");
        groupsSpinner.Start();
        bitBucketService.Authenticate();
        var workspaces = await bitBucketService.GetWorkspaces();
        groupsSpinner.Succeed("Workspaces successfully fetched from Bitbucket");
        
        // filter out workspaces where privacy is enforced in case user want a public repo
        if (projectInfo.Visibility == "public")
        {
            workspaces = workspaces.Where(w => !w.IsPrivacyEnforced).ToList();
        }

        var workspace = Prompts.AskForBitbucketWorkspace(workspaces);
        
        var projectsSpinner = new Spinner("Fetching projects from Bitbucket");
        projectsSpinner.Start();
        var projects = await bitBucketService.GetProjects(workspace);
        projectsSpinner.Succeed("Projects successfully fetched from Bitbucket");
        
        var project = Prompts.AskForBitbucketProject(projects);
        
        var repoCreationSpinner = new Spinner("Creating repo on Bitbucket");
        repoCreationSpinner.Start();
        var url = await bitBucketService.CreateRepository(projectInfo, workspace, project);
        repoCreationSpinner.Succeed("Remote repo created");
        
        projectInfo.RemoteUrl = url;

        return projectInfo;
    }

    private async Task<ProjectInfo> HandleGitea()
    {
        var token = config.GetValue("gitea", "token");
        if (string.IsNullOrEmpty(token))
        {
            token = Prompts.AskForToken("Gitea");
            config.UpdateValue("gitea", "token", token);
        }
        
        var availableGitignoreConfigs = await GetAvailableGitignoreConfigs();
        
        var projectInfo = Prompts.AskForProjectInfo(Repos.Gitea, availableGitignoreConfigs);
        
        var organizationsSpinner = new Spinner("Fetching organizations from Gitea");
        organizationsSpinner.Start();
        giteaService.Authenticate();
        var organizations = await giteaService.GetOrganizations();
        organizationsSpinner.Succeed("Organizations successfully fetched from Gitea");
        
        var organization = Prompts.AskForGiteaOrganization(organizations);
        
        var repoCreationSpinner = new Spinner("Creating repo on Gitea");
        repoCreationSpinner.Start();
        var url = await giteaService.CreateRepository(organization, projectInfo);
        repoCreationSpinner.Succeed("Remote repo created");
        
        projectInfo.RemoteUrl = url;
        
        return projectInfo;
    }
    
    private async Task<IEnumerable<string>> GetAvailableGitignoreConfigs()
    {
        // fetch remote information
        var authSpinner = new Spinner("Fetching info from Gitignore.io");
        authSpinner.Start();
        var availableGitignoreConfigs = await gitignoreService.List();
        authSpinner.Succeed("Info successfully fetched");
        return availableGitignoreConfigs;
    }
}