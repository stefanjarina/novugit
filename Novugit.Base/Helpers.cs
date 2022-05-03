﻿using System.Diagnostics;
using YamlDotNet.Serialization;

namespace Novugit.Base
{
    public static class Helpers
    {
        public static string ConvertObjectToYaml(object o)
        {
            var serializer = new Serializer();
            var text = serializer.Serialize(o);
            return text;
        }

        public static bool ExecuteCommandAndGetStatus(string cmdName, string args)
        {
            var process = CreateControlledProcess(cmdName, args);
            StartupControlledProcess(process);
            if (process == null)
            {
                return false;
            }

            var stdOut = process.StandardOutput.ReadToEnd();
            var stdErr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0)
                return true;

            throw new Exception(stdErr);
        }

        public static bool ExecuteCommandInteractively(string cmdName, string args, string inputDetectionString, string answer = null)
        {
            var process = CreateControlledProcess(cmdName, args);
            StartupControlledProcess(process);
            if (process == null)
            {
                return false;
            }

            var output = process.StandardOutput.ReadToEnd();

            if (output.Contains(inputDetectionString))
            {
                if (answer != null)
                {
                    process.StandardInput.WriteLine(answer);
                }
                else
                {
                    var userInput = Console.ReadLine();
                    process.StandardInput.WriteLine(userInput.Trim());
                }
            }

            var stdErr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0)
                return true;

            throw new Exception(stdErr);
        }

        private static Process CreateControlledProcess(string cmdName, string args)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.Arguments = args;
            process.StartInfo.FileName = cmdName;
            process.StartInfo.CreateNoWindow = true;
            return process;
        }

        private static void StartupControlledProcess(Process process)
        {
            if (process.Start())
            {
                if (OperatingSystem.IsWindows())
                {
                    process.StandardInput.NewLine = "\r\n";
                }
                else
                {
                    process.StandardInput.NewLine = "\n";
                }
            }
        }
    }
}
