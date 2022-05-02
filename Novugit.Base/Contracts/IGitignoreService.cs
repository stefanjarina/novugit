namespace Novugit.Base.Contracts;

public interface IGitignoreService
{
    Task<IEnumerable<string>> List();
    Task<string> FetchConfig(IEnumerable<string> configs);
}
