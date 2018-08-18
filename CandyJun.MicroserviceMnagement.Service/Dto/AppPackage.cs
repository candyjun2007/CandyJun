using System;
using System.Collections.Generic;
using System.Text;

namespace CandyJun.MicroserviceMnagement.Service.Dto
{
    public class AppPackage
    {
        public string AppCode { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string DownloadUrl { get; set; }
    }
}
