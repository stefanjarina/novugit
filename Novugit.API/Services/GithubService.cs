﻿using Novugit.Base;
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
        var token = config.DecryptToken(provider.Token);

        var credentials = new Credentials(token);
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
            throw new NovugitException("Failed to create repository on GitHub", "github", e);
        }
    }
}