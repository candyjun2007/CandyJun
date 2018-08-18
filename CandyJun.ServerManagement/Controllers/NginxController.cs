using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CandyJun.ServerManagement.AppCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CandyJun.ServerManagement.Controllers
{
    /// <summary>
    /// Nginx管理服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NginxController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public NginxController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 执行Nginx相关命令 格式：nginx arg
        /// </summary>
        /// <param name="arg">命令参数</param>
        /// <returns></returns>
        [HttpGet("Excute")]
        public string Excute(string arg)
        {
            var path = _configuration.GetValue<string>("NginxPath");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var cmd = $"cd {path} && nginx {arg}";
                return cmd.Bash();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"cd {path} & {(string.IsNullOrWhiteSpace(arg) ? "start " : "")}nginx {arg} & exit";
                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 启动Nginx服务
        /// </summary>
        /// <returns></returns>
        [HttpGet("StartNginx")]
        public string StartNginx()
        {
            var serviceName = _configuration.GetValue<string>("NginxServiceName");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"net start {serviceName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 停止Nginx服务
        /// </summary>
        /// <returns></returns>
        [HttpGet("StopNginx")]
        public string StopNginx()
        {
            var serviceName = _configuration.GetValue<string>("NginxServiceName");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"net stop {serviceName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }
    }
}