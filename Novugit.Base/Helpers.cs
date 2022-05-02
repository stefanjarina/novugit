using System.Diagnostics;
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
            var psi = new ProcessStartInfo(cmdName, args) { RedirectStandardOutput = true, RedirectStandardError = true };
            var proc = Process.Start(psi);
            if (proc == null)
            {
                return false;
            }

            var stdOut = proc.StandardOutput.ReadToEnd();
            var stdErr = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            if (proc.ExitCode == 0)
                return true;

            throw new Exception(stdErr);
        }
    }
}
