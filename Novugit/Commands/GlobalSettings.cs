using System.ComponentModel;
using Novugit.Base;
using Spectre.Console.Cli;

namespace Novugit.Commands;

/// <summary>
/// Base settings class that provides global options (--verbose, --no-color).
/// All command settings should inherit from this class.
/// </summary>
public class GlobalSettings : CommandSettings
{
    [CommandOption("-v|--verbose")]
    [Description("Enable verbose output with full stack traces on errors")]
    [DefaultValue(false)]
    public bool Verbose { get; init; }

    [CommandOption("--no-color")]
    [Description("Disable colored output")]
    [DefaultValue(false)]
    public bool NoColor { get; init; }

    /// <summary>
    /// Apply global settings to ConsoleOutput.
    /// Call this at the start of Execute/ExecuteAsync in each command.
    /// </summary>
    public void ApplyGlobalOptions()
    {
        ConsoleOutput.Verbose = Verbose;
        ConsoleOutput.NoColor = NoColor;
    }
}
