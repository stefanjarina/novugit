using Novugit.Base.Models;
using Novugit.Base.Enums;

namespace Novugit.Base.Contracts
{
    public interface IConfiguration
    {
        Config Config { get; set; }
        Provider GetProvider(string name);
        Provider GetProvider(Repos repo);
        string GetValue(string providerName, string key);
        void UpdateValue(string providerName, string key, string value);
        void RemoveValue(string providerName, string key);
    }
}
