using Novugit.Base.Contracts;
using Novugit.Base.Models;
using Novugit.Base.Models.bitbucket;
using RestSharp;
using RestSharp.Authenticators;

namespace Novugit.API.Services;

public class BitBucketService : IBitBucketService
{
    private readonly IConfiguration _config;
    private readonly RestClient _client;

    public BitBucketService(IConfiguration config)
    {
        _config = config;

        var provider = GetStoredProviderInfo();
        
        var baseUrl = provider.BaseUrl.EndsWith("/") ? provider.BaseUrl : provider.BaseUrl + "/";
        
        var options = new RestClientOptions(baseUrl)
        {
            ThrowOnDeserializationError = true,
            Authenticator = new HttpBasicAuthenticator(provider.Options["User"], provider.Token)
        };
        _client = new RestClient(options);
    }
    
    public RestClient GetInstance()
    {
        return _client;
    }

    public Provider GetStoredProviderInfo()
    {
        return _config.GetProvider("bitbucket");
    }

    public async Task<string> CreateRepository(ProjectInfo projectInfo, string workspace, string project)
    {
        var isPrivate = projectInfo.Visibility == "private";

        var projectData = new RepositoryCreateProjectOptions() { Key = project };
        
        var data = new RepositoryCreateOptions()
        {
            Scm = "git",
            Project = projectData,
            Description = projectInfo.Description,
            IsPrivate = isPrivate,
        };

        try
        {
            var request = new RestRequest($"repositories/{workspace}/{projectInfo.Name}", Method.Post).AddJsonBody(data);
            var repository = await _client.PostAsync<CreateRepositoryResponse>(request);

            return OperatingSystem.IsWindows()
                ? repository.Links.Clone.First(x => x.Name == "https").Href
                : repository.Links.Clone.First(x => x.Name == "ssh").Href;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GetProjects(string workspace)
    {
        var projects = await _client.GetAsync<GetProjectsResponse>($"workspaces/{workspace}/projects");

        return projects.Values.Select(proj => new Dictionary<string, string>
        {
            { "name", proj.Name }, { "key", proj.Key }
        });
    }

    public async Task<IEnumerable<WorkspaceDetail>> GetWorkspaces()
    {
        var workspaces = await _client.GetAsync<GetWorkspacesResponse>("user/permissions/workspaces");

        var workspacesWithDetails = new List<WorkspaceDetail>();
            
        foreach (var workspaceValue in workspaces.Values)
        {
            var workspaceDetails = await GetWorkspace(workspaceValue.Workspace.Slug);

            workspacesWithDetails.Add(workspaceDetails);
        }

        return workspacesWithDetails;
    }

    private async Task<WorkspaceDetail> GetWorkspace(string workspace)
    {
        return await _client.GetAsync<WorkspaceDetail>($"workspaces/{workspace}");
    }
}