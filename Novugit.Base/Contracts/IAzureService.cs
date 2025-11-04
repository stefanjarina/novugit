using Microsoft.VisualStudio.Services.WebApi;
using Novugit.Base.Models;

namespace Novugit.Base.Contracts;

public interface IAzureService : IGitApiService
{
    VssConnection GetInstance();
    void Authenticate();
    Task<string> CreateRepository(string project, ProjectInfo projectInfo);
    Task<IEnumerable<Dictionary<string, object>>> GetProjects();
}