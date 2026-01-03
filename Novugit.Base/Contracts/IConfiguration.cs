using Novugit.Base.Models;
using Novugit.Base.Enums;

namespace Novugit.Base.Contracts;

public interface IConfiguration
{
    Config Config { get; set; }
    Provider GetProvider(string name);
    Provider GetProvider(Repos repo);
    string DecryptToken(string encryptedToken);
    string GetValue(string providerName, string key, bool decrypt = false);
    void UpdateValue(string providerName, string key, string value, bool encrypt = false);
    void RemoveValue(string providerName, string key);
}