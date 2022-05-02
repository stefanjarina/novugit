using Novugit.Base.Contracts;
using Novugit.Base.Models;

namespace Novugit.API.Services;

public class GitlabService : IGitlabService
{
    private readonly IConfiguration _config;

    public GitlabService(IConfiguration config)
    {
        _config = config;
    }

    public void Authenticate()
    {
        throw new System.NotImplementedException();
    }

    public async Task GetInstance()
    {
        throw new System.NotImplementedException();
    }

    public Provider GetStoredProviderInfo()
    {
        return _config.GetProvider("gitlab");
    }

    public async Task<string> CreateRepository(string project, ProjectInfo projectInfo)
    {
        throw new System.NotImplementedException();
    }

    public async Task GetGroups()
    {
        throw new System.NotImplementedException();
    }
}
