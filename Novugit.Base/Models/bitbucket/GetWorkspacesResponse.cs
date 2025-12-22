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
    [JsonPropertyName("workspace")] public Workspace Workspace { get; set; }
}

public class Workspace
{
    [JsonPropertyName("slug")] public string Slug { get; set; }
}