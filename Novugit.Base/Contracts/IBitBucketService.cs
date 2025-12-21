using Novugit.Base.Models;
using Novugit.Base.Models.bitbucket;
using RestSharp;

namespace Novugit.Base.Contracts;

public interface IBitBucketService
{
    RestClient GetInstance();
    Task<string> CreateRepository(ProjectInfo projectInfo, string workspace, string project);
    Task<IEnumerable<Dictionary<string, string>>> GetProjects(string workspace);
    Task<IEnumerable<WorkspaceDetail>> GetWorkspaces();
}