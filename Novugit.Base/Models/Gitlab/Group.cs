using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class Group
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("share_with_group_lock")]
    public bool? ShareWithGroupLock { get; set; }

    [JsonPropertyName("require_two_factor_authentication")]
    public bool? RequireTwoFactorAuthentication { get; set; }

    [JsonPropertyName("two_factor_grace_period")]
    public int? TwoFactorGracePeriod { get; set; }

    [JsonPropertyName("project_creation_level")]
    public string ProjectCreationLevel { get; set; }

    [JsonPropertyName("auto_devops_enabled")]
    public object AutoDevopsEnabled { get; set; }

    [JsonPropertyName("subgroup_creation_level")]
    public string SubgroupCreationLevel { get; set; }

    [JsonPropertyName("emails_disabled")]
    public object EmailsDisabled { get; set; }

    [JsonPropertyName("mentions_disabled")]
    public object MentionsDisabled { get; set; }

    [JsonPropertyName("lfs_enabled")]
    public bool? LfsEnabled { get; set; }

    [JsonPropertyName("default_branch_protection")]
    public int? DefaultBranchProtection { get; set; }

    [JsonPropertyName("avatar_url")]
    public object AvatarUrl { get; set; }

    [JsonPropertyName("request_access_enabled")]
    public bool? RequestAccessEnabled { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("full_path")]
    public string FullPath { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("parent_id")]
    public int? ParentId { get; set; }

    [JsonPropertyName("ldap_cn")]
    public object LdapCn { get; set; }

    [JsonPropertyName("ldap_access")]
    public object LdapAccess { get; set; }
}