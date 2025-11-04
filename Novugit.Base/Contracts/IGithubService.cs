using Novugit.Base.Models;
using Octokit;

namespace Novugit.Base.Contracts;

public interface IGithubService : IGitApiService
{
    GitHubClient GetInstance();
    void Authenticate();
    Task<string> CreateRepository(ProjectInfo projectInfo);
}