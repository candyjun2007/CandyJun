using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyJun.ServerManagement.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class App
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppPoolName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Backup Backup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Publish Publish { get; set; }
    }
}
