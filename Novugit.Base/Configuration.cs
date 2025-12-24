using Microsoft.Extensions.Logging;
using Novugit.Base.Contracts;
using Novugit.Base.Enums;
using Novugit.Base.Models;
using YamlDotNet.Serialization;

namespace Novugit.Base;

public class Configuration : IConfiguration
{
    private readonly string _configPath;
    private readonly ILogger<Configuration> _logger;

    public Config Config { get; set; }

    public Configuration(ILogger<Configuration> logger)
    {
        _configPath = ConstructConfigPath();
        _logger = logger;

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

    public string GetValue(string providerName, string key)
    {
        var provider = GetProvider(providerName);
        if (provider == null) return "";

        var result = "";

        switch (key.ToLower())
        {
            case "token":
                result = provider.Token;
                break;
            default:
                if (provider.Options.TryGetValue(key, out var value))
                    result = value;
                break;
        }

        return result;
    }

    public void UpdateValue(string providerName, string key, string value)
    {
        var provider = GetProvider(providerName);
        if (provider == null) return;

        switch (key.ToLower())
        {
            case "token":
                provider.Token = value;
                break;
            default:
                provider.Options![key] = value;
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
            new() { Name = "gitea", Token = "", BaseUrl = "" },
            new() { Name = "github", Token = "", BaseUrl = "https://github.com" },
            new() { Name = "gitlab", Token = "", BaseUrl = "https://gitlab.com" }
        };

        Config = new Config { DefaultBranch = "main", Providers = providers };

        try
        {
            SaveConfig();
        }
        catch (Exception)
        {
            _logger.LogError("Couldn't create new empty configuration in {}", _configPath);
            Environment.Exit(-1);
        }
    }

    private void SaveConfig()
    {
        var configAsYaml = Helpers.ConvertObjectToYaml(Config);

        File.WriteAllText(_configPath, configAsYaml);
    }
}