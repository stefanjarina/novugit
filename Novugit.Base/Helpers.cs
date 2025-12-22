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
    
    public static async Task<bool> ExecuteCommandInteractivelyAsync(string cmdName, string args, string answer = null)
    {
        var command = Cli.Wrap(cmdName)
            .WithArguments(args)
            .WithValidation(CommandResultValidation.ZeroExitCode); // Automatically throws if ExitCode != 0

        // If we have a pre-defined answer, pipe it to the process.
        // Otherwise, pipe the current Console Input so the user can type interactively.
        if (!string.IsNullOrEmpty(answer))
        {
            command = command.WithStandardInputPipe(PipeSource.FromString(answer));
        }
        else
        {
            command = command.WithStandardInputPipe(PipeSource.FromStream(Console.OpenStandardInput()));
        }
        
        var stdOutBuffer = new StringBuilder();
        var stdErrBuffer = new StringBuilder();

        // Pipe Output and Error directly to the Console so the user sees what's happening
        var result = await command
            .WithStandardOutputPipe(
                PipeTarget.Merge(
                    PipeTarget.ToStream(Console.OpenStandardOutput()), PipeTarget.ToStringBuilder(stdOutBuffer))
                )
            .WithStandardErrorPipe(
                PipeTarget.Merge(
                    PipeTarget.ToStream(Console.OpenStandardError()), PipeTarget.ToStringBuilder(stdErrBuffer))
                )
            .ExecuteAsync();
        
        return result.ExitCode == 0 ? true : throw new Exception();
    }
}