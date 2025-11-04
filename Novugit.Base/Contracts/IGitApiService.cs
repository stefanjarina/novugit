using Novugit.Base.Models;

namespace Novugit.Base.Contracts;

public interface IGitApiService
{
    Provider GetStoredProviderInfo();
}