using System.Text.Json.Serialization;

namespace Novugit.Base.Models.gitea;

public class User
{
    [JsonPropertyName("username")] public string Username { get; set; }
}