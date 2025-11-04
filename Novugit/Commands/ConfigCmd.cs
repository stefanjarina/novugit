using McMaster.Extensions.CommandLineUtils;
using Novugit.Commands.ConfigCommands;

namespace Novugit.Commands;

[Command(Name = "config", Description = "Manage configuration")]
[Subcommand(typeof(GetCmd))]
[Subcommand(typeof(ListAllCmd))]
[Subcommand(typeof(SetCmd))]
[Subcommand(typeof(RemoveCmd))]
public class ConfigCmd
{
    protected int OnExecute(CommandLineApplication app)
    {
        app.ShowHelp();
        return 0;
    }
}