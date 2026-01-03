using Microsoft.AspNetCore.DataProtection;
using Novugit.Base.Contracts;

namespace Novugit.API.Services;

public class SecretService(IDataProtectionProvider dataProtectionProvider) : ISecretService
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("TokenProtection.v1");

    public string Encrypt(string value)
    {
        return _protector.Protect(value);
    }

    public string Decrypt(string encryptedValue)
    {
        return _protector.Unprotect(encryptedValue);
    }
}