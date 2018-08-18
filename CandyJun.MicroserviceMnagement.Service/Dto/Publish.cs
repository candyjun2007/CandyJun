using System;
using System.Collections.Generic;
using System.Text;

namespace CandyJun.MicroserviceMnagement.Service.Dto
{
    public class Publish
    {
        public int SerialNo { get; set; }
        public string Version { get; set; }
        public DateTime StopTime { get; set; }
        public DateTime PlanTime { get; set; }
        public DateTime? PublishTime { get; set; }
        public int Status { get; set; }
        public string ErrMsg { get; set; }
    }
}
