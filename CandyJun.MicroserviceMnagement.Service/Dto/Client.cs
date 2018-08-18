using System;
using System.Collections.Generic;
using System.Text;

namespace CandyJun.MicroserviceMnagement.Service.Dto
{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ip { get; set; }
        public string Os { get; set; }
    }
}
