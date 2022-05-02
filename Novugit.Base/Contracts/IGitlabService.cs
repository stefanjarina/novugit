using Novugit.Base.Models;

namespace Novugit.Base.Contracts;

public interface IGitlabService
{
    void Authenticate();
    Task GetInstance();
    Provider GetStoredProviderInfo();
    Task<string> CreateRepository(string project, ProjectInfo projectInfo);
    Task GetGroups();
}
