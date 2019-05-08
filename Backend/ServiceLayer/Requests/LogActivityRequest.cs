using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gucci.ServiceLayer.Requests
{
    public class LogActivityRequest
    {
        public string Jwt { get; set; }
        public string Ip { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
    }
}
