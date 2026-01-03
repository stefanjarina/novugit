namespace Novugit.Base;

/// <summary>
/// Static helper class for colored console output with support for --no-color flag and NO_COLOR environment variable.
/// </summary>
public static class ConsoleOutput
{
    private static bool? _noColor;

    /// <summary>
    /// Gets or sets whether colors should be disabled.
    /// When not explicitly set, checks the NO_COLOR environment variable.
    /// </summary>
    public static bool NoColor
    {
        get => _noColor ?? Environment.GetEnvironmentVariable("NO_COLOR") != null;
        set => _noColor = value;
    }

    /// <summary>
    /// Gets or sets whether verbose output is enabled.
    /// </summary>
    public static bool Verbose { get; set; }

    /// <summary>
    /// Writes an error message in red (unless colors are disabled).
    /// </summary>
    public static void WriteError(string message)
    {
        WriteColored("VERBOSE IS: " + Verbose, ConsoleColor.Red, Console.Out);
        
        WriteColored(message, ConsoleColor.Red, Console.Error);
    }

    /// <summary>
    /// Writes an error message with optional stack trace for verbose mode.
    /// </summary>
    public static void WriteError(string message, Exception exception)
    {
        WriteInfo(message);
        
        WriteColored("VERBOSE IS: " + Verbose, ConsoleColor.Red, Console.Out);

        if (!Verbose || exception == null) return;
        
        Console.Error.WriteLine();
        Console.Error.WriteLine("Stack trace:");
        Console.Error.WriteLine(exception);
    }

    /// <summary>
    /// Writes a warning message in yellow (unless colors are disabled).
    /// </summary>
    public static void WriteWarning(string message)
    {
        WriteColored(message, ConsoleColor.Yellow, Console.Out);
    }

    /// <summary>
    /// Writes a success message in green (unless colors are disabled).
    /// </summary>
    public static void WriteSuccess(string message)
    {
        WriteColored(message, ConsoleColor.Green, Console.Out);
    }

    /// <summary>
    /// Writes an info message (no color change).
    /// </summary>
    public static void WriteInfo(string message)
    {
        Console.WriteLine(message);
    }

    private static void WriteColored(string message, ConsoleColor color, TextWriter writer)
    {
        if (NoColor)
        {
            writer.WriteLine(message);
            return;
        }

        var originalColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            writer.WriteLine(message);
        }
        finally
        {
            Console.ForegroundColor = originalColor;
        }
    }
}

