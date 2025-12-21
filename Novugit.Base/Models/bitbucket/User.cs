using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class User
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("nickname")] public string Nickname { get; set; }
    [JsonPropertyName("display_name")] public string DisplayName { get; set; }
}