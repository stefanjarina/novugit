using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("bio")]
    public string Bio { get; set; }

    [JsonPropertyName("location")]
    public object Location { get; set; }

    [JsonPropertyName("public_email")]
    public string PublicEmail { get; set; }

    [JsonPropertyName("skype")]
    public string Skype { get; set; }

    [JsonPropertyName("linkedin")]
    public string Linkedin { get; set; }

    [JsonPropertyName("twitter")]
    public string Twitter { get; set; }

    [JsonPropertyName("website_url")]
    public string WebsiteUrl { get; set; }

    [JsonPropertyName("organization")]
    public string Organization { get; set; }

    [JsonPropertyName("job_title")]
    public string JobTitle { get; set; }

    [JsonPropertyName("pronouns")]
    public string Pronouns { get; set; }

    [JsonPropertyName("bot")]
    public bool? Bot { get; set; }

    [JsonPropertyName("work_information")]
    public object WorkInformation { get; set; }

    [JsonPropertyName("followers")]
    public int? Followers { get; set; }

    [JsonPropertyName("following")]
    public int? Following { get; set; }

    [JsonPropertyName("local_time")]
    public string LocalTime { get; set; }

    [JsonPropertyName("last_sign_in_at")]
    public DateTime LastSignInAt { get; set; }

    [JsonPropertyName("confirmed_at")]
    public DateTime ConfirmedAt { get; set; }

    [JsonPropertyName("theme_id")]
    public int? ThemeId { get; set; }

    [JsonPropertyName("last_activity_on")]
    public string LastActivityOn { get; set; }

    [JsonPropertyName("color_scheme_id")]
    public int? ColorSchemeId { get; set; }

    [JsonPropertyName("projects_limit")]
    public int? ProjectsLimit { get; set; }

    [JsonPropertyName("current_sign_in_at")]
    public DateTime CurrentSignInAt { get; set; }

    [JsonPropertyName("identities")]
    public List<Identity> Identities { get; set; }

    [JsonPropertyName("can_create_group")]
    public bool? CanCreateGroup { get; set; }

    [JsonPropertyName("can_create_project")]
    public bool? CanCreateProject { get; set; }

    [JsonPropertyName("two_factor_enabled")]
    public bool? TwoFactorEnabled { get; set; }

    [JsonPropertyName("external")]
    public bool? External { get; set; }

    [JsonPropertyName("private_profile")]
    public bool? PrivateProfile { get; set; }

    [JsonPropertyName("commit_email")]
    public string CommitEmail { get; set; }
}