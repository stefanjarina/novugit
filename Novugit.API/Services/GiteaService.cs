﻿using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Models;
using Novugit.Base.Models.gitea;
using RestSharp;
using RestSharp.Authenticators;

namespace Novugit.API.Services;

public class GiteaService(IConfiguration config) : IGiteaService
{
    private RestClient _client;
    
    public void Authenticate()
    {
        var provider = config.GetProvider("gitea");

        var baseUrl = provider.BaseUrl.EndsWith('/') ? $"{provider.BaseUrl}api/v1/" : $"{provider.BaseUrl}/api/v1/";
        
        var options = new RestClientOptions(baseUrl)
        {
            ThrowOnDeserializationError = true,
        };
        _client = new RestClient(options);
        _client.AddDefaultHeader("Authorization", $"token {provider.Token}");
    }

    public RestClient GetInstance()
    {
        return _client;
    }

    public async Task<string> CreateRepository(Organization organization, ProjectInfo projectInfo)
    {
        var createInOrganization = organization.Visibility != "personal";
        
        var url = createInOrganization ? $"orgs/{organization.Username}/repos" : "user/repos";

        var data = new RepositoryCreateOptions
        {
            Name = projectInfo.Name,
            Description = projectInfo.Description,
            Private = projectInfo.Visibility == "private",
            AutoInit = false,
            DefaultBranch = config.GetValue("DefaultBranch", "main")
        };

        try
        {
            var request = new RestRequest(url, Method.Post).AddJsonBody(data);
            var repository = await _client.PostAsync<ResponseCreateRepository>(request);

            return OperatingSystem.IsWindows() ? repository?.CloneUrl : repository?.SshUrl;
        }
        catch (Exception e)
        {
            throw new NovugitException("Failed to create repository on Gitea", "gitea", e);
        }
    }

    public async Task<List<Organization>> GetOrganizations()
    {
        var orgs = new List<Organization>();

        var user = await _client.GetAsync<User>("user");
        // we add user as personal org
        orgs.Add(new Organization { Name = user.Username, Username = user.Username, Visibility = "personal" });
        
        orgs.AddRange(await _client.GetAsync<List<Organization>>("user/orgs"));

        return orgs;
    }
}