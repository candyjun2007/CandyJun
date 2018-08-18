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
    /// IIS控制命令
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AppCmdController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public AppCmdController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="arg">命令参数</param>
        /// <returns></returns>
        [HttpGet()]
        public string Excute(string arg)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} {arg} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 启动网站
        /// </summary>
        /// <param name="siteName">网站名称</param>
        /// <returns></returns>
        [HttpGet("Site/Start/{siteName}")]
        public string SiteStart(string siteName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} start site {siteName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 停止网站
        /// </summary>
        /// <param name="siteName">网站名称</param>
        /// <returns></returns>
        [HttpGet("Site/Stop/{siteName}")]
        public string SiteStop(string siteName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} stop site {siteName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 启动应用程序池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param>
        /// <returns></returns>
        [HttpGet("AppPool/Start/{appPoolName}")]
        public string StartAppPool(string appPoolName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} start apppool /apppool.name:{appPoolName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 停止应用程序池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param>
        /// <returns></returns>
        [HttpGet("AppPool/Stop/{appPoolName}")]
        public string StopAppPool(string appPoolName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} stop apppool /apppool.name:{appPoolName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 根据应用名称启动网站
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns></returns>
        [HttpGet("Site/Start/App/{appName}")]
        public string AppStartSite(string appName)
        {
            appName = appName.Trim();
            var app = _configuration.GetSection("App").Get<App[]>()?.FirstOrDefault(f => f.Name.ToLower() == appName.ToLower());
            if (app == null)
            {
                return $"{appName}未配置";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} start site {app.ServiceName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 根据应用名称停止网站
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns></returns>
        [HttpGet("Site/Stop/App/{appName}")]
        public string AppStopSite(string appName)
        {
            appName = appName.Trim();
            var app = _configuration.GetSection("App").Get<App[]>()?.FirstOrDefault(f => f.Name.ToLower() == appName.ToLower());
            if (app == null)
            {
                return $"{appName}未配置";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} stop site {app.ServiceName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 根据应用名称启动应用程序池
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns></returns>
        [HttpGet("AppPool/Start/App/{appName}")]
        public string AppStartAppPool(string appName)
        {
            appName = appName.Trim();
            var app = _configuration.GetSection("App").Get<App[]>()?.FirstOrDefault(f => f.Name.ToLower() == appName.ToLower());
            if (app == null)
            {
                return $"{appName}未配置";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} start apppool /apppool.name:{app.AppPoolName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 根据应用名称停止应用程序池
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns></returns>
        [HttpGet("AppPool/Stop/App/{appName}")]
        public string AppStopAppPool(string appName)
        {
            appName = appName.Trim();
            var app = _configuration.GetSection("App").Get<App[]>()?.FirstOrDefault(f => f.Name.ToLower() == appName.ToLower());
            if (app == null)
            {
                return $"{appName}未配置";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var cmd = $"{_configuration.GetValue<string>("AppCmdPath")} stop apppool /apppool.name:{app.AppPoolName} & exit";

                return cmd.Batch();
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }
    }
}