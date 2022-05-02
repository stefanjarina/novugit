using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class Namespace
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("full_path")]
    public string FullPath { get; set; }

    [JsonPropertyName("parent_id")]
    public object ParentId { get; set; }

    [JsonPropertyName("avatar_url")]
    public object AvatarUrl { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("members_count_with_descendants")]
    public int? MembersCountWithDescendants { get; set; }

    [JsonPropertyName("billable_members_count")]
    public int? BillableMembersCount { get; set; }

    [JsonPropertyName("max_seats_used")]
    public int? MaxSeatsUsed { get; set; }

    [JsonPropertyName("seats_in_use")]
    public int? SeatsInUse { get; set; }

    [JsonPropertyName("plan")]
    public string Plan { get; set; }

    [JsonPropertyName("trial_ends_on")]
    public object TrialEndsOn { get; set; }

    [JsonPropertyName("trial")]
    public bool? Trial { get; set; }
}