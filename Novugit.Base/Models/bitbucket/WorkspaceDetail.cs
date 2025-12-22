using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class WorkspaceDetail
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; }
    [JsonPropertyName("is_privacy_enforced")] public bool IsPrivacyEnforced { get; set; }
}