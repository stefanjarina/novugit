﻿using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;

namespace Novugit.Commands;

[Command(
    Name = "novugit", Description = "", UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw)]
[Subcommand(typeof(InitCmd))]
[Subcommand(typeof(ConfigCmd))]
[Subcommand(typeof(GitignoreCmd))]
[HelpOption(Inherited = true)]
public class NovugitCmd
{
    [Option("-v|--verbose", Description = "Enable verbose output with full stack traces on errors", Inherited = true)]
    public bool Verbose { get; set; }

    [Option("--no-color", Description = "Disable colored output", Inherited = true)]
    public bool NoColor { get; set; }

    private int OnExecute(CommandLineApplication app)
    {
        ConsoleOutput.Verbose = Verbose;
        ConsoleOutput.NoColor = NoColor;
        
        app.ShowHelp();
        return 0;
    }
}