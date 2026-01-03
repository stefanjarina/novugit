using Spectre.Console;

namespace Novugit.Commands;

/// <summary>
/// Shared validation logic for provider arguments.
/// </summary>
public static class ProviderValidation
{
    /// <summary>
    /// Valid git platform providers supported by Novugit.
    /// </summary>
    public static readonly string[] ValidProviders =
    {
        "github",
        "gitlab",
        "azure",
        "bitbucket",
        "forgejo",
        "gitea"
    };

    /// <summary>
    /// Validates a provider string against the list of valid providers.
    /// </summary>
    /// <param name="provider">The provider string to validate</param>
    /// <returns>ValidationResult indicating success or error with helpful message</returns>
    public static ValidationResult ValidateProvider(string provider)
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            return ValidationResult.Error("Provider is required");
        }

        if (!ValidProviders.Contains(provider.ToLower()))
        {
            return ValidationResult.Error(
                $"Invalid provider '{provider}'. Valid options: {string.Join(", ", ValidProviders)}");
        }

        return ValidationResult.Success();
    }
}
