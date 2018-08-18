using System.Diagnostics;

namespace CandyJun.MicroserviceMnagement.Client
{
    public static class CmdHelper
    {
        public static string Batch(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\cmd.exe",
                    //Arguments = $" \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.StandardInput.WriteLine(escapedArgs);
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
