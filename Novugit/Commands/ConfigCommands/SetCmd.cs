using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "set", Description = "set token for specified repository")]
public class SetCmd : RepoArgBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<SetCmd> _logger;

    [Argument(1)] [Required] public string Key { get; set; }

    [Argument(2)] [Required] public string Value { get; set; }

    public SetCmd(IConfiguration config, ILogger<SetCmd> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected int OnExecute(CommandLineApplication app)
    {
        try
        {
            _config.UpdateValue(Repo, Key, Value);
            Console.WriteLine($"{Key} successfuly set");
            return 0;
        }
        catch (Exception)
        {
            _logger.LogError($"Problem to set config value");
            return 1;
        }
    }
}