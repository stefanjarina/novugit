﻿using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Novugit.Base;
using Novugit.Base.Contracts;
using Novugit.Base.Models;
using ProjectInfo = Novugit.Base.Models.ProjectInfo;

namespace Novugit.API.Services;

public class AzureService(IConfiguration config) : IAzureService
{
    private VssConnection _connection;

    public void Authenticate()
    {
        var provider = config.GetProvider("azure");
        var token = config.DecryptToken(provider.Token);

        var credentials = new VssBasicCredential(string.Empty, token);

        _connection = new VssConnection(new Uri($"{provider.BaseUrl}/{provider.Options["OrgName"]}"), credentials);
    }

    public VssConnection GetInstance()
    {
        return _connection;
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
                throw new NovugitException($"A Git repository with the name '{projectInfo.Name}' already exists", "azure", e);
            }

            throw new NovugitException("Failed to create repository on Azure DevOps", "azure", e);
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