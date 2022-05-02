using Novugit.Base.Models;
using RestSharp;

namespace Novugit.Base.Contracts;

public interface IGitlabService
{
    void Authenticate();
    RestClient GetInstance();
    Provider GetStoredProviderInfo();
    Task<string> CreateRepository(string group, ProjectInfo projectInfo);
    Task<List<Dictionary<string, string>>> GetGroups();
}
