using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "all", Description = "list whole configuration")]
public class ListAllCmd(IConfiguration config) : GlobalCommandOptionsBase
{
    protected int OnExecute(CommandLineApplication app)
    {
        ApplyGlobalOptions(app);
        
        ConsoleOutput.WriteInfo(Helpers.ConvertObjectToYaml(config.Config));
        return 0;
    }
}