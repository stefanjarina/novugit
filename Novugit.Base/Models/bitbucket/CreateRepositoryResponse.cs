using System.Text.Json.Serialization;

namespace Novugit.Base.Models.bitbucket;

public class CreateRepositoryResponse
{
    [JsonPropertyName("links")] public RepositoryCreateLinks Links { get; set; }
}

public class RepositoryCreateLinks
{
    [JsonPropertyName("clone")] public List<LinkWithName> Clone { get; set; }
}