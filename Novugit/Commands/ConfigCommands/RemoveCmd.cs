using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "remove", Description = "remove token for specified repository")]
public class RemoveCmd(IConfiguration config, ILogger<RemoveCmd> logger) : RepoArgBase
{
    [Argument(1)] [Required] public string Key { get; set; }

    protected int OnExecute(CommandLineApplication app)
    {
        try
        {
            config.RemoveValue(Repo, Key);
            Console.WriteLine($"Token successfuly removed");
            return 0;
        }
        catch (Exception)
        {
            logger.LogError($"Problem to remove token");
            return 1;
        }
    }
}