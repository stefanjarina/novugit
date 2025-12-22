using Novugit.Base.Models;
using Octokit;

namespace Novugit.Base.Contracts;

public interface IGithubService : IProvider
{
    GitHubClient GetInstance();
    Task<string> CreateRepository(ProjectInfo projectInfo);
}