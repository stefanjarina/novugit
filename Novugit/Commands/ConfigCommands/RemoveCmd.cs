﻿using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using Novugit.Base;
using Novugit.Base.Contracts;

namespace Novugit.Commands.ConfigCommands;

[Command(Name = "remove", Description = "remove token for specified repository")]
public class RemoveCmd(IConfiguration config) : RepoArgBase
{
    [Argument(1)] [Required] public string Key { get; set; }

    protected int OnExecute(CommandLineApplication app)
    {
        ApplyGlobalOptions(app);
        
        try
        {
            config.RemoveValue(Repo, Key);
            ConsoleOutput.WriteSuccess("Token successfully removed");
            return 0;
        }
        catch (Exception e)
        {
            ConsoleOutput.WriteError($"Failed to remove {Key}: {e.Message}", e);
            return 1;
        }
    }
}