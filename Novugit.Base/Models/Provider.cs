namespace Novugit.Base.Models
{
    public class Provider
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public string BaseUrl { get; set; }
        public IDictionary<string, string> Options { get; set; } = new Dictionary<string, string>();
    }
}
