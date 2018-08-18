using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyJun.ServerManagement.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Backup
    {
        /// <summary>
        /// 
        /// </summary>
        public string BackupPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Zip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DeleteAfterZip { get; set; }
    }
}
