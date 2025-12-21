using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class WorkspaceDetail
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("links")] public Links Links { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; }
    [JsonPropertyName("is_private")] public bool IsPrivate { get; set; }
    [JsonPropertyName("is_privacy_enforced")] public bool IsPrivacyEnforced { get; set; }
    [JsonPropertyName("forking_mode")] public string ForkingMode { get; set; }
    [JsonPropertyName("created_on")] public string CreatedOn { get; set; }
    [JsonPropertyName("updated_on")] public string UpdatedOn { get; set; }
}