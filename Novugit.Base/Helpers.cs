﻿using System.Diagnostics;
 using System.Text;
using CliWrap;
using Novugit.Base.Models;
using YamlDotNet.Serialization;

namespace Novugit.Base;

public static class Helpers
{
    public static string ConvertObjectToYaml(object o)
    {
        var serializer = new Serializer();
        var text = serializer.Serialize(o);
        return text;
    }

    public static CurrentDirectoryInfo GetCurrentDirInfo()
    {
        var di = new DirectoryInfo(Environment.CurrentDirectory);
        var files = di.GetFiles().Select(x => x.Name).ToList();
        var directories = di.GetDirectories().Select(x => x.Name).ToList();
        return new CurrentDirectoryInfo { Name = di.Name, Files = files, Directories = directories };
    }
    
    public static async Task<bool> ExecuteCommandInteractivelyAsync(string cmdName, string args)
    {
        var stdOutBuffer = new StringBuilder();
        var stdErrBuffer = new StringBuilder();

        var command = "yes\n" | Cli.Wrap(cmdName)
            .WithArguments(args)
            .WithValidation(CommandResultValidation.ZeroExitCode)
            .WithStandardInputPipe(
                PipeSource.FromStream(Console.OpenStandardInput()))
            .WithStandardOutputPipe(
                PipeTarget.Merge(
                    PipeTarget.ToStream(Console.OpenStandardOutput()), PipeTarget.ToStringBuilder(stdOutBuffer))
            )
            .WithStandardErrorPipe(
                PipeTarget.Merge(
                    PipeTarget.ToStream(Console.OpenStandardError()), PipeTarget.ToStringBuilder(stdErrBuffer))
            );
            
        var result = await command.ExecuteAsync();
        
        return result.ExitCode != 0 ? throw new NovugitException($"Command '{cmdName} {args}' failed with exit code {result.ExitCode}") : true;
    }
}