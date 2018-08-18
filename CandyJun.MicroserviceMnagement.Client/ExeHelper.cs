using System.Diagnostics;

namespace CandyJun.MicroserviceMnagement.Client
{
    public static class ExeHelper
    {
        public static string Exec(this string exe, params string[] args)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exe,
                    Arguments = args == null ? null : string.Join(" ", args),
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
