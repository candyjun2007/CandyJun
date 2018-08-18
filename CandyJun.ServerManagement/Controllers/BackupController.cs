using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
    /// 备份服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public BackupController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 备份应用
        /// </summary>
        /// <param name="name">应用名称</param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public string Backup(string name)
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
                var subDir = DateTime.Now.ToString("Backup_yyyyMMddHHmmssfff");
                var backupDir = Path.Combine(app.Backup.BackupPath, subDir);
                var i = 1;
                var zipPath = $"{backupDir}.zip";
                while (Directory.Exists(backupDir) || System.IO.File.Exists(zipPath))
                {
                    backupDir = Path.Combine(app.Backup.BackupPath, $"{subDir}-{i++}");
                    zipPath = $"{backupDir}.zip";
                }
                Directory.CreateDirectory(backupDir);
                var files = Directory.GetFiles(app.AppPath);
                foreach(var file in files)
                {
                    System.IO.File.Copy(file, Path.Combine(backupDir, Path.GetFileName(file)));
                }
                if (app.Backup.Zip)
                {
                    ZipFile.CreateFromDirectory(backupDir, zipPath);
                    if (app.Backup.DeleteAfterZip)
                    {
                        Directory.Delete(backupDir, true);
                    }
                }

                return app.Backup.Zip ? zipPath : backupDir;
            }
            else
            {
                throw new NotSupportedException($"{RuntimeInformation.OSDescription} not yet implemented.");
            }
        }
    }
}