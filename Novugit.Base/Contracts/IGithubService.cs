using Novugit.Base.Models;
using Octokit;

namespace Novugit.Base.Contracts;

public interface IGithubService
{
    void Authenticate();
    GitHubClient GetInstance();
    Provider GetStoredProviderInfo();
    Task<string> CreateRepository(ProjectInfo projectInfo);
}
