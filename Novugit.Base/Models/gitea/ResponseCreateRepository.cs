using System.Text.Json.Serialization;

namespace Novugit.Base.Models.gitea;

public class ResponseCreateRepository
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("full_name")] public string FullName { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("private")] public bool Private { get; set; }
    [JsonPropertyName("ssh_url")] public string SshUrl { get; set; }
    [JsonPropertyName("clone_url")] public string CloneUrl { get; set; }
}