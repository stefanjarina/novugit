using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class ProjectCreateResponse
{
    
    [JsonPropertyName("ssh_url_to_repo")] public string SshUrlToRepo { get; set; }
    [JsonPropertyName("http_url_to_repo")] public string HttpUrlToRepo { get; set; }
}