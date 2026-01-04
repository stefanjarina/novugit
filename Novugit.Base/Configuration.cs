﻿using Microsoft.AspNetCore.DataProtection;
 using Novugit.Base.Contracts;
using Novugit.Base.Enums;
using Novugit.Base.Models;
using YamlDotNet.Serialization;

namespace Novugit.Base;

public class Configuration : IConfiguration
{
    private readonly ISecretService _secretService;
    private readonly string _configPath;

    public Config Config { get; set; }

    public Configuration(ISecretService secretService)
    {
        _secretService = secretService;
        _configPath = ConstructConfigPath();

        // Load Configuration
        // This creates default config in home folder in case it is not present
        Load();
    }

    public Provider GetProvider(string name)
    {
        return Config.Providers.Find(x => x.Name == name);
    }

    public Provider GetProvider(Repos repo)
    {
        return Config.Providers.Find(x => x.Name.Equals(repo.ToString(), StringComparison.InvariantCultureIgnoreCase));
    }
    
    public string DecryptToken(string encryptedToken)
    {
        return _secretService.Decrypt(encryptedToken);
    }

    public string GetValue(string providerName, string key, bool decrypt = false)
    {
        var provider = GetProvider(providerName);
        if (provider == null) return "";

        var result = "";

        switch (key.ToLower())
        {
            case "token":
                result = decrypt ? _secretService.Decrypt(provider.Token) : provider.Token;
                break;
            case "baseurl":
                result = provider.BaseUrl;
                break;
            default:
                if (provider.Options.TryGetValue(key, out var value))
                    result = decrypt ? _secretService.Decrypt(value) : value;
                break;
        }

        return result;
    }

    public void UpdateValue(string providerName, string key, string value, bool encrypt = false)
    {
        var provider = GetProvider(providerName);
        if (provider == null) return;

        switch (key.ToLower())
        {
            case "token":
                provider.Token = _secretService.Encrypt(value);
                break;
            case "baseurl":
                provider.BaseUrl = value;
                break;
            default:
                provider.Options![key] = encrypt ? _secretService.Encrypt(value) : value;
                break;
        }

        SaveConfig();
    }

    public void RemoveValue(string providerName, string key)
    {
        var provider = GetProvider(providerName);
        if (provider == null) return;

        switch (key.ToLower())
        {
            case "token":
                provider.Token = "";
                break;
            case "baseurl":
                provider.BaseUrl = "";
                break;
            default:
                provider.Options!.Remove(key!);
                break;
        }

        SaveConfig();
    }

    private void Load()
    {
        if (!File.Exists(_configPath))
        {
            CreateEmptyConfig();
            Console.WriteLine($"New empty configuration created at '{_configPath}'");
        }

        if (Config != null) return;

        var input = new StringReader(File.ReadAllText(_configPath));

        var deserializer = new Deserializer();

        Config = deserializer.Deserialize<Config>(input);
    }

    private static string ConstructConfigPath()
    {
        var homePath = Environment.OSVersion.Platform is PlatformID.Unix or PlatformID.MacOSX
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

        homePath ??= Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName);

        return Path.Combine(homePath!, ".config", "novugit", "config.yml");
    }

    private void CreateEmptyConfig()
    {
        if (!Directory.Exists(Path.GetDirectoryName(_configPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);
        }

        var providers = new List<Provider>
        {
            new() { Name = "azure", Token = "", BaseUrl = "https://dev.azure.com" },
            new() { Name = "bitbucket", Token = "", BaseUrl = "https://api.bitbucket.org" },
            new() { Name = "forgejo", Token = "", BaseUrl = ""},
            new() { Name = "gitea", Token = "", BaseUrl = "" },
            new() { Name = "github", Token = "", BaseUrl = "https://github.com" },
            new() { Name = "gitlab", Token = "", BaseUrl = "https://gitlab.com" }
        };

        Config = new Config { DefaultBranch = "main", Providers = providers };

        try
        {
            SaveConfig();
        }
        catch (Exception e)
        {
            throw new NovugitException($"Couldn't create new empty configuration in '{_configPath}'", e);
        }
    }

    private void SaveConfig()
    {
        var configAsYaml = Helpers.ConvertObjectToYaml(Config);

        File.WriteAllText(_configPath, configAsYaml);
    }
}