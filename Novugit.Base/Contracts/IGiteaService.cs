using Novugit.Base.Models;
using Novugit.Base.Models.gitea;
using RestSharp;

namespace Novugit.Base.Contracts;

public interface IGiteaService : IProvider
{
    RestClient GetInstance();
    Task<string> CreateRepository(Organization organization, ProjectInfo projectInfo);
    Task<List<Organization>> GetOrganizations();
}