using Microsoft.VisualStudio.Services.Common;
using Novugit.Base.Contracts;

namespace Novugit.API.Services;

public class GitignoreService : IGitignoreService
{
    private const string BaseUrl = "https://www.toptal.com/developers/gitignore/api/";
    private HttpClient _client;

    public GitignoreService()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(10),
        };
    }

    public async Task<IEnumerable<string>> List()
    {
        IList<string> availableConfigs = new List<string>();

        var response = await _client.GetStringAsync("list");

        foreach (var line in response.Split())
        {
            var configs = line.Split(",");
            availableConfigs.AddRange(configs);
        }

        return availableConfigs;
    }

    public async Task<string> FetchConfig(IEnumerable<string> configs)
    {
        var query = string.Join(",", configs);
        var response = await _client.GetStringAsync(query);

        return response;
    }
}
