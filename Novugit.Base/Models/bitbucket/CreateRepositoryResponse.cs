using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class CreateRepositoryResponse
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("links")] public RepositoryCreateLinks Links { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("full_name")] public string FullName { get; set; }
    [JsonPropertyName("is_private")] public bool IsPrivate { get; set; }
    [JsonPropertyName("scm")] public string Scm { get; set; }
    [JsonPropertyName("owner")] public Owner Owner { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("created_on")] public string CreatedOn { get; set; }
    [JsonPropertyName("updated_on")] public string UpdatedOn { get; set; }
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("language")] public string Language { get; set; }
    [JsonPropertyName("has_issues")] public bool HasIssues { get; set; }
    [JsonPropertyName("has_wiki")] public bool HasWiki { get; set; }
    [JsonPropertyName("fork_policy")] public string ForkPolicy { get; set; }
    [JsonPropertyName("project")] public RepositoryCreateProject Project { get; set; }
    [JsonPropertyName("mainbranch")] public Mainbranch Mainbranch { get; set; }
    [JsonPropertyName("override_settings")] public OverrideSettings OverrideSettings { get; set; }
    [JsonPropertyName("parent")] public object Parent { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; }
    [JsonPropertyName("website")] public object Website { get; set; }
    [JsonPropertyName("workspace")] public Workspace Workspace { get; set; }
    [JsonPropertyName("enforced_signed_commits")] public object EnforcedSignedCommits { get; set; }
}

public class Owner
{
    [JsonPropertyName("display_name")] public string DisplayName { get; set; }
    [JsonPropertyName("links")] public Links Links { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("username")] public string Username { get; set; }
}

public class Mainbranch
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class OverrideSettings
{
    [JsonPropertyName("default_merge_strategy")] public bool DefaultMergeStrategy { get; set; }
    [JsonPropertyName("branching_model")] public bool BranchingModel { get; set; }
}

public class RepositoryCreateTypeValue
{
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class RepositoryCreateProject
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("key")] public string Key { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("links")] public Links Links { get; set; }
}

public class RepositoryCreateWorkspace
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("uuid")] public string Uuid { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; }
    [JsonPropertyName("links")] public RepositoryCreateLinks Links { get; set; }
}

public class RepositoryCreateLinks
{
    [JsonPropertyName("self")] public Link Self { get; set; }
    [JsonPropertyName("html")] public Link Html { get; set; }
    [JsonPropertyName("avatar")] public Link Avatar { get; set; }
    [JsonPropertyName("pullrequests")] public Link Pullrequests { get; set; }
    [JsonPropertyName("commits")] public Link Commits { get; set; }
    [JsonPropertyName("forks")] public Link Forks { get; set; }
    [JsonPropertyName("watchers")] public Link Watchers { get; set; }
    [JsonPropertyName("branches")] public Link Branches { get; set; }
    [JsonPropertyName("tags")] public Link Tags { get; set; }
    [JsonPropertyName("downloads")] public Link Downloads { get; set; }
    [JsonPropertyName("source")] public Link Source { get; set; }
    [JsonPropertyName("clone")] public List<LinkWithName> Clone { get; set; }
    [JsonPropertyName("hooks")] public Link Hooks { get; set; }
}