namespace Novugit.Base.Models;

public class ProjectInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Visibility { get; set; }
    public IEnumerable<string> GitIgnoreConfigs { get; set; }
    public IEnumerable<string> ExcludedLocalFiles { get; set; }
    public string RemoteUrl { get; set; }
}