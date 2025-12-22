using Novugit.Base.Contracts;
using Novugit.Base.Models;
using Octokit;

namespace Novugit.API.Services;

public class GithubService(IConfiguration config) : IGithubService
{
    private readonly GitHubClient _client = new(new ProductHeaderValue("novugit"));

    public void Authenticate()
    {
        var provider = config.GetProvider("github");

        var credentials = new Credentials(provider.Token);
        _client.Credentials = credentials;
    }

    public GitHubClient GetInstance()
    {
        return _client;
    }

    public async Task<string> CreateRepository(ProjectInfo projectInfo)
    {
        var newRepository = new NewRepository(projectInfo.Name)
        {
            Description = projectInfo.Description,
            Private = projectInfo.Visibility == "private",
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