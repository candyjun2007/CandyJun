using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CandyJun.ServerManagement.AppCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CandyJun.ServerManagement.Controllers
{
    /// <summary>
    /// 命令行执行服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        /// <summary>
        /// 运行命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>命令输出</returns>
        [HttpGet("Excute")]
        public string Excute(string cmd)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return cmd.Bash();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!cmd.Trim().EndsWith("exit"))
                {
                    cmd = $"{cmd} & exit";
                }
                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }
    }
}