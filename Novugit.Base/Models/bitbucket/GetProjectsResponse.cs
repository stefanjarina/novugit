using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class GetProjectsResponse
{
    [JsonPropertyName("pagelen")] public int Pagelen { get; set; }
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("values")] public List<Value> Values { get; set; }
}

public class Value
{
    [JsonPropertyName("key")] public string Key { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}
