﻿using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using Novugit.Base;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "set", Description = "set token for specified repository")]
public class SetCmd(IConfiguration config) : RepoArgBase
{
    [Argument(1)] [Required] public string Key { get; set; }
    [Argument(2)] [Required] public string Value { get; set; }

    protected int OnExecute(CommandLineApplication app)
    {
        ApplyGlobalOptions(app);
        
        try
        {
            config.UpdateValue(Repo, Key, Value);
            ConsoleOutput.WriteSuccess($"{Key} successfully set");
            return 0;
        }
        catch (Exception e)
        {
            ConsoleOutput.WriteError($"Failed to set {Key}: {e.Message}", e);
            return 1;
        }
    }
}