using Novugit.Base.Contracts;
using Novugit.Base.Models;
using Octokit;

namespace Novugit.API.Services;

public class GithubService : IGithubService
{
    private readonly IConfiguration _config;
    private GitHubClient _client;

    public GithubService(IConfiguration config)
    {
        _config = config;
        _client = new GitHubClient(new ProductHeaderValue("novugit"));
    }

    public void Authenticate()
    {
        var provider = GetStoredProviderInfo();

        var creds = new Credentials(provider.Token);
        _client.Credentials = creds;
    }

    public GitHubClient GetInstance()
    {
        return _client;
    }

    public Provider GetStoredProviderInfo()
    {
        return _config.GetProvider("github");
    }

    public async Task<string> CreateRepository(ProjectInfo projectInfo)
    {
        var newRepository = new NewRepository(projectInfo.Name)
        {
            Description = projectInfo.Description, Private = projectInfo.Visibility == "private",
        };

        try
        {
            var response = await _client.Repository.Create(newRepository);
            return OperatingSystem.IsWindows() ? response.CloneUrl : response.SshUrl;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}