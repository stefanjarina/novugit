using System.Text.Json.Serialization;

namespace Novugit.Base.Models.Gitlab;

public class Namespace
{
    [JsonPropertyName("id")] public int Id { get; set; }
}