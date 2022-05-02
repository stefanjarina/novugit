using McMaster.Extensions.CommandLineUtils;

namespace Novugit.Commands
{
    [Command(
        Name = "novugit", Description = "", UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw)]
    [Subcommand(typeof(InitCmd))]
    [Subcommand(typeof(ConfigCmd))]
    [HelpOption(Inherited = true)]
    public class NovugitCmd
    {
        private int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }
    }
}
