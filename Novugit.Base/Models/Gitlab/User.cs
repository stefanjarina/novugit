using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class User
{
    [JsonPropertyName("name")] public string Name { get; set; }
}