using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gucci.ServiceLayer.Requests
{
    public class LogActivityRequest
    {
        public string Jwt { get; }
        public string Ip { get; }
        public string StartPoint { get; }
        public string EndPoint { get; }
    }
}
