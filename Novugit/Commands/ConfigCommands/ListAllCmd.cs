using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "all", Description = "list whole configuration")]
public class ListAllCmd(IConfiguration config)
{
    protected int OnExecute(CommandLineApplication app)
    {
        Console.WriteLine(Helpers.ConvertObjectToYaml(config.Config));
        return 0;
    }
}