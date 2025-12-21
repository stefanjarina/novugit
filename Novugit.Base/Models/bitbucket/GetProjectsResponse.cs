using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class GetProjectsResponse
{
    [JsonPropertyName("pagelen")] public int Pagelen { get; set; }
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("values")] public List<Value> Values { get; set; }
}

public class Links
{
    [JsonPropertyName("self")] public Link Self { get; set; }
    [JsonPropertyName("avatar")] public Link Avatar { get; set; }
    [JsonPropertyName("html")] public Link Html { get; set; }
    [JsonPropertyName("repositories")] public Link Repositories { get; set; }
}

public class Value
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("owner")] public User Owner { get; set; }
    [JsonPropertyName("workspace")] public Workspace Workspace { get; set; }
    [JsonPropertyName("key")] public string Key { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("is_private")] public bool IsPrivate { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("links")] public Links Links { get; set; }
    [JsonPropertyName("created_on")] public DateTime CreatedOn { get; set; }
    [JsonPropertyName("updated_on")] public DateTime UpdatedOn { get; set; }
    [JsonPropertyName("has_publicly_visible_repos")] public bool HasPubliclyVisibleRepos { get; set; }
}
