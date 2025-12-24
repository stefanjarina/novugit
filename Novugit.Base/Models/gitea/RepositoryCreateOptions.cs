using System.Text.Json.Serialization;

namespace Novugit.Base.Models.gitea;

public class RepositoryCreateOptions
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("auto_init")] public bool AutoInit { get; set; }
    [JsonPropertyName("default_branch")] public string DefaultBranch { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("private")] public bool Private { get; set; }
    [JsonPropertyName("trust_model")] public string TrustModel { get; set; }
}