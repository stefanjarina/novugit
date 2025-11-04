namespace Novugit.Base.Models;

public class CurrentDirectoryInfo
{
    public string Name { get; set; }
    public List<string> Files { get; set; }
    public List<string> Directories { get; set; }
}