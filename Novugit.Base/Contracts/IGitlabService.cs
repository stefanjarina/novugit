using Novugit.Base.Models;
using RestSharp;

namespace Novugit.Base.Contracts;

public interface IGitlabService
{
    RestClient GetInstance();
    Task<string> CreateRepository(string group, ProjectInfo projectInfo);
    Task<List<Dictionary<string, string>>> GetGroups(string visibility);
}