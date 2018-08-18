using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CandyJun.ServerManagement.AppCode;
using CandyJun.ServerManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CandyJun.ServerManagement.Controllers
{
    /// <summary>
    /// Windows服务管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public ServiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        [HttpGet("Start/{serviceName}")]
        public string Start(string serviceName)
        {
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
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        [HttpGet("Stop/{serviceName}")]
        public string Stop(string serviceName)
        {
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

        /// <summary>
        /// 启动应用
        /// </summary>
        /// <param name="name">应用名称</param>
        /// <returns></returns>
        [HttpGet("StartApp/{name}")]
        public string StartApp(string name)
        {
            name = name.Trim();
            var app = _configuration.GetSection("App").Get<App[]>()?.FirstOrDefault(f => f.Name.ToLower() == name.ToLower());
            if (app == null)
            {
                return $"{name}未配置";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"net start {app.ServiceName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 停止应用
        /// </summary>
        /// <param name="name">应用名称</param>
        /// <returns></returns>
        [HttpGet("StopApp/{name}")]
        public string StopApp(string name)
        {
            name = name.Trim();
            var app = _configuration.GetSection("App").Get<App[]>()?.FirstOrDefault(f => f.Name.ToLower() == name.ToLower());
            if (app == null)
            {
                return $"{name}未配置";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"net stop {app.ServiceName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }
    }
}