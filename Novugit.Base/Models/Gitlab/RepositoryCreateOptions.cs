using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class RepositoryCreateOptions
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("namespace_id")] public string NamespaceId { get; set; }

    [JsonPropertyName("visibility")] public string Visibility { get; set; }
}