using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class Identity
{
    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    [JsonPropertyName("extern_uid")]
    public string ExternUid { get; set; }
}