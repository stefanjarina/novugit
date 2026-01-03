﻿using Microsoft.VisualStudio.Services.Common;
using Novugit.Base;
using Novugit.Base.Contracts;

namespace Novugit.API.Services;

public class GitignoreService : IGitignoreService
{
    private const string _baseUrl = "https://www.toptal.com/developers/gitignore/api/";
    private readonly HttpClient _client = new() { BaseAddress = new Uri(_baseUrl), Timeout = TimeSpan.FromSeconds(10), };

    public async Task<IEnumerable<string>> List()
    {
        IList<string> availableConfigs = new List<string>();

        try
        {
            var response = await _client.GetStringAsync("list");

            foreach (var line in response.Split())
            {
                var configs = line.Split(",");
                availableConfigs.AddRange(configs);
            }

            return availableConfigs;
        }
        catch (Exception e)
        {
            throw new NovugitException("Failed to fetch available gitignore templates from gitignore.io", e);
        }
    }

    public async Task<string> FetchConfig(IEnumerable<string> configs)
    {
        try
        {
            var query = string.Join(",", configs);
            var response = await _client.GetStringAsync(query);

            return response;
        }
        catch (Exception e)
        {
            throw new NovugitException("Failed to fetch gitignore configuration from gitignore.io", e);
        }
    }
}