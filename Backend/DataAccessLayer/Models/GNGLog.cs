using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccessLayer.Models
{
    public class GNGLog
    {
        public string LogID { get; set; }
        public string UserID { get; set; }
        public string IpAddress { get; set; }
        public string DateTime { get; set; }
        public string Description { get; set; }
    }
}