using Microsoft.VisualStudio.Services.WebApi;
using Novugit.Base.Models;

namespace Novugit.Base.Contracts;

public interface IAzureService
{
    void Authenticate();
    VssConnection GetInstance();
    Provider GetStoredProviderInfo();
    Task<string> CreateRepository(string project, ProjectInfo projectInfo);
    Task<IEnumerable<Dictionary<string, object>>> GetProjects();
}
