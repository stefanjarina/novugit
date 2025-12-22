using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class LinkWithName
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("href")] public string Href { get; set; }
}