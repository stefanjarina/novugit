using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Novugit.Base.Contracts;
using Novugit.Base.Models;
using ProjectInfo = Novugit.Base.Models.ProjectInfo;

namespace Novugit.API.Services;

public class AzureService : IAzureService
{
    private readonly IConfiguration _config;
    private VssConnection _connection;

    public AzureService(IConfiguration config)
    {
        _config = config;
    }

    public void Authenticate()
    {
        var provider = GetStoredProviderInfo();

        var credentials = new VssBasicCredential(string.Empty, provider.Token);

        _connection = new VssConnection(new Uri($"{provider.BaseUrl}/{provider.Options["OrgName"]}"), credentials);
    }

    public VssConnection GetInstance()
    {
        return _connection;
    }

    public Provider GetStoredProviderInfo()
    {
        return _config.GetProvider("azure");
    }

    public async Task<string> CreateRepository(string project, ProjectInfo projectInfo)
    {
        var gitClient = await _connection.GetClientAsync<GitHttpClient>();
        var projectClient = await _connection.GetClientAsync<ProjectHttpClient>();

        var projectReference = await projectClient.GetProject(project);

        var data = new GitRepositoryCreateOptions { Name = projectInfo.Name, ProjectReference = projectReference };

        try
        {
            var response = await gitClient.CreateRepositoryAsync(data);

            return OperatingSystem.IsWindows() ? response.RemoteUrl : response.SshUrl;
        }
        catch (Exception e)
        {
            if (e.Message == $"TF400948: A Git repository with the name {projectInfo.Name} already exists.")
            {
                Console.WriteLine($"A Git repository with the name {projectInfo.Name} already exists. Exiting...");
                Environment.Exit(1);
            }

            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Dictionary<string, object>>> GetProjects()
    {
        var projectClient = await _connection.GetClientAsync<ProjectHttpClient>();
        var result = await projectClient.GetProjects();

        var projects = result.Select(x =>
        {
            var dict = new Dictionary<string, object>
            {
                { "name", x.Name },
                { "id", x.Id }
            };

            return dict;
        });

        return projects;
    }
}