namespace Novugit.Base;

/// <summary>
/// Custom exception for Novugit that provides user-friendly error messages.
/// </summary>
public class NovugitException : Exception
{
    /// <summary>
    /// The provider that caused the exception (e.g., "github", "gitlab", "azure").
    /// Can be null for non-provider-specific errors.
    /// </summary>
    public string Provider { get; }

    public NovugitException(string message) : base(message)
    {
    }

    public NovugitException(string message, string provider) : base(message)
    {
        Provider = provider;
    }

    public NovugitException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NovugitException(string message, string provider, Exception innerException) : base(message, innerException)
    {
        Provider = provider;
    }
}

