using Novugit.Base.Contracts;
using Novugit.Base.Models;
using Novugit.Base.Models.Gitlab;
using RestSharp;
using RestSharp.Authenticators;

namespace Novugit.API.Services;

public class GitlabService : IGitlabService
{
    private readonly IConfiguration _config;
    private RestClient _client;

    public GitlabService(IConfiguration config)
    {
        _config = config;
        
        var provider = GetStoredProviderInfo();

        var baseUrl = provider.BaseUrl.EndsWith("/") ? $"{provider.BaseUrl}api/v4/" : $"{provider.BaseUrl}/api/v4/";

        var options = new RestClientOptions(baseUrl)
        {
            ThrowOnDeserializationError = true,
        };
        _client = new RestClient(options);
    }

    public void Authenticate()
    {
        var provider = GetStoredProviderInfo();

        _client.Authenticator = new JwtAuthenticator(provider.Token);
    }

    public RestClient GetInstance()
    {
        return _client;
    }

    public Provider GetStoredProviderInfo()
    {
        return _config.GetProvider("gitlab");
    }

    public async Task<string> CreateRepository(string projectId, ProjectInfo projectInfo)
    {
        var data = new RepositoryCreateOptions()
        {
            Name = projectInfo.Name,
            Description = projectInfo.Description,
            NamespaceId = projectId,
            Visibility = projectInfo.Visibility
        };

        try
        {
            var request = new RestRequest("projects", Method.Post).AddJsonBody(data);
            var project = await _client.PostAsync<ProjectCreateResponse>(request);

            return OperatingSystem.IsWindows() ? project?.HttpUrlToRepo : project?.SshUrlToRepo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Dictionary<string, string>>> GetGroups()
    {
        try
        {
            // get user
            var user = await _client.GetJsonAsync<User>("user");
            // get main user namespace
            var ns = await _client.GetJsonAsync<List<Namespace>>($"namespaces?search={user?.Name}");
            var userNamespaceId = ns?.First().Id.ToString();
            // get groups and subgroups
            var groups = await _client.GetJsonAsync<List<Group>>("groups");
            
            var gitlabGroups = new List<Dictionary<string, string>>
            {
                new Dictionary<string,string>
                {
                    {"name", user?.Name},
                    {"value", userNamespaceId},
                },
            };

            if (groups != null && groups.Count > 0)
            {
                gitlabGroups.AddRange(groups!.Select(group => new Dictionary<string, string> { { "name", group.Name }, { "value", group.Id.ToString() }, }));
            }

            return gitlabGroups;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
