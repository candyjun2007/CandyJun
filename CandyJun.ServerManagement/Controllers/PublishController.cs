using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CandyJun.ServerManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CandyJun.ServerManagement.Controllers
{
    /// <summary>
    /// 发布服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public PublishController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 发布应用
        /// </summary>
        /// <param name="name">应用名称</param>
        /// <param name="filePath">发布包路径</param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public string Publish(string name, string filePath)
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
                var subDir = Path.GetFileNameWithoutExtension(filePath);
                var publishDir = Path.Combine(app.Publish.PublishPath, subDir);
                var i = 1;
                var zipPath = Path.Combine(app.Publish.PublishPath, filePath);
                while (Directory.Exists(publishDir) )
                {
                    publishDir = Path.Combine(app.Publish.PublishPath, $"{subDir}-{i++}");
                }
                Directory.CreateDirectory(publishDir);
                ZipFile.ExtractToDirectory(zipPath, publishDir);
                var files = Directory.GetFiles(publishDir);
                foreach (var file in files)
                {
                    System.IO.File.Copy(file, Path.Combine(app.AppPath, Path.GetFileName(file)), true);
                }
                if (app.Publish.DeleteAfterPublish)
                {
                    Directory.Delete(publishDir, true);
                }

                return app.Publish.DeleteAfterPublish ? zipPath : publishDir;
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }

        /// <summary>
        /// 发布应用
        /// </summary>
        /// <param name="name">应用名称</param>
        /// <param name="fileUrl">发布包下载地址</param>
        /// <returns></returns>
        [HttpGet("Download/{name}")]
        public string PublishFromUrl(string name, string fileUrl)
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
                var subDir = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var i = 1;
                var zipPath = Path.Combine(app.Publish.PublishPath, $"{subDir}.zip");
                var publishDir = Path.Combine(app.Publish.PublishPath, subDir);
                while (System.IO.File.Exists(zipPath) || Directory.Exists(publishDir))
                {
                    var temp = $"{subDir}-{i++}";
                    zipPath = Path.Combine(app.Publish.PublishPath, $"{temp}.zip");
                    publishDir = Path.Combine(app.Publish.PublishPath, $"{temp}");
                }
                new System.Net.WebClient().DownloadFile(fileUrl, zipPath);
 
                Directory.CreateDirectory(publishDir);
                ZipFile.ExtractToDirectory(zipPath, publishDir);
                var files = Directory.GetFiles(publishDir);
                foreach (var file in files)
                {
                    System.IO.File.Copy(file, Path.Combine(app.AppPath, Path.GetFileName(file)), true);
                }
                if (app.Publish.DeleteAfterPublish)
                {
                    Directory.Delete(publishDir, true);
                }

                return app.Publish.DeleteAfterPublish ? zipPath : publishDir;
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }
    }
}