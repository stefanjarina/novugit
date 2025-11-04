using McMaster.Extensions.CommandLineUtils;
using Novugit.Base;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "all", Description = "list whole configuration")]
public class ListAllCmd
{
    private readonly IConfiguration _config;

    public ListAllCmd(IConfiguration config)
    {
        _config = config;
    }

    protected int OnExecute(CommandLineApplication app)
    {
        Console.WriteLine(Helpers.ConvertObjectToYaml(_config.Config));
        return 0;
    }
}