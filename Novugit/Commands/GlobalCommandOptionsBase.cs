using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;

namespace Novugit.Commands;

/// <summary>
/// Base class for all commands that provides global options (--verbose, --no-color).
/// Only the root command (NovugitCmd) should define the options with Inherited = true.
/// Subcommands inherit from this class to get the ApplyGlobalOptions method.
/// </summary>
public abstract class GlobalCommandOptionsBase
{
    /// <summary>
    /// Apply global options to ConsoleOutput. Call this at the start of OnExecute in each command.
    /// The options are inherited from the parent command via McMaster's Inherited = true.
    /// </summary>
    protected void ApplyGlobalOptions(CommandLineApplication app)
    {
        // Find the verbose and no-color options from the command hierarchy
        var verbose = app.GetOptions().FirstOrDefault(o => o.LongName == "verbose");
        var noColor = app.GetOptions().FirstOrDefault(o => o.LongName == "no-color");
        
        ConsoleOutput.Verbose = verbose?.HasValue() == true;
        ConsoleOutput.NoColor = noColor?.HasValue() == true;
    }
}

