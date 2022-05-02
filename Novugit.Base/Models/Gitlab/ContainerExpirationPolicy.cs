using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class ContainerExpirationPolicy
{
    [JsonPropertyName("cadence")]
    public string Cadence { get; set; }

    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("keep_n")]
    public int? KeepN { get; set; }

    [JsonPropertyName("older_than")]
    public string OlderThan { get; set; }

    [JsonPropertyName("name_regex")]
    public string NameRegex { get; set; }

    [JsonPropertyName("name_regex_keep")]
    public object NameRegexKeep { get; set; }

    [JsonPropertyName("next_run_at")]
    public DateTime? NextRunAt { get; set; }
}