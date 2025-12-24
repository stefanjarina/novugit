using System.Text.Json.Serialization;

namespace Novugit.Base.Models.gitea;

public class Organization
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("username")] public string Username { get; set; }
    [JsonPropertyName("visibility")] public string Visibility { get; set; }
}