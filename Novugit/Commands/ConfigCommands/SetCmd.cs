using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "set", Description = "set token for specified repository")]
public class SetCmd(IConfiguration config, ILogger<SetCmd> logger) : RepoArgBase
{
    [Argument(1)] [Required] public string Key { get; set; }
    [Argument(2)] [Required] public string Value { get; set; }

    protected int OnExecute(CommandLineApplication app)
    {
        try
        {
            config.UpdateValue(Repo, Key, Value);
            Console.WriteLine($"{Key} successfuly set");
            return 0;
        }
        catch (Exception)
        {
            logger.LogError($"Problem to set config value");
            return 1;
        }
    }
}