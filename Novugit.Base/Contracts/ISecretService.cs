namespace Novugit.Base.Contracts;

public interface ISecretService
{
    string Encrypt(string value);
    string Decrypt(string value);
}