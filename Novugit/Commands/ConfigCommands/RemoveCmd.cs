using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "remove", Description = "remove token for specified repository")]
public class RemoveCmd : RepoArgBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<RemoveCmd> _logger;

    [Argument(1)] [Required] public string Key { get; set; }

    public RemoveCmd(IConfiguration config, ILogger<RemoveCmd> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected int OnExecute(CommandLineApplication app)
    {
        try
        {
            _config.RemoveValue(Repo, Key);
            Console.WriteLine($"Token successfuly removed");
            return 0;
        }
        catch (Exception)
        {
            _logger.LogError($"Problem to remove token");
            return 1;
        }
    }
}