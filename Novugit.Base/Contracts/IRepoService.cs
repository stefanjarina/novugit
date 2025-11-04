using Novugit.Base.Enums;
using Novugit.Base.Models;

namespace Novugit.Base.Contracts;

public interface IRepoService
{
    Task<ProjectInfo> CreateRemoteRepo(Repos repo);
    Task CreateGitIgnoreFile(ProjectInfo projectInfo);
    void InitializeLocalGit(Repos repo, ProjectInfo projectInfo);
}