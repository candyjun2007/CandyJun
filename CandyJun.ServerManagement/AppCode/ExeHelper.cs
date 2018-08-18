using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CandyJun.ServerManagement.AppCode
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exe"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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
