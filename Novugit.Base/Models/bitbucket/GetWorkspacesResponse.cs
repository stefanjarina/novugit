using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class GetWorkspacesResponse
{
    [JsonPropertyName("pagelen")] public int Pagelen { get; set; }
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("values")] public List<WorkspaceValue> Values { get; set; }

}

public class WorkspaceValue
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("permission")] public string Permission { get; set; }
    [JsonPropertyName("last_accessed")] public DateTime LastAccessed { get; set; }
    [JsonPropertyName("added_on")] public DateTime AddedOn { get; set; }
    [JsonPropertyName("user")] public User User { get; set; }
    [JsonPropertyName("workspace")] public Workspace Workspace { get; set; }
}

public class Workspace
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}