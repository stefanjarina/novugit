using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class RepositoryCreateOptions
{
    [JsonPropertyName("scm")] public string Scm { get; set; } = "git";
    [JsonPropertyName("project")] public RepositoryCreateProjectOptions Project { get; set; }
    [JsonPropertyName("is_private")] public bool IsPrivate { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
}

public class RepositoryCreateProjectOptions
{
    [JsonPropertyName("key")] public string Key { get; set; }
}