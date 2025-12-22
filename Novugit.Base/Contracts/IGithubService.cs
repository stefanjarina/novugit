using Novugit.Base.Models;
using Octokit;

namespace Novugit.Base.Contracts;

public interface IGithubService
{
    GitHubClient GetInstance();
    void Authenticate();
    Task<string> CreateRepository(ProjectInfo projectInfo);
}